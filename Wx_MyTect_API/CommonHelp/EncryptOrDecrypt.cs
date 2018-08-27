using CYQ.Data;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CommonHelp
{
    public class EncryptOrDecrypt
    {
        #region MD5加密
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(password)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }


        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string password)
        {
            string cl = password;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }

        public static string MD5Encrypt64(string password)
        {
            string cl = password;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="codeLength">加密位数</param>
        /// <returns>加密密码</returns>
        public static string md5(string password, int codeLength)
        {
            if (!string.IsNullOrEmpty(password))
            {
                // 16位MD5加密（取32位加密的9~25字符）  
                if (codeLength == 16)
                {
                    return MD5Encrypt16(password);
                }

                // 32位加密
                if (codeLength == 32)
                {
                    return MD5Encrypt32(password);
                }
                // 64位加密
                if (codeLength == 64)
                {
                    return MD5Encrypt64(password);
                }
            }
            return string.Empty;
        }
        #endregion


        /// <summary>
        /// DES数据加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            using (RijndaelManaged cryptoHelper = new RijndaelManaged())
            {
                cryptoHelper.Key = Convert.FromBase64String(AppConfig.GetApp("Key"));
                cryptoHelper.IV = Convert.FromBase64String(AppConfig.GetApp("IV"));

                ICryptoTransform encryptor = cryptoHelper.CreateEncryptor();
                byte[] oldeText = Encoding.UTF8.GetBytes(text);
                byte[] newText = encryptor.TransformFinalBlock(oldeText, 0, oldeText.Length);
                encryptor.Dispose();
                return Convert.ToBase64String(newText);
            }
        }
        /// <summary>
        /// DES数据解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            using (RijndaelManaged cryptoHelper = new RijndaelManaged())
            {
                cryptoHelper.Key = Convert.FromBase64String(AppConfig.GetApp("Key"));
                cryptoHelper.IV = Convert.FromBase64String(AppConfig.GetApp("IV"));
                ICryptoTransform decryptor = cryptoHelper.CreateDecryptor();
                byte[] oleText = Convert.FromBase64String(text);
                byte[] newText = decryptor.TransformFinalBlock(oleText, 0, oleText.Length);
                decryptor.Dispose();
                return Encoding.UTF8.GetString(newText);
            }
        }
        
    }
}
