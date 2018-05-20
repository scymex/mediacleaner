using System;
using System.Reflection;
using System.Web.Http;
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
        public bool LoginSuccessful = false;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public LoginPlex(string username, int mediaserver_)
        {
            InitializeComponent();
            plexApi = new PlexClient();

            uname.Focus();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            try
            {
                var plexaccesstoken = plexApi.getAccessToken(uname.Text, pw.Password);
                Config.plexAccessToken = plexaccesstoken;
                LoginSuccessful = true;
                Close();
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    logger.Error("Trying to log in failed.");
                    wpw.Visibility = Visibility.Visible;
                }
                else
                {
                    logger.Error(ex);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
