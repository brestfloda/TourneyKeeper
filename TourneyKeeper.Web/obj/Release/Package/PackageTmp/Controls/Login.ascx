<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="TourneyKeeper.Web.Controls.Login" %>

<script>
    function ResetPasswordClick() {
        resetWarning.value = '';
        ResetPassword(resetPasswordEmailTextBox.value, ResetCallback);
    }

    function LoginClick() {
        loginWarning.value = '';
        Login($("#usernameTextbox").val(), $("#passwordTextbox").val(), LoginCallback);
    }

    function LoginCallback(success) {
        if (success) {
            $("#usernameTextbox").prop("name", $("#usernameTextbox").prop("id"));
            $("#passwordTextbox").prop("name", $("#passwordTextbox").prop("id"));
            $("#RememberMeHiddenField").prop("name", $("#RememberMeHiddenField").prop("id"));
            $("#navigationFrom").prop("name", $("#navigationFrom").prop("id"));
            $("#navigationFrom").prop("value", window.location.href);

            document.getElementById('aspnetForm').action = '/shared/login.ashx';
            $('#loginModal').closest('form').submit();
        }
        else {
            loginWarning.innerHTML = 'Login failed';
        }
    }

    function ResetCallback(success) {
        if (success) {
            resetWarning.innerHTML = 'You have been sent a new password';
        }
        else {
            resetWarning.innerHTML = 'Failed to reset your password';
        }
    }

    $(document).ready(function () {
        $('#loginModal').on('shown.bs.modal', function () {
            resetWarning.innerHTML = '';
            loginWarning.innerHTML = '';
            $('#usernameTextbox').focus();
            FB.getLoginStatus(function (response) {
                statusChangeCallback(response);
            });
        })
    });

    $(document).keypress(function (e) {
        if ($('#loginModal').is(':visible') && (e.keycode == 13 || e.which == 13)) {
            LoginClick();
        }
    });

    function statusChangeCallback(response) {
        console.log(response.status);
    }

    function checkLoginState() {
        FB.getLoginStatus(function (response) {
            statusChangeCallback(response);
        });
    }
</script>

<div class="modal fade" id="passwordModal" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="resetModalLabel">Reset password</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p class="text-danger" id="resetWarning"></p>
                <p>Enter the e-mail with which you registered and a new password will be sent to you.</p>
                E-mail:
                <asp:TextBox ID="resetPasswordEmailTextBox" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
            <div class="modal-footer">
                <a href="#" onclick="ResetPasswordClick();" class="btn btn-primary">Reset Password</a>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="loginModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modalLabel">Login or register</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p class="text-danger" id="loginWarning"></p>
                <asp:HiddenField ID="navigationFrom" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="RememberMeHiddenField" ClientIDMode="Static" runat="server" />
                <asp:TextBox ID="usernameTextbox" class="form-control" ClientIDMode="Static" runat="server" placeholder="Email address or username" autocomplete="username"></asp:TextBox>
                <asp:TextBox ID="passwordTextbox" class="form-control mb-2" ClientIDMode="Static" runat="server" placeholder="Password" TextMode="Password" autocomplete="current-password"></asp:TextBox>
                <div class="col-md-6 form-check">
                    <input type="checkbox" class="form-check-input" id="rememberMeCheckBox" onchange="javascript:$('#RememberMeHiddenField').val($('#rememberMeCheckBox').is(':checked'));">
                    <label class="form-check-label" for="rememberMeCheckBox">Remember me</label>
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" onclick="LoginClick();return false;" id="loginHref" class="btn btn-primary noWrapLink" style="padding: 13px 13px">Login</a>
                &nbsp;
                <a href="/Shared/CreateUser.aspx" class="btn btn-primary noWrapLink" style="padding: 13px 13px">New user</a>
                &nbsp;
                <a href="javascript:CloseModal('#loginModal');OpenModal('#passwordModal');" class="btn btn-primary noWrapLink" style="padding: 13px 13px">New password</a>
            </div>
            <div>
                <fb:login-button
                    scope="public_profile,email"
                    onlogin="checkLoginState();">
                </fb:login-button>
            </div>
        </div>
    </div>
</div>
