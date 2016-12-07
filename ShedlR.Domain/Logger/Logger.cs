using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using ShedlR.Domain.Interfaces;

namespace ShedlR.Domain.Logger
{
    public class Logger: ILogger, IDisposable
    {
        private NLog.Logger _logger;
        public Logger()
        {
            // Step 1. Create configuration object 
            LoggingConfiguration config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            FileTarget fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            // Step 3. Set target properties 
            //consoleTarget.Layout = @"${date:format=HH\:MM\:ss} ${logger} ${message}";
            consoleTarget.Layout = @"${longdate}|${level}| ${logger}| ${message}";
            fileTarget.FileName = "${basedir}/log.txt";
            //fileTarget.Layout = @"${longdate}|${level}| ${message}";
            fileTarget.Layout = @"${longdate}|${level}|${event-context:item=ClassName}|${event-context:item=MethodName}|${message}";
            // Step 4. Define rules
            LoggingRule rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            LoggingRule rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;

            _logger = LogManager.GetCurrentClassLogger();
        }
    
        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warning(string message)
        {
            _logger.Warn(message); 
        }
        public void Error(string message, string classname = "", string methodname ="")
        {
            if (classname != "" && methodname != "")
            {
                LogEventInfo theEvent = new LogEventInfo();
                theEvent.Level = LogLevel.Error;
                theEvent.Properties["ClassName"] = classname;
                theEvent.Properties["MethodName"] = methodname;
                theEvent.Message = message;
                theEvent.TimeStamp = DateTime.Now;
                _logger.Log(theEvent);
            }
            else
                _logger.Error(message);
        }
        public void FatalError(string message)
        {
            _logger.Fatal(message);
        }

        public void Dispose()
        {
            _logger.Factory.Flush();
            _logger.Factory.Dispose();
        }
    }
}
