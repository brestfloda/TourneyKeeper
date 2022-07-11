<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKMyTournaments.aspx.cs" Inherits="TourneyKeeper.Web.TKMyTournaments" MasterPageFile="TKShared.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#mytournamentsli").addClass("active");
    </script>

    <div class="row-fluid">
        <div class="col-md-12">
            <asp:GridView ID="myTournamentsGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" GridLines="None">
                <Columns>
                    <asp:TemplateField HeaderText="Tournament">
                        <ItemTemplate>
                            <asp:HyperLink ID="TournamentButton" runat="Server" NavigateUrl='<%# string.Format("/Shared/TKCreateTournament.aspx?Id={0}&PlayerId={1}", Eval("Id"), Eval("PlayerId")) %>'><%#Eval("Name") %></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Country" HeaderText="Country" InsertVisible="false" SortExpression="Country" />
                    <asp:BoundField DataField="TournamentDate" HeaderText="Tournament date" InsertVisible="false" DataFormatString="{0:D}" SortExpression="TournamentDate" />
                    <asp:BoundField DataField="TournamentTypeName" HeaderText="Type" InsertVisible="false" SortExpression="TournamentType" />
                    <asp:BoundField DataField="TKGameSystemName" HeaderText="System" InsertVisible="false" SortExpression="TKGameSystem.Name" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
