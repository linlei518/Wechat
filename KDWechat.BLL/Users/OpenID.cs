using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.BLL.Users
{
    public class openid_result
    {
        public int total { get; set; }
        public int count { get; set; }
        public openid_data data { get; set; }
        public string next_openid { get; set; }
    }

    public class openid_data
    {
        public string[] openid { get; set; }
    }
}
