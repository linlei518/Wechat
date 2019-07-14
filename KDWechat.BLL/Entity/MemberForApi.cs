using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Entity
{
    public class MemberForApi
    {
        public int id { get; set; }
        public string phone { get; set; }
        public string pwd { get; set; }
        public string salt { get; set; }
        public string nick_name { get; set; }
        public string e_mail { get; set; }
        public string username { get; set; }
        public Nullable<int> capital_star_id { get; set; }
        public System.DateTime add_time { get; set; }
        public System.DateTime login_time { get; set; }
        public string login_ip { get; set; }
        public Nullable<int> sex { get; set; }
        public Nullable<int> age { get; set; }
        public Nullable<int> income { get; set; }
        public string nation { get; set; }
        public Nullable<System.DateTime> birth { get; set; }
        public string ID_No { get; set; }
        public Nullable<int> ID_Type { get; set; }
    }
}
