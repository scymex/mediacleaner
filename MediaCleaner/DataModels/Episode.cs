using System;

namespace MediaCleaner.DataModels
{
    public class Episode
    {
        public string SeriesName { get; set; }
        public string SeasonNumber { get; set; }
        public string EpisodeNumber { get; set; }
        public string EpisodeTitle { get; set; }
        public string FilePath { get; set; }
        public bool IsFavorite { get; set; }
        public bool Played { get; set; }
        public DateTime dateAdded { get; set; }
    }
}
