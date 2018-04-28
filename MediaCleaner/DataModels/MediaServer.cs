using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaCleaner.Sonarr;

namespace MediaCleaner
{
    interface MediaServer
    {
        Item getItem(string EpisodePath);
        bool checkConnection();
    }


    public class Item
    {
        public string SeriesName { get; set; }
        public string Season { get; set; }
        public string Episode { get; set; }
        public string EpisodeTitle { get; set; }
        public string FilePath { get; set; }
        public bool IsFavorite { get; set; }
        public bool Played { get; set; }
        public DateTime dateAdded { get; set; }
    }

}
