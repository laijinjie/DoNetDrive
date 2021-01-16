using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNetDrive.Common.Extensions
{
    /// <summary>
    /// 集合扩展类
    /// </summary>
    public static class ListExtensions
    {

        /// <summary>
        /// 集合扩展，把集合转字符串拼接 参数 分割
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <param name="outer"></param>
        /// <param name="sSplit"></param>
        /// <returns></returns>
        public static string Join<TOuter>(this IEnumerable<TOuter> outer, string sSplit = ",")
        {
            if (outer.Count() == 0)
                return string.Empty;

            var sJoin = new StringBuilder(outer.Count() * 10);

            foreach (TOuter o in outer)
            {
                sJoin.Append(o.ToString());
                sJoin.Append(sSplit);
            }
            sJoin.Length -= sSplit.Length;

            return sJoin.ToString();
        }
    }
}
