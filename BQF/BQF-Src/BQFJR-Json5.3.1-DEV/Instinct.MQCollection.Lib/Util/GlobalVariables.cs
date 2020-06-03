using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.Collection.Lib.Util
{
    public class GlobalVariables
    {
        public static string InitialCatalog { get; set; }
        public static string DataSource { get; set; }
        public static string ApplicationName { get; set; }
        public static string UseWindowsAuthentication { get; set; }
        public static string DatabaseUserId { get; set; }
        public static string DatabasePassword { get; set; }
        public static string MaxPoolSize { get; set; }    
        public static string CnnString { get; set; }
    }
}
