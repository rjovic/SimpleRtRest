using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

using SimpleRtRest.RestClient.Authenticators;
using SimpleRtRest.RestClient.Exceptions;

namespace SimpleRtRest.RestClient
{
    public class RestClient : IRestClient
    {
        public bool IgnoreNullOnDeserialization { get; set; }

        private readonly HttpClient _client;
        private HttpRequestMessage _request;
       
        public RestClient()
        {
            _client = new HttpClient();

            Parameters = new List<Parameter>();
            DefaultUrlParameters = new List<Parameter>();
        }

        public RestClient(string baseUrl) : this()
        {
            BaseUrl = baseUrl;
        }

        public string UserAgent { get; set; }
        public string BaseUrl { get; set; }

        public IAuthenticator Authenticator { get; set; }
        
        public List<Parameter> Parameters { get; set; } 
        public List<Parameter> DefaultUrlParameters { get; set; }

        public void AddDefaultUrlSegment(string name, object value)
        {
            DefaultUrlParameters.Add(new Parameter { Name = name, Value = value });
        }

        private bool _isClientConfigured;

        private void ConfigureHttp()
        {
            if (!_isClientConfigured)
            {
                _client.BaseAddress = new Uri(BaseUrl);
                _client.DefaultRequestHeaders.Add("user-agent", UserAgent.Length > 0 ? UserAgent : "simpleRtRest-client-1.0");

               // check parameters (including httpBasic and SimpleAuthenticator params
                foreach (var parameter in Parameters)
                {
                    _client.DefaultRequestHeaders.Add(parameter.Name, parameter.Value.ToString());
                }

                _isClientConfigured = true;
            }

            var resource = _request.RequestUri.OriginalString;

            if (DefaultUrlParameters.Count > 0)
            {
                foreach (var urlParameter in DefaultUrlParameters)
                {
                    if (resource.Contains("{" + urlParameter.Name + "}"))
                    {
                        resource = resource.Replace("{" + urlParameter.Name + "}", urlParameter.Value.ToString());
                    }
                }

                _request.RequestUri = new Uri(resource, UriKind.Relative);
            }                      
        }
        
        private void DoAuthentication()
        {
            if(Authenticator != null)
            {
                Authenticator.Authenticate(this, null);
            }
        }

        private void AddParameters(IRestRequest request)
        {
            string parameters = String.Empty;
            foreach (var parameter in request.Parameters.Where(p => p.Type == ParameterType.GetOrPost))
            {
                if(parameters.Length > 0)
                {
                    parameters = parameters + "&";
                }

                parameters = parameters + (String.Format("{0}={1}", WebUtility.UrlEncode(parameter.Name), WebUtility.UrlEncode(parameter.Value.ToString())));
            }

            var body = request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);

            if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
            {
                if(body != null && parameters.Length > 0)
                {
                    throw new Exception("You cannot send UrlFormEncoded and json/xml data together in body content.");
                }

                if(body != null)
                {
                    // little hack : parametar name have MediaType info when ParametarType is RequestBody
                    _request.Content = new StringContent(body.Value.ToString(), Encoding.UTF8, body.Name);
                }
                else
                {
                    _request.Content = new StringContent(parameters, Encoding.UTF8, MediaTypes.FormUrlEncoded);
                }
            }
            else if(request.Method == HttpMethod.Get)
            {
                var resource = _request.RequestUri.OriginalString;
                resource = resource + "?" + parameters;

                _request.RequestUri = new Uri(resource, UriKind.Relative);
            }
        }
        
        public async Task<IRestResponse> Execute(IRestRequest request)
        {
            _request = new HttpRequestMessage(request.Method, request.Resource);
                        
            DoAuthentication();

            ConfigureHttp();
            AddParameters(request);

            HttpResponseMessage response = null;
            string content = null;

            if(!String.IsNullOrEmpty(request.RequestFormat))
            {
                _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(request.RequestFormat));
            }
            
            try
            {
                response = await _client.SendAsync(_request);
                content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    return new RestResponse()
                        {
                            RawData = content,
                            ResponseFormat = response.Content.Headers.ContentType.MediaType,
                            StatusCode = response.StatusCode
                        };
                }
               
                return new RestResponse()
                    {
                        ErrorMessage = content,
                        ResponseFormat = response.Content.Headers.ContentType.MediaType,
                        StatusCode = response.StatusCode
                    };
            }
            catch(HttpRequestException)
            {
                throw new HttpConnectionNotAvaliable();
            }
            catch (Exception ex)
            {
                if(response != null)
                {
                    return new RestResponse()
                        {
                            // return error code
                            StatusCode = response.StatusCode,
                            ErrorMessage = content,
                            ResponseFormat = response.Content.Headers.ContentType.MediaType,
                            ErrorException = ex
                        };
                }

                throw new HttpException(ex.Message);
            }
        }

        public async Task<IRestResponse<T>> Execute<T>(IRestRequest request) where T : new()
        {
            return Deserialize<T>(await Execute(request), request);
        }

        private RestResponse<T> Deserialize<T>(IRestResponse raw, IRestRequest request)
        {
            if (raw.StatusCode == HttpStatusCode.OK || raw.StatusCode == HttpStatusCode.Created)
            {
                var response = new RestResponse<T>();
                try
                {
                    response.Request = request;

                    response.RawData = raw.RawData;
                    response.StatusCode = raw.StatusCode;
                    response.ResponseFormat = raw.ResponseFormat;

                    Debug.WriteLine(raw.RawData);

                    if (response.ResponseFormat == DataFormat.Json)
                    {
                        if (IgnoreNullOnDeserialization)
                        {
                            var settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
                            response.Data = JsonConvert.DeserializeObject<T>(raw.RawData, settings);
                        }
                        else
                        {
                            response.Data = JsonConvert.DeserializeObject<T>(raw.RawData);
                        }
                    }
                    else
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        using (var reader = new StringReader(response.RawData))
                        {
                            response.Data = (T)serializer.Deserialize(reader);
                        }
                    }
                }
                catch (JsonException ex)
                {
                    // create excpetion var in response and pass it here
                    throw new JsonDeserializationException(ex.Message);
                }
                catch(InvalidOperationException ex)
                {
                    // create excpetion var in response and pass it here
                    throw new XmlDeserializationException(ex.Message);
                }

                return response;
            }

            return new RestResponse<T> {StatusCode = raw.StatusCode};
        }
    }
}
