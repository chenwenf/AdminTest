using Taurus.Core;
using Model;
using CYQ.Data;
using CommonHelp;
using System.Web;
using System;
using System.Web.Caching;

namespace Controllers.WebApi
{
    /// <summary>
    /// WebApi使用
    /// </summary>
    public class BaseApiController : Controller
    {
        public BaseApiController()
        {

        }


        #region 参数

        /// <summary>
        /// API域名
        /// </summary>
        public static readonly string _domianName = AppConfig.GetApp("DomianName");
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserModel userInfo { get; set; }
        /// <summary>
        /// 当前登录Id
        /// </summary>
        public long UserId
        {
            get { return userInfo.UserId; }
        }
        #endregion

        /// <summary>
        /// 检查Token
        /// </summary>
        /// <returns></returns>
        public override bool CheckToken()
        {
            //将Token验证合法性的代码写在这全局的地方，对所有的Controller都生效。
            string token = Query<string>("token");
            if (string.IsNullOrEmpty(token))
            {
                Write("not token", false);
                return false;
            }
            if (Context.Cache[token] != null)
            {
                var user = Context.Cache[token] as UserModel;
                if (user == null || user.ExpiryTime < DateTime.Now)
                {
                    Write("token已过期，请重新登录！", false);
                    return false;
                }
                else
                {
                    user.ExpiryTime = DateTime.Now.AddMinutes(30);
                    InsertCache(token, user);
                    userInfo = GetCache(token) as UserModel;
                }
            }
            return true;
        }

        /// <summary>
        /// 作为API使用时，CancelLoadHtml=true;
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public override bool BeforeInvoke(string methodName)
        {
            CancelLoadHtml = true;//使用API时取消读取html
            return true;
        }


        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="Values"></param>
        public void InsertCache(string key,object Values)
        {
            TimeSpan span= TimeSpan.FromMinutes(30);
            Context.Cache.Insert(key, Values, null, Cache.NoAbsoluteExpiration, span);
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            return Context.Cache[key];

        }

    }
}
