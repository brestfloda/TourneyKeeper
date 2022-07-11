<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CodexLeaderboard.ascx.cs" Inherits="TourneyKeeper.Web.Controls.CodexLeaderboard" %>

<script type="text/javascript">
    $("#statisticsli").addClass("active");
</script>
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
<script type="text/javascript">
    // Load google charts
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(drawChart);

    // Draw the chart and set the chart values
    function drawChart() {

        var codexmodel = {
            tournamentId: getUrlVars().id
        };

        $.getJSON('/webapi/codex/GetCodexCount', codexmodel)
            .done(function (res) {
                if (!res.Success) {
                    alert('Could not get codices from server');
                }
                else {
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', 'Name');
                    data.addColumn('number', 'Count');

                    JSON.parse(res.Message).forEach(function (row) {
                        data.addRow([
                            row.Name,
                            row.Count
                        ]);
                    });

                    var options = { 'title': 'Primary factions', 'width': 550, 'height': 400 };
                    var chart = new google.visualization.PieChart(document.getElementById('piechart'));
                    chart.draw(data, options);
                }
            });
    }
</script>

<div class="row-fluid">
    <div class="col-md-12 padding-0">
        <div id="piechart"></div>
    </div>
</div>
<div class="row-fluid">
    <div class="col-md-12 padding-0">
        <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
        <asp:GridView ID="LeaderboardGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" AllowSorting="true" OnSorting="LeaderboardGridViewSorting" GridLines="None">
            <Columns>
                <asp:BoundField DataField="Placement" HeaderText="Placement" InsertVisible="false" SortExpression="Placement" />
                <asp:BoundField DataField="PrimaryCodex" HeaderText="Primary codex" InsertVisible="false" SortExpression="PrimaryCodex" />
                <asp:BoundField DataField="Count" HeaderText="Count" InsertVisible="false" SortExpression="Count" />
                <asp:BoundField DataField="StdDev" HeaderText="StdDev" DataFormatString="{0:F1}" InsertVisible="false" SortExpression="StdDev" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                <asp:BoundField DataField="Points" HeaderText="Points" DataFormatString="{0:F1}" InsertVisible="false" SortExpression="Points" />
            </Columns>
        </asp:GridView>
    </div>
</div>
