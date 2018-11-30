/* ==============================================================================
* 命名空间：Quick.Common.UEditor
* 类 名 称：Config
* 创 建 者：Qing
* 创建时间：2018-11-30 10:04:38
* CLR 版本：4.0.30319.42000
* 保存的文件名：Config
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Quick.Common.UEditor
{
    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public static class Config
    {
        private static bool noCache = true;
        private static JObject BuildItems()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath("/App_Data/config.json"));
            return JObject.Parse(json);
        }

        public static JObject Items
        {
            get
            {
                if (noCache || _Items == null)
                {
                    _Items = BuildItems();
                }
                return _Items;
            }
        }

        private static JObject _Items;

        public static T GetValue<T>(string key)
        {
            return Items[key].Value<T>();
        }

        public static String[] GetStringList(string key)
        {
            return Items[key].Select(x => x.Value<String>()).ToArray();
        }

        public static String GetString(string key)
        {
            return GetValue<String>(key);
        }

        public static int GetInt(string key)
        {
            return GetValue<int>(key);
        }
    }
}
