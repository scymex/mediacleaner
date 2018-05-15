using System.Collections.Generic;

namespace MediaCleaner.DataModels.Emby
{
    public class Configuration
    {
        public bool PlayDefaultAudioTrack { get; set; }
        public bool DisplayMissingEpisodes { get; set; }
        public bool DisplayUnairedEpisodes { get; set; }
        public List<object> GroupedFolders { get; set; }
        public string SubtitleMode { get; set; }
        public bool DisplayCollectionsView { get; set; }
        public bool EnableLocalPassword { get; set; }
        public List<object> OrderedViews { get; set; }
        public List<object> LatestItemsExcludes { get; set; }
        public bool HidePlayedInLatest { get; set; }
        public bool RememberAudioSelections { get; set; }
        public bool RememberSubtitleSelections { get; set; }
        public bool EnableNextEpisodeAutoPlay { get; set; }
    }

    public class Policy
    {
        public bool IsAdministrator { get; set; }
        public bool IsHidden { get; set; }
        public bool IsDisabled { get; set; }
        public List<object> BlockedTags { get; set; }
        public bool EnableUserPreferenceAccess { get; set; }
        public List<object> AccessSchedules { get; set; }
        public List<object> BlockUnratedItems { get; set; }
        public bool EnableRemoteControlOfOtherUsers { get; set; }
        public bool EnableSharedDeviceControl { get; set; }
        public bool EnableLiveTvManagement { get; set; }
        public bool EnableLiveTvAccess { get; set; }
        public bool EnableMediaPlayback { get; set; }
        public bool EnableAudioPlaybackTranscoding { get; set; }
        public bool EnableVideoPlaybackTranscoding { get; set; }
        public bool EnablePlaybackRemuxing { get; set; }
        public bool EnableContentDeletion { get; set; }
        public bool EnableContentDownloading { get; set; }
        public bool EnableSyncTranscoding { get; set; }
        public List<object> EnabledDevices { get; set; }
        public bool EnableAllDevices { get; set; }
        public List<object> EnabledChannels { get; set; }
        public bool EnableAllChannels { get; set; }
        public List<object> EnabledFolders { get; set; }
        public bool EnableAllFolders { get; set; }
        public int InvalidLoginAttemptCount { get; set; }
        public bool EnablePublicSharing { get; set; }
    }

    public class PublicUser
    {
        public string Name { get; set; }
        public string ServerId { get; set; }
        public string Id { get; set; }
        public bool HasPassword { get; set; }
        public bool HasConfiguredPassword { get; set; }
        public bool HasConfiguredEasyPassword { get; set; }
        public Configuration Configuration { get; set; }
        public Policy Policy { get; set; }
        public string LastLoginDate { get; set; }
        public string LastActivityDate { get; set; }
    }
}
