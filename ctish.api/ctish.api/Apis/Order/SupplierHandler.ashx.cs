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

namespace ctish.api.Apis.Order
{
    /// <summary>
    /// 获取订单对应供应商的派单列表 的摘要说明
    /// </summary>
    public class SupplierHandler : BaseHandler
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
                    ModelAdo<OrderSupplierModel> orderModel = new ModelAdo<OrderSupplierModel>();
                    List<OrderSupplierModel> models = null;
                    int pagenumber = requestBody.page == 0 ? 1 : requestBody.page;
                    int totalCount = 1;
                    orderModel.PageSize = requestBody.pageSize == 0 ? orderModel.PageSize : requestBody.pageSize;
                    if (requestBody.oid == 0)
                    {
                        models = orderModel.GetList(pagenumber, " cuid=?cuid ", "", out totalCount, "",
                            new MySqlParameter("?cuid", userTokenModel.uid)
                            );
                    }
                    else
                    {
                        models = orderModel.GetList(pagenumber, " cuid=?cuid AND oid=?oid", "", out totalCount, "",
                                new MySqlParameter("?cuid", Convert.ToInt32(userTokenModel.uid)),
                                 new MySqlParameter("?oid", requestBody.oid)
                         );

                    }
                    if (models.Count >= 1)
                    {
                        responseBody = new ResponseBody
                        {
                            page = 1,
                            pageTotal = (totalCount + orderModel.PageSize - 1) / orderModel.PageSize,
                            total = totalCount,
                            suppliers = models
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
        /// </summary>
        public class RequestBody
        {
            public string accessToken { set; get; }
            public int oid { set; get; }
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
            public List<OrderSupplierModel> suppliers { get; set; }
        }
    }
}