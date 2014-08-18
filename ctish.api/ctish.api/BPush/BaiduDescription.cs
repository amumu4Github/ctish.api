using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ctish.api.BPush
{

    /// <summary>
    /// 百度推送消息格式
    /// </summary>
    public class BaiduDescription
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 消息来源用户ＩＤ
        /// </summary>
        public int ufrom { set; get; }

        /// <summary>
        /// 消息发往用户ＩＤ
        /// </summary> 
        public int uto { set; get; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { set; get; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public string createTime { set; get; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int type { set; get; }

        /// <summary>
        /// 消息状态（0未读，1已读）
        /// </summary>
        public int status { set; get; }

        public string getJsonString()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }
    }
}