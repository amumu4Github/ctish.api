using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ctish.api.ADO;
using ctish.api.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using ctish.api.Apis.Common;
using ctish.api.Apis.Config;
using ctish.api.Api.Base;
namespace ctish.api.Api.User
{
    /// <summary>
    /// 用户注册 的摘要说明
    /// </summary>
    public class RegisterHandler : BaseHandler
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
            if (requestBody.userName == null || requestBody.userPwd == null || requestBody.userType == null)
            {
                SystemResponse.Output(SystemResponse.TYPE_NULLPARAMETER, out statusCode, out responseJson);
            }
            else
            {
                string password = StringHelper.StringToMD5(requestBody.userPwd);

                ModelAdo<UserModel> userModel = new ModelAdo<UserModel>();
                UserModel model = userModel.GetModel(" rid=?rid and name =?name", "",
                      new MySqlParameter("?rid", requestBody.userType),
                      new MySqlParameter("?name", requestBody.userName));
                if (model != null)
                {
                    SystemResponse.Output(SystemResponse.TYPE_EXIST, out statusCode, out responseJson);
                }
                else
                {
                    UserModel token = new UserModel();
                    token.rid = Convert.ToInt32(requestBody.userType);
                    token.name = requestBody.userName;
                    token.password = password;
                    token.ctime = StringHelper.ConvertDateTimeInt(DateTime.Now);
                    if (userModel.Insert(token) >= 1)
                    {
                        responseJson = JsonConvert.SerializeObject(responseBody, Formatting.Indented);

                    }
                    else
                    {
                        SystemResponse.Output(SystemResponse.TYPE_REGISTER_ERROR, out statusCode, out responseJson);
                    }
                }

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 请求参数类
        /// </summary>
        public class RequestBody
        {
            public string userName { set; get; }
            public string userPwd { set; get; }
            public string userType { set; get; }
        }

        /// <summary>
        /// 返回参数类
        /// </summary>
        public class ResponseBody
        {
        }
    }
}