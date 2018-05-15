using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCleaner.DataModels.Sonarr
{
    public class SystemStatus
    {
        public string version { get; set; }
        public string buildTime { get; set; }
        public bool isDebug { get; set; }
        public bool isProduction { get; set; }
        public bool isAdmin { get; set; }
        public bool isUserInteractive { get; set; }
        public string startupPath { get; set; }
        public string appData { get; set; }
        public string osVersion { get; set; }
        public bool isMono { get; set; }
        public bool isLinux { get; set; }
        public bool isWindows { get; set; }
        public string branch { get; set; }
        public bool authentication { get; set; }
        public int startOfWeek { get; set; }
        public string urlBase { get; set; }
    }
}
