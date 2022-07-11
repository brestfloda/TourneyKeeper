<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKEditGame.aspx.cs" Inherits="TourneyKeeper.Web.TKEditGame" MasterPageFile="~/TKSite.master" %>

<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContentPlaceHolder">
    TourneyKeeper
</asp:Content>

<asp:Content ID="NavContent" runat="server" ContentPlaceHolderID="NavigationContent">
    <ul class="nav navbar-nav">
    </ul>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">
        $(function () {
            $('input[type=number]').focus(function () {
                $(this).val('');
            });
        });
    </script>

    <div class="row-fluid">
        <div class="col-md-12">
            <asp:HiddenField ID="GameId" runat="server" />
            <asp:HiddenField ID="TournamentId" runat="server" />
            <div class="row">
                <asp:PlaceHolder runat="server" ID="player1PointsPlaceHolder" Visible="true">
                    <div class="col-md-3">
                        <div class="md-form">
                            <asp:TextBox ID="player1Score" ClientIDMode="Static" CssClass="form-control" runat="server" autofocus="true" type="number" min="0"></asp:TextBox>
                            <label for="player1Score">
                                <asp:Literal ID="player1Label" runat="server"></asp:Literal></label>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="player1SecondaryPlaceholder" Visible="false">
                    <div class="col-md-3">
                        <div class="md-form">
                            <asp:TextBox ID="player1SecondaryScore" ClientIDMode="Static" CssClass="form-control" runat="server" autofocus="true" type="number" min="0"></asp:TextBox>
                            <label for="player1SecondaryScore">
                                <asp:Literal ID="secPoints1Label" runat="server"></asp:Literal></label>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="row">
                <asp:PlaceHolder runat="server" ID="player2PointsPlaceHolder" Visible="true">
                    <div class="col-md-3">
                        <div class="md-form">
                            <asp:TextBox ID="player2Score" ClientIDMode="Static" CssClass="form-control" runat="server" autofocus="true" type="number" min="0"></asp:TextBox>
                            <label for="player2Score">
                                <asp:Literal ID="player2Label" runat="server"></asp:Literal></label>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="player2SecondaryPlaceholder" Visible="false">
                    <div class="col-md-3">
                        <div class="md-form">
                            <asp:TextBox ID="player2SecondaryScore" ClientIDMode="Static" CssClass="form-control" runat="server" autofocus="true" type="number" min="0"></asp:TextBox>
                            <label for="player2SecondaryScore">
                                <asp:Literal ID="secPoints2Label" runat="server"></asp:Literal></label>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <a href="#" onclick="$(this).closest('form').submit();" class="btn btn-primary">Save game</a>
                </div>
                <div class="col-md-12">
                    <asp:Label runat="server" ForeColor="Red" ID="warningsLabel"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
