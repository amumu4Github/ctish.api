using ctish.api.ADO;
using ctish.api.Apis.Common;
using ctish.api.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MySql.Data.MySqlClient;
using ctish.api.Apis.Config;
using ctish.api.Api.Base;

namespace ctish.api.Apis.Order
{
    /// <summary>
    /// 订单详情信息 的摘要说明
    /// </summary>
    public class DetailHandler : BaseHandler
    {
        private RequestBody requestBody;
        private ResponseBody responseBody;
        public override void OnLoad(HttpContext context)
        {
            base.OnLoad(context);
            requestBody = new RequestBody();
            requestBody.accessToken = context.Request["accessToken"];
            requestBody.oid = Convert.ToInt32(context.Request["oid"]);
            if (requestBody.accessToken == null || requestBody.accessToken.Trim().Length == 0 || requestBody.oid == 0)
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
                    ModelAdo<OrderModel> orderModel = new ModelAdo<OrderModel>();
                    OrderModel model = orderModel.GetModel("id=?id", "",
                        new MySqlParameter("?id", requestBody.oid)
                        );
                    if (model != null)
                    {

                        responseBody = new ResponseBody();
                        responseBody.uid = model.uid;
                        responseBody.title = model.title;
                        responseBody.subtitle = model.subtitle;
                        responseBody.createDate = string.Format("{0:d}", StringHelper.GetNomalTime(model.createDate));
                        responseBody.status = model.ostatus;
                        responseBody.no = model.no;
                        responseBody.price = model.amount.ToString("f2");
                        responseBody.contact = model.contact;
                        responseBody.contactAddress = model.contactAdress;
                        responseBody.contactOPhone = model.contactOPhone;
                        responseBody.contactPhone = model.contactPhone;
                        responseBody.payment = model.payment;
                        responseBody.type = model.otid;
                        responseBody.remark = model.remark;
                        Ext from = new Ext();
                        from.uid = model.uid;
                        from.city = model.address1;
                        from.date = string.Format("{0:d}", StringHelper.GetNomalTime(model.time1));
                        responseBody.from = from;

                        Ext to = new Ext();
                        to.uid = model.uid;
                        to.city = model.address2;
                        to.date = string.Format("{0:d}", StringHelper.GetNomalTime(model.time2));
                        responseBody.to = to;

                        if (model.ostatus == 3)
                        {
                            Send send = new Send();
                            send.date = string.Format("{0:d}", StringHelper.GetNomalTime(model.sendTime));
                            send.suid = model.sendUid;
                            send.price = model.sendAmount.ToString("f2");
                            send.remark = model.sendRemark;
                            responseBody.send = send;
                        }
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
            public int oid { set; get; }
        }

        /// <summary>
        /// 返回参数类
        /// </summary>
        public class ResponseBody
        {
            public int uid { set; get; }
            public string title { set; get; }
            public string subtitle { set; get; }
            public string createDate { set; get; }
            public int status { set; get; }
            public string no { set; get; }
            public string price { set; get; }
            public int type { set; get; }
            public string contact { set; get; }
            public string contactAddress { set; get; }
            public string contactPhone { set; get; }
            public string contactOPhone { set; get; }
            public string payment { set; get; }
            public string remark { set; get; }

            public Ext from { set; get; }
            public Ext to { set; get; }
            public Send send { set; get; }
        }

        public class Ext
        {
            public int uid { set; get; }
            public string city { set; get; }
            public string date { set; get; }
        }

        public class Send
        {
            public int suid { set; get; }
            public string remark{set;get;}
            public string price { set; get; }
            public string date { set; get; }
        }

    }
}