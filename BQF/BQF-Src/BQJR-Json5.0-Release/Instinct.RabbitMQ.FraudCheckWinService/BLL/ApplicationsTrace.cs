using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.FraudCheckWinService.BLL
{
    /// <summary>
    /// 记录正在处理的订单
    /// </summary>
    public class ApplicationsTrace : Dictionary<string, int>
    {
        private static ApplicationsTrace _instance = null;
        private static readonly object SynObject = new object();

        // <summary>
        /// Gets the instance.
        /// </summary>
        public static ApplicationsTrace Instance
        {
            get
            {
                // Syn operation.
                lock (SynObject)
                {
                    return _instance ?? (_instance = new ApplicationsTrace());
                }
            }
        }
    }
}
