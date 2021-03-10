using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
namespace DoNetDrive.Protocol.Util
{
    /// <summary>
    /// 缓存区扩展函数
    /// </summary>
    public static class ByteBufferExtension
    {
        /// <summary>
        /// 写入一个uint32数值到缓存区
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IByteBuffer WriteUInt32(this IByteBuffer buf, uint value)
        {
            return buf.WriteInt((int)value);
        }
        /// <summary>
        /// 写入一个uint64数值到缓存区
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IByteBuffer WriteUInt64(this IByteBuffer buf, ulong value)
        {
            return buf.WriteLong((long)value);
        }
        /// <summary>
        /// 将缓存中数据编码为HEX
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static string ToHex(this IByteBuffer buf)
        {
            return ByteBufferUtil.HexDump(buf);
        }
    }
}
