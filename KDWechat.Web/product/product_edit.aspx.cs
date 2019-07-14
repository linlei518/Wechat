using System;
using System.Collections.Generic;
using System.Linq;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.product
{
    public partial class product_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteReturnPage(hfReturnUrl, "product_list");

                showInfo();
                if (id > 0)
                {
                    CheckUserAuthority("product_list", RoleActionType.Edit, "product_list");
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
            var model = DapperConnection.minebea.GetModel<t_product>(new { product_id = id });
            if (model != null)
            {
                txtName.Text = model.product_name;
                txtDescribe.Text = model.describe;
                txtapplication_describe.Text = model.application_describe;
                //ddlType.SelectedValue = model.type_id.ToString();


                //绑定多图
                hfImgList.Value = model.imgs_url;
                var imgs = model.imgs_url?.Split('|').ToList();
                var img_list = new List<Imgs_Url>();
                if (imgs != null)
                {
                    foreach (var item in imgs)
                    {
                        var img = new Imgs_Url();
                        img.img_url = item;
                        if (!string.IsNullOrWhiteSpace(item))
                            img_list.Add(img);
                    }
                    rptAlbumList.DataSource = img_list;
                    rptAlbumList.DataBind();
                }
                
            }
            else
            {
                JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                return;
            }
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
           

            t_product model = null;
            if (id > 0)
            {
                model = DapperConnection.minebea.GetModel<t_product>(new { product_id = id });
                if (model == null)
                {
                    JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                    return;
                }
                model.update_date = DateTime.Now;
                model.last_person = u_id;
            }
            else
            {
                model = new t_product
                {
                    create_date = DateTime.Now,
                    create_by = u_id,
                };
            }
            model.product_name = txtName.Text.Trim();
            model.describe = txtDescribe.Text.Trim();
            model.application_describe=txtapplication_describe.Text.Trim();
            //model.type_id = Utils.ObjToInt(ddlType.SelectedValue,0);
            model.imgs_url = hfImgList.Value;
            model.sort_id= Utils.StrToInt(txtSortId.Text,99);
           


            if (id > 0)
            {
                if (DapperConnection.minebea.UpdateModel<t_product>(model, "product_id"))
                {
                    AddLog($"修改产品信息：{model.product_name}", LogType.修改);
                    JsHelper.AlertAndRedirect("保存成功！", hfReturnUrl.Value);
                }
                else
                {
                    JsHelper.AlertAndRedirect("保存失败！", hfReturnUrl.Value);
                }
            }
            else
            {
                var newmodel = DapperConnection.minebea.AddModel<t_product>(model, "product_id");
                if (newmodel.product_id> 0)
                {
                    AddLog($"添加产品信息：{model.product_name}", LogType.修改);
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