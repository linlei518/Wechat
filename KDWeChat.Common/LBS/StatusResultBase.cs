using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    public class StatusResultBase
    {
        public int status { get; set; }
        public string message { get; set; }
        public int? id { get; set; }
    }
}
