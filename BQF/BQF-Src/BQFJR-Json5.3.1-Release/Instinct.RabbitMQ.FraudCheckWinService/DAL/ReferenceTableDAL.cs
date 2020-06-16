//using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using System;

namespace Instinct.RabbitMQ.FraudCheckWinService.DAL
{
    /// <summary>
    /// 引用表DAL
    /// </summary>
    class ReferenceTableDAL
    {

        public static DataTable  GetCompanySuffix()
        {
            try
            {
                return Util.SqlHelper.ExecuteDataTable(Util.GlobalVariable.CnnString, CommandType.StoredProcedure, "USP_Definitions_CompanySuffix_Select");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static DataTable  GetReferenceTableFieldDetails(int iTableId)
        {
            SqlParameter[] arParams = new SqlParameter[1];

            try
            {
                arParams[0] = new SqlParameter("@TableId", SqlDbType.TinyInt);
                arParams[0].Value = iTableId;

                return Util.SqlHelper.ExecuteDataTable(Util.GlobalVariable.CnnString, CommandType.StoredProcedure, "USP_Options_ReferenceTable_FieldDefinition_ByTableId_Select", arParams);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void TruncatePassiveTable(int iTableId)
        {
            SqlParameter[] arParams = new SqlParameter[1];

            try
            {
                arParams[0] = new SqlParameter("@TableId", SqlDbType.TinyInt);
                arParams[0].Value = iTableId;

                Util.SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.StoredProcedure, "USP_Options_ReferenceTable_Truncate", arParams);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void AddToReferenceTable(int iTableId, string sXML)
        {
            SqlParameter[] arParams = new SqlParameter[1];

            try
            {
                arParams[0] = new SqlParameter("@Doc", SqlDbType.Text);
                arParams[0].Value = sXML;

                Util.SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.StoredProcedure, "USP_ReferenceTable_R" + iTableId.ToString("00"), arParams);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void ApplyCalculations(int iTableId)
        {
            try
            {
                Util.SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.Text, "IF EXISTS (SELECT 1 FROM sys.objects WHERE type = \'P\' AND name = \'" + ("USP_ReferenceTable_R" + iTableId.ToString("00") + "_Calculations") + ("\') EXEC USP_ReferenceTable_R" + iTableId.ToString("00") + "_Calculations \'P\'"));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void ApplyCalculations(int iTableId,SqlTransaction tran)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.Text, "IF EXISTS (SELECT 1 FROM sys.objects WHERE type = \'P\' AND name = \'" + ("USP_ReferenceTable_R" + iTableId.ToString("00") + "_Calculations") + ("\') EXEC USP_ReferenceTable_R" + iTableId.ToString("00") + "_Calculations \'P\'"));
                Util.SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "IF EXISTS (SELECT 1 FROM sys.objects WHERE type = \'P\' AND name = \'" + ("USP_ReferenceTable_R" + iTableId.ToString("00") + "_Calculations") + ("\') EXEC USP_ReferenceTable_R" + iTableId.ToString("00") + "_Calculations \'A\'"));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void UpdateInstinctTables(int iTableId)
        {
            SqlParameter[] arParams = new SqlParameter[1];

            try
            {
                arParams[0] = new SqlParameter("@TableId", SqlDbType.TinyInt);
                arParams[0].Value = iTableId;
                Util.SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.StoredProcedure, "USP_Options_ReferenceTable_InstinctTable_Update", arParams);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void UpdateStatus(int iTableId)
        {
            SqlParameter[] arParams = new SqlParameter[1];

            try
            {
                arParams[0] = new SqlParameter("@TableId", SqlDbType.TinyInt);
                arParams[0].Value = iTableId;

                Util.SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.StoredProcedure, "USP_Options_ReferenceTable_Status_Update", arParams);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void UpdateSynonyms(int iTableId)
        {
            SqlParameter[] arParams = new SqlParameter[1];

            try
            {
                arParams[0] = new SqlParameter("@TableId", SqlDbType.TinyInt);
                arParams[0].Value = iTableId;

                Util.SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.StoredProcedure, "USP_Options_ReferenceTable_Synonym_Update", arParams);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public static short GetStaging_Status(int iTableId)
        {
            SqlParameter[] arParams = new SqlParameter[1];
            DataTable  dtcountData = default(DataTable);
            try
            {
                arParams[0] = new SqlParameter("@iTableId", SqlDbType.TinyInt);
                arParams[0].Value = iTableId;

                dtcountData = Util.SqlHelper.ExecuteDataTable(Util.GlobalVariable.CnnString, CommandType.StoredProcedure, "USP_Options_ReferenceTable_StagingDataStatus", arParams);

                if (Convert.ToInt32(dtcountData.Rows[0][0].ToString()) > 0)
                {
                    return (short)1;
                }
                else
                {
                    return (short)0;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void CopyDataFromStagingTable(SqlTransaction tran, string stagingTableName, string table, string fields, string appkey)
        {
            string sql = string.Format("DELETE FROM [DBO].[{0}] WITH(ROWLOCK) WHERE [APPKEY] = '{3}';\nINSERT INTO [DBO].[{0}] WITH(ROWLOCK) ({2}) SELECT {2} FROM [DBO].[{1}](NOLOCK) WHERE [APPKEY] = '{3}';DELETE FROM [DBO].[{1}] WITH(ROWLOCK) WHERE [APPKEY]='{3}'", table, stagingTableName, fields, appkey);

            Util.SqlHelper.ExecuteNonQuery(tran, CommandType.Text, sql);
        }

        public static void CopyDataFromStagingTable(string stagingTableName, string table, string fields, string appkey)
        {
            string sql = string.Format("DELETE FROM [DBO].[{0}] WITH(ROWLOCK) WHERE [APPKEY] = '{3}';\nINSERT INTO [DBO].[{0}] WITH(ROWLOCK) ({2}) SELECT {2} FROM [DBO].[{1}](NOLOCK) WHERE [APPKEY] = '{3}';DELETE FROM [DBO].[{1}] WITH(ROWLOCK) WHERE [APPKEY]='{3}'", table, stagingTableName, fields, appkey);

            Util.SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.Text, sql);
        }
        /// <summary>
        /// 新增引用表数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="table"></param>
        /// <param name="fields"></param>
        /// <param name="appkey"></param>
        public static void InsertReferenceData(DataTable dt, string table, ArrayList fields, string appkey)
        {
            StringBuilder sbSql = new StringBuilder();
            string template = "INSERT INTO [DBO].[{0}] WITH(ROWLOCK) ({1}) VALUES({2});";
            if (dt != null && dt.Rows.Count > 0 && fields!=null && fields.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    StringBuilder sbFields = new StringBuilder();
                    StringBuilder sbValues = new StringBuilder();
                    for(int i= 0;i<fields.Count;i++)
                    {
                        string skey = fields[i].ToString();
                        string svalue = Convert.ToString(row[skey]);
                        sbFields.Append(skey);
                        sbValues.Append(string.Format("'{0}'",svalue));
                        if (i < fields.Count - 1)
                        {                            
                            sbFields.Append(",");
                            sbValues.Append(",");
                        }                        
                    }
                    sbSql.AppendLine(string.Format(template, table, sbFields.ToString(), sbValues.ToString()));
                }
                Util.SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.Text, sbSql.ToString());
            }
            
        }

        public static void DeleteReferenceData(ArrayList dbtablelist,string appkey)
        {
            string sqltemplate = "DELETE FROM [{0}] WITH(ROWLOCK) WHERE AppKey='{1}';";
            StringBuilder delsqllist = new StringBuilder();
            if (dbtablelist != null && dbtablelist.Count > 0)
            {
                foreach (string stable in dbtablelist)
                {
                    string ssql = string.Format(sqltemplate, stable, appkey);
                    delsqllist.Append(ssql);
                }

                Util.SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.Text, delsqllist.ToString());
            }
        }

        public static DataTable GetSynonymsNames(string referenceTableName)
        {
            string sql = string.Format("SELECT LEFT(RIGHT(BASE_OBJECT_NAME,7),6) AS BASE_OBJECT_NAME,NAME FROM SYS.SYNONYMS(NOLOCK) WHERE NAME LIKE '{0}%'", referenceTableName);
            return Util.SqlHelper.ExecuteDataTable(Util.GlobalVariable.CnnString, sql, null);
        }
    }
}
