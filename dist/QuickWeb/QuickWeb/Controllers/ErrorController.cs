using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class ErrorController : Controller
    {
        [Route("AccessNoRight")]
        public ActionResult AccessNoRight()
        {
            //Response.StatusCode = 401;
            if (Request.HttpMethod.ToLower().Equals("get"))
            {
                return View();
            }
            return Json(new
            {
                StatusCode = 401,
                Success = false,
                Message = "没有权限！"
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("PageNotFound")]
        public ActionResult PageNotFound()
        {
            //Response.StatusCode = 404;
            if (Request.HttpMethod.ToLower().Equals("get"))
            {
                return View();
            }
            return Json(new
            {
                StatusCode = 404,
                Success = false,
                Message = "页面未找到！"
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("ServerError")]
        public ActionResult ServerError()
        {
            //Response.StatusCode = 500;
            if (Request.HttpMethod.ToLower().Equals("get"))
            {
                return View();
            }
            return Json(new
            {
                StatusCode = 500,
                Success = false,
                Message = "服务器内部错误！"
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("ServiceUnavailable")]
        public ActionResult ServiceUnavailable()
        {
            //Response.StatusCode = 503;
            if (Request.HttpMethod.ToLower().Equals("get"))
            {
                return View();
            }
            return Json(new
            {
                StatusCode = 503,
                Success = false,
                Message = "服务不可用！"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}