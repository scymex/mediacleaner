using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaCleaner.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class DataObject
    {
        public string title { get; set; }
    }


    public partial class Settings : Window
    {
        public bool SettingsChanged = false;

        public Settings()
        {

            InitializeComponent();

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MediaCleaner.Resource." + "icon_running.ico"))
            {
                this.Icon = BitmapFrame.Create(stream);
            }

            Initiate();
        }

        private void Initiate()
        {
            sonarr_apikey.Text = Config.sonarrAPIKey;
            sonarr_address.Text = Config.SonarrAddress;
            // Hours to keep
            hoursToKeep.Text = Config.hoursToKeep.ToString();
            // Episodes to keep
            episodesToKeep.Text = Config.episodesToKeep.ToString();
            // Interval between two checks
            interval.Text = Config.Interval.ToString();

            mediaserver.SelectedIndex = Config.MediaServer;

            favoriteEpisodes.IsChecked = Config.favoriteEpisodes;

            plex.Visibility = Visibility.Hidden;
            emby.Visibility = Visibility.Hidden;

            // 0 == PLEX
            if (Config.MediaServer == 0)
            {
                intitiatePlex();
                plex.Visibility = Visibility.Visible;
            } else if(Config.MediaServer == 1)
            {
                intitiateEmby();
                emby.Visibility = Visibility.Visible;
            }

            // Debug checkbox
            debug.IsChecked = Config.Debug;
            trace.IsChecked = Config.Trace;
        }

        private void intitiateEmby()
        {
            emby_username.Text = (Config.EmbyAddress == "") ? "-" : Config.embyUsername;
            emby_userid.Text = (Config.embyUserid == "") ? "-" : Config.embyUserid;
            emby_address.Text = (Config.EmbyAddress == "") ? "" : Config.EmbyAddress;

            if (Config.embyUsername != "" || Config.embyUserid != "" || Config.embyAccessToken != "")
            {
                changeUser_emby.Visibility = Visibility.Visible;
                login_emby.Visibility = Visibility.Hidden;
            }
        }

        private void intitiatePlex()
        {
            plex_username.Text = (Config.plexUsername == "") ? "-" : Config.plexUsername;
            plex_userid.Text = (Config.plexUuid == "") ? "-" : Config.plexUuid;
            plex_address.Text = (Config.PlexAddress == "") ? "" : Config.PlexAddress;

            if (Config.embyUsername != "" || Config.embyUserid != "" || Config.plexAccessToken != "")
            {
                changeUser_plex.Visibility = Visibility.Visible;
                login_plex.Visibility = Visibility.Hidden;
            }
        }

        private void Debug_Checked(object sender, RoutedEventArgs e)
        {
            Config.Debug = true;

        }

        private void Debug_Unchecked(object sender, RoutedEventArgs e)
        {
            Config.Debug = false;
            if (trace.IsChecked ?? false)
                trace.IsChecked = false;
        }

        private void Trace_Checked(object sender, RoutedEventArgs e)
        {
            Config.Trace = true;
            if(!debug.IsChecked ?? false)
                debug.IsChecked = true;
        }

        private void Trace_Unchecked(object sender, RoutedEventArgs e)
        {
            Config.Trace = false;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(episodesToKeep.Text) < 0 ||
                Int32.Parse(interval.Text) < 0 ||
                sonarr_address.Text == "" ||
                Int32.Parse(hoursToKeep.Text) < 0 ||
                sonarr_apikey.Text == "" ||
                (emby_address.Text == "" && plex_address.Text == "") )
            {
                MessageBox.Show("Some of the settings are not valid!", "Wrong settings!");
            }
            else
            {
                SettingsChanged = true;
                this.Close();
            }

            // Emby
            Config.EmbyAddress = emby_address.Text;

            // Plex
            Config.PlexAddress = plex_address.Text;

            Config.SonarrAddress = sonarr_address.Text;
            Config.sonarrAPIKey = sonarr_apikey.Text;
            Config.hoursToKeep = Int32.Parse(hoursToKeep.Text);
            Config.episodesToKeep = Int32.Parse(episodesToKeep.Text);
            Config.Interval = Int32.Parse(interval.Text);
            Config.favoriteEpisodes = favoriteEpisodes.IsChecked.HasValue ? true : false;
            Config.MediaServer = mediaserver.SelectedIndex;
        }

        private void Login_Plex(object sender, RoutedEventArgs e)
        {
            if(plex_address.Text == "")
            {
                MessageBox.Show("You have to give me an Address first!");
                return;
            }

            Config.PlexAddress = plex_address.Text;

            LoginPlex loginscreen = new LoginPlex("", 0);
            loginscreen.ShowDialog();
            if(loginscreen.LoginSuccessful)
            {
                plex_username.Text = Config.plexUsername;
                plex_userid.Text = Config.plexUuid;
                SettingsChanged = true;

                changeUser_plex.Visibility = Visibility.Visible;
                login_plex.Visibility = Visibility.Hidden;
            }
        }

        private void Login_Emby(object sender, RoutedEventArgs e)
        {
            if (emby_address.Text == "")
            {
                MessageBox.Show("You have to give me an Address first!");
                return;
            }

            Config.EmbyAddress = emby_address.Text;

            LoginEmby login = new LoginEmby();
            login.ShowDialog();
            if (login.LoginSuccessful)
            {
                emby_username.Text = Config.embyUsername;
                emby_userid.Text = Config.embyUserid;
                SettingsChanged = true;

                changeUser_emby.Visibility = Visibility.Visible;
                login_emby.Visibility = Visibility.Hidden;
            }
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selecteditem = (sender as ComboBox).SelectedIndex;
            favoriteEpisodes.IsEnabled = true;
            favoriteEpisodes.IsChecked = Properties.Settings.Default.favoriteEpisodes;

            plex.Visibility = Visibility.Hidden;
            emby.Visibility = Visibility.Hidden;


            if (selecteditem == 0)
            {
                plex.Visibility = Visibility.Visible;

                favoriteEpisodes.IsEnabled = false;
                favoriteEpisodes.IsChecked = false;
            }
            else if (selecteditem == 1)
            {
                intitiateEmby();
                emby.Visibility = Visibility.Visible;
            }


        }
    }
}
