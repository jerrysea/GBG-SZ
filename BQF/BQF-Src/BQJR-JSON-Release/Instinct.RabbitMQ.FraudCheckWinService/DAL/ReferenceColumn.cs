using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.FraudCheckWinService.DAL
{
    public class ReferenceColumn
    {
        public int Field_Id { get; set; }        
        public string Field_Name { get; set; }
        public string Description { get; set; }
        public string Value_Type { get; set; }
        public int Length { get; set; }
        public string Format { get; set; }
        public bool  Active { get; set; }
        public string Calculations { get; set; }

        public bool Checked { get; set; }

        public ReferenceColumn(int fieldid,string fieldname,string des,string valuetype,int length,string format,bool active,string cal)
        {
            this.Field_Id = fieldid;
            this.Field_Name = fieldname;
            this.Description = des;
            this.Value_Type = valuetype;
            this.Length = length;
            this.Format = format;
            this.Active = active;
            this.Calculations = cal;
        }

    }
}
