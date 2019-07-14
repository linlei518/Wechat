using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Entity
{
    public partial class wmt_t_fans
    {
        public int id { get; set; }
        public int wx_id { get; set; }
        public string wx_og_id { get; set; }
        public string open_id { get; set; }
        public string nick_name { get; set; }
        public Nullable<int> sex { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string language { get; set; }
        public string headimgurl { get; set; }
        public Nullable<int> status { get; set; }
        public string subscribe_time { get; set; }
        public string remove_time { get; set; }
    }
}
