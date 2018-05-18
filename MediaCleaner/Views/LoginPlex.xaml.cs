using System.Reflection;
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
        public string username = "";
        int mediaserver;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public LoginPlex(string username, int mediaserver_)
        {
            InitializeComponent();
            mediaserver = mediaserver_;
            plexApi = new PlexClient();

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MediaCleaner.Resource." + "icon_running.ico"))
            {
                this.Icon = BitmapFrame.Create(stream);
            }

            uname.Text = username;
            uname.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var plexaccesstoken = plexApi.getAccessToken(uname.Text, pw.Password);

            if (plexaccesstoken == "")
            {
                logger.Error("Trying to log in failed.");
                wpw.Visibility = Visibility.Visible;
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
