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
        private static ApplicationsTrace _instance = new ApplicationsTrace();
        //private static readonly object SynObject = new object();
        public static readonly object LckObject = new object();

        private ApplicationsTrace() { }

        // <summary>
        /// Gets the instance.
        /// </summary>
        public static ApplicationsTrace Instance
        {
            get
            {
                //if (_instance == null)
                //{
                //    // Syn operation.
                //    lock (SynObject)
                //    {
                //        if (_instance == null)
                //            _instance = new ApplicationsTrace();
                //    }
                //}
                return _instance;                
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
