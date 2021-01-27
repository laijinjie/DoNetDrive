using System;
using System.Collections.Generic;
using System.IO;

using System.Linq;
using System.Text;
using DoNetDrive.Common.Extensions;


namespace DoNetDrive.Common
{
    /// <summary>
    /// string 转换工具类
    /// </summary>
    public class StringUtil
    {
        /// <summary>
        /// 回车换行符
        /// </summary>
        public const string StringCrLf = "\r\n";
        /// <summary>
        /// 回车换行符
        /// </summary>
        public const string StringNewLine = "\r\n";

        /// <summary>
        /// ASCII编码
        /// </summary>
        public static Encoding ASCII = Encoding.ASCII;
        /// <summary>
        /// 字节转十六进制时十六进制字符串代码表
        /// </summary>
        private static byte[] mHexDigits;
        /// <summary>
        /// 十六进制转字节，十六进制每个字符对应的码值表
        /// </summary>
        private static byte[] mByteDigits;

        /// <summary>
        /// 初始化十六进制转换字典
        /// </summary>
        static StringUtil()
        {
            mHexDigits = ASCII.GetBytes("0123456789ABCDEF");


            byte[] bTmp;
            int iLen, i;
            mByteDigits = new byte[256];
            bTmp = ASCII.GetBytes("0123456789ABCDEF");
            iLen = bTmp.Length;
            for (i = 0; i < iLen; i++)
                mByteDigits[bTmp[i]] = (byte)i;
            bTmp = ASCII.GetBytes("abcdef");
            iLen = bTmp.Length;
            for (i = 0; i < iLen; i++)
                mByteDigits[bTmp[i]] = (byte)(i + 10);
        }



        /// <summary>
        /// 字节转十六进制
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ByteToHex(byte[] bData)
        {
            byte[] bHex;
            int i;
            int lIndex;

            if (bData.Length == 0)
                return "00";
            int ilen = bData.Length;
            // 初始化操作
            bHex = new byte[ilen * 2];

            // 开始转换
            lIndex = 0;
            for (i = 0; i < ilen; i++)
            {
                bHex[lIndex] = mHexDigits[bData[i] / 16]; lIndex = lIndex + 1;
                bHex[lIndex] = mHexDigits[bData[i] % 16]; lIndex = lIndex + 1;
            }

            return ASCII.GetString(bHex).TrimEnd('\0');
        }

        /// <summary>
        /// 十六进制转字节
        /// </summary>
        /// <param name="sHex">十六进制字符串</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] HexToByte(string sHex)
        {
            // 十六进制转字节
            long i;
            byte[] bBuf;
            bBuf = new byte[1];
            if (string.IsNullOrWhiteSpace(sHex))
                return bBuf;

            int iHexlen = sHex.Length;
            if (iHexlen % 2 == 1)
                sHex = "0" + sHex;

            // 开始转换
            bBuf = ASCII.GetBytes(sHex);
            byte iData;
            int iBufLen = bBuf.Length;
            long lIndex;
            byte[] bRtn;
            bRtn = new byte[iBufLen / 2];

            lIndex = 0;
            for (i = 0; i < iBufLen; i++)
            {
                iData = (byte)((int)mByteDigits[bBuf[i]] * 16); // 高位
                i = i + 1;
                iData = (byte)(iData + mByteDigits[bBuf[i]]); // 低位
                bRtn[lIndex] = iData;
                lIndex = lIndex + 1;
            }
            return bRtn;
        }


        /// <summary>
        /// 检查字符串是否为Ascii字符
        /// </summary>
        /// <param name="asciiString">待检查的字符串</param>
        /// <returns>true 表示都是ascii组成，false 表示有包含不是ascii的字符串</returns>
        public static bool IsAscii(string asciiString)
        {
            if (string.IsNullOrWhiteSpace( asciiString))
            {
                return false;
            }
            byte[] buf = System.Text.Encoding.Default.GetBytes(asciiString);
            int ilen = buf.Length;
            for (int i = 0; i < ilen; i++)
            {
                if (buf[i] < 32 || buf[i] > 125)
                {
                    return false;
                }
            }
            return true;
        }



        /// <summary>
        /// 检查字符串是否为十六进制
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsHex(string sValue)
        {
            if (string.IsNullOrWhiteSpace(sValue))
                return false;
            sValue = sValue.ToUpper();
            string sFormat = "1234567890ABCDEF";
            for (int i = 0; i <= sValue.Length - 1; i++)
            {
                if (sFormat.IndexOf(sValue.Substring(i, 1)) == -1)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 检查是否数值类型
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static bool IsNum(string sValue)
        {
            if (string.IsNullOrWhiteSpace(sValue))
                return false;

            string sFormat = "1234567890";
            for (int i = 0; i <= sValue.Length - 1; i++)
            {
                if (sFormat.IndexOf(sValue.Substring(i, 1)) == -1)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///  检查十六进制字符串格式及长度
        ///  </summary>
        ///  <returns></returns>
        ///  <remarks></remarks>
        public static string CheckHexLen(string sHex, int iLen)
        {
            sHex = sHex.ToUpper();
            if (sHex.Length > iLen)
                sHex = sHex.Substring(0, iLen);

            if (sHex.Length < iLen)
                sHex = sHex + new string('F', iLen - sHex.Length);

            if (!IsHex(sHex))
                sHex = new string('F', iLen);

            return sHex;
        }

        /// <summary>
        ///  检查字符串是否为指定长度
        ///  </summary>
        ///  <param name="sValue">待检查字符串</param>
        ///  <param name="iLength">需要的长度</param>
        ///  <param name="sNullString"></param>
        ///  <returns></returns>
        ///  <remarks></remarks>
        public static string CheckStringLen(string sValue, int iLength, char sNullString = ' ')
        {
            if (string.IsNullOrEmpty(sValue))
                sValue = new string(sNullString, iLength);
            if (sValue.Length > iLength)
                sValue = sValue.Substring(0, iLength);
            if (sValue.Length < iLength)
                sValue = sValue + new string(sNullString, iLength - sValue.Length);

            if (sValue.Length > iLength)
                sValue = sValue.Substring(0, iLength);

            return sValue;
        }

        /// <summary>
        ///  检查字符串列表
        ///  </summary>
        ///  <param name="strList">需要检查的字符串列表，包含已逗号分隔的字符串</param>
        ///  <param name="iStrLen">单个字符串的最大长度</param>
        ///  <param name="strIsNum">单个字符串是否必须为数字</param>
        ///  <returns></returns>
        public static string CheckStringList(string strList, int iStrLen, bool strIsNum)
        {
            var strs = strList.Split(',');
            // 检查格式，并重新打包
            var sRetList = new System.Text.StringBuilder(System.Convert.ToInt32(strs.Length * iStrLen * 1.2));
            int iStrCount = 0;
            bool bCheck;

            for (int i = 0; i <= strs.Length - 1; i++)
            {
                bCheck = true;
                if (strs[i].Length > iStrLen)
                    bCheck = false;

                if (bCheck)
                {
                    if (string.IsNullOrEmpty(strs[i]))
                        bCheck = false;
                }

                if (bCheck)
                {
                    if (strIsNum)
                    {
                        if (!strs[i].IsNum())
                            bCheck = false;
                    }
                }


                if (bCheck)
                {
                    if (strs[i].IndexOf("'") >= 0)
                        strs[i].Replace("'", string.Empty);
                    sRetList.Append("'");
                    sRetList.Append(strs[i]);
                    sRetList.Append("'");
                    sRetList.Append(",");
                    iStrCount += 1;
                }

                if (iStrCount > 500)
                    break;
            }
            if (sRetList.Length > 0)
                sRetList.Length -= 1;

            if (sRetList.Length > 0)
                return sRetList.ToString();
            else
                return string.Empty;
        }

        /// <summary>
        /// 检查ID列表，删除表中的id为1的段落
        /// </summary>
        /// <param name="IDList"></param>
        /// <returns></returns>
        public static string CheckIDList(string IDList)
        {
            var IDs = IDList.Split(',');
            // 检查格式，并重新打包
            var sIDList = new System.Text.StringBuilder(IDs.Length * 5);
            for (int i = 0; i <= IDs.Length - 1; i++)
            {
                if (IDs[i] == "1")
                    IDs[i] = "0";
                else if (IDs[i].ToInt32() == 0)
                    IDs[i] = "0";

                if (IDs[i] != "0")
                {
                    sIDList.Append(IDs[i]);
                    sIDList.Append(",");
                }
            }
            if (sIDList.Length > 0)
                sIDList.Length -= 1;

            if (sIDList.Length > 0)
                return sIDList.ToString();
            else
                return string.Empty;
        }

        /// <summary>
        /// 检查ID列表
        /// </summary>
        /// <param name="IDList"></param>
        /// <returns></returns>
        public static string CheckIDListNoDelete(string IDList)
        {
            if (IDList == null)
            {
                return string.Empty;
            }
            var IDs = IDList.Split(',');
            // 检查格式，并重新打包
            var sIDList = new System.Text.StringBuilder(IDs.Length * 5);
            int iIDCount = 0;

            for (int i = 0; i <= IDs.Length - 1; i++)
            {
                if (IDs[i].ToInt32() == 0)
                    IDs[i] = "0";

                if (IDs[i] != "0")
                {
                    sIDList.Append(IDs[i]);
                    sIDList.Append(",");
                    iIDCount += 1;
                }

                if (iIDCount > 500)
                    break;
            }
            if (sIDList.Length > 0)
                sIDList.Length -= 1;

            if (sIDList.Length > 0)
                return sIDList.ToString();
            else
                return string.Empty;
        }

        /// <summary>
        /// 检查ID列表
        /// </summary>
        /// <param name="IDList"></param>
        /// <returns></returns>
        public static string CheckStrListNoDelete(string IDList)
        {
            var IDs = IDList.Split(',');
            // 检查格式，并重新打包
            var sIDList = new System.Text.StringBuilder(IDs.Length * 5);
            int iIDCount = 0;

            for (int i = 0; i <= IDs.Length - 1; i++)
            {

                if (IDs[i] != "")
                {
                    sIDList.Append("'");
                    sIDList.Append(IDs[i]);
                    sIDList.Append("'");
                    sIDList.Append(",");
                    iIDCount += 1;
                }

                if (iIDCount > 500)
                    break;
            }
            if (sIDList.Length > 0)
                sIDList.Length -= 1;

            if (sIDList.Length > 0)
                return sIDList.ToString();
            else
                return string.Empty;
        }


        /// <summary>
        /// Base64字符串  
        /// </summary>
        /// <param name="source">原字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string source)
        {
            string enString = "";
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            try
            {
                enString = Convert.ToBase64String(bytes);
            }
            catch
            {
                enString = source;
            }
            return enString;
        }
    }
}
