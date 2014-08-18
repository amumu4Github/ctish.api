using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctish.api.Apis.Config
{
    public class SystemResponse
    {

        /// <summary>
        /// 操作处理成功
        /// </summary>
        public const int TYPE_OK = 200;
        /// <summary>
        /// 操作处理失败
        /// </summary>
        public const int TYPE_ERROR = 500;

        
        /// <summary>
        /// 无权限处理
        /// </summary>
        public const int TYPE_NO_PERMISSON = 408;

        /// <summary>
        /// 未能获取数据
        /// </summary>
        public const int TYPE_NULL = 1;
        /// <summary>
        /// Token数据已过期
        /// </summary>
        public const int TYPE_EXPIRE = 2;

        /// <summary>
        /// 请求参数异常
        /// </summary>
        public const int TYPE_NULLPARAMETER = 3;

        /// <summary>
        /// 用户数据已存在
        /// </summary>
        public const int TYPE_EXIST = 4;

        /// <summary>
        /// 注册失败
        /// </summary>
        public const int TYPE_REGISTER_ERROR = 5;


        /// <summary>
        /// 登陆失败
        /// </summary>
        public const int TYPE_LOGIN_ERROR = 6;


        /// <summary>
        /// 获取响应状态信息码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="StatusCode"></param>
        /// <param name="responseJson"></param>
        public static void Output(int type, out int StatusCode, out object responseJson)
        {
            switch (type)
            {
                case SystemResponse.TYPE_NULL:
                    StatusCode = 402;
                    responseJson = "未能获取数据";
                    break;
                case SystemResponse.TYPE_EXPIRE:
                    //401，nignx默认做了301跳转。跳转至Login
                    StatusCode = 405;
                    responseJson = "Token数据已过期";
                    break;
                case SystemResponse.TYPE_NULLPARAMETER:
                    StatusCode = 500;
                    responseJson = "请求参数异常";
                    break;
                case SystemResponse.TYPE_EXIST:
                    StatusCode = 402;
                    responseJson = "数据已经存在";
                    break;
                case SystemResponse.TYPE_REGISTER_ERROR:
                    StatusCode = 402;
                    responseJson = "注册失败";
                    break;
                case SystemResponse.TYPE_LOGIN_ERROR:
                    StatusCode = 404;
                    responseJson = "您输入的用户名密码不正确";
                    break;
                case SystemResponse.TYPE_ERROR:
                    StatusCode = 406;
                    responseJson = "数据处理失败";
                    break;
                case SystemResponse.TYPE_NO_PERMISSON:
                    StatusCode = 408;
                    responseJson = "您无权进行处理";

                    break;
                case SystemResponse.TYPE_OK:
                    StatusCode = 200;
                    responseJson = "数据处理成功";
                    break;
                default:
                    StatusCode = 200;
                    responseJson = "";
                    break;
            }
        }

    }
}