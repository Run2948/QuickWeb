using Quick.Common;
using Quick.Common.Net;
using Quick.Models.Dto;
using Quick.Models.Entity.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers.Common
{
    public class UserBaseController : BusinessController
    {
        #region 用户Session相关操作

        protected bool IsUserLogin()
        {
            return GetUserSession() != null;
        }

        protected UserInfoOutputDto GetUserSession()
        {
            return System.Web.HttpContext.Current.Session.Get<UserInfoOutputDto>(QuickKeys.USER_SESSION);
        }

        protected void SetUserSession(UserInfo member, int timeout = 20)
        {
            System.Web.HttpContext.Current.Session.Set(QuickKeys.USER_SESSION, member, timeout);
        }

        protected void SetUserLogOut()
        {
            System.Web.HttpContext.Current.Session.Remove(QuickKeys.USER_SESSION);
            System.Web.HttpContext.Current.Session.Abandon();
        }

        #endregion       
    }
}