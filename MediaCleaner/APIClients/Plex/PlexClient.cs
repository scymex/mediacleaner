using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using RestSharp;
using RestSharp.Deserializers;
using MediaCleaner.DataModels.Plex;

namespace MediaCleaner.APIClients
{
    class PlexClient
    {
        RestClient client;
        JsonDeserializer deserialCount = new JsonDeserializer();
        string URL_plex = Config.PlexAddress;

        public PlexClient()
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
            var request = addPlexClientInfoHeaders(new RestRequest("system", Method.GET));
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("X-Plex-Token", Config.plexAccessToken);
            var response = client.Execute(request);

            Log.Debug(string.Format("Plex response checkconnection: {0}", response.Content));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                throw response.ErrorException;
        }

        public List<Section> getSections()
        {
            var request = addPlexClientInfoHeaders(new RestRequest("library/sections", Method.GET));
            request.RequestFormat = DataFormat.Json;
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
            var request = addPlexClientInfoHeaders(new RestRequest(string.Format("library/sections/{0}/all", section_id), Method.GET));
            request.RequestFormat = DataFormat.Json;
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
            var request = addPlexClientInfoHeaders(new RestRequest(string.Format("library/metadata/{0}/children", key), Method.GET));
            request.RequestFormat = DataFormat.Json;
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
            var request = addPlexClientInfoHeaders(new RestRequest(string.Format("library/metadata/{0}/children", season_id), Method.GET));
            request.RequestFormat = DataFormat.Json;
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

            var request = addPlexClientInfoHeaders(new RestRequest("/users/sign_in.json", Method.POST));
            request.RequestFormat = DataFormat.Json;

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

        private RestRequest addPlexClientInfoHeaders(RestRequest request)
        {
            request.AddHeader("X-Plex-Device-Name", "Media Cleaner");
            request.AddHeader("X-Plex-Product", "MediaCleaner");
            request.AddHeader("X-Plex-Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            request.AddHeader("X-Plex-Client-Identifier", getClientToken());
            
            return request;
        }
    }
}
