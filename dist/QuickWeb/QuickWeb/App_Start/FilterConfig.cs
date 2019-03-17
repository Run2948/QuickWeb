﻿using $safeprojectname$.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$
{
    public class FilterConfig
    {
        /// <summary>
        /// 注册全局的过滤器
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new JsonNetResultAttribute());
#if !DEBUG
            filters.Add(new ViewCompressAttribute());
            filters.Add(new QuickExceptionAttribute());
#endif
        }
    }
}