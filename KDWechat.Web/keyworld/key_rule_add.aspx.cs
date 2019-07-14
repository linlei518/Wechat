using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using System.Text;
using System.Data;

namespace KDWechat.Web.keyworld
{
    public partial class key_rule_add : KDWechat.Web.UI.BasePage
    {
        protected string loadjs = string.Empty;
        protected string strLength = "600";
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CheckWXid();


                WriteReturnPage(hfReturlUrl, "key_rule_list.aspx?m_id=" + m_id);
                if (id > 0)
                {
                    CheckUserAuthority("key_reply_rule", RoleActionType.Edit, hfReturlUrl.Value);
                    bindDisplay();
                }
                else
                {
                    CheckUserAuthority("key_reply_rule", RoleActionType.Add, hfReturlUrl.Value);
                }

            }
        }



        private void bindDisplay()
        {
            t_wx_rules model = KDWechat.BLL.Chats.wx_rules.GetModel(id);

            if (model != null)
            {
                if (model.wx_id != wx_id)
                {
                    //Response.Redirect("multi_news_add.aspx?is_pub=" + is_pub+"&m_id="+m_id);
                    JsHelper.AlertAndRedirect("访问地址错误", hfReturlUrl.Value);
                }
                txtrule_name.Value = model.rule_name;
                hftitle.Value = model.rule_name;
                if (model.status == 1)
                {
                    rboStatusOk.Checked = true;
                }
                else if (model.status == 0)
                {
                    rboStatusNo.Checked = true;
                }

                #region 取出素材

                t_wx_rule_reply reply = KDWechat.BLL.Chats.wx_rule_reply.GetModelByRid(id, wx_id, wx_og_id);
                if (reply != null)
                {
                    switch (reply.reply_type)
                    {
                        case (int)msg_type.文本:
                            txtContents.Value = reply.contents;
                            strLength = (600 - reply.contents.Trim().Length).ToString();
                            break;
                        case (int)msg_type.图片:
                        case (int)msg_type.语音:
                        case (int)msg_type.视频:
                            int _channel_id = 1;
                            if (model.reply_type == (int)msg_type.语音)
                                _channel_id = 2;
                            else if (model.reply_type == (int)msg_type.视频)
                                _channel_id = 3;
                            t_wx_media_materials m = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)reply.source_id);
                            if (m != null)
                            {
                                loadjs = "selectResult(" + _channel_id + ", " + reply.source_id + ",'" + m.file_url + "','" + m.title + "','" + m.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(m.remark, 140).Replace("\n", "").Trim()) + "',0,'','" + (_channel_id == 3 ? m.hq_music_url : "") + "'," + (_channel_id == 3 ? (int)m.video_type : 0) + ");";

                            }
                            break;

                        case (int)msg_type.单图文:
                            t_wx_news_materials m2 = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)reply.source_id);
                            if (m2 != null)
                            {
                                loadjs = "selectResult(4, " + reply.source_id + ",'" + m2.cover_img + "','" + m2.title + "','" + m2.create_time.ToString("yyyy-MM-dd") + "','" + m2.summary.Replace("\n", "").Trim() + "',0,'','',0);";
                            }
                            break;
                        case (int)msg_type.多图文:
                            t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)reply.source_id);
                            if (multi != null)
                            {
                                //取出子级图文
                                //string child_str = string.Empty;
                                //List<t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(multi.id);
                                //if (list != null)
                                //{
                                //    foreach (t_wx_news_materials item in list)
                                //    {
                                //        child_str += "<div class=\"infoField\"><div class=\"img\"> <span><img src=\"" + item.cover_img + "\" > </span> </div><div class=\"title\"><h1>" + item.title + "</h1></div> </div>";
                                //    }

                                //}
                                loadjs = "selectResult(5, " + reply.source_id + ",'" + multi.cover_img + "','" + multi.title + "','" + multi.create_time.ToString("yyyy-MM-dd") + "','" + multi.summary.Replace("\n", "").Trim() + "',0,'" + multi.multi_html.Replace("\n", "").Trim() + "','',0);";
                            }
                            break;
                        case (int)msg_type.模块:
                            DataTable dt = BLL.Chats.module_wechat.GetListByQuery("select id,module_id,wx_id,app_name,app_img_url,app_remark,(select title from t_modules where id=module_id) as module_name  from t_module_wechat where id=" + (int)reply.source_id);
                            if (dt != null)
                            {
                                loadjs = "selectResult(8, " + reply.source_id + ",'" + dt.Rows[0]["app_img_url"] + "','" + "【" + dt.Rows[0]["module_name"] + "】" + dt.Rows[0]["app_name"] + "','','" + (Common.Utils.DropHTML(dt.Rows[0]["app_remark"].ToString(), 140).Replace("\n", "").Trim()) + "',0,'','',0);";
                            }
                            break;
                        case (int)msg_type.多客服:
                            loadjs = "multiCustomerClick(7);";//to do
                            break;
                    }
                }
                #endregion

                #region 取出关键字
                string keys = KDWechat.BLL.Chats.wx_rules_keywords.GetKeywordLists(id);
                hfkey.Value = keys;
                #endregion

            }
            else
            {
                JsHelper.AlertAndRedirect("该规则不存在", hfReturlUrl.Value);
            }
        }

        //提交
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            CheckWXid();
            string log_title = (id == 0 ? "新建" : "修改") + "关键词自动回复，规则名：";
            #region 规则主表
            t_wx_rules model = new t_wx_rules()
                {
                    create_time = DateTime.Now,
                    id = id,
                    rule_name = Common.Utils.DropHTML(txtrule_name.Value.Trim()),
                    status = rboStatusOk.Checked == true ? 1 : 0,
                    u_id = u_id,
                    wx_id = wx_id,
                    wx_og_id = wx_og_id
                };
            log_title += model.rule_name + "，关键词：" + hfkey.Value + "，回复类型：";
            model.sort_id = Common.Utils.StrToInt(hftype.Value, 0);
            switch (hftype.Value)
            {
                case "0":
                    model.sort_id = 6;
                    model.reply_type = (int)msg_type.文本;
                    log_title += "文本，内容为：" + Common.Utils.Filter(txtContents.Value.Trim());
                    break;
                case "1":
                    model.sort_id = 5;
                    model.reply_type = (int)msg_type.图片;
                    log_title += "图片，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                    break;
                case "2":
                    model.sort_id = 4;
                    model.reply_type = (int)msg_type.语音;
                    log_title += "语音，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                    break;
                case "3":
                    model.sort_id = 3;
                    model.reply_type = (int)msg_type.视频;
                    log_title += "视频，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                    break;

                case "5":
                    model.sort_id = 2;
                    model.reply_type = (int)msg_type.多图文;
                    log_title += "多图文，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                    break;
                case "4":
                    model.sort_id = 1;
                    model.reply_type = (int)msg_type.单图文;
                    log_title += "单图文，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                    break;
                case "8":
                    model.sort_id = 0;
                    model.reply_type = (int)msg_type.模块;
                    log_title += "模块类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                    break;
                case "10":
                    model.sort_id = 0;
                    model.reply_type = (int)msg_type.多客服;
                    log_title += "多客服类型";
                    break;
            }



            #endregion

            #region 关键词表
            List<t_wx_rules_keywords> list_key = new List<t_wx_rules_keywords>();
            string[] list = hfkey.Value.Trim().Split(new char[] { '|' });
            if (list.Length > 0)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    string key = Common.Utils.DropHTML(list[i]);
                    if (key.Trim().Length > 0)
                    {
                        list_key.Add(new t_wx_rules_keywords()
                        {
                            eq_type = 1,
                            key_words = key,
                            r_id = model.id,
                            reply_type = model.reply_type,
                            u_id = u_id,
                            wx_id = wx_id,
                            wx_og_id = wx_og_id
                        });
                    }

                }
            }

            #endregion

            #region 回复的素材
            t_wx_rule_reply reply = new t_wx_rule_reply()
            {
                contents = Common.Utils.DropHTMLOnly(txtContents.Value.Trim()),
                r_id = model.id,
                reply_type = model.reply_type,
                source_id = Common.Utils.StrToInt(hfid.Value, 0),
                wx_id = wx_id,
                wx_og_id = wx_og_id
            };
            #endregion

            if (id > 0 && KDWechat.BLL.Chats.wx_rules.GetModel(id) != null)
            {
                if (hftitle.Value != model.rule_name)
                {
                    int result = BLL.Chats.wx_wechats.CheckUserExists(hftitle.Value, model.rule_name,  "t_wx_rules", 0, 0, 0, wx_id);
                    if (result > 0)
                    {
                        JsHelper.AlertAndRedirect("规则名已存在", hfReturlUrl.Value, "fail");
                        return;
                    }
                }
                if (KDWechat.BLL.Chats.wx_rules.Update(model, list_key, reply))
                {
                    AddLog(log_title, LogType.修改);
                    JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                }
                else
                {
                    JsHelper.Alert(Page, "保存失败", "true");
                }
            }
            else
            {
                int result = BLL.Chats.wx_wechats.CheckUserExists(hftitle.Value, model.rule_name, "t_wx_rules", 0, 0, 0, wx_id);
                if (result > 0)
                {
                    JsHelper.AlertAndRedirect("规则名已存在", hfReturlUrl.Value, "fail");
                    return;
                }

                if (KDWechat.BLL.Chats.wx_rules.Add(model, list_key, reply) > 0)
                {
                    AddLog(log_title, LogType.添加);
                    JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                }
                else
                {
                    JsHelper.Alert(Page, "保存失败", "true");
                }


            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }
    }
}