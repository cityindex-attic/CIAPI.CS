using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CIAPI.Serialization;

namespace CIAPI
{
    ///<summary>
    /// Useful logging and debugging extensions
    ///</summary>
    public static class PrettyPrinterExtensions
    {
        ///<summary>
        /// Create string showing values of all public properties for object
        ///</summary>
        ///<param name="dto"></param>
        ///<returns></returns>
        public static string ToStringWithValues(this object dto)
        {
            var serializer = new Salient.ReliableHttpClient.Serialization.Newtonsoft.Serializer();
            return serializer.SerializeObject(dto);
        }
    }
}