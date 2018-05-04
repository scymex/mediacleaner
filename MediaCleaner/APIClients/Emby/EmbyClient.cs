using RestSharp;
using RestSharp.Deserializers;
using System.Collections.Generic;

namespace MediaCleaner.Emby
{
    class EmbyClient
    {
        // EMBY
        string URL_emby = Config.EmbyAddress;
        RestClient client;

        JsonDeserializer deserialCount = new JsonDeserializer();

        public EmbyClient()
        {
            client = new RestClient(URL_emby);
        }

        public bool checkConnection()
        {
            var request = new RestRequest("System/Info", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("X-MediaBrowser-Token", Config.embyAccessToken);
            var response = client.Execute(request);

            Log.Debug(string.Format("Emby response checkconnection: {0}", response.Content));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                throw response.ErrorException;
        }

        public List<UserItem> getUserItems()
        {
            var request = new RestRequest(string.Format("Users/{0}/Items?recursive=true&IncludeItemTypes=Episode&Fields=MediaSources,DateCreated", Config.embyUserid), Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("X-MediaBrowser-Token", Config.embyAccessToken);
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return deserialCount.Deserialize<UserItems>(response).Items;
            else
                throw response.ErrorException;
        }

        public bool validateUser()
        {
            if (Config.embyUserid == "" || Config.embyAccessToken == "")
                return false;
            else
                return true;
        }
        /*public bool CheckSettings()
        {
            var validSettings = true;
            if (!CheckApikey() || Properties.Settings.Default.emby_userid == "" || Properties.Settings.Default.emby_accesstoken == "" || Int32.Parse(Properties.Settings.Default.episodesToKeep) < 0 || Int32.Parse(Properties.Settings.Default.interval) < 0)
                validSettings = false;

            return validSettings;
        }*/

        public string getAccessToken(string username_, string password_)
        {
            var request = new RestRequest("Users/AuthenticateByName", Method.POST);
            request.AddHeader("Authorization", "MediaBrowser Client=\"PowerShellScript\", Device=\"PowerShellScriptEpisodeFiller\", DeviceId=\"1\", Version=\"1.0.0\"");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { Username = username_, password = APIHelper.SHA1Hash(password_), passwordMd5 = APIHelper.MD5Hash(password_) });
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return deserialCount.Deserialize<AuthenticateByName>(response).AccessToken;
            else
                return "";
        }

        public List<PublicUser> getPublicUsers()
        {
            var request = new RestRequest("Users/Public", Method.GET);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return deserialCount.Deserialize<List<PublicUser>>(response);
            else
                throw response.ErrorException;
        }


    }
}
