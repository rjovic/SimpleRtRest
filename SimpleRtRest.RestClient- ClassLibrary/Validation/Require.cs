using System;

namespace SimpleRtRest.RestClient.Validation
{
    public class Require
    {
        public static void Argument(string name, object value)
        {
            if (value == null)
            {
                throw new ArgumentException("Value cannot be null.", name);
            }
        }
    }
}
