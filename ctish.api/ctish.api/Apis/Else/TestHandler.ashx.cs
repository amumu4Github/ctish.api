using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctish.api.Apis.Else
{
    /// <summary>
    /// TestHandler 的摘要说明
    /// </summary>
    public class TestHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string strLen = "\"id\":67,\"ufrom\":2,\"uto\":1,\"content\":\"\",\"createTime\":\"2014/07/2516:31:24\",\"type\":0,\"status\":1,\"aps\":{\"alert\":\"收到一条新消息\"}";
            context.Response.Write(strLen.Length);
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