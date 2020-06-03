using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

using SCM.RabbitMQClient;

namespace Instinct.ConsoleTest
{
    public class LogHelper:SCM.RabbitMQClient.Common.ILog
    {
        public LogHelper()
        {
            SetConfig();
        }

        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        private static bool IsLoadConfig = false;
        private static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }        

        public static void SetProperties(string name, string value)
        {
            log4net.GlobalContext.Properties[name] = value;
            SetConfig();
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="info">提示信息</param>
        public static void InfoLog(string info)
        {
            if (!IsLoadConfig)
            {
                SetConfig();
                IsLoadConfig = true;
            }
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(ToLine(info));
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="info">提示信息</param>
        public static void InfoLog(string info, Exception ex)
        {
            if (!IsLoadConfig)
            {
                SetConfig();
                IsLoadConfig = true;
            }
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info, ex);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="info">调试信息</param>
        public static void DebugLog(string info)
        {
            if (!IsLoadConfig)
            {
                SetConfig();
                IsLoadConfig = true;
            }
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Debug(ToLine(info));
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="info">调试信息</param>
        public static void DebugLog(string info, Exception ex)
        {
            if (!IsLoadConfig)
            {
                SetConfig();
                IsLoadConfig = true;
            }
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Debug(info, ex);
            }
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="info">错误</param>
        /// <param name="ex">Exception</param>
        public static void ErrorLog(string info)
        {
            if (!IsLoadConfig)
            {
                SetConfig();
                IsLoadConfig = true;
            }
            if (loginfo.IsErrorEnabled)
            {
                loginfo.Error(ToLine(info));
            }
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="info">错误</param>
        /// <param name="ex">Exception</param>
        public static void ErrorLog(string info, Exception ex)
        {
            if (!IsLoadConfig)
            {
                SetConfig();
                IsLoadConfig = true;
            }
            if (loginfo.IsErrorEnabled)
            {
                loginfo.Error(info, ex);
            }
        }

        public void WriteException(string title, Exception exception)
        {
            LogHelper.ErrorLog(title, exception);
        }

        public void WriteInfo(string title, string message)
        {                        
            LogHelper.InfoLog(string.Format("{0}:{1}", title, ToLine(message)));
        }
        /// <summary>
        /// 将多行信息转换成单行
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string ToLine(string message)
        {
            if (message.Contains("\t"))
            {
                message = message.Replace("\t", "");
            }
            if (message.Contains("\n"))
            {
                message = message.Replace("\n", "");
            }
            if (message.Contains("\r"))
            {
                message = message.Replace("\r", "");
            }
            return message;
        }
    }
}
