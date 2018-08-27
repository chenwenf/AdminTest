using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CommonHelp
{
    public class ComHelper
    {
        #region cookie

        /// <summary>
        /// 设置cookie，2小时
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cookieName"></param>
        public static void SavaCookie(object data, string cookieName)
        {
            int minutes = 60 * 2;
            SavaCookie(data, cookieName, minutes);
        }
        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cookieName"></param>
        /// <param name="minutes">分钟</param>
        public static void SavaCookie(object data, string cookieName, int minutes)
        {
            HttpCookie systemcookie = new HttpCookie(cookieName)
            {
                HttpOnly = false,
                Value = data.ToString(),
                Expires = DateTime.Now.AddMinutes(minutes)
            };
            HttpContext.Current.Response.Cookies.Add(systemcookie);
        }

        /// <summary>
        /// 清楚token cookie
        /// </summary>
        public static void DelCookie(string cookieName)
        {
            HttpCookie systemcookie = new HttpCookie(cookieName)
            {
                Value = "",
                Expires = DateTime.Now.AddYears(-10)
            };
            HttpContext.Current.Response.Cookies.Add(systemcookie);

        }
        #endregion
    }
}
