var tables = {
    init: function () {
        //var table = $('#sample_1');
        var table = $('#table');

        // begin first table
        table.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "没有数据",
                "info": "一共 _TOTAL_ 条，当前第 _START_ 到 _END_ 条",
                "infoEmpty": "没有任何记录",
                "infoFiltered": "(从一共 _MAX_ 条数据中查询)",
                "lengthMenu": "每页显示 _MENU_ 条数据",
                "search": "查询:",
                "zeroRecords": "No matching records found",
                "paginate": {
                    "previous": "Prev",
                    "next": "Next",
                    "last": "Last",
                    "first": "First"
                }
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.

            "columnDefs": [{
                "targets": 0,
                "orderable": false,
                "searchable": false
            }],

            "lengthMenu": [
                [5, 15, 20, -1],
                [5, 15, 20, "全部"] // change per page values here
            ],
            // set the initial value
            "pageLength": 5,
            "pagingType": "bootstrap_full_number",
            "columnDefs": [{  // set default column settings
                'orderable': false,
                'targets': [0]
            }, {
                "searchable": false,
                "targets": [0]
            }],
            "order": [
                [1, "asc"]
            ] // set first column as a default sort by asc
        });

        var tableWrapper = jQuery('#sample_1_wrapper');

        table.find('.group-checkable').change(function () {
            var set = jQuery(this).attr("data-set");
            var checked = jQuery(this).is(":checked");
            jQuery(set).each(function () {
                if (checked) {
                    $(this).prop("checked", true);
                    $(this).parents('tr').addClass("active");
                } else {
                    $(this).prop("checked", false);
                    $(this).parents('tr').removeClass("active");
                }
            });
        });

        table.on('change', 'tbody tr .checkboxes', function () {
            $(this).parents('tr').toggleClass("active");
        });
    }
}

var jsTree = {
    init: function () {
        $.ajax({
            type: 'POST',
            url: 'GetDepartJson',
            data: {},
            dataType: 'json',
            success: function (v) {
                if (v.result == "1") {
                    $("#tree").jstree({
                        "core": {
                            "themes": {
                                "responsive": false
                            },
                            // so that create works
                            "check_callback": true,
                            'data': eval(v.html)
                        },
                        "types": {
                            "default": {
                                "icon": "fa fa-folder icon-state-warning icon-lg"
                            },
                            "file": {
                                "icon": "fa fa-file icon-state-warning icon-lg"
                            }
                        },
                        "plugins": ["contextmenu", "dnd", "state", "types"],
                        "contextmenu": {
                            "items": {
                                "create": {
                                    "separator_before": false,
                                    "separator_after": true,
                                    "_disabled": false, //(this.check("create_node", data.reference, {}, "last")),
                                    "label": "新建",
                                    "action": function (data) {




                                        var layer_index = layer.prompt({
                                            formType: 3,
                                            title: '请输入名称'
                                        }, function (value, index, elem) {
                                            layer.close(layer_index);

                                            var inst = $.jstree.reference(data.reference), obj = inst.get_node(data.reference);
                                            var clickedNode = inst.get_node(data.reference);

                                            //存到数据库
                                            depart.insert(clickedNode.id, value,
                                               function (row_key) {
                                                   inst.create_node(obj,
                                                       {
                                                           "id": row_key,
                                                           text: value
                                                       },
                                                       "last");
                                               },
                                            null);




                                        });
                                        //var inst = jQuery.jstree.reference(data.reference);
                                        //var clickedNode = inst.get_node(data.reference);
                                        //alert("add operation--clickedNode's id is:" + clickedNode.id);
                                    }
                                },
                                "rename": {
                                    "separator_before": false,
                                    "separator_after": false,
                                    "_disabled": false, //(this.check("rename_node", data.reference, this.get_parent(data.reference), "")),
                                    "label": "重命名",
                                    /*!
                                    "shortcut"			: 113,
                                    "shortcut_label"	: 'F2',
                                    "icon"				: "glyphicon glyphicon-leaf",
                                    */
                                    "action": function (data) {
                                        var inst = $.jstree.reference(data.reference),
                                            obj = inst.get_node(data.reference);
                                        inst.edit(obj);
                                    }
                                },
                                "remove": {
                                    "separator_before": false,
                                    "icon": false,
                                    "separator_after": false,
                                    "_disabled": false, //(this.check("delete_node", data.reference, this.get_parent(data.reference), "")),
                                    "label": "删除",
                                    "action": function (data) {

                                        layer.confirm('确定删除？', { icon: 4 }, function (index) {

                                            var inst = $.jstree.reference(data.reference),
                                             obj = inst.get_node(data.reference);
                                            var clickedNode = inst.get_node(data.reference);

                                            depart.deleteDepart(clickedNode.id, function () {
                                                if (inst.is_selected(obj)) {
                                                    inst.delete_node(inst.get_selected());
                                                }
                                                else {
                                                    inst.delete_node(obj);
                                                }
                                            })

                                            layer.close(index);
                                        });

                                    }
                                },
                                "user": {
                                    "separator_before": false,
                                    "separator_after": false,
                                    "_disabled": false,
                                    "label": "用户管理",
                                    "action": function (data) {
                                        var inst = $.jstree.reference(data.reference),
                                            obj = inst.get_node(data.reference);
                                        user.setDepartInfo(obj.id, obj.text);
                                        user.getList();

                                    }

                                }
                                //"ccp": {
                                //    "separator_before": true,
                                //    "icon": false,
                                //    "separator_after": false,
                                //    "label": "编辑",
                                //    "action": false,
                                //    "submenu": {
                                //        "cut": {
                                //            "separator_before": false,
                                //            "separator_after": false,
                                //            "label": "剪切",
                                //            "action": function (data) {
                                //                var inst = $.jstree.reference(data.reference),
                                //                    obj = inst.get_node(data.reference);
                                //                if (inst.is_selected(obj)) {
                                //                    inst.cut(inst.get_top_selected());
                                //                }
                                //                else {
                                //                    inst.cut(obj);
                                //                }
                                //            }
                                //        },
                                //        "copy": {
                                //            "separator_before": false,
                                //            "icon": false,
                                //            "separator_after": false,
                                //            "label": "复制",
                                //            "action": function (data) {
                                //                var inst = $.jstree.reference(data.reference),
                                //                    obj = inst.get_node(data.reference);
                                //                if (inst.is_selected(obj)) {
                                //                    inst.copy(inst.get_top_selected());
                                //                }
                                //                else {
                                //                    inst.copy(obj);
                                //                }
                                //            }
                                //        },
                                //        "paste": {
                                //            "separator_before": false,
                                //            "icon": false,
                                //            "_disabled": function (data) {
                                //                return !$.jstree.reference(data.reference).can_paste();
                                //            },
                                //            "separator_after": false,
                                //            "label": "粘贴",
                                //            "action": function (data) {
                                //                var inst = $.jstree.reference(data.reference),
                                //                    obj = inst.get_node(data.reference);
                                //                inst.paste(obj);
                                //            }
                                //        }
                                //    }
                                //}
                            }
                        }

                    })
                    .bind('select_node.jstree', function (e, data) {
                        user.setDepartInfo(data.node.id, data.node.text);
                        user.getList();
                    });

                    $('#tree').on('ready.jstree', function () {

                        $("#tree").jstree('open_all');
                        //取默认用户（depart_key=1）
                        user.getList();
                    });
                }
            },
            failure: function () {

            },
            beforeSend: function () {
                $("#tree").jstree("destroy");
            }
        });
    }
}


var depart = {
    insert: function (parentId, name, successCallback, failureCallback) {
        depart.saveDepartInfo(0, parentId, name, successCallback, failureCallback);
    },
    saveDepartInfo: function (row_key, parentId, name, successCallback, failureCallback) {
        $.ajax({
            type: 'POST',
            url: 'SaveDepartInfo',
            data: { row_key: row_key, parentId: parentId, name: name },
            dataType: 'json',
            success: function (v) {
                if (v.result == "1") {
                    if (v.row_key_str == '')
                        v.row_key_str = undefined;
                    successCallback(v.row_key_str);
                }
            },
            failure: function () {
                layer.msg("保存部门失败。", { icon: 9, time: 1000 });
                failureCallback();
            }
        });
    },
    deleteDepart: function (row_key, successCallback, failureCallback) {
        $.ajax({
            type: 'POST',
            url: 'DelDepartInfo',
            data: { row_key: row_key },
            dataType: 'json',
            success: function (v) {
                if (v.result == "1") {
                    successCallback();
                }
            },
            failure: function () {
                layer.msg("删除部门失败。", { icon: 9, time: 1000 });
                failureCallback();
            }
        });
    }
}

var user = {
    depart_key: 1,
    depart_name: '',
    setDepartInfo: function (key, text) {
        user.depart_key = key;
        user.depart_name = text;
    },
    getList: function () {
        $.ajax({
            type: 'POST',
            url: 'GetUserList',
            data: { depart_key: user.depart_key },
            dataType: 'json',
            success: function (v) {
                if (v.result == "1") {
                    $('#table_div').html(v.html);
                    tables.init();
                }
            },
            failure: function () {

            },
            beforeSend: function () {
                $('#table_div').html('');
                if (user.depart_name == '')
                    $('#user-list-pre').html('')
                else
                    $('#user-list-pre').html('"' + user.depart_name + '"');
            }
        });
    },
    detail: function (obj, row_key) {
        var index = layer.load();
        $.ajax({
            type: 'POST',
            url: 'UserDetail',
            data: { row_key: row_key, type: 1 },
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
    verify: function () {
        if ($('#login_name').val() == '') {
            layer.msg("登录名不能为空。", { icon: 9, time: 1000 });
            return false;
        }

        //对登录名格式验证

        if ($('#nick_name').val() == '') {
            layer.msg("昵称不能为空。", { icon: 9, time: 1000 });
            return false;
        }
    },
    saveUser: function (obj) {

        if (!user.verify())
            return;

        closeImgSelection();
        $('#depart_key').val(user.depart_key);

        var index = layer.load();

        $('#user_form').ajaxSubmit({
            success: function (v) {
                if (v.result == "1") {
                    layer.msg("保存用户成功。", { icon: 8, time: 1000 });
                    $(obj).next().click();
                    user.getList();

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

    deleteUser: function (ids) {
        if (ids == 0) {
            var res_list = new Array();
            $('#table tr.active').each(function () {
                res_list.push($(this).attr('tag-key'));
            })

            if (res_list.length > 0) {
                ids = res_list.join(',');
            }
            else {
                ids = '';
            }
        }
        if (ids != '') {
            layer.confirm('确定删除？', function () {
                layer.closeAll();
                var index = layer.load();
                $.ajax({
                    type: 'POST',
                    url: 'DeleteUserInfo',
                    data: { ids: ids },
                    dataType: 'json',
                    success: function (v) {
                        if (v.result == "1") {
                            layer.msg('删除用户成功', { icon: 8, time: 2000 });
                        }
                        else {
                            layer.msg('删除用户失败', { icon: 9, time: 2000 });
                        }
                    },
                    failure: function () {
                        layer.msg('调用方法失败', { icon: 9, time: 2000 });
                    },
                    complete: function () {
                        layer.close(index);
                        user.getList();
                    }
                });
            })
        }
        else {
            layer.alert('请选择要删除的数据！');
        }
    },
    resetPwd: function (ids) {
        if (ids == 0) {
            var res_list = new Array();
            $('#table tr.active').each(function () {
                res_list.push($(this).attr('tag-key'));
            })

            if (res_list.length > 0) {
                ids = res_list.join(',');
            }
            else {
                ids = '';
            }
        }
        if (ids != '') {
            layer.confirm('确定重置密码？', { icon: 4 }, function () {
                layer.closeAll();
                var index = layer.load();
                $.ajax({
                    type: 'POST',
                    url: 'ResetUserPwd',
                    data: { ids: ids },
                    dataType: 'json',
                    success: function (v) {
                        if (v.result == "1") {
                            layer.msg('重置密码成功', { icon: 8, time: 2000 });
                        }
                        else {
                            layer.msg('重置密码失败', { icon: 9, time: 2000 });
                        }
                    },
                    failure: function () {
                        layer.msg('调用方法失败', { icon: 9, time: 2000 });
                    },
                    complete: function () {
                        layer.close(index);
                        user.getList();
                    }
                });
            });
        }
        else {
            layer.alert('请选择要操作的数据！');
        }
    },



    //本地预览  
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

if (App.isAngularJsApp() === false) {
    jQuery(document).ready(function () {
        //tables.init();
        jsTree.init();


        $('#refresh-tree').click(function () {
            jsTree.init();
        });

        $('#refresh-table').click(function () {
            user.getList();
        });


    });
}


layer.config({
    extend: 'extend/layer.ext.js'
});

