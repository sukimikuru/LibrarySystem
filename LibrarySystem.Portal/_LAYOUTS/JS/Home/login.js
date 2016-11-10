var login = {
    setlogging: function () {
        $('.login').addClass('logging');
        setTimeout(function () {
            $('.login').addClass('logged');
        }, 300);

        setTimeout(function () {
            $('.authent').show().animate({ right: -320 }, {
                easing: 'easeOutQuint',
                duration: 600,
                queue: false
            });
            $('.authent').animate({ opacity: 1 }, {
                duration: 200,
                queue: false
            }).addClass('visible');
        }, 500);


    },
    setloggfinish: function (callback, success) {
        setTimeout(function () {
            $('.authent').show().animate({ right: 90 }, {
                easing: 'easeOutQuint',
                duration: 600,
                queue: false
            });
            $('.authent').animate({ opacity: 0 }, {
                duration: 200,
                queue: false
            }).addClass('visible');
            $('.login').removeClass('logged');
        }, 150);
        setTimeout(function () {
            $('.login').removeClass('logging');
        }, 250);
        if (success) {

            setTimeout(function () {
                $('.login div').fadeOut(123);
            }, 250);

            setTimeout(function () {
                $('.success').fadeIn();
            }, 300);
        }
        else {
            setTimeout(function () {
                $('.authent').removeAttr('style');
            }, 250);;
        }
        callback();
    },
    login_success: function (callback) {
        $('.authent').show().animate({ right: 90 }, {
            easing: 'easeOutQuint',
            duration: 600,
            queue: false
        });
        $('.authent').animate({ opacity: 0 }, {
            duration: 200,
            queue: false
        });
        $('.login').removeClass('logged');

        $('.login').removeClass('logging');

        $('.authent').removeAttr('style');

        $('.success').fadeIn();



        //setTimeout(function () { callback(); }, 200);
    },
    login_failed: function (callback) {
        $('.authent').show().animate({ right: 90 }, {
            easing: 'easeOutQuint',
            duration: 600,
            queue: false
        });
        $('.authent').animate({ opacity: 0 }, {
            duration: 200,
            queue: false
        });
        $('.login').removeClass('logged');
        $('.login').removeClass('logging');

        $('.authent').removeAttr('style');


        setTimeout(function () { callback(); }, 200);
    }

}

$(function () {
    $('#login_btn').click(function () {

        var RememberMe = false;
        if ($("#RememberMe").attr("checked") == "checked") {
            RememberMe = true;
        }
        if ($('#user_name').val().length == 0 || $('#password').val().length == 0) {
            layer.msg('用户名、密码不能为空。', { icon: 0, time: 2000 });
            return;
        }

        login.setlogging();

        $.ajax({
            type: "POST",
            url: '/home/LoginSys',
            data: { user_name: $('#user_name').val(), password: $('#password').val(), RememberMe: RememberMe },
            dataType: 'json',
            //timeout: '500000',
            success: function (v) {
                if (v.result == '1') {
                    setTimeout(function () {
                        login.setloggfinish(function () {
                            setTimeout(function () { window.location.href = '/user/list'; }, 1500);
                        }, true);

                    }, 2000);
                }
                else {

                    setTimeout(function () {

                        login.setloggfinish(function () {
                            setTimeout(function () { layer.msg(v.msg, { icon: 0, time: 2000 }); }, 800);
                        }, 0);


                    }, 2000);
                }

            },
            failure: function () {

            },
            complete: function (XMLHttpRequest, status) {
                //if (status == 'timeout') {
                //    setTimeout(function () {

                //        login.setloggfinish(function () {
                //            setTimeout(function () { layer.msg('登录超时！', { icon: 0, time: 2000 }); }, 800);
                //        }, 0);

                //    }, 2000);
                //}
            }
        });

    });
    $('input[type="text"],input[type="password"]').focus(function () {
        $(this).prev().animate({ 'opacity': '1' }, 200);
    });
    $('input[type="text"],input[type="password"]').blur(function () {
        $(this).prev().animate({ 'opacity': '.5' }, 200);
    });
    $('input[type="text"],input[type="password"]').keyup(function (event) {
        if (!$(this).val() == '') {
            $(this).next().animate({
                'opacity': '1',
                'right': '30'
            }, 200);
        } else {
            $(this).next().animate({
                'opacity': '0',
                'right': '20'
            }, 200);
        }

        if (event.which == 13) {
            if (this.id == 'user_name') {
                $('#password').focus();
            }
            else if (this.id == 'password') {
                $('#login_btn').click();
            }
        }
        
    });
    var open = 0;
    $('.tab').click(function () {
        $(this).fadeOut(200, function () {
            $(this).parent().animate({ 'left': '0' });
        });
    });



});


