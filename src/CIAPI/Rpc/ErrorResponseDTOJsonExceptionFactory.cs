using System;
using CIAPI.DTO;
using Salient.JsonClient;
using Newtonsoft.Json;

namespace CIAPI.Rpc
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
                        // we cannot guarantee that for some strange reason a news item will not contain the strings
                        // searched for above so just need to try to parse the json and swallow
                        ApiErrorResponseDTO errorResponseDTO = JsonConvert.DeserializeObject<ApiErrorResponseDTO>(json);
                        ApiException ex = new ApiException(errorResponseDTO.ErrorMessage + (!string.IsNullOrEmpty(extraInfo) ? "\r\n" + extraInfo : ""));
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