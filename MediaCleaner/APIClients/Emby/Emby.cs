using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MediaCleaner.DataModels;

namespace MediaCleaner.Emby
{
    class Emby : IMediaServer
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

        public Episode getItem(string episodePath)
        {
            if (UserItemList is null)
                UserItemList = embyAPI.getUserItems();

            var embyItem = UserItemList.FirstOrDefault(item1 => item1.MediaSources.Any(mediasource => mediasource.Path == episodePath));
            var UserItem = new Episode();

            UserItem.SeriesName = embyItem.SeriesName;
            try { UserItem.SeasonNumber = Int32.Parse(Regex.Replace(embyItem.SeasonName, "[^0-9]+", string.Empty)); } catch { UserItem.SeasonNumber = 0; }
            UserItem.EpisodeNumber = embyItem.IndexNumber;
            UserItem.EpisodeTitle = embyItem.Name;
            UserItem.FilePath = episodePath;
            UserItem.IsFavorite = embyItem.UserData.IsFavorite;
            UserItem.Played = embyItem.UserData.Played;
            UserItem.dateAdded = DateTime.Parse(embyItem.DateCreated);

            return UserItem;
        }

    }
}
