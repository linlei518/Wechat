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
    
    public partial class t_wx_qrcode
    {
        public int id { get; set; }
        public Nullable<int> project_id { get; set; }
        public int souce_id { get; set; }
        public string q_name { get; set; }
        public System.DateTime create_time { get; set; }
        public int wx_id { get; set; }
        public string wx_og_id { get; set; }
        public string ticket { get; set; }
        public Nullable<int> q_type { get; set; }
    }
}
