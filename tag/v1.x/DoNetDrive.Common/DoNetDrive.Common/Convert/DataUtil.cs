using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DoNetDrive.Common
{
    /// <summary>
    /// 数据转换工具类
    /// </summary>
    public class DataUtil
    {
        /// <summary>
        /// BCD格式日期字节数组转换为日期类型   需要传入6字节
        /// 格式 yyMMddHHmmss
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime ByteToDateTime(byte[] bData)
        {
            for (int i = 0; i <= bData.Length - 1; i++)
            {
                if (!StringUtil.IsNum(bData[i].ToString("X")))
                    return DateTime.MinValue;
            }

            bData = NumUtil.BCDToByte(bData);
            if (bData.Length < 6)
                return DateTime.MinValue;

            for (int i = 0; i <= 2; i++)
            {
                if (bData[i] == 0)
                    return DateTime.MinValue;
            }


            return new DateTime(2000 + bData[0], bData[1], bData[2], bData[3], bData[4], bData[5]);
        }


        /// <summary>
        /// 字节数组转换为日期类型  BCD日期 需要传入3字节
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime ByteToDate(byte[] bData)
        {
            for (int i = 0; i <= bData.Length - 1; i++)
            {
                if (!StringUtil.IsNum(Convert.ToString(bData[i], 16)))
                    return DateTime.MinValue;
            }

            bData = NumUtil.BCDToByte(bData);
            if (bData.Length < 3)
                return DateTime.MinValue;

            for (int i = 0; i <= 2; i++)
            {
                if (bData[i] == 0)
                    return DateTime.MinValue;
            }

            try
            {
                return new DateTime(2000 + bData[0], bData[1], bData[2]);
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }


        /// <summary>
        /// 返回BCD 格式yyMMddHHmmss
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBCDTime()
        {
            return GetBCDTime(DateTime.Now);
        }

        /// <summary>
        /// 返回BCD 格式yyMMddHHmmss
        /// </summary>
        /// <param name="oDate"></param>
        /// <returns></returns>
        public static byte[] GetBCDTime(DateTime oDate)
        {
            byte[] bData = new byte[7];
            string sHex;
            sHex = oDate.ToString("yyMMddHHmmss");
            Array.Copy(StringUtil.HexToByte(sHex), bData, 6);
            bData[6] = (byte)oDate.DayOfWeek; // 周
            return bData;
        }

        /// <summary>
        /// 将BCD数组转换为日期，BCD数据格式为 MMDD，只有 月和日
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        public static DateTime BCDToDateMMdd(byte[] bData)
        {
            bData = NumUtil.BCDToByte(bData);
            if (bData[0] > 12 | bData[0] == 0)
                return default(DateTime);

            if (bData[1] > 31 | bData[1] == 0)
                return default(DateTime);

            return new DateTime(DateTime.Now.Year, bData[0], bData[1]);
        }
        /// <summary>
        /// 将日期转换为BCD码，格式为 YYMMDD
        /// </summary>
        /// <param name="oDate"></param>
        /// <returns></returns>
        public static byte[] DateToBCDYYMMdd(DateTime oDate)
        {
            byte[] bData;
            if (oDate == default(DateTime) || oDate == null)
            {
                bData = new byte[3];
                return bData;
            }

            if (oDate.Equals(DateTime.MinValue))
            {
                bData = new byte[3];
                return bData;
            }
            else
                bData = new byte[] { (byte)(oDate.Year - 2000), (byte)oDate.Month, (byte)oDate.Day };

            return NumUtil.ByteToBCD(bData);
        }


        /// <summary>
        /// 将日期转换为BCD码，格式为 MMDD,只有 月和日
        /// </summary>
        /// <param name="oDate"></param>
        /// <returns></returns>
        public static byte[] DateToBCDMMdd(DateTime oDate)
        {
            byte[] bData;
            if (oDate == default(DateTime) || oDate == null)
            {
                bData = new byte[2];
                return bData;
            }

            if (oDate.Equals(DateTime.MinValue))
            {
                bData = new byte[2];
                return bData;
            }
            else
                bData = new byte[] {  (byte)oDate.Month, (byte)oDate.Day };

            return NumUtil.ByteToBCD(bData);
        }


    }
}
