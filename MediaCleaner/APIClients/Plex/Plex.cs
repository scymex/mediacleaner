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

        public Item getItem(Sonarr.Episode sonarritem)
        {
            if (UserItemList is null)
                UserItemList = plexAPI.getUserItems();

            var PlexItem = UserItemList.FirstOrDefault(item1 => item1.Media.Any(sourcelist => sourcelist.Part.Any(source => source.file == sonarritem.episodeFile.path)));
            var UserItem = new Item();

            var played = false;
            if (PlexItem.viewCount > 0)
                played = true;

            UserItem.IsFavorite = false;
            UserItem.Played = played;
            UserItem.dateAdded = DateTime.Parse(sonarritem.episodeFile.dateAdded);
            UserItem.SeriesName = PlexItem.grandparentTitle;
            UserItem.EpisodeTitle = PlexItem.title;

            return UserItem;
        }

    }
}
