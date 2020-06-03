using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.Collection.Lib.Util
{    
    public class Tool
    {
        public static string Space(int count)
        {
            StringBuilder sb = new StringBuilder();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }
        public static bool IsNumberic(object expression)
        {
            bool isnumber;
            double retnum;
            isnumber = Double.TryParse(Convert.ToString(expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retnum);
            return isnumber;
        }

        public static Encoding GetEncoding(string encodename)
        {
            Encoding e = null;
            try
            {
                if (encodename == null || encodename == "")
                    e = Encoding.Default;
                else
                    e = Encoding.GetEncoding(encodename);
            }
            catch (Exception ex)
            {
                e = Encoding.Default;
            }
            return e;
        }

        public static bool IsDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Leave0to9(string sValidatingString, string sExtendedCharacters = "")
        {
            int i = 0;
            int j = 0;
            char[] arrEntendedCharacters = null;
            char[] arrValidatingString = null;

            try
            {
                arrEntendedCharacters = sExtendedCharacters.ToCharArray();
                arrValidatingString = sValidatingString.ToCharArray();
                for (i = 0; i <= arrValidatingString.Length - 1; i++)
                {
                    if (arrValidatingString[i] < '0' || arrValidatingString[i] > '9')
                    {
                        if (sExtendedCharacters != "")
                        {
                            for (j = 0; j <= arrEntendedCharacters.Length - 1; j++)
                            {
                                if (arrEntendedCharacters[j] == arrValidatingString[i])
                                {
                                    break;
                                }
                            }
                            if (j == arrEntendedCharacters.Length)
                            {
                                //arrValidatingString(i) = ""
                                sValidatingString = sValidatingString.Replace(arrValidatingString[i], '\0');
                            }
                        }
                        else
                        {
                            sValidatingString = sValidatingString.Replace(arrValidatingString[i], '\0');
                        }
                    }
                }
                return sValidatingString;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string LeaveAtoZand0to9(string sValidatingString, string sExtendedCharacters = "")
        {
            int i = 0;
            int j = 0;
            char[] arrExtendedCharacters = null;
            char[] arrValidatingString = null;

            try
            {
                arrExtendedCharacters = sExtendedCharacters.ToCharArray();
                arrValidatingString = sValidatingString.ToCharArray();
                for (i = 0; i <= arrValidatingString.Length - 1; i++)
                {
                    if ((arrValidatingString[i] < 'A' || arrValidatingString[i] > 'Z')
                        && (arrValidatingString[i] < 'a' || arrValidatingString[i] > 'z')
                        && (arrValidatingString[i] < '0' || arrValidatingString[i] > '9')
                        && (Convert.ToInt32(arrValidatingString[i]) >= 32 && Convert.ToInt32(arrValidatingString[i]) <= 126))
                    {
                        if (sExtendedCharacters != "")
                        {
                            for (j = 0; j <= arrExtendedCharacters.Length - 1; j++)
                            {
                                if (arrExtendedCharacters[j] == arrValidatingString[i])
                                {
                                    break;
                                }
                            }
                            if (j == arrExtendedCharacters.Length)
                            {
                                //arrValidatingString(i) = ""
                                sValidatingString = sValidatingString.Replace(arrValidatingString[i], '\0');
                            }
                        }
                        else
                        {
                            sValidatingString = sValidatingString.Replace(arrValidatingString[i], '\0');
                        }
                    }
                }
                return sValidatingString;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string RemoveSingleQuoteCommaDot(string sValidatingString)
        {
            try
            {
                return sValidatingString.Replace("\'", "").Replace(",", "").Replace(".", "").Trim();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 创建数据库连接字符串
        /// </summary>
        public static void  ConnectDatabase()
        {
            try
            {
                if (Util.GlobalVariables.UseWindowsAuthentication == "Y")
                {
                    Util.GlobalVariables.CnnString = "Initial Catalog=" + Util.GlobalVariables.InitialCatalog + ";Data Source=" + Util.GlobalVariables.DataSource + ";Integrated Security=SSPI" + ";Max Pool Size=1000;Connect Timeout=300 " + Util.GlobalVariables.ApplicationName;
                }
                else
                {
                    Util.GlobalVariables.CnnString = "Initial Catalog=" + Util.GlobalVariables.InitialCatalog + ";Data Source=" + Util.GlobalVariables.DataSource + ";User ID=" +
                        Util.GlobalVariables.DatabaseUserId + ";Password=" + DecTech.Library.Encrypt.AESDecryption(Util.GlobalVariables.DatabasePassword) + ";Max Pool Size=1000;Connect Timeout=300 " + Util.GlobalVariables.ApplicationName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
               

    }    
}
