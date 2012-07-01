using System;

namespace Salient.ReliableHttpClient
{
    public enum ContentType
    {
        JSON,
        FORM,
        XML,
        TEXT
    }

    public static class Extensions
    {
        public static string ToHeaderValue(this ContentType value)
        {
            switch (value)

            {
                case ContentType.JSON:
                    return "application/json";
                    
                case ContentType.FORM:

                    return "application/x-www-form-urlencoded";
                case ContentType.XML:

                    return "application/xml";
                case ContentType.TEXT:
                    return "text/plain";
                default:
                    throw new ArgumentException("unrecognized ContentType " + value);
            }
        }
    }
}