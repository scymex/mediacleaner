using MediaCleaner.DataModels.Sonarr;
using RestSharp;
using RestSharp.Deserializers;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace MediaCleaner.APIClients
{
    class SonarrApi
    {
        // SONARR
        string URL_sonarr = Config.SonarrAddress + "/api";
        RestClient client;
        JsonDeserializer deserialCount = new JsonDeserializer();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public SonarrApi()
        {
            client = new RestClient(URL_sonarr);
        }

        public bool checkConnection()
        {
            var request = new RestRequest("system/status", Method.GET);

            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            if (response.ResponseStatus == ResponseStatus.Completed)
                return true;
            else
            {
                throw response.ErrorException;
            }
        }

        public bool checkSettings()
        {
            logger.Debug("[Sonarr:] API key: \"{0}\"; Address: \"{1}\";", Config.sonarrAPIKey, Config.SonarrAddress);
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

            logger.Debug("Sonarr response checkApikey: {0}", response.Content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                logger.Trace(response.Content);
                return true;
            }
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                logger.Trace(response.Content);
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            else
            {
                throw response.ErrorException;
            }
        }

        public List<Episode> getEpisodebySeries(string seriesId)
        {
            var request = new RestRequest("Episode", Method.GET);

            request.AddHeader("X-Api-Key", Config.sonarrAPIKey);
            request.AddParameter("SeriesId", seriesId);

            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                logger.Trace(response.Content);
                return deserialCount.Deserialize<List<Episode>>(response);
            }
            else
            {
                throw response.ErrorException;
            }
        }

        public List<Series> getSeriesList()
        {
            var request = new RestRequest("Series", Method.GET);
            request.AddHeader("X-Api-Key", Config.sonarrAPIKey);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                logger.Trace(response.Content);
                return deserialCount.Deserialize<List<Series>>(response);
            }
            else
            {
                throw response.ErrorException;
            }
        }

        public bool deleteEpisodeFile(int episodeId)
        {
            var request = new RestRequest(string.Format("EpisodeFile/{0}", episodeId), Method.DELETE);
            request.AddHeader("X-Api-Key", Config.sonarrAPIKey);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                logger.Trace(response.Content);
                return true;
            }
            else
            {
                throw response.ErrorException;
            }
        }
    }
}
