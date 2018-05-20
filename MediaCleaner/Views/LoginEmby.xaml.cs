using MediaCleaner.APIClients;
using MediaCleaner.DataModels.Emby;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaCleaner.Views
{
    public partial class LoginEmby : Window
    {
        EmbyClient embyApi;
        public bool LoginSuccessful;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public LoginEmby()
        {
            InitializeComponent();
            embyApi = new EmbyClient();

            try
            {
                userList.ItemsSource = embyApi.getPublicUsers();
            }
            catch (WebException exc)
            {
                logger.Error(exc);
            }
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            var listbox_ = sender as ListBox;
            var selectedUser = listbox_.SelectedItem as PublicUser;

            Config.embyUsername = selectedUser.Name;
            Config.embyUserid = selectedUser.Id;

            if (selectedUser.HasPassword)
            {
                LoginEmby_password login = new LoginEmby_password(selectedUser.Name);
                login.ShowDialog();
                if (login.LoginSuccessful == true)
                {
                    LoginSuccessful = true;
                    Close();
                }
            }
            else
            {
                try
                {
                    var embyaccesstoken = embyApi.getAccessToken(selectedUser.Name);
                    Config.embyAccessToken = embyaccesstoken;
                    LoginSuccessful = true;
                    Close();
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }
    }
}
