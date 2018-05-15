using MediaCleaner.DataModels.Sonarr;
using RestSharp;
using RestSharp.Deserializers;
using System.Collections.Generic;


namespace MediaCleaner.APIClients
{
    class SonarrApi
    {
        // SONARR
        string URL_sonarr = Config.SonarrAddress + "/api";
        RestClient client;
        JsonDeserializer deserialCount = new JsonDeserializer();

        public SonarrApi()
        {
            client = new RestClient(URL_sonarr);
        }

        public bool checkConnection()
        {
            var request = new RestRequest("system/status", Method.GET);

            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            Log.Debug(string.Format("Sonarr response checkconnection: {0}",response.Content));

            if (response.ResponseStatus == ResponseStatus.Completed)
                return true;
            else
                throw response.ErrorException;
        }

        public bool checkSettings()
        {
            Log.Debug(string.Format("[Sonarr:] API key: \"{0}\"; Address: \"{1}\";", Config.sonarrAPIKey, Config.SonarrAddress));
            if (Config.sonarrAPIKey == "" || Config.SonarrAddress == "")
            {
                return false;
            }

            return true;
        }

        public bool CheckApikey()
        {
            var request = new RestRequest("system/status", Method.GET);

            request.AddHeader("X-Api-Key", Config.sonarrAPIKey);

            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            Log.Debug(string.Format("Sonarr response checkApikey: {0}", response.Content));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public List<Episode> getEpisodebySeries(string seriesId)
        {
            var request = new RestRequest("Episode", Method.GET);

            request.AddHeader("X-Api-Key", Config.sonarrAPIKey);
            request.AddParameter("SeriesId", seriesId);

            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return deserialCount.Deserialize<List<Episode>>(response);
            else
                throw response.ErrorException;
        }

        public List<Series> getSeriesList()
        {
            var request = new RestRequest("Series", Method.GET);
            request.AddHeader("X-Api-Key", Config.sonarrAPIKey);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return deserialCount.Deserialize<List<Series>>(response);
            else
                throw response.ErrorException;
        }

        public bool deleteEpisodeFile(int episodeId)
        {
            var request = new RestRequest(string.Format("EpisodeFile/{0}", episodeId), Method.DELETE);
            request.AddHeader("X-Api-Key", Config.sonarrAPIKey);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                throw response.ErrorException;
        }
    }
}
