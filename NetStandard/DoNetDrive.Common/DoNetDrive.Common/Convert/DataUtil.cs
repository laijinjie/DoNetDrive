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
        /// 检查符合数字类型格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value.Trim();

            Regex r = new Regex("^\\d+$");

            return r.IsMatch(value);
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
            Array.Copy(HexToByte(sHex), bData, 6);
            bData[6] = (byte)oDate.DayOfWeek; // 周
            return bData;
        }

        private static byte[] HexToByte(string sHex)
        {
            try
            {
                sHex = sHex.ToUpper().Replace(" ", "");
                int len = sHex.Length / 2;
                byte[] data = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    data[i] = Convert.ToByte(sHex.Substring(i * 2, 2), 16);
                }
                return data;
            }
            catch (Exception ex)
            { }
            return null;
        }

    }
}
