using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRtRest.RestClient.Authenticators
{
    public interface IAuthenticator
    {
        void Authenticate(IRestClient client, IRestRequest request);
    }
}
