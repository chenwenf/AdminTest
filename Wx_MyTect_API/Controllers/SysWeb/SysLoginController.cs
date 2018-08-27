using CommonHelp;
using CYQ.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taurus.Core;

namespace Controllers.SysWeb
{
    public partial class SysLoginController : Controller
    {

        public void Login()
        {


        }

    }


    public partial class SysLoginController : Controller
    {

        //登录
        public void LoginInApi()
        {
            string userName = Query<string>("UserName");
            string pwd = Query<string>("Password");
            pwd = EncryptOrDecrypt.Encrypt(pwd);

            using (MAction m = new MAction("C_UserManager"))
            { //验证成功
                if (m.Fill(" UserName='" + userName + "' and PassWord='" + pwd + "'"))
                {
                    string userGuid = m.Data.Get<string>("UserGuid");
                    string token = EncryptOrDecrypt.Encrypt(userGuid);
                    ComHelper.SavaCookie(token, "admin_UserToken");
                    //设置跳转页面
                    Write("/SysHome/index", true);
                    return;
                }
                else
                {
                    Write("用户名或密码错误...", false);
                }
            }


        }
        //注销
        public void LoginOutApi()
        {
            ComHelper.DelCookie("admin_UserToken");
            Context.Response.Redirect("/SysLogin/login");
            return;

        }

    }
}
