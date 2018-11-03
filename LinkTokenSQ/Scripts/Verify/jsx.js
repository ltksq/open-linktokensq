function CheckVerify()
{
    window.clearInterval(timer1);
    var context = {  };
    $.ajaxPost(AP_ROOT + "Verify/CheckVerify",context, function (response) {
        if (response.IsSuccess) {
            window.location.href = "/";
        } else {

            // myalert(response.Message);
        }
    });

    timer1 = window.setTimeout(function () {
        CheckVerify();
    }, 5000);
}

var timer1 = window.setTimeout(function () {
    CheckVerify();
}, 5000);

