using NLog;
using System;
using System.IO;

namespace MediaCleaner
{
    public class Log
    {
        public static void EnableDebugLevel()
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                rule.EnableLoggingForLevel(LogLevel.Debug);
            }

            LogManager.ReconfigExistingLoggers();
        }

        public static void EnableTraceLevel()
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                rule.EnableLoggingForLevel(LogLevel.Trace);
                rule.EnableLoggingForLevel(LogLevel.Debug);
            }

            LogManager.ReconfigExistingLoggers();
        }

        public static void DisableDebugLevel()
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                rule.DisableLoggingForLevel(LogLevel.Debug);
                rule.DisableLoggingForLevel(LogLevel.Trace);
            }

            LogManager.ReconfigExistingLoggers();
        }

        public static void DisableTraceLevel()
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                rule.DisableLoggingForLevel(LogLevel.Trace);
            }

            LogManager.ReconfigExistingLoggers();
        }
    }
}
