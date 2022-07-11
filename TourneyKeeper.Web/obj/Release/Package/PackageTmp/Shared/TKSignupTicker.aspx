<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKSignupTicker.aspx.cs" Inherits="TourneyKeeper.Web.TKSignupTicker" MasterPageFile="~/TKSite.master" %>

<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContentPlaceHolder">
    TourneyKeeper
</asp:Content>

<asp:Content ID="NavContent" runat="server" ContentPlaceHolderID="NavigationContent">
    <ul class="nav navbar-nav">
    </ul>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <style type="text/css">
        ul.countdown {
            list-style: none;
            margin: 75px 0;
            padding: 0;
            display: block;
            text-align: center;
        }

            ul.countdown li {
                display: inline-block;
            }

                ul.countdown li span {
                    font-size: 80px;
                    font-weight: 300;
                    line-height: 80px;
                }

                ul.countdown li.seperator {
                    font-size: 80px;
                    line-height: 70px;
                    vertical-align: top;
                }

                ul.countdown li p {
                    color: #a7abb1;
                    font-size: 14px;
                }
    </style>

    <div class="row-fluid">
        <div class="col-md-12">
            <h1 id="signupheader">
                <p style="text-align: center">
                    Sign up opens in:
                </p>
            </h1>
            <ul class="countdown" id="countdownUl">
                <li><span class="days">00</span>
                    <p class="days_ref">days</p>
                </li>
                <li class="seperator">.</li>
                <li><span class="hours">00</span>
                    <p class="hours_ref">hours</p>
                </li>
                <li class="seperator">:</li>
                <li><span class="minutes">00</span>
                    <p class="minutes_ref">minutes</p>
                </li>
                <li class="seperator">:</li>
                <li><span class="seconds">00</span>
                    <p class="seconds_ref">seconds</p>
                </li>
            </ul>
            <h1 id="signup" style="visibility: collapse">
                <p style="text-align: center">
                    <asp:HyperLink runat="server" ID="signupLink">Sign up!</asp:HyperLink>
                </p>
            </h1>
        </div>
    </div>
    <script type="text/javascript" src="/tools/scripts/jquery.downCount.js"></script>
    <script type="text/javascript">
        $('.countdown').downCount({
            date: '<%=startDate%>',
            offset: +10
        }, function () {
            document.getElementById("countdownUl").style.visibility = "collapse";
            document.getElementById("signupheader").style.visibility = "collapse";
            document.getElementById("signup").style.visibility = "visible";
        });
    </script>
</asp:Content>
