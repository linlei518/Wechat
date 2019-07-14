using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    public class GeoList
    {
        public int status { get; set; }

        public int size { get; set; }

        public int total { get; set; }

        public List<GeoListItem> pois { get; set; }
    }
}
