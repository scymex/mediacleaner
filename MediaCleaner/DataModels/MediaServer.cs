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
        Item getItem(Episode sonarritem);
        bool checkConnection();
    }


    public class Item
    {
        public bool IsFavorite { get; set; }
        public bool Played { get; set; }
        public DateTime dateAdded { get; set; }
        public string SeriesName { get; set; }
        public string EpisodeTitle { get; set; }
    }
}
