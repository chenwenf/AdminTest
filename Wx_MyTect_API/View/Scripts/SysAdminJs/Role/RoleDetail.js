var $;
layui.use(['form', 'layer',], function () {
    var form = layui.form;
    var layer = parent.layer === undefined ? layui.layer : parent.layer;
    $ = layui.jquery;
    var index = parent.layer.getFrameIndex(window.name);

    $(function () {
        var id = Js_API.GetQueryString("id");
        var statu = Js_API.GetQueryString("statu");

        if (statu === "2") {
            $('#div1').show();
            var reqData = { "Id": id };
            Js_API.Post('/SysRoleManager/GetRoleOne', reqData, function (rdata) {
                getList(rdata);
                form.render('checkbox');
            });
        }
        else {
            //添加
            $('#div1').hide();
            $('#add').show();
        }

    });

    //监听提交
    form.on('submit(update)', function () {
        var form = $('#form').serialize();
        var limits = [];
        $("input:checkbox[name='limits']:checked").each(function () { // 遍历name=standard的多选框
            limits.push($(this).val());
        });
        Js_API.Post('/SysRoleManager/AddOrUpdRole', form + '&Ids=' + limits.join(','), function (data) {
            alert(data.msg)
            parent.layer.close(index);
            window.parent.location.reload();
        });
    });

    //监听提交
    form.on('submit(add)', function () {
        var form = $('#form').serialize();
        Js_API.Post('/SysRoleManager/AddOrUpdRole', form, function (data) {
            alert(data.msg)
            parent.layer.close(index);
            window.parent.location.reload();
        });
    });


    function getList(data) {
        $("#Id").val(data.Id);
        $("#txtName").val(data.Name);
        $("#Remark").val(data.Remark);
        $("#div_Role").html("");
        var roleDiv = '';
        if (data.managerList) {
            var list = data.managerList;
            for (var i = 0; i < list.length; i++) {
                roleDiv += '<input type=\'checkbox\' name=\'limits\' value=\'' + list[i].Id + '\'  lay-skin=\'primary\'  title=\'' + list[i].Title + '\'';
                if (list[i].IsCheck === 1) {
                    roleDiv += ' Checked  '
                }
                roleDiv += '/>'
            }
            $("#div_Role").html(roleDiv);

        }

    }

});