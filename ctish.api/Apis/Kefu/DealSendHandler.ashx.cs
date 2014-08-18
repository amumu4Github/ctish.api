using ctish.api.ADO;
using ctish.api.Api.Base;
using ctish.api.Apis.Common;
using ctish.api.Apis.Config;
using ctish.api.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctish.api.Apis.Kefu
{
    /// <summary>
    /// 客服对派单信息处理 的摘要说明
    /// </summary>
    public class DealSendHandler : BaseHandler
    {
        private RequestBody requestBody;
        private ResponseBody responseBody;
        public override void OnLoad(HttpContext context)
        {
            base.OnLoad(context);
            requestBody = new RequestBody();
            requestBody.accessToken = context.Request["accessToken"];
            requestBody.oid = context.Request["oid"];
            requestBody.action = Convert.ToInt32(context.Request["action"]);
            if (requestBody.accessToken == null || requestBody.accessToken.Trim().Length == 0 || requestBody.oid.Length == 0 || requestBody.action == 0)
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
                    #region 客服取消供应商抢单数据、修改订单状态回派单中
                    if (requestBody.action == 2)
                    {
                        //获取当前订单信息
                        ModelAdo<OrderModel> modelAdo = new ModelAdo<OrderModel>();
                        OrderModel order = modelAdo.GetModel("id=?oid AND ostatus=?ostatus", "",
                                new MySqlParameter("?oid", requestBody.oid),
                                new MySqlParameter("?ostatus", 3));
                        if (order != null)
                        {
                            //获取派单供应商对应订单中间表数据
                            ModelAdo<OrderUserModel> ouModelAdo = new ModelAdo<OrderUserModel>();
                            OrderUserModel orderUser = ouModelAdo.GetModel("oid=?oid AND uid=?uid", "",
                                new MySqlParameter("?oid", requestBody.oid),
                                new MySqlParameter("?uid", order.sendUid));
                            orderUser.amount = 0;
                            orderUser.remark = "被供应商退回抢单数据";
                            orderUser.createTime = 0;
                            orderUser.status = 1;
                            if (ouModelAdo.Update(orderUser) >= 1)
                            {
                                order.ostatus = 2;
                                order.sendUid = 0;
                                order.sendTime = 0;
                                order.sendRemark = "被供应商退回抢单数据";
                                order.sendAmount = 0;
                                if (modelAdo.Update(order) >= 1)
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
                    #endregion

                    #region 客服确认供应商抢单数据
                    else if (requestBody.action == 4)
                    {
                        //获取当前订单信息
                        ModelAdo<OrderModel> modelAdo = new ModelAdo<OrderModel>();
                        OrderModel order = modelAdo.GetModel("id=?oid AND ostatus=?ostatus", "",
                                new MySqlParameter("?oid", requestBody.oid),
                                new MySqlParameter("?ostatus", 3));
                        order.ostatus = 4;
                        order.sendRemark = "客服确认供应商抢单数据";
                        if (order != null)
                        {
                            if (modelAdo.Update(order) >= 1)
                            {

                                //删除派单供应商对应订单中间表数据
                                ModelAdo<OrderUserModel> ouModelAdo = new ModelAdo<OrderUserModel>();
                                int delCount = ouModelAdo.ExecuteSql("DELETE FROM ct_order_user WHERE oid=?oid",
                                     new MySqlParameter("?oid", requestBody.oid));
                                if (delCount >= 1)
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
                    #endregion

                    #region 客服确认关闭已确认数据
                    else if (requestBody.action == 5)
                    {
                        //获取当前订单信息
                        ModelAdo<OrderModel> closeModelAdo = new ModelAdo<OrderModel>();
                        OrderModel closeOrder = closeModelAdo.GetModel("id=?oid AND ostatus=?ostatus", "",
                                new MySqlParameter("?oid", requestBody.oid),
                                new MySqlParameter("?ostatus", 4));
                        closeOrder.ostatus = 5;
                        closeOrder.sendRemark = "客服确认完成订单数据";
                        if (closeOrder != null)
                        {
                            if (closeModelAdo.Update(closeOrder) >= 1)
                            {
                                SystemResponse.Output(SystemResponse.TYPE_OK, out statusCode, out responseJson);

                                //删除派单供应商对应订单中间表数据
                                ModelAdo<OrderUserModel> ouModelAdo = new ModelAdo<OrderUserModel>();
                                int delCount = ouModelAdo.ExecuteSql("DELETE FROM ct_order_user WHERE oid=?oid",
                                     new MySqlParameter("?oid", requestBody.oid));
                                //if (delCount >= 1)
                                //{
                                //}
                                //else
                                //{
                                //}
                            }
                            else
                            {
                                SystemResponse.Output(SystemResponse.TYPE_NULL, out statusCode, out responseJson);
                            }
                        }
                        else
                        {
                            SystemResponse.Output(SystemResponse.TYPE_NULL, out statusCode, out responseJson);
                        }
                    }
                    #endregion

                    #region 其他
                    else
                    {
                        SystemResponse.Output(SystemResponse.TYPE_NULLPARAMETER, out statusCode, out responseJson);
                    }
                    #endregion

                }

            }
        }


        /// <summary>
        /// 请求参数类
        /// </summary>
        public class RequestBody
        {
            public string accessToken { set; get; }
            /// <summary>
            /// 处理动作：2:取消派单个相关供应商；4:确认派单给相关供应商；
            /// </summary>
            public int action { set; get; }
            public string oid { set; get; }
        }

        /// <summary>
        /// 返回参数类
        /// </summary>
        public class ResponseBody
        {
        }
    }
}