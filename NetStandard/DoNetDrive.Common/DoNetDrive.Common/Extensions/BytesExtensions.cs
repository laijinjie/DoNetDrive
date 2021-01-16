using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNetDrive.Common.Extensions
{
    /// <summary>
    /// 字节扩展类
    /// </summary>
    public static class BytesExtensions
    {
        

        /// <summary>
        /// 转换字节编码为字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string GetString(this byte[] b)
        {
            if (b[0] == 0 | b[0] == 255)
                return string.Empty;
            var iIndex = Array.FindIndex(b, x => x == 0);
            if (iIndex > 0)
                b = b.Copy(0, iIndex);
            return StringExtensions.UserEncoding.GetString(b);
        }

        /// <summary>
        /// 将数组转换为Base64字符串
        /// </summary>
        /// <param name="Databuf"></param>
        /// <returns></returns>
        public static string ToBase64(this byte[] Databuf)
        {
            if (Databuf == null) return string.Empty;
            if (Databuf.Length == 0) return string.Empty;
            return Convert.ToBase64String(Databuf);
        }

        /// <summary>
        /// 转换字节数组为十六进制字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToHex(this byte[] b)
        {
            return StringUtil.ByteToHex(b);
        }

        /// <summary>
        /// ByteToInt32 扩展方法
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static UInt32 ToInt32(this byte[] b)
        {
            return NumUtil.ByteToInt32(b);
        }

        /// <summary>
        /// ByteToInt32 扩展方法
        /// </summary>
        /// <param name="b"></param>
        /// <param name="iBeginIndex"></param>
        /// <param name="iLen"></param>
        /// <returns></returns>
        public static UInt32 ToInt32(this byte[] b, int iBeginIndex, int iLen = 4)
        {
            return NumUtil.ByteToInt32(b, iBeginIndex, iLen);
        }

        /// <summary>
        /// ByteToInt32 扩展方法
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static UInt32 ToInt24(this byte[] b)
        {
            return NumUtil.ByteToInt32(b, 0, 3);
        }

        /// <summary>
        /// ByteToInt32 扩展方法
        /// </summary>
        /// <param name="b"></param>
        /// <param name="iBeginIndex"></param>
        /// <param name="iLen"></param>
        /// <returns></returns>
        public static UInt32 ToInt24(this byte[] b, int iBeginIndex, int iLen = 3)
        {
            return NumUtil.ByteToInt32(b, iBeginIndex, iLen);
        }


        /// <summary>
        /// ByteToInt64 扩展方法
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static UInt64 ToInt64(this byte[] b)
        {
            return NumUtil.ByteToInt64(b);
        }

        /// <summary>
        /// ByteToInt64 扩展方法
        /// </summary>
        /// <param name="b"></param>
        /// <param name="iBeginIndex"></param>
        /// <param name="iLen"></param>
        /// <returns></returns>
        public static UInt64 ToInt64(this byte[] b, int iBeginIndex, int iLen = 8)
        {
            return NumUtil.ByteToInt64(b, iBeginIndex, iLen);
        }

        /// <summary>
        /// 反转后 ByteToInt32 扩展方法
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static UInt32 ToInt32Rev(this byte[] b)
        {
            byte[] br = (byte[])b.Clone();
            Array.Reverse(br);
            return NumUtil.ByteToInt32(br);
        }

        /// <summary>
        /// 反转后 ByteToInt32 扩展方法
        /// </summary>
        /// <param name="b"></param>
        /// <param name="iBeginIndex"></param>
        /// <param name="iLen"></param>
        /// <returns></returns>
        public static UInt32 ToInt32Rev(this byte[] b, int iBeginIndex, int iLen = 4)
        {
            byte[] br = (byte[])b.Copy(iBeginIndex, iLen);
            Array.Reverse(br);
            return NumUtil.ByteToInt32(br);
        }


        /// <summary>
        /// ByteToInt16 扩展方法
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static UInt16 ToInt16(this byte[] b)
        {
            return NumUtil.ByteToInt16(b);
        }

        /// <summary>
        /// ByteToInt16 扩展方法
        /// </summary>
        /// <param name="b"></param>
        /// <param name="iBeginIndex"></param>
        /// <returns></returns>
        public static UInt16 ToInt16(this byte[] b, int iBeginIndex)
        {
            return NumUtil.ByteToInt16(b, iBeginIndex);
        }


        /// <summary>
        /// 检查2个byte数组 是否相同
        /// </summary>
        /// <param name="b"></param>
        /// <param name="eb"></param>
        /// <returns></returns>
        public static bool BytesEquals(this byte[] b, byte[] eb)
        {
            return Arrays<byte>.BytesEquals(b, eb);
        }
    }
}
