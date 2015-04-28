using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Xml.Serialization;

namespace SimpleRtRest.RestClient
{
    internal class RestRequest : IRestRequest
    {
        private HttpMethod _method = HttpMethod.Get;
        
        public HttpMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public string Resource { get; set; }
        public string RequestFormat { get; set; }
        public string DateTimeFormat { get; set; }

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
            Parameters.Add(new Parameter() { Name = name, Value = value, Type = ParameterType.GetOrPost});
            return this;
        }

        public IRestRequest AddParameter(string name, object value, ParameterType type)
        {
            Parameters.Add(new Parameter() { Name = name, Value = value, Type = type });
            return this;
        }

        public IRestRequest AddBody(object o)
        {
            string serialized;
            string contentType;
            
            switch (RequestFormat)
            {
                case DataFormat.Xml:
                    serialized = SerializeAsXml(o);
                    contentType = "application/xml";
                    break;
                case DataFormat.Json:
                    serialized = JsonConvert.SerializeObject(o);
                    contentType = "application/json";
                    break;
                default:
                    serialized = string.Empty;
                    contentType = string.Empty;
                    break;
            }

            return AddParameter(contentType, serialized, ParameterType.RequestBody);
        }
        
        public IRestRequest AddUrlSegment(string name, object value)
        {
            if(Resource.Contains("{" + name + "}"))
            {
                Resource = Resource.Replace("{" + name + "}", value.ToString());
            }

            return this;
        }

        private string SerializeAsXml(object o)
        {
            var serializer = new XmlSerializer(o.GetType());
            var writer = new StringWriter();
            serializer.Serialize(writer, o);

            return writer.ToString();
        }
    }
}
