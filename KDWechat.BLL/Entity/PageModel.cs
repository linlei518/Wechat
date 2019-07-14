using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Entity
{
    public class user_list_model
    {
        public int id { get; set; }
        public string unionid { get; set; }
        public string open_id { get; set; }
        public int group_id { get; set; }
        public string nick_name { get; set; }
        public string headimgurl { get; set; }
        public Nullable<System.DateTime> last_interact_time { get; set; }
        public int reply_state { get; set; }
        public int wx_id { get; set; }
    }
}
