 
var pageNum = 1;
var totalPage = 1;
//载入数据列表
function loadSodList() {
 
    
    var loadFlg = true;
    // 取消之前绑定的滚动事件，载入数据时重新绑定
    $(window).off("scroll");
    $(window).dropload({ afterDatafun: lowadData });
    function lowadData() {
        if (!loadFlg) return false;
        if (pageNum > totalPage) {
            if (pageNum > totalPage) {
                $("#loading").show().html('到底了,没有更多数据了');
            } else {
                setTimeout(function () {
                    $("#loading").hide();
                }, 1000);
            }
            return;
        }
        loadFlg = false;

        var txtKeyords = document.getElementById('txtKeyords').value;
        var dlstatus = document.getElementById('dlstatus').value;

        $.ajax({
            type: "post",
            data: { key: txtKeyords, status: dlstatus, pagenum: pageNum },
            url: pagedata_reurl,
            asyn: false,
            beforeSend: function () {
                $("#loading").show();
            },
            success: function (data) {
                if (data.RetCode == 0) {
                    $("#ajaxData").html("<center><span><br />暂无数据</span></center>");
                    $("#loading").hide();
                } else {

                    $('#ajaxData').append(data.Html);

                    totalPage = parseInt(data.ErrorMessage);

                    pageNum++;
                    loadFlg = true;

                    if (pageNum > totalPage) {
                        $("#loading").show().html('到底了,没有更多数据了');
                    } else {
                        setTimeout(function () {
                            $("#loading").hide();
                        }, 1000);
                    }
                }
            }
        });
    }
}
 