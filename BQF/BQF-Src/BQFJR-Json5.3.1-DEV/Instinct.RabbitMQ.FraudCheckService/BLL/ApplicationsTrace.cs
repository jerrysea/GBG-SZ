using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.FraudCheckService.BLL
{
    /// <summary>
    /// 记录正在处理的订单
    /// </summary>
    public class ApplicationsTrace : Dictionary<string, int>
    {
        private static ApplicationsTrace _instance = null;
        private static readonly object SynObject = new object();
        public static readonly object LckObject = new object();
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

        public bool AddElement(string sAppkey)
        {
            lock (LckObject)
            {
                if (!ApplicationsTrace.Instance.ContainsKey(sAppkey))
                {
                    ApplicationsTrace.Instance.Add(sAppkey, 1);
                    return true;
                }
                else {
                    ApplicationsTrace.Instance[sAppkey]++;
                    if (ApplicationsTrace.Instance[sAppkey] > 150)
                    {
                        ApplicationsTrace.Instance.Remove(sAppkey);
                        ApplicationsTrace.Instance.Add(sAppkey, 1);
                        return true;
                    }
                    return false;
                }
            }
        }

        public void RemoveElement(string sAppkey)
        {
            lock (LckObject)
            {
                if (ApplicationsTrace.Instance.ContainsKey(sAppkey))
                {
                    ApplicationsTrace.Instance.Remove(sAppkey);
                    
                }
            }
        }

    }
}
