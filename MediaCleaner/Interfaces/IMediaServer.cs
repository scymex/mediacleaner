using MediaCleaner.DataModels;

namespace MediaCleaner
{
    interface IMediaServer
    {
        int _timestamp { get; set; }
        Episode getItem(string EpisodePath);
        bool checkConnection();
        bool checkSettings();
    }
}
