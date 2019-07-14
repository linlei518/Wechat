using System;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.product
{
    public partial class product_type_edit : BasePage
    {
        public int product_id= RequestHelper.GetQueryInt("product_id", 0);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteReturnPage(hfReturnUrl, "product_type_list");

                if (id > 0)
                {
                    CheckUserAuthority("product_list", RoleActionType.Edit, "product_type_list");
                    bindData();
                }
                else
                {
                    
                }
            }
        }


        private void bindData()
        {
            var model = DapperConnection.minebea.GetModel<t_product_type>(new { type_id = id });
            if (model != null)
            {
                txtName.Text = model.type_name;
                //txt_link.Text = model.link_url;
                txtSortId.Text = model.sort_id.ToString();
                cbIsLock.Checked = model.is_lock == 1;
            }
            else
            {
                JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                return;
            }
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            t_product_type model = null;
            if (id > 0)
            {
                model = DapperConnection.minebea.GetModel<t_product_type>(new {typeid = id });
                if (model == null)
                {
                    JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                    return;
                }
            }
            else
            {
                model = new t_product_type
                {
                    create_date = DateTime.Now,
                 
                };
            }
            model.type_name = txtName.Text;
            model.is_lock = cbIsLock.Checked ? 1 : 0;
            //model.link_url = txt_link.Text;
            model.sort_id= Utils.StrToInt(txtSortId.Text,99);
            //model. = product_id;
            
            if (id > 0)
            {
                if (DapperConnection.minebea.UpdateModel<t_product_type>(model, "type_id"))
                {
                    AddLog($"修改产品类型信息：{model.type_name}", LogType.修改);
                    JsHelper.AlertAndRedirect("保存成功！", hfReturnUrl.Value);
                }
                else
                {
                    JsHelper.AlertAndRedirect("保存失败！", hfReturnUrl.Value);
                }
            }
            else
            {
                model.type_id = id;
                var newmodel = DapperConnection.minebea.AddModel<t_product_type>(model, "type_id");
                if (newmodel.id> 0)
                {
                    AddLog($"添加产品型号信息：{model.type_name}", LogType.修改);
                    JsHelper.AlertAndRedirect("保存成功！", hfReturnUrl.Value);
                }
                else
                {
                    JsHelper.AlertAndRedirect("保存失败！", hfReturnUrl.Value);
                }
            }
        }

       
        private class Imgs_Url
        {
            public string img_url { get; set; }
        }
    }
}