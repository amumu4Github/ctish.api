using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ctish.api.ADO;

namespace ctish.api.Model
{
    /// <summary>
    /// 客服订单表
    /// </summary>
    [Tablename(TableName = "ct_order", PrimaryKey = "id", ViewName = "")]
    public class OrderModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [ColumnameAttribute(Name = "id")]
        public int id { set; get; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [ColumnameAttribute(Name = "no")]
        public string no { set; get; }

        /// <summary>
        /// 订单创建人编号
        /// </summary>
        [ColumnameAttribute(Name = "otid")]
        public int otid { set; get; }

        /// <summary>
        /// 订单创建人编号
        /// </summary>
        [ColumnameAttribute(Name = "uid")]
        public int uid { set; get; }

        /// <summary>
        /// 订单标题
        /// </summary>
        [ColumnameAttribute(Name = "title")]
        public string title { set; get; }
        /// <summary>
        /// 订单副标题
        /// </summary>
        [ColumnameAttribute(Name = "subtitle")]
        public string subtitle { set; get; }


        /// <summary>
        /// 订单状态
        /// </summary>
        [ColumnameAttribute(Name = "ostatus")]
        public int ostatus { set; get; }



        /// <summary>
        /// 订单联系人
        /// </summary>
        [ColumnameAttribute(Name = "contacts")]
        public string contact { set; get; }

        /// <summary>
        /// 订单联系人电话
        /// </summary>
        [ColumnameAttribute(Name = "contactsOphone")]
        public string contactOPhone { set; get; }
        /// <summary>
        /// 订单联系人电话
        /// </summary>
        [ColumnameAttribute(Name = "contactsPhone")]
        public string contactPhone { set; get; }

        /// <summary>
        /// 订单联系人地址
        /// </summary>
        [ColumnameAttribute(Name = "contactsAddress")]
        public string contactAdress { set; get; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [ColumnameAttribute(Name = "payment")]
        public string payment { set; get; }


        /// <summary>
        /// 计划时间
        /// </summary>
        [ColumnameAttribute(Name = "schedule")]
        public int schedule { set; get; }

        /// <summary>
        /// 计划时间
        /// </summary>
        [ColumnameAttribute(Name = "amount")]
        public decimal amount { set; get; }


        /// <summary>
        /// address1
        /// </summary>
        [ColumnameAttribute(Name = "address1")]
        public string address1 { set; get; }

        /// <summary>
        /// address2
        /// </summary>
        [ColumnameAttribute(Name = "address2")]
        public string address2 { set; get; }


        /// <summary>
        /// time1
        /// </summary>
        [ColumnameAttribute(Name = "time1")]
        public int time1 { set; get; }


        /// <summary>
        /// time2
        /// </summary>
        [ColumnameAttribute(Name = "time2")]
        public int time2 { set; get; }


        /// <summary>
        /// time1
        /// </summary>
        [ColumnameAttribute(Name = "createTime")]
        public int createDate { set; get; }

        /// <summary>
        /// 状态码
        /// </summary>
        [ColumnameAttribute(Name = "status")]
        public int status { set; get; }

        /// <summary>
        /// 派送供应商id
        /// </summary>
        [ColumnameAttribute(Name = "sendUid")]
        public int sendUid { set; get; }

        /// <summary>
        /// 派送时间
        /// </summary>
        [ColumnameAttribute(Name = "sendTime")]
        public int sendTime { set; get; }

        /// <summary>
        /// 派送金额
        /// </summary>
        [ColumnameAttribute(Name = "sendAmount")]
        public decimal sendAmount { set; get; }


        /// <summary>
        /// 派送备注
        /// </summary>
        [ColumnameAttribute(Name = "sendRemark")]
        public string sendRemark { set; get; }



        /// <summary>
        /// 备注
        /// </summary>
        [ColumnameAttribute(Name = "remark")]
        public string remark { set; get; }

    }
}