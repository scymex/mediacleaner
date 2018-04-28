using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCleaner.Emby
{

    public class MediaSource
    {
        public string Protocol { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string Container { get; set; }
        public string Name { get; set; }
        public bool IsRemote { get; set; }
        public string ETag { get; set; }
        public object RunTimeTicks { get; set; }
        public bool ReadAtNativeFramerate { get; set; }
        public bool IgnoreDts { get; set; }
        public bool IgnoreIndex { get; set; }
        public bool GenPtsInput { get; set; }
        public bool SupportsTranscoding { get; set; }
        public bool SupportsDirectStream { get; set; }
        public bool SupportsDirectPlay { get; set; }
        public bool IsInfiniteStream { get; set; }
        public bool RequiresOpening { get; set; }
        public bool RequiresClosing { get; set; }
        public bool SupportsProbing { get; set; }
        public bool EnableMpDecimate { get; set; }
        public bool RequiresLooping { get; set; }
        public string VideoType { get; set; }
        public List<object> MediaStreams { get; set; }
        public List<object> Formats { get; set; }
        public int Bitrate { get; set; }
        public int DefaultAudioStreamIndex { get; set; }
        public int DefaultSubtitleStreamIndex { get; set; }
    }

    public class UserData
    {
        public long PlaybackPositionTicks { get; set; }
        public int PlayCount { get; set; }
        public bool IsFavorite { get; set; }
        public string LastPlayedDate { get; set; }
        public bool Played { get; set; }
        public string Key { get; set; }
        public double? PlayedPercentage { get; set; }
    }

    public class ImageTags
    {
        public string Primary { get; set; }
    }

    public class UserItem
    {
        public string Name { get; set; }
        public string ServerId { get; set; }
        public string Id { get; set; }
        public bool HasSubtitles { get; set; }
        public string Container { get; set; }
        public string PremiereDate { get; set; }
        public string DateCreated { get; set; }
        public List<MediaSource> MediaSources { get; set; }
        public double CommunityRating { get; set; }
        public object RunTimeTicks { get; set; }
        public int ProductionYear { get; set; }
        public int IndexNumber { get; set; }
        public int ParentIndexNumber { get; set; }
        public bool IsFolder { get; set; }
        public string Type { get; set; }
        public string ParentLogoItemId { get; set; }
        public string ParentBackdropItemId { get; set; }
        public List<string> ParentBackdropImageTags { get; set; }
        public int LocalTrailerCount { get; set; }
        public UserData UserData { get; set; }
        public string SeriesName { get; set; }
        public string SeriesId { get; set; }
        public string SeasonId { get; set; }
        public string SeriesPrimaryImageTag { get; set; }
        public string SeasonName { get; set; }
        public string VideoType { get; set; }
        public ImageTags ImageTags { get; set; }
        public List<object> BackdropImageTags { get; set; }
        public string ParentLogoImageTag { get; set; }
        public string ParentThumbItemId { get; set; }
        public string ParentThumbImageTag { get; set; }
        public string LocationType { get; set; }
        public string MediaType { get; set; }
        public bool? IsHD { get; set; }
    }

    public class UserItems
    {
        public List<UserItem> Items { get; set; }
        public int TotalRecordCount { get; set; }
    }
}