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

namespace ctish.api.Apis.Supplier
{
    /// <summary>
    /// 供应商处理派单 的摘要说明
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
            requestBody.amount = Convert.ToDecimal(context.Request["money"]);
            if (requestBody.accessToken == null || requestBody.accessToken.Trim().Length == 0 || requestBody.oid.Length == 0 || requestBody.action == 0)
            {
                SystemResponse.Output(SystemResponse.TYPE_NULLPARAMETER, out statusCode, out responseJson);
            }
            else if (requestBody.action == 2 && requestBody.amount == 0)
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
                    ModelAdo<OrderUserModel> modelAdo = new ModelAdo<OrderUserModel>();

                    //核对有没有供应商已抢单
                    OrderUserModel sendOrder = modelAdo.GetModel(" oid =?oid AND status=2", "",
                            new MySqlParameter("?oid", requestBody.oid));
                    //核对供应商与订单状态
                    OrderUserModel model = modelAdo.GetModel("oid=?oid AND uid=?uid", "",
                        new MySqlParameter("?oid", requestBody.oid),
                        new MySqlParameter("?uid", userTokenModel.uid));
                    //数据是否存在派单中间表
                    if (model != null)
                    {
                        #region 供应商确认抢单【派单中间表已存在，并且订单状态为2，派单中】
                        if (2 == requestBody.action && sendOrder == null)
                        {
                            model.status = 2;
                            model.amount = requestBody.amount;
                            model.createTime = StringHelper.ConvertDateTimeInt(DateTime.Now);
                            model.remark = "被供应商抢单的订单";

                            //修改订单状态为3，并录入相关抢单数据
                            ModelAdo<OrderModel> orderModelAdo = new ModelAdo<OrderModel>();
                            OrderModel orderModel = orderModelAdo.GetModel("id=?id", "",
                                new MySqlParameter("?id", requestBody.oid)
                                );
                            if (orderModel != null)
                            {
                                orderModel.ostatus = 3;
                                orderModel.sendUid = Convert.ToInt32(userTokenModel.uid);
                                orderModel.sendTime = model.createTime;
                                orderModel.sendRemark = "供应商抢单并输入价格 【待处理订单】";
                                orderModel.sendAmount = model.amount;
                                if (orderModelAdo.Update(orderModel) >= 1)
                                {
                                    if (modelAdo.Update(model) >= 1)
                                    {
                                        SystemResponse.Output(SystemResponse.TYPE_OK, out statusCode, out responseJson);
                                    }
                                    else
                                    {
                                        SystemResponse.Output(SystemResponse.TYPE_ERROR, out statusCode, out responseJson);

                                    }
                                }

                            }
                            else
                            {
                                SystemResponse.Output(SystemResponse.TYPE_ERROR, out statusCode, out responseJson);
                            }

                        }
                        #endregion
                        #region 用户退回订单 【派单中间表已存在，并且订单状态为3，待处理订单】
                        else if (3 == requestBody.action && sendOrder != null && sendOrder.uid == Convert.ToInt32(userTokenModel.uid))
                        {


                            //修改订单状态为2，并清除相关抢单数据
                            ModelAdo<OrderModel> orderModelAdo = new ModelAdo<OrderModel>();
                            OrderModel orderModel = orderModelAdo.GetModel("id=?id", "",
                                new MySqlParameter("?id", requestBody.oid)
                                );
                            if (orderModel != null)
                            {
                                orderModel.ostatus = 2;
                                orderModel.sendUid = 0;
                                orderModel.sendTime = 0;
                                orderModel.sendRemark = "供应商退回抢单【派单中】";
                                orderModel.sendAmount = 0;
                                if (orderModelAdo.Update(orderModel) >= 1)
                                {

                                    model.status = 3;
                                    model.remark = "被供应商退回的订单";
                                    modelAdo.Update(model);
                                    SystemResponse.Output(SystemResponse.TYPE_OK, out statusCode, out responseJson);
                                }
                                else
                                {
                                    SystemResponse.Output(SystemResponse.TYPE_NO_PERMISSON, out statusCode, out responseJson);
                                }
                            }
                            else
                            {
                                SystemResponse.Output(SystemResponse.TYPE_ERROR, out statusCode, out responseJson);
                            }
                        }
                        #endregion
                        #region 其他操作
                        else
                        {
                            SystemResponse.Output(SystemResponse.TYPE_NO_PERMISSON, out statusCode, out responseJson);
                        }
                        #endregion
                    }
                    else
                    {
                        SystemResponse.Output(SystemResponse.TYPE_NO_PERMISSON, out statusCode, out responseJson);
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
            /// <summary>
            /// 处理动作：1:xx,2:抢单,3:退回
            /// </summary>
            public int action { set; get; }
            public string oid { set; get; }
            public decimal amount { set; get; }
        }

        /// <summary>
        /// 返回参数类
        /// </summary>
        public class ResponseBody
        {
        }
    }
}