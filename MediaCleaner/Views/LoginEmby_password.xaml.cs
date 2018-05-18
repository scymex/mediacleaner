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
        TextBox usernameTB;
        PasswordBox passwordTB;
        TextBlock wrongpw;
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

            usernameTB = (TextBox)this.FindName("uname");
            passwordTB = (PasswordBox)this.FindName("pw");
            wrongpw = (TextBlock)this.FindName("wpw");

            usernameTB.Text = username;
            passwordTB.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var embyaccesstoken = embyApi.getAccessToken(usernameTB.Text, passwordTB.Password);

                Config.embyAccessToken = embyaccesstoken;
                LoginSuccessful = true;
                this.Close();
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    logger.Error("Trying to log in failed.");
                    wrongpw.Visibility = Visibility.Visible;
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
