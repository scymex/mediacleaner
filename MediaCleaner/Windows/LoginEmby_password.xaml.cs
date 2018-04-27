using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MediaCleaner.Emby;
using MediaCleaner.Plex;

namespace MediaCleaner
{
    /// <summary>
    /// Interaction logic for LoginEmby_password.xaml
    /// </summary>
    public partial class LoginEmby_password : Window
    {
        EmbyApi embyApi;
        TextBox usernameTB;
        PasswordBox passwordTB;
        TextBlock wrongpw;
        public bool LoginSuccessful = false;
        public string username = "";

        public LoginEmby_password(string username)
        {
            InitializeComponent();
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
