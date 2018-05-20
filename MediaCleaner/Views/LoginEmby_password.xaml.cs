using MediaCleaner.APIClients;
using System;
using System.Reflection;
using System.Web.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaCleaner.Views
{
    /// <summary>
    /// Interaction logic for LoginEmby_password.xaml
    /// </summary>
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

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MediaCleaner.Resource." + "icon_running.ico"))
            {
                this.Icon = BitmapFrame.Create(stream);
            }

            uname.Text = username;
            pw.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var embyaccesstoken = embyApi.getAccessToken(uname.Text, pw.Password);
                Config.embyAccessToken = embyaccesstoken;
                LoginSuccessful = true;
                this.Close();
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
