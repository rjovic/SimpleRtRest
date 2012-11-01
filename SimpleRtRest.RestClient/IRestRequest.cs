using System.Collections.Generic;
using System.Net.Http;

namespace SimpleRtRest.RestClient
{
    public interface IRestRequest
    {
        string Resource { get; set; }
        HttpMethod Method { get; set; }
        List<Parameter> Parameters { get; set; }

        IRestRequest AddParameter(Parameter p);
        IRestRequest AddParameter(string name, object value);
        IRestRequest AddParameter(string name, object value, ParameterType type);

        IRestRequest AddUrlSegment(string name, object value);
    }
}
