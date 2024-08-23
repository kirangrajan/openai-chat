namespace OpenApi.Shared
{
    public sealed class OpenAiOptions
    {
        public const string Name = "OpenAiOptions";

        public string Model { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }
}
