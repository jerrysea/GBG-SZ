using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instinct.RabbitMQ.FraudCheckWinService.DAL
{
    /// <summary>
    /// Fraud Check 线程
    /// </summary>
    public class InstinctFraudCheckThread
    {
        #region 属性
        /// <summary>
        /// 配置文件对象
        /// </summary>
        private ClsOnlineServiceCall.SetINIValue clsSetINIValue;
        /// <summary>
        /// 记录日志的函数
        /// </summary>
        public ClsOnlineServiceCall.InstinctServiceOnline.dlgWriteIISLong LogMSG;
        #endregion

        #region 构造
        public InstinctFraudCheckThread(ClsOnlineServiceCall.SetINIValue value,ClsOnlineServiceCall.InstinctServiceOnline.dlgWriteIISLong dlg)
        {
            this.clsSetINIValue = value;            
            
            this.LogMSG = dlg;
        }
        #endregion

        #region 公有函数
        /// <summary>
        /// 欺诈检查接口
        /// </summary>
        /// <param name="inputString">文本格式，"|"隔开</param>
        /// <returns></returns>
        public string InstinctFraudCheck_String(string inputString)
        {
            ClsOnlineServiceCall.InstinctServiceOnline clsOnlineCall = new ClsOnlineServiceCall.InstinctServiceOnline(inputString.Trim(), LogMSG);
            return clsOnlineCall.GetOutputStr;
        }
        /// <summary>
        /// 欺诈检查接口
        /// </summary>
        /// <param name="inputXMLString">XML格式</param>
        /// <returns></returns>
        public string InstinctFraudCheck_XMLString(string inputXMLString)
        {

            ClassXMLParse.ClsXMLParse clsParse = default(ClassXMLParse.ClsXMLParse);           
            ClsOnlineServiceCall.InstinctServiceOnline clsOnlineCall = default(ClsOnlineServiceCall.InstinctServiceOnline);
            bool bOutputUser = default(bool);
            bool bOutputRules = default(bool);
            bool bOutputRulesDescription = default(bool);
            bool bOutputActionCountNumber = default(bool);
            bool bOutputNatureOfFraud = default(bool);
            bool bOutputDiary = default(bool);
            bool bOutputDecisionReason = default(bool);
            bool bOutputUserDefinedAlert = default(bool);
            bool bOutputFraudAlertUserId = default(bool);
            string sInputString = "";

            clsParse = new ClassXMLParse.ClsXMLParse();            
            if (clsSetINIValue.SiteWithSpecialFunctions == "HLB")
            {
                sInputString = System.Convert.ToString(clsParse.GetInstinctInputStringForSpecificSite(inputXMLString, "HLB"));
            }
            else
            {
                sInputString = System.Convert.ToString(clsParse.GetInstinctInputString(inputXMLString));
            }

            clsOnlineCall = new ClsOnlineServiceCall.InstinctServiceOnline(sInputString, LogMSG);

            if (clsSetINIValue.UserIdInOutputFile.Trim() == "Y")
            {
                bOutputUser = true;
            }
            else
            {
                bOutputUser = false;
            }

            if (clsSetINIValue.RulesInOutputFile.Trim() == "Y")
            {
                bOutputRules = true;
            }
            else
            {
                bOutputRules = false;
            }

            if (clsSetINIValue.RulesDescriptionInOutputFile.Trim() == "Y")
            {
                bOutputRulesDescription = true;
            }
            else
            {
                bOutputRulesDescription = false;
            }

            if (clsSetINIValue.ActionCountNumberInOutputFile.Trim() == "Y")
            {
                bOutputActionCountNumber = true;
            }
            else
            {
                bOutputActionCountNumber = false;
            }

            if (clsSetINIValue.NatureOfFraudInOutputFile.Trim() == "Y")
            {
                bOutputNatureOfFraud = true;
            }
            else
            {
                bOutputNatureOfFraud = false;
            }

            if (clsSetINIValue.DecisionReasonInOutputFile.Trim() == "Y")
            {
                bOutputDecisionReason = true;
            }
            else
            {
                bOutputDecisionReason = false;
            }

            if (clsSetINIValue.UserDefinedAlertInOutputFile.Trim() == "Y")
            {
                bOutputUserDefinedAlert = true;
            }
            else
            {
                bOutputUserDefinedAlert = false;
            }

            if (clsSetINIValue.DiaryInOutputFile.Trim() == "Y")
            {
                bOutputDiary = true;
            }
            else
            {
                bOutputDiary = false;
            }

            if (clsSetINIValue.SiteWithSpecialFunctions.Trim().ToUpper() == "SPDB" && clsSetINIValue.FraudAlertUserId.Trim() == "Y")
            {
                bOutputFraudAlertUserId = true;
            }
            else
            {
                bOutputFraudAlertUserId = false;
            }

            clsParse.TriggeredRulesDefinitions = clsOnlineCall.TriggeredRulesDefinitions;

            return clsParse.GetOutputXMLString(clsOnlineCall.GetOutputStr, bOutputUser, bOutputRules, bOutputRulesDescription, bOutputDecisionReason, bOutputUserDefinedAlert, bOutputActionCountNumber, bOutputNatureOfFraud, bOutputDiary, clsSetINIValue.SiteWithSpecialFunctions);
        }
        /// <summary>
        /// 提交最终判定接口
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public string SubmitApplication_String(string inputString)
        {

            //ClsOnlineServiceCall.SetINIValue clsSetINIValue = default(ClsOnlineServiceCall.SetINIValue);
            ClsOnlineServiceCall.InstinctServiceOnline clsOnlineCall = default(ClsOnlineServiceCall.InstinctServiceOnline);

            //clsSetINIValue = new ClsOnlineServiceCall.SetINIValue();
            //SetConfigValues(ref clsSetINIValue);
            //********************* Hugh Start 22/02/2011,TFS 6452 (1.2.3 - Parameterise Field Retention - Online Service) *********************************'
            //The selected fields should be retained upon criminal replacement
            //(New Function) To copy the old value back if possibel
            //For SubmitApplication_String, the USP_Common_Applications_Add called by
            //   USP_Common_Applications_LoadAndFraudCheck, USP_Common_Applications_Add also called by batch servcie I file function.
            //**************************************************************************************************************************************************
            clsOnlineCall = new ClsOnlineServiceCall.InstinctServiceOnline(inputString, LogMSG);

            string returnStr = "";
            returnStr = System.Convert.ToString(clsOnlineCall.GetOutputStr);

            if (string.IsNullOrEmpty(returnStr))
            {
                return "";
            }
            else
            {
                if (returnStr.ToUpper().IndexOf("BCB") != -1)
                {
                    //Below is only for CIMB to remove the last vertical bar
                    return returnStr.Substring(0, returnStr.Length - 1);
                }
                else
                {
                    return returnStr;
                }
            }

        }
        #endregion

        #region 私有函数
        
        #endregion
    }
}
