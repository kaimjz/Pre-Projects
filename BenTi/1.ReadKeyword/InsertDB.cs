using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ontology.DAL;
using Ontology.Model;
using System.Data.Common;
using System.Data;

namespace BenTi
{
    public class InsertDB : Base_DAL
    {
        /// <summary>
        /// 添加主题词
        /// </summary>
        /// <param name="thesaurusM"></param>
        /// <param name="subjectList"></param>
        /// <param name="subattrList"></param>
        /// <param name="treeDicList"></param>
        /// <param name="subTreeList"></param>
        /// <returns></returns>
        public bool InsertData(Xml_Thesaurus thesaurusM, List<Xml_Subject_Temp> subjectList,
            List<Xml_SubAttribute_Temp> subattrList, List<Xml_TreeDic> treeDicList,
            List<Xml_SubTree_Temp> subTreeList)
        {
            using (DbConnection conn = new DBFactory().GetInstance())
            {
                conn.Open();
                DbTransaction tran = conn.BeginTransaction();
                try
                {
                    if (!InsertTransaction(thesaurusM, tran, conn))
                    {
                        throw new Exception("主题词表添加失败");
                    }
                    var i = 0;
                    var error = "";
                    foreach (var item in subjectList)
                    {
                        try
                        {

                            if (item.Subject.Length > 50)
                            {
                                i++;
                                error += item.Subject + "\r\n";
                                continue;
                            }
                            if (!InsertTransaction(item, tran, conn))
                            {
                                throw new Exception("主题词添加失败");
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                    foreach (var item in subattrList)
                    {
                        if (!InsertTransaction(item, tran, conn))
                        {
                            throw new Exception("主题词关系拓展表添加失败");
                        }
                    }
                    foreach (var item in treeDicList)
                    {
                        if (!InsertTransaction(item, tran, conn))
                        {
                            throw new Exception("树状结构表添加失败");
                        }
                    }
                    foreach (var item in subTreeList)
                    {
                        if (!InsertTransaction(item, tran, conn))
                        {
                            throw new Exception("主题词树状结构表添加失败");
                        }
                    }
                    //tran.Rollback();
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return false;
                }
            }
        }
    }
}
