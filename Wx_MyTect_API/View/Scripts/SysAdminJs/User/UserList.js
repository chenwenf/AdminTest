
layui.use('table', function () {
    var table = layui.table
        , form = layui.form;

    table.render({
        elem: "#tableData"
        , url: '/SysUser/GetUserList'
        , page: true
        , cellMinWidth: 120
        , cols: [[{ type: "checkbox" }
            , { field: "UserHead", title: "用户头像", templet: "#headimg" }
            , { field: "UserName", title: "用户昵称" }
            , {
            field: "Remark", title: "备注", templet: function (d) {
                if (typeof (d.Remark) === "undefined" || d.Remark === "") { return "暂未备注"; }
                return '<span style="color:red;">' + d.Remark + '</span>';
            }
        }
            , { field: "subNum", title: "下级人数" }

            , {
            field: "IsAmbassador", title: "等级", templet: function (d) {
                if (d.IsAmbassador === 1) {
                    var n1 = '推广大使';
                    return '<span style="color:#FFB800;">' + n1 + '</span>';
                }
                var n2 = '普通会员';
                return '<span style="color:#1E9FFF;">' + n2 + '</span>';
            }
        }
            , { width: 500, title: "操作", align: "left", toolbar: "#barUser" }
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
            var data = { ids: ids }

            JsAPI.PostAsyn('', data, function (data) {
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
        var userId = data.UserId;
        var index;
        if (layEvent === "detail") {
            index = layui.layer.open({
                title: "查看用户",
                type: 2,
                content: "UserDetail?statu=1&id=" + data.UserId,
                success: function (layero, index) {
                    var body = layer.getChildFrame("body", index);
                }
            });
            layui.layer.full(index);
        }
        //删除
        else if (layEvent === "del") {
            var ids = [];
            ids.push(data.UserId);
            deleteAjax(ids);
        }
        else if (layEvent === "edit") {
            index = layui.layer.open({
                title: "编辑用户",
                type: 2,
                content: "UserDetail?type=select&statu=2&id=" + data.UserId,
                success: function (layero, index) {
                    var body = layer.getChildFrame("body", index);
                }
            });
            layui.layer.full(index);
        }
    });


    var $ = layui.$, active = {
        reload: function () {
            var txtUserName = $("#txtUserName");
            //查看
            table.reload("tableDataDT", {
                page: {
                    curr: 1
                },
                where: {
                    UserName: txtUserName.val()
                }
            });
        },
        //多行删除
        getCheckData: function () {
            var checkStatus = table.checkStatus("tableDataDT");
            var data = checkStatus.data;
            var ids = [];
            for (var i in data) {
                if (data.hasOwnProperty(i)) {
                    ids.push(data[i].UserId);
                }
            }
            deleteAjax(ids);
        }
    };
    $("#searchbtn,#batchdel").on("click", function () {
        var type = $(this).data("type");
        active[type] ? active[type].call(this) : "";
    });

});