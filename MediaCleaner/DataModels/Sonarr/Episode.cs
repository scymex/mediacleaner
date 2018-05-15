namespace MediaCleaner.DataModels.Sonarr
{
    public class Episode
    {
        public int seriesId { get; set; }
        public int episodeFileId { get; set; }
        public int seasonNumber { get; set; }
        public int episodeNumber { get; set; }
        public string title { get; set; }
        public string airDate { get; set; }
        public string airDateUtc { get; set; }
        public string overview { get; set; }
        public bool hasFile { get; set; }
        public bool monitored { get; set; }
        public int? absoluteEpisodeNumber { get; set; }
        public bool unverifiedSceneNumbering { get; set; }
        public int id { get; set; }
        public EpisodeFile episodeFile { get; set; }
    }
}
