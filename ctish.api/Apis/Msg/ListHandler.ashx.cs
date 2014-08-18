using ctish.api.ADO;
using ctish.api.Api.Base;
using ctish.api.Apis.Common;
using ctish.api.Apis.Config;
using ctish.api.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctish.api.Apis.Msg
{
    /// <summary>
    /// 用户消息列表 的摘要说明
    /// </summary>
    public class ListHandler : BaseHandler
    {

        private RequestBody requestBody;
        private ResponseBody responseBody;
        public override void OnLoad(HttpContext context)
        {
            base.OnLoad(context);
            requestBody = new RequestBody();
            requestBody.accessToken = context.Request["accessToken"];
            requestBody.withId = Convert.ToInt32(context.Request["withId"]);
            requestBody.page = Convert.ToInt32(context.Request["page"]);
            requestBody.pageSize = Convert.ToInt32(context.Request["pageSize"]);

            if (requestBody.accessToken == null || requestBody.accessToken.Trim().Length == 0 || requestBody.withId == 0)
            {
                SystemResponse.Output(SystemResponse.TYPE_NULLPARAMETER, out statusCode, out responseJson);
            }
            else
            {
                //验证用户
                TokenHelper token = new TokenHelper();
                UserTokenModel userTokenModel = token.getUserToken(requestBody.accessToken);
                if (userTokenModel == null)
                {
                    SystemResponse.Output(SystemResponse.TYPE_EXPIRE, out statusCode, out responseJson);
                }
                else
                {
                    //获取消息列表
                    ModelAdo<MsgModel> modelAdo = new ModelAdo<MsgModel>();
                    int pagenumber = requestBody.page == 0 ? 1 : requestBody.page;
                    int totalCount = 1;
                    modelAdo.PageSize = requestBody.pageSize == 0 ? modelAdo.PageSize : requestBody.pageSize;

                    List<MsgModel> models = models = modelAdo.GetList(pagenumber,
                        "(ufrom=?ufrom AND uto=?uto) or (uto=?ufrom AND ufrom=?uto)", "createTime DESC ", out totalCount, "",
                            new MySqlParameter("?ufrom", userTokenModel.uid),
                            new MySqlParameter("?uto", requestBody.withId)
                            );
                    if (models.Count >= 1)
                    {
                        //构建返回对象
                        List<Msg> msgs = new List<Msg>();
                        models.Sort(new idComparer());
                        foreach (MsgModel model in models)
                        {
                            Msg msg = new Msg();
                            msg.id = model.id;
                            msg.content = model.content;
                            msg.createTime = string.Format("{0:yyyy/MM/dd HH:mm:ss}", StringHelper.GetNomalTime(model.createTime));
                            msg.ufrom = model.ufrom;
                            msg.uto = model.uto;
                            msg.type = model.type;
                            msg.status = model.status;
                            msgs.Add(msg);
                            //处理数据为已读
                            if (msg.status == 0)
                            {
                                model.status = 1;
                                modelAdo.Update(model);
                            }
                        }
                        responseBody = new ResponseBody
                        {
                            page = 1,
                            pageTotal = (totalCount + modelAdo.PageSize - 1) / modelAdo.PageSize,
                            total = totalCount,
                            msgs = msgs
                        };
                        responseJson = JsonConvert.SerializeObject(responseBody, Formatting.Indented);
                    }
                    else
                    {
                        SystemResponse.Output(SystemResponse.TYPE_NULL, out statusCode, out responseJson);
                    }
                }
            }
        }

        public class idComparer : IComparer<MsgModel>
        {
            //实现按ID升序排列
            public int Compare(MsgModel x, MsgModel y)
            {
                return (x.id.CompareTo(y.id)); 
            }
        }



        /// <summary>
        /// 请求参数类
        /// </summary>
        public class RequestBody
        {
            public string accessToken { set; get; }
            public int withId { set; get; }
            public int page { set; get; }
            public int pageSize { set; get; }
        }
        /// <summary>
        /// 返回参数类
        /// </summary>
        public class ResponseBody
        {
            public int page { set; get; }
            public int pageTotal { set; get; }
            public int total { set; get; }
            public List<Msg> msgs { set; get; }
        }
        public class Msg
        {
            public int id { set; get; }
            public int ufrom { set; get; }
            public int uto { set; get; }
            public string content { set; get; }
            public string createTime { set; get; }
            public int type { set; get; }
            public int status { set; get; }
        }
    }
}