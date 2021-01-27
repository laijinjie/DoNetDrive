
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DoNetDrive.Common.Extensions
{
    
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 编码
        /// </summary>
        public static System.Text.Encoding UserEncoding = System.Text.Encoding.ASCII;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool IsBase64(this string base64String, out byte[] bytes)
        {
            bytes = null;
            // Credit: oybek http://.com/users/794764/oybek
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                bytes = Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception)
            {
                // Handle the exception
            }

            return false;
        }


        /// <summary>
        /// 将base64字符串转换为字节数组
        /// </summary>
        /// <param name="base64str"></param>
        /// <returns></returns>
        public static byte[] FromBase64(this string base64str)
        {
            if (string.IsNullOrWhiteSpace(base64str)) return null;
            try
            {
                return Convert.FromBase64String(base64str);
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aString"></param>
        public static void Print(this string aString)
        {
            Console.WriteLine(aString);
        }

        /// <summary>
        /// 扩展方法 安全转int类型
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this string aString)
        {
            Int32 iValue = 0;
            if (Int32.TryParse(aString, out iValue))
                return iValue;
            else
                return 0;
        }


        /// <summary>
        /// 扩展方法 安全转UInt32类型
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this string aString)
        {
            UInt32 iValue = 0;
            if (UInt32.TryParse(aString, out iValue))
                return iValue;
            else
                return 0;
        }

        /// <summary>
        /// String 安全转 UInt32
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static UInt32 HexToUInt32(this string aString)
        {
            UInt32 iValue = 0;
            if (UInt32.TryParse(aString, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out iValue))
                return iValue;
            else
                return 0;
        }

        /// <summary>
        /// String 安全转 Double
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static double ToDouble(this string aString)
        {
            double iValue = 0;
            if (double.TryParse(aString, out iValue))
                return iValue;
            else
                return 0;
        }

        /// <summary>
        /// String 安全转 Long
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static long ToLong(this string aString)
        {
            long iValue = 0;
            if (long.TryParse(aString, out iValue))
                return iValue;
            else
                return 0;
        }

        /// <summary>
        /// 扩展方法 安全转 UInt64
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this string aString)
        {
            UInt64 iValue = 0;
            if (UInt64.TryParse(aString, out iValue))
                return iValue;
            else
                return 0;
        }

        /// <summary>
        /// 扩展方法 安全转16进制 转 UInt64
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static UInt64 HexToUInt64(this string aString)
        {
            UInt64 iValue = 0;
            if (UInt64.TryParse(aString, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out iValue))
                return iValue;
            else
                return 0;
        }


        /// <summary>
        /// 扩展方法 检查是否为数字
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static bool IsTryLong(this string aString)
        {
            long l;
            if (string.IsNullOrWhiteSpace(aString))
                return false;

            return long.TryParse(aString, out l);
        }

        /// <summary>
        /// 检查是否有纯数字组成
        /// </summary>
        /// <param name="numString">需要检查的字符串</param>
        /// <returns>true 表示是纯数字字符串；false 包含非法字符</returns>
        public static bool IsNum(this string numString)
        {
            if (numString.IsNullOrEmpty())
            {
                return false;
            }
            return StringUtil.IsNum(numString);
        }

        /// <summary>
        /// 检查字符串是否为空字符串或者为null
        /// </summary>
        /// <param name="str">需要检查的字符串</param>
        /// <returns>true表示为空或null</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        ///     ''' 扩展方法 检查字符串是否为十六进制
        ///     ''' </summary>
        ///     ''' <param name="s"></param>
        ///     ''' <returns></returns>
        public static bool IsHex(this string s)
        {
            return StringUtil.IsHex(s);
        }

        /// <summary>
        ///     ''' 扩展方法 十六进制转为字节数字
        ///     ''' </summary>
        ///     ''' <param name="s"></param>
        ///     ''' <returns></returns>
        public static byte[] HexToByte(this string s)
        {
            return StringUtil.HexToByte(s);
        }



        /// <summary>
        ///     ''' 扩展方法 获取字符串的字节编码
        ///     ''' </summary>
        ///     ''' <param name="s"></param>
        ///     ''' <returns></returns>
        public static byte[] GetBytes(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;
            return UserEncoding.GetBytes(s);
        }

        /// <summary>
        /// Format 扩展方法
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string SF(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        /// <summary>
        /// Split 扩展方法，过滤空值
        /// </summary>
        /// <param name="s"></param>
        /// <param name="sSplit"></param>
        /// <returns></returns>
        public static string[] SplitTrim(this string s, string sSplit)
        {
            return s.Split(new[] { sSplit }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 返回是否相同
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsSame(this string s, string d)
        {
            return string.Compare(s, d, true) == 0;
        }



        /// <summary>
        /// 填充字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="iLen"></param>
        /// <param name="sFillStr"></param>
        /// <returns></returns>
        public static string FillString(this string s, int iLen, char sFillStr)
        {
            return StringUtil.CheckStringLen(s, iLen, sFillStr);
        }
    }

}
