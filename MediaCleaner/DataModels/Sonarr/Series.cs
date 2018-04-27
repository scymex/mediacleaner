using System.Collections.Generic;

namespace MediaCleaner.Sonarr
{
    public class Image
    {
        public string coverType { get; set; }
        public string url { get; set; }
    }

    public class Statistics
    {
        public int episodeFileCount { get; set; }
        public int episodeCount { get; set; }
        public int totalEpisodeCount { get; set; }
        public object sizeOnDisk { get; set; }
        public double percentOfEpisodes { get; set; }
        public string nextAiring { get; set; }
        public string previousAiring { get; set; }
    }

    public class Season
    {
        public int seasonNumber { get; set; }
        public bool monitored { get; set; }
        public Statistics statistics { get; set; }
    }

    public class Ratings
    {
        public int votes { get; set; }
        public double value { get; set; }
    }

    public class Series
    {
        public string title { get; set; }
        public List<object> alternateTitles { get; set; }
        public string sortTitle { get; set; }
        public int seasonCount { get; set; }
        public int totalEpisodeCount { get; set; }
        public int episodeCount { get; set; }
        public int episodeFileCount { get; set; }
        public object sizeOnDisk { get; set; }
        public string status { get; set; }
        public string overview { get; set; }
        public string nextAiring { get; set; }
        public string previousAiring { get; set; }
        public string network { get; set; }
        public string airTime { get; set; }
        public List<Image> images { get; set; }
        public List<Season> seasons { get; set; }
        public int year { get; set; }
        public string path { get; set; }
        public int profileId { get; set; }
        public bool seasonFolder { get; set; }
        public bool monitored { get; set; }
        public bool useSceneNumbering { get; set; }
        public int runtime { get; set; }
        public int tvdbId { get; set; }
        public int tvRageId { get; set; }
        public int tvMazeId { get; set; }
        public string firstAired { get; set; }
        public string lastInfoSync { get; set; }
        public string seriesType { get; set; }
        public string cleanTitle { get; set; }
        public string imdbId { get; set; }
        public string titleSlug { get; set; }
        public string certification { get; set; }
        public List<object> genres { get; set; }
        public List<object> tags { get; set; }
        public string added { get; set; }
        public Ratings ratings { get; set; }
        public int qualityProfileId { get; set; }
        public int id { get; set; }
    }
}
