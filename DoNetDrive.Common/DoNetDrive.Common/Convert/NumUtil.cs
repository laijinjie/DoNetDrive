using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNetDrive.Common
{
    /// <summary>
    /// 数值类型的格式转换
    /// </summary>
    public class NumUtil
    {
        #region 字节转整数
        /// <summary>
        /// 字节数组转长整形
        /// </summary>
        /// <param name="b">字节数组 1-8个元素</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static UInt64 ByteToInt64(byte[] b)
        {
            var iLen = b.Length;
            if (iLen > 8)
                iLen = 8;
            return ByteToInt64(b, 0, iLen);
        }

        /// <summary>
        /// 字节数组转长整形
        /// </summary>
        /// <param name="b">需要计算的数组</param>
        /// <param name="iBeginIndex">数组起始索引号</param>
        /// <param name="iLen">数据长度 最大8字节</param>
        /// <returns></returns>
        public static UInt64 ByteToInt64(byte[] b, int iBeginIndex, int iLen = 8)
        {
            UInt64 lValue = 0;
            if (!(Arrays<byte>.CheckBuf(b, iBeginIndex, ref iLen, 8)))
                return lValue;
           
            int i;
            iLen -= 1;
            for (i = 0; i <= iLen; i++)
            {
                lValue = lValue + b[iBeginIndex + i];
                if (!(i == iLen))
                    lValue = lValue << 8;
            }

            return lValue;
        }


        /// <summary>
        /// 字节数组转 Int32 整形 高位在前
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static UInt32 ByteToInt32(byte[] bData)
        {
            return ByteToInt32(bData, 0, bData.Length);
        }

        /// <summary>
        /// 字节数组转 Int32 整形 高位在前
        /// </summary>
        /// <param name="bData">需要计算的数组</param>
        /// <param name="iBeginIndex">数组起始索引号</param>
        /// <param name="iLen">数据长度 最大4字节</param>
        /// <returns></returns>
        public static UInt32 ByteToInt32(byte[] bData, int iBeginIndex, int iLen = 4)
        {
            UInt32 iValue = 0;
            if (!(Arrays<byte>.CheckBuf(bData, iBeginIndex, ref iLen, 4)))
                return iValue;


            iLen -= 1;
            for (byte i = 0; i <= iLen; i++)
            {
                iValue = iValue << 8;
                iValue += bData[iBeginIndex + i];
            }
            return iValue;
        }


        /// <summary>
        /// 字节数组转短整型 Int16 高位在前
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ushort ByteToInt16(byte[] bData)
        {
            ushort iValue = bData[0];
            iValue = (ushort)((iValue << 8) + bData[1]);
            return iValue;
        }

        /// <summary>
        /// 字节数组转短整型 Int16 高位在前
        /// </summary>
        /// <param name="bData">需要计算的数组</param>
        /// <param name="iBeginIndex">数组起始索引号</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ushort ByteToInt16(byte[] bData, int iBeginIndex)
        {
            ushort iValue = bData[iBeginIndex];
            iValue = (ushort)((iValue << 8) + bData[iBeginIndex + 1]);
            return iValue;
        }



        /// <summary>
        /// 1字节转8Bit
        /// </summary>
        /// <param name="iNum"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] ByteToBit(byte iNum)
        {
            byte[] iBit = new byte[8];
            int iValue = 0, iMask = 1;

            for (int i = 0; i < 8; i++)
            {
                iValue = iNum & iMask;
                if (iValue > 0) iBit[i] = 1;
                iMask *= 2;
            }
            return iBit;
        }

        /// <summary>
        /// 字节数组转十进制数
        /// </summary>
        /// <param name="b">字节数组</param>
        /// <param name="iBegin">数组起始索引号</param>
        /// <param name="iLen">数据长度 最大8字节</param>
        /// <returns></returns>
        public static decimal BytesToDecimal(byte[] b, int iBegin, int iLen)
        {
            decimal lValue = 0;
            int i;
            if (!(Arrays<byte>.CheckBuf(b, iBegin, ref iLen, 10)))
                return lValue;
            if (iLen <= 8)
            {
                lValue = (decimal)ByteToInt64(b, iBegin, iLen);
                return lValue;
            }


            var iEndIndex = (iLen + iBegin) - 1;
            for (i = iBegin; i <= iEndIndex; i++)
            {
                lValue = lValue + b[i];
                if (!(i == iEndIndex))
                    lValue = lValue * 256;
            }

            return lValue;
        }

        /// <summary>
        /// 字节转BCD码
        /// </summary>
        /// <param name="iNum"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte ByteToBCD(byte iNum)
        {
            return (byte)((iNum / 10) * 16 + (iNum % 10));
        }

        /// <summary>
        /// 字节转BCD码
        /// </summary>
        /// <param name="bBufs"></param>
        /// <param name="iBegin"></param>
        /// <param name="ilen"></param>
        /// <returns></returns>
        public static byte[] ByteToBCD(byte[] bBufs, int iBegin, int ilen)
        {
            if (!(Arrays<byte>.CheckBuf(bBufs, iBegin, ref ilen, bBufs.Length)))
                return bBufs;
            int i;
            var iEndIndex = (ilen + iBegin) - 1;
            for (i = iBegin; i <= iEndIndex; i++)
            {
                bBufs[i] = (byte)((bBufs[i] / 10) * 16 + (bBufs[i] % 10));
            }

            return bBufs;
        }
        /// <summary>
        /// 字节转BCD码
        /// </summary>
        /// <param name="bBufs"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] ByteToBCD(byte[] bBufs)
        {
            return ByteToBCD(bBufs, 0, bBufs.Length);
        }

        #endregion


        #region 整数转字节

        /// <summary>
        /// 将长整形转为8字节数组
        /// </summary>
        /// <param name="lValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] Int64ToByte(UInt64 lValue)
        {
            return UInt64ToByte(lValue, new byte[8], 0, 8);
        }


        /// <summary>
        /// 将UInt64整形转为8字节数组 高位在前
        /// </summary>
        /// <param name="lValue"></param>
        /// <param name="bBuf"></param>
        /// <param name="iBeginIndex">数组起始索引号</param>
        /// <param name="iLen">数据长度 最大8字节</param
        public static byte[] Int64ToByte(UInt64 lValue, byte[] bBuf, int iBeginIndex, int iLen = 8)
        {
            if (!(Arrays<byte>.CheckBuf(bBuf, iBeginIndex, ref iLen, 8)))
                return bBuf;
            UInt64ToByte(lValue, bBuf, iBeginIndex, iLen);
            return bBuf;
        }

        /// <summary>
        /// 将一个UInt64数值转换为字节数组
        /// </summary>
        /// <param name="lValue"></param>
        /// <param name="bBuf"></param>
        /// <param name="iBeginIndex"></param>
        /// <param name="iLen"></param>
        private static byte[] UInt64ToByte(UInt64 lValue, byte[] bBuf, int iBeginIndex, int iLen)
        {
            int i;
            iLen -= 1;
            UInt64 lMask = 255;

            for (i = 0; i <= iLen; i++)
            {
                bBuf[(iBeginIndex + iLen - i)] = (byte)(lValue & lMask);

                lValue = lValue >> 8;
                if (lValue == 0)
                    break;
            }
            return bBuf;
        }


        /// <summary>
        /// Int24 整形 转 3字节数组 高位在前
        /// </summary>
        /// <param name="iValue"></param>
        /// <param name="bBuf">用来存放数据的数组</param>
        /// <param name="iBeginIndex">数组起始索引号</param>
        /// <param name="iLen">数据长度 最大8字节</param
        /// <returns></returns>
        public static byte[] Int24ToByte(int iValue, byte[] bBuf, int iBeginIndex, int iLen = 3)
        {
            if (!(Arrays<byte>.CheckBuf(bBuf, iBeginIndex, ref iLen, 3)))
                return bBuf;

            return UInt64ToByte((UInt64)iValue, bBuf, iBeginIndex, iLen);
        }

        /// <summary>
        /// Int32 整形 转 3字节数组 高位在前
        /// </summary>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public static byte[] Int24ToByte(int iValue)
        {
            return UInt64ToByte((UInt64)iValue, new byte[3], 0, 3);
        }

        /// <summary>
        /// Int32 整形 转 4字节数组 高位在前
        /// </summary>
        /// <param name="iValue"></param>
        /// <param name="bBuf">用来存放数据的数组</param>
        /// <param name="iBeginIndex">数组起始索引号</param>
        /// <param name="iLen">数据长度 最大8字节</param
        /// <returns></returns>
        public static byte[] Int32ToByte(UInt32 iValue, byte[] bBuf, int iBeginIndex, int iLen = 4)
        {
            if (!(Arrays<byte>.CheckBuf(bBuf, iBeginIndex, ref iLen, 4)))
                return bBuf;

            return UInt64ToByte((UInt64)iValue, bBuf, iBeginIndex, iLen);
        }

        /// <summary>
        /// Int32 整形 转 4字节数组 高位在前
        /// </summary>
        /// <param name="iValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] Int32ToByte(UInt32 iValue)
        {
            return UInt64ToByte((UInt64)iValue, new byte[4], 0, 4);
        }


        /// <summary>
        /// Int16 整形 转 字节数组  高位在前
        /// </summary>
        /// <param name="iValue"></param>
        /// <param name="bBuf"></param>
        /// <param name="iBeginIndex"></param>
        /// <param name="iLen"></param>
        /// <returns></returns>
        public static byte[] Int16ToByte(UInt16 iValue, byte[] bBuf, int iBeginIndex, int iLen = 2)
        {
            if (!(Arrays<byte>.CheckBuf(bBuf, iBeginIndex, ref iLen, 2)))
                return bBuf;

            ushort l = 255;
            bBuf[iBeginIndex + 1] = (byte)(iValue & l);
            iValue = (ushort)(iValue >> 8);
            bBuf[iBeginIndex] = (byte)(iValue & l);
            return bBuf;
        }


        /// <summary>
        /// Int16 整形 转 字节数组  高位在前
        /// </summary>
        /// <param name="iValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] Int16ToByte(UInt16 iValue)
        {
            byte[] b = new byte[2];
            ushort l = 255;
            b[1] = (byte)(iValue & l);
            iValue = (ushort)(iValue >> 8);
            b[0] = (byte)(iValue & l);
            return b;
        }



        /// <summary>
        /// BCD转字节值
        /// </summary>
        /// <param name="iNum"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte BCDToByte(byte iNum)
        {
            return (byte)(((iNum / 16) * 10) + (iNum % 16));
        }

        /// <summary>
        /// BCD转字节值
        /// </summary>
        /// <param name="bBufs"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] BCDToByte(byte[] bBufs)
        {
            return BCDToByte(bBufs, 0, bBufs.Length);
        }


        /// <summary>
        /// BCD转字节值
        /// </summary>
        /// <param name="bBufs"></param>
        /// <param name="iBegin"></param>
        /// <param name="ilen"></param>
        /// <returns></returns>
        public static byte[] BCDToByte(byte[] bBufs, int iBegin, int ilen)
        {
            if (!(Arrays<byte>.CheckBuf(bBufs, iBegin, ref ilen, bBufs.Length)))
                return bBufs;
            int i;
            var iEndIndex = (ilen + iBegin) - 1;
            for (i = iBegin; i <= iEndIndex; i++)
            {
                bBufs[i] = (byte)(((bBufs[i] / 16) * 10) + (bBufs[i] % 16));
            }

            return bBufs;
        }


        /// <summary>
        /// 8Bit转1字节
        /// </summary>
        /// <param name="iBits">存储位数据的8元素数组</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte BitToByte(byte[] iBits)
        {
            if (iBits.Length > 8) return 0;

            int value = 0;
            for (byte i = 7; i > 0; i--)
            {
                if (iBits[i] != 0) value += 1;
                value = value << 1;
            }
            if (iBits[0] != 0) value += 1;
            
            return (byte)value;
        }

       

        /// <summary>
        ///十进制数 转 字节数组
        /// </summary>
        /// <param name="lValue"></param>
        /// <returns></returns>
        public static byte[] DecimalToBytes(decimal lValue)
        {
            byte[] b = new byte[16];
            decimal t = UInt64.MaxValue;
            t += 1;
            decimal tmp;
            UInt64[] valuelist = new UInt64[2];
            if(lValue > UInt64.MaxValue)
            {
                tmp = lValue / t;
                tmp = (UInt64)tmp;
                valuelist[1] = (UInt64)(lValue - tmp * t);//低8字节
                valuelist[0] = (UInt64)tmp;//高8字节
            }
            else
            {
                valuelist[1] = (UInt64)lValue;//低8字节
                valuelist[0] = 0;//高8字节
            }
            byte[] tmpBuf = new byte[8];
            int iIndex = 0;
            for (int i = 0; i <=1 ; i++) 
            {
                UInt64ToByte(valuelist[i], tmpBuf, 0, 8);
                Array.Copy(tmpBuf, 0, b, iIndex, 8);
                iIndex += 8;
            }

            return b;
        }

        #endregion


        #region 有符号无符号转换
        /// <summary>
        /// 无符号Int64  转 有符号Int64
        /// </summary>
        /// <returns></returns>
        public static long UInt64ToInt64(UInt64 ivalue)
        {
            return (long)ivalue;
        }

        /// <summary>
        /// 无符号Int32  转 有符号Int32
        /// </summary>
        /// <returns></returns>
        public static int UInt32ToInt32(UInt32 ivalue)
        {
            return (int)ivalue;
        }

        /// <summary>
        /// 无符号Int16  转 有符号Int16
        /// </summary>
        /// <returns></returns>
        public static short UInt16ToInt16(ushort ivalue)
        {
            return (short)ivalue;
        }


        /// <summary>
        /// 无符号8位整形，转换为有符号8位整形。
        /// </summary>
        /// <param name="bValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static sbyte ByteToSByte(byte bValue)
        {
            return (sbyte)bValue;
        }


        /// <summary>
        /// 有符号Int64  转 无符号Int64
        /// </summary>
        /// <returns></returns>
        public static UInt64 Int64ToUInt64(long ivalue)
        {
            return (UInt64)ivalue;
        }

        /// <summary>
        /// 有符号Int32  转 无符号Int32
        /// </summary>
        /// <returns></returns>
        public static UInt32 UInt32ToInt32(int ivalue)
        {
            return (UInt32)ivalue;
        }

        /// <summary>
        /// 有符号Int16  转 无符号Int16
        /// </summary>
        /// <returns></returns>
        public static ushort Int16ToUInt16(short ivalue)
        {
            return (ushort)ivalue;
        }

        /// <summary>
        /// 有符号8位整形，转换为无符号8位整形。
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte SBytetobyte(sbyte b)
        {
            return (byte)b;
        }

        #endregion

    }
}
