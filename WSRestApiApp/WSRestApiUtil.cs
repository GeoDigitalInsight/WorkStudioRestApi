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
    }
}
