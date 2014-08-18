using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ctish.api.BPush
{
    public class IOSNotification
    {
        //public string title { get; set; } //通知标题，可以为空；如果为空则设为appid对应的应用名;
        //public BaiduDescription description { get; set; } //通知文本内容，不能为空;

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

        public IOSAPS aps { get; set; }

        public IOSNotification()
        {
        }

        public string getJsonString()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }

         

      
    }

}