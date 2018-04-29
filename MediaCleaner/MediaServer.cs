using MediaCleaner.DataModels;

namespace MediaCleaner
{
    public class MediaServer:IMediaServer
    {
        IMediaServer mServer;

        public MediaServer()
        {
            switch(Config.MediaServer)
            {
                case 0:
                    mServer = new Plex.Plex();
                break;
                case 1:
                    mServer = new Emby.Emby();
                break;

            }
        }

        public bool checkConnection()
        {
            return mServer.checkConnection();
        }

        public Episode getItem(string EpisodePath)
        {
            return mServer.getItem(EpisodePath);
        }
    }
}
