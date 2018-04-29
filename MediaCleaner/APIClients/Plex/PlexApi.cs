using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Deserializers;

namespace MediaCleaner.Plex
{
    class PlexApi
    {
        RestClient client;
        JsonDeserializer deserialCount = new JsonDeserializer();
        string URL_plex = Config.PlexAddress;

        public PlexApi()
        {
            client = new RestClient(URL_plex);
        }

        public string getClientToken()
        {
            if (Config.plexClientToken == "")
                Config.plexClientToken = System.Guid.NewGuid().ToString();

            return Config.plexClientToken;
        }

        public bool checkConnection()
        {
            var request = new RestRequest("system", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("X-Plex-Device-Name", "Sonarr Cleaner");
            request.AddHeader("X-Plex-Product", "Sonarr Cleaner 1.0.0");
            request.AddHeader("X-Plex-Version", "1.0.0");
            request.AddHeader("X-Plex-Client-Identifier", getClientToken());
            request.AddHeader("X-Plex-Token", Config.plexAccessToken);
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                throw response.ErrorException;
        }

        public List<Section> getSections()
        {
            var request = new RestRequest("library/sections", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("X-Plex-Device-Name", "Sonarr Cleaner");
            request.AddHeader("X-Plex-Product", "Sonarr Cleaner 1.0.0");
            request.AddHeader("X-Plex-Version", "1.0.0");
            request.AddHeader("X-Plex-Client-Identifier", getClientToken());
            request.AddHeader("X-Plex-Token", Config.plexAccessToken);
            request.AddHeader("Accept", "application/json");

            var response = client.Execute(request);
            List<Section> sectionList = deserialCount.Deserialize<SectionContainer>(response).MediaContainer.Directory;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return sectionList;
            else
                throw response.ErrorException;
        }

        public List<Show> getSeries(string section_id)
        {
            var request = new RestRequest(string.Format("library/sections/{0}/all", section_id), Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("X-Plex-Device-Name", "Sonarr Cleaner");
            request.AddHeader("X-Plex-Product", "Sonarr Cleaner 1.0.0");
            request.AddHeader("X-Plex-Version", "1.0.0");
            request.AddHeader("X-Plex-Client-Identifier", getClientToken());
            request.AddHeader("X-Plex-Token", Config.plexAccessToken);
            request.AddHeader("Accept", "application/json");

            var response = client.Execute(request);
            var shows = deserialCount.Deserialize<Shows>(response);
            var series = shows.MediaContainer.Metadata;


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return series;
            else
                throw response.ErrorException;
        }

        public List<Season> getSeasons (string key)
        {
            var request = new RestRequest(string.Format("library/metadata/{0}/children", key), Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("X-Plex-Device-Name", "Sonarr Cleaner");
            request.AddHeader("X-Plex-Product", "Sonarr Cleaner 1.0.0");
            request.AddHeader("X-Plex-Version", "1.0.0");
            request.AddHeader("X-Plex-Client-Identifier", getClientToken());
            request.AddHeader("X-Plex-Token", Config.plexAccessToken);
            request.AddHeader("Accept", "application/json");

            var response = client.Execute(request);
            List<Season> series = deserialCount.Deserialize<Seasons>(response).MediaContainer.Metadata;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return series;
            else
                throw response.ErrorException;
        }

        public List<Episode> getEpisodesbySeason(string season_id)
        {
            var request = new RestRequest(string.Format("library/metadata/{0}/children", season_id), Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("X-Plex-Device-Name", "Sonarr Cleaner");
            request.AddHeader("X-Plex-Product", "Sonarr Cleaner 1.0.0");
            request.AddHeader("X-Plex-Version", "1.0.0");
            request.AddHeader("X-Plex-Client-Identifier", getClientToken());
            request.AddHeader("X-Plex-Token", Config.plexAccessToken);
            request.AddHeader("Accept", "application/json");

            var response = client.Execute(request);

            List<Episode> episodeList = deserialCount.Deserialize<Episodes>(response).MediaContainer.Metadata;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return episodeList;
            else
                throw response.ErrorException;
        }

        public List<Episode> getUserItems()
        {
            var sections = getSections();
            List<Episode> useritems = new List<Episode>();
            foreach(var section in sections)
            {
                if(section.type == "show")
                {
                    var series = getSeries(section.key);

                    foreach(var show in series)
                    {
                        var seasons = getSeasons(show.ratingKey);
                        foreach(var season in seasons)
                        {
                            var episodeList = getEpisodesbySeason(season.ratingKey);

                            foreach(var episode in episodeList)
                            {
                                useritems.Add(episode);
                            }
                        }
                    }
                }
            }

            return useritems;
        }

        public string getAccessToken(string username, string password)
        {
            string URL_plextv = "https://plex.tv";
            RestClient client_plextv = new RestClient(URL_plextv);

            var base64string = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));

            var request = new RestRequest("/users/sign_in.json", Method.POST);
            request.RequestFormat = DataFormat.Json;
            //request.AddHeader("Authorization", string.Format("Basic {0}", base64string));
            /*request.AddHeader("X-Plex-Device-Name", "Sonarr Cleaner");
            request.AddHeader("X-Plex-Username", "asd");
            request.AddHeader("X-Plex-Product", "Sonarr Cleaner 1.0.0");
            request.AddHeader("X-Plex-Version", "1.0.0");
            request.AddHeader("X-Plex-Platform", "1.0.0");
            request.AddHeader("X-Plex-Platform-Version", "1.0.0");
            request.AddHeader("X-Plex-Device", "Yo");
            request.AddHeader("X-Plex-Client-Identifier", System.Guid.NewGuid().ToString());
            request.AddHeader("X-Plex-Provides", "controller");*/

            request.AddHeader("X-Plex-Device-Name", "Sonarr Cleaner");
            request.AddHeader("X-Plex-Product", "Sonarr Cleaner 1.0.0");
            request.AddHeader("X-Plex-Version", "1.0.0");
            request.AddHeader("X-Plex-Client-Identifier", getClientToken());

            request.AddJsonBody( new { user = new { login = username, password = password} });


            var response = client_plextv.Execute(request);

            var user = deserialCount.Deserialize<Users>(response).user;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {

                Config.plexUsername = user.username;
                Config.plexUuid = user.uuid;

                return user.authentication_token;
            }
            else
                return "";

        }
    }
}
