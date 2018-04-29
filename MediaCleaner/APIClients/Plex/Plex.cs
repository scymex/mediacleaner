using System.Collections.Generic;
using System.Linq;

namespace MediaCleaner.Plex
{
    class Plex : IMediaServer
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

        public DataModels.Episode getItem(string episodePath)
        {
            if (UserItemList is null)
                UserItemList = plexAPI.getUserItems();

            var PlexItem = UserItemList.FirstOrDefault(item1 => item1.Media.Any(sourcelist => sourcelist.Part.Any(source => source.file == episodePath)));
            var UserItem = new DataModels.Episode();

            var played = false;
            if (PlexItem.viewCount > 0)
                played = true;

            UserItem.SeriesName = PlexItem.grandparentTitle;
            UserItem.SeasonNumber = PlexItem.parentTitle;
            UserItem.EpisodeNumber = PlexItem.index.ToString();
            UserItem.EpisodeTitle = PlexItem.title;
            UserItem.FilePath = episodePath;
            UserItem.IsFavorite = false;
            UserItem.Played = played;
            UserItem.dateAdded = new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(PlexItem.addedAt);

            return UserItem;
        }

    }
}
