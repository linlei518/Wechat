﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KDWechat.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class creater_wxEntities : DbContext
    {
        public creater_wxEntities()
            : base("name=creater_wxEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<t_category> t_category { get; set; }
        public virtual DbSet<t_member_fans> t_member_fans { get; set; }
        public virtual DbSet<t_member_info> t_member_info { get; set; }
        public virtual DbSet<t_messages> t_messages { get; set; }
        public virtual DbSet<t_qy_manager> t_qy_manager { get; set; }
        public virtual DbSet<t_retrans_keyword> t_retrans_keyword { get; set; }
        public virtual DbSet<t_retrans_server> t_retrans_server { get; set; }
        public virtual DbSet<t_retrans_times> t_retrans_times { get; set; }
        public virtual DbSet<t_st_diymenu_click> t_st_diymenu_click { get; set; }
        public virtual DbSet<t_st_keyword_view> t_st_keyword_view { get; set; }
        public virtual DbSet<t_sys_letter> t_sys_letter { get; set; }
        public virtual DbSet<t_sys_letter_receiver> t_sys_letter_receiver { get; set; }
        public virtual DbSet<t_sys_navigation> t_sys_navigation { get; set; }
        public virtual DbSet<t_sys_tags> t_sys_tags { get; set; }
        public virtual DbSet<t_sys_user_manage_child_sys> t_sys_user_manage_child_sys { get; set; }
        public virtual DbSet<t_sys_users> t_sys_users { get; set; }
        public virtual DbSet<t_sys_users_fans> t_sys_users_fans { get; set; }
        public virtual DbSet<t_sys_users_power> t_sys_users_power { get; set; }
        public virtual DbSet<t_tags_category> t_tags_category { get; set; }
        public virtual DbSet<t_wx_basic_reply> t_wx_basic_reply { get; set; }
        public virtual DbSet<t_wx_diy_menus> t_wx_diy_menus { get; set; }
        public virtual DbSet<t_wx_error_logs> t_wx_error_logs { get; set; }
        public virtual DbSet<t_wx_fans> t_wx_fans { get; set; }
        public virtual DbSet<t_wx_fans_chats> t_wx_fans_chats { get; set; }
        public virtual DbSet<t_wx_fans_groups> t_wx_fans_groups { get; set; }
        public virtual DbSet<t_wx_fans_hisactivity> t_wx_fans_hisactivity { get; set; }
        public virtual DbSet<t_wx_fans_hislocation> t_wx_fans_hislocation { get; set; }
        public virtual DbSet<t_wx_fans_hisview> t_wx_fans_hisview { get; set; }
        public virtual DbSet<t_wx_fans_tags> t_wx_fans_tags { get; set; }
        public virtual DbSet<t_wx_group_msg_key> t_wx_group_msg_key { get; set; }
        public virtual DbSet<t_wx_group_msg_read_percent_statistics> t_wx_group_msg_read_percent_statistics { get; set; }
        public virtual DbSet<t_wx_group_msgs> t_wx_group_msgs { get; set; }
        public virtual DbSet<t_wx_group_tags> t_wx_group_tags { get; set; }
        public virtual DbSet<t_wx_jsapi_ticket> t_wx_jsapi_ticket { get; set; }
        public virtual DbSet<t_wx_lbs> t_wx_lbs { get; set; }
        public virtual DbSet<t_wx_logs> t_wx_logs { get; set; }
        public virtual DbSet<t_wx_media_materials> t_wx_media_materials { get; set; }
        public virtual DbSet<t_wx_news_materials> t_wx_news_materials { get; set; }
        public virtual DbSet<t_wx_qrcode> t_wx_qrcode { get; set; }
        public virtual DbSet<t_wx_qrcode_history> t_wx_qrcode_history { get; set; }
        public virtual DbSet<t_wx_rule_reply> t_wx_rule_reply { get; set; }
        public virtual DbSet<t_wx_rules> t_wx_rules { get; set; }
        public virtual DbSet<t_wx_rules_keywords> t_wx_rules_keywords { get; set; }
        public virtual DbSet<t_wx_templates> t_wx_templates { get; set; }
        public virtual DbSet<t_wx_templates_wechats> t_wx_templates_wechats { get; set; }
        public virtual DbSet<t_wx_wechats> t_wx_wechats { get; set; }
        public virtual DbSet<t_st_graohic_click> t_st_graohic_click { get; set; }
        public virtual DbSet<t_st_graphic_share> t_st_graphic_share { get; set; }
        public virtual DbSet<t_st_graphic_view> t_st_graphic_view { get; set; }
        public virtual DbSet<t_st_linnkout_view> t_st_linnkout_view { get; set; }
        public virtual DbSet<t_module_menu> t_module_menu { get; set; }
        public virtual DbSet<t_module_wechat> t_module_wechat { get; set; }
        public virtual DbSet<t_module_wx_switch> t_module_wx_switch { get; set; }
        public virtual DbSet<t_module_wx_user_role> t_module_wx_user_role { get; set; }
        public virtual DbSet<t_modules> t_modules { get; set; }
        public virtual DbSet<t_invitation> t_invitation { get; set; }
        public virtual DbSet<t_product> t_product { get; set; }
        public virtual DbSet<t_product_model> t_product_model { get; set; }
        public virtual DbSet<t_product_model_dic> t_product_model_dic { get; set; }
        public virtual DbSet<t_product_type> t_product_type { get; set; }
        public virtual DbSet<t_contact> t_contact { get; set; }
        public virtual DbSet<t_draw_list> t_draw_list { get; set; }
        public virtual DbSet<sysUser_WeChat_View> sysUser_WeChat_View { get; set; }
        public virtual DbSet<retrans_keyword_server_view> retrans_keyword_server_view { get; set; }
        public virtual DbSet<retrans_times_server_view> retrans_times_server_view { get; set; }
        public virtual DbSet<view_all_history_list> view_all_history_list { get; set; }
        public virtual DbSet<view_dis_history_list> view_dis_history_list { get; set; }
        public virtual DbSet<view_fans_actview> view_fans_actview { get; set; }
        public virtual DbSet<view_fans_hisview> view_fans_hisview { get; set; }
        public virtual DbSet<view_project_category> view_project_category { get; set; }
        public virtual DbSet<wechat_groupmsg_view> wechat_groupmsg_view { get; set; }
        public virtual DbSet<wx_tags_relation_View> wx_tags_relation_View { get; set; }
        public virtual DbSet<t_zh_user> t_zh_user { get; set; }
        public virtual DbSet<t_parking> t_parking { get; set; }
        public virtual DbSet<t_zh_report> t_zh_report { get; set; }
        public virtual DbSet<t_zh_advert> t_zh_advert { get; set; }
    
        public virtual ObjectResult<p_graphi_statistics_Result> p_graphi_statistics(Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate, Nullable<int> wx_id)
        {
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("startDate", startDate) :
                new ObjectParameter("startDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("endDate", endDate) :
                new ObjectParameter("endDate", typeof(System.DateTime));
    
            var wx_idParameter = wx_id.HasValue ?
                new ObjectParameter("wx_id", wx_id) :
                new ObjectParameter("wx_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<p_graphi_statistics_Result>("p_graphi_statistics", startDateParameter, endDateParameter, wx_idParameter);
        }
    
        public virtual int P_PageView(string queryStr, Nullable<int> pageSize, Nullable<int> pageCurrent, string fdShow, string fdOrder, ObjectParameter rows)
        {
            var queryStrParameter = queryStr != null ?
                new ObjectParameter("QueryStr", queryStr) :
                new ObjectParameter("QueryStr", typeof(string));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var pageCurrentParameter = pageCurrent.HasValue ?
                new ObjectParameter("PageCurrent", pageCurrent) :
                new ObjectParameter("PageCurrent", typeof(int));
    
            var fdShowParameter = fdShow != null ?
                new ObjectParameter("FdShow", fdShow) :
                new ObjectParameter("FdShow", typeof(string));
    
            var fdOrderParameter = fdOrder != null ?
                new ObjectParameter("FdOrder", fdOrder) :
                new ObjectParameter("FdOrder", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("P_PageView", queryStrParameter, pageSizeParameter, pageCurrentParameter, fdShowParameter, fdOrderParameter, rows);
        }
    }
}