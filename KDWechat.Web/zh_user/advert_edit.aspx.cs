using System;
using System.Collections.Generic;
using System.Linq;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.zh_user
{
    public partial class advert_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteReturnPage(hfReturnUrl, "advert_list.aspx");

                showInfo();
                if (id > 0)
                {
                    //CheckUserAuthority("advert_list", RoleActionType.Edit, "advert_list");
                    bindData();
                }
                else
                {
                
                }
                
            }
        }


        private void showInfo()
        {
            //var list = DapperConnection.minebea.GetList<t_product_type>(null,new {is_lock=0},new {sort_id=true});
            //ddlType.DataTextField = "type_name";
            //ddlType.DataValueField = "type_id";
            //ddlType.DataSource = list;
            //ddlType.DataBind();
        }

        private void bindData()
        {
            var model = DapperConnection.minebea.GetModel<t_zh_advert>(new { id = id });
            if (model != null)
            {
                txt_title.Text = model.title;
                txt_link_url.Text = model.link_url;
                txtFile.Value = model.img_url;
                img_show.Src = model.img_url;
                txt_sort.Text = model.sort_id.ToString();

                if (model.status == (int)Status.正常)
                    radStatusOk.Checked = true;
                else
                    radStatusFalse.Checked = true;

            }
            else
            {
                JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                return;
            }
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            int status = radStatusOk.Checked ? (int)Status.正常 : (int)Status.禁用;
            t_zh_advert model = null;
            if (id > 0)
            {
                model = DapperConnection.minebea.GetModel<t_zh_advert>(new {id = id});
                if (model == null)
                {
                    JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                    return;
                }

            }
            else
            {
                model = new t_zh_advert
                {
                    create_date = DateTime.Now,
                    create_by = u_id.ToString(),
                };
            }
            model.title = txt_title.Text;
            model.link_url = txt_link_url.Text;
            model.img_url = txtFile.Value;
            model.sort_id = Utils.StrToInt(txt_sort.Text,0);
            model.status = status;


            if (id > 0)
            {
                if (DapperConnection.minebea.UpdateModel<t_zh_advert>(model, "id"))
                {
                    AddLog($"修改广告位信息：{model.title}", LogType.修改);
                    JsHelper.AlertAndRedirect("保存成功！", hfReturnUrl.Value);
                }
                else
                {
                    JsHelper.AlertAndRedirect("保存失败！", hfReturnUrl.Value);
                }
            }
            else
            {
                var newmodel = DapperConnection.minebea.AddModel<t_zh_advert>(model, "id");
                if (newmodel.id > 0)
                {
                    AddLog($"添加广告位信息：{model.title}", LogType.修改);
                    JsHelper.AlertAndRedirect("保存成功！", hfReturnUrl.Value);
                }
                else
                {
                    JsHelper.AlertAndRedirect("保存失败！", hfReturnUrl.Value);
                }
            }
        }

        //private string GetRole()
        //{
        //    var products = "";
        //    //具体权限
        //    string[] child_list = hfactions.Value.Split(new string[] { "~!@#" }, StringSplitOptions.RemoveEmptyEntries);
        //    foreach (string child in child_list)
        //    {
        //        string[] c_list = child.Split('|');
        //        products += (c_list[1].Trim() == "" ? "" : ",") + c_list[1];
        //    }

        //    return products;
        //}

        private class Imgs_Url
        {
            public string img_url { get; set; }
        }
    }
}