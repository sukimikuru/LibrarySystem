var change = {
    verify: function () {
        if ($('#pwd').val() == '') {
            layer.msg("旧密码不能为空。", { icon: 9, time: 1000 });
            $('#pwd').focus();
            return false;
        } else if ($('#pwd1').val().length < 6) {
            layer.msg("新密码长度必须大于等于6。", { icon: 9, time: 1000 });
            $('#pwd1').focus();
            return false;
        } else if ($('#pwd1').val() != $('#pwd2').val()) {
            layer.msg("两次密码不一致。", { icon: 9, time: 1000 });
            $('#pwd2').focus();
            return false;
        }
        return true;
    },
    save: function () {
        if (change.verify()) {
            if (_.lockAjax('save_btn')) {
                $.ajax({
                    type: 'POST',
                    url: '/com/public/savepwd',
                    data: {
                        pwd: $('#pwd').val(), pwd1: $('#pwd1').val()
                    },
                    dataType: 'json',
                    success: function (v) {
                        if (v.result == "1") {
                            layer.msg("密码修改成功。", { icon: 8, time: 1000 });
                        } else {
                            layer.msg("密码修改失败。", { icon: 9, time: 1000 });
                        }
                    },
                    error: function () {
                        layer.msg("密码修改失败。", { icon: 9, time: 1000 });
                    },
                    complete: function () { _.releaseAjax('save_btn'); }
                });
            }
        }
    }
  
};
$(function () {
  
});