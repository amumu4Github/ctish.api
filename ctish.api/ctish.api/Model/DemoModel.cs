using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ctish.api.ADO;

namespace ctish.api.Model
{
    [Tablename(TableName = "ct_demo", PrimaryKey = "id",ViewName="ct_demo" )]
    public class DemoModel
    {
        public int id { set; get; }
        [ColumnameAttribute(DefaultValue=0,Name="name",canInsert=false,canUpdate=false)]
        public string name { set; get; }
    }
}