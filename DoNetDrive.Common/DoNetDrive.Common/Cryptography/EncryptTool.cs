using System;
using System.IO;

using System.Security.Cryptography;
using System.Text;

namespace DoNetDrive.Common
{
    /// <summary>
    /// 加密工具类
    /// </summary>
    public class EncryptTool
    {
        

        /// <summary>
        /// 获取随机IV（base64格式）
        /// </summary>
        /// <returns></returns>
        public static string GetRandomBase64IV()
        {
            Random rd = new Random();

            byte[] b = new byte[16];

            for (int i = 0; i < 16; i++)
            {
                b[i] = Convert.ToByte(rd.Next(1, 256));
            }

            return Convert.ToBase64String(b);
        }


        

        /// <summary>
        /// 字符串MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5StrToStr(string str)
        {
            byte[] result = System.Text.Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }

        /// <summary>
        /// 字节MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] MD5ByteToByte(byte[] b)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(b);
            return output;
        }

        /// <summary>
        /// 字符串MD5加密返回字节流
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] MD5StrToBty(string str)
        {
            var md5Hash = MD5.Create();
            var data = md5Hash.ComputeHash(Encoding.Default.GetBytes(str));
            return data;
        }
    }
}
