using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    public class GeoResult
    {
        public Location location { get; set; }
        public string formatted_address { get; set; }
        public string business { get; set; }
        public AddressComponent addressComponent { get; set; }
        public int cityCode { get; set; }
    }
}
