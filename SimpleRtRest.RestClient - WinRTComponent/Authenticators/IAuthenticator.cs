namespace SimpleRtRest.RestClient.Authenticators
{
    internal interface IAuthenticator
    {
        void Authenticate(IRestClient client, IRestRequest request);
    }
}
