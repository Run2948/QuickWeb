﻿ 
/* ==============================================================================
* 命名空间：ShopDrugs.Controllers
* 类 名 称：TempController
* 创 建 者：Qing
* 创建时间：2018-05-28 15:54:52
* CLR 版本：4.0.30319.42000
* 保存的文件名：TempController
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

using Quick.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    /// <summary>
    /// 不使用构造函数注入的属性，必须是共有成员属性(public)，不能是私有或受保护的成员属性
    /// </summary>
    public class TempController : Controller
    {
		public ILoginRecordService LoginRecordService { get;set; }	

		public ISystemSettingService SystemSettingService { get;set; }	

		public IUserInfoService UserInfoService { get;set; }	

	}
}