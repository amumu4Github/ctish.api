using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ctish.api.ADO;

namespace ctish.api.Model
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    [Tablename(TableName = "ct_user_token", PrimaryKey = "id")]
    public class UserTokenModel
    {
        ///
        /// <summary>
        /// 编号
        /// </summary>
        [ColumnameAttribute(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// 用户ID 
        /// </summary>
        [ColumnameAttribute(Name = "uid")]
        public string uid { get; set; }

        /// <summary>
        /// 角色ID 
        /// </summary>
        [ColumnameAttribute(Name = "rid")]
        public int rid { get; set; }


        /// <summary>
        /// Token信息
        /// </summary>
        [ColumnameAttribute(Name = "accessToken")]
        public string accessToken { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        [ColumnameAttribute(Name = "bpuserId")]
        public string bpuserId { get; set; }


        /// <summary>
        /// ???
        /// </summary>
        [ColumnameAttribute(Name = "channelId")]
        public string channelId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [ColumnameAttribute(Name = "deviceToken")]
        public string deviceToken { get; set; }

        [ColumnameAttribute(Name = "deviceType")]
        public int deviceType { get; set; }


        /// <summary>
        /// 最后登陆时间
        /// </summary>
        [ColumnameAttribute(Name = "loginTime")]
        public int loginTime { get; set; }


        /// <summary>
        /// 最后登陆时间
        /// </summary>
        [ColumnameAttribute(Name = "isLogin")]
        public int isLogin { get; set; }


    }
}