var zTree;
var treeNodes = null;
var setting = {
    async: {
        enable: true,
        url: "/department/getdeptjson",
        type: 'post',
        dataType: 'json'
    },
    data: {
        simpleData: {
            enable: true,
            idKey: "id",
            pIdKey: "pid"
        }
    },
    view: {
        dblClickExpand: false,
        selectedMulti: false
    },
    expandSpeed: "fast",
    showLine: true,
    callback: {
        onRightClick: zTreeOnRightClick
           , onClick: zTreeOnClick
           , onAsyncSuccess: zTreeonAsyncSuccess
    }
};
var selectedNode = null;
//显示右键菜单  
function showRMenu(type, x, y, treeNode) {
    selectedNode = treeNode;
    nodes = zTree.getNodes();
    if (nodes.length > 0) {
        if (selectedNode == nodes[0]) {
            $("#m_del").hide();
        }
        else {
            $("#m_del").show();
        }
        $("#rMenu").show();
        $("#rMenu").css({
            "top": $(document).scrollTop() + y - 40 + "px",
            "left": $(document).scrollLeft() + x + "px",
            "display": "block"
        });
    }
}
function AddDept() {
    hideRMenu();
    setActionPanelValue('a');
}
function DeleteDept() {
    hideRMenu();
    setActionPanelValue('d');
    $('#save_btn').click();
}
function setActionPanelValue(action_type) {
    $('#action_type').val(action_type);
    if (action_type == 'u' || action_type == 'd') {
        if (selectedNode) {
            $('#div_pdept').hide();
            $('#pdept_key').val(0);
            $('#pdept_name').html('');
            $('#dept_key').val(selectedNode.id);
            $('#dept_name').val(selectedNode.name);
            $('#old_dept_name').val(selectedNode.name);
            $('#dept_note').val(selectedNode.note);
            $('#dept_sort').val(selectedNode.sort);
        }
        else {
            var nodes = zTree.getNodes();
            if (nodes.length > 0) {
                selectedNode = nodes[0];
                setActionPanelValue('u');
            }
        }
    }
    else if (action_type == 'a') {
        if (selectedNode) {
            $('#div_pdept').show();
            $('#pdept_key').val(selectedNode.id);
            $('#pdept_name').html(selectedNode.name);
            $('#dept_key').val(0);
            $('#dept_name').val('');
            $('#old_dept_name').val('');
            $('#dept_note').val('');
            $('#dept_sort').val('1');
        }
        else {
            var nodes = zTree.getNodes();
            if (nodes.length > 0) {
                selectedNode = nodes[0];
                setActionPanelValue('a');
            }
        }
    }
}
//隐藏右键菜单  
function hideRMenu() {
    $("#rMenu").hide();
}
function zTreeOnClick(event, treeId, treeNode) {
    selectedNode = treeNode;
    setActionPanelValue('u');
}
function zTreeonAsyncSuccess(event, treeNode, msg) {
    zTree.expandAll(true);
    var nodes = zTree.getNodes();
    if (nodes.length > 0) {
        selectedNode = nodes[0];
        zTree.selectNode(selectedNode);
        setActionPanelValue('u');
    }
}
//鼠标右键事件-创建右键菜单  
function zTreeOnRightClick(event, treeId, treeNode) {
    if (!treeNode) {
        zTree.cancelSelectedNode();
        showRMenu("root", event.clientX, event.clientY, treeNode);
    } else if (treeNode && !treeNode.noR) { //noR属性为true表示禁止右键菜单  
        if (treeNode.newrole && event.target.tagName != "a" && $(event.target).parents("a").length == 0) {
            zTree.cancelSelectedNode();
            showRMenu("root", event.clientX, event.clientY, treeNode);
        } else {
            zTree.selectNode(treeNode);
            showRMenu("node", event.clientX, event.clientY, treeNode);
        }
    }
}
function verify() {
    if (!_form.input(_.id("dept_name"))) {
        return false;
    }
    //         if ($("#dept_name").val() != $("#old_dept_name").val()) {
    //            if (_form.inputOnly('dept_name', 'only')) {
    //                return false;
    //            }
    //        }
    $('.help-inline').hide();
    return true;
}

function reloadTree() {
    hideRMenu();
    zTree = $.fn.zTree.init($("#tree"), setting, treeNodes);
}




$(function () {
    //$('#div_pdept').hide();
    reloadTree();
    $("body").bind(//鼠标点击事件不在节点上时隐藏右键菜单  
        "mousedown",
        function (event) {
            if (!(event.target.id == "rMenu" || $(event.target)
                    .parents("#rMenu").length > 0)) {
                $("#rMenu").hide();
            }
        });
    $('#save_btn').click(function () {
        if (!verify()) {
            return;
        }
        var run = true;
        if ($('#action_type').val() == 'd') {
            _confirm.open({ content: _msg.del_confirm }, function () {
                _ajax.post({
                    url: '/com/dept/parsedept',
                    arg: { action_type: $('#action_type').val(), dept_key: $('#dept_key').val() },
                    type: 'string',
                    success: function (v) {
                        if (v == "true") {
                            tempNode = selectedNode.getPreNode();
                            if (tempNode == null)
                                tempNode = selectedNode.getParentNode();
                            zTree.removeNode(selectedNode);
                            selectedNode = tempNode;
                            zTree.selectNode(selectedNode);
                            setActionPanelValue('u');
                        }
                        else {
                            _alert.open({ type: 'sigh', content: "delete failure" });
                        }
                    },
                    failure: function () {

                    }
                });
            });
        }
        else if ($('#action_type').val() == 'a') {
            _ajax.post({
                url: '/com/dept/parsedept',
                arg: {
                    action_type: $('#action_type').val(), pdept_key: $('#pdept_key').val(),
                    dept_name: $('#dept_name').val(), dept_note: $('#dept_note').val(), dept_sort: $('#dept_sort').val()
                },
                type: 'string',
                success: function (v) {
                    if (v != '') {
                        var node = { id: v, name: $('#dept_name').val(), note: $('#dept_note').val(), sort: $('#dept_sort').val() };
                        zTree.addNodes(selectedNode, node);
                        selectedNode = zTree.getNodeByParam("id", v, null);
                        zTree.selectNode(selectedNode);
                        setActionPanelValue('u');
                    }
                    else
                        _alert.open({ type: 'sigh', content: "add failure" });
                },
                failure: function () {

                }
            });

        }
        else if ($('#action_type').val() == 'u') {
            _ajax.post({
                url: '/com/dept/parsedept',
                arg: {
                    action_type: $('#action_type').val(), dept_key: $('#dept_key').val(),
                    dept_name: $('#dept_name').val(), dept_note: $('#dept_note').val(), dept_sort: $('#dept_sort').val()
                },
                type: 'string',
                success: function (v) {
                    if (v != '') {
                        selectedNode.name = $('#dept_name').val();
                        selectedNode.note = $('#dept_note').val();
                        selectedNode.sort = $('#dept_sort').val();
                        zTree.selectNode(selectedNode);
                        zTree.updateNode(selectedNode);
                    }
                    else
                        _alert.open({ type: 'sigh', content: "update failure" });
                },
                failure: function () {

                }
            });
        }
    });

    $('#user_setting_btn').click(function () {
        dept_key_1 = '0';
        if (selectedNode != null) {
            dept_key_1 = selectedNode.id;
        }
        if (dept_key_1 != "0") {
            _box.open({
                title: 'Assign Users',
                content: { iframe: '/com/dept/usersetting?action_type=add&dept_key=' + dept_key_1, name: 'UserSetting' },
                button: { Close: '_box.close()', Assign: "window.frames['UserSetting'].setting.add()" },
                width: _.minWidth(800),
                height: _.minHeight(600)
            });
        }
        else {
            _alert.open({ type: 'fail', content: "Please select an orgnization first！" });
        }
    });

    $('#view_setting_btn').click(function () {
        dept_key_1 = '0';
        if (selectedNode != null) {
            dept_key_1 = selectedNode.id;
        }
        if (dept_key_1 != "0") {
            _box.open({
                title: 'View Users',
                content: { iframe: '/com/dept/usersetting?action_type=view&dept_key=' + dept_key_1, name: 'UserSetting' },
                button: { Close: '_box.close()', DeleteUser: "window.frames['UserSetting'].setting.del()" },
                width: _.minWidth(800),
                height: _.minHeight(600)
            });
        }
        else {
            _alert.open({ type: 'fail', content: "Please select an orgnization first！" });
        }
    });
});