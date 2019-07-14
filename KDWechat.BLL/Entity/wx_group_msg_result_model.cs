using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Entity
{
    public class wx_group_msg_result_model
    {

        public int errcode { get; set; }//0,
        public string errmsg { get; set; }//"send job submission success",
        public long msg_id { get; set; }//34182, 
        public long msg_data_id { get; set; }// 206227730

    }
}
