using System;

namespace SimpleRtRest.RestClient.Exceptions
{
    internal class XmlDeserializationException : Exception
    {
        public XmlDeserializationException()
            : base()
        {

        }

        public XmlDeserializationException(string message)
            : base(message)
        {

        }
    }
}
