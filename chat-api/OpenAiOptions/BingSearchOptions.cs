namespace OpenApi.Shared
{
    public sealed class BingSearchOptions
    {
        public const string Name = "BingSearchOptions";

        public string Endpoint { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }
}
