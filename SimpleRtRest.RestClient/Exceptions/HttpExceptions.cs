using System;

namespace SimpleRtRest.RestClient.Exceptions
{
    public class HttpConnectionNotAvaliable : Exception
    {
        public HttpConnectionNotAvaliable() : base()
        {
            
        }

        public HttpConnectionNotAvaliable(string message) : base(message)
        {
            
        }
    }

    public class HttpException : Exception
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
