using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNetDrive.Common
{
    /// <summary>
    /// 集合工具类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Arrays<T>
    {
        /// <summary>
        /// 检查缓冲区
        /// </summary>
        /// <param name="bBuf">缓冲区</param>
        /// <param name="iBeginIndex">起始索引号</param>
        /// <param name="iLen">长度</param>
        /// <param name="iMax">最大长度</param>
        /// <returns></returns>
        public static bool CheckBuf(Array bBuf, int iBeginIndex, ref int iLen, int iMax)
        {
            int iBufLen;
            if (iLen > iMax)
                iLen = iMax;

            if (bBuf == null)
                return false;
            iBufLen = bBuf.Length;
            if (iBufLen == 0)
                return false;
            if (iBeginIndex < 0 | iBeginIndex > iBufLen)
                return false;
            if (iLen + iBeginIndex > iBufLen)
                return false;
            return true;
        }


        /// <summary>
        /// 拷贝数组
        /// </summary>
        /// <param name="src">源数组，从此数组中取数据</param>
        /// <param name="iBegin">开始截取数组的起始索引，从0开始。</param>
        /// <param name="lLen">截取数组元素的长度。</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T[] copyOfRange(T[] src, int iBegin, int lLen)
        {
            T[] bTmp;
            bTmp = new T[lLen - 1 + 1];
            Array.Copy(src, iBegin, bTmp, 0, lLen);
            return bTmp;
        }


        /// <summary>
        /// 检查两个数组是否匹配
        /// </summary>
        /// <param name="src"></param>
        /// <param name="sDec"></param>
        /// <returns>true--匹配；false--不匹配</returns>
        /// <remarks></remarks>
        public static bool BytesEquals(T[] src, T[] sDec)
        {
            int iCount = 0;
            if (src == null)
            {
                if (sDec == null)
                    return true;
                else
                    return false;
            }

            if (sDec == null)
            {
                if (src == null)
                    return true;
                else
                    return false;
            }

            int i;
            for (i = 0; i <= src.Length - 1; i++)
            {
                if (!src[i].Equals(sDec[i]))
                {
                    iCount += 1;
                    break;
                }
            }

            if (iCount == 0)
                return true;
            else
                return false;
        }

        
        
    }
}
