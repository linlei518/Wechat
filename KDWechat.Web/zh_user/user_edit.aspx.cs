using System;
using System.Collections.Generic;
using System.Linq;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.zh_user
{
    public partial class user_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteReturnPage(hfReturnUrl, "user_list");

                showInfo();
                if (id > 0)
                {
                    CheckUserAuthority("user_list", RoleActionType.Edit, "user_list");
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
            var model = DapperConnection.minebea.GetModel<t_zh_user>(new { id = id });
            if (model != null)
            {
                txt_user_name.Text = model.user_name;
                txt_user_code.Text = model.user_code;
                txt_plate_number.Text = model.plate_number;
                txt_position.Text = model.user_position;
                txt_user_dpt.Text = model.user_dpt;
                txt_user_mail.Text = model.user_mail;
                txt_plate_number.Text = model.plate_number;
                txt_user_tel.Text = model.user_tel;
                //txt_pwd.Text = DESEncrypt.Decrypt(model.pwd);
                txt_pwd.Attributes.Add("value", DESEncrypt.Decrypt(model.pwd)); 
                txt_pwd.ToolTip = DESEncrypt.Decrypt(model.pwd);
            }
            else
            {
                JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                return;
            }
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {


            t_zh_user model = null;

            List<t_zh_user> other_user = null; //共享车位人员

            List<t_parking> park_list = null; //共享车位



            if (id > 0)
            {
                model = DapperConnection.minebea.GetModel<t_zh_user>(new {id = id});
             
                if (model == null)
                {
                    JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                    return;
                }
                other_user = DapperConnection.minebea.GetList<t_zh_user>(null, new { plate_number = model.plate_number });
                park_list = DapperConnection.minebea.GetList<t_parking>(null, new { plate_number = model.plate_number });

            }
            else
            {
                model = new t_zh_user
                {
                    create_date = DateTime.Now,
                    create_by = u_id.ToString(),
                };
            }
            model.user_name = txt_user_name.Text;
            model.user_mail = txt_user_name.Text;
            model.user_code = txt_user_code.Text;
            model.plate_number = txt_plate_number.Text;
            model.user_position = txt_position.Text;
            model.user_dpt = txt_user_dpt.Text;
            model.user_mail = txt_user_mail.Text;
            model.plate_number = txt_plate_number.Text;
            model.user_tel = txt_user_tel.Text;
            model.pwd = DESEncrypt.Encrypt(txt_pwd.Text.Trim());


            if (id > 0)
            {
                var sql_str = new List<string>();
                var sql_param = new List<object>();

                sql_str.Add(DapperHelper.GetUpdateParamSql(typeof (t_zh_user), "id"));
                sql_param.Add(model);

                foreach (var item in other_user)
                {
                    if (model.id != item.id)
                    {
                        item.plate_number = model.plate_number;
                        sql_str.Add(DapperHelper.GetUpdateParamSql(typeof(t_zh_user), "id"));
                        sql_param.Add(item);
                    }
                }
                foreach (var item in park_list)
                {
                    item.plate_number = model.plate_number;
                    sql_str.Add(DapperHelper.GetUpdateParamSql(typeof (t_parking), "id"));
                    sql_param.Add(item);
                }
                if (DapperConnection.minebea.ExecuteTransaction(sql_str, sql_param))
                {

                    AddLog($"修改员工信息：{model.user_name}", LogType.修改);
                    JsHelper.AlertAndRedirect("保存成功！", hfReturnUrl.Value);
                }
                else
                {
                    JsHelper.AlertAndRedirect("保存失败！", hfReturnUrl.Value);
                }
            }
            else
            {
                var newmodel = DapperConnection.minebea.AddModel<t_zh_user>(model, "id");
                if (newmodel.id > 0)
                {
                    AddLog($"添加员工信息：{model.user_name}", LogType.修改);
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