using Newtonsoft.Json;

namespace Salient.HAR.Model
{

    /// <summary>
    /// WARNING: the gold standard HAR viewer is a foward cursor parser and expects fields to be in order as per spec.
    /// refactoring this source code may result in undesirable serialization behavior
    /// </summary>
    public class HTTPArchiveType
    {
        //// HAR Schema Definition  
        //  
        //// Date time fields use ISO8601 (YYYY-MM-DDThh:mm:ss.sTZD, e.g. 2009-07-24T19:20:30.45+01:00)  
        //var dateTimePattern = /^(\d{4})(-)?(\d\d)(-)?(\d\d)(T)?(\d\d)(:)?(\d\d)(:)?(\d\d)(\.\d+)?(Z|([+-])(\d\d)(:)?(\d\d))/;  
        //  


        [JsonProperty(PropertyName = "log")]
        public LogType Log = new LogType();
    }
}

 