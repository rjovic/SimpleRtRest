using System.Net;

namespace SimpleRtRest.RestClient
{
    public interface IRestResponse
    {
        string RawData { get; set; }
        HttpStatusCode StatusCode { get; set; }
    }

    public interface IRestResponse<T> : IRestResponse
    {
        T Data { get; set; }
    }
}
