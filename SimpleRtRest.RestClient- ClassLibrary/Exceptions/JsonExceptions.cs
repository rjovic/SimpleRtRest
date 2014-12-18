using System;
namespace SimpleRtRest.RestClient.Exceptions
{
    public class JsonDeserializationException : Exception
    {
        public JsonDeserializationException() : base()
        {
            
        }

        public JsonDeserializationException(string message) : base(message)
        {
            
        }
    }
}
