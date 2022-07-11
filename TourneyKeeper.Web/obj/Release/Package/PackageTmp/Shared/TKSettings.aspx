<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKSettings.aspx.cs" Inherits="TourneyKeeper.Web.TKSettings" MasterPageFile="TKShared.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#settingsli").addClass("active");
        $(document).ready(function () {
            $('.mdb-select').materialSelect();
        });
    </script>

    <div class="md-form">
        <asp:TextBox runat="server" ID="idLabel" Enabled="false" ClientIDMode="Static" class="form-control"></asp:TextBox>
        <label for="idLabel">Id</label>
    </div>
    <div class="md-form">
        <asp:TextBox runat="server" ID="nameTextBox" ClientIDMode="Static" class="form-control"></asp:TextBox>
        <label for="nameTextBox">Name</label>
    </div>
    <div class="md-form">
        <asp:TextBox runat="server" ID="emailTextBox" ClientIDMode="Static" class="form-control"></asp:TextBox>
        <label for="emailTextBox">E-mail</label>
    </div>
    <div class="md-form">
        <asp:DropDownList ID="countryDropdown" CssClass="mdb-select colorful-select dropdown-primary" runat="server"></asp:DropDownList>
    </div>
    <div class="md-form">
        <asp:TextBox runat="server" ID="password1TextBox" ClientIDMode="Static" class="form-control"></asp:TextBox>
        <label for="password1TextBox">New password</label>
    </div>
    <div class="md-form">
        <asp:TextBox runat="server" ID="password2TextBox" ClientIDMode="Static" class="form-control"></asp:TextBox>
        <label for="password2TextBox">New password again</label>
    </div>
    <div class="md-form">
        <asp:LinkButton runat="server" OnClick="UpdateClick" Text="Update" CssClass="btn btn-large btn-primary" />
    </div>
</asp:Content>
