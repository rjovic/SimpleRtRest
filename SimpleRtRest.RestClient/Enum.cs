namespace SimpleRtRest.RestClient
{
    public enum ParameterType
    {
        Header,
        UrlSegment,
        UrlParam,
        GetOrPost,
        RequestBody
    }

    public static class MediaTypes
    {
        public const string FormUrlEncoded = "application/x-www-form-urlencoded";
        public const string Json = "application/json";
        public const string Xml = "application/xml";
    }

    public static class DataFormat
    {
        public const string Json = "application/json";
        public const string Xml = "application/xml";
    }
}
