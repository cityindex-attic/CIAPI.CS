using System;
using System.Reflection;
using System.Text;

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
            var sb = new StringBuilder();
            foreach (PropertyInfo propertyInfo in dto.GetType().GetProperties())
            {
                string formattedValue = "";
                switch (propertyInfo.PropertyType.Name)
                {
                    case "DateTime":
                        formattedValue = ((DateTime) propertyInfo.GetValue(dto, null)).ToString("u");
                        break;
                    default:
                        formattedValue = propertyInfo.GetValue(dto, null).ToString();
                        break;
                }
                sb.AppendFormat("\t{0}={1}", propertyInfo.Name, formattedValue);
            }
            return string.Format("{0}: \n{1}", dto.GetType().Name, sb);
        }
    }
}