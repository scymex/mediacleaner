using MediaCleaner.DataModels;

namespace MediaCleaner
{
    public class MediaServer:IMediaServer
    {
        IMediaServer mServer;
        private int mSType;

        public MediaServer()
        {
            initMediaServer();
        }

        private void initMediaServer()
        {
            switch (Config.MediaServer)
            {
                case 0:
                    mServer = new Plex.Plex();
                    break;
                case 1:
                    mServer = new Emby.Emby();
                    break;
            }

            mSType = Config.MediaServer;
        }

        private void checkMediaServer()
        {
            if (mSType != Config.MediaServer)
                initMediaServer();
        }

        public bool checkConnection()
        {
            checkMediaServer();
            return mServer.checkConnection();
        }

        public Episode getItem(string EpisodePath)
        {
            checkMediaServer();
            return mServer.getItem(EpisodePath);
        }

        public bool checkSettings()
        {
            checkMediaServer();
            Log.Debug(string.Format("[Mediaserver type id:] {0}", Config.MediaServer));

            return mServer.checkSettings();
        }
    }
}
