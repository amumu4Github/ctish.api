using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ctish.api.BPush
{
    public class IOSAPS
    {
        public string alert { set; get; }
        //public string sound { set; get; }
        //public string badge { set; get; }

        public string getJsonString()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }
    }
}