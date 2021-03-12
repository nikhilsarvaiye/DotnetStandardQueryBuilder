namespace DotnetStandardQueryBuilder.OData
{
    public class UriParserSettings
    {
        public bool EnableCaseInsensitive { get; set; } = true;

        public static UriParserSettings Default { get; } = new UriParserSettings { EnableCaseInsensitive = true };
    }
}
