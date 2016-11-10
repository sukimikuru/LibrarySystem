var change = {
    verify: function () {
        if ($(window.frames[0].document).find('#pwd').val() == '') {
            layer.msg("旧密码不能为空。", { icon: 9, time: 1000 });
            $(window.frames[0].document).find('#pwd').focus();
            return false;
        } else if ($(window.frames[0].document).find('#pwd1').val().length < 6) {
            layer.msg("新密码长度必须大于等于6。", { icon: 9, time: 1000 });
            $(window.frames[0].document).find('#pwd1').focus();
            return false;
        } else if ($(window.frames[0].document).find('#pwd1').val() != $(window.frames[0].document).find('#pwd2').val()) {
            layer.msg("两次密码不一致。", { icon: 9, time: 1000 });
            $(window.frames[0].document).find('#pwd2').focus();
            return false;
        }
        return true;
    },
    save: function () {
        if (change.verify()) {
            var index = layer.load();

            $.ajax({
                type: 'POST',
                url: '/home/savepwd',
                data: {
                    pwd: $(window.frames[0].document).find('#pwd').val(), pwd1: $(window.frames[0].document).find('#pwd1').val()
                },
                dataType: 'json',
                success: function (v) {
                    if (v.result == "1") {
                        layer.closeAll();
                        layer.msg("密码修改成功。", { icon: 8, time: 1000 });
                    }
                    else if (v.result == "2") {
                        layer.msg("原密码有误。", { icon: 9, time: 1000 });
                    }
                    else {
                        layer.msg("密码修改失败。", { icon: 9, time: 1000 });
                    }
                },
                error: function () {
                    layer.msg("密码修改失败。", { icon: 9, time: 1000 });
                },
                complete: function () { layer.close(index); }
            });
        }

    }

};

(function () {
    window['_'] = {
        isIE6_8: function () {
            var result = false;
            if (navigator.userAgent.indexOf("MSIE") > 0) {
                if (navigator.userAgent.indexOf("MSIE 6.") > 0) {
                    result = true;
                }
                else if (navigator.userAgent.indexOf("MSIE 7.") > 0) {
                    result = true;
                }
                else if (navigator.userAgent.indexOf("MSIE 8.") > 0) {
                    result = true;
                }
            }
            return result;
        },
        changepwd: function () {
            layer.open({
                id: 'changepwd',
                title: '修改密码',
                type: 2,
                area: ['450px', '330px'],
                shade: [0.5, '#000'],
                maxmin: false,
                content: '/home/changepwd',
                btn: ['确定', '取消'],
                yes: function (index, layero) {
                    change.save();
                },
                btn2: function (index, layero) {
                    layer.close(index);
                }


            });
        }

    }
})();

(function () {
    //用户修改用户自己
    window['_user'] = {
        detail: function (obj, row_key) {
            var index = layer.load();
            $.ajax({
                type: 'POST',
                url: '/user/UserDetail',
                data: { row_key: row_key, type: 2 },
                dataType: 'json',
                success: function (v) {
                    if (v.result == "1") {
                        $('#user-detail-div').html(v.html);
                    }
                },
                complete: function () {
                    layer.close(index);
                    $(obj).next().click();
                    $('.close-user-detail').click(function () {
                        closeImgSelection();
                    });
                }
            });
        },
        saveUser: function (obj) {
            closeImgSelection();

            var index = layer.load();

            $('#user_form').ajaxSubmit({
                success: function (v) {
                    if (v.result == "1") {

                        layer.msg("保存用户成功。", { icon: 8, time: 1000 });
                        $(obj).next().click();

                        //如果修改的是自己的,更新右上角
                        if (v.update_cookie == "1") {
                            $('#myinfo-headimg').attr('src', v.head_img);
                            $('#myinfo-nickname').html(v.nick_name);
                        }

                    }
                    else if (v.result == "2") {
                        layer.msg("用户名重复。", { icon: 9, time: 1000 });
                    }
                    else {
                        layer.msg("保存用户失败。", { icon: 9, time: 1000 });
                    }
                },
                error: function (e) {
                    alert(e);
                },
                failure: function () {
                    layer.msg("调用方法失败。", { icon: 9, time: 1000 });
                },
                complete: function () {
                    layer.close(index);
                }
            })
        },
        readURL: function (input) {
            var strSrc = $("#loadFile").val();
            //验证上传文件格式是否正确    
            var pos = strSrc.lastIndexOf(".");
            var lastname = strSrc.substring(pos, strSrc.length);
            if (lastname.toLowerCase() != ".jpg" && lastname.toLowerCase() != ".png" && lastname.toLowerCase() != ".jpeg" && lastname.toLowerCase() != ".bmp" && lastname.toLowerCase() != ".gif") {
                layer.alert("您上传的文件类型为" + lastname + "，图片必须为 JPG、JPEG、PNG、BMP、GIF 类型", { icon: 0 });
                return false;
            }
            if (_.isIE6_8()) {
                $('.isIE').val("1");
                input.select();
                var reallocalpath = document.selection.createRange().text;
                $('#user_head_upload_box').hide();
                $('#user_head_show_box').show();
                document.getElementById('user_head_origin').style.width = 200;
                document.getElementById('user_head_origin').style.height = 200;
                document.getElementById('user_head_origin').style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='image',src=\"" + reallocalpath + "\")";
                document.getElementById('user_head_origin').src = 'data:image/gif;base64,R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==';//设置img的src为base64编码的透明图片，要不会显示红xx
                $('#head_x').val("0");
                $('#head_y').val("0");
                $('#head_width').val("200");
                $('#head_height').val("200");
            } else {
                //验证上传文件是否超出了大小    +
                if (input.files[0].size / 1024 > 150) {
                    layer.alert("您上传的文件大小超出了150K限制", { icon: 0 });
                    return false;
                }
                $('#user_head_upload_box').hide();
                $('#user_head_show_box').show();
                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        UserHeadUtil.initialize(e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                }
            }
        },
    }
})();

(function () {
    window['_wu'] = {
        extensions: 'doc,docx,xls,xlsx,ppt,pptx,pdf,txt,mp4,mp3,avi,wmv,swf,gif,jpg,jpeg,bmp,png',
        mimeTypes: '*',
        title: '自定义文件',
        maxNum: 10,
        uploader: null,
        uploadSuccessed: null,
        uploadBeginTime: null,
        uploaded: null,
        callback: null,
        addFile: function (file) {
            var $li = $('<li id="' + file.id + '">' +
                            '<div class="mc"><span class="title">' + file.name + '</span>' +
                                '<a href="javascript:_wu.cancel(\'' + file.id + '\');" class="gb fr"></a>' +
                                '<a href="javascript:_wu.stop(\'' + file.id + '\');" class="zt hide fr"></a>' +
                                '<a href="javascript:_wu.upload(\'' + file.id + '\');" class="sc fr"></a>' +
                             '</div>' +
                             '<div class="scjdt">' +
                                '<div class="jd" style="width:0px;"></div>' +
                            '</div>' +
                            '<div class="sd">' +
                                 '<span>大小：<label class="totalSize">' + WebUploader.formatSize(file.size, 2) + '</label></span>' +
                                '<span>已上传：<label class="uploadSize">0</label></span>' +
                                '<span>速度：<label class="speed">0</label></span>' +
                                '<span>剩余时间：<label class="levelTime">00:00:00</label></span>' +
                             '</div>' +
                           '</li>');
            $('.doc_up_list ul').append($li);
            $('.doc_up_list').show();
            $('.doc_up_area').removeClass('no_doc');
        },
        div: function (exp1, exp2) {
            var n1 = Math.round(exp1); //四舍五入   
            var n2 = Math.round(exp2); //四舍五入  

            var rslt = n1 / n2; //除  

            if (rslt >= 0) {
                rslt = Math.floor(rslt); //返回小于等于原rslt的最大整数。   
            }
            else {
                rslt = Math.ceil(rslt); //返回大于等于原rslt的最小整数。   
            }
            return rslt;
        },
        prefix: function (size, num) {
            var sLen = ('' + num).length;
            if (sLen >= size) {
                return '' + num;
            }
            var preZero = (new Array(size)).join('0');
            return preZero.substring(0, size - sLen) + num;
        },
        formatTime: function (tiff) {
            tiff = parseInt(tiff);
            hour = _wu.div(tiff, 3600);
            min = _wu.div((tiff - hour * 3600), 60);
            sec = tiff % 60;
            return _wu.prefix(2, hour) + ":" + _wu.prefix(2, min) + ":" + _wu.prefix(2, sec);
        },
        upload: function (fileId) {
            if (_wu.checkSpace()) {
                if (fileId != '') {
                    _wu.uploadBeginTime[fileId] = new Date();
                    _wu.uploader.upload(fileId);
                    $('#' + fileId + " .sc").hide();
                } else {
                    var files = _wu.uploader.getFiles();
                    for (i = files.length - 1; i >= 0; i--) {
                        _wu.uploadBeginTime[files[i].id] = new Date();
                        $('#' + files[i].id + " .sc").hide();
                    }
                    _wu.uploader.upload();
                }
            }
        },
        stop: function (fileId) {
            if (fileId != '') {
                _wu.uploader.stop(fileId);
                $('#' + fileId + " .zt").hide();
                $('#' + fileId + " .sc").show();
            } else {
                _wu.uploader.stop();
                var files = _wu.uploader.getFiles();
                for (i = files.length - 1; i >= 0; i--) {
                    _wu.uploadBeginTime[files[i].id] = new Date();
                    $('#' + files[i].id + " .zt").hide();
                    $('#' + files[i].id + " .sc").show();
                }
            }
        },
        cancel: function (fileId) {
            if (fileId != '') {
                if ($('#' + fileId).attr("name") != "" && $('#' + fileId).attr("name") != 'undefined') {
                    _wu.del(fileId);
                }
                var file = _wu.uploader.getFile(fileId);
                _wu.uploader.removeFile(file, true);
                $('#' + fileId).remove();
                if (_wu.uploaded.contains(fileId)) {
                    _wu.uploaded.remove(fileId, true);
                }
            } else {
                var len = $('.doc_up_list ul li').length;
                for (var i = len - 1; i >= 0; i--) {
                    fileId = $('.doc_up_list ul li').eq(i).attr("id");
                    if ($('#' + fileId).attr("name") != "" && $('#' + fileId).attr("name") != 'undefined') {
                        _wu.del(fileId);
                    }
                    var file = _wu.uploader.getFile(fileId);
                    _wu.uploader.removeFile(file, true);
                    $('#' + fileId).remove();
                }
            }
            _wu.setUploadedInfo();
            if ($('.doc_up_list ul li').length > 0)
                $('#file_count').html("已选" + $('.doc_up_list ul li').length + "个文件");
            else
                $('#file_count').html('没有上传文件');

        },
        del: function (fileId) {
            if (_wu.uploaded.contains(fileId)) {
                path = _wu.uploaded.items(fileId).split('|')[1];
                $.ajax({
                    type: 'POST',
                    url: '/home/delfile',
                    data: { path: path },
                    dataType: 'json',
                    success: function (v) {

                    },
                    error: function () {
                    }
                });
            }
        },
        checkFilesStatus: function () {
            //var successNum = _wu.uploader.getStats().successNum;
            //var cancelNum = _wu.uploader.getStats().cancelNum;

            //var filesCount = successNum - cancelNum;
            //if (filesCount == 0) {
            //    return false;
            //}
            //if (filesCount != _wu.uploaded.count()) {
            //    return false;
            //}
            if (_wu.uploader.getStats().progressNum > 0 || _wu.uploader.getStats().queueNum > 0 || _wu.uploader.getStats().successNum == 0)
                return false;
            return true;
        },
        setUploadedInfo: function () {
            var str = _wu.uploaded.joinValue(',');
            $('#upload_infos').val(str);

            //如果资源没有命名，就用文件名
            if (str.split('|').length > 0 && $('#name').val() == '') {
                var full_name = str.split('|')[0];
                $('#name').val(full_name);
            }

            if (_wu.checkFilesStatus() && _wu.uploadSuccessed != null) {
                _wu.uploadSuccessed();
            }
        },
        init: function (ext, mimeTypes, title, maxNum) {
            if (ext != undefined && ext != null) {
                _wu.extensions = ext;
            }
            if (mimeTypes != undefined && mimeTypes != null) {
                _wu.mimeTypes = mimeTypes;
            }
            if (title != undefined && title != null) {
                _wu.title = title;
            }
            if (maxNum != undefined && maxNum != null) {
                _wu.maxNum = maxNum;
            }
            $('#file_count').html('没有上传文件');
            _wu.uploaded = new Hashtable();
            _wu.uploadBeginTime = new Array();
            if (_wu.uploader != null) {
                _wu.uploader.destroy();
            }
            _wu.uploader = WebUploader.create({
                // swf文件路径
                swf: '/theme/webuploader/Uploader.swf',
                // 文件接收服务端。
                server: '/home/fileupload',
                dnd: '.doc_up_area',
                disableGlobalDnd: true,
                auto: true,
                pick: '#picker',
                fileNumLimit: _wu.maxNum,
                fileSizeLimit: 1024 * 1024 * 1024,
                fileSingleSizeLimit: 1024 * 1024 * 1024,
                accept: {
                    //title: _wu.title,
                    extensions: _wu.extensions
                    //,mimeTypes: _wu.mimeTypes
                }
            });
            _wu.uploader.onBeforeFileQueued = function (file) {
                if (_wu.extensions != "" && _wu.extensions.indexOf(file.ext.toLowerCase()) <= -1) {
                    layer.msg(file.name + "格式不符", { icon: 0, time: 1000 });
                }
            };
            _wu.uploader.onFilesQueued = function (files) {
                if (files != null) {
                    for (i = 0; i < files.length; i++) {
                        _wu.addFile(files[i]);
                    }
                    $('#file_count').html("已选" + $('.doc_up_list ul li').length + "个文件");
                }
            };
            _wu.uploader.onUploadProgress = function (file, percentage) {
                var $li = $('#' + file.id);
                $li.find('.scjdt .jd').css('width', percentage * 513 + "px");
                $li.find('.sd .uploadSize').html(WebUploader.formatSize(percentage * file.size, 2));
                var date = new Date();
                tiff = (date - _wu.uploadBeginTime[file.id]) / 1000;
                speed = 0;
                if (tiff > 0) {
                    speed = percentage * file.size / tiff;
                }
                left_time = 0;
                if (speed > 0) {
                    left_time = (1 - percentage) * file.size / speed;
                }
                $li.find('.sd .speed').html(WebUploader.formatSize(speed, 2));
                $li.find('.sd .levelTime').html(_wu.formatTime(left_time));

            };
            _wu.uploader.onUploadError = function (file) {
            };
            _wu.uploader.onUploadSuccess = function (file, data) {
                if (data.result == "1") {
                    percentage = 1;
                    var $li = $('#' + file.id);
                    $li.attr("name", data.docKey);
                    $li.find('.scjdt .jd').css('width', percentage * 513 + "px");
                    $li.find('.sd .uploadSize').html(WebUploader.formatSize(percentage * file.size, 2));
                    var date = new Date();
                    tiff = (date - _wu.uploadBeginTime[file.id]) / 1000;
                    speed = 0;
                    if (tiff > 0) {
                        speed = percentage * file.size / tiff;
                    }
                    left_time = 0;
                    if (speed > 0) {
                        left_time = (1 - percentage) * file.size / speed;
                    }
                    $li.find('.sd .speed').html(WebUploader.formatSize(speed, 2));
                    $li.find('.sd .levelTime').html(_wu.formatTime(left_time));
                    if (_wu.uploaded.contains(file.id)) {
                        _wu.uploaded.remove(file.id);
                    }
                    _wu.uploaded.add(file.id, file.name + "|" + data.path);
                    _wu.setUploadedInfo();
                    //add by ywd 2016-1-14
                    if (_wu.callback != '' && _wu.callback != null) {
                        eval(_wu.callback + "('" + file.name + "')");
                    }

                } else {
                    var html = $('#' + file.id).find('.title').html();
                    $('#' + file.id).find('.title').html(html + '<b style="color:red;">上传失败</b>');
                }
            };
            _wu.uploader.onUploadComplete = function (file) {
                var $li = $('#' + file.id);
                $li.find('.mc .zt').remove();
                $li.find('.mc .sc').remove();
                $li.find('.scjdt').remove();
            };
            _wu.uploader.onUploadStart = function (file) {
                _wu.uploadBeginTime[file.id] = new Date();
            }
            _wu.uploader.onUploadBeforeSend = function (object, data, headers) {
                data.isCover = $('#isCover').is(':checked') ? "True" : "False";
            };
            $('.doc_up_list ul').empty();
            $('.doc_up_list').hide();
        },
        showBtn: function (left, top) {
            setTimeout(function () {
                $('.webuploader-element-invisible').parent("div").css({
                    "left": left,
                    "top": top
                });
            }, 500);
        }
    }
})();

$(function () {
    $("input:password").bind("copy cut paste", function (e) {
        return false;
    });

    checkBeLogOut();
});

function Hashtable() {
    this._hash = {};
    this._count = 0;
    this.add = function (key, value) {
        if (this._hash.hasOwnProperty(key)) return false;
        else { this._hash[key] = value; this._count++; return true; }
    }
    this.set = function (key, value) {
        if (!this._hash.hasOwnProperty(key)) return false;
        else { this._hash[key] = value; return true; }
    }
    this.remove = function (key) { delete this._hash[key]; this._count--; }
    this.count = function () { return this._count; }
    this.items = function (key) { if (this.contains(key)) return this._hash[key]; else return null; }
    this.contains = function (key) { return this._hash.hasOwnProperty(key); }
    this.clear = function () { this._hash = {}; this._count = 0; }
    this.each = function (func) { $.each(this._hash, function (n, value) { return func(n, value) }); }
    this.joinValue = function (spliter) {
        str = "";
        $.each(this._hash, function (n, value) {
            if (str == "") {
                str = value;
            } else {
                str += spliter + value;
            }
        });
        return str;
    }
    this.joinKey = function (spliter) {
        str = "";
        $.each(this._hash, function (n, value) {
            if (str == "") {
                str = n;
            } else {
                str += spliter + n;
            }
        });
        return str;
    }
}

function checkBeLogOut() {
    $.ajax({
        type: 'POST',
        url: '/Home/BeLogOut',
        data: {},
        dataType: 'json',
        success: function (v) {
            if (v.result == '1') {
                layer.alert(v.msg, { closeBtn: 0 }, function () { location.href = '/home/login' });
            }
            else {
                setTimeout(function () { checkBeLogOut(); }, 2000);
            }

        },
        complete: function (XMLHttpRequest, status) {
            if (status != 'success') {
                setTimeout(function () { checkBeLogOut(); }, 2000);
            }
        }
    });
}