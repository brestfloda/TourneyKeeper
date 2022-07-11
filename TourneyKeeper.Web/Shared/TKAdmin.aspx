<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKAdmin.aspx.cs" Inherits="TourneyKeeper.Web.TKAdmin" MasterPageFile="TKShared.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#gamesli").addClass("active");
    </script>

    <div class="row-fluid">
        <div class="span12" style="margin: 6px; padding-top: 6px; padding-bottom: 1px">
            <div class="row">
                <div class="col-md-4">
                    <asp:LinkButton runat="server" OnClick="CalculateRankingClick" Text="Calculate Ranking" CssClass="btn btn-large btn-primary" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:LinkButton runat="server" OnClick="WLD1Click" Text="Calculate WLD1" CssClass="btn btn-large btn-primary" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:LinkButton runat="server" OnClick="WLD2Click" Text="Calculate WLD2" CssClass="btn btn-large btn-primary" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
