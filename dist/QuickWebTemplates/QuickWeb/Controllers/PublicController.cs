using Quick.Common;
using $safeprojectname$.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class PublicController : BusinessController
    {
        #region 平台通用图形验证码
        public ActionResult VerifyCode(int l = 4, int f = 14, int h = 26)
        {
            string code = DrawingSecurityCode.GenerateCheckCode(l);
            TempData["valid_code"] = code;  // 将验证码存储到 TempData 中
            System.Web.HttpContext.Current.CreateCheckCodeImage(code, fontsize: f, height: h);
            Response.ContentType = "image/jpeg";
            return File(Encoding.UTF8.GetBytes(code), Response.ContentType);
        }
        #endregion


    }
}