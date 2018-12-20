﻿using Quick.Common;
using Quick.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$
{
    public class StartupConfig
    {
        public static void Startup()
        {
            //移除aspx视图引擎
            ViewEngines.Engines.RemoveAt(0);
            AutoMapperConfig.Register();
            using (new DataContext()) { }
            System.Web.Http.GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }
    }
}