function SignOut() {
    var model = {
        TournamentId: $('#tournamentIdForSignOut').val(),
        PlayerId: $('#playerIdForSignOut').val()
    };

    $.post('/webapi/tournamentplayer/SignOut', model)
        .done(function (res) {
            if (!res.Success) {
                alert("Could not sign out");
            }
            else {
                location.reload();
            }
        });
}

function Login(login, password, callback) {
    var model = {
        Login: login,
        Password: password
    };

    $.post('/webapi/login/login', model)
        .done(function (res) {
            if (!res.Success) {
                callback(false);
            }
            else {
                callback(true);
            }
        });
}

function ResetPassword(emailToReset, callback) {
    var model = {
        Email: emailToReset
    };

    $.post('/webapi/login/ResetPassword', model)
        .done(function (res) {
            if (!res.Success) {
                callback(false);
            }
            else {
                callback(true);
            }
        });
}

function UpdateField(controller, field, id, value, control, token) {
    var model = {
        Id: id,
        Value: value,
        FieldToUpdate: field,
        Token: token
    };

    $.post(controller, model)
        .done(function (res) {
            if (!res.Success) {
                alert(res.Message);
            }
            else {
                AnimateSuccess(control.id);
            }
        });
}

function UpdateFieldCallBack(controller, field, id, value, control, token, callback) {
    var model = {
        Id: id,
        Value: value,
        FieldToUpdate: field,
        Token: token
    };

    $.post(controller, model)
        .done(function (res) {
            if (!res.Success) {
                alert(res.Message);
            }
            else {
                AnimateSuccess(control.id);

                callback();
            }
        });
}

function AnimateSuccess(controlId) {
    var element = $("#" + controlId);
    if (element.hasClass("mdb-select")) {
        element.closest("div").animate({ backgroundColor: '#dff0d8' }, 2000);

        setTimeout(function () {
            element.closest("div").animate({ backgroundColor: '#ffffff' }, 2000);
        }, 2000);
    }
    else if (element.hasClass("form-check-input")) {
        element.closest("div").animate({ backgroundColor: '#dff0d8' }, 2000);

        setTimeout(function () {
            element.closest("div").animate({ backgroundColor: '#ffffff' }, 2000);
        }, 2000);
    }
    else {
        element.animate({ backgroundColor: '#dff0d8' }, 2000);

        setTimeout(function () {
            element.animate({ backgroundColor: '#ffffff' }, 2000);
        }, 2000);
    }
}

function UpdateFieldCallBackFieldAndResult(controller, field, id, value, control, token, callback) {
    var model = {
        Id: id,
        Value: value,
        FieldToUpdate: field,
        Token: token
    };

    $.post(controller, model)
        .done(function (res) {
            if (!res.Success) {
                callback(control, res);
            }
            else {
                $("#" + control.id).animate({ backgroundColor: '#dff0d8' }, 2000);

                setTimeout(function () {
                    $("#" + control.id).animate({ backgroundColor: '#ffffff' }, 2000);
                }, 2000);
                callback(control, res);
            }
        });
}

function ShowDatePicker(id) {
    $(document).ready(function () {
        $(id).datepicker({
            inline: true,
            dateFormat: "dd-mm-yy",
            firstDay: 1,
            showOtherMonths: true
        });
    });
}
function OpenPopupViaLink(link) {
    window.open(link);
}
function OpenPopup(id) {
    window.open("/Shared/ShowArmy.aspx?id=" + id, "Custom", "scrollbars=yes,resizable=yes,menubar=no,status=no,toolbar=no,width=800,height=600");
}

function isPostBack() {
    return document.getElementById('_ispostback').value === 'True';
}

function OpenModal(name) {
    $(name).modal({
        'backdrop': 'static'
    });
}

function CloseModal(name) {
    $(name).modal('hide');
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function UpdateActivePlayers() {
    var model = {
        Id: getUrlVars().id
    };

    $.post('/WebAPI/Tournament/GetActivePlayers', model)
        .done(function (res) {
            if (!res.Success) {
                alert(res.Message);
            }
            else {
                numPlayersLabel.textContent = res.Message;
            }
        });
}
