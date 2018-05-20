using MediaCleaner.DataModels;
using MediaCleaner.MediaServers;

namespace MediaCleaner
{
    public class MediaServer:IMediaServer
    {
        IMediaServer mServer;
        private int mSType;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public MediaServer()
        {
            initMediaServer();
        }

        public void initMediaServer()
        {
            switch (Config.MediaServer)
            {
                case 0:
                    mServer = new Plex();
                    break;
                case 1:
                    mServer = new Emby();
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
            return mServer.checkSettings();
        }

    }
}
