using ctish.api.ADO;
using ctish.api.Api.Base;
using ctish.api.Apis.Common;
using ctish.api.Apis.Config;
using ctish.api.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctish.api.Apis.User
{
    /// <summary>
    /// 用户关系列表 的摘要说明
    /// </summary>
    public class FriendShipHandler : BaseHandler
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
                    ModelAdo<UserModel> userModel = new ModelAdo<UserModel>();
                    List<UserModel> models = userModel.GetList(" id NOT IN (?id) AND status != 1", "","",
                        new MySqlParameter("?id", userTokenModel.uid));
                    if (models != null)
                    {
                        List<Friend> Friends = new List<Friend>();
                        foreach (UserModel model in models) {
                            Friend friend = new Friend();
                            friend.id = model.id;
                            friend.name = model.name;
                            friend.avatar = model.avatar;
                            Friends.Add(friend);
                        }
                        responseBody = new ResponseBody();
                        responseBody.Friends = Friends;
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
            public  List<Friend> Friends { get; set; }
        }

        public class Friend {
            public int id { get; set; }
            public string name { get; set; }
            public string avatar { get; set; }
        }
    }
}