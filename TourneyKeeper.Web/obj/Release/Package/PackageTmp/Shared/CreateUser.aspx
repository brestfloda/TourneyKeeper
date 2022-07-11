<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="TourneyKeeper.Web.CreateUser" MasterPageFile="~/TKSite.master" %>

<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContentPlaceHolder">
    TourneyKeeper
</asp:Content>

<asp:Content ID="NavContent" runat="server" ContentPlaceHolderID="NavigationContent">
    <ul class="nav navbar-nav">
    </ul>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">
        $("#adminli").addClass("active");
    </script>

    <p class="h4 mb-4">Sign up</p>

    <asp:HiddenField ID="createdOk" ClientIDMode="Static" runat="server" />

    <asp:TextBox type="text" ID="nameTextBox" class="form-control" placeholder="Name" runat="server" />
    <asp:DropDownList ID="countryDropdown" class="form-control" runat="server"></asp:DropDownList>
    <asp:TextBox type="email" ID="emailTextBox" class="form-control" placeholder="E-mail" runat="server" />
    <asp:TextBox type="username" ID="usernameRegisterTextBox" class="form-control" placeholder="Username" runat="server" />
    <asp:TextBox type="password" ID="passwordTextBox" class="form-control" placeholder="Password" aria-describedby="defaultRegisterFormPasswordHelpBlock" runat="server" />
    <small id="defaultRegisterFormPasswordHelpBlock" class="form-text text-muted mb-4">We recommend at least 8 characters and 1 digit</small>
    <span style="color: red;">
        <asp:Literal ID="signupWarning" runat="server"></asp:Literal>
    </span>

    <a href="#" onclick="javascript:$(this).closest('form').submit();" id="loginHref" class="btn btn-primary btn-block">Register</a>

    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#createdOk")[0].value == 'true') {
                OpenModal('#loginModal');
            }
        });
    </script>
</asp:Content>
