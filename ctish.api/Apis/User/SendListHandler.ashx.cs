using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ctish.api.ADO;
using ctish.api.Model;
using MySql.Data.MySqlClient;

namespace ctish.api.Apis.User
{
    /// <summary>
    /// SendListHandler 的摘要说明
    /// </summary>
    public class SendListHandler : IHttpHandler
    {

        private int status = 1;
        private string msg = "";
        private Object output = "";
        private Object pageData = "";
        private Object Data = "";
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["oid"] == null)
            {
                status = 0;
                msg = "订单编号参数不能为空！";
                goto returnJson;
            }

            string pageSize = context.Request["pageSize"];
            string oid = context.Request["oid"];


            ModelAdo<SupplierOrderModel> supplierSendModel = new ModelAdo<SupplierOrderModel>();
            if (pageSize != null)
            {
                supplierSendModel.PageSize = int.Parse(pageSize);
            }

            int RecordCount = supplierSendModel.GetRecordCount(" userType=0  ");
            List<SupplierOrderModel> list = supplierSendModel.GetList(" userType=0 AND (oid=?oid or status is NULL) ", "", "", new MySqlParameter("?oid", oid));


            //List<SupplierOrderModel> list = supplierSendModel.GetList("select * from tv_view_supplier_order  where userType=0 AND (oid=1 or status is NULL) ");

            if (list.Count == 0)
            {
                status = 0;
                msg = "未能找到数据集合！";
                goto returnJson;
            }
            else
            {
                pageData = new
                {
                    recordCount = RecordCount,
                    pageSize = supplierSendModel.PageSize
                };
                Data = from model in list
                       select new
                       {
                           uid = model.uid,
                           
                           status = model.status,
                           oid = model.oid
                       };
                output = new
                {
                    pageData = pageData,
                    Data = Data
                };
            }

        returnJson:
            Dictionary<string, object> returnJson = new Dictionary<string, object>
            {
                { "status", status },
                { "msg", msg },
                { "output", output }
            };
            string json = JsonConvert.SerializeObject(returnJson, Formatting.Indented);
            context.Response.ContentType = "application/json";
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}