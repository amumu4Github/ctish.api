using ctish.api.Api.Base;
using ctish.api.Apis.Common;
using ctish.api.Apis.Config;
using ctish.api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctish.api.Apis.User
{
    /// <summary>
    /// 退出登陆用户 的摘要说明
    /// </summary>
    public class LogOutHandler : BaseHandler
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
                UserTokenModel tokenModel = token.getUserToken(requestBody.accessToken);
                if (tokenModel == null)
                {
                    SystemResponse.Output(SystemResponse.TYPE_EXPIRE, out statusCode, out responseJson);
                }
                else
                {
                    tokenModel.isLogin = 0;
                    tokenModel.loginTime = 0;
                    tokenModel.accessToken = "";
                    tokenModel.bpuserId = "";
                    tokenModel.channelId = "";
                    tokenModel.deviceToken = "";
                    tokenModel.deviceType = 0;
                    if (token.updateToken(tokenModel) >= 1)
                    {
                        SystemResponse.Output(SystemResponse.TYPE_OK, out statusCode, out responseJson);
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