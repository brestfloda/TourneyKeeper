<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeamScoreWeightedLeaderboard.aspx.cs" Inherits="TourneyKeeper.Web.TeamScoreWeightedLeaderboard" MasterPageFile="TKTeam.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#statisticsli").addClass("active");
    </script>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:GridView ID="LeaderboardGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Placement" HeaderText="Position" InsertVisible="false" />
                    <asp:BoundField DataField="Name" HeaderText="Team" InsertVisible="false" />
                    <asp:BoundField DataField="Points" HeaderText="Points" InsertVisible="false" />
                    <asp:BoundField DataField="OpponentsPoints" HeaderText="Opponents points" InsertVisible="false" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="TotalPoints" HeaderText="Total points" InsertVisible="false" />
                    <asp:BoundField DataField="TotalOpponentsPoints" HeaderText="Total opponents points" InsertVisible="false" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:TemplateField HeaderText="Dorner Points" InsertVisible="false">
                        <HeaderTemplate>
                            <asp:Label ID="SubnetHeader" ToolTip="(Opponents battlepoints * players battlepoints) / 500" runat="server" Text="Dorner Points"></asp:Label></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSubnet" runat="server" Text='<%# Bind("WeightedScore") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
