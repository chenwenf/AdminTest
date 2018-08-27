
var $;
layui.config({
    base: "/Scripts/Sys/"
}).use(['form', 'layer', 'upload', 'laydate','layedit'], function () {
    var form = layui.form;
    var layer = parent.layer === undefined ? layui.layer : parent.layer;
    var laydate = layui.laydate;
    $ = layui.jquery, upload = layui.upload;
    var index = parent.layer.getFrameIndex(window.name);


    var img1 = upload.render({
        elem: '#test1'
        , url: '/UploadImgAPI/UploadImg'
        , before: function (obj) {
            //Ԥ�������ļ�ʾ������֧��ie8
            obj.preview(function (index, file, result) {
                $('#ImageUrl1').attr('src', result); //ͼƬ���ӣ�base64��
            });
        }
        , done: function (res) {
            //����ϴ�ʧ��
            if (res.code > 0) {
            }
            //�ϴ��ɹ�
            $('#ImageUrl1').attr('src', res.data.src);
        }
        , error: function () {

        }
    });


    //ָ�����ڿؼ�Ԫ��
    var start = {
        elem: '#CreateTime',//ָ��Ԫ��
        type: 'datetime'
    };

    //��������
    laydate.render(start);

    //���帻�ı�
    var layedit = layui.layedit;

    layedit.set({
        uploadImage: {
            url: "/UploadImgAPI/UploadImg",
            type: 'post'//�ӿ�url
        }
    });
    //ָ�����ı�����
    var index1 = layedit.build('Description');

    $(function () {
        var userid = Js_API.GetQueryString("id");
        var statu = Js_API.GetQueryString("statu");
        if (statu === "2") {
            $('#div1').show();
        }
        var reqData = { UserId: userid };
        Js_API.Post('/SysUser/GetUserOne', reqData, function (data) {
            getList(data);
            //������Ⱦ������
            form.render('select');
        });

    });

    //�����ύ
    form.on('submit(updateUser)', function () {
        var content = layui.layedit.getContent(index1);
        $('#Description').val(content);
        var img1 = $("#ImageUrl1").attr("src");

        var form = $('#form').serialize();
        Js_API.Post('/SysUser/UpdUserInfo', form, function (data) {
            alert(data.msg);
                parent.layer.close(index);
                window.parent.location.reload();
        });
    
    });

    function getList(data) {
        $("#UserId").val(data.UserId);
        $("#UserName").val(data.UserName);
        $("#WxCountry").val(data.WxCountry);
        $("#WxProvince").val(data.WxProvince);
        $("#WxCity").val(data.WxCity);
        $("#Phone").val(data.Phone);
        $("#CreateTime").val(data.CreateTime);
        $("#Remark").val(data.Remark);
        if (data.Sex === 0) {
            $("#Sex").val("0");
        } else if (data.Sex === 1) {
            $("#Sex").val("1");
        } else {
            $("#Sex").val("2");
        }

    }
});


