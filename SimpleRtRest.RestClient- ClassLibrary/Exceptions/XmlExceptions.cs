using System;

namespace SimpleRtRest.RestClient.Exceptions
{
    public class XmlDeserializationException : Exception
    {
        public XmlDeserializationException() : base()
        {

        }

        public XmlDeserializationException(string message) : base(message)
        {

        }
    }
}
