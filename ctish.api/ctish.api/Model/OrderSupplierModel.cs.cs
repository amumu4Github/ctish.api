using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ctish.api.ADO;

namespace ctish.api.Model
{
    [Tablename(TableName = "ct_order", PrimaryKey = "oid", ViewName = "tv_order_supplier")]
    public class OrderSupplierModel
    {
        [ColumnameAttribute(Name = "oid")]
        public int oid { set; get; }
        [ColumnameAttribute(Name = "ostatus")]
        public int ostatus { set; get; }
        [ColumnameAttribute(Name = "uid")]
        public int uid { set; get; }
        [ColumnameAttribute(Name = "cuid")]
        public int cuid { set; get; }
        [ColumnameAttribute(Name = "name")]
        public string name { set; get; }
        [ColumnameAttribute(Name = "status")]
        public int status { set; get; }

    }
}