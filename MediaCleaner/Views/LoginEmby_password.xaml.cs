using MediaCleaner.APIClients;
using System;
using System.Reflection;
using System.Web.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaCleaner.Views
{
    public partial class LoginEmby_password : Window
    {
        EmbyClient embyApi;
        public bool LoginSuccessful = false;
        public string username = "";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public LoginEmby_password(string username)
        {
            InitializeComponent();
            embyApi = new EmbyClient();

            uname.Text = username;
            pw.Focus();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            try
            {
                var embyaccesstoken = embyApi.getAccessToken(uname.Text, pw.Password);
                Config.embyAccessToken = embyaccesstoken;
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

                logger.Error(ex.Message);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            
        }
    }
}
