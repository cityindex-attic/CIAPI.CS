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
                if (typeof(Array).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    foreach (var item in (Array)propertyInfo.GetValue(dto, null))
                    {
                        formattedValue += item.ToStringWithValues();
                    }
                    
                }
                else
                {
                    switch (propertyInfo.PropertyType.Name)
                    {
                        case "DateTime":
                            formattedValue = ((DateTime)propertyInfo.GetValue(dto, null)).ToString("u");
                            break;
                        default:
                            formattedValue = propertyInfo.GetValue(dto, null).ToString();
                            break;
                    }
                }
                sb.AppendFormat("\t{0}={1}", propertyInfo.Name, formattedValue);
            }
            return string.Format("{0}: \n{1}\n", dto.GetType().Name, sb);
        }
    }
}