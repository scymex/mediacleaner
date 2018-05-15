using NLog;
using System;
using System.IO;

namespace MediaCleaner
{
    public class Log
    {
        public static void EnableDebug()
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                rule.EnableLoggingForLevel(LogLevel.Trace);
                rule.EnableLoggingForLevel(LogLevel.Debug);
            }

            LogManager.ReconfigExistingLoggers();
        }
    }
}
