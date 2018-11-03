var scodeclick = 0;
$(document).ready(function () {
    var clickbtn = false;
    $("#scode").click(function () {
 
        if (scodeclick > 7) {
            myalert('同一位置点太多.');
            return;
        }
        scodeclick++;
        $(this).attr("src", $(this).attr("src") + "?");
    });
    $("#showTooltips").click(function () {
        if (clickbtn) return;
        var txttel = $("#txttel").val();
  
        var txttelcode = $("#txttelcode").val();
        var wkcaddress = $("#wkcaddress").val();
        var txtlpwd1 = $("#txtlpwd1").val();
        var txtjpwd1 = $("#txtjpwd1").val();

        if (txttel=="") {
            myalert("请输入手机号码");
            return;
        }
        if (txttelcode == "") {
            myalert("请输入短信验证码");
            return;
        }
 
       
        if (txtlpwd1 != "" && $("#txtlpwd2").val()!="" && txtlpwd1 != $("#txtlpwd2").val()) {
            myalert("两次登录密码不一致.");
            return;
        }
 

      
        clickbtn = true;
        var context = {
            "tel": txttel, "area": $("#selarea").val(), "scode": txttelcode, "pwd1": txtlpwd1
            , "pwd2": '', "wkcaddress": wkcaddress
            , "nickname": $("#txtnikename").val()
        };
        $.ajaxPost(AP_ROOT + "ModifyUInfo/UInfoModify", context, function (response) {
            if (response.IsSuccess) {
                clickbtn = false;
                window.location.href = response.Message;
            } else {
                clickbtn = false;
                myalert(response.Message);
            }
        });
    });

    $("#btntelcode").click(function () {

        if (scodeclick > 7) {
            myalert('同一位置点太多.');
            return;
        }
        scodeclick++;

        var txttel = $("#txttel").val();

        var txtscode = $("#txtscode").val();
       
        if (txttel == "") {
            myalert("请输入手机号码");
            return;
        }
        if (txtscode == "") {
            myalert("请输入验证码");
            return;
        }
         
        var context = { "tel": txttel, "area": $("#selarea").val(), "scode": txtscode,flag:1 };
        $.ajaxPost(AP_ROOT + "Register/GetTelCode", context, function (response) {
            if (response.IsSuccess) {
                msgdjs = 60;
                document.getElementById("btntelcode").disabled = true;
                timer1 = window.setTimeout(function () {
                    msgwait();
                }, 1000);
                myalert("验证码已发送,请注意查收.");
                $("#scode").attr("src", $("#scode").attr("src") + "?");
            } else {
                $("#scode").attr("src", $("#scode").attr("src") + "?");
                myalert(response.Message);
            }
        });
    });
});


var msgdjs = -1;

function msgwait() {
    window.clearInterval(timer1);
    if (msgdjs == -1) return;
    document.getElementById("btntelcode").disabled = true;
    document.getElementById('btntelcode').innerHTML = "("+msgdjs+"s)重发";
    if (msgdjs-- <= 0) {
        document.getElementById("btntelcode").disabled = false;
        document.getElementById('btntelcode').innerHTML = "获取验证码";
    }
    else
    {
        timer1 = window.setTimeout(function () {
            msgwait();
        }, 1000);
    }
}

var timer1 = window.setTimeout(function () {
    msgwait();
}, 1000);

