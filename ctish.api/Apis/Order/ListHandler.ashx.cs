using ctish.api.Apis.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ctish.api.Apis.Order
{
    /// <summary>
    /// 获取订单列表 的摘要说明
    /// 1.客服创建等待派单【新订单】
    /// 2.客服派单等待供应商抢单【派单中】
    /// 3.供应商抢单并输入价格 【待处理订单】
    /// 4.客服已确认【正在执行订单】
    /// 5.订单已关闭【已完成订单】
    /// </summary>
    public class ListHandler : IHttpHandler
    {
        private int status = 1;
        private string msg = "";
        private Object output = "";
        public void ProcessRequest(HttpContext context)
        {
        //    if (context.Request["otype"] == null)
        //    {
        //        status = 0;
        //        msg = "订单类型参数不能为空！";
        //        goto returnJson;
        //    }
        //    int otype = Int32.Parse(context.Request["otype"]);


        //    int ostatus = 0;

        //    libs.BLL.ct_order bllOrder = new libs.BLL.ct_order();

        //    StringBuilder sbWhere = new StringBuilder();
        //    sbWhere.Append("otype =" + otype);
        //    sbWhere.Append(" and ostatus=" + ostatus + " ");

        //    int RecordCount = bllOrder.GetRecordCount(sbWhere.ToString());
        //    List<libs.Model.ct_order> modelOrders = bllOrder.GetModelList(sbWhere.ToString());
        //    if (RecordCount == 0)
        //    {
        //        status = 0;
        //        msg = "未能找到数据集合！";
        //        goto returnJson;
        //    }
        //    else
        //    {
        //        output = from model in modelOrders
        //                 select new
        //        {
        //            id = model.id,
        //            no = model.no,
        //            title = model.title,
        //            address1 = model.address1,
        //            address2 = model.address2,
        //            price = model.amount.ToString("f2"),
        //            time1 = string.Format("{0:d}", StringHelper.GetNomalTime(model.time1)),
        //            time2 = string.Format("{0:d}", StringHelper.GetNomalTime(model.time2))
        //        };
        //    }

        //returnJson:
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