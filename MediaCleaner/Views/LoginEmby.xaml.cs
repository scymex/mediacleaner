using MediaCleaner.APIClients;
using MediaCleaner.DataModels.Emby;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaCleaner.Views
{
    /// <summary>
    /// Interaction logic for LoginEmby.xaml

    public class SampleData
    {
        public string username
        {
            get;
            set;
        }

        public string userid
        {
            get;
            set;
        }

        public bool HasPassword
        {
            get;
            set;
        }
    }

    public partial class LoginEmby : Window
    {
        EmbyClient embyApi;
        public bool LoginSuccessful;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public LoginEmby()
        {
            InitializeComponent();
            embyApi = new EmbyClient();

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MediaCleaner.Resource." + "icon_running.ico"))
            {
                this.Icon = BitmapFrame.Create(stream);
            }


            List<SampleData> data = new List<SampleData>();

            var PublicUsers = new List<PublicUser>();
            try
            {
                PublicUsers = embyApi.getPublicUsers();
            }
            catch (System.Net.WebException exc)
            {
                logger.Error(string.Format("[Emby] {0}",exc.Status.ToString()));
            }

            foreach (var user in PublicUsers)
            {
                data.Add(new SampleData() { username = user.Name, userid = user.Id, HasPassword = user.HasPassword});
            }


            this.list.ItemsSource = data;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var item = sender as ListBox;
            var item1 = item.SelectedItem as SampleData;

            var username = item1.username;
            var userid = item1.userid;

            Config.embyUsername = username;
            Config.embyUserid = userid;

            if (item1.HasPassword)
            {
                LoginEmby_password login = new LoginEmby_password(username);
                login.ShowDialog();
                if (login.LoginSuccessful == true)
                {
                    LoginSuccessful = true;
                    this.Close();
                }

            }
            else
            {
                try
                {
                    var embyaccesstoken = embyApi.getAccessToken(username, "");
                    Config.embyAccessToken = embyaccesstoken;
                    LoginSuccessful = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }
    }
}
