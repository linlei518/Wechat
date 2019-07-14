using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;
using System.Text;
using System.Linq.Expressions;
using System.Data;

namespace KDWechat.Web.keyworld
{
    public partial class menu_edit : KDWechat.Web.UI.BasePage
    {
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected string action
        {
            get { return RequestHelper.GetQueryString("action"); }
        }

        protected string loadjs = string.Empty;
        protected string strLength = "600";

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("wechat_diymenu");

            if(!IsPostBack)
            {
                WriteReturnPage(hfReturlUrl, "menu_list.aspx?m_id=" + m_id );
                BindMenuParent(0);
                
                if(action=="Edit" && id>0)
                {
                    BindData(id); 
                }
                
            }
        }

        //绑定菜单数据
        private void BindMenuParent(int par_id)
        {
           
            List<DAL.t_wx_diy_menus> menuList = BLL.Chats.wx_diy_menus.GetListByWxIdAndParentId(wx_id,par_id); //bll.GetModelList("parent_id=" + par_id + " and wx_id=" + wx_id);
            this.ddlMParent.Items.Clear();
            if (menuList.Count < 3)  //顶级菜单最多为3个
            {
                this.ddlMParent.Items.Add(new ListItem("顶级菜单", "0"));
            }
            foreach (var item in menuList)
            {
                this.ddlMParent.Items.Add(new ListItem(item.menu_name, item.id.ToString()));
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="m_id"></param>
        private void BindData(int m_id)
        {
            DAL.t_wx_diy_menus modeldiy = new DAL.t_wx_diy_menus();

            modeldiy = BLL.Chats.wx_diy_menus.GetModel(m_id);
            if (modeldiy!=null) //韦章飞 修改。之前没判断是否为空
            {
                if (modeldiy.wx_id != wx_id) //韦章飞 修改。判断当前菜单是否是当前微信号的
                {
                    JsHelper.AlertAndRedirect("访问地址错误!", hfReturlUrl.Value);
                    return;
                }

                string parent_id = modeldiy.parent_id.ToString();
                if (parent_id == "0")
                {
                    this.ddlMParent.Items.Insert(0, new ListItem("顶级菜单", "0"));
                    this.ddlMParent.SelectedIndex = 0;
                    ddlMParent.Enabled = false;
                }
                else
                {
                    ddlMParent.SelectedValue = parent_id;
                }
                mname.Value = modeldiy.menu_name;
                hftitle.Value = modeldiy.menu_name;
                hfparentid.Value = modeldiy.parent_id.ToString();
                int reply_type = Common.Utils.ObjToInt(modeldiy.reply_type, -1);

                switch (reply_type)
                {
                    case -1:
                        break;
                    case (int)msg_type.文本:
                        if (modeldiy.contents != null)
                        {
                            txtContents.Value = modeldiy.contents;
                            strLength = (600 - modeldiy.contents.Trim().Length).ToString();
                        }
                        break;
                    case (int)msg_type.图片:

                    case (int)msg_type.语音:
                    case (int)msg_type.视频:
                        int _channel_id = 1;
                        if (modeldiy.reply_type == (int)msg_type.语音)
                            _channel_id = 2;
                        else if (modeldiy.reply_type == (int)msg_type.视频)
                            _channel_id = 3;
                        t_wx_media_materials m = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)modeldiy.soucre_id);
                        if (m != null)
                        {
                            loadjs = "selectResult(" + _channel_id + ", " + modeldiy.soucre_id + ",'" + (_channel_id == 3 ? m.hq_music_url : m.file_url) + "','" + m.title + "','" + m.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(m.remark, 40).Replace("\n", "").Trim()) + "',0);";

                        }
                        break;

                    case (int)msg_type.单图文:
                        t_wx_news_materials m2 = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)modeldiy.soucre_id);
                        if (m2 != null)
                        {
                            loadjs = "selectResult(4, " + modeldiy.soucre_id + ",'" + m2.cover_img + "','" + m2.title + "','" + m2.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(m2.summary, 40).Replace("\n", "").Trim()) + "',0);";
                        }
                        break;

                    case (int)msg_type.多图文:
                        t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)modeldiy.soucre_id);
                        if (multi != null)
                        {
                            //取出子级图文
                            string child_str = string.Empty;
                            List<t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(multi.id);
                            if (list != null)
                            {
                                foreach (t_wx_news_materials item in list)
                                {
                                    child_str += "<div class=\"infoField\"><div class=\"img\"> <span><img src=\"" + item.cover_img + "\" > </span> <div class=\"tip\">缩略图</div></div><div class=\"title\"><h1>" + item.title + "</h1></div> </div>";
                                }

                            }
                            loadjs = "selectResult(5, " + modeldiy.soucre_id + ",'" + multi.cover_img + "','" + multi.title + "','" + multi.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(multi.summary, 40).Replace("\n", "").Trim()) + "',0,'" + child_str + "');";
                        }
                        break;
                    case (int)msg_type.外链:
                        if (modeldiy.menu_url != null)
                        {
                            txtlike.Value = modeldiy.menu_url;
                        }
                        loadjs = "selectList(6)";

                        break;
                    case (int)msg_type.授权:
                        if (modeldiy.menu_url != null)
                        {
                            txtauthor.Value = modeldiy.menu_url;
                        }
                        loadjs = "selectList(7)";

                        break;
                    case (int)msg_type.模块:
                        DataTable dt = BLL.Chats.module_wechat.GetListByQuery("select id,module_id,wx_id,title,img_url,description,(select title from t_modules where id=module_id) as module_name  from t_module_wechat where id=" + (int)modeldiy.soucre_id);
                        if (dt != null)
                        {
                            loadjs = "selectResult(8, " + modeldiy.soucre_id + ",'" + dt.Rows[0]["img_url"] + "','" + "【" + dt.Rows[0]["module_name"] + "】" + dt.Rows[0]["title"] + "','','" + (Common.Utils.DropHTML(dt.Rows[0]["description"].ToString(), 40).Replace("\n", "").Trim()) + "',0,'','',0);";
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                JsHelper.AlertAndRedirect("数据不存在", "menu_list.aspx?m_id="+m_id);
            }
           
        }
      

     
      
        //检查子菜单数量
        private bool CheckChirdrenCount(int menuParent)
        {

            int rows = BLL.Chats.wx_diy_menus.GetCountByWxIdAndParentId(wx_id, menuParent);

            if (rows < 5)//二级菜单最多为5个
            {

                return true;
            }

            return false;
        }
        /// <summary>
        /// 修改有子菜单的菜单的状态
        /// </summary>
        private void UpdateRetype(int par_id)
        {
            BLL.Chats.wx_diy_menus.UpdateParent(par_id, "default", -1);
        }
        /// <summary>
        /// 增加操作
        /// </summary>
        /// <returns></returns>
        private bool DoAdd()
        {
            string log_title = "新增自定义菜单，";
            bool hasChirdren = false;

            int par_Id = Common.Utils.StrToInt(ddlMParent.SelectedValue,0);
            string name = mname.Value.Trim();
            log_title += "名称为:"+name;
            //判断字计数
            if (par_Id == 0)
            {
                if (!GetByteLen(name, 16))
                {
                    JsHelper.AlertAndRedirect("一级菜单名称不多于8个汉字或16个字母", "menu_edit.aspx?m_id=" + m_id);
                    return false;
                }
            }
            else 
            {
                if (!GetByteLen(name, 16))
                {
                    JsHelper.AlertAndRedirect("二级菜单名称不多于8个汉字或16个字母", "menu_edit.aspx?m_id=" + m_id);
                    return false;
                }
            }

            hasChirdren = CheckChirdrenCount(par_Id);

            if (hasChirdren)
            {
                if (par_Id > 0)
                {
                    UpdateRetype(par_Id);
                }

                int intSel = Common.Utils.StrToInt(hftype.Value, -1);
               
                string m_key = HzToPy.GetChineseSpell(name) + Number(6,true); //菜单key(首字母大写+六位随机数),不可修改
                int m_id = 0;

                //菜单赋值
                DAL.t_wx_diy_menus model = new DAL.t_wx_diy_menus();
                model.wx_id = wx_id;
                model.wx_og_id = wx_og_id;
                model.u_id = u_id;
                model.menu_name = name;
                model.menu_url = "";
                model.menu_key = m_key;
                model.create_time = DateTime.Now;
                model.sort_id = 0; //排序,默认为0
                model.parent_id = Common.Utils.StrToInt(ddlMParent.SelectedValue, 0);
                model.soucre_id = Common.Utils.StrToInt(hfid.Value, 0);
               
                switch (intSel)
                {
                    case 0: 
                        model.reply_type = (int)msg_type.文本;               
                        model.menu_type = "click";  //菜单类型，click view(超链接：view，推送事件：click)
                        model.contents = Common.Utils.Filter(txtContents.Value.Trim());
                       
                        break;
                    case 1: 
                        model.reply_type = (int)msg_type.图片;                
                        model.menu_type = "click";
                        model.contents = "";
                       
                        break;
                    case 2:                     
                        model.reply_type = (int)msg_type.语音;                
                        model.menu_type = "click";
                        model.contents = "";
                       
                        break;
                    case 3:
                        model.menu_type = "click";
                        model.reply_type = (int)msg_type.视频;
                        model.contents = "";
                        break;
                    case 4:                     
                        model.reply_type = (int)msg_type.单图文;                
                        model.menu_type = "click";
                        model.contents = "";

                        break;
                    case 5:
                        model.reply_type = (int)msg_type.多图文;
                        model.menu_type = "click";
                        model.contents = "";
                        break;
                    case 6:
                        model.reply_type = (int)msg_type.外链;
                        model.menu_type = "view";
                        model.menu_url = txtlike.Value.Trim();
                        break;
                    case 7:
                        model.reply_type = (int)msg_type.授权;
                        model.menu_type = "view";
                        model.menu_url = txtauthor.Value.Trim();
                        break;
                    case 8:
                        model.reply_type = (int)msg_type.模块;
                        model.menu_type = "click";
                        model.contents = "";
                        break;
                    default:                     //一级菜单
                        model.reply_type = -1;
                        model.menu_type = "";
                        model.contents = "";
                       
                        break;
                }
                m_id = BLL.Chats.wx_diy_menus.Add(model).id;
                if (m_id > 0)
                {
                    AddLog(log_title,LogType.添加);
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            else
            {
                JsHelper.AlertAndRedirect("二级菜单最多添加5个", "menulist.aspx?m_id=" + m_id);
                return false;
            }

        }
        /// <summary>
        /// 判断字节数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        protected bool GetByteLen(string str,int len)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            int b_len = bytes.Length;
            if (b_len > len)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        private bool DoEdit(int m_id)
        {
            string log_title = "修改自定义菜单，";

            int par_Id = Common.Utils.StrToInt(ddlMParent.SelectedValue,0);
            string name = mname.Value.Trim();

            //判断字计数
            if (par_Id == 0)
            {
                if (!GetByteLen(name, 16))
                {
                    JsHelper.AlertAndRedirect("一级菜单名称不多于8个汉字或16个字母", "menu_edit.aspx?id="+id+"&m_id="+m_id);
                    return false;
                }
            }
            else
            {
                if (!GetByteLen(name, 16))
                {
                    JsHelper.AlertAndRedirect("二级菜单名称不多于8个汉字或16个字母", "menu_edit.aspx?id=" + id+"&m_id="+m_id);
                    return false;
                }
            }

            if (par_Id > 0)
            {
                UpdateRetype(par_Id);//有子菜单的父菜单回复类型修改为-1;
            }

            int intSel = Common.Utils.StrToInt(hftype.Value, -1);
            //菜单赋值
            DAL.t_wx_diy_menus model = new DAL.t_wx_diy_menus();
            model = BLL.Chats.wx_diy_menus.GetModel(m_id);
            log_title += "ID:" + m_id;
            model.menu_name = name;
            model.create_time = DateTime.Now;
            model.parent_id = Common.Utils.StrToInt(ddlMParent.SelectedValue, 0);
            model.soucre_id = Common.Utils.StrToInt(hfid.Value, 0);
            string m_key = model.menu_key;

            switch (intSel)
            {
                case 0:
                    model.reply_type = (int)msg_type.文本;
                    model.menu_type = "click";  //菜单类型，click view(超链接：view，推送事件：click)
                    model.contents = Common.Utils.Filter(txtContents.Value.Trim());
                    break;
                case 1:
                    model.reply_type = (int)msg_type.图片;
                    model.menu_type = "click";
                    model.contents = "";
                    break;
                case 2:
                    model.reply_type = (int)msg_type.语音;
                    model.menu_type = "click";
                    model.contents = "";

                    break;
                case 3:
                    model.menu_type = "click";
                    model.reply_type = (int)msg_type.视频;
                    model.contents = "";
                    break;
                case 4:
                    model.reply_type = (int)msg_type.单图文;
                    model.menu_type = "click";
                    model.contents = "";
                    break;
                case 5:
                    model.reply_type = (int)msg_type.多图文;
                    model.menu_type = "click";
                    model.contents = "";
                    break;
                case 6:
                    model.reply_type = (int)msg_type.外链;
                    model.menu_type = "view";
                    model.menu_url = txtlike.Value.Trim();
                    break;
                case 7:
                    model.reply_type = (int)msg_type.授权;
                    model.menu_type = "view";
                    model.menu_url = txtauthor.Value.Trim();
                    break;
                case 8:
                    model.reply_type = (int)msg_type.模块;
                    model.menu_type = "click";
                    //model.menu_url = txtmodule.Value.Trim();
                    break;
                default:                     //一级菜单
                    model.reply_type = -1;
                    model.menu_type = "";
                    model.contents = "";
                    break;
            }
            m_id = BLL.Chats.wx_diy_menus.Update(model).id;
            if (m_id > 0)
            {
                AddLog(log_title,LogType.修改);
                return true;
            }
            else
            {
                return false;
            }
         
          
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
           
           
            if (id > 0) //修改
            {

                if (!DoEdit(this.id))
                {

                    JsHelper.Alert("保存过程中发生错误啦");
                    return;
                }
                JsHelper.AlertAndRedirect("修改菜单成功啦", "menu_list.aspx?m_id="+m_id);

            }
            else //添加
            {

                if (!DoAdd())
                {
                    JsHelper.Alert("保存过程中发生错误啦");
                    return;
                }
                JsHelper.AlertAndRedirect("添加菜单成功啦", "menu_list.aspx?m_id=" + m_id);
            }
        }
       
       
    }
}