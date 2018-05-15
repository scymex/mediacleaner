using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MediaCleaner.APIClients;
using MediaCleaner.DataModels;
using MediaCleaner.DataModels.Emby;

namespace MediaCleaner.MediaServers
{
    class Emby : IMediaServer
    {
        EmbyClient embyAPI;
        List<UserItem> UserItemList;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Emby ()
        {
            embyAPI = new EmbyClient();
        }


        public bool checkConnection()
        {
            return embyAPI.checkConnection();
        }

        public bool checkSettings()
        {
            logger.Debug("[Mediaserver Emby] username: \"{0}\"; embyUserid: \"{1}\"; embyAccessToken: \"{2}\"; embyAddress: \"{3}\";", Config.embyUsername, Config.embyUserid, Config.embyAccessToken, Config.EmbyAddress);

            if (Config.embyUsername == "" || Config.embyUserid == "" || Config.embyAccessToken == "" || Config.EmbyAddress == "")
                return false;

            return true;
        }

        public Episode getItem(string episodePath)
        {
            if (UserItemList is null)
                UserItemList = embyAPI.getUserItems();

            var embyItem = UserItemList.FirstOrDefault(item1 => item1.MediaSources.Any(mediasource => mediasource.Path == episodePath));

            if (embyItem == null)
                return null;

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
