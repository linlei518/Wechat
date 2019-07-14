window.WeixinShare = function (config) {
    if (!config) config = {};

    this.imgConfigUrl = config.imgUrl || document.getElementsByTagName('img')[0].src || '';
    this.linkURL = config.url || window.location.href;		//分享的网址链接
    this.title = config.title || window.document.title;		//分享的标题
    this.desc = config.content || this.title;
    this.showControl = config.showControl || false;

    this.readyToShare = false;
    this.readyBridge = false;
  
    this.shareCallback = config.callback || function () { };
    this.canShare = (typeof config.canShare !== 'undefined') ? config.canShare : true;
    this.noShareFun = config.noShare || function () { };
    this.noWeixin = config.noWeixin || false;
    this.setup();
}

WeixinShare.prototype = {
    setup: function () {
        this.isWeixin();
        this.bindLoadEvents();
    },
    isWeixin: function () {
        var ua = navigator.userAgent.toLowerCase();
        var flag = false;
        if (ua.match(/MicroMessenger/i) == "micromessenger") {
            flag = true;
        }
        if (!flag) {
          //  this.noWeixin();
        }
    },
    bindLoadEvents: function () {
        var _this = this;
        if (document.addEventListener) {
            document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
        } else if (document.attachEvent) {
            document.attachEvent('WeixinJSBridgeReady', onBridgeReady);
            document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
        }
        if (this.imgConfigUrl != '') {
            var tempImg = new Image();

            tempImg.onload = function () {
                tempImg.onload = null;
                tempImg.onerror = null;
                _this.readyToShare = true;
                if (_this.readyBridge) {
                    _this.shareInitOk();
                }
            }
            tempImg.onerror = function () {
                tempImg.onload = null;
                tempImg.onerror = null;
                _this.readyToShare = true;
                if (_this.readyBridge) {
                    _this.shareInitOk();
                }
            }

            tempImg.src = this.imgConfigUrl;
        }
        function onBridgeReady() {
            _this.readyBridge = true;
            if (_this.readyToShare) {
                _this.shareInitOk();
            }
        }

    },
    shareInitOk: function () {
        this.imgUrl = this.imgConfigUrl;	//分享的图片链接
        this.linkURL = this.checkUrl(this.linkURL);		//分享的网址链接
        var _this = this;



        if (this.imgUrl.toLowerCase() == 'none') {
            this.imgUrl = '';
        } else {
            this.imgUrl = this.checkUrl(this.imgUrl);
        }

        // 发送给好友; 
        WeixinJSBridge.on('menu:share:appmessage', function (argv) {
            if (!_this.canShare) {
                _this.noShareFun();
                return false;
            }
            _this.shareCallback();
            WeixinJSBridge.invoke('sendAppMessage', {
                "img_url": _this.imgUrl,
                "link": _this.linkURL,
                "desc": _this.desc,
                "title": _this.title
            }, function (res) {

                WeixinJSBridge.log(res.err_msg);
            });
        });
        // 分享到朋友圈;
        WeixinJSBridge.on('menu:share:timeline', function (argv) {
            if (!_this.canShare) {
                _this.noShareFun();
                return false;
            }
            _this.shareCallback();
            WeixinJSBridge.invoke('shareTimeline', {
                "img_url": _this.imgUrl,
                "link": _this.linkURL,
                "desc": _this.desc,
                "title": _this.title
            }, function (res) {
                WeixinJSBridge.log(res.err_msg);
            });
        });

        // 分享到微博;
        var weiboContent = '';
        WeixinJSBridge.on('menu:share:weibo', function (argv) {
            if (!_this.canShare) {
                _this.noShareFun();
                return false;
            }
            _this.shareCallback();
            WeixinJSBridge.invoke('shareWeibo', {
                "img_url": _this.imgUrl,
                "content": (_this.title == _this.desc) ? _this.title : _this.title + ' ' + _this.desc,
                "url": _this.linkURL
            }, function (res) {
                WeixinJSBridge.log(res.err_msg);
            });
        });

        // 隐藏右上角的选项菜单入口;
        if (!_this.showControl) {
            WeixinJSBridge.call('hideToolbar');
        }
    },
    checkUrl: function (linkU) {
        if (!linkU) return false;
        var tempUrl = '';
        var urlStrs = window.location.href.replace('http://', '').split('/');
        var url = urlStrs[0];
        if (linkU.indexOf('http://') != -1) {
            tempUrl = linkU;
        } else if (linkU.indexOf(url) != -1) {
            tempUrl = 'http://' + linkU;
        } else {
            if (linkU.indexOf('../') != -1) {
                var k = 0
                while (linkU.indexOf('../') != -1) {
                    linkU = linkU.replace('../', '');
                    k++;
                }
                tempUrl = 'http://' + url + '/';
                for (var i = 1; i < urlStrs.length - k - 1; i++) {
                    tempUrl += urlStrs[i] + '/';
                }
                tempUrl += linkU;
            } else if (linkU.indexOf('./') != -1 || linkU[0] != '/') {
                while (linkU.indexOf('./') != -1) {
                    linkU = linkU.replace('./', '');
                }
                tempUrl = 'http://' + url + '/';
                for (var i = 1; i < urlStrs.length - 1; i++) {
                    tempUrl += urlStrs[i] + '/';
                }
                tempUrl += linkU;
            } else {
                tempUrl = 'http://' + url + '/' + linkU;
            }
        }
        return tempUrl;
    },
    changeShareUrl: function (url) {
        this.linkURL = this.checkUrl(url);
    },
    stopShare: function () {
        this.canShare = false;
    },
    beginShare: function () {
        this.canShare = true;
    }
}