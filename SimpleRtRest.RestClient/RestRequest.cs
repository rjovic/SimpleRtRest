using System.Collections.Generic;
using System.Net.Http;
namespace SimpleRtRest.RestClient
{
    public class RestRequest : IRestRequest
    {
        private HttpMethod _method = HttpMethod.Get;

        public HttpMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public string Resource { get; set; }
        public List<Parameter> Parameters { get; set; }

        public RestRequest()
        {
            Parameters = new List<Parameter>();
        }

        public RestRequest(HttpMethod method) : this()
        {
            Method = method;
        }

        public RestRequest(string resource) : this(resource, HttpMethod.Get)
        {
            Resource = resource;
        }

        public RestRequest(string resource, HttpMethod method) : this()
        {
            Resource = resource;
            Method = method;
        }
        
        public IRestRequest AddParameter(Parameter parameter)
        {
            Parameters.Add(parameter);
            return this;
        }

        public IRestRequest AddParameter(string name, object value)
        {
            Parameters.Add(new Parameter() { Name = name, Value = value, Type = ParameterType.Content});
            return this;
        }

        public IRestRequest AddParameter(string name, object value, ParameterType type)
        {
            Parameters.Add(new Parameter() { Name = name, Value = value, Type = type });
            return this;
        }

        public IRestRequest AddUrlSegment(string name, object value)
        {
            if(Resource.Contains("{" + name + "}"))
            {
                Resource = Resource.Replace("{" + name + "}", value.ToString());
            }

            return this;
        }

        public string DateTimeFormat { get; set; }
    }
}
