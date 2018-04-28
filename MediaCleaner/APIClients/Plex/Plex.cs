using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaCleaner.Sonarr;

namespace MediaCleaner.Plex
{
    class Plex : MediaServer
    {
        PlexApi plexAPI;
        List<Episode> UserItemList;


        public Plex ()
        {
            plexAPI = new PlexApi();
        }
        public bool checkConnection()
        {
            return plexAPI.checkConnection();
        }

        public Item getItem(string episodePath)
        {
            if (UserItemList is null)
                UserItemList = plexAPI.getUserItems();

            var PlexItem = UserItemList.FirstOrDefault(item1 => item1.Media.Any(sourcelist => sourcelist.Part.Any(source => source.file == episodePath)));
            var UserItem = new Item();

            var played = false;
            if (PlexItem.viewCount > 0)
                played = true;

            UserItem.SeriesName = PlexItem.grandparentTitle;
            UserItem.Season = PlexItem.parentTitle;
            UserItem.Episode = PlexItem.index.ToString();
            UserItem.EpisodeTitle = PlexItem.title;
            UserItem.FilePath = episodePath;
            UserItem.IsFavorite = false;
            UserItem.Played = played;
            UserItem.dateAdded = new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(PlexItem.addedAt);

            return UserItem;
        }

    }
}
