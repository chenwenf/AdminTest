using CommonHelp;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Taurus.Core;

namespace Controllers.WebApi
{
    
    public partial class WebApiController : BaseApiController
    {

    }

    
    public partial class WebApiController
    {
        [Token]
        [HttpPost]
        public void GetList()
        {

            Write("ok", true);
        }

    }

    public partial class WebApiController
    {
        //登录使用
        [HttpGet]
        public void UserLogin()
        {
            string userName =Query("userName", "admin"),
                   userPwd = Query("userPwd", "123456");

            UserModel user = new UserModel {
                UserName = userName,
                CreateTime = DateTime.Now,
                ExpiryTime=DateTime.Now.AddMinutes(30)
           
            };
            string signStr =CommonHelp.EncryptOrDecrypt.Encrypt(userName+ TimeStampHelp.GetTimeStamp(DateTime.Now));
            InsertCache(signStr, user);//添加缓存
            Write(signStr);
        }

    }


}
