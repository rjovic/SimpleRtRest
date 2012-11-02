namespace SimpleRtRest.RestClient.Authenticators
{
    public class SimpleAuthenticator : IAuthenticator
    {
        private readonly string _usernameParamName;
        private readonly string _usernameParamValue;

        private readonly string _passwordParamName;
        private readonly string _passwordParamValue;

        public SimpleAuthenticator(string usernameParamName, string usernameParamValue, string passwordParamName, string passwordParamValue)
        {
            _usernameParamName = usernameParamName;
            _usernameParamValue = usernameParamValue;

            _passwordParamName = passwordParamName;
            _passwordParamValue = passwordParamValue;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            client.Parameters.Add(new Parameter() {Name = _usernameParamName, Value = _usernameParamValue});
            client.Parameters.Add(new Parameter() {Name = _passwordParamName, Value = _passwordParamValue});
        }
    }
}
