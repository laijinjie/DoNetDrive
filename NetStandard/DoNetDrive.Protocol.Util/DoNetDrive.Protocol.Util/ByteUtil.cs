using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Util
{
    public class ByteUtil
    {
        public static byte ByteToBCD(byte iNum)
        {
            int iValue = iNum;
            iValue = (iValue / 10) * 16 + (iValue % 10);
            return (byte)iValue;
        }


        public static byte[] ByteToBCD(byte[] iNum)
        {
            int iLen = iNum.Length;
            for (int i = 0; i < iLen; i++)
            {
                iNum[i] = ByteToBCD(iNum[i]);
            }
            return iNum;
        }


        /// <summary>
        /// BCD 转 字节
        /// </summary>
        /// <param name="iNum"></param>
        /// <returns></returns>
        public static byte BCDToByte(byte iNum)
        {
            int iValue = iNum;
            iValue = ((iValue / 16) * 10) + (iValue % 16);
            return (byte)iValue;
        }



        /// <summary>
        /// 从一个ByteBuf中现有索引，开始转换指定长度，BCD数据 转 十进制字节
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static IByteBuffer BCDToByte(IByteBuffer buf, int iIndex, int iLen)
        {
            while (iLen > 0)
            {
                int iValue = buf.GetByte(iIndex);

                iValue = ((iValue / 16) * 10) + (iValue % 16);

                buf.SetByte(iIndex, iValue);

                iIndex++;
                iLen--;
            }

            return buf;
        }


        /// <summary>
        /// 从一个ByteBuf中现有索引，开始转换指定长度，十进制字节 转  BCD数据 
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static IByteBuffer ByteToBCD(IByteBuffer buf, int iIndex, int iLen)
        {
            while (iLen > 0)
            {
                int iValue = buf.GetByte(iIndex);

                iValue = (iValue / 10) * 16 + (iValue % 10);

                buf.SetByte(iIndex, iValue);

                iIndex++;
                iLen--;
            }

            return buf;
        }


        //BCD 转 字节
        public static byte[] BCDToByte(byte[] iNum)
        {
            int iLen = iNum.Length;
            for (int i = 0; i < iLen; i++)
            {
                iNum[i] = BCDToByte(iNum[i]);
            }
            return iNum;
        }



        /// <summary>
        /// 从Bytebuf中读取一个int24
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static int ByteBufReadInt24(IByteBuffer buffer)
        {
            int num = 0;
            num = (int)buffer.ReadByte() << 16;
            num += (int)buffer.ReadByte() <<8;
            num += buffer.ReadByte() ;
            return num;
        }
    }
}