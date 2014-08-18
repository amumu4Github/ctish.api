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
    /// 供应商订单列表 的摘要说明
    /// </summary>
    public class OrderHandler : BaseHandler
    {
        private RequestBody requestBody;
        private ResponseBody responseBody;
        public override void OnLoad(HttpContext context)
        {
            base.OnLoad(context);
            requestBody = new RequestBody();
            requestBody.accessToken = context.Request["accessToken"];
            requestBody.page = Convert.ToInt32(context.Request["page"]);
            requestBody.pageSize = Convert.ToInt32(context.Request["pageSize"]);
            requestBody.status = Convert.ToInt32(context.Request["status"]);
            if (requestBody.accessToken == null || requestBody.accessToken.Trim().Length == 0)
            {
                SystemResponse.Output(SystemResponse.TYPE_NULLPARAMETER, out statusCode, out responseJson);
            }
            //验证用户
            TokenHelper token = new TokenHelper();
            UserTokenModel userTokenModel = token.getUserToken(requestBody.accessToken);
            if (userTokenModel == null)
            {
                SystemResponse.Output(SystemResponse.TYPE_EXPIRE, out statusCode, out responseJson);
            }
            else
            {

                ModelAdo<SupplierOrderModel> orderModel = new ModelAdo<SupplierOrderModel>();
                List<SupplierOrderModel> models = null;
                int pagenumber = requestBody.page == 0 ? 1 : requestBody.page;
                int totalCount = 1;
                orderModel.PageSize = requestBody.pageSize == 0 ? orderModel.PageSize : requestBody.pageSize;

                if (requestBody.status == 4)
                {
                    ModelAdo<OrderModel> closeOrderModel = new ModelAdo<OrderModel>();
                    List<OrderModel> closeOrderModels = closeOrderModel.GetList(pagenumber, " sendUid=?sendUid AND ostatus=5", "", out totalCount, "",
                            new MySqlParameter("?sendUid", userTokenModel.uid)
                            );
                    if (closeOrderModels.Count >= 1)
                    {
                        //构建返回对象
                        List<Order> orders = new List<Order>();
                        foreach (OrderModel model in closeOrderModels)
                        {
                            Order order = new Order();
                            order.uid = model.uid.ToString();
                            order.title = model.title;
                            order.createDate = string.Format("{0:d}", StringHelper.GetNomalTime(model.createDate));
                            order.status = model.ostatus;
                            order.price = model.amount.ToString("f2");
                            order.oid = model.id.ToString();
                            order.type = model.otid;
                            Ext from = new Ext();
                            from.uid = model.uid.ToString();
                            from.city = model.address1;
                            from.date = string.Format("{0:d}", StringHelper.GetNomalTime(model.time1));
                            order.from = from;

                            Ext to = new Ext();
                            to.uid = model.uid.ToString();
                            to.city = model.address2;
                            to.date = string.Format("{0:d}", StringHelper.GetNomalTime(model.time2));
                            order.to = to;

                            orders.Add(order);
                        }
                        responseBody = new ResponseBody
                        {
                            page = 1,
                            pageTotal = (totalCount + orderModel.PageSize - 1) / orderModel.PageSize,
                            total = totalCount,
                            orders = orders
                        };
                        responseJson = JsonConvert.SerializeObject(responseBody, Formatting.Indented);
                    }
                    else
                    {
                        SystemResponse.Output(SystemResponse.TYPE_NULL, out statusCode, out responseJson);
                    }
                }
                else
                {
                    if (requestBody.status == 0)
                    {
                        models = orderModel.GetList(pagenumber, " uid=?uid ", "", out totalCount, "",
                            new MySqlParameter("?uid", userTokenModel.uid)
                            );
                    }
                    else
                    {
                        models = orderModel.GetList(pagenumber, " uid=?uid AND status=?status", "", out totalCount, "",
                                new MySqlParameter("?uid", Convert.ToInt32(userTokenModel.uid)),
                                 new MySqlParameter("?status", requestBody.status)
                         );
                    }

                    if (models.Count >= 1)
                    {
                        //构建返回对象
                        List<Order> orders = new List<Order>();
                        foreach (SupplierOrderModel model in models)
                        {
                            Order order = new Order();
                            order.uid = model.uid.ToString();
                            order.oid = model.oid.ToString();
                            order.title = model.title;
                            order.createDate = string.Format("{0:d}", StringHelper.GetNomalTime(model.createDate));
                            order.status = model.ostatus;
                            order.price = model.amount.ToString("f2");
                            order.type = model.otid;
                            Ext from = new Ext();
                            from.uid = model.uid.ToString();
                            from.city = model.address1;
                            from.date = string.Format("{0:d}", StringHelper.GetNomalTime(model.time1));
                            order.from = from;

                            Ext to = new Ext();
                            to.uid = model.uid.ToString();
                            to.city = model.address2;
                            to.date = string.Format("{0:d}", StringHelper.GetNomalTime(model.time2));
                            order.to = to;

                            orders.Add(order);
                        }
                        responseBody = new ResponseBody
                        {
                            page = 1,
                            pageTotal = (totalCount + orderModel.PageSize - 1) / orderModel.PageSize,
                            total = totalCount,
                            orders = orders
                        };
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
        /// ostatus:***********************
        /// 1.客服创建等待派单【新订单】
        /// 2.客服派单等待供应商抢单【派单中】
        /// 3.供应商抢单并输入价格 【待处理订单】
        /// 4.客服已确认【正在执行订单】
        /// 5.订单已关闭【已完成订单】
        /// status:***********************
        /// 1:初始中
        /// 2:已抢单
        /// 3:退回
        /// </summary>
        public class RequestBody
        {
            public string accessToken { set; get; }
            public int status { set; get; }
            public int page { set; get; }
            public int pageSize { set; get; }
        }

        /// <summary>
        /// 返回参数类
        /// </summary>
        public class ResponseBody
        {
            public int page { set; get; }
            public int pageTotal { set; get; }
            public int total { set; get; }
            public List<Order> orders { set; get; }
        }
        public class Order
        {
            public string uid { set; get; }
            public string oid { set; get; }
            public string title { set; get; }
            public string createDate { set; get; }
            public int status { set; get; }
            public string price { set; get; }
            public int type { set; get; }
            public Ext from { set; get; }
            public Ext to { set; get; }
        }
        public class Ext
        {
            public string uid { set; get; }
            public string city { set; get; }
            public string date { set; get; }
        }
    }
}