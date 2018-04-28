using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaCleaner.Sonarr;

namespace MediaCleaner.Emby
{
    class Emby : MediaServer
    {
        EmbyApi embyAPI;
        List<UserItem> UserItemList;

        public Emby ()
        {
            embyAPI = new EmbyApi();
        }


        public bool checkConnection()
        {
            return embyAPI.checkConnection();
        }

        public Item getItem(string episodePath)
        {
            if (UserItemList is null)
                UserItemList = embyAPI.getUserItems();

            var embyItem = UserItemList.FirstOrDefault(item1 => item1.MediaSources.Any(mediasource => mediasource.Path == episodePath));
            var UserItem = new Item();

            UserItem.SeriesName = embyItem.SeriesName;
            UserItem.Season = embyItem.SeasonName;
            UserItem.Episode = embyItem.IndexNumber.ToString();
            UserItem.EpisodeTitle = embyItem.Name;
            UserItem.FilePath = episodePath;
            UserItem.IsFavorite = embyItem.UserData.IsFavorite;
            UserItem.Played = embyItem.UserData.Played;
            UserItem.dateAdded = DateTime.Parse(embyItem.DateCreated);

            return UserItem;
        }

    }
}
