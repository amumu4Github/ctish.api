using ctish.api.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctish.api.Model
{
    /// <summary>
    /// 消息信息表
    /// </summary>
    [Tablename(TableName = "ct_msg", PrimaryKey = "id", ViewName = "")]
    public class MsgModel
    {

        /// <summary>
        /// 消息编号
        /// </summary>
        [ColumnameAttribute(Name = "id")]
        public int id { set; get; }
        
        /// <summary>
        /// 消息来源用户ＩＤ
        /// </summary>
        [ColumnameAttribute(Name = "ufrom")]
        public int ufrom { set; get; }
        
        /// <summary>
        /// 消息发往用户ＩＤ
        /// </summary>
        [ColumnameAttribute(Name = "uto")]
        public int uto { set; get; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [ColumnameAttribute(Name = "content")]
        public string content { set; get; }
        
        /// <summary>
        /// 消息创建时间
        /// </summary>
        [ColumnameAttribute(Name = "createTime")]
        public int createTime { set; get; }
        
        /// <summary>
        /// 消息类型
        /// </summary>
        [ColumnameAttribute(Name = "type")]
        public int type { set; get; }
        
        /// <summary>
        /// 消息状态（0未读，1已读）
        /// </summary>
        [ColumnameAttribute(Name = "status")]
        public int status { set; get; }
    }
}