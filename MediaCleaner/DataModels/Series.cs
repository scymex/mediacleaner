using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCleaner.DataModels
{
    public class Series
    {
        string SeriesTitle { get; set; }
        List<Season> seasons { get; set; }
    }
}
