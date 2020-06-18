using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.InterfaceClient.Util
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

        public static Encoding  GetEncoding(string encodename)
        {
            Encoding e = null;
            try
            {
                if (encodename==null || encodename == "")
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
    } 
}
