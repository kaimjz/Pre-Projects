using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.International.Converters.PinYinConverter;
using Ontology.Common;
using Ontology.Model;

namespace BenTi
{
    public class ReadTxt
    {
        private ReadKeyword.LoadPercent percent;
        private string FilePath = string.Empty;
        private List<string> BlockList = new List<string>();//块状主题词文本
        private List<string> BlockList2 = new List<string>();//块状主题词文本2
        private List<Keyword> KeywordList = new List<Keyword>();
        private List<Keyword_Sub> SubList = new List<Keyword_Sub>();

        public ReadTxt(string filePath, ReadKeyword.LoadPercent p)
        {
            percent = p;
            FilePath = filePath;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="filePath"></param>
        public void Execute()
        {
            try
            {

                System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
                if (cache != null)
                {
                    var block = cache.Get("BlockList") as List<string>;
                    var block2 = cache.Get("BlockList2") as List<string>;
                    if (block != null && block2 != null)
                    {
                        BlockList.AddRange(block);
                        BlockList2.AddRange(block2);
                    }
                    else
                    {
                        percent("获得块状主题词文本:0%");
                        GetBlockList(FilePath);//获得块状主题词文本
                        cache.Insert("BlockList", BlockList);
                        cache.Insert("BlockList2", BlockList2);
                    }
                }
                percent("处理块状主题词文本1:0%");
                HandleBlockList();//处理1
                percent("处理块状主题词文本2:0%");
                HandleBlockList2();//处理2
                percent("添加数据库:0%");
                InsertDB();//添加数据库
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常错误:" + ex.Message, "提示");
            }
        }

        /// <summary>
        /// 添加数据库
        /// </summary>
        private void InsertDB()
        {
            var subjectList = new List<Xml_Subject_Temp>();//主题词列表

            var subattrList = new List<Xml_SubAttribute_Temp>();//词间关系

            var treeDicList = new List<Xml_TreeDic>();//结构号表

            var subTreeList = new List<Xml_SubTree_Temp>();//主题词 结构号
            var thesaurusM = new Xml_Thesaurus()
            {
                ID = Guid.NewGuid(),
                Thesauri = "中国药学主题词表",
                CoverPath = "",
                CreateDate = DateTime.Now,
                CreateUserID = Guid.NewGuid()
            };

            var iNumber = 0;
            var iii = 0;
            foreach (var item in KeywordList)
            {
                try
                {
                    iii++;
                    if (iii == (KeywordList.Count / 100) * iNumber || iNumber == 0)
                    {
                        iNumber++;
                        percent("添加数据库:" + iNumber + "%");
                    }
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        continue;
                    }
                    item.Name_PY = string.IsNullOrEmpty(item.Name_PY) ? ToPy(item.Name) : item.Name_PY;
                    var pyIndex = string.IsNullOrEmpty(item.Name_PY) ? "" : item.Name_PY.Substring(0, 1);
                    var stroke = CalculateStroke.GetStrokeCount(item.Name.Substring(0, 1));

                    //主题词表
                    var subjectM = subjectList.Find(p => p.Subject == item.Name);
                    if (subjectM == null)
                    {
                        subjectM = new Xml_Subject_Temp()
                        {
                            ID = Guid.NewGuid(),
                            Subject = item.Name,
                            Subject_en = item.Name_En,
                            Subject_py = item.Name_PY,
                            Subject_pyIndex = pyIndex,
                            Subject_Stroke = stroke,
                            Annotation = item.Annotation,
                            SubjectType = true,//主题词
                            Genre = 1,//正式
                            Status = 0,//未审核
                            CreateDate = DateTime.Now,
                            ThesauriID = thesaurusM.ID
                        };
                        subjectList.Add(subjectM);
                    }
                    //关系词
                    foreach (var sub in item.Subs)
                    {
                        if (string.IsNullOrEmpty(sub.Name))
                        {
                            continue;
                        }
                        if (sub.Name.IndexOf("同义注释") != -1)
                        {
                            subjectList.Remove(subjectM);
                            subjectM.Annotation += "\r\n" + sub.Name;
                            subjectM.Annotation = subjectM.Annotation.TrimString("\r\n");
                            subjectList.Add(subjectM);
                            continue;
                        }
                        var findAttr = subjectList.Find(p => p.Subject == sub.Name);
                        var attrId = Guid.NewGuid();
                        if (findAttr == null)
                        {
                            var subPy = ToPy(sub.Name);
                            var subPyIndex = string.IsNullOrEmpty(subPy) ? "" : subPy.Substring(0, 1);
                            var subStroke = CalculateStroke.GetStrokeCount(item.Name.Substring(0, 1));
                            var subject_subM = new Xml_Subject_Temp()
                            {
                                ID = attrId,
                                Subject = sub.Name,
                                Subject_en = "",
                                Subject_py = subPy,
                                Subject_pyIndex = subPyIndex,
                                Subject_Stroke = subStroke,
                                Annotation = "",
                                SubjectType = false,//关系词
                                Genre = 0,//非正式
                                Status = 0,//未审核
                                CreateDate = DateTime.Now,
                                ThesauriID = thesaurusM.ID
                            };
                            if ((subject_subM.Subject + "").Length > 50 || (subject_subM.Subject + "") == "" ||
                                subject_subM.Subject.IndexOf("PB.010.020.005.178") != -1)
                            {

                            }
                            subjectList.Add(subject_subM);
                        }
                        var subattrM = new Xml_SubAttribute_Temp()
                        {
                            ID = Guid.NewGuid(),
                            SubjectID = subjectM.ID,
                            Subject_SubID = attrId,
                            Subject = sub.Type,
                        };
                        subattrList.Add(subattrM);
                    }
                    //树状编码
                    foreach (var code in item.Codes)
                    {
                        if (string.IsNullOrEmpty(code))
                        {
                            continue;
                        }
                        var parentCode = "";
                        if (code.Length > 5)
                        {
                            parentCode = code.Substring(0, code.LastIndexOf("."));
                        }
                        var codeM = treeDicList.Find(p => p.LevelCode == code);
                        var parentCodeM = treeDicList.Find(p => p.LevelCode == parentCode);
                        if (codeM == null)
                        {
                            var codeLevel = code.SplitNoNull(".") == null ? 1 : code.SplitNoNull(".").Length;
                            codeM = new Xml_TreeDic()
                            {
                                ID = Guid.NewGuid(),
                                LevelCode = code,
                                Level = codeLevel,
                            };
                            treeDicList.Add(codeM);
                        }
                        if (parentCodeM == null)
                        {
                            var parentCodeLevel = parentCode.SplitNoNull(".") == null ? 1 :
                                parentCode.SplitNoNull(".").Length;
                            parentCodeM = new Xml_TreeDic()
                            {
                                ID = Guid.NewGuid(),
                                LevelCode = parentCode,
                                Level = parentCodeLevel,
                            };
                            if (parentCodeLevel != 0)
                            {
                                treeDicList.Add(parentCodeM);
                            }
                        }
                        var subTreeM = new Xml_SubTree_Temp()
                        {
                            ID = Guid.NewGuid(),
                            SubjectID = subjectM.ID,
                            LevelCodeID = codeM.ID,
                            LevelCode = code,
                            Level = codeM.Level,
                            ParentLevelID = parentCodeM.ID,
                            ParentCode = parentCode,
                            CreateDate = DateTime.Now,
                        };
                        subTreeList.Add(subTreeM);
                    }
                    if (subjectM.Subject.IndexOf("PB.010.020.005.178") != -1)
                    {

                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
            percent("添加数据库:100%");

            if (new InsertDB().InsertData(thesaurusM, subjectList, subattrList, treeDicList, subTreeList))
            {
                percent("提取成功");
            }
            else
            {
                percent("提取失败");
            }
        }

        /// <summary>
        /// 处理块状主题词文本1
        /// </summary>
        private void HandleBlockList()
        {
            var iNumber = 0;
            var iii = 0;
            foreach (var item in BlockList)
            {
                iii++;
                if (iii == (BlockList.Count / 100) * iNumber)
                {
                    iNumber++;
                    percent("处理块状主题词文本1:" + iNumber + "%");
                }
                var lines = item.SplitNoNull("\r\n");
                if (lines.IsNull())
                {
                    continue;
                }
                var keyM = new Keyword();

                //获取name py En code
                for (int i = 0; i < lines.Length; i++)
                {
                    var lStr = lines[i].TrimString("\t");
                    if (i == 0)
                    {
                        if (IsPinYin(lStr))
                        {
                            keyM.Name_PY = lStr;
                        }
                        else
                        {
                            keyM.Name = lStr;
                        }
                    }
                    else if (i == 1)
                    {
                        if (lines[0] == lines[1])
                        {
                            keyM.Name_PY = lStr;
                        }
                        if (string.IsNullOrEmpty(keyM.Name))
                        {
                            keyM.Name = lStr;
                        }
                        else if (Regex.IsMatch(lStr, @"^P[A-Z]{1}(\.[0-9])*?"))
                        {
                            keyM.CodeStr = lStr;
                        }
                    }
                    else if (i == 2)
                    {
                        if (Regex.IsMatch(lStr, @"^[a-z-\sA-Z]*?$"))
                        {
                            keyM.Name_En = lStr;
                        }
                        else if (Regex.IsMatch(lStr, @"^P[A-Z]{1}(\.[0-9])*?"))
                        {
                            keyM.CodeStr = lStr;
                        }
                    }
                    else if (i == 3)
                    {
                        if (Regex.IsMatch(lStr, @"^P[A-Z]{1}(\.[0-9])*?"))
                        {
                            keyM.CodeStr = lStr;
                        }
                    }
                }
                //词义注释
                keyM.Annotation = "";
                Match mAnnotation = Regex.Match(item, "词义注释.*?$");
                if (mAnnotation != null && mAnnotation.Success)
                {
                    keyM.Annotation = Regex.Replace(mAnnotation.Value + "", @"词义注释[\w\W]*?", "");
                }
                string nowYDSFC = "";
                string[] subStrs = null;
                foreach (var YDSFC in "Y,D,S,F,C".Split(','))
                {
                    Match msub = Regex.Match(item, YDSFC + @"\t[\s|\S]*?$");
                    if (msub != null && msub.Success)
                    {
                        var strReplace = Regex.Replace(msub.Value + "", @"词义注释[\w\W]*?$", "");
                        subStrs = strReplace.SplitNoNull("\r\n");
                        if (strReplace.IndexOf("词义注释") != -1)
                        {

                        }
                        nowYDSFC = YDSFC;
                        break;
                    }
                }
                if (!subStrs.IsNull())
                {
                    foreach (var sStr in subStrs)
                    {
                        var newsStr = sStr.TrimString("\t");
                        nowYDSFC = newsStr.IndexOf("Y\t") != -1 ? "Y" :
                            newsStr.IndexOf("D\t") != -1 ? "D" :
                            newsStr.IndexOf("S\t") != -1 ? "S" :
                            newsStr.IndexOf("F\t") != -1 ? "F" :
                            newsStr.IndexOf("C\t") != -1 ? "C" : nowYDSFC;
                        if (newsStr.IndexOf(nowYDSFC) != -1)
                        {
                            newsStr = newsStr.TrimStart(nowYDSFC.ToCharArray());
                        }
                        newsStr = newsStr.TrimString("\t");
                        var subM = new Keyword_Sub()
                        {
                            Type = nowYDSFC,
                            Name = newsStr,
                        };
                        if (newsStr.Length > 50)
                        {

                        }
                        keyM.Subs.Add(subM);
                        SubList.Add(subM);
                    }
                }
                //Console.WriteLine(keyM.ToString());
                KeywordList.Add(keyM);
            }
        }

        /// <summary>
        /// 处理块状主题词文本2
        /// </summary>
        private void HandleBlockList2()
        {
            var iNumber = 0;
            var iii = 0;
            foreach (var item in BlockList2)
            {
                iii++;
                if (iii == (BlockList2.Count / 100) * iNumber || iNumber == 0)
                {
                    iNumber++;
                    percent("处理块状主题词文本2:" + iNumber + "%");
                }
                var lStr = item.TrimString("\t");
                var strs = lStr.SplitNoNull("\t");
                if (strs.IsNull() || strs.Length != 2)
                {
                    continue;
                }
                var keyM = new Keyword()
                {
                    Name = strs[0],
                    CodeStr = strs[1]
                };
                //Console.WriteLine(keyM.ToString());
                KeywordList.Add(keyM);
            }
        }

        /// <summary>
        /// 获得块状主题词文本
        /// </summary>
        /// <param name="filePath"></param>
        private void GetBlockList(string filePath)
        {
            var lines = ReadAllLines(filePath);
            if (lines.IsNull())
            {
                return;
            }
            string blockStr = "";
            percent("获取块状主题词文本:0%");
            var iNumber = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == (lines.Length / 100) * iNumber || iNumber == 0)
                {
                    iNumber++;
                    percent("获取块状主题词文本:" + iNumber + "%");
                }
                var lineStr = lines[i];

                var isPinYin = IsPinYin(lineStr);
                if (isPinYin || (i + 1 < lines.Length && lineStr == lines[i + 1] && i != 0) || (i != 0 && lines[i - 1].IndexOf("词义注释") != -1))
                {
                    blockStr = blockStr.TrimString("\r\n");
                    BlockList.Add(blockStr);
                    blockStr = "";
                }
                blockStr += lineStr + "\r\n";
            }
            if (blockStr != "")
            {
                var lastIndex = blockStr.IndexOf("药用动物、植物、矿物");
                if (lastIndex != -1)
                {
                    var lastBlockStr = blockStr.Substring(0, lastIndex).TrimString("\r\n");
                    BlockList.Add(lastBlockStr);
                    blockStr = blockStr.Substring(lastIndex);
                    //最后的前几级  只有名和code
                    var lastList = blockStr.SplitNoNull("\r\n").ToList();
                    BlockList2.AddRange(lastList);
                }
            }
            percent("获取块状主题词文本:100%");
        }

        /// <summary>
        /// 读取文本所有行文本
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string[] ReadAllLines(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                return lines;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// 判断是否是拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsPinYin(string str)
        {
            str = str.Replace("ü", "u").Replace("-", "");
            if (Regex.IsMatch(str, @"^[A-Z]{1}[a-z\s]{1,}$|^[A-Z]{2,}$")
                && str != "D  jiu shi suan qing jia"
                && str != "wei C yin qiao pian" || str == "Vitamin C yin qiao tablet")
            {
                return false;
            }

            var strs = str.SplitNoNull(" ");
            if (strs.IsNull())
            {
                return false;
            }
            var strss = strs.Where(p => (p + "").Length > 1).ToList();
            var pyCount = 0;
            foreach (var s in strss)
            {
                if (ChineseChar.IsValidPinyin(s + "1") || ChineseChar.IsValidPinyin(s + "2") ||
                    ChineseChar.IsValidPinyin(s + "3") || ChineseChar.IsValidPinyin(s + "4"))
                    pyCount++;
            }
            if (pyCount > 1)
            {

            }
            return pyCount > 0;
        }

        /// <summary>
        /// 转化拼音
        /// </summary>
        /// <param name="pyStr"></param>
        /// <returns></returns>
        private string ToPy(string pyStr)
        {
            char[] pyList = pyStr.ToCharArray();
            string reStr = "";
            for (int i = 0; i < pyList.Length; i++)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(pyList[i]);
                    var py = chineseChar.Pinyins[0].TrimString("123456");
                    py = py.Substring(0, 1) + py.Substring(1, py.Length - 1).ToLower();
                    reStr += " " + py;
                }
                catch (Exception ex)
                {
                    reStr += pyList[i];
                }
            }
            return reStr.Trim(' ');
        }
    }
}