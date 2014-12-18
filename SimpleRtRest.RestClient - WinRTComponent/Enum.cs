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
        internal const string FormUrlEncoded = "application/x-www-form-urlencoded";
        internal const string Json = "application/json";
        internal const string Xml = "application/xml";
    }

    public static class DataFormat
    {
        internal const string Json = "application/json";
        internal const string Xml = "application/xml";
    }
}
