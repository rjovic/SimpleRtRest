using System.Collections.Generic;
using System.Net.Http;

namespace SimpleRtRest.RestClient
{
    internal interface IRestRequest
    {
        string Resource { get; set; }
        string RequestFormat { get; set; }

        HttpMethod Method { get; set; }
        List<Parameter> Parameters { get; set; }

        string DateTimeFormat { get; set; }

        IRestRequest AddParameter(Parameter p);
        IRestRequest AddParameter(string name, object value);
        IRestRequest AddParameter(string name, object value, ParameterType type);

        IRestRequest AddBody(object o);

        IRestRequest AddUrlSegment(string name, object value);
    }
}
