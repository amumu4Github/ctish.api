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
    [Tablename(TableName="ct_user",PrimaryKey="id")]
    public class UserModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [ColumnameAttribute(Name="id")]
        public int id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [ColumnameAttribute(Name = "no")]
        public string no { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [ColumnameAttribute(Name = "rid")]
        public int rid { get; set; }

        /// <summary>
        /// 登陆用户名
        /// </summary>
        [ColumnameAttribute(Name = "name")]
        public string name { get; set; }


        /// <summary>
        /// 登陆密码
        /// </summary>
        [ColumnameAttribute(Name = "password")]
        public string password { get; set; }

        /// <summary>
        /// 登陆用户名
        /// </summary>
        [ColumnameAttribute(Name = "nickname")]
        public string nickname { get; set; }


        /// <summary>
        /// 电话号码
        /// </summary>
        [ColumnameAttribute(Name = "phone")]
        public string phone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [ColumnameAttribute(Name = "avatar")]
        public string avatar { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        [ColumnameAttribute(Name = "email")]
        public string email { get; set; }

        
        /// <summary>
        /// 插入时间戳
        /// </summary>
        [ColumnameAttribute(Name = "createTime")]
        public int ctime { get;set;}

    }
}