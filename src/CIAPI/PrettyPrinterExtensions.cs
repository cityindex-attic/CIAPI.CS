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
                var formattedValue = "";
                var value = propertyInfo.GetValue(dto, null) ?? "NULL";
                
                if (typeof(Array).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    try
                    {
                        foreach (var item in (Array)value)
                        {
                            formattedValue += item.ToStringWithValues();
                        }
                    }
                    catch (Exception e)
                    {
                        formattedValue += e.Message;
                    }
                }
                else
                {
                    switch (propertyInfo.PropertyType.Name)
                    {
                        case "DateTime":
                            formattedValue = ((DateTime)value).ToString("u");
                            break;
                        default:
                            if (propertyInfo.PropertyType.FullName.StartsWith("CIAPI.DTO"))
                            {
                                formattedValue = value.ToStringWithValues();  
                            }
                            else
                            {
                                formattedValue = value.ToString();
                            }
                            break;
                    }
                }
                sb.AppendFormat("\t{0}={1}", propertyInfo.Name, formattedValue);
            }
            return string.Format("{0}: \n{1}\n", dto.GetType().Name, sb);
        }
    }
}