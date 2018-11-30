/* ==============================================================================
* 命名空间：Quick.Common.UEditor
* 类 名 称：Handler
* 创 建 者：Qing
* 创建时间：2018-11-30 10:06:02
* CLR 版本：4.0.30319.42000
* 保存的文件名：Handler
* 文件版本：V1.0.0.0
*
* 功能描述：N/A 
*
* 修改历史：
*
*
* ==============================================================================
*         CopyRight @ 班纳工作室 2018. All rights reserved
* ==============================================================================*/



using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace $safeprojectname$.UEditor
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public abstract class Handler
    {
        public Handler(HttpContext context)
        {
            this.Request = context.Request;
            this.Response = context.Response;
            this.Context = context;
            this.Server = context.Server;
        }

        public abstract void Process();

        protected void WriteJson(object response)
        {
            string jsonpCallback = Request["callback"],
                json = JsonConvert.SerializeObject(response);
            if (String.IsNullOrWhiteSpace(jsonpCallback))
            {
                Response.AddHeader("Content-Type", "text/plain");
                Response.Write(json);
            }
            else
            {
                Response.AddHeader("Content-Type", "application/javascript");
                Response.Write(String.Format("{0}({1});", jsonpCallback, json));
            }
            Response.End();
        }

        public HttpRequest Request { get; private set; }
        public HttpResponse Response { get; private set; }
        public HttpContext Context { get; private set; }
        public HttpServerUtility Server { get; private set; }
    }
}
