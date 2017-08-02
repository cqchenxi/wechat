using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WeChat
{
    public class Signature
    {
        #region 验证签名
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce)
        {
            string[] arr = new[] { Config.Token, timestamp, nonce }.OrderBy(z => z).ToArray();
            string arrString = string.Join("", arr);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            string tempString = BitConverter.ToString(sha1Arr).Replace("-", "").ToLower();
            if (signature == tempString)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}