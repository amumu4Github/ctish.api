using ctish.api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using ctish.api.ADO;

namespace ctish.api.Apis.Common
{
    public class TokenHelper
    {
        private ModelAdo<UserTokenModel> userTokenModel;
        /// <summary>
        /// token 信息，获取token详细信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public UserTokenModel getUserToken(string accessToken)
        {
            userTokenModel = new ModelAdo<Model.UserTokenModel>();
            UserTokenModel model = userTokenModel.GetModel(" accessToken=?accessToken ", "", new MySqlParameter("?accessToken", accessToken));

            return model;
        }
        /// <summary>
        /// token 信息，获取token详细信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public UserTokenModel getUserToken(int uid)
        {
            userTokenModel = new ModelAdo<Model.UserTokenModel>();
            UserTokenModel model = userTokenModel.GetModel(" uid=?uid ", "", new MySqlParameter("?uid", uid));

            return model;
        }

        /// <summary>
        /// token 信息，修改token详细信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public int updateToken(UserTokenModel model)
        {
            int obj = 0;
            userTokenModel = new ModelAdo<Model.UserTokenModel>();
            UserTokenModel token = userTokenModel.GetModel(" uid =?uid ", "", new MySqlParameter("?uid", model.uid));
            if (token != null)
            {
                model.id = token.id;
                obj = userTokenModel.Update(model);
            }
            else
            {
                obj = userTokenModel.Insert(model);
            }
            return obj;
        }
    }
}