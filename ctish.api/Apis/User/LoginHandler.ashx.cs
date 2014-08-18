using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ctish.api.ADO;
using ctish.api.Model;
using MySql.Data.MySqlClient;
using ctish.api.Apis.Common; 
using ctish.api.Apis.Config;
using ctish.api.Api.Base;

namespace ctish.api.Api.User
{
    /// <summary>
    /// UserHandler 的摘要说明
    /// </summary>
    public class LoginHandler : BaseHandler
    { 
        private RequestBody requestBody;
        private ResponseBody responseBody;
        public override void OnLoad(HttpContext context)
        {
            base.OnLoad(context);
            requestBody = new RequestBody();
            requestBody.userName = context.Request["userName"];
            requestBody.userPwd = context.Request["userPwd"];
            requestBody.userType = context.Request["userType"];
            requestBody.bpuserid = context.Request["bpuserid"];
            requestBody.channelid = context.Request["channelid"];
            requestBody.deviceToken = context.Request["deviceToken"];
            requestBody.deviceType = context.Request["deviceType"];
            if (requestBody.userType == null || requestBody.userName == null || requestBody.userPwd == null)
            {
                SystemResponse.Output(SystemResponse.TYPE_NULLPARAMETER, out statusCode, out responseJson);
            }
            else
            {
                string password = StringHelper.StringToMD5(requestBody.userPwd);
                ModelAdo<UserModel> userModel = new ModelAdo<UserModel>();
                UserModel model = userModel.GetModel(" rid=?rid and name =?name and password=?password", "",
                      new MySqlParameter("?rid", requestBody.userType),
                      new MySqlParameter("?name", requestBody.userName),
                      new MySqlParameter("?password", password));

                if (model == null)
                { 
                    SystemResponse.Output(SystemResponse.TYPE_LOGIN_ERROR, out statusCode, out responseJson);
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    string accessToken = StringHelper.StringToMD5(dt.ToString());
                    TokenHelper token = new TokenHelper();
                    UserTokenModel modelUserToken = new UserTokenModel();
                    modelUserToken.uid = model.id.ToString();
                    modelUserToken.rid = model.rid;
                    modelUserToken.accessToken = accessToken;
                    modelUserToken.bpuserId = requestBody.bpuserid;
                    modelUserToken.channelId = requestBody.channelid;
                    modelUserToken.deviceToken = requestBody.deviceToken;
                    modelUserToken.deviceType =Convert.ToInt32( requestBody.deviceType);
                    modelUserToken.loginTime = StringHelper.ConvertDateTimeInt(DateTime.Now);
                    modelUserToken.isLogin = 1;
                    token.updateToken(modelUserToken);

                    responseBody = new ResponseBody
                    {
                        accessToken = accessToken,
                        uid = model.id.ToString(),
                        userName = model.name,
                        nickName = model.nickname,
                        userAvatar = model.avatar,
                        userType = model.rid
                    };
                    responseJson = JsonConvert.SerializeObject(responseBody, Formatting.Indented);
                }
            } 
        }
         
        /// <summary>
        /// 请求参数类
        /// </summary>
        public class RequestBody
        {
            public string userName { get; set; }
            public string userPwd { get; set; }
            public string userType { get; set; }
            public string deviceToken { get; set; }
            public string bpuserid { get; set; }
            public string channelid { get; set; }
            public string deviceType { get; set; }
        }

        /// <summary>
        /// 返回参数类
        /// </summary>
        public class ResponseBody
        {
            public string accessToken { get; set; }
            public string uid { get; set; }
            public string userName { get; set; }
            public string nickName { get; set; }
            public string userAvatar { get; set; }
            public int userType { get; set; }
        }
    }
}