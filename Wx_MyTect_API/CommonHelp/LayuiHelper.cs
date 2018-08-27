using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHelp
{
    /// <summary>
    /// layui帮助类
    /// </summary>
    public class LayuiHelper
    {
        /// <summary>
        /// 统一返回layui table Json数据格式
        /// </summary>
        /// <param name="jsonDta"></param>
        /// <param name="total">总记录数</param>
        /// <param name="currtotal">当前数据集行数</param>
        /// <returns></returns>
        public static string GetResponseJson(string jsonDta, int total, int currtotal)
        {
            string data = string.Empty;
            if (currtotal == 1)
            {
                //data = "{\"code\":0,\"msg\":\"\",\"count\":" + total + ",\"data\":[" + jsonDta + "]}";//cyq.data版本ToJson当记录一行时没有转成[]数组而是对象
                data = "{\"code\":0,\"msg\":\"\",\"count\":" + total + ",\"data\":[" + jsonDta + "]}";
            }
            else if (currtotal > 1)
            {
                data = "{\"code\":0,\"msg\":\"\",\"count\":" + total + ",\"data\":" + jsonDta + "}";
            }
            else
            {
                data = "{\"code\":-1,\"msg\":\"未查询到数据\",\"count\":0,\"data\":[]}";
            }
            return data;
        }

        public class LayuiDTO
        {
            public string code { get; set; }
            public string msg { get; set; }

            public int total { get; set; }

            public string data { get; set; }
        }


    }
}
