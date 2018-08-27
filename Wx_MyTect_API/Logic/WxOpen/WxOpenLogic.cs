using CYQ.Data;
using CYQ.Data.Tool;
using Logic.Senparc;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.Containers;
using Senparc.Weixin.WxOpen.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taurus.Core;

namespace Logic.WxOpen
{
    public partial class WxOpenLogic : BaseLogic
    {

        public WxOpenLogic(IController controller)
            : base(controller)
        {


        }
    }

    public partial class WxOpenLogic
    {
        /// <summary>
        /// 根据code拉取openid
        /// </summary>
        public object WxOpenOAuthNew()
        {
            try
            {
                string code = Query<string>("code");
                var result = new WxHelper().JsCode2Json(code);
                if (result.errcode == ReturnCode.请求成功)
                {
                    var sessionBag = SessionContainer.UpdateSession(null, result.openid, result.session_key);
                    //Log.WriteLogToTxt("sessionBag：" + JsonHelper.ToJson(sessionBag));
                    //返回自定义信息标识；SessionKey属于敏感信息，不能进行传输 , sessionKey = sessionBag.SessionKey 
                    var responseData = new { success = true, sessionId = sessionBag.Key, sessionKey = sessionBag.SessionKey }; return responseData;
                }
                else
                {
                    var responseData = new { success = false, msg = result.errmsg }; return responseData;
                }

            }
            catch (Exception ex)
            {
                Log.WriteLogToTxt("异常信息：" + ex.ToString());
            }


            return null;

        }


        /// <summary>
        /// 效验信息并进行注册
        /// </summary>
        public void WxOpenCheck()
        {
            string sessionId = Query<string>("sessionId");
            string rawData = Query<string>("rawData");
            string signature = Query<string>("signature");
            string encryptedData = Query<string>("encryptedData");
            string iv = Query<string>("iv");

            var checkSuccess = EncryptHelper.CheckSignature(sessionId, rawData, signature);
            if (checkSuccess)
            {
                //如果不能通过sessionId做为标识，那么通过rawData信息中的union，openid判断并返回新标识/用户数据给客户端
                #region//判断并进行注册

                #endregion
                var sessionIdValue = SessionContainer.GetSession(sessionId);

                var decodedEntity = EncryptHelper.DecodeUserInfoBySessionId(sessionId, encryptedData, iv);
                //Write(JsonHelper.ToJson(decodedEntity), true);

                //暂时客户端使用虚假数据
                using (MAction m = new MAction("Eco_User"))
                {
                    m.Fill("UserId=101");
                    Write(JsonHelper.ToJson(m.Data), true);
                }
            }
            else
            {
                Write("签名错误", false);
            }

        }

    }

}
