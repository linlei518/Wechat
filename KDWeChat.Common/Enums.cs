using System.ComponentModel;

namespace KDWechat.Common
{
    public class Enums
    {
        /// <summary>
        /// 数据库聚合函数
        /// </summary>
        public enum SqlValueType { Count, Sum, Max, Min, Avg }


        /// <summary>
        /// 排序
        /// </summary>
        public enum OrderByTypeEnum { ASC, DESC }


        /// <summary>
        /// 菜单类型
        /// </summary>
        public enum Menu_Type { 转到功能页 = 1, 转到另外的网站 = 2 }

        public enum DataLockStatus { 启用 = 1, 禁用 = 0 }


        /// <summary>
        /// 是否
        /// </summary>
        public enum YesOrNo { 是 = 1, 否 = 0 }

        /// <summary>
        /// 车位状态
        /// </summary>
        public enum ParkingStatus { 已停车 = 0, 空 = 1 }

        /// <summary>
        /// 权限动作
        /// </summary>
        public enum RoleActionType
        {
            /// <summary>
            /// 查看
            /// </summary>
            [Description("查看")] View,

            /// <summary>
            /// 添加
            /// </summary>
            [Description("添加")] Add,

            /// <summary>
            /// 修改
            /// </summary>
            [Description("修改")] Edit,

            /// <summary>
            /// 删除
            /// </summary>
            [Description("删除")] Delete,

            /// <summary>
            /// 审核
            /// </summary>
            [Description("审核")]Aduit,

            /// <summary>
            /// 回复
            /// </summary>
            [Description("回复")] Reply,

            /// <summary>
            /// 导入
            /// </summary>
            [Description("导入")] Import,

            /// <summary>
            /// 导出
            /// </summary>
            [Description("导出")] Export,

            /// <summary>
            /// 发布 
            /// </summary>
            [Description("发布")] Release,

            /// <summary>
            /// 付款 
            /// </summary>
            [Description("付款")] Pay,

            /// <summary>
            /// 管理权限
            /// </summary>
            [Description("管理")] Manage,

            /// <summary>
            /// 登录
            /// </summary>
            [Description("登录")] Login,

          
          
            
        }


        /// <summary>
        /// 系统导航菜单类别枚举
        /// </summary>
        public enum ManageTypeEnum
        {
            /// <summary>
            /// 主平台
            /// </summary>
            System = 1,
            /// <summary>
            /// 商户
            /// </summary>
            Merchant = 2
        }

        /// <summary>
        /// 对象枚举
        /// </summary>
        public enum ObjType
        {
            /// <summary>
            /// 用户
            /// </summary>
            User = 1,
            /// <summary>
            /// 部门
            /// </summary>
            Dpt = 2
        }


        /// <summary>
        /// 使用状态
        /// </summary>
        public enum Status { 正常 = 1, 禁用 = 0, 维护 = 3 }

        /// <summary>
        /// 上传类型
        /// </summary>
        public enum upload_type
        {
            [Description("Brand|Image")]
            品牌图片 = 1,
            [Description("Merchant|Image")]
            商户图片 = 2,
            [Description("Merchant|File")]
            商户附件 = 3,
            [Description("SKU|Image")]
            SKU图片 = 4,
            [Description("Consult|Image")]
            资讯图片 = 5,
            [Description("Consult|Image")]
            资讯附件 = 6,
            [Description("Advert|Image")]
            广告图片 = 7,
            [Description("Activity|Image")]
            活动图片 = 8,
            [Description("Sys|Image")]
            基础配置图片 = 8,
            [Description("Sys|File")]
            基础配置附件 = 9,
            [Description("Advert|File")]
            频道图片 = 10,
            [Description("Medal|Image")]
            会员勋章 = 11,
            [Description("Invite|File")]
            邀请码附件 = 12,
            [Description("Coupon|File")]
            优惠券导入 = 13,
            [Description("Prod|Image")]
            实物商品详情 = 14,
            [Description("ProdVirtual|Image")]
            虚拟商品详情 = 15,
            [Description("Comment|Image")]
            评论图片 = 16,
            [Description("Menu|Image")]
            菜单图片 = 17,
            [Description("Mission|Image")]
            任务图片 = 18,
            [Description("Floor|Image")]
            楼层图片 = 19,
            [Description("Product|Image")]
            积分商品图片 = 20,
            [Description("Other|Image")]
            其他 = 100
        }

     
      

        /// <summary>
        /// 后台数据流转状态
        /// </summary>
        public enum Biz_Status
        {
            已删除 = -100,
            编辑中 = 100,
            待审核 = 200,
            已审核 = 300,
            已完成 = 400,
        }

        /// <summary>
        /// 订单数据流转状态
        /// </summary>
        public enum Biz_Order_Status
        {
            已取消 = -100,
            待支付 = 100,
            已支付 = 200,
            //待发货 = 300,
            已完成 = 400,
        }

        /// <summary>
        /// 物流类型
        /// </summary>
        public enum LogisticsType
        {
            自提 = 100,
            快递 = 200,
        }

        /// <summary>
        /// 积分流转类型
        /// </summary>
        public enum  integral_type
        {
            增加 = 0,
            扣减 = 1,
            冻结 = 2,
            解冻 = 3,
        }

        /// <summary>
        /// 积分流转业务类型
        /// </summary>
        public enum biz_integral_type
        {
            预算流转 = 0,
            任务流转 = 100,
            商城流转 = 200,
            自定义流转=300,
            首次登陆赠送= 400,
        }

        /// <summary>
        /// 积分任务报名业务类型
        /// </summary>
        public enum biz_mission_type
        {
            待审核 = 0,//已报名待审核
            审核驳回 = 100,//拒绝报名
            审核通过 = 200,
           
        }


    }
}