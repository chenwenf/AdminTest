using CYQ.Data;
using CYQ.Data.Tool;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using Senparc.Weixin.Threads;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp;
using Senparc.Weixin.WxOpen.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logic.Senparc
{
  public  class WxHelper : ApplicationException
    {
        #region //内部变量


        private static readonly string WxOpenId = AppConfig.GetApp("WxOpenAppId");
        private static readonly string WxOpenSecret = AppConfig.GetApp("WxOpenAppSecret");
        private static readonly string MchId = ""; //商户号
        private static readonly string MchIdkey = "";//商户号Key

        private static readonly string CertPath = AppDomain.CurrentDomain.BaseDirectory + AppConfig.GetApp("certPath"); // 证书位置
        private static readonly string CertPassword = AppConfig.GetApp("certPwd");//秘钥

        #endregion


        /// <summary>
        /// 通过code获取openid，session_key
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsCode2JsonResult JsCode2Json(string code)
        {
            var result = SnsApi.JsCode2Json(WxOpenId, WxOpenSecret, code);
            return result;
        }
        /// <summary>
        /// session_key 合法性校验
        /// https://mp.weixin.qq.com/debug/wxagame/dev/tutorial/http-signature.html
        /// </summary>
        /// <param name="openId">用户唯一标识符</param>
        /// <param name="sessionKey">用户登录态签名</param>
        /// <param name="buffer">托管数据，类型为字符串，长度不超过1000字节（官方文档没有提供说明，可留空）</param>
        /// <returns></returns>
        public object CheckSession(string openId, string sessionKey, string buffer = null)
        {
            var result = WxAppApi.CheckSession(WxOpenId, openId, sessionKey, buffer);
            return result.errmsg;
        }

        /// <summary>
        /// 解密UserInfo消息（通过SessionId获取）
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <exception cref="WxOpenException">当SessionId或SessionKey无效时抛出异常</exception>
        /// <returns></returns>
        public static object DecodeUserInfoBySessionId(string sessionId, string encryptedData, string iv)
        {
            var result = EncryptHelper.DecodeUserInfoBySessionId(sessionId, encryptedData, iv);
            return result;
        }


        #region [微信支付相关]
        /// <summary>
        /// 调起微信支付JSAPI
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="orderNo">订单号</param>
        /// <param name="body"></param>
        /// <param name="price">金额 单位分</param>
        /// <param name="hostAddress">请求地址ip</param>
        /// <param name="notifyUrl">回调地址</param>
        /// <param name="payType">默认0 公众号支付，1小程序支付</param>
        /// <returns></returns>
        public static object TenPayByJsapi(string openId, string orderNo,
            string body, int price, string hostAddress, string notifyUrl, int payType = 0)
        {
            string code = "";
            try
            {
                string spBillno = orderNo;
                int totalFee = price;
                string ip = hostAddress;
                string payAppid = WxOpenId, parSecret = WxOpenId;// 公众号配置
                string payMchId = "", payMchIdkey = "";
                if (payType != 0)
                {
                    //小程序配置
                    payMchId = "1482472582";
                    payMchIdkey = "679E465F44514F46A05F881997E6631A";
                    payAppid = WxOpenId;
                    parSecret = WxOpenSecret;

                }

                TenPayV3Info payInfo = new TenPayV3Info(payAppid, parSecret, payMchId, payMchIdkey, notifyUrl);
                Log.WriteLogToTxt("PAYV3Info" + JsonHelper.ToJson(payInfo), LogType.Debug);
                var timeStamp = TenPayV3Util.GetTimestamp();
                var nonceStr = TenPayV3Util.GetNoncestr();

                var xmlDataInfo = new TenPayV3UnifiedorderRequestData(payInfo.AppId, payInfo.MchId, body,
                    spBillno, totalFee, ip, payInfo.TenPayV3Notify, TenPayV3Type.JSAPI, openId, payInfo.Key, nonceStr);

                var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口

                Log.WriteLogToTxt("统一下单结果:" + JsonHelper.ToJson(result), LogType.Debug);

                //参数生成
                var package = string.Format("prepay_id={0}", result.prepay_id);
                var paySign = TenPayV3.GetJsPaySign(payInfo.AppId, timeStamp, nonceStr, package, payInfo.Key);

                #region 原支付返回参数，2018-06-23修改
                /*
                 h5唤起支付
                 */
                code = "{ appId: '" + payAppid
                   + "',timeStamp: '" + timeStamp
                   + "', nonceStr: '" + nonceStr
                   + "',package: '" + package
                   + "',paySign:'" + paySign
                   + "',signType:'" + "MD5"
                   + "'}";
                #endregion
                //WxJSAPIPayDTO payPara = new WxJSAPIPayDTO()
                //{
                //    appId = payAppid,
                //    timeStamp = timeStamp,
                //    nonceStr = nonceStr,
                //    package = package,
                //    paySign = paySign,
                //    signType = "MD5"
                //};

                return code;
            }
            catch (Exception ex)
            {
                WxWriteLogError(ex, "异常调起统一下单");
            }
            return null;
            //return code;
        }

        /// <summary>
        /// 统一下单回调
        /// </summary>
        /// <param name="context"></param>
        /// <param name="outTradeNo">返回的订单号</param>
        /// <returns></returns>
        public static bool PayNotifyUrl(HttpContext context, out string outTradeNo)
        {
            try
            {
                ResponseHandler resHandler = new ResponseHandler(context);
                Log.WriteLogToTxt("统一下单回调：" + resHandler.ParseXML(), LogType.Debug);
                string returnCode = resHandler.GetParameter("return_code");
                resHandler.GetParameter("return_msg");
                outTradeNo = "";
                resHandler.SetKey(MchIdkey);
                //验证请求是否从微信发过来（安全）
                if (resHandler.IsTenpaySign() && returnCode.ToUpper() == "SUCCESS")
                {
                    outTradeNo = resHandler.GetParameter("out_trade_no");
                    return true;
                    //直到这里，才能认为交易真正成功了，可以进行数据库操作，但是别忘了返回规定格式的消息！
                }
            }
            catch (Exception ex)
            {
                WxWriteLogError(ex);
            }
            outTradeNo = "";
            return false;

        }

        /// <summary>
        /// 企业付款
        /// </summary>
        /// <param name="outTradeNo">流水号</param>
        /// <param name="openId"></param>
        /// <param name="amount">金额 decimal（单位 元，最少一元起付）</param>
        /// <param name="desc">描述</param>
        /// <param name="ip">请求Ip</param>
        /// <returns></returns>
        public static string Transfers(string outTradeNo, string openId, decimal amount, string desc, string ip)
        {
            try
            {
                const string deviceInfo = "";
                string nonceStr = TenPayV3Util.GetNoncestr();
                var xmlDataInfo = new TenPayV3TransfersRequestData(
                    WxOpenId, //商户账号appid
                    MchId, //商户号
                    deviceInfo,//设备号 非必填
                    nonceStr, //随机字符串
                    outTradeNo, //partner_trade_no商户订单号(只能是字母或者数字，不能包含有符号)
                    openId,//
                    MchIdkey, //商户号key
                    "NO_CHECK",// 校验用户姓名选项 NO_CHECK：不校验真实姓名 
                    "", //收款用户姓名 
                    amount, //金额
                    desc, //企业付款描述信息
                    ip //调用接口的机器Ip地址
                    );

                string cert = CertPath;//证书绝对路径
                string certPwd = CertPassword;//证书密码
                var result = TenPayV3.Transfers(xmlDataInfo, cert, certPwd);
                Log.WriteLogToTxt("企业付款 result:" + JsonHelper.ToJson(result), LogType.Info);
                return result.result_code == "SUCCESS" ? "成功" : result.err_code_des;
            }
            catch (Exception ex)
            {
                WxWriteLogError(ex, "异常企业付款");
            }
            return "";

        }

        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="billNo">订单号</param>
        /// <param name="billFee">退款金额（单位：分）</param>
        /// <param name="tenPayV3Notif">回调地址</param>
        /// <returns></returns>
        public static string Refund(string billNo, int billFee, string tenPayV3Notif)
        {
            try
            {
                TenPayV3Info tenPayV3Info = new TenPayV3Info(WxOpenId, WxOpenSecret, MchId, MchIdkey, tenPayV3Notif);
                string nonceStr = TenPayV3Util.GetNoncestr();
                string outTradeNo = billNo;
                string outRefundNo = "OutRefunNo-" + DateTime.Now.Ticks;
                int totalFee = billFee;
                int refundFee = totalFee;
                string opUserId = tenPayV3Info.MchId;
                var dataInfo = new TenPayV3RefundRequestData(
                    tenPayV3Info.AppId,
                    tenPayV3Info.MchId,
                    tenPayV3Info.Key,
                    null, //描述
                    nonceStr,
                    null, //微信订单号	
                    outTradeNo,//商户订单号 二选一
                    outRefundNo,//商户退款单号
                    totalFee,//订单金额
                    refundFee,//退款金额
                    opUserId,
                    null//退款资金来源
                    );
                var cert = CertPath;//根据自己的证书位置修改
                var password = CertPassword;//默认为商户号，建议修改
                var result = TenPayV3.Refund(dataInfo, cert, password);
                Log.WriteLogToTxt("订单退款 result:" + JsonHelper.ToJson(result), LogType.Info);
                return result.result_code == "FAIL" ? result.err_code_des : "成功";
            }
            catch (Exception ex)
            {
                WxWriteLogError(ex, "异常退款");
            }
            return "";

        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static string OrderQuery(string orderId)
        {
            try
            {
                string nonceStr = TenPayV3Util.GetNoncestr();
                RequestHandler packageReqHandler = new RequestHandler(null);

                //设置package订单参数
                packageReqHandler.SetParameter("appid", WxOpenId);       //公众账号ID
                packageReqHandler.SetParameter("mch_id", MchId);          //商户号
                packageReqHandler.SetParameter("transaction_id", "");       //填入微信订单号 
                packageReqHandler.SetParameter("out_trade_no", orderId);         //填入商家订单号
                packageReqHandler.SetParameter("nonce_str", nonceStr);             //随机字符串
                string sign = packageReqHandler.CreateMd5Sign("key", MchIdkey);
                packageReqHandler.SetParameter("sign", sign);                       //签名

                string data = packageReqHandler.ParseXML();

                var result = TenPayV3.OrderQuery(data);

                return result;
            }
            catch (Exception ex)
            {
                WxWriteLogError(ex);
            }
            return null;
        }

        #endregion













        /// <summary>
        /// 激活微信缓存
        /// </summary>
        public void RegisterWeixinThreads()
        {
            try
            {
                ThreadUtility.Register();//如果不注册此线程，则AccessToken、JsTicket等都无法使用SDK自动储存和管理。
                //Register.RegisterWxOpenAccount(WxOpenId, WxOpenSecret);
            }
            catch (Exception ex)
            {
                WxWriteLogError(ex);
            }

        }

        private static void WxWriteLogError(Exception ex, string msg = "")
        {
            Log.WriteLogToTxt("【微信】" + ex.TargetSite.Name + "【异常信息Message】：" + ex.Message + "【自定义信息】：" + msg, LogType.Error);
        }











    }
}
