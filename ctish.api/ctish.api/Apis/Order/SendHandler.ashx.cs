using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MySql.Data.MySqlClient;
using ctish.api.ADO;
using ctish.api.Model;
using Newtonsoft.Json.Linq;

namespace ctish.api.Apis.Order
{
    /// <summary>
    /// 客户派单信息 的摘要说明
    /// [1：供应商组派发；2：供应商派发]
    /// </summary>
    public class SendHandler : IHttpHandler
    {

        private int status = 1;
        private string msg = "";
        private Object output = "";
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["oid"] == null)
            {
                status = 0;
                msg = "订单编号参数不能为空！";
                goto returnJson;
            }
            if (context.Request["type"] == null)
            {
                status = 0;
                msg = "派单类型参数不能为空！";
                goto returnJson;
            }
            if (context.Request["stid"] == null)
            {
                status = 0;
                msg = "供应商组编号参数不能为空！";
                goto returnJson;
            }
            int oid = Int32.Parse(context.Request["oid"]);
            int type = Int32.Parse(context.Request["type"]);
            string stid = context.Request["stid"];


            ModelAdo<OrderSTypeModel> orderSTypeModel = new ModelAdo<OrderSTypeModel>();
            if (type == 1)
            {
                int RecordCount = orderSTypeModel.GetRecordCount(" oid = " + oid);
                if (RecordCount >= 1)
                {
                    //删除原有中间表数据
                    int tempSize = orderSTypeModel.ExecuteSql("DELETE FROM ct_order_stype WHERE oid = ?oid",
                          new MySqlParameter("?oid", oid));
                    if (tempSize == 0)
                    {
                        //删除成功，插入数据

                    }
                    else
                    {
                        //未能正确删除数据
                        status = 0;
                        msg = "数据处理失败集合！";
                        goto returnJson;
                    }
                }
                //直接插入数据
                JArray jsonStid = JArray.Parse(stid);
                if (jsonStid.Count == 0) {

                    //供应商组不能为空
                    status = 0;
                    msg = "供应商组不能为空！";
                    goto returnJson;
                }
                string insertStr = "INSERT INTO ct_order_stype(oid,stid,status) VALUES";
                foreach (int item in jsonStid)
                {
                    insertStr += "(" + oid + "," + item + ",1),";
                }
                insertStr = insertStr.Substring(0, insertStr.Length - 1);
                output = insertStr;

            }




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