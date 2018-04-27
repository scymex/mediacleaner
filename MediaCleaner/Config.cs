using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCleaner
{
    public class Config
    {
        // Sonarr server ip and port
        public static string SonarrAddress
        {
            get
            {
                return Properties.Settings.Default.sonarr_address;
            }
            set
            {
                Properties.Settings.Default.sonarr_address = value;
                Properties.Settings.Default.Save();
            }
        }

        // Emby server ip and port
        public static string EmbyAddress
        {
            get
            {
                return Properties.Settings.Default.emby_address;
            }
            set
            {
                Properties.Settings.Default.emby_address = value;
                Properties.Settings.Default.Save();
            }
        }

        // Plex server ip and port
        public static string PlexAddress
        {
            get
            {
                return Properties.Settings.Default.plex_address;
            }
            set
            {
                Properties.Settings.Default.plex_address = value;
                Properties.Settings.Default.Save();
            }
        }

        // Emby Username
        public static string embyUsername
        {
            get
            {
                return Properties.Settings.Default.emby_username;
            }
            set
            {
                Properties.Settings.Default.emby_username = value;
                Properties.Settings.Default.Save();
            }
        }

        // Emby User ID

        public static string embyUserid
        {
            get
            {
                return Properties.Settings.Default.emby_userid;
            }
            set
            {
                Properties.Settings.Default.emby_userid = value;
                Properties.Settings.Default.Save();
            }
        }

        // Emby Access Token

        public static string embyAccessToken
        {
            get
            {
                return Properties.Settings.Default.emby_accesstoken;
            }
            set
            {
                Properties.Settings.Default.emby_accesstoken = value;
                Properties.Settings.Default.Save();
            }
        }

        // Plex Access Token

        public static string plexAccessToken
        {
            get
            {
                return Properties.Settings.Default.plex_accestoken;
            }
            set
            {
                Properties.Settings.Default.plex_accestoken = value;
                Properties.Settings.Default.Save();
            }
        }

        // Plex Username

        public static string plexUsername
        {
            get
            {
                return Properties.Settings.Default.plex_username;
            }
            set
            {
                Properties.Settings.Default.plex_username = value;
                Properties.Settings.Default.Save();
            }
        }

        // Plex uuid

        public static string plexUuid
        {
            get
            {
                return Properties.Settings.Default.plex_uuid;
            }
            set
            {
                Properties.Settings.Default.plex_uuid = value;
                Properties.Settings.Default.Save();
            }
        }

        // Plex uuid

        public static string plexClientToken
        {
            get
            {
                return Properties.Settings.Default.plex_clienttoken;
            }
            set
            {
                Properties.Settings.Default.plex_clienttoken = value;
                Properties.Settings.Default.Save();
            }
        }

        // Sonarr API key

        public static string sonarrAPIKey
        {
            get
            {
                return Properties.Settings.Default.sonarr_apikey;
            }
            set
            {
                Properties.Settings.Default.sonarr_apikey = value;
                Properties.Settings.Default.Save();
            }
        }

        // Debug

        public static bool Debug
        {
            get
            {
                return Properties.Settings.Default.debug;
            }
            set
            {
                Properties.Settings.Default.debug = value;
                Properties.Settings.Default.Save();
            }
        }

        // Interval (between two checks)

        public static int Interval
        {
            get
            {
                return Properties.Settings.Default.interval;
            }
            set
            {
                Properties.Settings.Default.interval = value;
                Properties.Settings.Default.Save();
            }
        }

        // Minimum time (in hours) before deleting the episode

        public static int hoursToKeep
        {
            get
            {
                return Properties.Settings.Default.hoursToKeep;
            }
            set
            {
                Properties.Settings.Default.hoursToKeep = value;
                Properties.Settings.Default.Save();
            }
        }

        // Minimum episode quantity to keep

        public static int episodesToKeep
        {
            get
            {
                return Properties.Settings.Default.episodesToKeep;
            }
            set
            {
                Properties.Settings.Default.episodesToKeep = value;
                Properties.Settings.Default.Save();
            }
        }

        // True = we'll keep the favorite episodes (emby)

        public static bool favoriteEpisodes
        {
            get
            {
                return Properties.Settings.Default.favoriteEpisodes;
            }
            set
            {
                Properties.Settings.Default.favoriteEpisodes = value;
                Properties.Settings.Default.Save();
            }
        }


        // MediaServerType

        public static int MediaServer
        {
            get
            {
                return Properties.Settings.Default.MediaServer;
            }
            set
            {
                Properties.Settings.Default.MediaServer = value;
                Properties.Settings.Default.Save();
            }
        }

    }
}
