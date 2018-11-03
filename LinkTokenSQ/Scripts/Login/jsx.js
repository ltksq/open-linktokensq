
var scodeclick = 0;
$(document).ready(function () {
    $("#scode").click(function () {
 
        if (scodeclick > 7) {
            myalert('同一位置点太多.');
            return;
        }
        scodeclick++;
        $(this).attr("src", $(this).attr("src") + "?");
    });
    $("#showTooltips").click(function () {
        //登录
        var txttel = $("#txttel").val();
        var txtpwd = $("#txtpwd").val();
        var txtscode = $("#txtscode").val();
        if (txttel=="") {
            myalert("请输入手机号码");
            return;
        }
        if (txtpwd == "") {
            myalert("请输入密码");
            return;
        }
        if (txtscode == "") {
            myalert("请输入验证码");
            return;
        }
        var context = { "ccode": $("#selarea").val(), "userid": txttel, "pwd": txtpwd, "scode": txtscode };
        $.ajaxPost(AP_ROOT+"Login/CheckLogin", context, function (response) {
            if (response.IsSuccess) {
                window.location.href = response.Message;

            } else {
                $("#scode").attr("src", $("#scode").attr("src") + "?");
                myalert(response.Message);
            }
        });
    });
    
    $("#forgetpwd").click(function () {
        if ($("#txttel").val() == "") {
            myalert("请输入手机号.");
            return;
        }
        var context = { "userid": $("#txttel").val() };
        $.ajaxPost(AP_ROOT + "modifyuinfo/SetTempDataUserid", context, function (response) {
            if (response.IsSuccess) {
                window.location.href = response.Message;
            } else {
                myalert("手机号码未注册.");
            }
        });
    });
});

