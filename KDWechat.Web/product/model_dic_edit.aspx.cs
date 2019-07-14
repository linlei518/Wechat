using System;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.product
{
    public partial class model_dic_edit : BasePage
    {
        public int model_id => RequestHelper.GetQueryInt("model_id", 0);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteReturnPage(hfReturnUrl, "product_list");

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


        private void bindData()
        {
            var model = DapperConnection.minebea.GetModel<t_product_model_dic>(new { id = id });
            if (model != null)
            {
                txtKey.Text = model.model_key;
                txtVal.Text = model.model_val;
                txtSortId.Text = model.sort_id.ToString();
               
            }
            else
            {
                JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                return;
            }
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            t_product_model_dic model = null;
            if (id > 0)
            {
                model = DapperConnection.minebea.GetModel<t_product_model_dic>(new {id = id });
                if (model == null)
                {
                    JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                    return;
                }
            }
            else
            {
                model = new t_product_model_dic
                {
               
                };
            }
            model.model_key = txtKey.Text;
            model.model_val = txtVal.Text;
            model.model_id = model_id;
            model.sort_id= Utils.StrToInt(txtSortId.Text,99);
         
            
            if (id > 0)
            {
                if (DapperConnection.minebea.UpdateModel<t_product_model_dic>(model, "id"))
                {
                    AddLog($"修改型号属性信息：{model.model_key}", LogType.修改);
                    JsHelper.AlertAndRedirect("保存成功！", hfReturnUrl.Value);
                }
                else
                {
                    JsHelper.AlertAndRedirect("保存失败！", hfReturnUrl.Value);
                }
            }
            else
            {
                model.id = id;
                var newmodel = DapperConnection.minebea.AddModel<t_product_model_dic>(model, "id");
                if (newmodel.id> 0)
                {
                    AddLog($"添加型号属性信息：{model.model_key}", LogType.修改);
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