using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MediaCleaner.Emby;
using MediaCleaner.Sonarr;

namespace MediaCleaner
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
        // Plex
        Grid plexGrid;
        TextBox plexAddress;
        Button plexChangeUserBT;
        Button plexLoginBT;
        TextBlock plexUsernameTB;
        TextBlock plexUserIDTB;
        // Emby
        Grid embyGrid;
        TextBox embyAddress;
        Button embyChangeUserBT;
        Button embyLoginBT;
        TextBlock embyUsernameTB;
        TextBlock embyUserIDTB;

        TextBox output;
        TextBox apikeyTB;
        TextBox hoursToKeepTB;
        TextBox episodesToKeepTB;
        TextBox intervalTB;
        CheckBox debugCB;
        CheckBox favoriteEpisodesCB;
        ComboBox mediaServerCOB;

        SonarrApi sonarrApi;
        public bool SettingsChanged = false;

        public Settings()
        {

            InitializeComponent();

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MediaCleaner.Resource." + "icon_running.ico"))
            {
                this.Icon = BitmapFrame.Create(stream);
            }

            // Plex
            plexGrid = (Grid)this.FindName("plex");
            plexAddress = (TextBox)this.FindName("plex_address");
            plexChangeUserBT = (Button)this.FindName("changeUser_plex");
            plexLoginBT = (Button)this.FindName("login_plex");
            plexUsernameTB = (TextBlock)this.FindName("plex_username");
            plexUserIDTB = (TextBlock)this.FindName("plex_userid");

            // Emby
            embyGrid = (Grid)this.FindName("emby");
            embyAddress = (TextBox)this.FindName("emby_address");
            embyChangeUserBT = (Button)this.FindName("changeUser_emby");
            embyLoginBT = (Button)this.FindName("login_emby");
            embyUsernameTB = (TextBlock)this.FindName("emby_username");
            embyUserIDTB = (TextBlock)this.FindName("emby_userid");

            apikeyTB = (TextBox)this.FindName("sonarr_apikey");
            hoursToKeepTB = (TextBox)this.FindName("hoursToKeep");
            episodesToKeepTB = (TextBox)this.FindName("episodesToKeep");
            intervalTB = (TextBox)this.FindName("interval");
            output = (TextBox)this.FindName("tx");
            debugCB = (CheckBox)this.FindName("debug");
            favoriteEpisodesCB = (CheckBox)this.FindName("favoriteEpisodes");
            mediaServerCOB = (ComboBox)this.FindName("mediaserver");

            sonarrApi = new SonarrApi();

            Initiate();
        }

        private void Initiate()
        {
            // APikey
            if (Config.sonarrAPIKey == "")
                apikeyTB.Text = "";
            else
                apikeyTB.Text = Config.sonarrAPIKey;
            // Hours to keep
            hoursToKeepTB.Text = Config.hoursToKeep.ToString();
            // Episodes to keep
            episodesToKeepTB.Text = Config.episodesToKeep.ToString();
            // Interval between two checks
            intervalTB.Text = Config.Interval.ToString();

            mediaServerCOB.SelectedIndex = Config.MediaServer;

            favoriteEpisodes.IsChecked = Config.favoriteEpisodes;

            plexGrid.Visibility = Visibility.Hidden;
            embyGrid.Visibility = Visibility.Hidden;

            // 0 == PLEX
            if (Config.MediaServer == 0)
            {
                intitiatePlex();
                plexGrid.Visibility = Visibility.Visible;
            } else if(Config.MediaServer == 1)
            {
                intitiateEmby();
                embyGrid.Visibility = Visibility.Visible;
            }

            // Debug checkbox
            debugCB.IsChecked = Config.Debug;
        }

        private void intitiateEmby()
        {
            emby_username.Text = (Config.EmbyAddress == "") ? "-" : Config.embyUsername;
            emby_userid.Text = (Config.embyUserid == "") ? "-" : Config.embyUserid;
            embyAddress.Text = (Config.EmbyAddress == "") ? "" : Config.EmbyAddress;

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
            plexAddress.Text = (Config.EmbyAddress == "") ? "" : Config.PlexAddress;

            if (Config.embyUsername != "" || Config.embyUserid != "" || Config.plexAccessToken != "")
            {
                changeUser_plex.Visibility = Visibility.Visible;
                login_plex.Visibility = Visibility.Hidden;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Config.Debug = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Config.Debug = false;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Config.SonarrAddress = sonarr_address.Text;
            Config.sonarrAPIKey = apikeyTB.Text;
            Config.hoursToKeep = Int32.Parse(hoursToKeep.Text);
            Config.episodesToKeep = Int32.Parse(episodesToKeepTB.Text);
            Config.Interval = Int32.Parse(intervalTB.Text);
            Config.favoriteEpisodes = favoriteEpisodesCB.IsChecked.HasValue ? true : false;
            Config.MediaServer = mediaServerCOB.SelectedIndex;

            // Emby
            Config.EmbyAddress = embyAddress.Text;

            // Plex
            Config.PlexAddress = plexAddress.Text;

            if (sonarrApi.CheckApikey() ||
                Config.episodesToKeep < 0 ||
                Config.Interval < 0)
            {
                SettingsChanged = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Some of the settings are not valid!", "Wrong settings!");
            }
        }

        private void Login_Plex(object sender, RoutedEventArgs e)
        {
            if(plexAddress.Text == "")
            {
                MessageBox.Show("You have to give me an Address first!");
                return;
            }

            Config.PlexAddress = plexAddress.Text;

            LoginPlex loginscreen = new LoginPlex("", 0);
            loginscreen.ShowDialog();
            if(loginscreen.LoginSuccessful)
            {
                plexUsernameTB.Text = Config.plexUsername;
                plexUserIDTB.Text = Config.plexUuid;
                SettingsChanged = true;

                plexChangeUserBT.Visibility = Visibility.Visible;
                plexLoginBT.Visibility = Visibility.Hidden;
            }
        }

        private void Login_Emby(object sender, RoutedEventArgs e)
        {
            if (embyAddress.Text == "")
            {
                MessageBox.Show("You have to give me an Address first!");
                return;
            }

            Config.EmbyAddress = embyAddress.Text;

            LoginEmby login = new LoginEmby();
            login.ShowDialog();
            if (login.LoginSuccessful)
            {
                embyUsernameTB.Text = Config.embyUsername;
                embyUserIDTB.Text = Config.embyUserid;
                SettingsChanged = true;

                embyChangeUserBT.Visibility = Visibility.Visible;
                embyLoginBT.Visibility = Visibility.Hidden;
            }
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selecteditem = (sender as ComboBox).SelectedIndex;
            favoriteEpisodesCB.IsEnabled = true;
            favoriteEpisodesCB.IsChecked = Properties.Settings.Default.favoriteEpisodes;

            plexGrid.Visibility = Visibility.Hidden;
            embyGrid.Visibility = Visibility.Hidden;


            if (selecteditem == 0)
            {
                plexGrid.Visibility = Visibility.Visible;

                favoriteEpisodesCB.IsEnabled = false;
                favoriteEpisodesCB.IsChecked = false;
            }
            else if (selecteditem == 1)
            {
                intitiateEmby();
                embyGrid.Visibility = Visibility.Visible;
            }


        }
    }
}
