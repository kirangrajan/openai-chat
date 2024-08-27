using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenApi.Shared;
using System.Text;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel.Plugins.Web;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Azure;


namespace OpenApi.Services
{
    public class ChatService : IChatService
    {
        private readonly OpenAiOptions _openAiOptions;
        private readonly BingSearchOptions _bingSearchOptions;
        private BingSearchPlugin _bingSearchPlugin;

        static ChatHistory _chatMessages;
        public ChatService(IOptions<OpenAiOptions> options, IOptions<BingSearchOptions> searchOptions)
        {
            InitialiseChatHistory();
            _openAiOptions = options.Value;
            _bingSearchOptions = searchOptions.Value;
            _bingSearchPlugin = new BingSearchPlugin(_bingSearchOptions.Key);
        }

        public void InitialiseChatHistory()
        {
            if (_chatMessages == null)
            {
                _chatMessages = new ChatHistory("""
                                           You are a friendly assistant who helps users with their tasks.
                                           You will complete required steps and request approval before taking any consequential actions.
                                           If the user doesn't provide enough information for you to complete a task, you will keep asking questions until you have enough information to complete the task.
                                           """);
            }
        }

        public async Task<ChatHistory> SendAndReceive(string message)
        {
            var kernel = InitialiseKernal();

            var response = await GetChatResponse(message, kernel, null);

            _chatMessages.AddAssistantMessage(response);
            return _chatMessages;
        }

        public async Task<ChatHistory> SendAndReceiveWithPluginSupport(string message)
        {
            var kernel = InitialiseKernal();

#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            kernel.Plugins.AddFromType<TimePlugin>();
#pragma warning restore SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            var response =  await GetChatResponse(message, kernel, ToolCallBehavior.AutoInvokeKernelFunctions);

            _chatMessages.AddAssistantMessage(response);
            return _chatMessages;
        }

        public async Task<ChatHistory> SendAndReceiveWithSearchSupport(string message)
        {
            var kernel = InitialiseKernal();

#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var bingConnector = new BingConnector(_bingSearchOptions.Key);
            kernel.ImportPluginFromObject(new WebSearchEnginePlugin(bingConnector), "bing");
#pragma warning restore SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.


          
            var chatResponse = await GetChatResponse(message, kernel, ToolCallBehavior.AutoInvokeKernelFunctions);
            if (IsResponseUnsatisfactory(message, chatResponse))
            {
                // Step 3: Perform a Bing search as fallback
                var bingSearchResults = await _bingSearchPlugin.SearchAsync(message);

                // Optionally, re-query the chat completion with enhanced prompt
                _chatMessages.AddMessage(AuthorRole.Tool, $"Additional context from Bing: {bingSearchResults}");
                var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

                OpenAIPromptExecutionSettings settings = new()
                {
                    MaxTokens = 3000,
                    Temperature = 0.7f,
                    FrequencyPenalty = 0,
                    PresencePenalty = 0
                };

                var assistantMessageStringBuilder = await GetChatResponse(kernel, chatCompletionService, settings);

                chatResponse = assistantMessageStringBuilder.ToString();
            }

            _chatMessages.AddAssistantMessage(chatResponse);
            return _chatMessages;
        }

        private static async Task<string> GetChatResponse(string message, Kernel kernel, ToolCallBehavior? toolCallBehavior)
        {
            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            OpenAIPromptExecutionSettings settings = new()
            {
                MaxTokens = 3000,
                Temperature = 0.7f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            if (toolCallBehavior != null)
            {
                settings.ToolCallBehavior = toolCallBehavior;
            }

            _chatMessages.AddUserMessage(message);

            var assistantMessageStringBuilder = await GetChatResponse(kernel, chatCompletionService, settings);

            return assistantMessageStringBuilder.ToString();
        }

        private static async Task<StringBuilder> GetChatResponse(Kernel kernel, IChatCompletionService chatCompletionService, OpenAIPromptExecutionSettings settings)
        {
            // Get the chat completions
            var response = chatCompletionService.GetStreamingChatMessageContentsAsync(
                chatHistory: _chatMessages,
                executionSettings: settings,
                kernel: kernel);

            var assistantMessageStringBuilder = new StringBuilder();

            await foreach (var content in response)
            {
                if (!string.IsNullOrEmpty(content.Content))
                {
                    assistantMessageStringBuilder.Append(content.Content);
                    Console.Write(content.Content);
                }
            }

            return assistantMessageStringBuilder;
        }

        private Kernel InitialiseKernal()
        {
            // Create a kernel builder
            var builder = Kernel.CreateBuilder();

            // Add the Azure OpenAI chat completions service
            builder.AddAzureOpenAIChatCompletion(_openAiOptions.Model, _openAiOptions.Endpoint, _openAiOptions.Key);
            var kernel = builder.Build();
            return kernel;
        }

        private bool IsResponseUnsatisfactory(string prompt, string response)
        {
            return ContainsUnsatisfactoryKeywords(response) ||
                   IsTooShort(response) ||
                   IsRepetitive(prompt, response);
        }

        private bool ContainsUnsatisfactoryKeywords(string response)
        {
            string[] keywords = { "I don't know", "I'm not sure", "It depends", "Please clarify", "can't access", "currently unable to" };
            foreach (var keyword in keywords)
            {
                if (response.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsTooShort(string response, int minLength = 50)
        {
            return response.Length < minLength;
        }

        private bool IsRepetitive(string prompt, string response)
        {
            return prompt.Equals(response, StringComparison.OrdinalIgnoreCase);
        }
    }
}





