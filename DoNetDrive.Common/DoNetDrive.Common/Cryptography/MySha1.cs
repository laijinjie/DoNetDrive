using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DoNetDrive.Common.Cryptography
{
    /// <summary>
    /// SHA1加密工具类
    /// </summary>
    public class MySha1
    {
        /// <summary>
        /// 使用SHA1加密
        /// </summary>
        /// <param name="sValue"></param>
        /// <param name="sGUID"></param>
        /// <returns></returns>
        public static string CreateSha1(string sValue, string sGUID)
        {
            return Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(sValue + sGUID)));
        }

        /// <summary>
        /// 使用SHA1加密
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static string CreateSha1(string sValue)
        {
            return CreateSha1(sValue, "297322ce-f112-4711-84a7-7ed9331c2d99");
        }
    }
}
