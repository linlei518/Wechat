using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Entity
{
    public class Group_Msg_Statistics
    {
        public int wx_id;
        public int id;
        public int msg_count;
        public int msg_type;
        public int source_id;
    }

    public class All_Property_Statistics<T>
    {
        public int count;
        public T property;
        public int wx_id;
    }

}
