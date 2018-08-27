using CYQ.Data;
using CYQ.Data.Cache;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taurus.Core;

namespace Controllers.SysWeb
{
    public partial class SysBaseController : Controller
    {
        public SysBaseController()
        {  //获取cookie
            var cookie = Context.Request.Cookies.Get("admin_UserToken");
            //判断cookie是否为空
            if (cookie != null)
            {
                var token = cookie.Value;
                IsCheckUser(token);
            }
            else
            {
                UrlToLogin();
            }

        }
        /// <summary>
        /// 当前用户
        /// </summary>
        public UserModel userModel { get; set; }
        public CacheManage cache = CacheManage.Instance;
        /// <summary>
        /// 检测用户
        /// </summary>
        /// <param name="token"></param>
        public void IsCheckUser(string token)
        {
            this.userModel = null;
            string userGuid = CommonHelp.EncryptOrDecrypt.Decrypt(token);
            if (string.IsNullOrEmpty(userGuid))
            {
                UrlToLogin();
                return;
            }
            if (userModel == null)
            {
                var cacheUserList = cache.Get<List<UserModel>>("cacheUser_Manager");
                #region 重新获取缓存数据
                if (cacheUserList == null || cacheUserList.Count <= 0)
                {
                    //重新获取缓存数据
                    using (MAction m = new MAction("C_UserManager"))
                    {
                        cacheUserList = m.Select().ToList<UserModel>();
                        cache.Set("cacheUser_Manager", cacheUserList, 120);
                    }
                }
                #endregion

                var userInfo = cacheUserList.FirstOrDefault(a => a.UserGuid == userGuid);
                if (userInfo != null)
                {
                    userModel = userInfo;
                    CommonHelp.ComHelper.SavaCookie(token, "admin_UserToken");
                }
                else
                {
                    UrlToLogin();
                    return;
                }

            }
            else
            {
                UrlToLogin();
                return;

            }


        }

        /// <summary>
        /// 登录窗口
        /// </summary>
        public void UrlToLogin()
        {
            Context.Response.Redirect("/SysLogin/login");
        }

        /// <summary>
        /// 重写默认页面；当前只输入一级路由时自动跳转到Default,再转发到index
        /// </summary>
        public override void Default()
        {

            Context.Response.Redirect("/Syshome/index");
        }

    }
}
