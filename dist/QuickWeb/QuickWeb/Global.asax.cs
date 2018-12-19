using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Masuit.Tools.Logging;
using System.Web.Optimization;
using Z.EntityFramework.Extensions;

namespace QuickWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            LicenseManager.AddLicense("67;100-MASUIT", "809739091397182EC1ECEA8770EB4218");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // 使用之前请根据需要配置Bundles文件
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.Register();
            StartupConfig.Startup();
        }

        void Application_Error(object sender, EventArgs e)
        {
            HttpException exception = ((HttpApplication)sender).Context.Error as HttpException;
            int? errorCode = exception?.GetHttpCode() ?? 503;
            switch (errorCode)
            {
                case 404:
                    Response.Redirect("/PageNotFound");
                    break;
                case 503:
                    LogManager.Error(exception ?? ((HttpApplication)sender).Server.GetLastError());
                    Response.Redirect("/ServiceUnavailable");
                    break;
                default:
                    return;
            }
        }
    }
}