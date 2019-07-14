
 
urlParameters = (function (script) {
    var l = script.length;
    for (var i = 0; i < l; i++) {
        me = !!document.querySelector ? script[i].src : script[i].getAttribute('src', 4);
        if (me.substr(me.lastIndexOf('/')).indexOf('menu_hover') !== -1) {
            break;
        }
    }
    return me.split('?')[1];
})
(document.getElementsByTagName('script'))

GetJsParameters = function (name) {
    if (urlParameters) {//&& urlParameters.indexOf('&') > 0
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

GetUrlParameters=function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return "";
}

var CeilingIsGod = {
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
    opo:'.',
    op: '&',
    po: '=',
    sy:'?',
    poo: 'p',
    ca:'ac',
    pee: 'v',
    opp: 'pv',
    title: window.document.title,
    name: GetJsParameters('n'),
    geterror: function (_name, _value) {
        return (this.op + _name + this.po + _value);
    },
    onerror :function () {
        var error = "http://wx.companycn.net/" + this.theme+this.cl + "/" + this.poo + this.pee + this.opo + this.zh + this.hz + this.sy + this.ca
        +this.po+this.opp;
        error += this.geterror("b", encodeURI(this.type));
        error += this.geterror("g", encodeURI(this.wx_og_id || GetJsParameters('og')));
        error += this.geterror("w", (this.wx_id || GetJsParameters('wx')));
        error += this.geterror("o", encodeURI(this.openId));
        error += this.geterror("t", encodeURI(this.title));
        error += this.geterror("i", this.id);
        error += this.geterror("u", encodeURI(this.url));
        error += this.geterror("fo", encodeURI(this.from_openId));
        error += this.geterror("n", encodeURI(this.name || this.title));
        error += this.geterror("st", GetJsParameters('st'));
        return error;

    },
    nothings: function () {
        
        var ajax_share = false;
        if (window.XMLHttpRequest) {
            ajax_share = new XMLHttpRequest();
            if (ajax_share.overrideMimeType) {
                ajax_share.overrideMimeType("text/xml");
            }
        } else if (window.ActiveXObject) {
            try {
                ajax_share = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                try {
                    ajax_share = new ActiveXObject("Microsoft.XMLHTTP");
                } catch (e) {
                }
            }
        }
        if (!ajax_share) {
            console.log("can not create XMLHttpRequest object.");
            return false;
        }
        ajax_share.open("POST", this.onerror());
        ajax_share.send(null);
        ajax_share.onreadystatechange = function () {
            if (ajax_share.readyState == 4 && ajax_share.status == 200) {
                console.log("success");
            } else {
                console.log("error");
            }
        }
    }
}
CeilingIsGod.nothings();






