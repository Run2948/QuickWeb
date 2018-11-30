/* ==============================================================================
* 命名空间：Quick.Common.UEditor
* 类 名 称：ConfigHandler
* 创 建 者：Qing
* 创建时间：2018-11-30 10:05:20
* CLR 版本：4.0.30319.42000
* 保存的文件名：ConfigHandler
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



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace $safeprojectname$.UEditor
{
    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public class ConfigHandler : Handler
    {
        public ConfigHandler(HttpContext context) : base(context)
        {
        }

        public override void Process()
        {
            WriteJson(Config.Items);
        }
    }
}
