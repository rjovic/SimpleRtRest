using System;
using System.Net;

namespace SimpleRtRest.RestClient
{
    internal interface IRestResponse
    {
        IRestRequest Request { get; set; }
        
        string RawData { get; set; }
        string ErrorMessage { get; set; }
        string ResponseFormat { get; set; }
        HttpStatusCode StatusCode { get; set; }

        Exception ErrorException { get; set; }
    }

    internal interface IRestResponseGeneric<T> : IRestResponse
    {
        T Data { get; set; }
    }
}
