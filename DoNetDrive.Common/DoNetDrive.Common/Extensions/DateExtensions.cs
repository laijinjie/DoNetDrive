using System;

namespace DoNetDrive.Common.Extensions
{

    /// <summary>
    /// 日期扩展方法类
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string UserDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public static string UserDateFormat = "yyyy-MM-dd";

        /// <summary>
        /// HH:mm:ss
        /// </summary>
        public static string UserTimeFormat = "HH:mm:ss";

        /// <summary>
        /// 输出格式化为 UserDateTimeFormat 的日期字符串
        /// 默认格式 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToDateTimeStr(this DateTime d)
        {
            return d.ToString(UserDateTimeFormat);
        }

        /// <summary>
        /// 输出格式化为 HH:mm:ss.ffff 的时间字符串
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToTimeffff(this DateTime d)
        {
            return d.ToString("HH:mm:ss.ffff");
        }

        /// <summary>
        /// 输出格式化为 UserDateFormat 的日期字符串
        /// 默认格式 yyyy-MM-dd
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToDateStr(this DateTime d)
        {
            return d.ToString(UserDateFormat);
        }
        /// <summary>
        /// 输出格式化为 UserDateFormat 的时间字符串
        /// 默认格式 HH:mm:ss
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToTimeStr(this DateTime d)
        {
            return d.ToString(UserTimeFormat);
        }

        /// <summary>
        /// 返回OADate
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static DateTime ToDate(this double b)
        {
            return DateTime.FromOADate(b);
        }

        /// <summary>
        /// 返回OADate yyyy-MM-dd 格式
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToDateStr(this double b)
        {
            return b.ToDate().ToDateStr();
        }

        /// <summary>
        /// 返回返回OADate yyyy-MM-dd HH:mm:ss 格式
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToDateTimeStr(this double b)
        {
            return b.ToDate().ToDateTimeStr();
        }

       


        /// <summary>
        /// 判断符合日期格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsDate(this string dt)
        {
            DateTime d = DateTime.MinValue;
            return DateTime.TryParse(dt, out d);
        }

        /// <summary>
        /// 将字符串转换为日期格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime toDate(this string dt)
        {
            DateTime d = DateTime.MinValue;
            DateTime.TryParse(dt, out d);
            return d;
        }


        /// <summary>
        /// 将日期转换为BCD码中的 YYMMDD 格式，返回3个字节
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte[] ToBCDYYMMDD(this DateTime b)
        {
            byte[] bData = new byte[3];
            bData[0] = (byte)(b.Year - 2000);
            bData[1] = (byte)(b.Month);
            bData[2] = (byte)(b.Day);
            
            return NumUtil.ByteToBCD(bData) ;
        }

        /// <summary>
        /// 将日期转换为BCD码中的 YYMMDD 格式，返回3个字节
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte[] ToBCDYYYYMMDD(this DateTime b)
        {
            byte[] bData = new byte[8];
            byte[] bYear = b.Year.To4Bytes();
            for (int i = 0; i < bYear.Length; i++)
            {
                bData[i] = bYear[i];
            }
            byte[] bMonth = b.Month.To2Bytes();
            byte[] bDay = b.Day.To2Bytes();

            bData[4] = bMonth[0];
            bData[5] = bMonth[1];

            bData[6] = bDay[0];
            bData[7] = bDay[1];
            return NumUtil.ByteToBCD(bData);
        }

        /// <summary>
        /// 将日期转换为BCD码中的 YYMMDD 格式，返回3个字节
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static byte[] DateToBCDYYMMdd(this DateTime b)
        {
            return ToBCDYYMMDD(b);
        }

        /// <summary>
        /// 将日期转换为BCD码中的 YYMMDD 格式，返回3个字节
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static byte[] DateToBCDYYYYMMdd(this DateTime b)
        {
            return ToBCDYYYYMMDD(b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blist"></param>
        /// <returns></returns>
        public static DateTime BCDYYYYMMddToDate(this byte[] blist)
        {
            byte[] bYear = NumUtil.ByteToBCD(blist.Copy(0, 4));// ;
            int year = (int)bYear.ToInt32();

            byte[] bMonth = NumUtil.ByteToBCD(blist, 4, 2);
            int month = (int)bMonth.ToInt32();

            byte[] bDay = blist.Copy(6, 2);
            int day = (int)bDay.ToInt32();

            return new DateTime(year,month,day);
        }
    }

}
