using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenTi
{
    public static class ExtensionMethod
    {
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this string[] obj)
        {
            if (obj == null || obj.Length <= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trims"></param>
        /// <returns></returns>
        public static string TrimString(this string str, string trims)
        {
            return (str + "").Trim(trims.ToCharArray());
        }

        /// <summary>
        /// 分离数组 并移除空的字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string[] SplitNoNull(this string str, string split)
        {
            return (str + "").Split(split.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
