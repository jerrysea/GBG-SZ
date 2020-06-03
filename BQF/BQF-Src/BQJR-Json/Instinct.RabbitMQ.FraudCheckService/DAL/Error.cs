using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.FraudCheckService.DAL
{
    public class Error
    {
        public string Organisation { get; set; }
        public string Country_Code { get; set; }
        public string Group_Member_Code { get; set; }
        public string Application_Number { get; set; }
        public string Application_Date { get; set; }
        public string Application_Type { get; set; }
        public string Error_Code { get; set; }
        public string Remark { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Output>");
            sb.Append("<Organisation>");
            sb.Append(Organisation);
            sb.Append("</Organisation>");
            sb.Append("<Country_Code>");
            sb.Append(Country_Code);
            sb.Append("</Country_Code>");
            sb.Append("<Group_Member_Code>");
            sb.Append(Group_Member_Code);
            sb.Append("</Group_Member_Code>");
            sb.Append("<Application_Number>");
            sb.Append(Application_Number);
            sb.Append("</Application_Number>");
            sb.Append("<Application_Date>");
            sb.Append(Application_Date);
            sb.Append("</Application_Date>");
            sb.Append("<Application_Type>");
            sb.Append(Application_Type);
            sb.Append("</Application_Type>");
            sb.Append("<Error_Code>");
            sb.Append(Error_Code);
            sb.Append("</Error_Code>");
            sb.Append("<Remark>");
            sb.Append(Remark);
            sb.Append("</Remark>");
            sb.Append("</Output>");
            return sb.ToString();
        }
        /// <summary>
        /// XML格式ERROR 
        /// </summary>
        /// <returns></returns>
        public string ToOutputSchemaString()
        {
            return "<OutputSchema>" + this.ToString() + "</OutputSchema>";
        }
        /// <summary>
        /// TXT 格式ERROR
        /// </summary>
        /// <returns></returns>
        public string ToTXTString()
        {
            return Organisation + "|" + Country_Code + "|" + "|" + Application_Number + "|" + "|" + "|" + Application_Type + "|" + "|" + "|" + "|" + Error_Code + "|";
        }
    }
}
