using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCleaner.DataModels.Plex
{
    public class Genre
    {
        public string tag { get; set; }
    }

    public class Role
    {
        public string tag { get; set; }
    }

    public class Show
    {
        public string ratingKey { get; set; }
        public string key { get; set; }
        public string studio { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string titleSort { get; set; }
        public string contentRating { get; set; }
        public string summary { get; set; }
        public int index { get; set; }
        public double rating { get; set; }
        public int viewCount { get; set; }
        public int lastViewedAt { get; set; }
        public int year { get; set; }
        public string thumb { get; set; }
        public string art { get; set; }
        public string banner { get; set; }
        public string theme { get; set; }
        public int duration { get; set; }
        public string originallyAvailableAt { get; set; }
        public int leafCount { get; set; }
        public int viewedLeafCount { get; set; }
        public int childCount { get; set; }
        public int addedAt { get; set; }
        public int updatedAt { get; set; }
        public List<Genre> Genre { get; set; }
        public List<Role> Role { get; set; }
        public string hasPremiumPrimaryExtra { get; set; }
        public string primaryExtraKey { get; set; }
        public string hasPremiumExtras { get; set; }
    }

    public class ShowMediaContainer
    {
        public int size { get; set; }
        public bool allowSync { get; set; }
        public string art { get; set; }
        public string identifier { get; set; }
        public int librarySectionID { get; set; }
        public string librarySectionTitle { get; set; }
        public string librarySectionUUID { get; set; }
        public string mediaTagPrefix { get; set; }
        public int mediaTagVersion { get; set; }
        public bool nocache { get; set; }
        public string thumb { get; set; }
        public string title1 { get; set; }
        public string title2 { get; set; }
        public string viewGroup { get; set; }
        public int viewMode { get; set; }
        public List<Show> Metadata { get; set; }
    }

    public class Shows
    {
        public ShowMediaContainer MediaContainer { get; set; }
    }
}
