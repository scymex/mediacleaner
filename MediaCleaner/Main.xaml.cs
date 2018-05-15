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
using MediaCleaner.APIClients;
using MediaCleaner.DataModels.Sonarr;
using MediaCleaner.Views;

namespace MediaCleaner
{
    public partial class MainApplication : Application 
    {
        public static NotifyIcon notifyIcon;
        ContextMenuStrip contextMenu;
        ToolStripMenuItem runCleaningNow;
        ToolStripMenuItem openSettings;
        ToolStripMenuItem exitApplication;
        DispatcherTimer dispatcherTimer;
        MediaServer mServer;
        SonarrApi sonarrApi;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            foreach (string arg in e.Args)
            {
                if (arg == "/debug")
                    Config.Debug = true;
            }

            if (Config.Debug)
                Log.EnableDebug();

            logger.Info("The application is running.");
            logger.Debug("The application is running in debug mode.");

            mServer = new MediaServer();
            sonarrApi = new SonarrApi();

            notifyIcon = new NotifyIcon();
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
            if (!checkSettings())
                return;

            logger.Debug("Interval: \"{0}\"; Minimum time to keep files: \"{1}\"; Minimum episode quantity to keep: \"{2}\"; Keeping favorite episodes: \"{3}\";", Config.Interval, Config.hoursToKeep, Config.episodesToKeep, Config.favoriteEpisodes);

            var seriesList = new List<Series>();
            var error = false;

            try
            {
                mServer.checkConnection();
            } catch (System.Net.WebException exc)
            {
                logger.Error("Exception: [MediaServer] {0}", exc.Status);
                error = true;
            }

            try
            {
                sonarrApi.checkConnection();
            }
            catch (System.Net.WebException exc)
            {
                logger.Error("Exception: [Sonarr] {0}", exc.Status);
                error = true;
            }

            if (!error && !sonarrApi.CheckApikey()) {
                logger.Error("Sonarr: Unauthorized");
                error = true;
            }

            if (error)
            {
                dispatcherTimer.Interval = TimeSpan.FromMinutes(5);
                logger.Debug("Next thick at: {0}", DateTime.Now.AddMinutes(5).ToString("yy/MM/dd H:mm:ss"));
                return;
            }
            else
            {
                dispatcherTimer.Interval = TimeSpan.FromMinutes(Config.Interval);
                logger.Debug("Next thick at: {0}", DateTime.Now.AddMinutes(Config.Interval).ToString("yy/MM/dd H:mm:ss"));
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

                logger.Info("[{0}] - Season: {1} - Episode: {2} - [{3}]: FilePath: {4}; IsFavorite: {5}; Played: {6}; dateAdded: {7}; now-dateAdded: {8}; deletable: {9}; Reason why its not deletable: {10}",
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
                    );

                if (deletable)
                {
                    logger.Info("File deleted: {0}", file.FilePath);

                    fileHandler.deleteFile(file.FilePath);
                }

                fileCounter++;
                if (file.Played)
                    episodeCounter++;
            }

            logger.Info("Files: {0}", fileCounter);
            logger.Info("Deletable files: {0}", deletableCounter);
            logger.Info("Non-Deletable files: {0}", fileCounter - deletableCounter);
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
            logger.Info("A Cleaning phase just started.");
        }

        private void stop()
        {
            dispatcherTimer.Stop();
        }



        private void OpenSettings(Object sender, EventArgs e)
        {
            var preSettingsInterval = Config.Interval;
            Settings settings = new Settings();
            settings.ShowDialog();
            if(settings.SettingsChanged)
            {
                if (Config.Debug)
                    Log.EnableDebug();

                if (preSettingsInterval != Config.Interval)
                {
                    dispatcherTimer.Interval = TimeSpan.FromMinutes(Config.Interval);
                    logger.Debug("Some of the settings are changed, including the interval. New interval: {0}", Config.Interval);
                }
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
