/* ==============================================================================
* 命名空间：Quick.Models 
* 类 名 称：DataInitializer
* 创 建 者：Qing
* 创建时间：2019/03/17 18:06:58
* CLR 版本：4.0.30319.42000
* 保存的文件名：DataInitializer
* 文件版本：V1.0.0.0
*
* 功能描述：N/A 
*
* 修改历史：
*
*
* ==============================================================================
*         CopyRight @ 班纳工作室 2019. All rights reserved
* ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Models
{
    public class DataInitializer : CreateDatabaseIfNotExists<Quick.Models.Application.DataContext>
    {
        public DataInitializer()
        {

        }

        protected override void Seed(Quick.Models.Application.DataContext context)
        {

        }
    }
}
