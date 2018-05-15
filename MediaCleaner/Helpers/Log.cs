using System;
using System.IO;

namespace MediaCleaner
{
    public class Log
    {
        static string LogFile           = "Log.txt";
        static string DeletedLogFile    = "DeleteLog.txt";

        public static void Info(string text)
        {
            newLog("Info", text, LogFile);
        }

        public static void Error(string text)
        {
            newLog("Error", text, LogFile);
        }

        public static void Debug(string text)
        {
            if(Config.Debug)
                newLog("Debug", text, LogFile);
        }

        public static void Deleted(string text)
        {
            newLog("FileDelete", text, DeletedLogFile);
        }

        private static void newLog(string type, string text, string file)
        {
            using (StreamWriter sw = File.AppendText(file))
            {
                sw.WriteLine(string.Format("[{0}] [{1}] {2}", DateTime.Now.ToString("yy/MM/dd H:mm:ss"), type, text));
                System.Diagnostics.Debug.WriteLine(string.Format("[{0}] [{1}] {2}", DateTime.Now.ToString("yy/MM/dd H:mm:ss"), type, text));
            }
        }
    }
}
