using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.Common
{
    /// <summary>
    /// 使用状态
    /// </summary>
    public enum Status { 正常 = 1, 禁用 = 0, 维护 = 3 }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserFlag { 总部账号 = 1, 地区账号 = 2, 子账号 = 3, 超级管理员 = 4 }

    /// <summary>
    /// 微信号类型
    /// </summary>
    public enum WeChatServiceType { 普通订阅号 = 1, 认证后订阅号 = 2, 普通服务号 = 3, 认证后服务号 = 4 }

    /// <summary>
    /// 是否已婚
    /// </summary>
    public enum MarriageType { 未婚 = 1, 已婚 = 2 }

    /// <summary>
    /// 是否有孩子
    /// </summary>
    public enum have_childType { 未知, 无, 有 }

    /// <summary>
    /// 消费能力
    /// </summary>
    public enum spending_powerMode { 未知, 强, 中上, 一般, 较弱 }

    /// <summary>
    /// 是否正在关注
    /// </summary>
    public enum AttentionMode { 已解除关注, 正在关注 }

    /// <summary>
    /// 分组类型
    /// </summary>
    public enum channel_idType { 关注用户分组 = 1, 关注用户标签 = 2, 素材分组 = 3 }

    /// <summary>
    /// 是否公用
    /// </summary>
    public enum is_publicMode { 否, 是 }

    /// <summary>
    /// 微信消息类型
    /// </summary>
    public enum msg_type { 文本 = 1, 单图文 = 2, 图片 = 3, 语音 = 4, 视频 = 5, 多图文 = 6, 外链 = 7, 授权 = 8, 模块 = 9, 多客服 = 10 }


    /// <summary>
    /// 多媒体消息类型
    /// </summary>
    public enum media_type { 素材图片库 = 1, 素材语音 = 2, 素材视频 = 3, 图文模板图片库 = 4, _360全景图片 = 5, 销售头像图片 = 6, 项目模块 = 7, 公众号头像 = 8, 站内信图片 = 9, 消息订阅图片 = 10, 帮助中心图片 = 11, 刮刮卡图片 = 12, 微邀请图片 = 13, 微邀请音频 = 14, 微相册图片 = 15, 手机站广告位图片=16 }

    /// <summary>
    /// 是否及时发送
    /// </summary>
    public enum is_sendMode { 否, 是,已删除=999 }

    /// <summary>
    /// 是否定时发送
    /// </summary>
    public enum is_timerMode { 否, 是 }

    /// <summary>
    /// 关键字匹配类型
    /// </summary>
    public enum eq_type { 完全匹配 = 1, 包含匹配 = 2 }

    /// <summary>
    /// 图文信息类型
    /// </summary>
    public enum ResponseNewsType { 单图文 = 1, 多图文 = 2 }

    /// <summary>
    /// 视频信息类型
    /// </summary>
    public enum video_type { 本地视频 = 1, 微视 = 2 }

    /// <summary>
    /// 模板适用类型
    /// </summary>
    public enum TemplateType { 微信 = 1, 微网站 = 2, 项目 = 3, 微邀请 = 4 }

    /// <summary>
    ///  模板分类
    /// </summary>
    public enum TemplateCategoryType { 自定义图文模版 = -1, 系统图文模版 = 0, 微网站首页 = 1, 微网站列表页 = 2, 微网站详细页 = 3 }

    /// <summary>
    /// 分组类别（标签,分组）
    /// </summary>
    public enum GroupTagType { 分组 = 1, 标签 = 2 }

    /// <summary>
    /// 微信错误编码
    /// </summary>
    public enum WeChatErrorCode { 涉嫌广告 = 10001, 涉嫌政治 = 20001, 涉嫌社会 = 20004, 涉嫌色情 = 20002, 涉嫌违法犯罪 = 20006, 涉嫌欺诈 = 20008, 涉嫌版权 = 20013, 涉嫌互推 = 22000, 涉嫌其他 = 21000 }

    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BussinessType { CLC, CMA, AScott }

    /// <summary>
    /// 地区类型
    /// </summary>
    public enum AreaType { 华东, 华北, 华南, 西南, 华西,东北,华中 }

    /// <summary>
    /// 微信，fans表对应的性别
    /// </summary>
    public enum WeChatSex { 未知, 男, 女 }

    /// <summary>
    /// 数据库
    /// </summary>
    public enum DbDataBaseEnum
    {
        /// <summary>
        /// 操作日志库
        /// </summary>
        KD_LOGS = 1,
        /// <summary>
        /// 用户信息库
        /// </summary>
        KD_USERS = 2,
        /// <summary>
        /// 微信架构库
        /// </summary>
        KD_WECHATS = 3,
        /// <summary>
        /// 模块库
        /// </summary>
        KD_MODULES = 4,
        /// <summary>
        /// 统计库
        /// </summary>
        KD_STATISTICS = 5,

        /// <summary>
        /// 手机站库
        /// </summary>
        KD_MOBILE = 6
    }
    /// <summary>
    /// 关注与无匹配回复时的类型
    /// </summary>
    public enum AutoReply { 关注时 = 1, 无匹配时 = 2 }
    /// <summary>
    /// 上传的类型(已不用)
    /// </summary>
    public enum UploadType { 图片 = 1, 语音 = 2, 视频 = 3, 其他 = 4 }

    /// <summary>
    /// userList包含筛选
    /// </summary>
    public enum MsgContainType { 手机, 姓名, 身份证 }

    /// <summary>
    /// log类型
    /// </summary>
    public enum LogType { 添加 = 1, 删除 = 2, 修改 = 3 }
    /// <summary>
    /// 聊天信息的发送者
    /// </summary>
    public enum FromUserType { 用户 = 1, 公众号 = 2 }
    /// <summary>
    /// 粉丝聊天的状态
    /// </summary>
    public enum FansChatStatus { 暂无 = 4, 未回复 = 1, 已回复 = 2, 已过期 = 3 }
    /// <summary>
    /// 会员类型
    /// </summary>
    public enum MemberType { 注册用户, 非注册用户 }
    /// <summary>
    /// 站内信
    /// </summary>
    public enum LetterType { 未读 = 0, 已读 = 1, 删除 = 2 }
    /// <summary>
    /// 模块分类
    /// </summary>
    public enum ModuleType { 咨询模块 = 1, 活动模块 = 2, 业务模块 = 3, 其他 = 4 }
    /// <summary>
    /// 模块类型
    /// </summary>
    public enum ModuleMode { 第三方模块, 系统模块 }
    /// <summary>
    /// 统计类型
    /// </summary>
    public enum StatisticsType { 关注用户, 消息 }
    /// <summary>
    /// 粉丝客服状态
    /// </summary>
    public enum FansState { 选择项目状态, 客服聊天状态, 自动回复状态,手机注册发送验证码状态,上传小票状态 }
    /// <summary>
    /// 销售关系中的聊天已读状态
    /// </summary>
    public enum SaleChatIsReadType { 已读, 未读 }
    /// <summary>
    /// 销售关系中的聊天接收状态
    /// </summary>
    public enum SaleChatSendType { 用户接收, 用户发送 }
    /// <summary>
    /// 销售人员是否为经理
    /// </summary>
    public enum SaleIsManager { 否, 是 }

    /// <summary>
    /// 销售聊天状态
    /// </summary>
    public enum SaleChatsMode { 所有状态, 已回复, 未回复, 已过期, 未回复已过期 }
    /// <summary>
    /// 销售
    /// </summary>
    public enum SaleUserStatus { 禁用, 正常, 休假 }
    /// <summary>
    /// 页面记录类型
    /// </summary>
    public enum HistoryViewType { 浏览数 = 1, 点赞数 = 2, 分享数 = 3 }

    /// <summary>
    /// fans表回复状态
    /// </summary>
    public enum FansReplyState { 暂无, 已回复, 未回复 }
    /// <summary>
    /// 新的聊天状态，用于user_LIST
    /// </summary>
    public enum FansChatsTypeNew { 暂无, 未回复, 已回复, 已过期, 未回复已过期 }

    /// <summary>
    /// 系统标签表
    /// </summary>
    public enum SysTag { 粉丝兴趣爱好 = 1, 项目地区 = 2, 项目城市 = 3, 项目类型 = 4, 项目状态 = 5, 项目户型 = 6, 项目价格 = 7, 项目系列 = 8 }

    /// <summary>
    /// 项目内容栏目
    /// </summary>
    public enum ProjectContentType { 首页背景图 = 0, 微官网 = 1, 项目介绍 = 2, 地理位置 = 3, 交通配套 = 4, 预约看房 = 5, 联系热线 = 6, 项目图片 = 7, 项目全景 = 8, 户型图 = 9, 滑动图片 = 10, 技术参数 = 11, 招租信息 = 12, 最新活动 = 13, 关注二维码 = 14 }

    /// <summary>
    /// 微邀请内容栏目
    /// </summary>
    public enum InviteContentType { 活动介绍 = 1, 活动时程 = 2, 活动相册 = 3, 嘉宾介绍 = 4, 活动视频 = 5, 活动回函 = 6, 活动报名 = 7, 活动地图 = 8 }

    public enum Prize_type { 刮刮卡 = 1, 砸金蛋 = 2, 大转盘 = 3 }

    /// <summary>
    /// 权限动作
    /// </summary>
    public enum RoleActionType
    {
        /// <summary>
        /// 查看权限
        /// </summary>
        View,
        /// <summary>
        /// 添加权限
        /// </summary>
        Add,
        /// <summary>
        /// 修改权限
        /// </summary>
        Edit,
        /// <summary>
        /// 删除权限
        /// </summary>
        Delete,
        /// <summary>
        /// 导入权限
        /// </summary>
        Import,
        /// <summary>
        /// 导出权限
        /// </summary>
        Export,
        /// <summary>
        /// 发布权限
        /// </summary>
        Release,
        /// <summary>
        /// 回复权限
        /// </summary>
        Reply,

        /// <summary>
        /// 审核权限
        /// </summary>
        Aduit,
        /// <summary>
        /// 管理权限
        /// </summary>
        Manage

    }
    /// <summary>
    /// 二维码类别
    /// </summary>
    public enum QrCodeType { 项目用, 拓客用 }
    /// <summary>
    /// 销售粉丝类型
    /// </summary>
    public enum SaleFansType { 微信粉丝, 临时粉丝 }


    /// <summary>
    /// mongoDB数据库库名
    /// </summary>
    public enum MongoDBName
    {
        /// <summary>
        /// 网站日志库
        /// </summary>
        kd_logs,
        /// <summary>
        /// 图文统计库
        /// </summary>
        kd_graphic
    }

    public enum GroupMsgCheckMode { 未审核, 已审核 }

    /// <summary>
    /// 项目详细模板
    /// </summary>
    public enum ProjectDetaileTemplate { 项目简洁模版, 项目高级模版, 项目滑动模板 }

    /// <summary>
    /// 项目详细模板
    /// </summary>
    public enum ProjectDetaileTemplateNew { 项目简介详细模版, 项目高级详细模版, 图片滑动详细模板, 办公楼详细模板, 购物中心详细模板, 住宅房产详细模板 }

    /// <summary>
    /// 分类频道
    /// </summary>
    public enum CategoryChannel { 帮助中心 = 1, 项目类型 = 2, 项目城市地区 = 3, 项目状态 = 4, 项目价格 = 5, 项目户型 = 6, 项目系列 = 7 }

}
