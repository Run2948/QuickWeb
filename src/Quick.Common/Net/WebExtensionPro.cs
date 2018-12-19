﻿/* ==============================================================================
* 命名空间：Quick.Common.Net
* 类 名 称：WebExtensionPro
* 创 建 者：Qing
* 创建时间：2018-12-18 14:02:12
* CLR 版本：4.0.30319.42000
* 保存的文件名：WebExtensionPro
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

using Masuit.Tools;
using Masuit.Tools.Logging;
using Masuit.Tools.Models;
using Masuit.Tools.Security;
using Newtonsoft.Json;
using Quick.Common.NoSQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Quick.Common.Net
{
    public static class WebExtensionPro
    {
        #region 获取线程内唯一的EF上下文对象

        /// <summary>
        /// 获取线程内唯一的EF上下文对象
        /// </summary>
        /// <typeparam name="T">EF上下文容器对象</typeparam>
        /// <returns>EF上下文容器对象</returns>
        public static T GetDbContext<T>() where T : new()
        {
            T db;
            if (CallContext.GetData("db") == null) //由于CallContext比HttpContext先存在，所以首选CallContext为线程内唯一对象
            {
                db = new T();
                CallContext.SetData("db", db);
            }
            db = (T)CallContext.GetData("db");
            return db;
        }

        #endregion

        #region 写入Session

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set(this HttpSessionStateBase session, string key, dynamic value) => session[key] = value;

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间(分钟)</param>
        public static void Set(this HttpSessionStateBase session, string key, dynamic value, int timeout)
        {
            session.Timeout = timeout;
            session[key] = value;
        }

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set(this HttpSessionState session, string key, dynamic value) => session[key] = value;

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间(分钟)</param>
        public static void Set(this HttpSessionState session, string key, dynamic value, int timeout)
        {
            session.Timeout = timeout;
            session[key] = value;
        }

        /// <summary>
        /// 将Session存到Redis，需要先在config中配置链接字符串，连接字符串在config配置文件中的ConnectionStrings节下配置，name固定为RedisHosts，值的格式：127.0.0.1:6379,allowadmin=true，若未正确配置，将按默认值“127.0.0.1:6379,allowadmin=true”进行操作，如：<br/>
        /// &lt;connectionStrings&gt;<br/>
        ///      &lt;add name = "RedisHosts" connectionString="127.0.0.1:6379,allowadmin=true"/&gt;<br/>
        /// &lt;/connectionStrings&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key">键</param>
        /// <param name="obj">需要存的对象</param>
        /// <param name="db">Redis数据库编号</param>
        /// <param name="expire">过期时间，默认20分钟</param>
        /// <returns></returns>
        public static void SetByRedis<T>(this HttpSessionState session, string key, T obj, int db = 1, int expire = 20)
        {
            if (HttpContext.Current is null)
            {
                throw new Exception("请确保此方法调用是在同步线程中执行！");
            }
            var sessionKey = HttpContext.Current.Request.Cookies["SessionID"]?.Value;
            if (string.IsNullOrEmpty(sessionKey))
            {
                sessionKey = Guid.NewGuid().ToString().AESEncrypt();
                HttpCookie cookie = new HttpCookie("SessionID", sessionKey);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            if (session != null)
            {
                session[key] = obj;
            }
            try
            {
                using (RedisHelper redisHelper = RedisHelper.GetInstance(db))
                {
                    redisHelper.SetHash("Session:" + sessionKey, key, obj, TimeSpan.FromMinutes(expire)); //存储数据到缓存服务器，这里将字符串"my value"缓存，key 是"test"
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 将Session存到Redis，需要先在config中配置链接字符串，连接字符串在config配置文件中的ConnectionStrings节下配置，name固定为RedisHosts，值的格式：127.0.0.1:6379,allowadmin=true，若未正确配置，将按默认值“127.0.0.1:6379,allowadmin=true”进行操作，如：<br/>
        /// &lt;connectionStrings&gt;<br/>
        ///      &lt;add name = "RedisHosts" connectionString="127.0.0.1:6379,allowadmin=true"/&gt;<br/>
        /// &lt;/connectionStrings&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key">键</param>
        /// <param name="obj">需要存的对象</param>
        /// <param name="db">Redis数据库编号</param>
        /// <param name="expire">过期时间，默认20分钟</param>
        /// <returns></returns> 
        public static void SetByRedis<T>(this HttpSessionStateBase session, string key, T obj, int db = 1, int expire = 20) where T : class
        {
            if (HttpContext.Current is null)
            {
                throw new Exception("请确保此方法调用是在同步线程中执行！");
            }
            var sessionKey = HttpContext.Current.Request.Cookies["SessionID"]?.Value;
            if (string.IsNullOrEmpty(sessionKey))
            {
                sessionKey = Guid.NewGuid().ToString().AESEncrypt();
                HttpCookie cookie = new HttpCookie("SessionID", sessionKey);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            if (session != null)
            {
                session[key] = obj;
            }

            try
            {
                using (RedisHelper redisHelper = RedisHelper.GetInstance(db))
                {
                    redisHelper.SetHash("Session:" + sessionKey, key, obj, TimeSpan.FromMinutes(expire)); //存储数据到缓存服务器，这里将字符串"my value"缓存，key 是"test"
                }
            }
            catch
            {
                // ignored
            }
        }

        #endregion

        #region 获取Session

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="session"></param>
        /// <param name="key">键</param>
        /// <returns>对象</returns>
        public static T Get<T>(this HttpSessionStateBase session, string key) => (T)session[key];

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="session"></param>
        /// <param name="key">键</param>
        /// <returns>对象</returns>
        public static T Get<T>(this HttpSessionState session, string key) => (T)session[key];

        /// <summary>
        /// 从Redis取Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="key">键</param>
        /// <param name="db">Redis数据库编号</param>
        /// <param name="expire">过期时间，默认20分钟</param>
        /// <returns></returns> 
        public static T GetByRedis<T>(this HttpSessionState _, string key, int db = 1, int expire = 20) where T : class
        {
            if (HttpContext.Current is null)
            {
                throw new Exception("请确保此方法调用是在同步线程中执行！");
            }

            var sessionKey = HttpContext.Current.Request.Cookies["SessionID"]?.Value;
            if (!string.IsNullOrEmpty(sessionKey))
            {
                T obj = default(T);
                if (_ != default(T))
                {
                    obj = _.Get<T>(key);
                }

                if (obj == default(T))
                {
                    try
                    {
                        sessionKey = "Session:" + sessionKey;
                        using (RedisHelper redisHelper = RedisHelper.GetInstance(db))
                        {
                            if (redisHelper.KeyExists(sessionKey) && redisHelper.HashExists(sessionKey, key))
                            {
                                redisHelper.Expire(sessionKey, TimeSpan.FromMinutes(expire));
                                return redisHelper.GetHash<T>(sessionKey, key);
                            }
                            return default(T);
                        }
                    }
                    catch
                    {
                        return default(T);
                    }
                }
                return obj;
            }
            return default(T);
        }

        /// <summary>
        /// 从Redis取Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="key">键</param>
        /// <param name="db">Redis数据库编号</param>
        /// <param name="expire">过期时间，默认20分钟</param>
        /// <returns></returns>
        public static T GetByRedis<T>(this HttpSessionStateBase _, string key, int db = 1, int expire = 20) where T : class
        {
            if (HttpContext.Current is null)
            {
                throw new Exception("请确保此方法调用是在同步线程中执行！");
            }

            var sessionKey = HttpContext.Current.Request.Cookies["SessionID"]?.Value;
            if (!string.IsNullOrEmpty(sessionKey))
            {
                T obj = default(T);
                if (_ != default(T))
                {
                    obj = _.Get<T>(key);
                }

                if (obj == null)
                {
                    try
                    {
                        sessionKey = "Session:" + sessionKey;
                        using (RedisHelper redisHelper = RedisHelper.GetInstance(db))
                        {
                            if (redisHelper.KeyExists(sessionKey) && redisHelper.HashExists(sessionKey, key))
                            {
                                redisHelper.Expire(sessionKey, TimeSpan.FromMinutes(expire));
                                return redisHelper.GetHash<T>(sessionKey, key);
                            }
                            return default(T);
                        }
                    }
                    catch
                    {
                        return default(T);
                    }
                }
                return obj;
            }
            return default(T);
        }

        /// <summary>
        /// 从Session移除对应键
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        public static void Remove(this HttpSessionStateBase session, string key) => session.Remove(key);

        /// <summary>
        /// 从Session移除对应键
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        public static void Remove(this HttpSessionState session, string key) => session.Remove(key);

        /// <summary>
        /// 从Redis移除对应键的Session
        /// </summary>
        /// <param name="_"></param>
        /// <param name="key"></param>
        /// <param name="db">Redis数据库编号</param>
        /// <returns></returns>
        public static void RemoveByRedis(this HttpSessionStateBase _, string key, int db = 1)
        {
            if (HttpContext.Current is null)
            {
                throw new Exception("请确保此方法调用是在同步线程中执行！");
            }
            var sessionKey = HttpContext.Current.Request.Cookies["SessionID"]?.Value;
            if (!string.IsNullOrEmpty(sessionKey))
            {
                if (_ != null)
                {
                    _[key] = null;
                }

                try
                {
                    sessionKey = "Session:" + sessionKey;
                    using (RedisHelper redisHelper = RedisHelper.GetInstance(db))
                    {
                        if (redisHelper.KeyExists(sessionKey) && redisHelper.HashExists(sessionKey, key))
                        {
                            redisHelper.DeleteHash(sessionKey, key);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogManager.Error(e);
                }
            }
        }

        /// <summary>
        /// 从Redis移除对应键的Session
        /// </summary>
        /// <param name="_"></param>
        /// <param name="key"></param>
        /// <param name="db">Redis数据库编号</param>
        /// <returns></returns>
        public static void RemoveByRedis(this HttpSessionState _, string key, int db = 1)
        {
            if (HttpContext.Current is null)
            {
                throw new Exception("请确保此方法调用是在同步线程中执行！");
            }
            var sessionKey = HttpContext.Current.Request.Cookies["SessionID"]?.Value;
            if (!string.IsNullOrEmpty(sessionKey))
            {
                if (_ != null)
                {
                    _[key] = null;
                }

                try
                {
                    sessionKey = "Session:" + sessionKey;
                    using (RedisHelper redisHelper = RedisHelper.GetInstance(db))
                    {
                        if (redisHelper.KeyExists(sessionKey) && redisHelper.HashExists(sessionKey, key))
                        {
                            redisHelper.DeleteHash(sessionKey, key);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogManager.Error(e);
                }
            }
        }

        /// <summary>
        /// Session个数
        /// </summary>
        /// <param name="session"></param>
        /// <param name="db">Redis数据库编号</param>
        /// <returns></returns>
        public static int SessionCount(this HttpSessionState session, int db = 1)
        {
            try
            {
                using (RedisHelper redisHelper = RedisHelper.GetInstance(db))
                {
                    return redisHelper.GetServer().Keys(1, "Session:*").Count();
                }
            }
            catch (Exception e)
            {
                LogManager.Error(e);
                return 0;
            }
        }
        /// <summary>
        /// Session个数
        /// </summary>
        /// <param name="session"></param>
        /// <param name="db">Redis数据库编号</param>
        /// <returns></returns>
        public static int SessionCount(this HttpSessionStateBase session, int db = 1)
        {
            try
            {
                using (RedisHelper redisHelper = RedisHelper.GetInstance(db))
                {
                    return redisHelper.GetServer().Keys(1, "Session:*").Count();
                }
            }
            catch (Exception e)
            {
                LogManager.Error(e);
                return 0;
            }
        }

        #endregion

        #region 获取客户端IP地址信息

        /// <summary>
        /// 根据IP地址获取详细地理信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static async Task<Tuple<string, List<string>>> GetIPAddressInfo(this string ip)
        {
            ip.MatchInetAddress(out var isIpAddress);
            if (isIpAddress)
            {
                var address = await GetPhysicsAddressInfo(ip);
                if (address.Status == 0)
                {
                    string detail = $"{address.AddressResult.FormattedAddress} {address.AddressResult.AddressComponent.Direction}{address.AddressResult.AddressComponent.Distance ?? "0"}米";
                    List<string> pois = address.AddressResult.Pois.Select(p => $"{p.AddressDetail}{p.Name} {p.Direction}{p.Distance ?? "0"}米").ToList();
                    return new Tuple<string, List<string>>(detail, pois);
                }
                return new Tuple<string, List<string>>("IP地址不正确", new List<string>());
            }
            return new Tuple<string, List<string>>($"{ip}不是一个合法的IP地址", new List<string>());
        }

        /// <summary>
        /// 根据IP地址获取详细地理信息对象
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static async Task<PhysicsAddress> GetPhysicsAddressInfo(this string ip)
        {
            ip.MatchInetAddress(out var isIpAddress);
            if (isIpAddress)
            {
                string ak = ConfigurationManager.AppSettings["BaiduAK"];
                if (string.IsNullOrEmpty(ak))
                {
                    throw new Exception("未配置BaiduAK，请先在您的应用程序web.config或者App.config中的AppSettings节点下添加BaiduAK配置节(注意大小写)");
                }
                using (HttpClient client = new HttpClient() { BaseAddress = new Uri("http://api.map.baidu.com") })
                {
                    client.DefaultRequestHeaders.Referrer = new Uri("http://lbsyun.baidu.com/jsdemo.htm");
                    var task = client.GetAsync($"/location/ip?ak={ak}&ip={ip}&coor=bd09ll").ContinueWith(async t =>
                    {
                        if (t.IsFaulted || t.IsCanceled)
                        {
                            return null;
                        }
                        var res = await t;
                        if (res.IsSuccessStatusCode)
                        {
                            var ipAddress = JsonConvert.DeserializeObject<BaiduIP>(await res.Content.ReadAsStringAsync());
                            if (ipAddress.Status == 0)
                            {
                                LatiLongitude point = ipAddress.AddressInfo.LatiLongitude;
                                string result = client.GetStringAsync($"/geocoder/v2/?location={point.Y},{point.X}&output=json&pois=1&radius=1000&latest_admin=1&coordtype=bd09ll&ak={ak}").Result;
                                PhysicsAddress address = JsonConvert.DeserializeObject<PhysicsAddress>(result);
                                if (address.Status == 0)
                                {
                                    return address;
                                }
                            }
                            else
                            {
                                using (var client2 = new HttpClient { BaseAddress = new Uri("http://ip.taobao.com") })
                                {
                                    return await await client2.GetAsync($"/service/getIpInfo.php?ip={ip}").ContinueWith(async tt =>
                                    {
                                        if (tt.IsFaulted || tt.IsCanceled)
                                        {
                                            return null;
                                        }
                                        var result = await tt;
                                        if (result.IsSuccessStatusCode)
                                        {
                                            TaobaoIP taobaoIp = JsonConvert.DeserializeObject<TaobaoIP>(await result.Content.ReadAsStringAsync());
                                            if (taobaoIp.Code == 0)
                                            {
                                                return new PhysicsAddress()
                                                {
                                                    Status = 0,
                                                    AddressResult = new AddressResult()
                                                    {
                                                        FormattedAddress = taobaoIp.IpData.Country + taobaoIp.IpData.Region + taobaoIp.IpData.City,
                                                        AddressComponent = new AddressComponent()
                                                        {
                                                            Province = taobaoIp.IpData.Region
                                                        },
                                                        Pois = new List<Pois>()
                                                    }
                                                };
                                            }
                                        }
                                        return null;
                                    });
                                }
                            }
                        }
                        return null;
                    });
                    return await await task;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据IP地址获取ISP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetISP(this string ip)
        {
            if (ip.MatchInetAddress())
            {
                using (var client = new HttpClient { BaseAddress = new Uri("http://ip.taobao.com") })
                {
                    var task = client.GetAsync($"/service/getIpInfo.php?ip={ip}").ContinueWith(async t =>
                    {
                        if (t.IsFaulted)
                        {
                            return $"未能找到{ip}的ISP信息";
                        }
                        var result = await t;
                        TaobaoIP taobaoIp = JsonConvert.DeserializeObject<TaobaoIP>(await result.Content.ReadAsStringAsync());
                        if (taobaoIp.Code == 0)
                        {
                            return taobaoIp.IpData.Isp;
                        }
                        return $"未能找到{ip}的ISP信息";
                    });
                    return task.Result.Result;
                }
            }
            return $"{ip}不是一个合法的IP";
        }

        /// <summary>
        /// 根据IP地址获取 省市信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetProvince(this string ip)
        {
            if (ip.MatchInetAddress())
            {
                using (var client = new HttpClient { BaseAddress = new Uri("http://ip.taobao.com") })
                {
                    var task = client.GetAsync($"/service/getIpInfo.php?ip={ip}").ContinueWith(async t =>
                    {
                        if (t.IsFaulted)
                        {
                            return null;
                        }
                        var result = await t;
                        TaobaoIP taobaoIp = JsonConvert.DeserializeObject<TaobaoIP>(await result.Content.ReadAsStringAsync());
                        if (taobaoIp.Code == 0)
                        {
                            return taobaoIp.IpData.Region;
                        }
                        return null;
                    });
                    return task.Result.Result;
                }
            }
            return null;
        }

        #endregion
    }
}
