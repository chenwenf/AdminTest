layui.use('table', function () {
    var table = layui.table
        , form = layui.form;

    table.render({
        elem: "#tableData"
        , url: '/SysRoleManager/GetRoleLink'
        , page: true
        , cellMinWidth: 100
        , cols: [[{ type: "checkbox" }
            , { width: 80, field: "Title", title: "名称" }
            , {
            width: 80, field: "Icon", title: "标签", templet: function (d) {
                return '<span class="iconfont">' + d.Icon + '</span>';
            }
        }
            , { field: "Href", title: "路径" }
            , {
            width: 80, field: "Type", title: "类型", templet: function (d) {
                if (d.Type === 0) {
                    var n1 = '菜单';
                    return '<span style="color:#FFB800;">' + n1 + '</span>';
                }
                var n2 = '按钮';
                return '<span style="color:#1E9FFF;">' + n2 + '</span>';
            }
        }
            , { width: 80, field: "CssId", title: "样式名称/编号" }
            , { field: "CreateTime", title: "创建时间" }
            , { width: 200, title: "操作", align: "left", toolbar: "#barUser" }
        ]],
        id: "tableDataDT"
    });

    //删除
    function deleteAjax(ids) {
        if (ids.length <= 0) {
            layer.alert("请选择要删除的数据", { icon: 0, title: "提示信息" });
            return;
        }
        layer.confirm("确定删除选中的信息？", { icon: 3, title: "提示信息" }, function (index) {
            index = layer.msg("删除中，请稍候", { icon: 16, time: 60000, shade: 0.8 });
            var data = { "ids": ids.join(',') }

            Js_API.Post('/SysRoleManager/DelRoleLink', data, function (data) {
                layer.close(index);
                if (data.success) {
                    table.reload("tableDataDT");
                } else {
                    layer.alert(data.msg);
                }
            });
        });
    }
    table.on("tool(tableData)", function (obj) {
        var data = obj.data, layEvent = obj.event;
        var Id = data.Id;
        var index;
        //删除
        if (layEvent === "del") {
            var ids = [];
            ids.push(data.Id);
            deleteAjax(ids);
        }
        else if (layEvent === "edit") {
            index = layui.layer.open({
                title: "编辑用户",
                type: 2,
                content: "RoleLinkDetail?statu=2&id=" + data.Id,
                success: function (layero, index) {
                    var body = layer.getChildFrame("body", index);
                }
            });
            layui.layer.full(index);
        }
    });

    var $ = layui.$, active = {
        reload: function () {
            var txtTitle = $("#txtTitle");
            //查看
            table.reload("tableDataDT", {
                page: {
                    curr: 1
                },
                where: {
                    title: txtTitle.val()
                }
            });
        },
        add: function () {
            var index;
            index = layui.layer.open({
                title: "添加",
                type: 2,
                content: "RoleLinkDetail",
                success: function (layero, index) {
                    var body = layer.getChildFrame("body", index);
                }
            });
            layui.layer.full(index);
        },
        //多行删除
        getCheckData: function () {
            var checkStatus = table.checkStatus("tableDataDT");
            var data = checkStatus.data;
            var ids = [];
            for (var i in data) {
                if (data.hasOwnProperty(i)) {
                    ids.push(data[i].Id);
                }
            }
            deleteAjax(ids);
        }
    };
    $("#searchbtn,#Add,#batchdel").on("click", function () {
        var type = $(this).data("type");
        active[type] ? active[type].call(this) : "";
    });
});