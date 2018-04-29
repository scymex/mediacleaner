using MediaCleaner.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCleaner
{
    interface IMediaServer
    {
        Episode getItem(string EpisodePath);
        bool checkConnection();
    }
}
