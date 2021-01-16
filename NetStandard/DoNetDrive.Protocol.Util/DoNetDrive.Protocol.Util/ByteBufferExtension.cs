using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
namespace DoNetDrive.Protocol.Util
{
    public static class ByteBufferExtension
    {
        public static IByteBuffer WriteUInt32(this IByteBuffer buf, uint value)
        {
            return buf.WriteInt((int)value);
        }

        public static IByteBuffer WriteUInt64(this IByteBuffer buf, ulong value)
        {
            return buf.WriteLong((long)value);
        }

        public static string ToHex(this IByteBuffer buf)
        {
            return ByteBufferUtil.HexDump(buf);
        }
    }
}
