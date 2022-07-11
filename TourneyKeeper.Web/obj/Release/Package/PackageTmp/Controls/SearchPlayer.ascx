<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchPlayer.ascx.cs" Inherits="TourneyKeeper.Web.Controls.SearchPlayer" %>

<script type="text/javascript">
    function SearchClick() {
        var tournamentId = getUrlVars().id;
        if (!tournamentId) {
            tournamentId = getUrlVars().Id;
        }
        tournamentId = tournamentId.replace("#", "");

        Search(tournamentId, $("#playerNameTextbox").val(), SearchCallback);
    }

    function Search(tournamentId, searchString, callback) {
        var model = {
            TournamentId: tournamentId,
            SearchString: searchString
        };

        $.post('/webapi/player/search', model)
            .done(function (res) {
                if (!res.Success) {
                    callback(false);
                }
                else {
                    callback(res);
                }
            });
    }

    function SearchCallback(res) {
        if (!res) {
            alert('Search failed');
            return;
        }
        var tournamentId = getUrlVars().id;
        if (!tournamentId) {
            tournamentId = getUrlVars().Id;
        }
        tournamentId = tournamentId.replace("#", "");
        $("#playerTable tr").remove();
        $.each(JSON.parse(res.Message), function (key, value) {
            playerAddRow(tournamentId, value.PlayerId, value.PlayerName);
        });
    }

    function playerAddRow(tournamentId, playerId, playerName) {
        if ($("#playerTable tbody").length === 0) {
            $("#playerTable").append("<tbody></tbody>");
        }
        $("#playerTable tbody").append(
            playerBuildTableRow(tournamentId, playerId, playerName));
    }

    function playerBuildTableRow(tournamentId, playerId, playerName) {
        var ret =
            "<tr>" +
            "<td><a href='javascript:AddPlayerClick(" + tournamentId + ", " + playerId + ", \"" + playerName.replace(/[\""]/g, '\\"') + "\")'>" + playerName + "</a></td>" +
            "</tr>";
        return ret;
    }

    function AddPlayerClick(tournamentId, playerId, playerName) {
        var model = {
            TournamentId: tournamentId,
            PlayerId: playerId,
            PlayerName: playerName
        };

        $.post('/webapi/tournament/addplayer', model)
            .done(function (res) {
                if (!res.Success) {
                    alert('Something went wrong when adding player to tournament');
                }
                else {
                    window.location.reload(true);
                }
            });
    }
</script>

<div class="row-fluid">
    <div class="col-md-12 padding-0">
        <button disabled="disabled" type="button" class="btn btn-large btn-primary" onclick="javascript:OpenModal('#addPlayerModal');">Add Player</button>
        Number of active players:
                <asp:Label ID="numPlayersLabel" ClientIDMode="Static" runat="server"></asp:Label>
    </div>
</div>
<div class="modal fade" id="addPlayerModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modalLabel">Find player</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <h5>Search for player below.</h5>
                Search:
                            <asp:TextBox ID="playerNameTextbox" ClientIDMode="Static" runat="server"></asp:TextBox>
                &nbsp;
                <a href="#" onclick="SearchClick();return false;" id="searchHref" class="btn btn-primary noWrapLink" style="padding: 13px 13px">Search</a>

                <div class="row">
                    <div class="col-md-12">
                        <table id="playerTable"
                            class="table table-bordered
                 table-condensed table-striped">
                            <thead>
                                <tr>
                                    <th>Player</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <a href="javascript:CloseModal('#addPlayerModal');" class="btn btn-large btn-primary">Close</a>
            </div>
        </div>
    </div>
</div>
