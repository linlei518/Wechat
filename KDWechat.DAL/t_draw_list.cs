//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KDWechat.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_draw_list
    {
        public int id { get; set; }
        public string open_id { get; set; }
        public string user_name { get; set; }
        public string phone { get; set; }
        public Nullable<System.DateTime> create_date { get; set; }
        public Nullable<int> prize { get; set; }
        public string prize_name { get; set; }
        public string head_img_url { get; set; }
        public string sex { get; set; }
        public string nick_name { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> prize_number { get; set; }
    }
}
