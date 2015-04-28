namespace SimpleRtRest.RestClient.Authenticators
{
    public interface IAuthenticator
    {
        void Authenticate(IRestClient client, IRestRequest request);
    }
}
