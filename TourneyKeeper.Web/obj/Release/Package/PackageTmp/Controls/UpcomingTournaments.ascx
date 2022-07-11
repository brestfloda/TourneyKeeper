<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpcomingTournaments.ascx.cs" Inherits="TourneyKeeper.Web.Controls.UpcomingTournaments" %>

<h4>Upcoming tournaments</h4>
<asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
<asp:GridView ID="CurrentTournamentsGridView" runat="server"
    AutoGenerateColumns="False" CssClass="table table-striped" AllowSorting="True" OnDataBound="CurrentTournamentsGridView_DataBound"
    UseAccessibleHeader="true" OnSorting="CurrentTournamentsGridViewSorting" GridLines="None">
    <Columns>
        <asp:TemplateField HeaderText="Tournament" SortExpression="Name" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell">
            <ItemTemplate>
                <asp:HyperLink ID="TournamentButton" runat="Server" NavigateUrl='<%# RenderTournamentLink((int)Eval("Id"), (TourneyKeeper.Common.TournamentType)Eval("TournamentType"), (bool)Eval("UseAbout")) %>'>
                                            <%# Eval("Name") %>
                <%# ((bool)Eval("OnlineSignup") && (DateTime)Eval("TournamentDate") > DateTime.Now && (!((DateTime?)Eval("OnlineSignupStart")).HasValue || ((DateTime?)Eval("OnlineSignupStart")).Value < DateTime.Now )) ? " - signup open" : ""%>
                <%# ((bool)Eval("OnlineSignup") && (DateTime)Eval("TournamentDate") > DateTime.Now && (((DateTime?)Eval("OnlineSignupStart")).HasValue && ((DateTime?)Eval("OnlineSignupStart")).Value > DateTime.Now )) ? " - signup opens soon" : ""%>
                </asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tournament" SortExpression="Name" ItemStyle-CssClass="d-table-cell d-sm-none" HeaderStyle-CssClass="d-table-cell d-sm-none">
            <ItemTemplate>
                <div class="container">
                    <div class="row">
                        <asp:HyperLink ID="TournamentButton" runat="Server" NavigateUrl='<%# RenderTournamentLink((int)Eval("Id"), (TourneyKeeper.Common.TournamentType)Eval("TournamentType"), (bool)Eval("UseAbout")) %>'>
                                            <%# Eval("Name") %>
                        </asp:HyperLink>
                        <%# ((bool)Eval("OnlineSignup") && (DateTime)Eval("TournamentDate") > DateTime.Now && (!((DateTime?)Eval("OnlineSignupStart")).HasValue || ((DateTime?)Eval("OnlineSignupStart")).Value < DateTime.Now )) ? " - signup open" : ""%>
                        <%# ((bool)Eval("OnlineSignup") && (DateTime)Eval("TournamentDate") > DateTime.Now && (((DateTime?)Eval("OnlineSignupStart")).HasValue && ((DateTime?)Eval("OnlineSignupStart")).Value > DateTime.Now )) ? " - signup opens soon" : ""%>
                    </div>
                    <div class="row">
                        <div class="form-inline">
                            <div class="form-group">
                                <img width="16" height="16" src='<%# ((TourneyKeeper.Common.TournamentType)Eval("TournamentType"))== TourneyKeeper.Common.TournamentType.Singles?"../Images/single.png":"../Images/team.png"%>' />
                                <%# DataBinder.Eval(Container.DataItem,"TournamentDate","{0:D}")%>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Country" SortExpression="Country">
            <HeaderTemplate>
                <asp:LinkButton ID="countryHeading" runat="server" CommandName="Sort" CommandArgument="Country">Country</asp:LinkButton>
                <br />
                <asp:DropDownList ID="ddCountry" ForeColor="Black" Width="60" CssClass="mdb-select colorful-select dropdown-primary md-form"
                    AutoPostBack="true"
                    runat="server" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Country") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="TournamentDate" HeaderText="Date" DataFormatString="{0:D}" SortExpression="TournamentDate" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
        <asp:BoundField DataField="TournamentTypeName" HeaderText="Type" InsertVisible="false" SortExpression="TournamentType" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
        <asp:TemplateField HeaderText="Game system" SortExpression="GameSystem">
            <HeaderTemplate>
                <asp:LinkButton ID="gameSystemHeading" runat="server" CommandName="Sort" CommandArgument="GameSystem">Gamesystem</asp:LinkButton>
                <br />
                <asp:DropDownList ID="ddGameSystem" ForeColor="Black" Width="60" CssClass="mdb-select colorful-select dropdown-primary md-form"
                    AutoPostBack="true"
                    DataTextField="Name" DataValueField="Id"
                    runat="server" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("TKGameSystem.Name") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
