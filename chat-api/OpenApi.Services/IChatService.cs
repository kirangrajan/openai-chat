using Microsoft.SemanticKernel.ChatCompletion;

namespace OpenApi.Services
{
    public interface IChatService
    {
        Task<ChatHistory> SendAndReceive(string message);

        Task<ChatHistory> SendAndReceiveWithPluginSupport(string message);

        Task<ChatHistory> SendAndReceiveWithSearchSupport(string message);
    }
}
