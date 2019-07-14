using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.Common.Config
{ 
    /// <summary>
    /// 统计配置实体类
    /// </summary>
    [Serializable]
    public class statisticsconfig
    {
        public statisticsconfig()
        { }

        private List<model> _dbtables = new List<model>();

        /// <summary>
        /// 有效的数据库表名
        /// </summary>
        public List<model> dbtables
        {
            get { return _dbtables; }
            set { _dbtables = value; }
        }
    }


     [Serializable]
    public class model
    {
        private string _id;

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

         private string _name;

         public string name
         {
             get { return _name; }
             set { _name = value; }
         }
    }
}
