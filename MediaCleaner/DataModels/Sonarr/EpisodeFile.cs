namespace MediaCleaner.Sonarr
{
    public class Quality2
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Revision
    {
        public int version { get; set; }
        public int real { get; set; }
    }

    public class Quality
    {
        public Quality2 quality { get; set; }
        public Revision revision { get; set; }
    }

    public class EpisodeFile
    {
        public int seriesId { get; set; }
        public int seasonNumber { get; set; }
        public string relativePath { get; set; }
        public string path { get; set; }
        public object size { get; set; }
        public string dateAdded { get; set; }
        public string sceneName { get; set; }
        public Quality quality { get; set; }
        public bool qualityCutoffNotMet { get; set; }
        public int id { get; set; }
    }
}
