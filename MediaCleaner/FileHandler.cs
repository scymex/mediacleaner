using System.Linq;
using System.Collections.Generic;
using MediaCleaner.DataModels;
using System;
using MediaCleaner.APIClients;

namespace MediaCleaner
{
    class FileHandler
    {

        SonarrApi sonarrApi;
        MediaServer mServer;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public FileHandler(SonarrApi sonarrApi_, MediaServer mServer_)
        {
            sonarrApi = sonarrApi_;
            mServer = mServer_;
            mServer.initMediaServer(); // we need to reset the media server, there is a logic flaw in getting the items, this is the easiest and fastest way to fix it for now.
        }

        public List<Episode> getEpisodeList ()
        {
            var fileList = getFileList();
            var episodeList = new List<Episode>();

            foreach(string filePath in fileList)
            {
                var episode = mServer.getItem(filePath);

                if (episode == null)
                {
                    logger.Info("Can't find this file in the mediaserver: {0}", filePath);
                    continue;
                }

                episodeList.Add(episode);
            }

            return episodeList;
        }

        public List<Episode> getEpisodeListbyOrder(List<Episode> episodeList)
        {
            episodeList.OrderBy(episode => episode.SeriesName).ThenBy(episode => episode.SeasonNumber).ThenBy(episode => episode.EpisodeNumber);

            return episodeList;
        }

        public bool deleteFile(string filePath)
        {
            // ????????? 

            var seriesList = new List<DataModels.Sonarr.Series>();

            try
            {
                seriesList = sonarrApi.getSeriesList();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            foreach (var series in seriesList)
            {
                var EpisodeList = new List<DataModels.Sonarr.Episode>();

                try
                {
                    EpisodeList = sonarrApi.getEpisodebySeries(series.id.ToString());
                }
                catch(Exception ex)
                {
                    logger.Error(ex);
                }

                for (var i = EpisodeList.Count - 1; i >= 0; i--)
                {
                    var episode = EpisodeList[i];

                    if (episode.hasFile == true)
                    {
                        if (episode.episodeFile.path == filePath)
                            try
                            {
                                sonarrApi.deleteEpisodeFile(episode.episodeFileId);
                                return true;
                            } catch (Exception ex)
                            {
                                logger.Error(ex, "There was an error deleting \"{0}\" Exception: ", filePath);
                                return false;
                            }
                    }
                }
            }

            logger.Error("There was an error while deleting the file: Can't find this file: \"{0}\"", filePath);

            return false;
        }

        public List<string> getFileList()
        {
            // get file list by path
            // TODO
            //

            // get file list by sonarr
            var fileList = new List<string>();

            var seriesList = new List<DataModels.Sonarr.Series>();

            try
            {
                seriesList = sonarrApi.getSeriesList();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            foreach (var series in seriesList)
            {
                var EpisodeList = new List<DataModels.Sonarr.Episode>();

                try
                {
                    EpisodeList = sonarrApi.getEpisodebySeries(series.id.ToString());
                }
                catch(Exception ex)
                {
                    logger.Error(ex);
                }

                for (var i = EpisodeList.Count - 1; i >= 0; i--)
                {
                    var episode = EpisodeList[i];

                    if (episode.hasFile == true)
                    {
                        fileList.Add(episode.episodeFile.path);
                    }
                }
            }

            return fileList;
        }
    }
}
