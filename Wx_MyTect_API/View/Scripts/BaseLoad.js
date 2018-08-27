$(function ()
{


})

//写入cookies
function setCookie(name, value) {
    var Days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

//读取cookies
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

    if (arr = document.cookie.match(reg)) return unescape(arr[2]);
    else return null;
}


//要引入Jquery，这样$符号才有定义 
var Js_API = new Object;

/*获取url参数值*/
Js_API.GetQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

/*删除url某参数值*/
Js_API.funcUrlDel = function (name) {
    var loca = window.location;
    var baseUrl = loca.origin + loca.pathname + "?";
    var query = loca.search.substr(1);
    if (query.indexOf(name) > -1) {
        var obj = {}
        var arr = query.split("&");
        for (var i = 0; i < arr.length; i++) {
            arr[i] = arr[i].split("=");
            obj[arr[i][0]] = arr[i][1];
        };
        delete obj[name];
        var url = baseUrl + JSON.stringify(obj).replace(/[\"\{\}]/g, "").replace(/\:/g, "=").replace(/\,/g, "&");
        return url;
    };
}

/*Get提交*/
Js_API.Get = function (url, data, success, error, async, cache) {
    ajax(url, success, error, data, 'GET', false, async, 'json', cache);
}

/*post提交*/
Js_API.Post = function(url, data, success, error, async, cache) {
    ajax(url, success, error, data, 'POST', false, async, 'json', cache);
}







var ajaxStatus = true;
window.onload = function () {
    //设置ajax当前状态(是否可以发送);
    ajaxStatus = true;
};
// ajax封装
function ajax(url, success, error, data, type, alone, async, dataType, cache) {
    var success = success || function (data) {
        /*console.log('请求成功');*/
        setTimeout(function () {
            //进行提示信息
        }, 500);
        if (data.status) {//服务器处理成功
            setTimeout(function () {
                if (data.url) {
                    location.replace(data.url);
                } else {
                    location.reload(true);
                }
            }, 1500);
        } else {//服务器处理失败
           // if (alone) {//改变ajax提交状态
                ajaxStatus = true;
            //}
        }
    };
    var error = error || function (data) {
        /*console.error('请求成功失败');*/
        /*data.status;//错误状态吗*/

        setTimeout(function () {
            if (data.status == 404) {
                console.info('请求失败，请求未找到');
            } else if (data.status == 503) {
                console.info('请求失败，服务器内部错误');
            } else {
                console.info('请求失败,网络连接超时');
            }
            ajaxStatus = true;
        }, 500);
    };
    var data = data || null;
    var type = type || 'GET';//请求类型
    var alone = alone || false;//独立提交（一次有效的提交）
    var async = async || true;//异步请求
    var dataType = dataType || 'json';//接收数据类型
    var cache = cache || false;//浏览器历史缓存

    //添加时间搓，解决浏览器的缓存问题
    if (cache) {
        url += "?nowtime=" + new Date().getTime();
    }

    /*判断是否可以发送请求*/
    if (!ajaxStatus) {
        return false;
    }
    ajaxStatus = false;//禁用ajax请求
    /*正常情况下1秒后可以再次多个异步请求，为true时只可以有一次有效请求（例如添加数据）*/
    if (!alone) {
        setTimeout(function () {
            ajaxStatus = true;
        },1000); //这里设置2秒后可以再次多个异步请求
    }
    
    $.ajax({
        'url': url,
        'data': data,
        'type': type,
        'dataType': dataType,
        'async': async,
        'success': success,
        'error': error,
        'jsonpCallback': 'jsonp' + (new Date()).valueOf().toString().substr(-4),
        'beforeSend': function () {
            //设置加载中的样式
            //console.info('请求开始：设置加载中样式');

        },
    });
}