using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace BenTi_Key
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory;
            filePath = filePath.Substring(0, filePath.IndexOf("bin")) + "\\一次抓取提取.txt";
            if (!File.Exists(filePath))
            {
                return;
            }
            string txtContent = File.ReadAllText(filePath);
            //linkClickHandler(329417)"> 五脏开窍</span>  onclick=\".*?<
            MatchCollection matchs = Regex.Matches(txtContent, @"linkClickHandler\(.*?<", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            StringBuilder strContent = new StringBuilder();
            if (matchs != null && matchs.Count > 0)
            {
                foreach (Match m in matchs)
                {
                    var value = m.Value.TrimEnd('<').Replace("'", "").Replace("\"", "");
                    var newValue = Regex.Replace(value, @"linkClickHandler\(", "", RegexOptions.None);
                    newValue = newValue.Replace(")>", "\t");
                    strContent.AppendLine(newValue);
                    Console.WriteLine(newValue);
                }
            }
            if (strContent.ToString() != "")
            {
                string savePath = filePath.Replace(Path.GetFileNameWithoutExtension(filePath), Path.GetFileNameWithoutExtension(filePath) + "Result");
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
                File.AppendAllText(savePath, strContent.ToString());
            }
            Console.ReadKey();
        }
    }
}
