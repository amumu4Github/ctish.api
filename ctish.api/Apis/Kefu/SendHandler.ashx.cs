using ctish.api.ADO;
using ctish.api.Api.Base;
using ctish.api.Apis.Common;
using ctish.api.Apis.Config;
using ctish.api.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;

namespace ctish.api.Apis.Kefu
{
    /// <summary>
    /// 客服派单 的摘要说明
    /// </summary>
    public class SendHandler : BaseHandler
    {


        private RequestBody requestBody;
        private ResponseBody responseBody;
        public override void OnLoad(HttpContext context)
        {
            base.OnLoad(context);
            requestBody = new RequestBody();
            requestBody.accessToken = context.Request["accessToken"];
            requestBody.oid = context.Request["oid"];
            requestBody.uid = context.Request["uid"];
            if (requestBody.accessToken == null || requestBody.accessToken.Trim().Length == 0 || requestBody.oid.Length == 0 || requestBody.uid.Length == 0)
            {
                SystemResponse.Output(SystemResponse.TYPE_NULLPARAMETER, out statusCode, out responseJson);
            }
            else
            {

                List<int> values = JsonConvert.DeserializeObject<List<int>>(requestBody.uid);

                //验证用户
                TokenHelper token = new TokenHelper();
                UserTokenModel userTokenModel = token.getUserToken(requestBody.accessToken);
                if (userTokenModel == null)
                {
                    SystemResponse.Output(SystemResponse.TYPE_EXPIRE, out statusCode, out responseJson);
                }
                else
                {

                    //获取订单表数据
                    ModelAdo<OrderModel> modelAdoOrder = new ModelAdo<OrderModel>();
                    OrderModel orderModel = modelAdoOrder.GetModel("(id=?id AND ostatus=?ostatus) or (id=?id AND ostatus=?ostatus1) ", "",
                        new MySqlParameter("?id", requestBody.oid),
                        new MySqlParameter("?ostatus", 2),
                        new MySqlParameter("?ostatus1", 1));

                    if (orderModel != null)
                    {
                        ModelAdo<OrderUserModel> modelAdo = new ModelAdo<OrderUserModel>();
                        int existCount = modelAdo.GetRecordCount("oid=?oid",
                            new MySqlParameter("?oid", requestBody.oid));
                        if (existCount >= 1)
                        {
                            int delCount = modelAdo.ExecuteSql("DELETE FROM ct_order_user WHERE oid=?oid",
                                new MySqlParameter("?oid", requestBody.oid));
                            if (delCount >= 1)
                            {
                                StringBuilder sbValues = new StringBuilder();
                                sbValues.Append(" INSERT INTO ct_order_user(oid,uid,status,remark) VALUES ");
                                for (int i = 0; i < values.Count; i++)
                                {
                                    sbValues.Append("(" + requestBody.oid + "," + values[i] + ",1,'派送中的订单'),");
                                }
                                sbValues.Remove(sbValues.Length - 1, 1).Append(";");
                                int inCount = modelAdo.ExecuteSql(sbValues.ToString());
                                if (inCount >= 1)
                                {
                                    if (orderModel != null)
                                    {
                                        orderModel.ostatus = 2;
                                        modelAdoOrder.Update(orderModel);
                                    }
                                    SystemResponse.Output(SystemResponse.TYPE_OK, out statusCode, out responseJson);
                                }
                                else
                                {
                                    SystemResponse.Output(SystemResponse.TYPE_NULL, out statusCode, out responseJson);
                                }

                            }
                            else
                            {

                                SystemResponse.Output(SystemResponse.TYPE_ERROR, out statusCode, out responseJson);
                            }
                        }
                        else
                        {
                            StringBuilder sbValues = new StringBuilder();
                            sbValues.Append(" INSERT INTO ct_order_user(oid,uid,status) VALUES ");
                            for (int i = 0; i < values.Count; i++)
                            {
                                sbValues.Append("(" + requestBody.oid + "," + values[i] + ",1),");
                            }
                            sbValues.Remove(sbValues.Length - 1, 1).Append(";");
                            int inCount = modelAdo.ExecuteSql(sbValues.ToString());
                            if (inCount >= 1)
                            {
                                SystemResponse.Output(SystemResponse.TYPE_OK, out statusCode, out responseJson);
                            }
                            else
                            {
                                SystemResponse.Output(SystemResponse.TYPE_NULL, out statusCode, out responseJson);
                            }
                        }
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
            public string oid { set; get; }
            public string uid { set; get; }
        }

        /// <summary>
        /// 返回参数类
        /// </summary>
        public class ResponseBody
        {
            public string msg { get; set; }
        }
    }
}