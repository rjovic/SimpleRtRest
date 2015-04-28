using System;
using System.Net;

namespace SimpleRtRest.RestClient
{
    internal class RestResponseBase
    {
        public IRestRequest Request { get; set; }

        public string RawData { get; set; }
        public string ErrorMessage { get; set; }
        public string ResponseFormat { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public Exception ErrorException { get; set; }
    }

    internal class RestResponseGeneric<T> : RestResponseBase, IRestResponseGeneric<T>
    {
        public T Data { get; set; }
    }

    internal class RestResponse : RestResponseBase, IRestResponse
    {
        
    }
}
