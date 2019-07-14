using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Users
{
    public class Fans_Filter
    {
        public int id { get; set; }
        public string sh { get; set; }
        public string name { get; set; }
        public string values { get; set; }
    }

    public class Fans_Filter_Collection
    {
        public string category{get;set;}
        public string properties { get; set; }
    }
}
