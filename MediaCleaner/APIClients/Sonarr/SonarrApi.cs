using RestSharp;
using RestSharp.Deserializers;
using System.Collections.Generic;


namespace MediaCleaner.Sonarr
{
    class SonarrApi
    {
        // SONARR
        string URL_sonarr = "http://192.168.1.214:8989/api";
        RestClient client;
        JsonDeserializer deserialCount = new JsonDeserializer();

        public SonarrApi()
        {
            client = new RestClient(URL_sonarr);
        }

        public bool CheckApikey()
        {
            var request = new RestRequest("system/status", Method.GET);

            request.AddHeader("X-Api-Key", Config.sonarrAPIKey);

            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

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
