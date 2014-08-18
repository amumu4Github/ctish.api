using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ctish.api.ADO;

namespace ctish.api.Model
{
    /// <summary>
    /// 供应商组 - 订单表【中间表】
    /// </summary>
    [Tablename(TableName = "ct_order_stype", PrimaryKey = "id", ViewName = "")]
    public class OrderSTypeModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [ColumnameAttribute(Name = "id")]
        public int id { set; get; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [ColumnameAttribute(Name = "oid")]
        public int oid { set; get; }

        /// <summary>
        /// 父编号
        /// </summary>
        [ColumnameAttribute(Name = "stid")]
        public int stid { set; get; }

        /// <summary>
        /// 状态状态(0初始状态，1已派发）
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