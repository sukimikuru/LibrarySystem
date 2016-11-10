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
                [5, 15, 20, "All"] // change per page values here
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
                                "res": {
                                    "separator_before": false,
                                    "separator_after": false,
                                    "_disabled": false,
                                    "label": "资源管理",
                                    "action": function (data) {
                                        var inst = $.jstree.reference(data.reference),
                                            obj = inst.get_node(data.reference);
                                        res.setDepartInfo(obj.id, obj.text);
                                        res.getList();

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
                    });

                    $('#tree').on('ready.jstree', function () {

                        $("#tree").jstree('open_all');
                        //取默认用户（depart_key=1）
                        res.getList();
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

var res = {
    depart_key: 1,
    depart_name: '',
    setDepartInfo: function (key, text) {
        res.depart_key = key;
        res.depart_name = text;
    },
    getList: function () {
        $.ajax({
            type: 'POST',
            url: 'GetResList',
            data: {},
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
                //if (user.depart_name == '')
                //    $('#user-list-pre').html('')
                //else
                //    $('#user-list-pre').html('"' + user.depart_name + '"');
            }
        });
    },
    detail: function (obj, row_key) {
        var index = layer.load();
        $.ajax({
            type: 'POST',
            url: 'ResDetail',
            data: { row_key: row_key },
            dataType: 'json',
            success: function (v) {
                if (v.result == "1") {
                    $('#res-detail-div').html(v.html);
                }
            },
            complete: function () {
                layer.close(index);
                $(obj).next().click();
                _wu.init('doc,docx,xls,xlsx,ppt,pptx,pdf,txt,mp4,mp3,avi,wmv,swf,jpg', null, null, 1);
            }
        });
    },
    verify: function () {
        if ($('#name').val() == '') {
            layer.msg("名称不能为空。", { icon: 9, time: 1000 });
            return false;
        }

        var res_type = $('#res_type').val();
        if (res_type == "3") {
            if ($('#web_path').val() == '') {
                layer.msg("地址不能为空。", { icon: 9, time: 1000 });
                return false;
            }


            //验证地址的格式

        }
        else {
            if ($('#upload_infos').val() == '') {
                layer.msg("请先上传文件。", { icon: 9, time: 1000 });
                return false;
            }
        }

        return true;
    },
    saveRes: function (obj) {

        if (!res.verify())
            return;

        var row_key = $('#res-detail-div .row_key').val();
        var index = layer.load();
        $.ajax({
            type: 'POST',
            url: 'SaveResInfo',
            data: { row_key: row_key, name: $('#name').val(), note: $('#note').val(), res_type: $('#res_type').val(), upload_infos: $('#upload_infos').val(), exist_infos: $('#exist_infos').val(), web_path: $('#web_path').val() },
            dataType: 'json',
            success: function (v) {
                if (v.result == "1") {
                    layer.msg("保存资源成功。", { icon: 8, time: 1000 });
                }
                else {
                    layer.msg("保存资源失败。", { icon: 9, time: 1000 });
                }
            },
            failure: function () {
                layer.msg("保存资源失败。", { icon: 9, time: 1000 });
            },
            complete: function () {
                layer.close(index);
                res.getList();
            },
            beforeSend: function () {
                $(obj).next().click();
            }
        });
    },
    deleteRes: function (ids) {
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
                var index = layer.load();
                $.ajax({
                    type: 'POST',
                    url: 'DeleteResInfo',
                    data: { ids: ids },
                    dataType: 'json',
                    success: function (v) {
                        if (v.result == "1") {
                            layer.msg('删除资源成功', { icon: 8, time: 2000 });
                        }
                        else {
                            layer.msg('删除资源失败', { icon: 9, time: 2000 });
                        }
                    },
                    failure: function () {
                        layer.msg('删除资源失败', { icon: 9, time: 2000 });
                    },
                    complete: function () {
                        layer.close(index);
                        res.getList();
                    }
                });
            })
        }
        else {
            layer.alert('请选择要删除的数据！');
        }
    },
    delPath: function (obj) {
        layer.confirm('确定删除文件？', {
            btn: ['确定', '取消']
        }, function (index) {
            $(obj).parent().parent().remove();
            layer.close(index);
        });
    },
    changeType: function () {
        var res_type = $('#res_type').val();
        if (res_type == "3") {
            $('#upload-div').show();
            $('#path-div').hide();
            $('#upload-div').hide();
            $('#path-div').show();
        }
        else {
            $('#upload-div').show();
            $('#path-div').hide();
        }

    }

}

if (App.isAngularJsApp() === false) {
    jQuery(document).ready(function () {
        //tables.init();
        //jsTree.init();

        res.getList();

        $('#refresh-table').click(function () {
            res.getList();
        });



    });
}




layer.config({
    extend: 'extend/layer.ext.js'
});

