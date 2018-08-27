var $;
layui.config({
    base: "/Scripts/Sys/"
}).use(['layer', 'jquery'], function () {
    layer = layui.layer,
        $ = layui.jquery;
    $('#btn_login').click(function () {
        var username = $('#username').val();
        var password = $('#password').val();
        var data = "{'UserName':" + username + ",'Password':" + password + "}";
        Js_API.Post('/SysLogin/LoginInApi', data, function (result) {
            if (result.success) {
                window.location.href = result.msg;
            } else {
                alert(result.msg);
                $('#password').val("");
            }
        });
        return false;
    });
});
