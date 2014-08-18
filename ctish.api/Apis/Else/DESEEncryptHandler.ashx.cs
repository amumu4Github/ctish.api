using ctish.api.Apis.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctish.api.Api.Else
{
    /// <summary>
    /// DESEEncryptHandler 的摘要说明[弃用，nginx报错]
    /// </summary>
    public class DESEEncryptHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string key = context.Request["key"];
            string type = context.Request["type"];
            string returnStr = "";
            if ("1".Equals(type))
            {
                returnStr = DESEncrypt.Encrypt(key);
            }
            else {
                returnStr = DESEncrypt.Decrypt(key);
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnStr);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}