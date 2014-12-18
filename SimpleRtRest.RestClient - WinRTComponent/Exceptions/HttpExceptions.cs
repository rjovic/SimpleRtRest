using System;

namespace SimpleRtRest.RestClient.Exceptions
{
    internal class HttpConnectionNotAvaliable : Exception
    {
        public HttpConnectionNotAvaliable() : base()
        {
            
        }

        public HttpConnectionNotAvaliable(string message) : base(message)
        {
            
        }
    }

    internal class HttpException : Exception
    {
        public HttpException()
            : base()
        {

        }

        public HttpException(string message)
            : base(message)
        {

        }
    }
}
