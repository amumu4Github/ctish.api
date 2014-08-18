using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ctish.api.BPush;
using ctish.api.Api.Base;
using ctish.api.Apis.Common;
using ctish.api.Model;
using ctish.api.Apis.Config;
using ctish.api.ADO;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Web.Script.Serialization;

namespace ctish.api.Apis.Msg
{
    /// <summary>
    /// 消息推送 的摘要说明
    /// </summary>
    public class PushHandler : BaseHandler
    {

        private RequestBody requestBody;
        private ResponseBody responseBody = null;
        public override void OnLoad(HttpContext context)
        {

            base.OnLoad(context);
            requestBody = new RequestBody();
            requestBody.accessToken = context.Request["accessToken"];
            requestBody.uTo = Convert.ToInt32(context.Request["to"]);
            requestBody.content = context.Request["content"];

            if (requestBody.content.Length == 0 || requestBody.accessToken.Trim().Length == 0 || requestBody.uTo == 0)
            {
                SystemResponse.Output(SystemResponse.TYPE_NULLPARAMETER, out statusCode, out responseJson);
            }
            else
            {
                //验证用户
                TokenHelper token = new TokenHelper();
                UserTokenModel fromModel = token.getUserToken(requestBody.accessToken);
                UserTokenModel toModel = token.getUserToken(requestBody.uTo);
                if (fromModel == null)
                {
                    SystemResponse.Output(SystemResponse.TYPE_EXPIRE, out statusCode, out responseJson);
                }
                else
                {
                    int msgstatus = 0;

                    #region 入库至本地
                    ModelAdo<MsgModel> modelAdo = new ModelAdo<MsgModel>();
                    MsgModel msg = new MsgModel();
                    msg.ufrom = Convert.ToInt32(fromModel.uid);
                    msg.uto = requestBody.uTo;
                    msg.content = requestBody.content;
                    msg.createTime = StringHelper.ConvertDateTimeInt(DateTime.Now);
                    msg.status = msgstatus;
                    if (modelAdo.Insert(msg) >= 1)
                    {
                        #region 百度推送
                        if (toModel != null && toModel.bpuserId.Length>=1 && toModel.channelId.Length>=1)
                        {
                            //获取插入本地数据
                            MsgModel msgPush = modelAdo.GetModel("ufrom=?ufrom AND uto=?uto AND createTime=?createTime AND status=0", "",
                                new MySqlParameter("?ufrom", msg.ufrom),
                                new MySqlParameter("?uto", msg.uto),
                                new MySqlParameter("?createTime", msg.createTime));
                            if (msgPush != null)
                            {
                                try
                                {
                                    //百度配置信息
                                    string secretKey = ConfigurationManager.AppSettings["baidu_secret_key"];
                                    string apiKey = ConfigurationManager.AppSettings["baidu_api_key"];
                                    uint depolyStatus = uint.Parse(ConfigurationManager.AppSettings["baidu_depoly_status"]);

                                    String messages = "";
                                    TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                                    uint unixTime = (uint)ts.TotalSeconds;
                                    string messageksy = "api";
                                    uint message_type = 1;
                                    BaiduPush Bpush = new BaiduPush("POST", secretKey);

                                   

                                    if (toModel.deviceType == 1)
                                    {
                                        message_type = 1;
                                        toModel.deviceType = 4;
                                        IOSNotification notifaction = new IOSNotification();
                                        notifaction.id = msgPush.id;
                                        notifaction.ufrom = msgPush.ufrom;
                                        notifaction.uto = msgPush.uto;
                                        notifaction.content = msgPush.content.Trim();
                                        notifaction.createTime = string.Format("{0:yyyy/MM/dd HH:mm:ss}", System.DateTime.Now);
                                        notifaction.type = msgPush.type;
                                        notifaction.status = 1;
                                        IOSAPS aps = new IOSAPS()
                                        {
                                            alert = "收到一条新消息",
                                        };
                                        notifaction.aps = aps;
                                        messages = notifaction.getJsonString();
                                    }
                                    else
                                    {
                                        message_type = 0;
                                        toModel.deviceType = 3;
                                        BaiduPushNotification notifaction = new BaiduPushNotification();
                                        notifaction.title = "";
                                        //构建custom_content信息
                                        BaiduDescription bdMsg = new BaiduDescription();
                                        bdMsg.id = msgPush.id;
                                        bdMsg.ufrom = msgPush.ufrom;
                                        bdMsg.uto = msgPush.uto;
                                        bdMsg.content = msgPush.content;
                                        bdMsg.createTime = string.Format("{0:yyyy/MM/dd HH:mm:ss}", System.DateTime.Now);
                                        bdMsg.type = msgPush.type;
                                        bdMsg.status = 1;
                                        notifaction.description = "收到一条新消息";
                                        notifaction.custom_content = bdMsg;
                                        messages = notifaction.getJsonString();

                                    }



                                    PushOptions pOpts = new PushOptions("push_msg", apiKey, toModel.bpuserId.ToString(),
                                        toModel.channelId.ToString(), Convert.ToUInt32(toModel.deviceType), messages, messageksy, unixTime);
                                    pOpts.message_type = message_type;
                                    pOpts.deploy_status = depolyStatus;
                                    pOpts.push_type = 1;
                                    string response = Bpush.PushMessage(pOpts);
                                    responseJson = response;
                                    msgstatus = 1;

                                    //处理数据为已读
                                    if (msg.status == 0)
                                    {
                                        msgPush.status = 1;
                                        modelAdo.Update(msgPush);
                                    }

                                    //SystemResponse.Output(SystemResponse.TYPE_OK, out statusCode, out responseJson);
                                    //responseJson = strBDMsg;
                                }
                                catch (Exception ex)
                                {
                                    responseJson = ex.ToString();
                                    SystemResponse.Output(SystemResponse.TYPE_ERROR, out statusCode, out responseJson);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            SystemResponse.Output(SystemResponse.TYPE_ERROR, out statusCode, out responseJson);
                        }
                    #endregion

                    }
                }
            }
        }

        /// <summary>
        /// 请求参数类
        /// </summary>
        public class RequestBody
        {
            public string accessToken { set; get; }
            public int uTo { set; get; }
            public string content { set; get; }
        }




        /// <summary>
        /// 返回参数类
        /// </summary>
        public class ResponseBody
        {
        }


        
       
    }
}