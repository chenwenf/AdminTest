using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYQ.Data;
using CYQ.Data.Tool;
using Logic.Senparc;
using Logic.WxOpen;
using Taurus.Core;

namespace Controllers.WxOpen
{
    public partial class WxOpenApiController : Controller
    {
        WxOpenLogic wxOpenLogic;

        public WxOpenApiController()
        {

            wxOpenLogic = new WxOpenLogic(this);
        }

    }

    public partial class WxOpenApiController
    {

        public void WxOpenOAuthNew()
        {

            Write(wxOpenLogic.WxOpenOAuthNew());

        }

        public void WxOpenCheck()
        {

            wxOpenLogic.WxOpenCheck();

        }



        #region [微信在线支付]
        /// <summary>
        /// 微信支付[用于在线支付]
        /// </summary>
        [HttpPost]
        public void WxPaySubmit()
        {
            int orderId = Query<int>("OrderId");
            int payType = Query<int>("payType", 0);

            decimal total = 1.5m;//为了兼容vs2013不能 out decimal total
            string orderMarking = "c"+ orderId+DateTime.Now.ToString("yyyyMMdd");//为了兼容vs2013不能 out string orderMarking
            string notifyUrl = "/WeiXinApi/NotifyUrl";
            //string notifyUrl = "http://ecofarm.dadongnet.cn" + "/WeiXinapi/NotifyUrl";
            string userHostAddress = Context.Request.UserHostAddress;
            int totalint = (int)(total * 100);
            var payObj = WxHelper.TenPayByJsapi("", orderMarking, orderMarking, totalint, userHostAddress,
                notifyUrl, payType);
            string json = JsonHelper.ToJson(payObj);
            Write(json, true);


        }

        /// <summary>
        /// 支付回调地址[用于在线支付]
        /// </summary>
        public void NotifyUrl()
        {
            try
            {
                string OrderMarking;
                bool f = WxHelper.PayNotifyUrl(Context, out OrderMarking);
                if (!f) return;

                //支付成功业务处理
                if (true)
                {
                    Write("success");
                }

            }
            catch (Exception ex)
            {
                Log.WriteLogToTxt("异常支付回调" + ex.Message, LogType.Error);
            }
        }

        #endregion
    }


}
