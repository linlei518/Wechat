using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KDWechat.BLL.Module
{
    public class md_sale_fans
    {
        public static t_md_sale_fans GetFans()
        {
            var fan_id =RequestHelper.GetQueryInt("fan_id", 0);
            if ( fan_id== 0)
            {
                var opid =RequestHelper.GetQueryString("opid");
                if (string.IsNullOrEmpty(opid))
                {
                    var tempID = Utils.GetCookie("tempid");
                    if (string.IsNullOrEmpty(tempID))
                    {
                        tempID = RequestHelper.GetQueryString("tempid");
                        tempID= string.IsNullOrEmpty(tempID)?Guid.NewGuid().ToString().Replace("-",""):tempID;
                        var tempFans = new t_md_sale_fans()
                        {
                            fan_id = 0,
                            type = (int)SaleFansType.临时粉丝,
                            opid = tempID
                        };
                        Companycn.Core.EntityFramework.EFHelper.AddModel<kd_moduleEntities, t_md_sale_fans>(tempFans);
                        return tempFans;
                    }
                    else
                    {
                        return Companycn.Core.EntityFramework.EFHelper.GetModel<kd_moduleEntities, t_md_sale_fans>(x => x.opid == tempID && x.type == (int)SaleFansType.临时粉丝);
                    }
                }
                else
                {
                    return Companycn.Core.EntityFramework.EFHelper.GetModel<kd_moduleEntities, t_md_sale_fans>(x => x.opid == opid && x.type == (int)SaleFansType.微信粉丝);
                }
            }
            else
            {
                return Companycn.Core.EntityFramework.EFHelper.GetModel<kd_moduleEntities, t_md_sale_fans>(x => x.id == fan_id && x.type == (int)SaleFansType.微信粉丝);
            }
        }
    }
}
