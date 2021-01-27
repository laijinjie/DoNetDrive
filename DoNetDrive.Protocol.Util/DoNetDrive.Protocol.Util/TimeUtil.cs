using DotNetty.Buffers;
using System;


namespace DoNetDrive.Protocol.Util
{
    public class TimeUtil
    {
        public static DateTime BCDTimeToDate_ssmmhhddMMyy(byte[] btTime)
        {
            btTime = ByteUtil.BCDToByte(btTime);
            int year = btTime[5];
            int month = btTime[4];
            int dayOfMonth = btTime[3];
            int hourOfDay = btTime[2];
            int minute = btTime[1];
            int second = btTime[0];


            if (year > 99)
            {
                return DateTime.Now;
            }
            if (month == 0 || month > 12)
            {
                return DateTime.Now;
            }
            if (dayOfMonth == 0 || dayOfMonth > 31)
            {
                return DateTime.Now;
            }
            if (hourOfDay > 23)
            {
                return DateTime.Now;
            }

            if (minute > 59)
            {
                return DateTime.Now;
            }

            if (second > 59)
            {
                return DateTime.Now;
            }

            DateTime dTime = new DateTime(2000 + year, month, dayOfMonth, hourOfDay, minute, second);
            return dTime;
        }

        public static DateTime BCDTimeToDate_yyMMddhh(IByteBuffer buf)
        {
            buf = ByteUtil.BCDToByte(buf, buf.ReaderIndex, 4);
            int year = buf.ReadByte();
            int month = buf.ReadByte();
            int day = buf.ReadByte();
            int hour = buf.ReadByte();

            if (year > 99)
            {
                return DateTime.Now;
            }
            if (month == 0 || month > 12)
            {
                return DateTime.Now;
            }
            if (day == 0 || day > 31)
            {
                return DateTime.Now;
            }
            if (hour > 23)
            {
                return DateTime.Now;
            }

            DateTime dTime = new DateTime(2000 + year, month, day, hour, 59, 59);
            return dTime;
        }

        /// <summary>
        /// 从buf读取3个字节转换成日期
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static DateTime BCDTimeToDate_yyMMdd(IByteBuffer buf)
        {
            buf = ByteUtil.BCDToByte(buf, buf.ReaderIndex, 3);
            int year = buf.ReadByte();
            int month = buf.ReadByte();
            int day = buf.ReadByte();

            if (year > 99)
            {
                return DateTime.Now;
            }
            if (month == 0 || month > 12)
            {
                return DateTime.Now;
            }
            if (day == 0 || day > 31)
            {
                return DateTime.Now;
            }


            DateTime dTime = new DateTime(2000 + year, month, day);
            return dTime;
        }

        public static void DateToBCD_yyMMddhh(byte[] btData, DateTime date)
        {
            if (date == null)
            {
                for (int i = 0; i < 4; i++)
                {
                    btData[i] = 0;
                }
            }
            else
            {
                btData[0] = (byte)(date.Year - 2000);
                btData[1] = (byte)(date.Month);
                btData[2] = (byte)date.Day;
                btData[3] = (byte)date.Hour;
                btData = ByteUtil.ByteToBCD(btData);
            }
        }

        /// <summary>
        /// 日期类型转换为BCD格式日期字节数组
        /// </summary>
        /// <param name="btData"></param>
        /// <param name="date"></param>
        public static void DateToBCD_ssmmhhddMMwwyy(byte[] btData, DateTime date)
        {
            if (date == null)
            {
                for (int i = 0; i < 7; i++)
                {
                    btData[i] = 0;
                }
            }
            else
            {
                btData[6] = (byte)(date.Year - 2000);
                btData[5] = (byte)GetWeekNum();
                btData[4] = (byte)date.Month;
                btData[3] = (byte)date.Day;
                btData[2] = (byte)date.Hour;
                btData[1] = (byte)date.Minute;
                btData[0] = (byte)date.Second;
                btData = ByteUtil.ByteToBCD(btData);
            }
        }

        /// <summary>
        /// 日期类型转换为BCD格式日期字节数组
        /// </summary>
        /// <param name="btData"></param>
        /// <param name="date"></param>
        public static void DateToBCD_ssmmhhddMMyy(byte[] btData, DateTime date)
        {
            if (date == null)
            {
                for (int i = 0; i < 6; i++)
                {
                    btData[i] = 0;
                }
            }
            else
            {
                btData[5] = (byte)(date.Year - 2000);
                btData[4] = (byte)date.Month;
                btData[3] = (byte)date.Day;
                btData[2] = (byte)date.Hour;
                btData[1] = (byte)date.Minute;
                btData[0] = (byte)date.Second;
                btData = ByteUtil.ByteToBCD(btData);
            }
        }

        /// <summary>
        /// 定义星期代表数值（星期表示：1表示星期一；2表示星期二。。。。6表示星期六；7表示星期日；）
        /// </summary>
        /// <returns></returns>
        public static int GetWeekNum()
        {
            int weekNum = 1;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    weekNum = 1;
                    break;
                case DayOfWeek.Tuesday:
                    weekNum = 2;
                    break;
                case DayOfWeek.Wednesday:
                    weekNum = 3;
                    break;
                case DayOfWeek.Thursday:
                    weekNum = 4;
                    break;
                case DayOfWeek.Friday:
                    weekNum = 5;
                    break;
                case DayOfWeek.Saturday:
                    weekNum = 6;
                    break;
                case DayOfWeek.Sunday:
                    weekNum = 7;
                    break;
            }
            return weekNum;
        }

        /// <summary>
        /// 从一个ByteBuf中读取5个BCD字节，并转换为日期格式；  yyMMddhhmm
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static DateTime BCDTimeToDate_yyMMddhhmm(IByteBuffer buf)
        {

            buf = ByteUtil.BCDToByte(buf, buf.ReaderIndex, 5);
            int year = buf.ReadByte();
            int month = buf.ReadByte();
            int day = buf.ReadByte();
            int hour = buf.ReadByte();
            int minute = buf.ReadByte();

            if (year > 99)
            {
                return DateTime.Now;
            }
            if (month == 0 || month > 12)
            {
                return DateTime.Now;
            }
            if (day == 0 || day > 31)
            {
                return DateTime.Now;
            }
            if (hour > 23)
            {
                return DateTime.Now;
            }

            if (minute > 59)
            {
                return DateTime.Now;
            }

            DateTime dTime = new DateTime(2000 + year, month, day, hour, minute, 59);
            return dTime;
        }

        /// <summary>
        /// 从一个ByteBuf中读取6个BCD字节，并转换为日期格式；  yyMMddhhmmss
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static DateTime BCDTimeToDate_yyMMddhhmmss(IByteBuffer buf)
        {
            buf = ByteUtil.BCDToByte(buf, buf.ReaderIndex, 6);
            int year = buf.ReadByte();
            int month = buf.ReadByte();
            int day = buf.ReadByte();
            int hour = buf.ReadByte();
            int minute = buf.ReadByte();
            int sec = buf.ReadByte();

            if (year > 99)
            {
                return DateTime.Now;
            }
            if (month == 0 || month > 12)
            {
                return DateTime.Now;
            }
            if (day == 0 || day > 31)
            {
                return DateTime.Now;
            }
            if (hour > 23)
            {
                return DateTime.Now;
            }

            if (minute > 59)
            {
                return DateTime.Now;
            }
            if (sec > 59)
            {
                return DateTime.Now;
            }

            DateTime dTime = new DateTime(2000 + year, month, day, hour, minute, sec);
            return dTime;
        }

        /// <summary>
        /// 从一个ByteBuf中读取6个BCD字节，并转换为日期格式；  yyMMddhhmmss
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static DateTime BCDTimeToDate_yyMMddhhmmssByDex(IByteBuffer buf)
        {
            int year = buf.ReadByte();
            int month = buf.ReadByte();
            int day = buf.ReadByte();
            int hour = buf.ReadByte();
            int minute = buf.ReadByte();
            int sec = buf.ReadByte();

            if (year > 99)
            {
                return DateTime.Now;
            }
            if (month == 0 || month > 12)
            {
                return DateTime.Now;
            }
            if (day == 0 || day > 31)
            {
                return DateTime.Now;
            }
            if (hour > 23)
            {
                return DateTime.Now;
            }

            if (minute > 59)
            {
                return DateTime.Now;
            }
            if (sec > 59)
            {
                return DateTime.Now;
            }

            DateTime dTime = new DateTime(2000 + year, month, day, hour, minute, sec);
            return dTime;
        }

        /// <summary>
        /// 从一个ByteBuf中读取7个BCD字节，并转换为日期格式；  yyyyMMddhhmmss
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static DateTime BCDTimeToDate_yyyyMMddhhmmss(IByteBuffer buf)
        {
            buf = ByteUtil.BCDToByte(buf, buf.ReaderIndex, 7);
            int year = buf.ReadByte();
            int year1 = buf.ReadByte();
            int month = buf.ReadByte();
            int day = buf.ReadByte();
            int hour = buf.ReadByte();
            int minute = buf.ReadByte();
            int sec = buf.ReadByte();

            if (year > 99)
            {
                return DateTime.MinValue;
            }
            if (year1 > 99)
            {
                return DateTime.MinValue;
            }
            if (month == 0 || month > 12)
            {
                return DateTime.MinValue;
            }
            if (day == 0 || day > 31)
            {
                return DateTime.MinValue;
            }
            if (hour > 23)
            {
                return DateTime.MinValue;
            }

            if (minute > 59)
            {
                return DateTime.MinValue;
            }
            if (sec > 59)
            {
                return DateTime.MinValue;
            }

            DateTime dTime = new DateTime(year * 100 + year1, month, day, hour, minute, sec);
            return dTime;
        }



        /// <summary>
        /// 将日期转换为5个字节BCD格式的数据，并写入到Buf中；yyMMddhhmm
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="date"></param>
        public static void DateToBCD_yyMMddhhmm(IByteBuffer buf, DateTime date)
        {
            if (date == null)
            {
                for (int i = 0; i < 5; i++)
                {
                    buf.WriteByte(0);
                }
            }
            else
            {
                int iWriteIndex = buf.WriterIndex;
                buf.WriteByte(date.Year - 2000);
                buf.WriteByte(date.Month);
                buf.WriteByte(date.Day);
                buf.WriteByte(date.Hour);
                buf.WriteByte(date.Minute);

                ByteUtil.ByteToBCD(buf, iWriteIndex, 5);
            }
        }

        /// <summary>
        /// 将日期转换为3个字节BCD格式的数据，并写入到Buf中；yyMMdd
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="date"></param>
        public static void DateToBCD_yyMMdd(IByteBuffer buf, DateTime date)
        {
            if (date == null)
            {
                for (int i = 0; i < 5; i++)
                {
                    buf.WriteByte(0);
                }
            }
            else
            {
                int iWriteIndex = buf.WriterIndex;
                buf.WriteByte(date.Year - 2000);
                buf.WriteByte(date.Month);
                buf.WriteByte(date.Day);

                ByteUtil.ByteToBCD(buf, iWriteIndex, 3);
            }
        }

        public static void DateToBCD_yyMMddhhmmss(byte[] btData, DateTime date)
        {
            if (date == null)
            {
                for (int i = 0; i < 6; i++)
                {
                    btData[i] = 0;
                }
            }
            else
            {
                btData[0] = (byte)(date.Year - 2000);
                btData[1] = (byte)(date.Month);
                btData[2] = (byte)date.Day;
                btData[3] = (byte)date.Hour;
                btData[4] = (byte)date.Minute;
                btData = ByteUtil.ByteToBCD(btData);
            }
        }

        /**
         * 将时间类型格式化为 yyyy-MM-dd HH:mm:ss
         *
         * @param date 需要格式化的时间
         * @return 时间字符串
         */
        public static String FormatTime(DateTime date)
        {
            if (date == null)
            {
                return "";
            }
            string format = date.ToString("yyyy-MM-dd HH:mm:ss");
            return format;
        }

        /**
         * 将时间类型格式化为 yyyy-MM-dd HH:mm
         *
         * @param date 需要格式化的时间
         * @return 时间字符串
         */
        public static String FormatTimeHHmm(DateTime date)
        {
            if (date == null)
            {
                return "";
            }
            string format = date.ToString("yyyy-MM-dd HH:mm");
            return format;
        }

        public static String FormatTimeMillisecond(DateTime date)
        {
            if (date == null)
            {
                return "";
            }
            string format = date.ToString("HH:mm:ss.sss");
            return format;
        }

        /// <summary>
        /// BCD格式日期字节数组转换为日期类型
        /// </summary>
        /// <param name="btTime"></param>
        /// <returns></returns>
        public static DateTime BCDTimeToDate_ssmmHHddMMWWyy(IByteBuffer buf)
        {
            buf = ByteUtil.BCDToByte(buf, buf.ReaderIndex, 7);

            int sec = buf.ReadByte();
            int minute = buf.ReadByte();
            int hour = buf.ReadByte();
            int day = buf.ReadByte();
            int month = buf.ReadByte();
            buf.ReadByte();
            int year = buf.ReadByte();

            if (year > 99)
            {
                return DateTime.Now;
            }
            if (month == 0 || month > 12)
            {
                return DateTime.Now;
            }
            if (day == 0 || day > 31)
            {
                return DateTime.Now;
            }
            if (hour > 23)
            {
                return DateTime.Now;
            }

            if (minute > 59)
            {
                return DateTime.Now;
            }
            if (sec > 59)
            {
                return DateTime.Now;
            }

            DateTime dTime = new DateTime(2000 + year, month, day, hour, minute, sec);
            return dTime;
        }
    }
}
