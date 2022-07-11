<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TourneyKeeper.Web.Default" MasterPageFile="~/Frontpage.master" %>

<%@ Register Src="~/Controls/UpcomingTournaments.ascx" TagPrefix="uc1" TagName="UpcomingTournaments" %>
<%@ Register Src="~/Controls/PlayingNow.ascx" TagPrefix="uc2" TagName="PlayingNow" %>
<%@ Register Src="~/Controls/YouArePlaying.ascx" TagPrefix="uc3" TagName="YouArePlaying" %>

<asp:Content ID="DefaultBodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <div class="row">
        <asp:PlaceHolder ID="youArePlayingPlaceHolder" runat="server" Visible="false">
            <div class="col-lg-4">
                <uc3:YouArePlaying runat="server" ID="YouArePlaying" />
            </div>
        </asp:PlaceHolder>
        <asp:Literal ID="playingNowLiteral" runat="server">
            <div class="col-lg-4">
        </asp:Literal>
        <uc2:PlayingNow runat="server" ID="PlayingNow" />
    </div>
    <asp:Literal ID="upcomingTournamentsLiteral" runat="server">
    <div class="col-lg-6">
    </asp:Literal>
    <uc1:UpcomingTournaments runat="server" ID="UpcomingTournaments" />
    </div>
    </div>
    <%--    
        <div class="col-md-4">
        </div>
                    <div class="col-md-4">
                <h4 style="padding-left: 15px">Mobile app</h4>
                <div style="border-radius: 4px; background: #e7e7e7; margin-right: 10px; margin-bottom: 10px">
                    <a href='https://play.google.com/store/apps/details?id=tourneykeeper.tourneykeeper&rdid=tourneykeeper.tourneykeeper&pcampaignid=MKT-Other-global-all-co-prtnr-py-PartBadge-Mar2515-1'>
                        <img alt='Get it on Google Play' width="300" src='https://play.google.com/intl/en_us/badges/images/generic/en_badge_web_generic.png' /></a>
                </div>
            </div>
    </div>--%>
</asp:Content>
