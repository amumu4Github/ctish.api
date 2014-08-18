using ctish.api.Apis.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ctish.api.Api.Base
{
    /// <summary>
    /// 请求接口基类
    /// </summary>
    public class BaseHandler : IHttpHandler
    {
        public int statusCode = 200;
        public object responseJson = null;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = SystemConfig.ContentType;
            context.Response.HeaderEncoding = SystemConfig.Encoding;
            context.Response.ContentEncoding = SystemConfig.Encoding; 
            //处理代码
            OnLoad(context);

            context.Response.StatusCode = statusCode;
            context.Response.Write(responseJson);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 代码实现
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnLoad(HttpContext context)
        {
        }

    }
}