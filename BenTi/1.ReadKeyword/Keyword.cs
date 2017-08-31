using System.Collections.Generic;
using System;

namespace BenTi
{
    public class Keyword
    {
        public Keyword()
        {
            ID = Guid.NewGuid();
            Name = "";
            Name_PY = "";
            Name_En = "";
            Annotation = "";
        }
        /// <summary>
        /// id
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 主题词名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 主题词拼音
        /// </summary>
        public string Name_PY { get; set; }

        /// <summary>
        /// 主题词英文
        /// </summary>
        public string Name_En { get; set; }

        /// <summary>
        /// 树状编码字符串
        /// </summary>
        public string CodeStr
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var codes = value.Split("，".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                    if (!codes.IsNull())
                    {
                        Codes.AddRange(codes);
                    }
                }
            }
        }

        /// <summary>
        /// 树状编码集合
        /// </summary>

        public List<string> Codes = new List<string>();

        /// <summary>
        /// 用代属分参集合
        /// </summary>

        public List<Keyword_Sub> Subs = new List<Keyword_Sub>();

        /// <summary>
        /// 注释
        /// </summary>
        public string Annotation { get; set; }

        public override string ToString()
        {
            string str = "";
            if (!string.IsNullOrEmpty(Name_PY))
            {
                str += "Name_PY:" + Name_PY + "\r\n";
            }
            if (!string.IsNullOrEmpty(Name))
            {
                str += "Name:" + Name + "\r\n";
            }
            if (!string.IsNullOrEmpty(Name_En))
            {
                str += "Name_En:" + Name_En + "\r\n";
            }
            if (Codes.Count > 0)
            {
                str += "Codes:" + string.Join(",", Codes) + "\r\n";
            }
            if (Subs.Count > 0)
            {
                str += "Subs:\r\n";
                foreach (var item in Subs)
                {
                    str += item.Type + "\t" + item.Name + "\r\n";
                }
            }
            if (!string.IsNullOrEmpty(Annotation))
            {
                str += "Annotation:" + Annotation + "\r\n";
            }
            return str;
        }
    }

    /// <summary>
    /// 关系词
    /// </summary>
    public class Keyword_Sub
    {

        /// <summary>
        /// 用代属分参集合 Y D S F C
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 主题词名
        /// </summary>
        public string Name { get; set; }
    }
}