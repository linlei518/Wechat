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
    
    public partial class t_product_model
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<int> product_id { get; set; }
        public string rated_voltage { get; set; }
        public Nullable<double> speed { get; set; }
        public Nullable<System.DateTime> create_date { get; set; }
        public Nullable<int> is_publish { get; set; }
        public string link_url { get; set; }
        public Nullable<int> sort_id { get; set; }
    }
}