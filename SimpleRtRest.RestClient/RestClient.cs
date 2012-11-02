﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using SimpleRtRest.RestClient.Authenticators;
using System.Net;
using System.Text;

namespace SimpleRtRest.RestClient
{
    public class RestClient : IRestClient
    {
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
            foreach (var parameter in request.Parameters.Where(p => p.Type == ParameterType.Content))
            {
                if(parameters.Length > 0)
                {
                    parameters = parameters + "&";
                }

                parameters = parameters + (String.Format("{0}={1}", WebUtility.UrlEncode(parameter.Name), WebUtility.UrlEncode(parameter.Value.ToString())));
            }

            if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
            {
                _request.Content = new StringContent(parameters, Encoding.UTF8, MediaTypes.FormUrlEncoded);
            }
        }
        
        public async Task<IRestResponse> Execute(IRestRequest request)
        {
            _request = new HttpRequestMessage(request.Method, request.Resource);
                        
            DoAuthentication();

            ConfigureHttp();
            AddParameters(request);
            
            try
            {
                var response = await _client.SendAsync(_request);
                var content = await response.Content.ReadAsStringAsync();

                return new RestResponse()
                {
                    RawData = content,
                    StatusCode = response.StatusCode
                };
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        public async Task<IRestResponse<T>> Execute<T>(IRestRequest request) where T : new()
        {
            return Deserialize<T>(await Execute(request));
        }

        private RestResponse<T> Deserialize<T>(IRestResponse raw)
        {
            var response = new RestResponse<T>();
            try
            {
                response.RawData = raw.RawData;
                response.StatusCode = raw.StatusCode;
                response.Data = JsonConvert.DeserializeObject<T>(raw.RawData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : ", ex.Message);
                Debug.WriteLine("Data : ", raw.RawData);
                // create excpetion var in response and pass it here
            }

            return response;
        }
    }
}
