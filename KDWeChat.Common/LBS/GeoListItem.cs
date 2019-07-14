using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    public class GeoListItem
    {
        public string title { get; set; }
        public double[] location { get; set; }
        public string city { get; set; }
        public DateTime create_time { get; set; }
        public int geotable_id { get; set; }
        public string address { get; set; }
        public string tags { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string contents { get; set; }
        public int city_id { get; set; }
        public int id { get; set; }
    }
}
