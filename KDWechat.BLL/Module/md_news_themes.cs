using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;
using Companycn.Core.DbHelper;
using System.Data.SqlClient;
using System.Data;

namespace KDWechat.BLL.Module
{
    public  class md_news_themes
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_news_themes GetModel(int wx_id,string wx_og_id)
        {
            t_md_news_themes model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_news_themes.Where(x => x.wx_id == wx_id && x.wx_og_id == wx_og_id).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_news_themes Add(t_md_news_themes model)
        {

            model = EFHelper.AddModule<t_md_news_themes>(model);
            return model;//返回添加后的信息
        }
        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_news_themes Update(t_md_news_themes model)
        {
            return EFHelper.UpdateModule<t_md_news_themes>(model);
        }
    }
}
