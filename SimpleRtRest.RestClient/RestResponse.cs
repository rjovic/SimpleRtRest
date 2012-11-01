using System.Net;
namespace SimpleRtRest.RestClient
{
    public class RestResponseBase
    {
        public string RawData { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class RestResponse<T> : RestResponseBase, IRestResponse<T>
    {
        public T Data { get; set; }
    }

    public class RestResponse : RestResponseBase, IRestResponse
    {
        
    }
}
