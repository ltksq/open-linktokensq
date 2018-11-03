

(function ($) {
    //form调用ajax, url:连接的Ashx地址、func：回调函数function(response, success){}
    $.fn.extend({
        ajaxPost: function (url, func, isShowLoading) {
            var request = $(this).serialize();
            return $.ajaxPost(url, request, func, isShowLoading);
        }
    });
    $.fn.extend({
        ajaxForOrder: function (url, func) {
            var request = $(this).serialize();
            $.ajaxPostOrder(url, request, func);
        }
    });
    $.ConvertToNumber = function (obj) {
        if (obj == null || obj == '' || obj == 'undefined' || isNaN(obj)) {
            return 0;
        } else {
            return obj;
        }
    }
    $.FormatShow = function (obj) {
        if (obj == null || obj == '' || obj == 'undefined') {
            return '';
        } else {
            return '【' + obj + '】';
        }
    }
    $.FormatString = function (obj) {
        if (obj == null || obj == 'null' || obj == '' || obj == 'undefined') {
            return '';
        } else {
            return obj;
        }
    }

    $.IsNullOrEmptyOrUndefined = function (obj) {
        if (obj == null || obj == '' || obj == 'undefined') {
            return true;
        }
        return false;
    }
    $.createHiddenMethod = function (name, value, form) {
        var el = form[name];
        if (!el) {
            el = document.createElement('input');
            el.type = 'hidden';
            el.name = name;
            form.appendChild(el);
        }
        el.value = value;
    }
    $.openOrderLoading = function () {
        var div = {
            show: function () {
                if ($(".loadingDivOrder").length) {
                    $(".loadingDivOrder").show();
                    $("#divProcessingBarOrder").show();
                } else {
                    $(document.body).append(div.mask);
                    $(document.body).append(div.processingBar);
                }

                var top = 0;
                if ($(window).scrollTop() > 0) {
                    top = $(window).height() + $(window).scrollTop() - (($(window).height() + div.processingBar.height()) * 0.2);
                } else {
                    top = ($(window).height() - div.processingBar.height()) * 0.9;
                }

                div.processingBar.css({
                    "position": "absolute",
                    "left": ($(document.body).width() - 300) / 2,
                    "top": top,
                    "z-index": 1170
                }).show();
            }
        };
        div.mask = $("<div class='loadingDivOrder' id='loadingDivOrder'></div>")
           .css({
               "background-Color": "rgb(255, 255, 255)",
               "position": "absolute",
               "border": "medium none",
               "left": 0,
               "right": 0,
               "top": 0,
               "bottom": 0,
               "-moz-opacity": 0,
               "opacity": 0,
               "filter": "alpha(opacity=0)",
               "z-index": 1160,
               "width": "100%",
               "height": $(window).height() > $(document.body).height() ? $(window).height() : $(document.body).height()
           });

        div.processingBar = $("<div id=\"divProcessingBarOrder\" class=\"c_mask\"><div class=\"c_mask_bd c_loading\"><img src=\"/images/loading.gif\" alt=\"\"><strong class=\"font16\">处理中，请稍后...</strong></div></div>")
            .css({
                "width": "180px"
            });
        //div.show();
    }

    $.closeOrderLoading = function () {
        if ($(".loadingDivOrder").length) {
            $(".loadingDivOrder").hide();
            $("#divProcessingBarOrder").hide();
        }
    }

    $.ajaxPostOrder = function (url, requestData, func) {
        if (!$.isFunction(func)) {
            alert("传入的回调方法参数func，不是方法。请检查调用的地方！！");
            return;
        }
        $.openOrderLoading();
        $.ajax({
            type: "post",
            url: url,
            data: requestData,
            cache: false,
            success: function (responseData, textStatus, jqXHR) {
                var postResponse = {};
                if (jqXHR.getResponseHeader("Content-Type").split(";")[0] === "application/json") {
                    postResponse = responseData;
                } else {
                    postResponse.IsSuccess = true;
                    postResponse.Message = "";
                    postResponse.Data = responseData;
                }
                $.closeOrderLoading();
                func(postResponse);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.closeOrderLoading();
                $.showMessageBox("访问人较多,刷新一下即可.");
            }
        });
    }

    $.ajaxPost = function (url, requestData, func, isShowLoading) {
        if (!$.isFunction(func)) {
            alert("传入的回调方法参数func，不是方法。请检查调用的地方！！");
            return;
        }
        if (isShowLoading == null || isShowLoading) {
            $.openLoading();
        }
        try{ 
            return $.ajax({
                type: "post",
                url: url,
                data: requestData,
                cache: false,
                success: function (responseData, textStatus, jqXHR) {
                    var postResponse = {};
                    if (jqXHR.getResponseHeader("Content-Type") && jqXHR.getResponseHeader("Content-Type").split(";")[0] === "application/json") {
                        postResponse = responseData;
                    } else {
                        postResponse.IsSuccess = true;
                        postResponse.Message = "";
                        postResponse.Data = responseData;
                    }
                    //setTimeout(function () { $.closeLoading(); func(postResponse) }, 500);
                    if (isShowLoading == null || !isShowLoading) {
                        $.closeLoading();
                    }
                    func(postResponse);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //setTimeout(function () { $.closeLoading(); $.showMessageBox(jqXHR.status + " " + jqXHR.statusText) }, 500);
                    if (isShowLoading == null || isShowLoading) {
                        $.closeLoading();
                    }
                    //$.showMessageBox(jqXHR.status + " " + jqXHR.statusText);
                   $.showMessageBox("访问人较多,刷新一下即可.");
                }
            });
        }
        catch(ex){}
    };

    $.ajaxPostPK = function (url, requestData, func, isShowLoading) {
        if (!$.isFunction(func)) {
            alert("传入的回调方法参数func，不是方法。请检查调用的地方！！");
            return;
        }
        if (isShowLoading == null || isShowLoading) {
            $.openLoading();
        }
        try {
            return $.ajax({
                type: "post",
                url: url,
                data: requestData,
                cache: false,
                success: function (responseData, textStatus, jqXHR) {
                    var postResponse = {};
                    if (jqXHR.getResponseHeader("Content-Type") && jqXHR.getResponseHeader("Content-Type").split(";")[0] === "application/json") {
                        postResponse = responseData;
                    } else {
                        postResponse.IsSuccess = true;
                        postResponse.Message = "";
                        postResponse.Data = responseData;
                    }
                    //setTimeout(function () { $.closeLoading(); func(postResponse) }, 500);
                    if (isShowLoading == null || !isShowLoading) {
                        $.closeLoading();
                    }
                    func(postResponse);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //setTimeout(function () { $.closeLoading(); $.showMessageBox(jqXHR.status + " " + jqXHR.statusText) }, 500);
                    if (isShowLoading == null || isShowLoading) {
                        $.closeLoading();
                    }
                    //$.showMessageBox(jqXHR.status + " " + jqXHR.statusText);
                    //$.showMessageBox("访问人较多,刷新一下即可.");
                    try {
                        window.clearTimeout(timer1);
                        timer1 = window.setTimeout(function () { arenaview() }, 2000);
                    } catch (exx) { }
                }
            });
        }
        catch (ex) { }
    };

    //form调用ajax, url:连接的Ashx地址、func：回调函数function(response, success){}
    $.fn.extend({
        ajaxJson: function (url, func) {
            var jsonObj = $(this).serializeObject();
            $.ajaxJson(url, jsonObj, func);
        }
    });

    $.ajaxJson = function (url, jsonObj, func) {
        //var request = JSON.stringify(jsonObj);
        var request = $.toJSON(jsonObj);
        $.ajaxPost(url, request, func);
    }


    //鼠标提示框
    $.addTips = function (srcElem, p) {
        var g = {
            setting: {},
            show: function () {
                var content = g.src.attr("mod_jmpinfo_content");  //提示内容
                if (content == null) {
                    content = g.src.parent().find("span").attr("mod_jmpinfo_content")
                }
                if (content == null) return;
                var tip = $("#tuna_jmpinfo");
                if (tip.length) {

                    tip.find("div.jmp_bd:first").html(content);
                    tip.css({
                        top: parseInt(g.src.position().top + 20),
                        left: parseInt(g.src.position().left)
                    });
                    tip.show();
                } else {

                    var tips = "<div id=\"tuna_jmpinfo\" style=\"position: absolute; z-index: 1120;left:" + parseInt(g.src.position().left) + "px;top:" + parseInt(g.src.position().top + 20) + "px\">";
                    tips += "<div class=\"base_jmp jmp_text base_jmp_t\" style=\"width: 340px;\">";
                    tips += "<b class=\"tri_t\" style=\"left: 16px;\"></b>";
                    tips += "<div class=\"jmp_bd\">";
                    tips += content;
                    tips += "</div>";
                    tips += "</div>";
                    tips += "</div>";

                    $(document.body).append($(tips));
                }
            },
            hide: function () {
                var tip = $("#tuna_jmpinfo");
                if (tip.length) {
                    tip.hide();
                }
            }
        };

        g.setting = p || {
            containerId: "tuna_jmpinfo"
        };

        g.src = srcElem;
        g.src.mouseover(function () {
            g.show();
        });
        g.src.mouseout(function () {
            g.hide();
        });

        //        $(document).click(function () { g.hide(); });
        return g;
    };

    $.showNotice = function () {
        $("TEXTAREA,input[mod-notice-tip]").each(function () {
            if ($(this).val() == "" || $(this).val() == $(this).attr("mod-notice-tip")) {
                $(this).val($(this).attr("mod-notice-tip"));
            }
            if ($(this).attr("mod-notice-tip") != null) {
                if ($(this).val() == "" || $(this).val() == $(this).attr("mod-notice-tip")) {
                    $(this).val($(this).attr("mod-notice-tip")).css("color", "#979393");
                }
                $(this).focus(function () {
                    if ($(this).val() == $(this).attr("mod-notice-tip")) {
                        $(this).val('');
                        $(this).val('').css("color", "#000000");
                    }
                });
                $(this).blur(function () {
                    if ($(this).val() == "") {
                        $(this).val($(this).attr("mod-notice-tip"));
                        $(this).val($(this).attr("mod-notice-tip")).css("color", "#979393");
                    }
                });

                $(this).keyup(function () {
                    $(this).css("color", "#000000");
                });
            }
        });
    }
    $.numberInputCheck = function () {
        //只允许输入数字
        $(".isNumberInput").keyup(function () {
            $(this).val($(this).val().replace(/\D|^0/g, ''));
            // if ($(this).val().length == 0) $(this).val("0");
        }).bind("paste", function () {
            //CTR+V事件处理
            // $(this).val($(this).val().replace(/\D|^0/g, ''));
            var el = $(this);
            //直接获取文本框的值 获得是文本框粘贴之前的值，使用setTimeout 就可以获得粘贴的值
            setTimeout(function () {
                var numValue = $(el).val();
                numValue = numValue.replace(/\D|^0/g, '');
                $(this).val(numValue);
            }, 100);
        }).css("ime-mode", "disabled");

    }
    $.numberInputCheckInculdZero = function () {
        //只允许输入数字
        $(".isNumberInput").keyup(function () {
            $(this).val($(this).val().replace(/\D|^/g, ''));
            // if ($(this).val().length == 0) $(this).val("0");
        }).bind("paste", function () {
            //CTR+V事件处理
            // $(this).val($(this).val().replace(/\D|^0/g, ''));
            var el = $(this);
            //直接获取文本框的值 获得是文本框粘贴之前的值，使用setTimeout 就可以获得粘贴的值
            setTimeout(function () {
                var numValue = $(el).val();
                numValue = numValue.replace(/\D|^/g, '');
                $(this).val(numValue);
            }, 100);
        }).css("ime-mode", "disabled");

    }
    $.floatDataInputCheck = function () {
        //只允许输入浮点数字
        $(".floatDataInput").keyup(function () {
            $(this).val($(this).val().replace(/[^\d\.\-]/g, ''));
            //if ($(this).val().length == 0) $(this).val("0");
        }).bind("paste", function () {
            //CTR+V事件处理
            var el = $(this);
            //直接获取文本框的值 获得是文本框粘贴之前的值，使用setTimeout 就可以获得粘贴的值
            setTimeout(function () {
                var numValue = $(el).val();
                numValue = numValue.replace(/[^\d\.\-]/g, '');
                $(this).val(numValue);
            }, 100);
        }).css("ime-mode", "disabled");

    }
    $.isSearchIdsInputCheck = function () {
        //只允许输入数字+逗号
        $(".isSearchIdsInput").keyup(function () {
            $(this).val($(this).val().replace(/[^\d\,]|^\,|^0/g, ''));
            if ($(this).val().length == 0) $(this).val("");
        }).bind("paste", function () {
            //CTR+V事件处理
            var el = $(this);
            //直接获取文本框的值 获得是文本框粘贴之前的值，使用setTimeout 就可以获得粘贴的值
            setTimeout(function () {
                var numValue = $(el).val();
                numValue = numValue.replace(/[^\d\,]|^\,|^0/g, '');
                $(this).val(numValue);
            }, 100);

        }).css("ime-mode", "disabled");

    }
    $.InputTextTrim = function () {
        //只允许输入数字+逗号
        $(".TextCheckTrim").blur(function () {
            $(this).val($(this).val().replace(/(^\s+)|(\s+$)/g, ''));
            if ($(this).val().length == 0) $(this).val("");
        }).bind("paste", function () {
            //CTR+V事件处理
            var el = $(this);
            //直接获取文本框的值 获得是文本框粘贴之前的值，使用setTimeout 就可以获得粘贴的值
            setTimeout(function () {
                var textValue = $(el).val();
                textValue = textValue.replace(/(^\s+)|(\s+$)/g, '');
                $(this).val(textValue);
            }, 100);

        });
    }

    $.openLoading = function () {
        var div = {
            show: function () {
                if ($(".loadingDiv").length) {
                    $(".loadingDiv").show();
                    $("#divProcessingBar").show();
                } else {
                    $(document.body).append(div.mask);
                    $(document.body).append(div.processingBar);
                }

                var top = 0;
                if ($(window).scrollTop() > 0) {
                    top = $(window).height() + $(window).scrollTop() - ($(window).height() + div.processingBar.height()) / 2;
                } else {
                    top = ($(window).height() - div.processingBar.height()) / 2
                }

                div.processingBar.css({
                    "position": "absolute",
                    "left": ($(document.body).width() - 300) / 2,
                    "top": top,
                    "z-index": 1170
                }).show();
            }
        };
        div.mask = $("<div class='loadingDiv' id='loadingDiv'></div>")
           .css({
               "background-Color": "rgb(255, 255, 255)",
               "position": "absolute",
               "border": "medium none",
               "left": 0,
               "right": 0,
               "top": 0,
               "bottom": 0,
               "-moz-opacity": 0,
               "opacity": 0,
               "filter": "alpha(opacity=0)",
               "z-index": 1160,
               "width": "100%",
               "height": $(window).height() > $(document.body).height() ? $(window).height() : $(document.body).height()
           });

        div.processingBar = $("<div id=\"divProcessingBar\" class=\"c_mask\"><div class=\"c_mask_bd c_loading\"><img src=\"/images/loading.gif\" alt=\"\"><strong class=\"font16\">处理中，请稍后...</strong></div></div>")
            .css({
                "width": "300px"
            });
        //div.show();
    }

    $.closeLoading = function () {
        if ($(".loadingDiv").length) {
            //setTimeout(function () { $(".loadingDiv").hide(); $("#divProcessingBar").hide(); }, 500);
            $(".loadingDiv").hide();
            $("#divProcessingBar").hide();
        }
    }

    $.showMessageBox = function (msg, url) {
        this.hrefurl = url;
        var div = {
            show: function () {
                if ($(".messageBoxMask").length) {
                    $(".messageBoxMask").show();
                } else {
                    $(document.body).append(div.mask);
                }
                if ($("#divMessageBox").length) {
                    $("#divMessageBox").remove();
                    $(document.body).append(div.messageBox);
                } else {
                    $(document.body).append(div.messageBox);
                }

                var top = 0;
                if ($(window).scrollTop() > 0) {
                    top = $(window).height() + $(window).scrollTop() - ($(window).height() + div.messageBox.height()) / 2;
                } else {
                    top = ($(window).height() - div.messageBox.height()) / 2
                }

                div.messageBox.css({
                    "position": "absolute",
                    "left": ($(document.body).width() - 300) / 2,
                    "top": top,
                    "z-index": 1170
                }).show();
            }
        };
        div.mask = $("<div class='messageBoxMask' id='messageBoxMask'></div>")
           .css({
               "background-Color": "rgba(255, 255, 255, 0.4)",
               "position": "absolute",
               "border": "medium none",
               "left": 0,
               "right": 0,
               "top": 0,
               "bottom": 0,
               "z-index": 1160,
               "width": "100%",
               "height": $(window).height() > $(document.body).height() ? $(window).height() : $(document.body).height()
           });
        div.messageBox = $("<div id=\"divMessageBox\" class=\"c_mask\"><h3><a class=\"delete\" href=\"javascript:void(0);\" onclick=\"$.hideMessageBox()\">×</a>提示</h3><div class=\"c_mask_bd\"><div class=\"c_alert3\"><span class=\"ico_alert\"></span><p class=\"single\">" + msg + "</p><br /></div><input id=\"btnShowMessage\" type=\"button\" class=\"btn btn_azure\" value=\"确 定\" style=\"margin-left:80px\" onclick=\"$.hideMessageBox()\" /></div></div>")
            .css({
                "width": "300px"
            });
        div.show();
        $("#btnShowMessage").focus();
        $(document).keyup(function (event) {
            if (event.keyCode == 13) {
                $.hideMessageBox();
            }
        });
    }
    $.showMessageBox2 = function (msg, btntext) {
        var div = {
            show: function () {
                if ($(".messageBoxMask").length) {
                    $(".messageBoxMask").show();
                } else {
                    $(document.body).append(div.mask);
                }
                if ($("#divMessageBox").length) {
                    $("#divMessageBox").remove();
                    $(document.body).append(div.messageBox);
                } else {
                    $(document.body).append(div.messageBox);
                }

                var top = 0;
                if ($(window).scrollTop() > 0) {
                    top = $(window).height() + $(window).scrollTop() - ($(window).height() + div.messageBox.height()) / 2;
                } else {
                    top = ($(window).height() - div.messageBox.height()) / 2
                }

                div.messageBox.css({
                    "position": "absolute",
                    "left": ($(document.body).width() - 300) / 2,
                    "top": top,
                    "z-index": 1170
                }).show();
            }
        };
        div.mask = $("<div class='messageBoxMask' id='messageBoxMask'></div>")
           .css({
               "background-Color": "rgba(255, 255, 255, 0.4)",
               "position": "absolute",
               "border": "medium none",
               "left": 0,
               "right": 0,
               "top": 0,
               "bottom": 0,
               "z-index": 1160,
               "width": "100%",
               "height": $(window).height() > $(document.body).height() ? $(window).height() : $(document.body).height()
           });
        div.messageBox = $("<div id=\"divMessageBox\" class=\"c_mask\"><h3><a class=\"delete\" href=\"javascript:void(0);\" onclick=\"$.hideMessageBox()\">×</a>提示</h3><div class=\"c_mask_bd\"><div class=\"c_alert3\"><span class=\"ico_alert\"></span><p class=\"single\">" + msg + "</p><br /></div><input id=\"btnShowMessage\" type=\"button\" class=\"btn btn_azure\" value=\"" + btntext + "\" style=\"margin-left:80px\" onclick=\"$.hideMessageBox()\" /></div></div>")
            .css({
                "width": "300px"
            });
        div.show();
        $("#btnShowMessage").focus();
        $(document).keyup(function (event) {
            if (event.keyCode == 13) {
                $.hideMessageBox();
            }
        });
    }
    $.formatterDate = function (obj, str) {
        var date = new Date(obj);
        var datetime = date.getFullYear()
                + str// "年"
                + ((date.getMonth() + 1) > 10 ? (date.getMonth() + 1) : "0"
                        + (date.getMonth() + 1))
                + str// "月"
                + (date.getDate() < 10 ? "0" + date.getDate() : date
                        .getDate());
        return datetime;
    }

    $.hideMessageBox = function () {
        if ($(".messageBoxMask").length) {
            //setTimeout(function () { $(".loadingDiv").hide(); $("#divProcessingBar").hide(); }, 500);
            $(".messageBoxMask").hide();
            $("#divMessageBox").hide();
            if (this.hrefurl != null) { location.href = this.hrefurl; }
        }
    }

    //关闭弹出层
    $.closeWin = function (srcElem, p) {
        if ($(".coverlay").length) $(".coverlay").hide();
        srcElem.hide();
    }

    //弹出层
    $.showWin = function (srcElem, p) {
        var g = {
            setting: {},
            show: function () {
                if ($(".coverlay").length)
                    $(".coverlay").show();
                else
                    $(document.body).append(g.mask);

                var top = 0;
                if ($(window).scrollTop() > 0) {
                    top = $(window).height() + $(window).scrollTop() - ($(window).height() + g.src.height()) / 2;
                } else {
                    top = ($(window).height() - g.src.height()) / 2
                }

                g.src.css({
                    "position": "absolute",
                    "left": ($(document.body).width() - g.src.width()) / 2,
                    "top": (top < 0 ? 10 : top),
                    "z-index": 98
                }).show();
            },
            close: function () {
                if ($(".coverlay").length) $(".coverlay").hide();
                g.src.hide();
            }
        };
        g.mask = $("<div class='coverlay' id='coverlay'/>")
           .css({
               "background-Color": "rgb(255, 255, 255)",
               "position": "absolute",
               "border": "medium none",
               "left": 0,
               "right": 0,
               "top": 0,
               "bottom": 0,
               "-moz-opacity": 0.4,
               "opacity": 0.4,
               "filter": "alpha(opacity=40)",
               "z-index": 90,
               "width": "100%",
               "height": $(window).height() > $(document.body).height() ? $(window).height() : $(document.body).height()
           });
        g.setting = p || {
            containerId: "tuna_jmpinfo"
        };

        g.src = srcElem;

        g.show();

        return g;
    };

    //鼠标提示框
    $.fn.extend({
        cTips: function (p) {
            return $.addTips($(this), p);
        }
    });

    //弹出层
    $.fn.extend({
        cPopWin: function (p) {
            return $.showWin($(this), p);
        }
    });
    //弹出层
    $.fn.extend({
        openWin: function (p) {
            return $.showWin($(this), p);
        }
    });

    //关闭弹出层
    $.fn.extend({
        closeWin: function (p) {
            return $.closeWin($(this), p);
        }
    });

    //验证是否为手机格式
    $.fn.extend({
        isMobile: function () {
            var isMobile = /^1[34578]\d{9}$/;
            return isMobile.test($(this).val());
        }
    });

    //验证是否中文
    $.fn.extend({
        isCH: function () {
            var is_ch = /^[\u0391-\uFFE5]+$/;
            return is_ch.test($(this).val());
        }
    });
    //验证座机
    $.fn.extend({
        isPhone: function () {
            var isphone = /^(.)+$/;
            return isphone.test($(this).val());
        }
    });
    //验证座机手机
    $.fn.extend({
        isTelephoe: function () {
            var istelephoe = /^\d[\d-]{4,18}\d$/;
            return istelephoe.test($(this).val());
        }
    });

    //验证是否为整数
    $.fn.extend({
        isInt: function () {
            var isInt = /^\+?[0-9][0-9]*$/
            return isInt.test($(this).val());
        }
    });

    //验证是否为金额
    $.fn.extend({
        isMoney: function () {
            var isMoney = /^[0-9]*(\.[0-9]{1,2})?$/
            return isMoney.test($(this).val());
        }
    });

    //验证是否为邮箱格式
    $.fn.extend({
        isEmail: function () {
            var isEmail = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/
            return isEmail.test($(this).val());
        }
    });

    //验证是否为时间格式：hh:mm
    $.fn.extend({
        isTimehm: function () {
            var isTimehm = /^(0\d{1}|1\d{1}|2[0-3]):([0-5]\d{1})$/
            return isTimehm.test($(this).val());
        }
    });




    //验证是否为日期
    $.fn.extend({
        isDateymd: function () {
            var _sText = $(this).val();
            var sYear = _sText.substring(0, 4);
            var sMonth = _sText.substring(5, 7);
            var sDay = _sText.substring(8, 10);
            var regexp = /^[1-9]{1}[0-9]{3}-[0-1]?[0-9]{1}-[0-3]?[0-9]{1}$/;
            var largeMonth = "01,03,05,07,08,10,12";
            var smallMonth = "02,04,06,09,11";

            if (regexp.test(_sText)) {//进行正则表达式验证
                var iYear = parseInt(sYear); //将年份转化成整数类型

                if (iYear % 100 != 0 && iYear % 4 == 0 || iYear % 400 == 0) {//进行闰年判断,闰年<=29,非闰年<=28
                    if (sMonth == "02") {
                        return sDay <= 29 ? true : false;
                    } else {
                        //进行大月和小月的判断
                        if (largeMonth.indexOf(sMonth) >= 0) {
                            return sDay <= 31 ? true : false;
                        }
                        if (smallMonth.indexOf(sMonth) >= 0) {
                            return sDay <= 30 ? true : false;
                        }
                    }
                } else {
                    if (sMonth == "02") {
                        return sDay <= 28 ? true : false;
                    } else {
                        //进行大月和小月的判断
                        if (largeMonth.indexOf(sMonth) >= 0) {
                            return sDay <= 31 ? true : false;
                        }
                        if (smallMonth.indexOf(sMonth) >= 0) {
                            return sDay <= 30 ? true : false;
                        }
                    }
                }
            } else
                return false;
        }
    });

    //将表单转换成Json对象
    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                if (this.value != '') {
                    o[this.name].push(this.value || '');
                }
            } else {
                if (this.value != '') {
                    o[this.name] = this.value || '';
                }
            }
        });
        return o;
    }

    //Table添加TR数据
    $.fn.extend({
        addRow: function (args) {
            var tr = null;
            $(this).find("tr").each(function () {
                if ($(this).css("display") == "none") {
                    tr = $(this).clone().show();
                }
            });
            for (i = 1; i <= arguments.length; i++) {
                tr.children("td::nth-child(" + i + ")").html(arguments[i - 1]);
            }
            $(this).append(tr);
            return tr;
        }
    });
    //Table清楚TR数据
    $.fn.extend({
        clearRow: function () {
            var trlist = $(this).find("tr");
            for (i = 1; i < trlist.length; i++) {
                if ($(trlist[i]).css("display") != "none") {
                    $(trlist[i]).remove();
                }
            }
        }
    });
    $.fn.extend({
        clearAll: function () {
            var trlist = $(this).find("tr");
            for (i = 1; i < trlist.length; i++) {
                $(trlist[i]).remove();
            }
        }
    });
    //Table获取Json对象
    $.fn.extend({
        getJson: function (jsonObj, cn) {
            var trlist = $(this).find("tr");
            var temp = "jsonObj." + cn + "=[]";
            eval(temp);

            for (i = 2; i < trlist.length; i++) {
                temp = "";
                $(trlist[i]).find("input").each(function (i, e) {
                    if (i == 0 && $(e).attr("type") == "checkbox" && !$(e).attr("checked")) {
                        return false;
                    }
                    if (i == 0 && $(e).attr("type") == "checkbox") {
                    }
                    else {
                        if ($(e).attr("name") != "" && $(e).val() != "") {
                            temp += $(e).attr("name") + ":" + $(e).val() + ",";
                        }
                    }
                });
                if (temp != "") {
                    temp = "jsonObj." + cn + ".push({" + temp.substring(0, temp.length - 1) + "});";
                    eval(temp);
                }
            }
        }
    });

    //序列化table中的所有控件，现包括text,hidden,checkbox,select
    $.fn.extend({
        serializeTable: function (jsonObj, jsonName) {
            eval("jsonObj." + jsonName + "=[]");

            var trList = $(this).find("tr");
            for (i = 0; i < trList.length; i++) {
                var evalString = "";
                $(trList[i]).find("input").each(function (i, e) {
                    if ($(e).attr("name")) {
                        if ($(e).attr("type") && $(e).attr("type") == "text") {
                            evalString += "'" + $(e).attr("name") + "':'" + $(e).val() + "',";
                        }
                        else if ($(e).attr("type") && $(e).attr("type") == "hidden") {
                            evalString += "'" + $(e).attr("name") + "':'" + $(e).val() + "',";
                        }
                        else if ($(e).attr("type") && $(e).attr("type") == "checkbox") {
                            evalString += "'" + $(e).attr("name") + "':" + ($(e).attr("checked") ? "true" : "false") + ",";
                        }
                        else if ($(e).attr("type") && $(e).attr("type") == "radio") {
                        }
                    }
                });
                $(trList[i]).find("select").each(function (i, e) {
                    if ($(e).attr("name")) {
                        evalString += "'" + $(e).attr("name") + "':'" + $(e).val() + "',";
                    }
                });

                if (evalString) {
                    //evalString = evalString.replace(/\[/g, "\\[").replace(/\]/g, "\\]");
                    evalString = "jsonObj." + jsonName + ".push({" + evalString.substring(0, evalString.length - 1) + "});";
                    eval(evalString);
                }

            }
        }
    })

    String.prototype.trimEnd = function (trimStr) {
        if (!trimStr) { return this; }
        var temp = this;
        while (true) {
            if (temp.substr(temp.length - trimStr.length, trimStr.length) != trimStr) {
                break;
            }
            temp = temp.substr(0, temp.length - trimStr.length);
        }
        return temp;
    };

    //封装Checkbox的操作
    $.checkbox = {
        //全选，n:Name
        all: function (n) {
            $("input[name=" + n + "]").each(
                function () { $(this).attr("checked", "checked"); }
            );
        },
        //全不选，n:Name
        not: function (n) {
            $("input[name=" + n + "]").each(
                function () { $(this).removeAttr("checked"); }
            );
        },
        //反选，n:Name
        or: function (n) {
            $("input[name=" + n + "]").each(
            function () {
                if ($(this).attr("checked")) {
                    $(this).removeAttr("checked");
                } else { $(this).attr("checked", "checked"); }
            });
        },

        //选中个数，n:Name
        selcount: function (n) {
            return $("input[name=" + n + "]:checked").length;
        },

        //获取Value数组
        vals: function (n) {
            var vals = [];
            $("input[name=" + n + "]:checked").each(
                function (i) { vals[i] = $(this).val(); }
            );
            return vals;
        }
    };

    //封装Radio的操作
    $.radio = {
        val: function (n) {
            return $("input[name=" + n + "]:checked").val();
        },
        init: function () {
            $("input[type=radio]").each(function () {

                $("input[name='" + $(this).attr("name") + "']").each(function (i, e) {
                    if ($(e).attr("type") == "radio" && $(e).attr("checked")) {
                        $(this).parent().addClass("o_label_cur");
                    } else {
                        $(this).parent().removeClass("o_label_cur");
                    }
                });

                $(this).click(function () {
                    $("input[name='" + $(this).attr("name") + "']").each(function (i, e) {
                        if ($(e).attr("type") == "radio") {
                            $(this).parent().removeClass("o_label_cur");
                        }
                    });
                    $(this).parent().addClass("o_label_cur");
                });
            });
        }
    };

    $.checkbox = {
        val: function (n) {
            return $("input[name=" + n + "]:checked").val();
        },
        init: function () {
            $("input[type=checkbox]").each(function () {
                var ck = $("#" + this.id);
                if (ck.attr("checked") == false) {
                    ck.parent(".o_label").removeClass("o_label_cur")
                } else {
                    ck.parent(".o_label").addClass("o_label_cur")
                }

                $(this).click(function () {
                    var ck = $("#" + this.id);
                    if (ck.attr("checked") == false) {
                        ck.parent(".o_label").removeClass("o_label_cur")
                    } else {
                        ck.parent(".o_label").addClass("o_label_cur")
                    }
                });
            });
        }
    };

    $.calendar = {
        load: function (data, left, right, isapprove, showType, func) {
            var html = '<div class="reserve_box">';
            html += '<ul class="reserve_datebox layoutfix">';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.PrevCalendarMonth.Year + ',' + data.PrevCalendarMonth.Month + ');" class="back"></a><span class="year">' + data.CurrCalendarMonth.Year + '年</span></li>';
            html += '<li class="current">' + data.CurrCalendarMonth.Month + '月</li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.CalendarMonth[1].Year + ',' + data.CalendarMonth[1].Month + ');">' + data.CalendarMonth[1].Month + '月</a></li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.CalendarMonth[2].Year + ',' + data.CalendarMonth[2].Month + ');">' + data.CalendarMonth[2].Month + '月</a></li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.CalendarMonth[3].Year + ',' + data.CalendarMonth[3].Month + ');">' + data.CalendarMonth[3].Month + '月</a></li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.CalendarMonth[4].Year + ',' + data.CalendarMonth[4].Month + ');">' + data.CalendarMonth[4].Month + '月</a></li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.CalendarMonth[5].Year + ',' + data.CalendarMonth[5].Month + ');">' + data.CalendarMonth[5].Month + '月</a></li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.NextCalendarMonth.Year + ',' + data.NextCalendarMonth.Month + ');" class="go"></a></li>';
            html += '</ul>'
            html += '</div>'
            html += '<div class="basefix">';
            html += '<div class="reserve_record">';
            for (i = 0; i < data.CalendarWeek.length; i++) {
                html += '<dl>';
                if (i == 0) {
                    html += '<dt class="month"><span>' + data.CurrCalendarMonth.Month + '</span>月</dt>';
                } else {
                    html += '<dt></dt>';
                }
                //获取当前周状态列表
                var ItemStatus = this.getStatus(data, i);
                //设置左边列框
                if (ItemStatus.length > 0) {
                    for (k = 0; k < ItemStatus.length; k++) {
                        for (j = 0; j < data.CalendarCatetory.length; j++) {
                            html += '<dd>' + data.CalendarCatetory[j].Name + '</dd>';
                        }
                    }
                } else {
                    html += '<dd></dd>';
                }
                html += '</dl>';
            }
            html += '</div>';
            html += '<table width="100%" class="reserve_table">';
            html += '<thead>';
            html += '<tr>';
            var priceTxt = '';

            if (showType === "Inventory") {
                priceTxt = '';
            } else {
                priceTxt = "(卖/底)"
            }
            html += '<th style="width:14.28%;">星期日' + priceTxt + '</th>';
            html += '<th style="width:14.28%;">星期一' + priceTxt + '</th>';
            html += '<th style="width:14.28%;">星期二' + priceTxt + '</th>';
            html += '<th style="width:14.28%;">星期三' + priceTxt + '</th>';
            html += '<th style="width:14.28%;">星期四' + priceTxt + '</th>';
            html += '<th style="width:14.28%;">星期五' + priceTxt + '</th>';
            html += '<th >星期六' + priceTxt + '</th>';
            html += '</tr>';
            html += '</thead>';
            html += '<tbody>';
            for (i = 0; i < data.CalendarWeek.length; i++) {
                html += "<tr>";
                for (j = 0; j < data.CalendarWeek[i].CalendarDate.length; j++) {
                    //判断当前周是否存在数据和数据审核状态

                    //获取当前周状态列表
                    var ItemStatus = this.getStatus(data, i);

                    //设置弹出框或自定义事件
                    if (func != null) {
                        html += "<td>";
                    }
                    else {
                        if (data.CalendarWeek[i].CalendarDate[j].Data.length > 0) {
                            html += "<td onclick='calendarClick(" + data.CalendarWeek[i].CalendarDate[j].Year + "," + data.CalendarWeek[i].CalendarDate[j].Month + "," + data.CalendarWeek[i].CalendarDate[j].Day + ")'>";
                        } else {
                            html += "<td>";
                        }
                    }
                    //设置年月栏
                    if (data.CalendarWeek[i].CalendarDate[j].Month != data.CurrCalendarMonth.Month) {
                        html += '<p class="day">' + data.CalendarWeek[i].CalendarDate[j].Month + '/' + data.CalendarWeek[i].CalendarDate[j].Day + '</p>';
                    } else {
                        html += '<p class="day">' + data.CalendarWeek[i].CalendarDate[j].Day + '</p>';
                    }
                    //存在数据
                    if (ItemStatus.length > 0) {
                        switch (ItemStatus.length) {
                            case 1:
                                html += this.setHtml(data, i, j, left, right, isapprove, func, ItemStatus, showType);
                                break;
                            case 2:
                                html += this.setHtml(data, i, j, left, right, isapprove, func, ItemStatus, showType);
                                break;
                        }
                    } else {
                        html += '<p class="none_border"></p>';
                    }
                    html += '</td>';
                }
                html += "</tr>"
            }
            html += '</tbody>';
            html += '</table>';
            html += '</div>';
            if (isapprove != null && isapprove) {
                // html += '<div class="reserve_state">审核状态：<span class="pas">通过</span><span class="audit">待审核</span><span class="refuse">拒绝</span></div>';
            }
            return html;
        },
        loadSchedule: function (data) {
            var currentDate = new Date();

            var html = '<div class="reserve_box">';
            html += '<ul class="reserve_datebox layoutfix">';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.PrevCalendarMonth.Year + ',' + data.PrevCalendarMonth.Month + ');" class="back"></a><span class="year">' + data.CurrCalendarMonth.Year + '年</span></li>';
            html += '<li class="current">' + data.CurrCalendarMonth.Month + '月</li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.CalendarMonth[1].Year + ',' + data.CalendarMonth[1].Month + ');">' + data.CalendarMonth[1].Month + '月</a></li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.CalendarMonth[2].Year + ',' + data.CalendarMonth[2].Month + ');">' + data.CalendarMonth[2].Month + '月</a></li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.CalendarMonth[3].Year + ',' + data.CalendarMonth[3].Month + ');">' + data.CalendarMonth[3].Month + '月</a></li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.CalendarMonth[4].Year + ',' + data.CalendarMonth[4].Month + ');">' + data.CalendarMonth[4].Month + '月</a></li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.CalendarMonth[5].Year + ',' + data.CalendarMonth[5].Month + ');">' + data.CalendarMonth[5].Month + '月</a></li>';
            html += '<li><a href="javascript:void(0)" onclick="loadCalendar(' + data.NextCalendarMonth.Year + ',' + data.NextCalendarMonth.Month + ');" class="go"></a></li>';
            html += '</ul>'
            html += '</div>'
            html += '<div class="basefix schedule_date">';
            html += '<div class="reserve_record">';
            for (i = 0; i < data.CalendarWeek.length; i++) {
                html += '<dl>';
                if (i == 0) {
                    html += '<dt class="month"><span>' + data.CurrCalendarMonth.Month + '</span>月</dt>';
                } else {
                    html += '<dt></dt>';
                }
                //获取当前周状态列表
                html += '<dd></dd>';
                html += '</dl>';
            }
            html += '</div>';
            html += '<table width="100%" class="reserve_table">';
            html += '<thead>';
            html += '<tr>';
            html += '<th style="width:90px;">星期日</th>';
            html += '<th style="width:90px;">星期一</th>';
            html += '<th style="width:90px;">星期二</th>';
            html += '<th style="width:90px;">星期三</th>';
            html += '<th style="width:90px;">星期四</th>';
            html += '<th style="width:90px;">星期五</th>';
            html += '<th style="width:90px;">星期六</th>';
            html += '</tr>';
            html += '</thead>';
            html += '<tbody>';
            for (i = 0; i < data.CalendarWeek.length; i++) {
                html += "<tr>";
                for (j = 0; j < data.CalendarWeek[i].CalendarDate.length; j++) {
                    html += "<td>";
                    //设置年月栏
                    if (data.CalendarWeek[i].CalendarDate[j].Month != data.CurrCalendarMonth.Month) {
                        html += '<p class="day">' + data.CalendarWeek[i].CalendarDate[j].Month + '/' + data.CalendarWeek[i].CalendarDate[j].Day + '</p>';
                    } else {
                        if ((currentDate.getFullYear() < data.CalendarWeek[i].CalendarDate[j].Year) ||
                            ((currentDate.getFullYear() == data.CalendarWeek[i].CalendarDate[j].Year) && (currentDate.getMonth() + 1) < data.CalendarWeek[i].CalendarDate[j].Month) ||
                            ((currentDate.getFullYear() == data.CalendarWeek[i].CalendarDate[j].Year) && (currentDate.getMonth() + 1) == data.CalendarWeek[i].CalendarDate[j].Month) && (currentDate.getDate() <= data.CalendarWeek[i].CalendarDate[j].Day)) {
                            if (data.CalendarWeek[i].CalendarDate[j].Data.length > 0) {
                                html += '<p class="day closeday"><a href="javascript:openSchedule(' + data.CalendarWeek[i].CalendarDate[j].Year + ',' + data.CalendarWeek[i].CalendarDate[j].Month + ',' + data.CalendarWeek[i].CalendarDate[j].Day + ');" class="btn btn_azure_ss">打开</a>' + data.CalendarWeek[i].CalendarDate[j].Day + '</p>';
                            }
                            else {
                                html += '<p class="day goday"><a href="javascript:closeSchedule(' + data.CalendarWeek[i].CalendarDate[j].Year + ',' + data.CalendarWeek[i].CalendarDate[j].Month + ',' + data.CalendarWeek[i].CalendarDate[j].Day + ');" class="btn btn_s">关闭</a>' + data.CalendarWeek[i].CalendarDate[j].Day + '</p>';
                            }
                        }
                        else {
                            html += '<p class="day closeday">' + data.CalendarWeek[i].CalendarDate[j].Day + '</p>';
                        }
                    }
                    if (data.CalendarWeek[i].CalendarDate[j].Data.length > 0) {
                        html += '<p class="none_border closedate">关闭</p>';
                    } else {
                        html += '<p class="none_border">出发</p>';
                    }
                    html += '</td>';
                }
                html += "</tr>";
            }
            html += '</tbody>';
            html += '</table>';
            html += '</div>';
            return html;
        },
        search: function (data, year, month, day) {
            for (i = 0; i < data.CalendarWeek.length; i++) {
                for (j = 0; j < data.CalendarWeek[i].CalendarDate.length; j++) {
                    if (data.CalendarWeek[i].CalendarDate[j].Year == year &&
                        data.CalendarWeek[i].CalendarDate[j].Month == month &&
                        data.CalendarWeek[i].CalendarDate[j].Day == day) {
                        return data.CalendarWeek[i].CalendarDate[j];
                    }
                }
            }
            return null;
        },
        getStatus: function (data, i) {
            var ItemStatus = [];
            for (k = 0; k < data.CalendarWeek[i].CalendarDate.length; k++) {
                for (l = 0; l < data.CalendarWeek[i].CalendarDate[k].Data.length; l++) {
                    if (ItemStatus.length == 0) {
                        ItemStatus[ItemStatus.length] = data.CalendarWeek[i].CalendarDate[k].Data[l].Status;
                    } else {
                        var ishav = false;
                        for (m = 0; m < ItemStatus.length; m++) {
                            if (ItemStatus[m] == 3 && data.CalendarWeek[i].CalendarDate[k].Data[l].Status == 3) {
                                ishav = true; break;
                            }
                            if (ItemStatus[m] != 3 && data.CalendarWeek[i].CalendarDate[k].Data[l].Status != 3) {
                                ishav = true; break;
                            }
                        }
                        if (!ishav) {
                            ItemStatus[m] = data.CalendarWeek[i].CalendarDate[k].Data[l].Status;
                        }
                    }
                }
            }
            return ItemStatus;
        },
        setHtml: function (data, i, j, left, right, isapprove, func, ItemStatus, showType) {
            var currentDate = new Date();

            var html = "";
            for (s = 0; s < ItemStatus.length; s++) {
                for (k = 0; k < data.CalendarCatetory.length; k++) {
                    var text = "";
                    for (l = 0; l < data.CalendarWeek[i].CalendarDate[j].Data.length; l++) {
                        if (data.CalendarWeek[i].CalendarDate[j].Data[l].ID == data.CalendarCatetory[k].ID) {
                            if ((ItemStatus.length == 1) ||
                            (ItemStatus.length > 1 && ((s == 0 && data.CalendarWeek[i].CalendarDate[j].Data[l].Status == 3) ||
                                (s != 0 && data.CalendarWeek[i].CalendarDate[j].Data[l].Status != 3)))) {
                                var item = data.CalendarWeek[i].CalendarDate[j].Data[l];
                                var leftV = "", rightV = "";
                                if (left != null) { eval('leftV = item.' + left); if (leftV == null) leftV = ""; }
                                if (right != null) { eval('rightV = item.' + right); if (rightV == null) rightV = ""; }
                                if (func == null) {
                                    if (showType !== "Inventory") {
                                        text = '<span class="l pas">' + leftV + '</span><span class="l pas">' + rightV + '</span>';
                                    }

                                    //加档期显示
                                    text = text + this.GetInventoryHtml(item.Inventory, showType);

                                } else {
                                    text = func(data.CalendarWeek[i].CalendarDate[j].Data[l]);
                                }
                            }
                        }
                    }
                    if ((ItemStatus.length == 1 && k == data.CalendarCatetory.length - 1) || (s == 1 && k == data.CalendarCatetory.length - 1)) {
                        if (showType == "Inventory") {
                            html += '<p style="text-align:center;">' + text + '</p>';
                        } else {
                            html += '<p class="none_border">' + text + '</p>';
                        }
                    } else {
                        if (showType == "Inventory") {
                            html += '<p style="text-align:center;">' + text + '</p>';
                        } else {
                            html += '<p>' + text + '</p>';
                        }
                    }
                }
            }
            return html;
        },
        GetInventoryHtml: function (inventory, showType) {
            var inventoryTxt = this.GetInventoryTxt(inventory, showType);
            var inventoryHtml = '';

            if (inventoryTxt != '')
                inventoryHtml = inventoryTxt;

            return inventoryHtml;
        },
        GetInventoryTxt: function (inventory, showType) {
            var rcss = '';
            if (showType !== "Inventory") {
                rcss = 'r';
            }
            if (inventory === 1)
                return '<span class="' + rcss + ' pas">' + '可订' + '</span>';
            else if (inventory === 0)
                return '<span class="' + rcss + '" style="color:red;font-weight:bold;">' + '不可订' + '</span>';
            else if (inventory === 2)
                return '<span class="' + rcss + '" style="color:#FF9933;font-weight:bold;">' + '暂订' + '</span>';
            else
                return '';
        }
    }

    //比较
    $.compare = {
        date: function (b, e, msg) {
            if (b == null && e == null) {
                return true;
            }
            if (new Date(Date.parse(b.replace(/-/g, "/"))) > new Date(Date.parse(e.replace(/-/g, "/")))) {
                $.showMessageBox(msg);
                return false;
            }
            return true;
        },
        dateMsg: function (b, e, msg) {
            if (b == null && e == null) {
                return true;
            }
            if (new Date(Date.parse(b.replace(/-/g, "/"))) > new Date(Date.parse(e.replace(/-/g, "/")))) {
                $.showMessageBox(msg);
                return false;
            }
            return true;
        },
        int: function (b, e, msg) {
            if (new Number(b) > new Number(e)) {
                $.showMessageBox(msg);
                return false;
            }
            return true;
        },

        number: function (b, e, msg) {
            if (Number(b) > Number(e)) {
                $.showMessageBox("最小" + msg + "不能大于最大" + msg + "!");
                return false;
            }
            return true;
        }
    }

    $.input = {
        init: function () {
            $("input[type='text']").each(function (i, e) {
                if ($(e).attr("id") != null) {

                    if (($(e).attr("id").indexOf("PMEID") != -1 || $(e).attr("id").indexOf("PAEID") != -1)) {
                        $(e).keyup(function () {
                            $(this).val($(this).val().replace("，", ",")); //全角逗号转换成半角
                            $(this).val($(this).val().replace("。", ".")); //全角句号转换成半角
                            $(this).val($(this).val().replace("＼", "、")); //全角顿号转换成半角
                            $(this).val($(this).val().replace("；", ";")); //全角逗号转换成半角
                            $(this).val($(this).val().replace(/[\u4e00-\u9fa5|;|\s]/g, '')); //过滤掉半角分号、空格以及任何汉字
                        }).bind("paste", function () {
                            $(this).val($(this).val().replace("，", ",")); //全角逗号转换成半角
                            $(this).val($(this).val().replace("。", ".")); //全角句号转换成半角
                            $(this).val($(this).val().replace("＼", "、")); //全角顿号转换成半角
                            $(this).val($(this).val().replace("；", ";")); //全角逗号转换成半角
                            $(this).val($(this).val().replace(/[\u4e00-\u9fa5|;|\s]/g, '')); //过滤掉半角分号、空格以及任何汉字
                        }).css("ime-mode", "disabled");
                    }


                    if (($(e).attr("id").indexOf("HotelIDs") != -1 || $(e).attr("id").indexOf("RoomIDs") != -1) ||
                        ($(e).attr("id").indexOf("LineTourIDs") != -1 || $(e).attr("id").indexOf("ResourceIDs") != -1)) {
                        $(e).keyup(function (event) {

                            if (event.keyCode >= 35 && event.keyCode <= 39) {
                            } else {
                                var isInt = /[\d\,]$/
                                if (!isInt.test($(this).val())) {
                                    $(this).val($(this).val().replace("，", ",")); //全角逗号转换成半角
                                    $(this).val($(this).val().replace(/[^\d\,]/g, ''));
                                }
                            }
                        }).bind("paste", function () {
                            var isInt = /[\d\,]$/
                            if (!isInt.test($(this).val())) {
                                $(this).val($(this).val().replace("，", ",")); //全角逗号转换成半角
                                $(this).val($(this).val().replace(/[^\d\,]/g, ''));
                            }
                        }).css("ime-mode", "disabled");
                    }

                    if ($(e).attr("id").indexOf("AdvanceBookingTime") != -1) {
                        $(e).css("ime-mode", "disabled");
                    }

                    if ($(e).attr("id").indexOf("MaxVoucherAmount") != -1 || $(e).attr("id").indexOf("MaxTravelDays") != -1 ||
                        $(e).attr("id").indexOf("MinTravelDays") != -1 || $(e).attr("id").indexOf("MinPersonQuantity") != -1 ||
                        $(e).attr("id").indexOf("MaxPersonQuantity") != -1 || $(e).attr("id").indexOf("EffectDays") != -1 || $(e).attr("id").indexOf("TransportationID") != -1 ||
                        $(e).attr("id").indexOf("SortNum") != -1 || $(e).attr("id").indexOf("Vendor_ID") != -1 || $(e).attr("id").indexOf("txtEditSortNum") != -1 || $(e).attr("id").indexOf("txtSelectedTagQuantityLimit") != -1 || $(e).attr("id").indexOf("txtSequenceNumber") != -1 || $(e).attr("id").indexOf("txtToProductID") != -1 || $(e).attr("id").indexOf("VendorID") != -1 ||
                        ($(e).attr("id").indexOf("HotelIDs") == -1 && $(e).attr("id").indexOf("HotelID") != -1) ||
                        ($(e).attr("id").indexOf("RoomIDs") == -1 && $(e).attr("id").indexOf("RoomID") != -1) || $(e).attr("id").indexOf("IResourceID") != -1
                        || $(e).attr("id").indexOf("OResourceID") != -1 || ($(e).attr("id").indexOf("ItemIDs") == -1 && $(e).attr("id").indexOf("ItemID") != -1)
                        || ($(e).attr("id").indexOf("ResourceIDs") == -1 && $(e).attr("id").indexOf("ResourceID") != -1)) {
                        $(e).keyup(function () {
                            $(this).val($(this).val().replace(/\D/g, '')); //只能输入正整数
                        }).bind("paste", function () {
                            $(this).val($(this).val().replace(/\D/g, '')); //只能输入正整数
                        }).css("ime-mode", "disabled");
                    }

                }
            });
        }
    }

    $.scroll = {
        init: function (element) {
            $(document.body).scroll(function () {
                var window_top = $(this).scrollTop();
                if (window_top >= obj_top) {
                    if (!$.browser.msie || ($.browser.msie && $.browser.version >= 7)) {
                        element.css({
                            position: "fixed",
                            top: 10
                        });
                    } else {
                        element.css({
                            position: "absolute",
                            top: window_top + 10
                        });
                    }
                } else {
                    element.css("position", "static");
                }
            });
        }
    }

    $.calender = {
        init: function () {
            $("input[type='text']").each(function (i, e) {
                if ($(e).attr("mod") == null || $(e).attr("mod").indexOf("calendar") == -1) {
                    return;
                }
                var datafmt = 'yyyy-MM-dd';
                if ($(e).attr("datafmt") != null && $(e).attr("datafmt")) {
                    datafmt = $(e).attr("datafmt");
                }
                $(e).bind("click", function () { WdatePicker(e, { doubleCalendar: true, dateFmt: datafmt }); });
                $(e).bind("keydown", function () { $(e).val(""); return false; });
            });
        }
    }

    $.fn.extend({
        cPaging: function (p) {

            var pager = {
                //控件对象
                controls: {},
                option: {},
                renderLeft: function (srcEle, total) {
                    var lhtml = "";
                    lhtml += "<li class=\"left\">共<span>" + total + "</span>条&nbsp;&nbsp;每页显示&nbsp;";
                    lhtml += " <select style=\"width: 50px;\">";
                    lhtml += "     <option value='10'>10</option>";
                    lhtml += "     <option value='20' selected='selected'>20</option>";
                    lhtml += "     <option value='50'>50</option>";
                    lhtml += "     <option value='100'>100</option>";
                    lhtml += " </select>&nbsp;条</li>";
                    pager.option.srcEle.html(lhtml);

                    pager.controls.selPageSize = srcEle.find("select");        //分页大小选择下拉框

                    //隐藏的分页参数
                    pager.controls.selPageSize.val(pager.controls.hid_pageSize.val() || 50);

                },
                bind: function (f, args, scope) {
                    scope = scope || window;
                    return function () {

                        return f.call(scope, args);
                    }
                },

                //分页Render
                renderRight: function (totalCount, pageNo, pageSize, totoalPageSize, srcEle) {

                    var rhtml = "";
                    rhtml += "<li class=\"right\"><div class=\"c_page\"><a class=\"c_up\" href=\"javascript:void(0);\">上一页</a>";
                    rhtml += "<div class=\"c_page_list layoutfix\">";
                    if (eval(pageNo) > 5) {

                        rhtml += "<a href=\"javascript:void(0)\">1</a>";
                        rhtml += "<span class=\"c_page_ellipsis\">...</span>";
                        for (var i = 0; i < 3; i++) {
                            rhtml += "<a href=\"javascript:void(0)\">" + (pageNo - 3 + i) + "</a>";
                        }
                        rhtml += "<a href=\"javascript:void(0)\" class=\"current\">" + (pageNo - 3 + i) + "</a>";
                    }
                    else {
                        for (var i = 0; i < pageNo; i++) {
                            if (i + 1 == pageNo) {
                                rhtml += "<a href=\"javascript:void(0)\" class=\"current\">" + (i + 1) + "</a>";
                            }
                            else {
                                rhtml += "<a href=\"javascript:void(0)\">" + (i + 1) + "</a>";
                            }
                        }
                    }
                    if (eval(pageNo) + 3 < eval(totoalPageSize) - 1) {
                        for (var i = 0; i < 3; i++) {
                            rhtml += "<a href=\"javascript:void(0)\">" + (pageNo + i + 1) + "</a>";
                        }
                        rhtml += "<span class=\"c_page_ellipsis\">...</span>";
                        rhtml += "<a href=\"javascript:void(0)\">" + totoalPageSize + "</a>";
                    }
                    else {
                        for (var i = 0; i < eval(totoalPageSize - pageNo) ; i++) {
                            rhtml += "<a href=\"javascript:void(0)\">" + (pageNo + i + 1) + "</a>";
                        }
                    }
                    rhtml += "</div>";
                    rhtml += "<a class=\"c_down\" href=\"javascript:void(0);\">下一页</a>";
                    rhtml += "<div class=\"c_pagevalue\">到<input type=\"text\" name=\"\" class=\"c_page_num\"  value=\"" + pageNo + "\" />页<input type=\"button\" name=\"\" value=\"确定\" class=\"c_page_submit\"  /></div></div><li>";

                    if (srcEle.find("li.right").length)
                        srcEle.find("li.right").html(rhtml);
                    else
                        srcEle.append(rhtml);



                    pager.controls.container = srcEle;      //容器ul class="info_inall layoutfix"
                    pager.controls.inputPageNum = srcEle.find("input.c_page_num");   //输入页数框
                    pager.controls.pageUp = srcEle.find("a.c_up");      //上一页按钮
                    pager.controls.pageDown = srcEle.find("a.c_down");      //下一页按钮
                    pager.controls.btnGo = srcEle.find("input.c_page_submit");      //跳转到指定页



                }, //end of drawPaging


                //分页Action
                drawPagingAction: function (pageNo, totoalPageSize, pageSize) {

                    pager.controls.container.find("div.c_page>a.c_down,div.c_page>a.c_up,div.c_page_list.layoutfix>a").click(function () {
                        var cName = $(this).attr("class");

                        switch (cName) {
                            case "c_up":
                                if (pageNo <= 1) return;
                                pager.controls.hid_pageNo.val(--pageNo);
                                break;
                            case "c_down":
                                if (pageNo >= totoalPageSize) return;
                                pager.controls.hid_pageNo.val(++pageNo);
                                break;
                            default:
                                pager.controls.hid_pageNo.val($(this).text());
                                break;
                        }
                        pager.controls.hid_pageSize.val(pageSize);
                        if (pager.option.callBack) {
                            var func = pager.bind(pager.option.callBack, pager.option.callBackArgs, null);

                            func();
                        }


                    });


                    //确定按钮
                    pager.controls.btnGo.click(function () {
                        if (pager.controls.inputPageNum.val() == "") {
                            alert("请输入页码数");
                            pager.controls.inputPageNum.focus();
                            return;
                        }
                        var isIntVal = /^[0-9]*[1-9][0-9]*$/;

                        if (!isIntVal.test(pager.controls.inputPageNum.val())) {
                            alert("请输入正确的页码!");
                            return;
                        }
                        if (Number(pager.controls.inputPageNum.val()) > pager.option.totoalPageSize) {
                            alert("请输入有效范围的页码!");
                            return;
                        }


                        pager.controls.hid_pageSize.val(pager.controls.selPageSize.val());
                        pager.controls.hid_pageNo.val(pager.controls.inputPageNum.val());

                        if (pager.option.callBack) {
                            var func = pager.bind(pager.option.callBack, pager.option.callBackArgs, null);

                            func();
                        }
                    });

                    //用户改变分页大小
                    pager.controls.selPageSize.change(function () {
                        pager.controls.hid_pageSize.val($(this).val());
                        pager.controls.hid_pageNo.val(pager.controls.inputPageNum.val());

                        if (pager.option.totalCount / Number($(this).val()) < Number(pager.controls.inputPageNum.val())) {
                            pager.controls.hid_pageNo.val(1);
                        }

                        if (pager.option.callBack) {
                            var func = pager.bind(pager.option.callBack, pager.option.callBackArgs, null);

                            func();
                        }
                    });

                },

                init: function (p, srcEle) {
                    var isPaging = true,
                        totalNum = p.totalCount;

                    pager.option = p || { totalCount: 100, pageNo: 1, pageSize: 50, callBack: null, callBackArgs: null };



                    pager.option.srcEle = srcEle;
                    pager.controls.hid_pageNo = $('input[name$="PageNo"]');         //隐藏控件，页号
                    pager.controls.hid_pageSize = $('input[name$="PageSize"]');     // 隐藏控件，分页大小

                    var pagesizeVal = pager.controls.hid_pageSize.val();

                    if (pager.option.totalCount < 1 || pager.option.pageNo < 1) {
                        isPaging = false;
                    }

                    pager.option.totoalPageSize = Math.ceil(pager.option.totalCount / pagesizeVal);


                    if (pager.option.pageNo > pager.option.totoalPageSize) {
                        isPaging = false;
                    }

                    if (isPaging) {
                        pager.renderLeft(pager.option.srcEle, pager.option.totalCount);
                        pager.renderRight(pager.option.totalCount, pager.option.pageNo, pager.option.pageSize, pager.option.totoalPageSize, pager.option.srcEle);
                        pager.drawPagingAction(pager.option.pageNo, pager.option.totoalPageSize, pager.option.pageSize);
                    }

                }
            };


            pager.init(p, $(this));



        } //end of extend
    });

})(jQuery);


function AssociateMagr(ctlid, ctlname, div, api, data, objexpname, ctlctyID, ctlctyName, ctScenicSpotID) {
    this.GlobalID = $(ctlid).val();
    this.GlobalName = $(ctlname).val();
    this.Event = null;

    this.Associate = function (obj) {
        //拼接对象
        var data1 = "{";
        for (var k in data) {
            var v = data[k];
            if (typeof (v) != "object") { data1 += k + ":\"" + v + "\","; }
            else { data1 += k + ":\"" + v.val() + "\","; }
        }
        data1 = data1.substr(0, data1.length - 1);
        data1 += "}";

        var id = ctlid.val();
        var name = ctlname.val();
        if ($.trim(name) == "") { return false; }

        $.ajaxPost(api, eval("(" + data1 + ")"), function (response) {
            $(div).find(".c_address_list").empty();
            if (response.IsSuccess) {
                $.each(response.Data, function (i, e) {
                    var html = $("<a href='javascript:void(0)'>" + e.Name + "</a>");
                    //                    if (e.Name != null && e.Name != "") {
                    //                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em></a>");
                    //                    }
                    if (e.ScenicSpotName != null && e.ScenicSpotName != "") {
                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em>/(" + e.ScenicSpotName + ")</em></a>");
                    }
                    else if (e.ProvinceName != null && e.ProvinceName != "") {
                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em>(" + e.ProvinceName + ")</em></a>");
                    }
                    else if (e.CountryName != null && e.CountryName != "") {
                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em>(" + e.CountryName + ")</em></a>");
                    }
                    else if (e.Area != null && e.Area.Name != "") {
                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em>(" + e.Area.Name + ")</em></a>");
                    }
                    //                    else if (e.ScenicSpotName != null && e.ScenicSpotName != "") {
                    //                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em>/(" + e.ScenicSpotName + ")</em></a>");
                    //                    }
                    if (e.DistrictName != null && e.DistrictName != "") {
                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em>/(" + e.DistrictName + ")</em></a>");
                    }

                    if (e.HotelName != null && e.HotelName != "") {
                        html = $("<a href='javascript:void(0)'>" + e.HotelName + "<em>/(" + e.HotelName + ")</em></a>");
                    }

                    //                    else {
                    //                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em>(" + e.ProvinceName + ")</em></a>");
                    //                    }

                    //                    if (e.ProvinceName != null && e.ProvinceName != "") {
                    //                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em>(" + e.ProvinceName + ")</em></a>");
                    //                    } else if (e.CountryName != null && e.CountryName != "") {
                    //                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em>(" + e.CountryName + ")</em></a>");
                    //                    } else if (e.Area != null && e.Area.Name != "") {
                    //                        html = $("<a href='javascript:void(0)'>" + e.Name + "<em>(" + e.Area.Name + ")</em></a>");
                    //                    }


                    $(div).find(".c_address_list").append(html);
                    html.click(function () {
                        if (e.ScenicSpotName != null && e.ScenicSpotName != "") {
                            var temp = e.Name + "/" + e.ScenicSpotName;
                            $(ctlname).val(temp);
                        } else if (e.ProvinceName != null && e.ProvinceName != "") {
                            $(ctlname).val(e.Name);
                        }
                        else if (e.CountryName != null && e.CountryName != "") {
                            $(ctlname).val(e.Name);
                        } else if (e.Area != null && e.Area.Name != null && objexpname != null) {
                            $(ctlname).val(e.Name);
                            $(objexpname).html(e.Area.Name);
                        }

                        else if (e.HotelName != null) {
                            $(ctlname).val(e.HotelName);
                            $("#hiddenHotelName").val(e.HotelName);
                            //$(objexpname).html(e.Area.Name);
                        }

                        else {
                            $(ctlname).val(e.Name);
                        }


                        //                        if (e.ProvinceName != null && e.ProvinceName != "") {
                        //                            $(ctlname).val(e.Name);
                        //                        } else if (e.CountryName != null && e.CountryName != "") {
                        //                            $(ctlname).val(e.Name);
                        //                        } else if (e.Area != null && e.Area.Name != null && objexpname != null) {
                        //                            $(ctlname).val(e.Name);
                        //                            $(objexpname).html(e.Area.Name);
                        //                        }
                        //                        else {
                        //                            $(ctlname).val(e.Name);
                        //                        }


                        $(ctlid).val(e.ID);
                        if (e.CountryID != null) {
                            $(ctlctyID).val(e.CountryID);
                            if ($(ctlctyName).length > 0) {
                                if ($(ctlctyName).attr("id") == "ResourceModel_DestinationCountry_Name") {//如果是资源维护页
                                    var res_cus_temp = $("#ResourceModel_CustomerInfoTemplateID");
                                    if (res_cus_temp.length > 0) {
                                        if (e.CountryID <= 0) {
                                            ctlctyName.html("");
                                        }
                                        LoadCustomerInfoTemplate();
                                    }
                                }
                            }
                        }

                        if (e.ScenicSpotID != null) {
                            $(ctScenicSpotID).val(e.ScenicSpotID);
                        }

                        if (e.CountryName != null && e.CountryName != "") {
                            $(ctlctyName).html(e.CountryName);
                        }

                        if (e.HotelID != null) {
                            $(ctScenicSpotID).val(e.HotelID);
                            $("#HotelID").val(e.HotelID);
                        }

                        $(div).hide();
                        obj.GlobalName = e.Name;
                        obj.GlobalID = e.ID;
                        return false;
                    });
                });
            }
        }, false);
        $(div).css({
            "position": "absolute",
            "left": $(ctlname).offset().left,
            "top": $(ctlname).offset().top + 20,
            "z-index": 9999
        }).show();
    }
    $(ctlname).bind('keyup', { obj: this }, function (event) {
        if (event.keyCode == 38) {
            if ($(div).find("a").length > 0) {
                var hover = $(div).find("a[class='hover']");
                if (hover.length > 0) {
                    if ($(hover).prev().length > 0) {
                        $(hover).removeClass("hover");
                        $(hover).removeAttr("style");
                        $(hover).prev().addClass("hover");
                        $(hover).prev().css("background-color", "#E8F4FF");
                    }
                } else {
                    $($(div).find("a")[0]).css("background-color", "#E8F4FF");
                    $($(div).find("a")[0]).addClass("hover");
                }
            }
        } else if (event.keyCode == 40) {
            if ($(div).find("a").length > 0) {
                var hover = $(div).find("a[class='hover']");
                if (hover.length > 0) {
                    if ($(hover).next().length > 0) {
                        $(hover).removeClass("hover");
                        $(hover).removeAttr("style");
                        $(hover).next().addClass("hover");
                        $(hover).next().css("background-color", "#E8F4FF");
                    }
                } else {
                    $($(div).find("a")[0]).css("background-color", "#E8F4FF");
                    $($(div).find("a")[0]).addClass("hover");
                }
            }
        } else if (event.keyCode == 13) {
            if ($(div).find("a").length > 0) {
                var hover = $(div).find("a[class='hover']");
                if (hover.length > 0) {
                    $(hover).click();
                }
            }
        } else {
            clearTimeout(event.data.obj.Event);
            event.data.obj.Event = setTimeout(function () { event.data.obj.Associate(event.data.obj); }, 500);
        }
    });

    $(ctlname).bind('blur', { obj: this }, function (event) {
        if ($(this).val() == "" || ($(this).val() == $(this).attr("mod-notice-tip"))) {
            event.data.obj.GlobalName = "";
            $(ctlid).val("");
            if ($(ctlctyID).length > 0) {
                $(ctlctyID).val("");
            }

            if ($(ctScenicSpotID).length > 0) {
                $(ctScenicSpotID).val("");
            }

            if ($(ctlctyName).length > 0) {
                $(ctlctyName).html("");
            }
            if ($(objexpname).length > 0) {
                $(objexpname).html("");
            }
        }
        if ($(ctlid).val() == "") {
            if ($(this).attr("mod-notice-tip")) {
                $(this).val($(this).attr("mod-notice-tip"));
            } else {
                $(this).val("");
            }
        }
        //        if (event.data.obj.GlobalName != "") {
        //            $(this).val(event.data.obj.GlobalName);
        //            $(ctlid).val(event.data.obj.GlobalID);
        //        }
    });

    return this;
}

//根据需求,在文本框输入后,按回车,直接查询
function EnterPress(e, btnID) { //传入 event和要获得焦点的控件,通用
    var e = e || window.event;
    if (e.keyCode == 13) {
        $("#" + btnID).focus();
    }
}
