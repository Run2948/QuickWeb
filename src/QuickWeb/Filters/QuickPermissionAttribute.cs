using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace QuickWeb.Filters
{
    public class QuickPermissionAttribute : AuthorizeAttribute
    {
        /*
        protected string ControllerName = string.Empty;

        protected string ActionName = string.Empty;

        protected bool IsAjax = false;

        protected bool IsDebug = ConfigurationManager.AppSettings["IsDebug"] != null && ConfigurationManager.AppSettings["IsDebug"].ToBoolean();

        #region Session相关

        protected bool IsAdminLogin()
        {
            //return HttpContext.Current.Session.GetByRedis<AdminOutputDto>(RedisKeys.SHOP_DRUGS_ADMIN_SESSION, dbNum: 0) != null;
            return HttpContext.Current.Session.Get<AdminOutputDto>(RedisKeys.SHOP_DRUGS_ADMIN_SESSION) != null;
        }

        protected AdminOutputDto GetAdminSession()
        {
            //return HttpContext.Current.Session.GetByRedis<AdminOutputDto>(RedisKeys.SHOP_DRUGS_ADMIN_SESSION, dbNum: 0);
            return HttpContext.Current.Session.Get<AdminOutputDto>(RedisKeys.SHOP_DRUGS_ADMIN_SESSION);
        }

        #endregion

        /// <summary>
        /// 授权对象
        /// </summary>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IsAjax = filterContext.HttpContext.Request.IsAjaxRequest();
            ControllerName = filterContext.RouteData.Values["controller"]?.ToString();
            ActionName = filterContext.RouteData.Values["action"]?.ToString();

            // 调试模式
            if (IsDebug)
                return;

            // 放过 登录页面
            if (ControllerName.IsEqualString(QuickKeys.SHOP_DRUGS_ADMIN_LOGIN))
                return;

            // 拦截 未登录用户
            if (!IsAdminLogin())
            {
                filterContext.Result = new ContentResult() { Content = QuickKeys.AdminLoginUrlString() };
                return;
            }

            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// 授权逻辑
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // 默认 都不允许
            bool allow = false;

            // 获取 管理员账户信息
            AdminOutputDto admin = GetAdminSession();

            // 放过 超级管理所有请求权限 
            if (admin.is_super)
                return true;

            // 拦截 普通管理员的特定权限请求
            if (ControllerName.IsEqualString(QuickKeys.SHOP_DRUGS_SUPER_PERMISSION))
                return false;

            using (DbContext db = WebExtension.GetDbContext<DefaultDbContext>())
            {
                var permissions = db.Set<lbk_permissions>().AsNoTracking().FirstOrDefault(p => p.controller_name.IsEqualString(ControllerName) && p.action_name.IsEqualString(ActionName) && p.area_name.IsEqualString("Admin"));
                // 放过 不存在权限的请求
                if (permissions == null)
                    allow = true;
                // 拦截 未分配权限组的用户
                var admin_group = db.Set<lbk_admin_group>().AsNoTracking().FirstOrDefault(g => g.id == admin.admin_gid);
                if (admin_group == null)
                    allow = false;
                // 拦截 分配权限组但未授权请求权限的用户
                else
                    allow = CultureInfo.InvariantCulture.CompareInfo.IndexOf(admin_group.limits, $"{ControllerName}-{ActionName}") >= 0;
                return allow;
            }
        }

        /// <summary>
        /// 无权操作
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new RedirectResult(QuickKeys.AdminNoPermissionUrlString());
        }
        */
    }
}