using System;
using System.Configuration;

namespace CassiniDev.Configuration
{
    ///<summary>
    ///</summary>
    public class CassiniDevConfigurationSection : ConfigurationSection
    {

        ///<summary>
        ///</summary>
        public static CassiniDevConfigurationSection Instance
        {
            get
            {
                try
                {
                    object section = ConfigurationManager.GetSection("cassinidev");
                    if (section != null)
                    {
                        return (CassiniDevConfigurationSection)section;
                    }
                }

#pragma warning disable 168
                catch (Exception ex)
#pragma warning restore 168
                {
                    
                    // #TODO: log it
                    throw;
                }
                return null;
            }
        }

        ///<summary>
        ///</summary>
        [ConfigurationProperty("profiles")]
        public CassiniDevProfileElementCollection Profiles
        {
            get
            {
                return (CassiniDevProfileElementCollection)this["profiles"];
            }
        }
    }
}
