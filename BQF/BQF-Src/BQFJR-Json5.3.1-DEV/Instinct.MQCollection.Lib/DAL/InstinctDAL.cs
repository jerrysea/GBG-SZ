using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Instinct.Collection.Lib.DAL
{
    public class InstinctDAL
    {
        #region 属性
        private string dbcnn;

        private Dictionary<string, string> applicationFields;
        private Dictionary<string, Dictionary<string, string>> subTableFields;
        private ArrayList subTables;
        private Hashtable tableMap;

        public delegate void WriteException(string title, Exception ex);
        public delegate void WriteInfo(string msg);

        WriteException ErrorLog;
        WriteInfo InfoLog;
        #endregion

        #region 构造
        public InstinctDAL(string cnn,ArrayList subTableValues,WriteException logerror,WriteInfo loginfo)
        {
            this.dbcnn = cnn;
            this.subTables = subTableValues;
            this.ErrorLog = logerror;
            this.InfoLog = loginfo;
            Init();
        }
        #endregion

        #region 公有函数
        
        #region Application 数据处理
        /// <summary>
        /// 更新处理
        /// </summary>
        /// <param name="inputxml"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool ProcessInputXml(string inputxml,string database="A")
        {
            bool bRet = true;
            XmlDocument inputdoc = null;
            string path = "/Result/Application";
            try
            {   
                inputdoc = Util.XMLProcess.XMLLoad(inputxml);
                XmlNodeList results = Util.XMLProcess.ReadAll(path, inputdoc);
                
                StringBuilder errMsg = new StringBuilder();
                if (results != null && results.Count > 0)
                {
                    foreach (XmlNode result in results)
                    {
                        string sAppKey = string.Empty;

                        if (ValidateXMLNode(result, ref errMsg))
                        {
                            string appMsg = string.Empty;
                            string sSql = GenSQLByNode(result, out sAppKey,database);
                            InfoLog(string.Format("SQL={0}",sSql));
                            if (Exists(sAppKey, ref appMsg,database))
                            {
                                if (RunUpdate(sSql, ref appMsg))
                                {
                                    WriteDiary(sAppKey, "The final audit result has been pushed.",database);
                                }
                                else
                                {
                                    errMsg.Append(appMsg);
                                    bRet = false;
                                }
                            }
                            else
                            {
                                if (appMsg == "")
                                    errMsg.Append(string.Format("The application [{0}] does not exist.",sAppKey ));
                                else
                                    errMsg.Append(appMsg);
                                bRet = false;
                            }
                        }
                        else
                        {
                            bRet = false;
                        }
                    }

                    if (errMsg.Length > 0)
                    {
                        ErrorLog("Update", new Exception(errMsg.ToString()));
                        bRet = false;
                    }
                }
                else
                {
                    InfoLog("The count of final audit results is 0. ");
                    InfoLog("The root path should be \"/Result/Application\".");
                    bRet = false;
                }
            }
            catch (Exception ex)
            {
                bRet = false;
                ErrorLog("ProcessInputXml", ex);
            }

            return bRet;
        }

        /// <summary>
        /// 初始化字段类型
        /// </summary>
        public void Init()
        {
            //table map
            this.tableMap = new Hashtable();
            this.tableMap.Add("Application",1);
            this.tableMap.Add("Applicant", 2);
            this.tableMap.Add("A_Accountant_Solicitor", 3);
            this.tableMap.Add("Guarantor", 4);
            this.tableMap.Add("Introducer_Agent", 5);
            this.tableMap.Add("Reference", 6);
            this.tableMap.Add("Security", 7);
            this.tableMap.Add("PreviousAddress_Company", 8);
            this.tableMap.Add("Valuer", 9);
            this.tableMap.Add("User", 10);
            this.tableMap.Add("CreditBureau", 11);
            this.tableMap.Add("User2", 12);
            this.tableMap.Add("UCA", 13);
            this.tableMap.Add("CBA", 14);
            this.tableMap.Add("U2A", 15);
            this.tableMap.Add("UCB", 16);
            this.tableMap.Add("CBB", 17);
            this.tableMap.Add("U2B", 18);
            this.tableMap.Add("UCC", 19);
            this.tableMap.Add("CBC", 20);
            this.tableMap.Add("U2C", 21);

            //组装主表属性
            this.applicationFields = GetApplicationLayout(Convert.ToInt32(this.tableMap["Application"]));
            this.subTableFields = new Dictionary<string, Dictionary<string, string>>();
            //组装子表属性
            if (this.subTables != null && this.subTables.Count > 0)
            {                
                foreach (string tabname in this.subTables)
                {
                    if (this.tableMap.ContainsKey(tabname))
                    {
                        int tabid = Convert.ToInt32 (this.tableMap[tabname]);
                        Dictionary<string, string> tabattribute = GetApplicationLayout(tabid);
                        this.subTableFields.Add(tabname, tabattribute);
                    }
                    else
                    {
                        throw new Exception(string.Format("TABLE NAME[{0}] DOES NOT EXISTS IN INSTINCT.",tabname ));
                    }
                }
            }
        }
        #endregion

        #endregion

        #region 私有函数
        #region 验证
        /// <summary>
        /// check the field type and appkey exists
        /// </summary>
        /// <param name="appnode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool ValidateXMLNode(XmlNode appnode, ref StringBuilder errMsg)
        {
            bool bRet = true;

            bool bOrg = false;
            bool bCn = false;
            bool bAppNo = false;
            bool bAppType = false;
            string sOrg = string.Empty;
            string sCN = string.Empty;
            string sAppNumber = string.Empty;
            string sAppType = string.Empty;

            if (appnode == null || appnode.ChildNodes.Count == 0)
            {
                bRet = false;
                errMsg.Append("XML NODE is empty.");
            }
            else
            {
                XmlNodeList fields = appnode.ChildNodes;
                foreach (XmlNode field in fields)
                {
                    string fieldname = "";
                    string fieldvalue = "";
                    if (Util.XMLProcess.HasChildNodes(field))
                    {
                        fieldname = field.Name.Trim();
                        Dictionary<string,string> tableattribute=null;
                        if (this.subTableFields.ContainsKey(fieldname))
                        {
                            tableattribute = this.subTableFields[fieldname];
                            foreach (XmlNode subfield in field.ChildNodes)
                            {
                                if (Util.XMLProcess.HasChildNodes(subfield))
                                {
                                    bRet = false;
                                    errMsg.Append("THE CURRENT NODE[{0}] HAS ERROR LAYOUT,WHICH SHOULD NOT HAS SON .");
                                }
                                else
                                {
                                    string subfieldname = subfield.Name.Trim().ToUpper();
                                    string subfiledvalue = subfield.InnerText.Trim();
                                    if (tableattribute.ContainsKey(subfieldname))
                                    {
                                        string subfieldtype = tableattribute[subfieldname];
                                        bRet = validateField(fieldname,subfieldname, subfiledvalue, subfieldtype, ref errMsg);
                                    }
                                }
                            }
                        }
                        else
                        {
                            bRet = false;
                            errMsg.Append(string.Format("THE CURRENT NODE[{0}] IS NOT DEFINED.",fieldname ));
                        }
                    }
                    else
                    {
                        fieldname = field.Name.Trim().ToUpper();
                        fieldvalue = field.InnerText.Trim();
                        if (applicationFields.ContainsKey(fieldname))
                        {
                            string fieldtype = applicationFields[fieldname];
                            bRet = validateField("Application",fieldname, fieldvalue, fieldtype, ref errMsg);

                            if (fieldname == "ORGANISATION")
                            {
                                bOrg = true;
                                sOrg = fieldvalue;
                            }

                            if (fieldname == "COUNTRY_CODE")
                            {
                                bCn = true;
                                sCN = fieldvalue;
                            }

                            if (fieldname == "APPLICATION_NUMBER")
                            {
                                bAppNo = true;
                                sAppNumber = fieldvalue;
                            }

                            if (fieldname == "APPLICATION_TYPE")
                            {
                                bAppType = true;
                                sAppType = fieldvalue;
                            }
                        }
                        else
                        {
                            errMsg.Append(string.Format("The field[{0}] is not defined.", fieldname));
                            bRet = false;
                        }
                    }                    
                }
                //check if the primary key exists
                if (!(bOrg && bCn && bAppNo && bAppType))
                {
                    errMsg.Append("The APPKEY does not exist.");
                    bRet = false;
                }
                if (sOrg + sCN + sAppNumber + sAppType == "")
                {
                    errMsg.Append("The APPKEY does not exist.");
                    bRet = false;
                }
            }
            return bRet;
        }
        /// <summary>
        /// 验证字段
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fieldvalue"></param>
        /// <param name="fieldtype"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool validateField(string tabname,string fieldname,string fieldvalue,string fieldtype,ref StringBuilder errMsg)
        {
            bool bRet = true;
            if (fieldtype == "BIGINT" && fieldvalue != null && fieldvalue != "" && !Util.Tool.IsNumberic(fieldvalue))
            {
                errMsg.Append(string.Format("The Field[{0}.{1}]'s value[{2}] is not numeric.",tabname, fieldname, fieldvalue));
                bRet = false;
            }

            if (fieldtype.StartsWith("DATE") && fieldvalue != null && fieldvalue != "" && !Util.Tool.IsDate(fieldvalue))
            {
                errMsg.Append(string.Format("The Field[{0}.{1}]'s value[{1}] is not date.", tabname,fieldname, fieldvalue));
                bRet = false;
            }
            return bRet;
        }
        #endregion

        #region 数据库操作
        /// <summary>
        /// 获取更新最终结果语句
        /// </summary>
        /// <param name="appNode"></param>
        /// <param name="sAppKey"></param>
        /// <returns></returns>
        private string GenSQLByNode(XmlNode appNode, out string sAppKey,string database)
        {
            StringBuilder sSql = new StringBuilder();

            string sOrg = string.Empty;
            string sCN = string.Empty;
            string sAppNumber = string.Empty;
            string sAppType = string.Empty;

            string sUpdateSql = string.Format("UPDATE {0}_APPLICATION WITH(ROWLOCK) ",database);
            string sWhereSql = " WHERE APPKEY='{0}';";
            StringBuilder sbFields = new StringBuilder();
            string sFieldTemplate = " SET {0}='{1}',";
            string sInnerFieldTemplate = " {0}='{1}',";

            ArrayList subSqlList = new ArrayList();

            bool IsApplication = false;

            foreach (XmlNode field in appNode.ChildNodes)
            {
                string fieldname = field.Name.Trim().ToUpper();
                string fieldvalue = field.InnerText.Trim();
                if (Util.XMLProcess.HasChildNodes(field))
                {
                    string subSql = GetSQLBySubNode(field,database);
                    subSqlList.Add(subSql);
                }
                else
                {
                    if (fieldname == "ORGANISATION")
                    {
                        sOrg = fieldvalue;
                    }
                    else if (fieldname == "COUNTRY_CODE")
                    {
                        sCN = fieldvalue;
                    }
                    else if (fieldname == "APPLICATION_NUMBER")
                    {
                        sAppNumber = fieldvalue;
                    }
                    else if (fieldname == "APPLICATION_TYPE")
                    {
                        sAppType = fieldvalue;
                    }
                    else
                    {
                        if (fieldname == "APPLICATION_DATE" || fieldname == "GROUP_MEMBER_CODE")
                        {
                            continue;
                        }
                        IsApplication = true;
                        if (sbFields.Length == 0)
                        {
                            sbFields.Append(string.Format(sFieldTemplate, fieldname, fieldvalue));
                        }
                        else
                        {
                            sbFields.Append(string.Format(sInnerFieldTemplate, fieldname, fieldvalue));
                        }
                    }
                }                
            }

            sAppKey = sOrg + sCN + sAppNumber + sAppType;
            sWhereSql = string.Format(sWhereSql, sAppKey);

            if (IsApplication)
            {
                sSql.Append(sUpdateSql + sbFields.ToString().TrimEnd(',') + sWhereSql);
            }            

            if (subSqlList.Count > 0)
            {
                foreach (string substr in subSqlList)
                {
                    string subssql = substr + string.Format(" AND APPKEY='{0}';", sAppKey);
                    sSql.Append(subssql);
                }
            }

            return sSql.ToString();
        }
        /// <summary>
        /// 获取子节点更新语句
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetSQLBySubNode(XmlNode node,string database)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbSet = new StringBuilder();

            string sInnerFieldTemplate = " {0}='{1}' ";

            string tabname = node.Name;
            if (!tabname.StartsWith("A_"))
            {
                tabname = database+ "_" + tabname;
            }            
            
            foreach (XmlNode field in node.ChildNodes)
            {
                string fieldname = field.Name;
                string fieldvalue = field.InnerText.Trim();
                string fieldwhere = field.Attributes["where"] == null ? "" : field.Attributes["where"].Value.ToString().ToUpper();
                string statement = string.Format(sInnerFieldTemplate, fieldname, fieldvalue);

                if (fieldwhere == "1" || fieldwhere == "TRUE")
                {
                    sbWhere.Append(statement);
                    sbWhere.Append("AND");
                }
                else
                {
                    sbSet.Append(statement);
                    sbSet.Append(",");
                }
            }

            sb.Append("UPDATE ");
            sb.Append(tabname+ " WITH(ROWLOCK) ");
            sb.Append(" SET ");
            sb.Append(sbSet.ToString().TrimEnd(','));
            sb.Append(" WHERE 1=1 ");
            if (sbWhere.Length > 0)
            {
                sb.Append(" AND " + sbWhere.ToString().Trim().TrimEnd("AND".ToCharArray()));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 执行更新脚本
        /// </summary>
        /// <param name="udpateSql"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool RunUpdate(string udpateSql, ref string errMsg)
        {
            bool bRet = true;
            try
            {
                Util.SqlHelper.ExecuteNonQuery(this.dbcnn, udpateSql, null);
            }
            catch (Exception ex)
            {
                bRet = false;
                errMsg = ex.ToString();
            }
            return bRet;
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="sAppKey"></param>
        /// <returns></returns>
        private bool Exists(string sAppKey, ref string errMsg,string database)
        {
            bool bexists = true;
            string sSq = string.Format("select 1 from {1}_Application(nolock) where appkey='{0}'",sAppKey,database);
            try
            {
                int value = Util.SqlHelper.ExecuteScalar<int>(this.dbcnn, sSq, null);
                if (value == 0)
                    bexists = false;
            }
            catch (Exception ex)
            {
                bexists = false;
                errMsg = ex.ToString();
            }
            
            return bexists;
        }
        /// <summary>
        /// 获取Applicaiton 字段结构
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetApplicationLayout(int iCategoryNumber=1)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            string spname = "USP_Common_LayoutAndType_Select";
            SqlParameter p = new SqlParameter("@CategoryNumber", SqlDbType.SmallInt);
            p.Value = iCategoryNumber;

            try
            {
                DataTable dt = Util.SqlHelper.ExecuteDataTable(Util.GlobalVariables.CnnString, CommandType.StoredProcedure, spname, new SqlParameter[] { p });

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string fieldname = row["Category_Field_Name"].ToString();
                        string fieldtype = row["Category_Field_Type"].ToString();
                        fields.Add(fieldname.Trim().ToUpper(), fieldtype.Trim().ToUpper());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fields;
        }
        /// <summary>
        /// Write A Dairy
        /// </summary>
        /// <param name="sAppKey"></param>
        /// <param name="sDiaryNote"></param>
        private void WriteDiary(string sAppKey, string sDiaryNote,string database)
        {
            string spname = "USP_Common_DiaryNotes_Insert";

            SqlParameter pAppKey = new SqlParameter("@AppKey", sAppKey);
            SqlParameter pDiaryType = new SqlParameter("@DiaryType", "S");
            SqlParameter pDiaryNote = new SqlParameter("@DiaryNote", sDiaryNote);
            SqlParameter pUserId = new SqlParameter("@UserId", "SYSTEM");
            SqlParameter pDatabase = new SqlParameter("@Database", database);

            try
            {
                Util.SqlHelper.ExecuteNonQuery(this.dbcnn, CommandType.StoredProcedure, spname, new SqlParameter[] { pAppKey, pDiaryType, pDiaryNote, pUserId, pDatabase });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        
        #endregion
        
        #endregion
    }
}
