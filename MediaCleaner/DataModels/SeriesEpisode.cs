namespace MediaCleaner.DataModels
{
    class SeriesEpisode
    {

        public string seriesTitle { get; set; }
        public string episodeTitle { get; set; }
        public string seasonNumber { get; set; }
        public string episodeNumber { get; set; }
        public string episodeFilePath { get; set; }
        public string watched { get; set; }
        public bool isFavorite { get; set; }
    }
}
