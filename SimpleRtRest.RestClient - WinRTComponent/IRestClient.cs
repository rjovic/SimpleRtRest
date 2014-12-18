using System.Collections.Generic;
using System.Threading.Tasks;

using SimpleRtRest.RestClient.Authenticators;

namespace SimpleRtRest.RestClient
{
    internal interface IRestClient
    {
        string BaseUrl { get; set; }
        string UserAgent { get; set; }

        List<Parameter> Parameters { get; set; }
        List<Parameter> DefaultUrlParameters { get; set; }
        IAuthenticator Authenticator { get; set; }

        Task<IRestResponse> Execute(IRestRequest request);
        Task<IRestResponseGeneric<T>> Execute<T>(IRestRequest request) where T : new();
    }
}
