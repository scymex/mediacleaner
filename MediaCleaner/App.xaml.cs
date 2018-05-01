using System;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using ContextMenuStrip = System.Windows.Forms.ContextMenuStrip;
using ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaCleaner.Sonarr;

namespace MediaCleaner
{
    public partial class App : Application 
    {
        public static NotifyIcon notifyIcon;
        ContextMenuStrip contextMenu;
        ToolStripMenuItem runCleaningNow;
        ToolStripMenuItem openSettings;
        ToolStripMenuItem exitApplication;
        DispatcherTimer dispatcherTimer;
        MediaServer mServer;
        SonarrApi sonarrApi;

        protected override void OnStartup(StartupEventArgs e)
        {
            foreach (string arg in e.Args)
            {
                if (arg == "/debug")
                    Config.Debug = true;
            }

            if(Config.Debug)
                Log.Info("APP is running and its in debug mode.");
            else
                Log.Info("APP is running.");

            mServer = new MediaServer();
            sonarrApi = new SonarrApi();

            App.notifyIcon = new NotifyIcon();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MediaCleaner.Resource." + "icon_stopped.ico"))
            {
                notifyIcon.Icon = new Icon(stream);
            }
            notifyIcon.Visible = true;

            notifyIcon.Text = "Starting...";
            notifyIcon.Visible = true;

            contextMenu = new ContextMenuStrip();
            runCleaningNow = new ToolStripMenuItem();
            openSettings = new ToolStripMenuItem();
            exitApplication = new ToolStripMenuItem();

            notifyIcon.ContextMenuStrip = contextMenu;

            runCleaningNow.Text = "Run a cleaning phase now";
            runCleaningNow.Click += new EventHandler(RunCleaningNow);
            contextMenu.Items.Add(runCleaningNow);

            openSettings.Text = "Settings";
            openSettings.Click += new EventHandler(OpenSettings);
            contextMenu.Items.Add(openSettings);

            exitApplication.Text = "Exit";
            exitApplication.Click += new EventHandler(ShutdownApp);
            contextMenu.Items.Add(exitApplication);

            if(checkSettings())
            {
                start();
            }
            else
            {
                wrongSettings();
            }
        }

        private bool checkSettings()
        {
            if (!mServer.checkSettings())
                return false;

            if (!sonarrApi.checkSettings())
                return false;

            if (Config.episodesToKeep < 0 || Config.Interval < 0)
                return false;

            return true;
        }

        private void wrongSettings()
        {
            notifyIcon.Text = "Some of the settings are wrong!";

            Settings settings = new Settings();
            settings.ShowDialog();
            if (settings.SettingsChanged && checkSettings())
            {
                start();
                settings.SettingsChanged = false;
            } else
            {
                wrongSettings();
            }
        }

        private void TheBigThingThicker(object sender, EventArgs e)
        {
            Task task = Task.Run((Action)TheBigThing);
        }

        private void TheBigThing ()
        {
            if (Config.Debug)
            {
                Log.Debug("| Debugging is ON!");
                Log.Debug("| SETTINGS:");
                Log.Debug(string.Format("|    Emby Username:                    {0}", Config.embyUsername));
                Log.Debug(string.Format("|    Emby User ID:                     {0}", Config.embyUserid));
                Log.Debug(string.Format("|    Emby Access Token:                {0}", Config.embyAccessToken));
                Log.Debug(string.Format("|    Plex Username:                    {0}", Config.plexUsername));
                Log.Debug(string.Format("|    Plex Uuid:                        {0}", Config.plexUuid));
                Log.Debug(string.Format("|    Plex Access Token:                {0}", Config.plexAccessToken));
                Log.Debug(string.Format("|    MediaServer Type id:              {0}", Config.MediaServer));
                Log.Debug(string.Format("|    Emby Access Token:                {0}", Config.embyAccessToken));
                Log.Debug(string.Format("|    Sonarr API key:                   {0}", Config.sonarrAPIKey));
                Log.Debug(string.Format("|    Interval:                         {0}", Config.Interval));
                Log.Debug(string.Format("|    Minimum hours to keep:            {0}", Config.hoursToKeep));
                Log.Debug(string.Format("|    Minimum episode quantity to keep: {0}", Config.episodesToKeep));
                Log.Debug(string.Format("|    Keep favorite episodes:           {0}", Config.embyUsername));
            }

            var seriesList = new List<Series>();
            var error = false;

            try
            {
                mServer.checkConnection();
            } catch (System.Net.WebException exc)
            {
                Log.Error(string.Format("Exception: [MediaServer] {0}", exc.Status));
                error = true;
            }

            try
            {
                sonarrApi.checkConnection();
            }
            catch (System.Net.WebException exc)
            {
                Log.Error(string.Format("Exception: [Sonarr] {0}", exc.Status));
                error = true;
            }

            if (!error && !sonarrApi.CheckApikey()) {
                Log.Error("Sonarr: Unauthorized");
                error = true;
            }

            if (error)
            {
                dispatcherTimer.Interval = TimeSpan.FromMinutes(5);
                Log.Debug(string.Format("Next thick at: {0}", DateTime.Now.AddMinutes(5).ToString("yy/MM/dd H:mm:ss")));
                return;
            }
            else
            {
                dispatcherTimer.Interval = TimeSpan.FromMinutes(Config.Interval);
                Log.Debug(string.Format("Next thick at: {0}", DateTime.Now.AddMinutes(Config.Interval).ToString("yy/MM/dd H:mm:ss")));
            }

            var fileHandler = new FileHandler(sonarrApi, mServer);
            var episodeList = fileHandler.getEpisodeListbyOrder(fileHandler.getEpisodeList());

            var fileCounter = 0;
            var deletableCounter = 0;
            var episodeCounter = 0;

            for (int i = 0; i < episodeList.Count - 1; i++) {
                var file = episodeList[i];
                if (i != 0)
                {
                    if (episodeList[i].SeriesName != episodeList[i - 1].SeriesName)
                        episodeCounter = 0;
                }

                bool deletable = false;
                string not_Deletable = "";

                if (Math.Round((DateTime.Now - file.dateAdded).TotalHours) > Config.hoursToKeep
                    && file.Played == true 
                    && episodeCounter > Config.episodesToKeep - 1)
                {
                    if (Config.favoriteEpisodes)
                    {
                        if (!file.IsFavorite)
                        {
                            deletable = true;
                            deletableCounter++;
                        }
                    }
                    else
                    {
                        deletable = true;
                        deletableCounter++;
                    }
                }

                if(file.Played == false)
                    not_Deletable += "Played; ";
                if(Config.favoriteEpisodes && file.IsFavorite)
                    not_Deletable += "favoriteEpisode; ";
                if (episodeCounter < Config.episodesToKeep - 1)
                    not_Deletable += "episodeCounter; ";
                if(Math.Round((DateTime.Now - file.dateAdded).TotalHours) < Config.hoursToKeep)
                    not_Deletable += "hourstoKeep; ";

                Log.Info(string.Format("[{0}] - Season: {1} - Episode: {2} - [{3}]: FilePath: {4}; IsFavorite: {5}; Played: {6}; dateAdded: {7}; now-dateAdded: {8}; deletable: {9}; Reason why its not deletable: {10}",
                    file.SeriesName,
                    file.SeasonNumber,
                    file.EpisodeNumber,
                    file.EpisodeTitle,
                    file.FilePath,
                    file.IsFavorite,
                    file.Played,
                    file.dateAdded,
                    Math.Round((DateTime.Now - file.dateAdded).TotalHours),
                    deletable,
                    not_Deletable
                    ));

                if (deletable)
                {
                    Log.Info(string.Format("File deleted: {0}", file.FilePath));
                    Log.Deleted(string.Format("Deleted: {0}", file.FilePath));
                    if (!Config.Debug)
                    {
                        fileHandler.deleteFile(file.FilePath);
                    }
                }

                fileCounter++;
                if (file.Played)
                    episodeCounter++;
            }

            Log.Info(string.Format("Files: {0}", fileCounter));
            Log.Info(string.Format("Deletable files: {0}", deletableCounter));
            Log.Info(string.Format("Non-Deletable files: {0}", fileCounter - deletableCounter));
        }

        private void start()
        {
            notifyIcon.Text = "MediaCleaner";

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MediaCleaner.Resource." + "icon_running.ico"))
            {
                notifyIcon.Icon = new Icon(stream);
            }
            //  DispatcherTimer setup
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(TheBigThingThicker);
            dispatcherTimer.Interval = TimeSpan.FromMinutes(Config.Interval);
            dispatcherTimer.Start();
            Task task = Task.Run((Action)TheBigThing);
            Log.Info("A Cleaning phase just started.");
        }

        private void stop()
        {
            dispatcherTimer.Stop();
        }



        private void OpenSettings(Object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
            if(settings.SettingsChanged)
            {
                dispatcherTimer.Interval = TimeSpan.FromMinutes(Config.Interval);
                settings.SettingsChanged = false;
            }
        }

        private void ShutdownApp(Object sender, EventArgs e)
        {
            notifyIcon.Dispose();
            Current.Shutdown();
        }

        private void RunCleaningNow(Object sender, EventArgs e)
        {
            Task task = Task.Run((Action)TheBigThing);
        }
    }
}
