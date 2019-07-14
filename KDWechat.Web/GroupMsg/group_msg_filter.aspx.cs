using KDWechat.BLL.Users;
using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.GroupMsg
{
    public partial class group_msg_filter : Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                displayBind();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["s"]) && Request.HttpMethod == "POST")//判断是否执行异步EXCEL的上传及统计
            {
                CheckAjaxData();
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["ut"]) && Request.HttpMethod == "POST")//判断是否执行异步计算筛选人数
            {
                AjaxGetNo();
            }

        }

        private void displayBind()
        {
            var groupList = wx_group_tags.GetListByChannelId((int)channel_idType.关注用户分组, wx_id);
            ddlGroup.DataSource = groupList;
            ddlGroup.DataTextField = "title";
            ddlGroup.DataValueField = "id";
            ddlGroup.DataBind();

            var tagList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_group_tags, int>(x => x.channel_id==(int)channel_idType.关注用户标签 && (x.wx_id==0||x.wx_id==wx_id), x => x.id, int.MaxValue, 1);//wx_group_tags.GetListByChannelId((int)channel_idType.关注用户标签, wx_id);
            ddlTag.DataSource = tagList;
            ddlTag.DataTextField = ddlGroup.DataTextField;
            ddlTag.DataValueField = ddlGroup.DataValueField;
            ddlTag.DataBind();

        }


        private void AjaxGetNo()
        {
            var list =GetGroupID();
            Response.Write("此次筛选共得出：" + list.Count() + " 名用户。|"+Utils.GetArrayStr(list,","));//筛选人数，返回的数据会被直接alert出现
            Response.End();
        }

        private void CheckAjaxData()
        {

            var file = Request.Files[0];//获取文件
            string toFileFullPath = Server.MapPath("~/Upload/excel/");//文件地址
            if (!Directory.Exists(toFileFullPath))//动态添加文件夹
            {
                Directory.CreateDirectory(toFileFullPath);
            }
            try
            {
                //尝试获取文件名并存储
                string fileN = toFileFullPath + RequestHelper.GetQueryString("s");
                file.SaveAs(fileN);
                //excel读取openid列表
                var lit = GemBoxExcelLiteHelper.InputFromExcel(fileN, "sheet1");
                string[] openIDS = new string[lit.Rows.Count];
                for (int i = 0; i < lit.Rows.Count; i++)
                {
                    openIDS[i] = lit.Rows[i]["openid"].ToString().Trim();
                }
                //ID列表去重
                var ids = openIDS.Distinct().ToArray();
                string opids = Utils.GetArrayStr(ids,",");
                Response.Write("1|" + openIDS.Count().ToString() + "|" + ids.Count().ToString()+"|"+opids);
                //返回结果，以|分割
                //第一位是状态，1：成功，0：失败
                //第二位是excel中的openid数（未去重）
                //第三位是去重后的OPENID数量
            }
            catch
            {
                Response.Write("0|0|0");
            }
            finally
            {
                Response.End();
            }
        }

        private string[] GetGroupID()
        {
            string[] openIDS = null;
            if (ddlSendGroup.SelectedIndex == 0)//判断DDL的选中状态，0为筛选，1为excel上传
            {
                int group = -1;
                int tag = -1;
                string province = Request.Form["Province"];
                string city = Request.Form["City"];
                string area = Request.Form["Area"];
                int sex = -1;
                Expression<Func<t_wx_fans, bool>> where = x => x.wx_id == wx_id && x.status == (int)Status.正常; //初始化最初的筛选条件
                //以下根据筛选条件进行where拼接
                if (ddlGroup.SelectedItem.Value != "-1"&&ddlGroup.SelectedItem.Value!="-2")
                {
                    group = Utils.StrToInt(ddlGroup.SelectedValue, -1);
                    where = where.And(x => x.group_id == group);
                }
                if (ddlTag.SelectedItem.Value != "-2" && ddlTag.SelectedItem.Value != "-1")
                {
                    tag = Utils.StrToInt(ddlTag.SelectedValue, -1);
                    int[] fansIdArray =null;
                    if (ddlTag.SelectedItem.Value == "0")
                        fansIdArray = wx_fans_tags.GetArray(wx_id);
                    else
                        fansIdArray = wx_fans_tags.GetFansIDListByGroupID(wx_id, tag);
                    where = where.And(x => fansIdArray.Contains(x.id));

                }
                if (!string.IsNullOrEmpty(province) && province != "选择省份")
                {
                    province = province.Replace("省", "").Replace("市", "");
                    where = where.And(x => x.province == province);
                }
                if (!string.IsNullOrEmpty(city) && city != "全部")
                {
                    if (province.Contains("上海") || province.Contains("北京") || province.Contains("天津") || province.Contains("重庆"))
                    {
                        city = area.Replace("区", "");
                    }
                    else
                        city = city.Replace("市", "");
                    where = where.And(x => x.city == city);
                }
                //if (!string.IsNullOrEmpty(area) && area != "全部")
                //{
                //    area = area.Replace("区", "").Replace("县", "");
                //    where = where.And(x => x.area == area);
                //}
                if (radSexFemale.Checked)
                {
                    sex = (int)WeChatSex.女;
                    where = where.And(x => x.sex == (int)sex);
                }
                else if (radSexMale.Checked)
                {
                    sex = (int)WeChatSex.男;
                    where = where.And(x => x.sex == (int)sex);
                }
                else if (radSexUnknow.Checked)
                {
                    sex = (int)WeChatSex.未知;
                    where = where.And(x => x.sex == (int)sex);
                }
                openIDS = wx_fans.GetOpenIDs(where);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                    {
                        string filePath = Server.MapPath("~/upload/excel/");
                        var lit = GemBoxExcelLiteHelper.InputFromExcel(filePath + Request.Form["excelfileName"], "sheet1");
                        openIDS = new string[lit.Rows.Count];
                        for (int i = 0; i < lit.Rows.Count; i++)
                        {
                            openIDS[i] = lit.Rows[i][0].ToString();
                        }
                    }
                    else
                    {
                        string toFileFullPath = Server.MapPath("~/Upload/excel/");//文件地址
                        if (!Directory.Exists(toFileFullPath))//动态添加文件夹
                        {
                            Directory.CreateDirectory(toFileFullPath);
                        }
                        //尝试获取文件名并存储
                        string fileN = toFileFullPath + RequestHelper.GetFormString("excelfileName");
                        var file = Request.Files[0];
                        file.SaveAs(fileN);
                        //filExcelList.SaveAs(fileN);
                        //excel读取openid列表
                        var lit = GemBoxExcelLiteHelper.InputFromExcel(fileN, "sheet1");
                        openIDS = new string[lit.Rows.Count];
                        for (int i = 0; i < lit.Rows.Count; i++)
                        {
                            openIDS[i] = lit.Rows[i]["openid"].ToString().Trim();
                        }
                        //ID列表去重
                        openIDS = openIDS.Distinct().ToArray();
                        //string opids = Utils.GetArrayStr(ids, ",");
                    }
                }
                catch
                {
                    return new string[0];
                }
            }
            return openIDS;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var array =GetGroupID().Distinct().ToArray();
            Response.Write("<script>parent.GetList('"+Utils.GetArrayStr(array,",")+"','"+array.Length+"');</script>");
        }



    }
}