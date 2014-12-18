using System;
namespace SimpleRtRest.RestClient.Exceptions
{
    internal class JsonDeserializationException : Exception
    {
        public JsonDeserializationException() : base()
        {
            
        }

        public JsonDeserializationException(string message) : base(message)
        {
            
        }
    }
}
