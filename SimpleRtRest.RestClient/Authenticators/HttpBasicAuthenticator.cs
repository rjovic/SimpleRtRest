using System;
using System.Text;

namespace SimpleRtRest.RestClient.Authenticators
{
    public class HttpBasicAuthenticator : IAuthenticator
    {
        private string _username;
        private string _password;

        public HttpBasicAuthenticator(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _username, _password)));
            var authorizationHeader = string.Format("Basic {0}", token);

            client.Parameters.Add(new Parameter {Name = "Authorization", Value = authorizationHeader, Type = ParameterType.Header});
        }
    }
}
