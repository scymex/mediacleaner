﻿using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MediaCleaner.APIClients;

namespace MediaCleaner.Views
{
    /// <summary>
    /// Interaction logic for LoginEmby_password.xaml
    /// </summary>
    public partial class LoginPlex : Window
    {
        PlexClient plexApi;
        TextBox usernameTB;
        PasswordBox passwordTB;
        TextBlock wrongpw;
        public bool LoginSuccessful = false;
        public string username = "";
        int mediaserver;

        public LoginPlex(string username, int mediaserver_)
        {
            InitializeComponent();
            mediaserver = mediaserver_;
            plexApi = new PlexClient();

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
    }
}