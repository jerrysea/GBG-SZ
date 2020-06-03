using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace Instinct.RabbitMQ.FraudCheckService.BLL
{
    public class ParticipleParameter
    {        
        public List<Participle> List;

        #region inner class
        public class Participle
        {
            public string PType;
            public string PName;            
            public string FullValue;
            public string XMLContent;
            public int SequenceNumber;
        }
        #endregion

        public string ToXML()
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("<Application>");           
            foreach (Participle p in List)
            {
                StringBuilder pcontent = new StringBuilder();
                pcontent.AppendLine(string.Format("<Participle Type=\"{0}\" Referred_Field=\"{1}\" SequenceNumber=\"{2}\">", p.PType,p.PName,p.SequenceNumber));
                pcontent.AppendLine(string.Format("<FullValue>{0}</FullValue>",SecurityElement.Escape(p.FullValue)));
                pcontent.Append(p.XMLContent);
                pcontent.AppendLine("</Participle>");
                content.Append(pcontent.ToString());
            }
            content.AppendLine("</Application>");
            return content.ToString();
        }
    }
}
