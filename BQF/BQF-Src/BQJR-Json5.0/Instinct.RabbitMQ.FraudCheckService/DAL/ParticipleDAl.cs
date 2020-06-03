using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.FraudCheckService.DAL
{
    /// <summary>
    /// 分词DAL
    /// </summary>
    public class ParticipleDAl
    {
        /// <summary>
        /// 获取表结构
        /// </summary>
        /// <param name="tableid"></param>
        /// <returns></returns>
        public static DataTable GetTableLayoutInfo(int tableid)
        {
            DataTable dt = null;
            string spname = "USP_Common_Layout_Select";
            try
            {
                System.Data.SqlClient.SqlParameter pCategoryNumber = new System.Data.SqlClient.SqlParameter("@CategoryNumber",tableid);
                dt = Util.SqlHelper.ExecuteDataTable(Util.GlobalVariable.CnnString,CommandType.StoredProcedure, spname, new System.Data.SqlClient.SqlParameter[] { pCategoryNumber });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="sAppKey"></param>
        /// <param name="xml"></param>
        /// <param name="database"></param>
        public static void ExecuteParticiple(string sAppKey,string xml,string database)
        {
            string spname = "USP_FM_Participle_Add";
            try
            {
                System.Data.SqlClient.SqlParameter pAppKey = new System.Data.SqlClient.SqlParameter("@AppKey",sAppKey);
                System.Data.SqlClient.SqlParameter pContent = new System.Data.SqlClient.SqlParameter("@Doc", SqlDbType.Xml);
                pContent.Value = xml;
                System.Data.SqlClient.SqlParameter pDatabase = new System.Data.SqlClient.SqlParameter("@Database", database);
                Util.SqlHelper.ExecuteNonQuery(Util.GlobalVariable.CnnString, CommandType.StoredProcedure, spname, new System.Data.SqlClient.SqlParameter[] { pAppKey, pContent, pDatabase });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
