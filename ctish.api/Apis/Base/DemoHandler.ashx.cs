using ctish.api.Api.Base;
using ctish.api.Apis.Common;
using ctish.api.Apis.Config;
using ctish.api.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ctish.api.Apis.Base
{
    /// <summary>
    /// DemoHandler 的摘要说明
    /// </summary>
    public class DemoHandler : BaseHandler
    {

        private RequestBody requestBody;
        private ResponseBody responseBody;
        public override void OnLoad(HttpContext context)
        {
            base.OnLoad(context);
            requestBody = new RequestBody();
            requestBody.accessToken = context.Request["accessToken"];
            if (requestBody.accessToken == null || requestBody.accessToken.Trim().Length == 0)
            {
                SystemResponse.Output(SystemResponse.TYPE_NULLPARAMETER, out statusCode, out responseJson);
            }
            else
            {
                //验证用户
                TokenHelper token = new TokenHelper();
                UserTokenModel userTokenModel = token.getUserToken(requestBody.accessToken);
                if (userTokenModel == null)
                {
                    SystemResponse.Output(SystemResponse.TYPE_EXPIRE, out statusCode, out responseJson);
                }
                else
                {
                    object model = new object();
                    if (model != null)
                    {

                        responseBody = new ResponseBody();
                        responseJson = JsonConvert.SerializeObject(responseBody, Formatting.Indented);
                    }
                    else
                    {
                        SystemResponse.Output(SystemResponse.TYPE_NULL, out statusCode, out responseJson);
                    }
                }
            }
        }


        /// <summary>
        /// 请求参数类
        /// </summary>
        public class RequestBody
        {
            public string accessToken { set; get; }
        }

        /// <summary>
        /// 返回参数类
        /// </summary>
        public class ResponseBody
        {
        }
    }
}