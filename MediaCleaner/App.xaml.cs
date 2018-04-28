using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using ContextMenuStrip = System.Windows.Forms.ContextMenuStrip;
using ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem;
using System.Collections.Generic;
using MediaCleaner.Emby;
using System.Threading.Tasks;
using MediaCleaner.Sonarr;

namespace MediaCleaner
{
    public partial class App : Application 
    {
        public static NotifyIcon notifyIcon;
        ContextMenuStrip contextMenu;
        public ToolStripMenuItem openMonitor;
        ToolStripMenuItem exitApplication;
        DispatcherTimer dispatcherTimer;
        SonarrApi sonarrApi;

        protected override void OnStartup(StartupEventArgs e)
        {

            sonarrApi = new SonarrApi();

            Log.Info("APP is running.");

            App.notifyIcon = new NotifyIcon();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MediaCleaner.Resource." + "icon_stopped.ico"))
            {
                notifyIcon.Icon = new Icon(stream);
            }
            notifyIcon.Visible = true;

            notifyIcon.Text = "Starting...";
            notifyIcon.Visible = true;

            contextMenu = new ContextMenuStrip();
            openMonitor = new ToolStripMenuItem();
            exitApplication = new ToolStripMenuItem();

            notifyIcon.ContextMenuStrip = contextMenu;

            openMonitor.Text = "Settings";
            openMonitor.Click += new EventHandler(OpenSettings);
            contextMenu.Items.Add(openMonitor);

            exitApplication.Text = "Exit";
            exitApplication.Click += new EventHandler(ShutdownApp);
            contextMenu.Items.Add(exitApplication);

            Config.Debug = false;

            if(validateSettings())
            {
                start();
            }
            else
            {
                wrongSettings();
            }
        }

        private bool validateSettings()
        {
            if(Config.MediaServer == 0)
            {
                if (Config.plexUsername == "" || Config.plexUuid == "" || Config.plexAccessToken == "")
                    return false;
            }
            else if(Config.MediaServer == 1)
            {
                if (Config.embyUsername == "" || Config.embyUserid == "" || Config.embyAccessToken == "")
                    return false;
            }

            if (Config.sonarrAPIKey == "" || Config.episodesToKeep < 0 ||
                Config.Interval < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void wrongSettings()
        {
            notifyIcon.Text = "Some of the settings are wrong!";

            Settings settings = new Settings();
            settings.ShowDialog();
            if (settings.SettingsChanged && validateSettings())
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
            MediaServer mediaServer;
            if (Config.MediaServer == 1)
                mediaServer = new Emby.Emby();
            else if (Config.MediaServer == 0)
                mediaServer = new Plex.Plex();
            else
                return;

            try
            {
                mediaServer.checkConnection();
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

            seriesList = sonarrApi.getSeriesList();

            var fileCounter = 0;
            var deletableCounter = 0;
            var episodeCounter = 0;

            foreach (var series in seriesList)
            {
                var asd = new List<Episode>();
                try
                {
                    asd = sonarrApi.getEpisodebySeries(series.id.ToString());
                }
                catch (System.Net.WebException exc)
                {
                    Log.Error(string.Format("Exception: [Sonarr] {0}", exc.Status));
                }

                episodeCounter = 0;

                for (var i = asd.Count - 1; i > 0; i--)
                {
                    var item = asd[i];


                    if (item.hasFile == true)
                    {
                        Item UserItem;

                        bool deletable = false;
                        try
                        {
                            UserItem = mediaServer.getItem(item);
                        }
                        catch (System.Net.WebException exc)
                        {
                            Log.Error(string.Format("Exception: [EMBY] {0}", exc.Status));
                            continue;
                        }
                        catch
                        {
                            continue;
                        }

                        if (UserItem == null)
                        {
                            Log.Info(string.Format("Couldnt find about for this one in Emby: {0}", item.episodeFile.path));
                            continue;
                        }

                        if (Math.Round((DateTime.Now - DateTime.Parse(item.episodeFile.dateAdded)).TotalHours) > Config.hoursToKeep && UserItem.Played == true && episodeCounter > Config.episodesToKeep-1)
                        {
                            if (Config.favoriteEpisodes)
                            {
                                if (!UserItem.IsFavorite)
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

                        Log.Info(string.Format("[{0}] - [{1}] [Sonarr file id]: {2}; Watched: {3}; dateAdded: {4}; time between added and now: {5} Hours; Favorite: {6}; Deletable: {7}", UserItem.SeriesName, UserItem.EpisodeTitle, item.episodeFileId, UserItem.Played, item.episodeFile.dateAdded, Math.Round((DateTime.Now - UserItem.dateAdded).TotalHours), UserItem.IsFavorite, deletable));

                        if (!Config.Debug && deletable)
                        {
                            Log.Info(string.Format("            Deleted: {0}", item.episodeFile.path));
                            Log.Deleted(string.Format("Deleted: {0}", item.episodeFile.path));
                            sonarrApi.deleteEpisodeFile(item.episodeFileId);
                        }

                        fileCounter++;
                        if(UserItem.Played)
                            episodeCounter++;
                    }
                }
            }

            Log.Info(string.Format("Files: {0}", fileCounter));
            Log.Info(string.Format("Deletable files: {0}", deletableCounter));
            Log.Info(string.Format("Non-Deletable files: {0}", fileCounter - deletableCounter));
        }

        private void start()
        {
            notifyIcon.Text = "Sonarr - Emby Cleaner";

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
    }
}
