using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Utils
{
    public class LogHelper
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Methods
        /// <summary>
        /// Loads the log4 net configuration.
        /// </summary>
        public static void LoadLog4NetConfiguration()
        {
            // Only configure log4net once per process
            if (!log4net.LogManager.GetRepository().Configured)
            {
                XmlConfigurator.Configure();

                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Loaded log4net config in {0}.", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly.FullName);
                }
            }
        }


        public static void Info(ILog log, string message)
        {
            log.Info(message);
        }

        public static void Info(ILog log, string message, Exception ex)
        {
            log.Info(message, ex);
        }

        public static void Error(ILog log, string message)
        {
            log.Info(message);
        }

        public static void Error(ILog log, string message, Exception ex)
        {
            log.Info(message, ex);
        }

        #endregion
    }
}
