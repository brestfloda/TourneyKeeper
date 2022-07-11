<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKSignup.aspx.cs" Inherits="TourneyKeeper.Web.TKSignup" MasterPageFile="~/TKSite.master" ValidateRequest="false" %>

<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContentPlaceHolder">
    TourneyKeeper
</asp:Content>

<asp:Content ID="NavContent" runat="server" ContentPlaceHolderID="NavigationContent">
    <ul class="nav navbar-nav">
    </ul>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">
        $.urlParam = function (name) {
            var results = new RegExp('[\?&]' + name + '=([^&#]*)')
                .exec(window.location.href);

            return results[1] || 0;
        }

        $(document).ready(function () {
            $('.mdb-select').materialSelect();
            $("#playerDropDown").change(function () {
                if ($("#playerDropDown option:selected").text() == "Player") {
                    $("#playerDiv").show();
                    $("#nonplayerDiv").removeAttr("style").hide();
                    $("#IsPlayerHiddenField").prop("value", "true");
                }
                else if ($("#playerDropDown option:selected").text() == "Non-player") {
                    $("#playerDiv").show();
                    $("#nonplayerDiv").removeAttr("style").hide();
                    $("#IsPlayerHiddenField").prop("value", "false");
                }
                else {
                    $("#playerDiv").removeAttr("style").hide();
                    $("#nonplayerDiv").removeAttr("style").hide();
                }
            });

            CalculateTotal();
        });

        function CalculateTotal() {
            var grid = document.getElementById('OptionsGridView');

            var price;
            var tmptotalprice = 0;
            var amount;

            for (i = 1; i < (grid.rows.length - 1); i++) {
                price = parseFloat(grid.rows[i].cells[2].innerText.replace(',', '.'));
                amount = Number(grid.rows[i].cells[3].getElementsByTagName('input')[0].value);

                if (amount > 0) {
                    tmptotalprice += amount * price;
                }
            }
            $('#totalprice').empty();
            $('#totalprice').append(tmptotalprice);
        }
    </script>

    <asp:HiddenField runat="server" ID="tournamentId" />
    <asp:HiddenField runat="server" ID="IsPlayerHiddenField" ClientIDMode="Static" Value="" />
    <asp:PlaceHolder ID="TeamPlaceHolder" runat="server" Visible="false">
        <div class="row-fluid">
            <div class="col-md-12">
                <asp:Label runat="server">Create new team:</asp:Label>
                &nbsp;
                <asp:TextBox ID="NewTeamTextBox" runat="server"></asp:TextBox>
                <asp:LinkButton runat="server" ClientIDMode="Static" ID="CreateTeamLinkButton" OnClick="CreateTeamClick" Text="Create new team" CssClass="btn btn-md btn-primary" />
            </div>
        </div>
        <div class="row-fluid">
            <div class="col-md-12">
                <asp:DropDownList ID="TeamDropDownList" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" runat="server" DataValueField="Id" DataTextField="Name" OnSelectedIndexChanged="TeamDropDownList_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row-fluid">
        <div class="col-md-12">
            <asp:DropDownList ID="PrimaryCodexDropDownList" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
        </div>
        <div class="col-md-12">
            <asp:DropDownList ID="SecondaryCodexDropDownList" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
        </div>
        <div class="col-md-12">
            <asp:DropDownList ID="TertiaryCodexDropDownList" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
        </div>
        <div class="col-md-12">
            <asp:DropDownList ID="QuaternaryCodexDropDownList" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
        </div>
        <div class="col-md-12">
            <asp:TextBox runat="server" TextMode="MultiLine" ID="ArmyListTextBox" CssClass="form-control" Height="200"></asp:TextBox>
        </div>
        <div class="col-md-12">
            <asp:LinkButton runat="server" ClientIDMode="Static" ID="signupRankedButton" OnClick="SignUpRankedClick" Text="Sign up ranked" CssClass="btn btn-primary" />
            <asp:LinkButton runat="server" ClientIDMode="Static" ID="signupUnrankedButton" OnClick="SignUpUnrankedClick" Text="Sign up unranked" CssClass="btn btn-primary" />
            <asp:LinkButton runat="server" ClientIDMode="Static" ID="signupNonPlayerButton" OnClick="SignUpNonPlayerClick" Text="Sign up as non-player" Visible="false" CssClass="btn btn-primary" />
        </div>
        <div class="col-md-12">
            <asp:Literal runat="server" ID="tournamentFullLiteral"></asp:Literal>
        </div>
    </div>
</asp:Content>
