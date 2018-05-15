using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCleaner.DataModels.Plex
{
    public class Location
    {
        public int id { get; set; }
        public string path { get; set; }
    }

    public class Section
    {
        public bool allowSync { get; set; }
        public string art { get; set; }
        public string composite { get; set; }
        public bool filters { get; set; }
        public bool refreshing { get; set; }
        public string thumb { get; set; }
        public string key { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string agent { get; set; }
        public string scanner { get; set; }
        public string language { get; set; }
        public string uuid { get; set; }
        public int updatedAt { get; set; }
        public int createdAt { get; set; }
        public List<Location> Location { get; set; }
    }



    public class SectionMediaContainer
    {
        public int size { get; set; }
        public bool allowSync { get; set; }
        public string identifier { get; set; }
        public string mediaTagPrefix { get; set; }
        public int mediaTagVersion { get; set; }
        public string title1 { get; set; }
        public List<Section> Directory { get; set; }
    }

    public class SectionContainer
    {
        public SectionMediaContainer MediaContainer { get; set; }
    }
}
