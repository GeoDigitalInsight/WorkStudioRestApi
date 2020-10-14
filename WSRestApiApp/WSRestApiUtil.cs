using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace WSRestApiApp
{
    public class WSRestApiUtil
    {
        public static String NewGuid()
        {
            //WorkStudio Guids must be uppercase type B guid (https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=netcore-3.1)
            return Guid.NewGuid().ToString("B").ToUpper();
        }


        //WorkStudio does not tread JSON exactly to the spec.  JSON for WorkStudio is usually case specific but is 
        //not gauranteed to be.  Because of this we are safest to treat every property name like it is not
        //case specific.  The following is a template for doing this.
        public static T GetJSONValue<T>(JObject jsonObj, String ACaseInsensitivePropertyName)
        {
            JToken? val = jsonObj.GetValue(ACaseInsensitivePropertyName, StringComparison.OrdinalIgnoreCase);
            return val.Value<T>();
        }
    }
}
