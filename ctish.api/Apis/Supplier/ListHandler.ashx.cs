using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ctish.api.Apis.Supplier
{
    /// <summary>
    /// 供应商派单列表信息 的摘要说明
    /// </summary>
    public class ListHandler : IHttpHandler
    {
        private int status = 1;
        private string msg = "";
        private Object output = "";
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["oid"] == null)
            {
                status = 0;
                msg = "编号参数不能为空！";
                goto returnJson;
            }
            string uid = context.Request["uid"];


            //libs.BLL.ct_order bllOrder = new libs.BLL.ct_order();

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append("xx = ");
            sbWhere.Append(" and xxx= ");


            if (0 == 0)
            {
                status = 0;
                msg = "未能找到数据集合！";
                goto returnJson;
            }
            else
            {

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