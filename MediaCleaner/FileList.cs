using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using ContextMenuStrip = System.Windows.Forms.ContextMenuStrip;
using ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem;
using System.Collections.Generic;
using MediaCleaner.Emby;
using System.Threading.Tasks;
using MediaCleaner.Sonarr;
using MediaCleaner.DataModels;

namespace MediaCleaner
{
    class FileList
    {

        SonarrApi sonarrApi;
        MediaServer mServer;

        public FileList()
        {
            sonarrApi = new SonarrApi();

            switch(Config.MediaServer)
            {
                case 0:
                    mServer = new Plex.Plex();
                    break;
                case 1:
                    mServer = new Emby.Emby();
                    break;
            }
        }

        public List<Item> getEpisodeList ()
        {
            var fileList = getFileList();
            var episodeList = new List<Item>();

            foreach(string filePath in fileList)
            {
                var episode = mServer.getItem(filePath);
                if (episode == null)
                {
                    Log.Info(string.Format("Can't find this file in the mediaserver: {0}", filePath));
                    continue;
                }

                episodeList.Add(episode);
            }

            return episodeList;
        }

        public List<Item> getEpisodeListbyOrder(List<Item> episodeList)
        {
            episodeList.OrderBy(episode => episode.SeriesName).ThenBy(episode => episode.Season).ThenBy(episode => episode.Episode);

            return episodeList;
        }

        public List<string> getFileList()
        {
            // get file list by path
            // TODO
            //

            // get file list by sonarr
            var fileList = new List<string>();

            var seriesList = new List<Sonarr.Series>();
            seriesList = sonarrApi.getSeriesList();

            foreach (var series in seriesList)
            {
                var EpisodeList = new List<Episode>();
                EpisodeList = sonarrApi.getEpisodebySeries(series.id.ToString());

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
