var $;
layui.use(['form', 'layer',], function () {
    var form = layui.form;
    var layer = parent.layer === undefined ? layui.layer : parent.layer;
    $ = layui.jquery;
    var index = parent.layer.getFrameIndex(window.name);

    $(function () {
        var id = Js_API.GetQueryString("id");
        var statu = Js_API.GetQueryString("statu");
        Js_API.Post('/SysRoleManager/GetParentLinkList', { "parentId": 0 }, function (data) {
            getParentList(data);
            form.render('select');
            if (statu === "2") {
                $('#div1').show();
                var reqData = { "Id": id };
                Js_API.Post('/SysRoleManager/GetRoleLinkOne', reqData, function (rdata) {
                    getList(rdata);
                    //重新渲染下拉框
                    form.render('select');
                });
            }
            else {
                //添加
                $('#div1').hide();
                $('#add').show();
            }
        });

        $('#Icon').on('change', function () {
            $("#Icon_1").html($("#Icon").val());
        })

    });

    //监听提交
    form.on('submit(update)', function () {
        var form = $('#form').serialize();
        Js_API.Post('/SysRoleManager/AddOrUpdRoleLink', form, function (data) {
            alert("保存成功...");
            parent.layer.close(index);
            window.parent.location.reload();
        });
    });

    //监听提交
    form.on('submit(add)', function () {
        var form = $('#form').serialize();
        Js_API.Post('/SysRoleManager/AddOrUpdRoleLink', form, function (data) {
            alert("保存成功...");
            parent.layer.close(index);
            window.parent.location.reload();
        });
    });

    function getParentList(data) {
        $("#ParentId").html("")
        var ParentId = '<option value="0">顶级菜单</option>';
        if (data.length > 1) {
            for (var i = 0; i < data.length; i++) {
                ParentId += '<option value="' + data[i].Id + '">' + data[i].Title + '</option>';
            }
        }
        else {
            ParentId += '<option value="' + data.Id + '">' + data.Title + '</option>';
        }

        $("#ParentId").html(ParentId)
    }

    function getList(data) {
        $("#Id").val(data.Id);
        $("#txtTitle").val(data.Title);
        $("#Href").val(data.Href);
        $("#Icon").val(data.Icon);
        $("#Icon_1").html(data.Icon);
        $("#Type").val(data.Type);
        $("#CssId").val(data.CssId);
        $("#ParentId").val(data.ParentId);
    }
    
});