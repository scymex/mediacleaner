using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCleaner.Plex
{
    public class Directory
    {
        public int leafCount { get; set; }
        public string thumb { get; set; }
        public int viewedLeafCount { get; set; }
        public string key { get; set; }
        public string title { get; set; }
    }

    public class Season
    {
        public string ratingKey { get; set; }
        public string key { get; set; }
        public string parentRatingKey { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string parentKey { get; set; }
        public string parentTitle { get; set; }
        public string summary { get; set; }
        public int index { get; set; }
        public int parentIndex { get; set; }
        public int viewCount { get; set; }
        public int lastViewedAt { get; set; }
        public string thumb { get; set; }
        public string art { get; set; }
        public string parentThumb { get; set; }
        public string parentTheme { get; set; }
        public int leafCount { get; set; }
        public int viewedLeafCount { get; set; }
        public int addedAt { get; set; }
        public int updatedAt { get; set; }
        public string hasPremiumPrimaryExtra { get; set; }
        public string primaryExtraKey { get; set; }
    }

    public class SeasonMediaContainer
    {
        public int size { get; set; }
        public bool allowSync { get; set; }
        public string art { get; set; }
        public string banner { get; set; }
        public string identifier { get; set; }
        public string key { get; set; }
        public int librarySectionID { get; set; }
        public string librarySectionTitle { get; set; }
        public string librarySectionUUID { get; set; }
        public string mediaTagPrefix { get; set; }
        public int mediaTagVersion { get; set; }
        public bool nocache { get; set; }
        public int parentIndex { get; set; }
        public string parentTitle { get; set; }
        public int parentYear { get; set; }
        public string summary { get; set; }
        public string theme { get; set; }
        public string thumb { get; set; }
        public string title1 { get; set; }
        public string title2 { get; set; }
        public string viewGroup { get; set; }
        public int viewMode { get; set; }
        public List<Directory> Directory { get; set; }
        public List<Season> Metadata { get; set; }
    }

    public class Seasons
    {
        public SeasonMediaContainer MediaContainer { get; set; }
    }
}
