using MediaCleaner.DataModels;

namespace MediaCleaner
{
    interface IMediaServer
    {
        Episode getItem(string EpisodePath);
        bool checkConnection();
        bool checkSettings();
    }
}
