﻿using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MediaCleaner.Emby;
using MediaCleaner.Plex;

namespace MediaCleaner
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        EmbyApi embyApi;
        PlexApi plexApi;
        TextBox usernameTB;
        PasswordBox passwordTB;
        TextBlock wrongpw;
        public bool LoginSuccessful = false;
        public string username = "";
        int mediaserver;

        public LoginScreen(string username, int mediaserver_)
        {
            InitializeComponent();
            mediaserver = mediaserver_;
            if(mediaserver == 0)
                plexApi = new PlexApi();
            else if(mediaserver == 1)
                embyApi = new EmbyApi();

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MediaCleaner.Resource." + "icon_running.ico"))
            {
                this.Icon = BitmapFrame.Create(stream);
            }

            usernameTB = (TextBox)this.FindName("uname");
            passwordTB = (PasswordBox)this.FindName("pw");
            wrongpw = (TextBlock)this.FindName("wpw");

            usernameTB.Text = username;
            passwordTB.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (mediaserver == 0)
            {
                var plexaccesstoken = plexApi.getAccessToken(usernameTB.Text, passwordTB.Password);

                if (plexaccesstoken == "")
                {
                    Log.Error("Trying to log in failed.");
                    wrongpw.Visibility = Visibility.Visible;
                }
                else
                {
                    Config.plexAccessToken = plexaccesstoken;
                    LoginSuccessful = true;
                    this.Close();
                }
            }
            else if (mediaserver == 1)
            {
                var embyaccesstoken = embyApi.getAccessToken(usernameTB.Text, passwordTB.Password);

                if (embyaccesstoken == "")
                {
                    Log.Error("Trying to log in failed.");
                    wrongpw.Visibility = Visibility.Visible;
                }
                else
                {
                    Config.embyAccessToken = embyaccesstoken;
                    LoginSuccessful = true;
                    this.Close();
                }
            }
        }
    }
}