using System;
using System.Collections.Generic;
using System.Linq;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.zh_user
{
    public partial class park_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteReturnPage(hfReturnUrl, "park_list");

                showInfo();
                if (id > 0)
                {
                    CheckUserAuthority("park_list", RoleActionType.Edit, "park_list");
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
            var model = DapperConnection.minebea.GetModel<t_parking>(new { id = id });
            if (model != null)
            {
                txt_parking_num.Text = model.parking_num;
                txt_user_name.Text = model.user_name;
                hid_user_code.Value = model.user_code;
                lab_user_code.Text = model.user_code;
                txt_plate_number.Text = model.plate_number;
                txt_user_tel.Text = model.user_tel;
                ddlpark_id.SelectedValue = model.park_id.ToString();
                hid_user_id.Value = model.user_id.ToString();
            }
            else
            {
                JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                return;
            }
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {


            t_parking model = null;
            if (id > 0)
            {
                model = DapperConnection.minebea.GetModel<t_parking>(new { id = id });
                if (model == null)
                {
                    JsHelper.AlertAndRedirect("内容已不存在！", hfReturnUrl.Value);
                    return;
                }
              
            }
            else
            {
                model = new t_parking
                {
                    create_date = DateTime.Now,
                    create_by = u_id.ToString(),
                };
            }

            model.user_name = txt_user_name.Text;
            model.user_code = hid_user_code.Value;
            model.plate_number = txt_plate_number.Text;
            model.parking_num = txt_parking_num.Text;
            model.user_tel = txt_user_tel.Text;
            model.park_id = Utils.StrToInt(ddlpark_id.SelectedValue,0);
            model.user_id = Utils.StrToInt(hid_user_id.Value, 0);

            if (id > 0)
            {
                model.updata_by = u_id.ToString();
                model.updata_date = DateTime.Now;
                if (DapperConnection.minebea.UpdateModel<t_parking>(model, "id"))
                {
                    AddLog($"修改车位信息：{model.user_name}", LogType.修改);
                    JsHelper.AlertAndRedirect("保存成功！", hfReturnUrl.Value);
                }
                else
                {
                    JsHelper.AlertAndRedirect("保存失败！", hfReturnUrl.Value);
                }
            }
            else
            {
               var  oldmodel = DapperConnection.minebea.GetModel<t_parking>(new { parking_num =model.parking_num });
                model.status = (int)Enums.ParkingStatus.空;
                if (oldmodel != null) //车位号重复
                {
                    JsHelper.Alert("车位编号已存在！");
                    return;
                }

                var newmodel = DapperConnection.minebea.AddModel<t_parking>(model, "id");
                if (newmodel.id> 0)
                {
                    AddLog($"添加车位信息：{model.user_name}", LogType.修改);
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