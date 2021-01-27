using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNetDrive.Common.Extensions
{
    /// <summary>
    /// 数组扩展方法类
    /// </summary>
    public static class ArrayExtensions
    {

        /// <summary>
        /// 拷贝数组中的一个片段到一个新的数组
        /// </summary>
        /// <returns></returns>
        public static byte[] Copy(this byte[] a1, int iSurIndex,int iLen)
        {
            return Arrays<byte>.copyOfRange(a1, iSurIndex,iLen);
        }


        private sealed class FindString
        {
            public string string_0;

            public bool method_0(string string_1)
            {
                return string.Compare(string_1, this.string_0, StringComparison.OrdinalIgnoreCase) == 0;
            }
        }

        /// <summary>
        /// 搜索与指定谓词定义的条件匹配的元素，然后返回整个 System.Array 中第一个匹配项的从零开始的索引
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int FindTextIndex(this string[] array, string value)
        {
            FindString @class = new FindString();
            @class.string_0 = value;
            return Array.FindIndex<string>(array, new Predicate<string>(@class.method_0));
        }
        /// <summary>
        /// 搜索与指定谓词定义的条件匹配的元素，然后返回整个 System.Array 中第一个匹配项的从零开始的索引
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int FindTextIndex(this List<string> list, string value)
        {
            FindString @class = new FindString();
            @class.string_0 = value;
            return list.FindIndex(new Predicate<string>(@class.method_0));
        }
    }
}
