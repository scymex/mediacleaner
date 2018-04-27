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
        TextBox output;
        TextBox apikeyTB;
        TextBox hoursToKeepTB;
        TextBox episodesToKeepTB;
        TextBox intervalTB;
        TextBlock usernameTB;
        TextBlock useridTB;
        CheckBox debugCB;
        CheckBox favoriteEpisodesCB;
        Button changeUserBT;
        Button loginBT;
        ComboBox mediaServerCOB;
        Label unameEmby;
        Label useridEmby;
        Label unamePlex;
        Label useridPlex;
        Label keepfavoriteNot;

        EmbyApi embyApi;
        SonarrApi sonarrApi;
        public bool SettingsChanged = false;

        public Settings()
        {

            InitializeComponent();

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MediaCleaner.Resource." + "icon_running.ico"))
            {
                this.Icon = BitmapFrame.Create(stream);
            }

            apikeyTB = (TextBox)this.FindName("apikey");
            hoursToKeepTB = (TextBox)this.FindName("hoursToKeep");
            episodesToKeepTB = (TextBox)this.FindName("episodesToKeep");
            intervalTB = (TextBox)this.FindName("interval");
            output = (TextBox)this.FindName("tx");
            usernameTB = (TextBlock)this.FindName("username");
            useridTB = (TextBlock)this.FindName("userid");
            debugCB = (CheckBox)this.FindName("debug");
            favoriteEpisodesCB = (CheckBox)this.FindName("favoriteEpisodes");
            changeUserBT = (Button)this.FindName("changeUser");
            loginBT = (Button)this.FindName("login");
            mediaServerCOB = (ComboBox)this.FindName("mediaserver");
            unameEmby = (Label)this.FindName("username_emby_label");
            useridEmby = (Label)this.FindName("userid_emby_label");
            unamePlex = (Label)this.FindName("username_plex_label");
            useridPlex = (Label)this.FindName("userid_plex_label");
            keepfavoriteNot = (Label)this.FindName("keepfavoritenot");

            embyApi = new EmbyApi();
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


            if (Config.MediaServer == 0)
            {
                unameEmby.Visibility = Visibility.Hidden;
                useridEmby.Visibility = Visibility.Hidden;
                unamePlex.Visibility = Visibility.Visible;
                useridPlex.Visibility = Visibility.Visible;
                keepfavoriteNot.Visibility = Visibility.Visible;
                favoriteEpisodesCB.IsEnabled = false;
                favoriteEpisodesCB.IsChecked = false;

                // Emby username
                if (Config.plexUsername == "")
                    usernameTB.Text = "-";
                else
                    usernameTB.Text = Config.plexUsername;
                // emby userid
                if (Config.plexUuid == "")
                    useridTB.Text = "-";
                else
                    useridTB.Text = Config.plexUuid;

                if (Config.plexUuid != "" || Config.plexUsername != "")
                {
                    changeUserBT.Visibility = Visibility.Visible;
                    loginBT.Visibility = Visibility.Hidden;
                }
            } else if(Config.MediaServer == 1)
            {
                // Emby username
                if (Config.embyUsername == "")
                    usernameTB.Text = "-";
                else
                    usernameTB.Text = Config.embyUsername;
                // emby userid
                if (Config.embyUserid == "")
                    useridTB.Text = "-";
                else
                    useridTB.Text = Config.embyUserid;

                if (Config.embyUsername != "" || Config.embyUserid != "")
                {
                    changeUserBT.Visibility = Visibility.Visible;
                    loginBT.Visibility = Visibility.Hidden;
                }
                unameEmby.Visibility = Visibility.Visible;
                useridEmby.Visibility = Visibility.Visible;
                unamePlex.Visibility = Visibility.Hidden;
                useridPlex.Visibility = Visibility.Hidden;
            }

            // Debug checkbox
            debugCB.IsChecked = Config.Debug;
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
            Config.sonarrAPIKey = apikeyTB.Text;
            Config.hoursToKeep = Int32.Parse(hoursToKeep.Text);
            Config.episodesToKeep = Int32.Parse(episodesToKeepTB.Text);
            Config.Interval = Int32.Parse(intervalTB.Text);
            Config.favoriteEpisodes = favoriteEpisodesCB.IsChecked.HasValue ? true : false;

            if (mediaServerCOB.SelectedIndex != Config.MediaServer && !SettingsChanged)
            {
                Config.embyUsername = "";
                Config.embyUserid = "";
                Config.embyAccessToken = "";
            }

            Config.MediaServer = mediaServerCOB.SelectedIndex;

            if (sonarrApi.CheckApikey() || 
                Config.embyUserid == "" || 
                Config.embyAccessToken == "" || 
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

        private void Login(object sender, RoutedEventArgs e)
        {
            if (mediaServerCOB.SelectedIndex == 0)
            {
                LoginScreen loginscreen = new LoginScreen("", 0);
                loginscreen.ShowDialog();
                if(loginscreen.LoginSuccessful)
                {
                    usernameTB.Text = Config.plexUsername;
                    useridTB.Text = Config.plexUuid;
                    SettingsChanged = true;

                    changeUserBT.Visibility = Visibility.Visible;
                    loginBT.Visibility = Visibility.Hidden;
                }


            }
            else if(mediaServerCOB.SelectedIndex == 1)
            {
                Login login = new Login();
                login.ShowDialog();
                if (login.LoginSuccessful)
                {
                    usernameTB.Text = Config.embyUsername;
                    useridTB.Text = Config.embyUserid;
                    SettingsChanged = true;

                    changeUserBT.Visibility = Visibility.Visible;
                    loginBT.Visibility = Visibility.Hidden;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selecteditem = (sender as ComboBox).SelectedIndex;
            keepfavoriteNot.Visibility = Visibility.Hidden;
            favoriteEpisodesCB.IsEnabled = true;
            favoriteEpisodesCB.IsChecked = Properties.Settings.Default.favoriteEpisodes;

            if (selecteditem == 0)
            {
                unameEmby.Visibility = Visibility.Hidden;
                useridEmby.Visibility = Visibility.Hidden;
                unamePlex.Visibility = Visibility.Visible;
                useridPlex.Visibility = Visibility.Visible;

                keepfavoriteNot.Visibility = Visibility.Visible;
                favoriteEpisodesCB.IsEnabled = false;
                favoriteEpisodesCB.IsChecked = false;
            }
            else if (selecteditem == 1)
            {
                unameEmby.Visibility = Visibility.Visible;
                useridEmby.Visibility = Visibility.Visible;
                unamePlex.Visibility = Visibility.Hidden;
                useridPlex.Visibility = Visibility.Hidden;
            }

            if (Config.MediaServer != selecteditem)
            {
                changeUserBT.Visibility = Visibility.Hidden;
                loginBT.Visibility = Visibility.Visible;
                usernameTB.Text = "-";
                useridTB.Text = "-";
            }

            else
            {
                if (selecteditem == 0)
                {
                    if (Config.plexUsername != "" || Config.plexUuid != "")
                    {
                        changeUserBT.Visibility = Visibility.Visible;
                        loginBT.Visibility = Visibility.Hidden;
                        usernameTB.Text = Config.plexUsername;
                        useridTB.Text = Config.plexUuid;
                    }
                    else
                    {
                        changeUserBT.Visibility = Visibility.Hidden;
                        loginBT.Visibility = Visibility.Visible;
                        usernameTB.Text = "-";
                        useridTB.Text = "-";
                    }
                }
                if (selecteditem == 1)
                {
                    if (Config.embyUsername != "" || Config.embyUserid != "")
                    {
                        changeUserBT.Visibility = Visibility.Visible;
                        loginBT.Visibility = Visibility.Hidden;
                        usernameTB.Text = Config.embyUsername;
                        useridTB.Text = Config.embyUserid;
                    }
                    else
                    {
                        changeUserBT.Visibility = Visibility.Hidden;
                        loginBT.Visibility = Visibility.Visible;
                        usernameTB.Text = "-";
                        useridTB.Text = "-";
                    }
                }

            }
        }
    }
}
