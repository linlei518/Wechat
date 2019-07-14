window.WeixinShare = function (config) {
    if (!config) config = {};
    this.appId = config.appId;
    this.timestamp = config.timestamp;
    this.nonceStr = config.nonceStr;
    this.jsapiTicket = config.jsapiTicket;
    this.signature = config.signature;
    this.imgUrl = config.imgUrl || (document.getElementsByTagName('img')[0].src || '');
    this.linkUrl = config.linkUrl || window.location.href;		//分享的网址链接
    this.title = config.title || window.document.title;		//分享的标题
    this.content = config.content || this.title;
    this.debug = config.debug || false;
    var _jsApiList=[
              'onMenuShareTimeline',
              'onMenuShareAppMessage',
              'onMenuShareQQ',
              'onMenuShareWeibo' 
    ];
    this.jsApiList = config.jsApiList || _jsApiList;
    this.shareAppMessageSuccessCallback = config.shareAppMessageSuccessCallback || function () { };  //分享给朋友成功回调函数
    this.shareTimelineSuccessCallback = config.shareTimelineSuccessCallback || function () { };      //分享到朋友圈成功回调函数
    this.shareQQSuccessCallback = config.shareQQSuccessCallback || function () { };                 //分享到QQ成功回调函数
    this.shareWeiboSuccessCallback = config.shareWeiboSuccessCallback || function () { };                 //分享到微博成功回调函数
    this.getLocationSuccessCallback = config.getLocationSuccessCallback || function () { };                 // 获取坐标成功回调函数
    this.getLocationErrorCallback = config.getLocationErrorCallback || function () { };                 // 获取坐标失败回调函数
    this.setup();
}

WeixinShare.prototype = {
    setup: function () {
       
        _this = this;
        // alert(_this.debug);
        wx.config({
            debug: this.debug,
            appId: this.appId,
            timestamp: this.timestamp,
            nonceStr: this.nonceStr,
            signature: this.signature,
            jsApiList: this.jsApiList
        });
        wx.ready(function () {

           

            //分享给朋友
            wx.onMenuShareAppMessage({  
                title: _this.title,
                desc: _this.content,
                link: _this.linkUrl,
                imgUrl: _this.imgUrl,
                trigger: function (res) {
                },
                success: function (res) {
                   
                    _this.shareAppMessageSuccessCallback();
                },
                cancel: function (res) {

                },
                fail: function (res) {
                    alert(JSON.stringify(res));
                }
            });
            //分享到朋友圈
            wx.onMenuShareTimeline({  
                title: _this.title,
                link: _this.linkUrl,
                imgUrl: _this.imgUrl,
                trigger: function (res) {
                },
                success: function (res) {
                   
                    _this.shareTimelineSuccessCallback();
                },
                cancel: function (res) {
                    
                },
                fail: function (res) {
                    alert(JSON.stringify(res));
                }
            });

            //分享到QQ
            wx.onMenuShareQQ({
                title: _this.title,
                desc: _this.content,
                link: _this.linkUrl,
                imgUrl: _this.imgUrl,
                trigger: function (res) {
                },
                complete: function (res) {
                    //alert(JSON.stringify(res));
                },
                success: function (res) {
                    _this.shareQQSuccessCallback();
                },
                cancel: function (res) {
                },
                fail: function (res) {
                    alert(JSON.stringify(res));
                }
            });
            //分享到微博
            wx.onMenuShareWeibo({
                title: _this.title,
                desc: _this.content,
                link: _this.linkUrl,
                imgUrl: _this.imgUrl,
                trigger: function (res) {
                },
                complete: function (res) {
                    //alert(JSON.stringify(res));
                },
                success: function (res) {
                    _this.shareWeiboSuccessCallback();
                },
                cancel: function (res) {
                },
                fail: function (res) {
                    alert(JSON.stringify(res));
                }
            });
            //获取地址
            wx.getLocation({
                success: function (res) {
                   
                    _this.getLocationSuccessCallback(res);
                   
                },
                cancel: function (res) {
                    _this.getLocationErrorCallback();
                }
            });
        })
    }
}


