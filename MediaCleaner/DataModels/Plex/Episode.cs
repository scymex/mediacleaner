using System.Collections.Generic;

namespace MediaCleaner.DataModels.Plex
{
    public class Part
    {
        public int id { get; set; }
        public string key { get; set; }
        public int duration { get; set; }
        public string file { get; set; }
        public long size { get; set; }
        public string audioProfile { get; set; }
        public string container { get; set; }
        public string videoProfile { get; set; }
    }

    public class Medium
    {
        public string videoResolution { get; set; }
        public int id { get; set; }
        public int duration { get; set; }
        public int bitrate { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public double aspectRatio { get; set; }
        public int audioChannels { get; set; }
        public string audioCodec { get; set; }
        public string videoCodec { get; set; }
        public string container { get; set; }
        public string videoFrameRate { get; set; }
        public string audioProfile { get; set; }
        public string videoProfile { get; set; }
        public List<Part> Part { get; set; }
    }

    public class Director
    {
        public string tag { get; set; }
    }

    public class Writer
    {
        public string tag { get; set; }
    }

    public class Episode
    {
        public string ratingKey { get; set; }
        public string key { get; set; }
        public string parentRatingKey { get; set; }
        public string grandparentRatingKey { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string grandparentKey { get; set; }
        public string parentKey { get; set; }
        public string grandparentTitle { get; set; }
        public string parentTitle { get; set; }
        public string contentRating { get; set; }
        public string summary { get; set; }
        public int index { get; set; }
        public int parentIndex { get; set; }
        public double rating { get; set; }
        public int viewCount { get; set; }
        public int lastViewedAt { get; set; }
        public int year { get; set; }
        public string thumb { get; set; }
        public string art { get; set; }
        public string parentThumb { get; set; }
        public string grandparentThumb { get; set; }
        public string grandparentArt { get; set; }
        public string grandparentTheme { get; set; }
        public int duration { get; set; }
        public string originallyAvailableAt { get; set; }
        public int addedAt { get; set; }
        public int updatedAt { get; set; }
        public List<Medium> Media { get; set; }
        public List<Director> Director { get; set; }
        public List<Writer> Writer { get; set; }
        public string titleSort { get; set; }
    }

    public class EpisodesMediaContainer
    {
        public int size { get; set; }
        public bool allowSync { get; set; }
        public string art { get; set; }
        public string banner { get; set; }
        public string grandparentContentRating { get; set; }
        public int grandparentRatingKey { get; set; }
        public string grandparentStudio { get; set; }
        public string grandparentTheme { get; set; }
        public string grandparentThumb { get; set; }
        public string grandparentTitle { get; set; }
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
        public string theme { get; set; }
        public string thumb { get; set; }
        public string title1 { get; set; }
        public string title2 { get; set; }
        public string viewGroup { get; set; }
        public int viewMode { get; set; }
        public List<Episode> Metadata { get; set; }
    }

    public class Episodes
    {
        public EpisodesMediaContainer MediaContainer { get; set; }
    }
}
