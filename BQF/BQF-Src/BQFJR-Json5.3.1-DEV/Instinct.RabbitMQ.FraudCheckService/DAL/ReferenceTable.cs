using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.FraudCheckService.DAL
{
    public class ReferenceTable
    {
        public int Table_Id { get; set; }
        public int Count { get; set; }
        public Dictionary<string, ReferenceColumn> Columns { get; set; }

        public ReferenceTable(int tableid)
        {
            this.Table_Id = tableid;
            this.Count = 0;
            this.Columns = new Dictionary<string, ReferenceColumn>();
        }

        public void AddColumn(string fieldname, ReferenceColumn col)
        {
            this.Columns.Add(fieldname, col);
            this.Count++;
        }
    }
}
