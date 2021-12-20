using System;

namespace DoNetDrive.Common.Extensions
{
    /// <summary>
    /// 数字扩展方法类
    /// </summary>
    public static class NumExtensions
    {
        /// <summary>
        ///     ''' 返回8个字节
        ///     ''' </summary>
        ///     ''' <param name="l"></param>
        ///     ''' <returns></returns>
        public static byte[] ToBytes(this long l)
        {
            return NumUtil.Int64ToByte((UInt64)l);
        }

        /// <summary>
        /// Int32ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To4Bytes(this long l)
        {
            UInt32 v = (UInt32)(l & UInt32.MaxValue);
            return NumUtil.Int32ToByte(v);
        }

        /// <summary>
        /// Int24ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To3Bytes(this long l)
        {
            int v = (int)(l & UInt32.MaxValue);
            return NumUtil.Int24ToByte(v);
        }


        /// <summary>
        /// Int16ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To2Bytes(this long l)
        {
            UInt16 v = (UInt16)(l & UInt16.MaxValue);
            return NumUtil.Int16ToByte(v);
        }


        /// <summary>
        ///     ''' 返回8个字节
        ///     ''' </summary>
        ///     ''' <param name="l"></param>
        ///     ''' <returns></returns>
        public static byte[] To8Bytes(this UInt64 l)
        {
            return NumUtil.Int64ToByte(l);
        }

        /// <summary>
        /// To4Bytes 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To4Bytes(this UInt64 l)
        {
            UInt32 v = (UInt32)(l & UInt32.MaxValue);
            return NumUtil.Int32ToByte(v);
        }

        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To3Bytes(this UInt64 l)
        {
            int v = (int)(l & UInt32.MaxValue);
            return NumUtil.Int24ToByte(v);
        }

        /// <summary>
        /// Int16ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To2Bytes(this UInt64 l)
        {
            UInt16 v = (UInt16)(l & UInt16.MaxValue);
            return NumUtil.Int16ToByte(v);
        }



        /// <summary>
        /// Int32ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To4Bytes(this int l)
        {
            return NumUtil.Int32ToByte((UInt32)l);
        }

        /// <summary>
        /// Int24ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To3Bytes(this int l)
        {
            return NumUtil.Int24ToByte(l);
        }

        /// <summary>
        /// Int16ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To2Bytes(this int l)
        {
            UInt16 v = (UInt16)(l & UInt16.MaxValue);
            return NumUtil.Int16ToByte(v);
        }

        /// <summary>
        /// Int16ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To2Bytes(this UInt16 l)
        {
            return NumUtil.Int16ToByte(l);
        }

        /// <summary>
        /// Int16ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To2Bytes(this Int16 l)
        {
            return NumUtil.Int16ToByte((UInt16)l);
        }

        /// <summary>
        /// Int32ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To4Bytes(this UInt32 l)
        {
            return NumUtil.Int32ToByte(l);
        }

        /// <summary>
        /// Int24ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To3Bytes(this UInt32 l)
        {
            return NumUtil.Int24ToByte((int)l);
        }

        /// <summary>
        /// Int16ToByte 扩展方法
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static byte[] To2Bytes(this UInt32 l)
        {
            UInt16 v = (UInt16)(l & UInt16.MaxValue);
            return NumUtil.Int16ToByte(v);
        }
    }

}
