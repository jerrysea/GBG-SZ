using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Instinct.Watch.Web
{
    /// <summary>
    /// Summary description for InstinctWatchCheck
    /// </summary>
    [WebService(Namespace = "http://dectechsolutions.com/Instinct")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class InstinctWatchCheck : System.Web.Services.WebService
    {       
        [WebMethod]
        public string GetDataImportingStatus(string batchno)
        {
            string res = string.Empty;
            try
            {
                Bll.DoChecking check = new Bll.DoChecking(batchno);
                check.CheckStatus();
                Bll.Result rs = check.GetStatus();
                res = rs.ToTxtString();
            }
            catch (Exception ex)
            {
                res = "Error";
                Util.LogHelper.ErrorLog("Instinct.Watch.Service", "GetDataImportingStatus. Error = " + ex.Message);
            }
            return res;
        }
    }
}
