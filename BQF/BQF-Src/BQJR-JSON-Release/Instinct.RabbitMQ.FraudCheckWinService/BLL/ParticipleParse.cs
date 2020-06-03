using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

namespace Instinct.RabbitMQ.FraudCheckWinService.BLL
{
    /// <summary>
    /// 分词业务逻辑
    /// </summary>
    public class ParticipleParse
    {
        #region members
        public string ParticipleLayout_Path;
        public List<ParticipleLayout> ParticipleLayouts;
        public Hashtable TableMap;
        #endregion

        #region init
        public ParticipleParse()
        {
            this.ParticipleLayout_Path = AppDomain.CurrentDomain.BaseDirectory + "ParticipleLayout.xml";
            this.ParticipleLayouts = new List<ParticipleLayout>();
            this.TableMap = GetTableIndex();
        }
        #endregion

        #region methods
        /// <summary>
        /// 初始化 检查环境配置
        /// </summary>
        public void Init()
        {
            string errMsg = "";
            LoadParticipleLayout();
            if (!Validate(ref errMsg))
            {
                throw new Exception(errMsg);
            }
        }

        /// <summary>
        /// 从Input 输入获取分词参数
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sAppKey"></param>
        /// <returns></returns>
        public ParticipleParameter GetValueFromInput(XmlDocument input,string sAppKey)
        {
            List<ParticipleParameter.Participle> list = GetParticipleEntity(input);
            foreach (ParticipleParameter.Participle p in list)
            {
                if (p.PType.ToUpper() == "ADDRESS")
                {
                    FM_Address address = Segmentation.FetchAddressEntity(p.FullValue);
                    p.XMLContent = ConvertFMAddress2XML(address);
                }
                else if (p.PType.ToUpper() == "COMPANY")
                {
                    FM_Company company = Segmentation.FetchCompanyEntity(p.FullValue);
                    p.XMLContent = ConvertFMCompany2XML(company);
                }
            }
            ParticipleParameter pInput = new ParticipleParameter();            
            pInput.List = list;
            return pInput;
        }

        #region 抽取Input分词值
        private List<ParticipleParameter.Participle> GetParticipleEntity(XmlDocument input)
        {
            List<ParticipleParameter.Participle> list = new List<ParticipleParameter.Participle>();
            if (this.ParticipleLayouts.Count > 0)
            {
                foreach (ParticipleLayout layout in this.ParticipleLayouts)
                {
                    string tabname = layout.TableName;
                    string xpath = "/ApplicationSchema";
                    XmlNodeList NodeList = null;
                    if (tabname.ToUpper() == "APPLICATION")
                    {
                        xpath = xpath + "Application";
                    }
                    else
                    {
                        if (tabname.ToUpper() == "ACCOUNTANT_SOLICITOR")
                            xpath = xpath + "/Application/A_" + tabname;
                        else
                            xpath = xpath + "/Application/" + tabname;
                    }
                    NodeList = input.DocumentElement.SelectNodes(xpath);
                    if (NodeList != null)
                    {
                        int SequenceNumber = 0;
                        foreach (XmlNode xn in NodeList)
                        {
                            SequenceNumber++;
                            ParticipleParameter.Participle p = new ParticipleParameter.Participle();
                            p.PName = layout.PName;
                            p.PType = layout.PType;
                            p.SequenceNumber = SequenceNumber;

                            StringBuilder sbFullValue = new StringBuilder();
                            if (layout.Fields != null && layout.Fields.Count > 0)
                            {
                                foreach (string f in layout.Fields)
                                {
                                    XmlNode fn = xn.SelectSingleNode(f);
                                    if (fn != null)
                                    {
                                        sbFullValue.Append(fn.InnerText);
                                    }
                                }
                                p.FullValue = sbFullValue.ToString().Trim();
                            }
                            list.Add(p);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region 检验分词配置

        /// <summary>
        /// 加载解析Participle Layouts
        /// 
        /// </summary>
        private void LoadParticipleLayout()
        {
            if (System.IO.File.Exists(this.ParticipleLayout_Path))
            {
                XmlDocument doc = Util.XMLProcess.Load(this.ParticipleLayout_Path);
                XmlNodeList TabNodes = Util.XMLProcess.ReadAllChild("/ParticipleSchema", doc);
                foreach (XmlNode xn in TabNodes)
                {
                    string tabname = xn.Name;
                    if (xn.HasChildNodes)
                    {
                        foreach (XmlNode participlen in xn.ChildNodes)
                        {
                            if (participlen.Name == "Participle")
                            {
                                string pName = participlen.Attributes["PName"].Value;
                                string pType = participlen.Attributes["PType"].Value;
                                if (participlen.HasChildNodes)
                                {
                                    ParticipleLayout layout = new ParticipleLayout();
                                    layout.TableName = tabname;
                                    layout.PName = pName;
                                    layout.PType = pType;
                                    layout.Fields = new List<string>();
                                    foreach (XmlNode fn in participlen.ChildNodes)
                                    {
                                        layout.Fields.Add(fn.Name);
                                    }
                                    this.ParticipleLayouts.Add(layout);
                                }
                                else
                                {
                                    throw new Exception("The Participle xml node has no fields.");
                                }
                            }
                            else
                            {
                                throw new Exception("The Participle xml node 's layout is wrong.");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("The table xml node has no \"participle\" nodes.");
                    }
                }
            }
            else
            {
                throw new Exception("The participle layout file does not exist.");
            }
        }
        /// <summary>
        /// 验证表结果合法性
        /// True : 合法 ; False: 非法
        /// </summary>
        /// <returns></returns>
        private bool Validate(ref string errMsg)
        {
            bool bCheck = true;
            StringBuilder sbMsg = new StringBuilder();
            try
            {
                if (this.ParticipleLayouts != null && this.ParticipleLayouts.Count > 0)
                {
                    int iunmatched = 0;
                    foreach (ParticipleLayout layout in this.ParticipleLayouts)
                    {
                        string layoutMsg = "";
                        string tabname = layout.TableName;
                        int tabindex = Convert.ToInt32(this.TableMap[tabname]);
                        DataTable fieldsDt = DAL.ParticipleDAl.GetTableLayoutInfo(tabindex);
                        List<string> fields = layout.Fields;
                        if (!CheckMatched(fields, fieldsDt, ref layoutMsg))
                        {
                            sbMsg.AppendLine(string.Format("The table \"{0}\" does not matched. error info:{1}", tabname, layoutMsg));
                            iunmatched++;
                        }
                    }
                    if (iunmatched > 0)
                    {
                        bCheck = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bCheck = false;
                sbMsg.AppendLine(ex.ToString());
            }
            errMsg = sbMsg.ToString();
            return bCheck;
        }


        /// <summary>
        /// 获取表结构与表索引
        /// </summary>
        /// <returns></returns>
        private Hashtable GetTableIndex()
        {
            Hashtable hs = new Hashtable();
            hs.Add("Application", 1);
            hs.Add("Applicant", 2);
            hs.Add("Accountant_Solicitor", 3);
            hs.Add("Guarantor", 4);
            hs.Add("Introducer_Agent", 5);
            hs.Add("Reference", 6);
            hs.Add("Security", 7);
            hs.Add("PreviousAddress_Company", 8);
            hs.Add("Valuer", 9);
            hs.Add("User", 10);
            hs.Add("CreditBureau", 11);
            hs.Add("User2", 12);
            hs.Add("UCA", 13);
            hs.Add("CBA", 14);
            hs.Add("U2A", 15);
            hs.Add("UCB", 16);
            hs.Add("CBB", 17);
            hs.Add("U2B", 18);
            hs.Add("UCC", 19);
            hs.Add("CBC", 20);
            hs.Add("U2C", 21);
            return hs;
        }

        /// <summary>
        /// 检查字段名称与layout 是否匹配
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="fieldsDt"></param>
        /// <returns></returns>
        private bool CheckMatched(List<string> fields, DataTable fieldsDt, ref string errMsg)
        {
            bool bMatched = true;
            StringBuilder sbMsg = new StringBuilder();
            if (fields != null && fieldsDt != null)
            {
                if (fields.Count <= fieldsDt.Rows.Count)
                {
                    int iunmatched = 0;
                    foreach (string f in fields)
                    {
                        DataRow[] rows = fieldsDt.Select(string.Format("Category_Field_Name='{0}'", f));
                        if (rows.Length != 1)
                        {
                            iunmatched++;
                            sbMsg.AppendLine(string.Format("The layout does not contain the filed \"{0}\".", f));
                        }
                    }
                    if (iunmatched > 0)
                    {
                        bMatched = false;
                    }
                }
                else
                {
                    bMatched = false;
                    sbMsg.AppendLine("The number of the layout is less than the number of the fileds.");
                }
            }
            else
            {
                bMatched = false;
                sbMsg.AppendLine("The layout/the fields is empty.");
            }
            errMsg = sbMsg.ToString();
            return bMatched;
        }

        #endregion

        #region 分词
        /// <summary>
        /// Address转换XML
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string ConvertFMAddress2XML(FM_Address address)
        {
            StringBuilder content = new StringBuilder();
            if (address != null)
            {
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "Province", address.Province));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "City", address.City));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "District", address.District));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "Street", address.Street));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "Detail", address.Detail));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "AddressNumber", address.AddressNumber));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "Landmark", address.Landmark));
            }
            return content.ToString();
        }
        /// <summary>
        /// Company转换XML
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        private string ConvertFMCompany2XML(FM_Company company)
        {
            StringBuilder content = new StringBuilder();
            if (company != null)
            {
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "Province", company.Province));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "City", company.City));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "District", company.District));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "Industry", company.Industry));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "CompanyType", company.CompanyType));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "Branch", company.Branch));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "CoreName", company.CoreName));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "Department", company.Department));
                content.AppendLine(string.Format("<{0}>{1}</{0}>", "Other", company.Other));
            }
            return content.ToString();
        }
        #endregion

        #endregion
    }
}
