using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MySql.Data.MySqlClient;
using ctish.api.ADO;
using ctish.api.Model;

namespace ctish.api.Apis.Supplier
{
    /// <summary>
    /// 获取供应商类型列表 的摘要说明
    /// </summary>
    public class TypeListHandler : IHttpHandler
    {
        private int status = 1;
        private string msg = "";
        private Object output = "";
        public void ProcessRequest(HttpContext context)
        {

            int status = 0;
            ModelAdo<OrderTypeModel> orderTypeModel = new ModelAdo<OrderTypeModel>();
            List<OrderTypeModel> list = orderTypeModel.GetList("status = ?status"," sort desc","", new MySqlParameter("?status", status));


            if (list.Count == 0)
            {
                status = 0;
                msg = "未能找到数据集合！";
                goto returnJson;
            }
            else
            {
                output = from model in list
                         select new
                {
                    id = model.id,
                    pid = model.pid,
                    name = model.name,
                    remark = model.remark,
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