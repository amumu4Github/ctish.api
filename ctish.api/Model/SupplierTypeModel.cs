using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ctish.api.ADO;

namespace ctish.api.Model
{
    /// <summary>
    /// 供应商类型表
    /// </summary>
    [Tablename(TableName = "ct_sipplier_type", PrimaryKey = "id",ViewName="" )]
    public class SupplierTypeModel
    {
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
        /// 状态状态(0使用，1关闭）
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