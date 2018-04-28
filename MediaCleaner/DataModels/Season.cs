using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCleaner.DataModels
{
    class Season
    {
        int season { get; set; }
        List<SeriesEpisode>  episodeList { get; set; }
    }
}
