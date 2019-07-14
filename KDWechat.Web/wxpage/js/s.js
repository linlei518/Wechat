

urlParameters = (function (script) {
    var l = script.length;
    for (var i = 0; i < l; i++) {
        me = !!document.querySelector ? script[i].src : script[i].getAttribute('src', 4);
        if (me.substr(me.lastIndexOf('/')).indexOf('menu_hover') !== -1) {
            break;
        }
    }
    return me.split('?')[1];
})(document.getElementsByTagName('script'))
urlAddress = (function (script) {
    var l = script.length;
    for (var i = 0; i < l; i++) {
        me = !!document.querySelector ? script[i].src : script[i].getAttribute('src', 4);
        if (me.substr(me.lastIndexOf('/')).indexOf('menu_hover') !== -1) {
            break;
        }
    }
    return me;
})(document.getElementsByTagName('script'))

Protocol = function () {
    if (urlAddress) {
        var _kdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        var temp = urlAddress.substr(me.indexOf('/') + 2);
        return _kdhmProtocol+temp.substr(0,temp.indexOf('/'));
    } else {
        return "";
    }
}


GetJsParameters = function (name) {
    if (urlParameters) { 
        var parame = urlParameters.split('&'), i = 0, l = parame.length, arr;
        for (var i = 0 ; i < l; i++) {
            arr = parame[i].split('=');
            if (name === arr[0]) {
                return arr[1];
            }
        }
    }
    return "";
}

 

GetUrlParameters = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return "";
}

var CeilingIsGodShare = {
    theme: "han",
    type: GetJsParameters('t'),
    wx_og_id: GetUrlParameters('wx_og_id'),
    wx_id: GetUrlParameters('wx_id'),
    openId: GetUrlParameters('openId'),
    from_openId: GetUrlParameters('from_openId'),
    id: GetUrlParameters('id'),
    url: window.location.href,
    hz: 'hx',
    zh: 'as',
    cl: 'dles',
    opo: '.',
    op: '&',
    po: '=',
    sy: '?',
    poo: 'p',
    ca: 'ac',
    pee: 'v',
    opp: 'sh',
    title: window.document.title,
    name: GetJsParameters('n'),
    insertlog: function (_name, _value) {
        return (this.op + _name + this.po + _value);
    },
    onerror: function () {
        var error = Protocol()+"/" + this.theme + this.cl + "/" + this.poo + this.pee + this.opo + this.zh + this.hz + this.sy + this.ca
        + this.po + this.opp;
        error += this.insertlog("b", encodeURI(this.type));
        error += this.insertlog("g", encodeURI(this.wx_og_id || GetJsParameters('og')));
        error += this.insertlog("w", (this.wx_id || GetJsParameters('wx')));
        error += this.insertlog("o", encodeURI(this.openId));
        error += this.insertlog("t", encodeURI(this.title));
        error += this.insertlog("i", (this.id || GetUrlParameters('reserve_id')));
        error += this.insertlog("u", encodeURI(this.url.replace("&", "|")));
        error += this.insertlog("fo", encodeURI(this.from_openId));
        error += this.insertlog("n", encodeURI(this.name || this.title));
        error += this.insertlog("st", encodeURI(GetJsParameters('st')));
        return error;

    },
    nothings: function () {
         
        var bgnsm = false;
        if (window.XMLHttpRequest) {
            bgnsm = new XMLHttpRequest();
            if (bgnsm.overrideMimeType) {
                bgnsm.overrideMimeType("text/xml");
            }
        } else if (window.ActiveXObject) {
            try {
                bgnsm = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                try {
                    bgnsm = new ActiveXObject("Microsoft.XMLHTTP");
                } catch (e) {
                }
            }
        }
        if (!bgnsm) {
            console.log("can not create XMLHttpRequest object.");
            return false;
        }
        bgnsm.open("POST", this.onerror());
        bgnsm.send(null);
        bgnsm.onreadystatechange = function () {
            if (bgnsm.readyState == 4 && bgnsm.status == 200) {
                console.log("success");
            } else {
                console.log("error");
            }
        }
    }
}
GetFristImg = function () {
    if (document.getElementsByTagName('img').length > 0) {
        return document.getElementsByTagName('img')[0].src;
    } else {
        return "http://" + window.location.host + "/images/pic_header_01.jpg";
    }
}


//var _protocol = (("https:" == document.location.protocol) ? "https://" : "http://");
//var img = document.getElementsByTagName('img')[0].src;
//if (typeof (img) == "undefined") {
//    img = _protocol + location.host + "/images/logo_01.png";
//} 
//var weixinShare = new WeixinShare({
//    title: window.document.title,
//    content: decodeURI(GetJsParameters('c')),
//    linkUrl: CeilingIsGodShare.url.replace("openId", "from_openId").replace("from_openId", "none"),
//    imgUrl: img,
//    appId: GetJsParameters("app_id"),
//    debug: false,
//    timestamp:  GetJsParameters("timestamp"),
//    nonceStr:  GetJsParameters("nonceStr"),
//    signature: GetJsParameters("signature"),
//    shareAppMessageSuccessCallback: function () {
//        CeilingIsGodShare.nothings();
//    },
//    shareTimelineSuccessCallback: function () {
//        CeilingIsGodShare.nothings();
//    },
//    shareQQSuccessCallback: function () {
//        CeilingIsGodShare.nothings();
//    },
//    shareWeiboSuccessCallback: function () {
//        CeilingIsGodShare.nothings();
//    }

//});
 
 
//var weixinShare = new WeixinShare({
//    title: CeilingIsGodShare.title,
//    content: decodeURI(GetJsParameters('c')) ,
//    url: CeilingIsGodShare.url.replace("openId", "from_openId").replace("from_openId", "none"),
//    imgUrl: GetFristImg(),
//    showControl: false,
//    callback: function () {
//        CeilingIsGodShare.nothings();
//    }
//});









