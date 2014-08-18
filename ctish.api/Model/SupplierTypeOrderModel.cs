using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ctish.api.ADO;

namespace ctish.api.Model
{
    /// <summary>
    /// 供应商类型-订单信息列表
    /// </summary>
    [Tablename(TableName = "ct_supplier_type", PrimaryKey = "id", ViewName = "tv_view_supplier_type_order")]
    public class SupplierTypeOrderModel
    {
        /// <summary>
        /// <summary>
        /// 编号
        /// </summary>
        [ColumnameAttribute(Name = "id")]
        public int id { set; get; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [ColumnameAttribute(Name = "name")]
        public string name { set; get; }

        /// <summary>
        /// 父编号
        /// </summary>
        [ColumnameAttribute(Name = "pid")]
        public int pid { set; get; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [ColumnameAttribute(Name = "remark")]
        public string remark { set; get; }


        /// <summary>
        /// 是否已派单
        /// </summary>
        [ColumnameAttribute(Name = "status")]
        public int status { set; get; }


        /// <summary>
        /// 订单ID
        /// </summary>
        [ColumnameAttribute(Name = "oid")]
        public int oid { set; get; }
    }
}