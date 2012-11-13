namespace SimpleRtRest.RestClient
{
    public enum ParameterType
    {
        Header,
        UrlSegment,
        UrlParam,
        Content,
    }

    public static class MediaTypes
    {
        public const string FormUrlEncoded = "application/x-www-form-urlencoded";
        public const string Json = "application/json";
        public const string Xml = "application/xml";
    }
}
