using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserModel
    {

        public string UserName { get; set; }
        public string UserGuid { get; set; }
        public long  UserId { get; set; }

        public string UserHead { get; set; }
        /// <summary>
        /// 性别 0 未知 1男 2女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 微信数据  国家 CN中国
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 微信数据 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 微信数据  城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiryTime { get; set; }

    }
}
