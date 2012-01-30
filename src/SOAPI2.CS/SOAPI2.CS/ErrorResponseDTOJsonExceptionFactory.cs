using System;
using CityIndex.JsonClient;

namespace SOAPI2
{
    public class ErrorResponseDTOJsonExceptionFactory : IJsonExceptionFactory
    {
        public Exception ParseException(string json)
        {
            return ParseException(null, json, null);
        }

        public Exception ParseException(string extraInfo, string json, Exception inner)
        {
            if (!string.IsNullOrEmpty(json))
            {
                if (json.Contains("\"ErrorMessage\"") && json.Contains("\"ErrorCode\""))
                {
                    try
                    {
                        ApiException ex = new ApiException("");//#TODO
                        ex.ResponseText = json;
                        return ex;
                    }
                        // ReSharper disable EmptyGeneralCatchClause

                    catch
                        // ReSharper restore EmptyGeneralCatchClause
                    {


                    }
                }

            }
            return null;
        }
    }
}