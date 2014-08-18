using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ctish.api.ADO;

namespace ctish.api.Model
{
    /// <summary>
    /// 派单信息中间表
    /// </summary>
    [Tablename(TableName = "ct_order_user", PrimaryKey = "id", ViewName = "")]
    public class OrderUserModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [ColumnameAttribute(Name="id")]
        public int id { set; get; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [ColumnameAttribute(Name = "oid")]
        public int oid { set; get; }
        /// <summary>
        /// 用户编号
        /// </summary>
        [ColumnameAttribute(Name = "uid")]
        public int uid { set; get; }

        /// <summary>
        /// 抢单总金额
        /// </summary>
        [ColumnameAttribute(Name = "amount")]
        public decimal amount { set; get; }


        /// <summary>
        /// 抢单时间戳
        /// </summary>
        [ColumnameAttribute(Name = "createTime")]
        public int createTime { set; get; }

        /// <summary>
        /// 状态码  （1:初始中,2:已抢单,3:退回；4:已完成；）
        /// </summary>
        [ColumnameAttribute(Name = "status")]
        public int status { set; get; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [ColumnameAttribute(Name = "remark")]
        public string remark { set; get; }

    }
}