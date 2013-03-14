using System;
using System.Net;

namespace SimpleRtRest.RestClient
{
    public class RestResponseBase
    {
        public IRestRequest Request { get; set; }

        public string RawData { get; set; }
        public string ErrorMessage { get; set; }
        public string ResponseFormat { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public Exception ErrorException { get; set; }
    }

    public class RestResponse<T> : RestResponseBase, IRestResponse<T>
    {
        public T Data { get; set; }
    }

    public class RestResponse : RestResponseBase, IRestResponse
    {
        
    }
}
