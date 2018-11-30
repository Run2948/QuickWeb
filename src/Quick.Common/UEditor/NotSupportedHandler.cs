/* ==============================================================================
* 命名空间：Quick.Common.UEditor
* 类 名 称：NotSupportedHandler
* 创 建 者：Qing
* 创建时间：2018-11-30 10:07:02
* CLR 版本：4.0.30319.42000
* 保存的文件名：NotSupportedHandler
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

namespace Quick.Common.UEditor
{
    /// <summary>
    /// NotSupportedHandler 的摘要说明
    /// </summary>
    public class NotSupportedHandler : Handler
    {
        public NotSupportedHandler(HttpContext context)
            : base(context)
        {
        }

        public override void Process()
        {
            WriteJson(new
            {
                state = "action 参数为空或者 action 不被支持。"
            });
        }
    }
}
