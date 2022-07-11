<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKCreateTournament.aspx.cs" Inherits="TourneyKeeper.Web.TKCreateTournament" MasterPageFile="TKShared.master" ValidateRequest="false" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#createtournamentli").addClass("active");

        $(document).ready(function () {
            $('.mdb-select').material_select();

            allowEditCheckBox.checked = $("#AllowEditHiddenField").val() == "True";
            nationalTournamentCheckBox.checked = $("#NationalTournamentHiddenField").val() == "True";
            hideResultsForRoundCheckBox.checked = $("#HideResultsForRoundHiddenField").val() == "True";
            showSoftScoresCheckBox.checked = $("#ShowSoftScoresHiddenField").val() == "True";
            useSecondaryCheckBox.checked = $("#UseSecondaryHiddenField").val() == "True";
            onlineSignupCheckBox.checked = $("#OnlineSignupHiddenField").val() == "True";
            useAboutCheckBox.checked = $("#UseAboutHiddenField").val() == "True";
            playersDefaultActiveCheckBox.checked = $("#PlayersDefaultActiveHiddenField").val() == "True";
            useSeedCheckBox.checked = $("#UseSeedHiddenField").val() == "True";
            playersDefaultActiveCheckBox.checked = $("#PlayersDefaultActiveHiddenField").val() == "True";
            useSeedCheckBox.checked = $("#UseSeedHiddenField").val() == "True";
            if ($("#activeCheckBox").length) {
                activeCheckBox.checked = $("#ActiveHiddenField").val() == "True";
            }

            ShowDatePicker("#<%=tournamentDateTextbox.ClientID%>");
            ShowDatePicker("#<%=tournamentEndDateTextbox.ClientID%>");
            ShowDatePicker("#<%=onlineSignupStartTextbox.ClientID%>");
            ShowDatePicker("#<%=showListsDateTextBox.ClientID%>");

            $('[data-toggle="tooltip"]').tooltip();

            if ($("#tournamentTypeDropDown").val() == 2) { //team
                document.getElementById('teamSettings').style.display = '';
                ShowHideTeamScoring();
            }
            else { //singles
                document.getElementById('singlesSettings').style.display = '';
                document.getElementById('teamscoringDiv').style.display = 'none';
                if ($('#singlesScoringDropDown').val == 3) //ITC
                {
                    document.getElementById('useSeedCheckBoxDiv').style.display = 'none';
                    document.getElementById('useSecondaryCheckBoxDiv').style.display = 'none';
                }
                else {
                    document.getElementById('useSeedCheckBoxDiv').style.display = '';
                    document.getElementById('useSecondaryCheckBoxDiv').style.display = '';
                }
            }

            if ($("#useAboutCheckBox")[0].checked) {
                $("#aboutRow").show();
            }
            else {
                $("#aboutRow").hide();
            }
        });

        function ShowHideTeamScoring() {
            if ($("#teamScoringDropDown").val() === '<%=(int)TourneyKeeper.Common.TeamScoringSystem.Cutoff%>') {
                document.getElementById('teamscoringDiv').style.display = '';
                $("#teamScoreLow").text("Max score for loss");
                $("#teamScoreHigh").text("Min score for win");
            }
            else if ($("#teamScoringDropDown").val() === '<%=(int)TourneyKeeper.Common.TeamScoringSystem.Max%>') {
                document.getElementById('teamscoringDiv').style.display = '';
                $("#teamScoreLow").text("Low cap");
                $("#teamScoreHigh").text("High cap");
            }
            else {
                document.getElementById('teamscoringDiv').style.display = 'none';
            }
        }

        function HideTextBox(ddlId) {
            var data = '';
            if (ddlId == 'ITC') {
                data = 'ITC';
            }
            else {
                data = $("#" + ddlId + "").val();
            }

            if (data == 1 || data == 'ITC') {//singles
                document.getElementById('teamSettings').style.display = 'none';
                document.getElementById('singlesSettings').style.display = '';
                if ($('#singlesScoringDropDown').val() == 3) //ITC
                {
                    document.getElementById('useSeedCheckBoxDiv').style.display = 'none';
                    document.getElementById('useSecondaryCheckBoxDiv').style.display = 'none';
                }
                else {
                    document.getElementById('useSeedCheckBoxDiv').style.display = '';
                    document.getElementById('useSecondaryCheckBoxDiv').style.display = '';
                }
            }
            else {//team
                document.getElementById('teamSettings').style.display = '';
                document.getElementById('singlesSettings').style.display = 'none';
                document.getElementById('useSeedCheckBoxDiv').style.display = '';
                document.getElementById('useSecondaryCheckBoxDiv').style.display = '';
            }
        }
    </script>

    <asp:HiddenField ID="tournamentIdHidden" runat="server" />
    <asp:HiddenField ID="HideResultsForRoundHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="TournamentTypeHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="CountryHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="SinglesScoringHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="TeamScoringHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="ActiveHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="GameSystemHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="AllowEditHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="NationalTournamentHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="RequiredToReportHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="ShowSoftScoresHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="UseSecondaryHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="OnlineSignupHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="UseAboutHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="PlayersDefaultActiveHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="UseSeedHiddenField" ClientIDMode="Static" runat="server" />

    <div class="modal" tabindex="-1" role="dialog" id="teamModal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Team info</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p><b>Battlefront scoring</b></p>
                    <p>Used for Flames of War</p>
                    <p><b>Cutoff</b></p>
                    <p>
                        From ETC 40K. If one team accumulates enough battle points for a win, then that team wins 2 points and the losing team gets 0 points. Otherwise the match ends 1-1.
                                The lower score is the maximum amount of points you can achieve without winning the match and the higher score is the minimum amount of points required to win the match.
                    </p>
                    <p><b>Max</b></p>
                    <p>
                        From ETC 9th Age. The match points are capped at top and bottom battle points.
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal" tabindex="-1" role="dialog" id="singlesModal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Singles info</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p><b>Players default active</b></p>
                    <p>When a player is signed up for the tournament, this determines whether or not players are active by default or not.</p>
                    <p><b>Singles scoring</b></p>
                    <p>ITC follows the rules for pairings and determining the winner as detailed in the rules of ITC.</p>
                    <p><b>Use seed</b></p>
                    <p>When using "Use seed", then you must setup seeds for each player.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal" tabindex="-1" role="dialog" id="optionsModal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Options info</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p><b>Allow players to report results</b></p>
                    <p>Highly recommended to enable this.</p>
                    <p><b>Required to report</b></p>
                    <p>Using player in tournament is recommended. People are usually quite helpful.</p>
                    <p><b>National tournament</b></p>
                    <p>If checked, then the tournament will only show to people of the selected country on the front page.</p>
                    <p><b>Show soft scores</b></p>
                    <p>You can enter soft scores and choose to show them at the end of the tournament. Soft scores are not used for pairings.</p>
                    <p><b>Open for online signup</b></p>
                    <p>Select the date and time of your choosing for when the tournament is open for signups.</p>
                    <p><b>Use secondary points</b></p>
                    <p>Depends on scoring for your tournament.</p>
                    <p><b>Organizer email</b></p>
                    <p>If you write an organizer email, this will be shown at the bottom of all pages concerning the tournament. Otherwise, admin@tourneykeeper.net is shown.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="row-fluid">
        <div class="card card-primary mb-2">
            <div class="card-body">
                <h4 class="card-title">Basic information</h4>
                <div class="row">
                    <div class="col-md-6">
                        <select class="mdb-select md-form colorful-select dropdown-primary" id="gameSystemDropDown" onchange="javascript:$('#GameSystemHiddenField').val(this.value);">
                            <asp:Literal ID="gameSystemDropDownLiteral" runat="server"></asp:Literal>
                        </select>
                        <label class="mdb-main-label">Game system</label>
                    </div>
                    <div class="col-md-6">
                        <div class="md-form">
                            <asp:TextBox ID="tournamentNameTextbox" ClientIDMode="Static" CssClass="form-control" runat="server" autofocus="true"></asp:TextBox>
                            <label for="tournamentNameTextbox">Tournament name</label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="tournamentNameTextbox" ErrorMessage="Please enter a name for the tournament" ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="md-form">
                            <asp:TextBox ID="tournamentDateTextbox" CssClass="form-control" runat="server" placeholder="DD-MM-YYYY" autocomplete="off"></asp:TextBox>
                            <label for="tournamentDateTextbox">Tournament start</label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="tournamentDateTextbox" ErrorMessage="Please enter a date for the tournament" ForeColor="Red" Display="Dynamic" />
                            <asp:RangeValidator runat="server" ID="rngDate" ControlToValidate="tournamentDateTextbox" Type="Date" MinimumValue="01-01-2013" MaximumValue="31-12-2030" ErrorMessage="Please enter a valid date" Display="Dynamic" ForeColor="Red" />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="md-form">
                            <asp:TextBox ID="tournamentEndDateTextbox" CssClass="form-control" runat="server" placeholder="DD-MM-YYYY" autocomplete="off"></asp:TextBox>
                            <label for="tournamentEndDateTextbox">Tournament end</label>
                            <asp:RangeValidator runat="server" ID="RangeValidator2" ControlToValidate="tournamentEndDateTextbox" Type="Date" MinimumValue="01-01-2013" MaximumValue="31-12-2030" ErrorMessage="Please enter a valid date" Display="Dynamic" ForeColor="Red" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <select class="mdb-select md-form colorful-select dropdown-primary" id="tournamentTypeDropDown" onchange="javascript:$('#TournamentTypeHiddenField').val(this.value);HideTextBox('tournamentTypeDropDown');">
                            <asp:Literal ID="tournamentTypeDropDownLiteral" runat="server"></asp:Literal>
                        </select>
                        <label class="mdb-main-label">Tournament type</label>
                    </div>
                    <div class="col-md-6">
                        <select class="mdb-select md-form colorful-select dropdown-primary" id="countryDropDown" onchange="javascript:$('#CountryHiddenField').val(this.value);">
                            <asp:Literal ID="countryDropDownLiteral" runat="server"></asp:Literal>
                        </select>
                        <label class="mdb-main-label">Country</label>
                    </div>
                </div>
                <div class="row">
                    <asp:PlaceHolder ID="activeRowPlaceHolder" runat="server" Visible="false">
                        <div class="col-md-6 form-check">
                            <input type="checkbox" class="form-check-input" id="activeCheckBox" onchange="javascript:$('#ActiveHiddenField').val($('#activeCheckBox').is(':checked'));">
                            <label class="form-check-label" for="activeCheckBox">Active</label>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>
        </div>
        <div id="teamSettings" style="display: none">
            <div class="card card-primary mb-2">
                <div class="card-body">
                    <h4 class="card-title">Team settings&nbsp;
                        <button type="button" data-toggle="modal" data-target="#teamModal" class="btn-sm btn-default">Help<i class="fas fa-info ml-1"></i></button></h4>
                    <div class="row">
                        <div class="col-md-6">
                            <select class="mdb-select md-form colorful-select dropdown-primary" id="teamScoringDropDown" onchange="javascript:$('#TeamScoringHiddenField').val(this.value);ShowHideTeamScoring();">
                                <asp:Literal ID="teamScoringDropDownLiteral" runat="server"></asp:Literal>
                            </select>
                            <label class="mdb-main-label">Team scoring</label>
                        </div>
                        <div class="col-md-6">
                            <div class="md-form">
                                <asp:TextBox ID="teamSizeTextbox" TextMode="Number" min="2" max="8" CssClass="form-control" runat="server" autofocus="true"></asp:TextBox>
                                <label for="teamSizeTextbox">Team size</label>
                                <asp:RangeValidator runat="server" ID="RangeValidator5" ControlToValidate="teamSizeTextbox" Type="Integer" MinimumValue="2" MaximumValue="8" ErrorMessage="Please enter a whole number" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>
                    <div class="row" id="teamscoringDiv">
                        <div class="col-md-3">
                            <div class="md-form">
                                <asp:TextBox ID="maxLossTextBox" ClientIDMode="Static" CssClass="form-control" runat="server" TextMode="Number" min="1" max="100"></asp:TextBox>
                                <label id="teamScoreLow" for="maxLossTextBox">Max score for loss (low)</label>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="md-form">
                                <asp:TextBox ID="minWinTextBox" ClientIDMode="Static" CssClass="form-control" runat="server" TextMode="number" min="1" max="100"></asp:TextBox>
                                <label id="teamScoreHigh" for="minWinTextBox">Min score for win (high)</label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="singlesSettings" style="display: none">
            <div class="card card-primary mb-2">
                <div class="card-body">
                    <h4 class="card-title">Singles settings&nbsp;
                        <button type="button" data-toggle="modal" data-target="#singlesModal" class="btn-sm btn-default">Help<i class="fas fa-info ml-1"></i></button></h4>
                    <div class="row">
                        <div class="col-md-6 form-check align-self-center">
                            <input type="checkbox" class="form-check-input" id="playersDefaultActiveCheckBox" onchange="javascript:$('#PlayersDefaultActiveHiddenField').val($('#playersDefaultActiveCheckBox').is(':checked'));">
                            <label class="form-check-label" for="playersDefaultActiveCheckBox">Players default active</label>
                        </div>
                        <div class="col-md-6">
                            <select class="mdb-select md-form colorful-select dropdown-primary" id="singlesScoringDropDown" onchange="javascript:$('#SinglesScoringHiddenField').val(this.value);HideTextBox('ITC');">
                                <asp:Literal ID="singlesScoringDropDownLiteral" runat="server"></asp:Literal>
                            </select>
                            <label class="mdb-main-label">Singles scoring</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 form-check align-self-center" id="useSeedCheckBoxDiv">
                            <input type="checkbox" class="form-check-input" id="useSeedCheckBox" onchange="javascript:$('#UseSeedHiddenField').val($('#useSeedCheckBox').is(':checked'));">
                            <label class="form-check-label" for="useSeedCheckBox">Use seed</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="card card-primary mb-2">
            <div class="card-body">
                <h4 class="card-title">Options&nbsp;
                        <button type="button" data-toggle="modal" data-target="#optionsModal" class="btn-sm btn-default">Help<i class="fas fa-info ml-1"></i></button></h4>
                <div class="row">
                    <div class="col-md-6 form-check align-self-center">
                        <input type="checkbox" class="form-check-input" id="nationalTournamentCheckBox" onchange="javascript:$('#NationalTournamentHiddenField').val($('#nationalTournamentCheckBox').is(':checked'));">
                        <label class="form-check-label" for="nationalTournamentCheckBox">National tournament</label>
                    </div>
                    <div class="col-md-6">
                        <div class="md-form">
                            <asp:TextBox ID="maxPlayersTextbox" TextMode="Number" max="200" CssClass="form-control" runat="server" autofocus="true"></asp:TextBox>
                            <label for="maxPlayersTextbox">Max number of players</label>
                            <asp:RangeValidator runat="server" ID="RangeValidator1" ControlToValidate="maxPlayersTextbox" Type="Integer" MinimumValue="0" MaximumValue="1000" ErrorMessage="Please enter a whole number" Display="Dynamic" ForeColor="Red" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 form-check align-self-center">
                        <input type="checkbox" class="form-check-input" id="allowEditCheckBox" onchange="javascript:$('#AllowEditHiddenField').val($('#allowEditCheckBox').is(':checked'));">
                        <label class="form-check-label" for="allowEditCheckBox">Allow players to report results</label>
                    </div>
                    <div class="col-md-6">
                        <select class="mdb-select md-form colorful-select dropdown-primary" id="requiredToReportDropDown" onchange="javascript:$('#RequiredToReportHiddenField').val(this.value);">
                            <asp:Literal ID="requiredToReportLiteral" runat="server"></asp:Literal>
                        </select>
                        <label class="mdb-main-label">Required to report</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 form-check align-self-center">
                        <input type="checkbox" class="form-check-input" id="showSoftScoresCheckBox" onchange="javascript:$('#ShowSoftScoresHiddenField').val($('#showSoftScoresCheckBox').is(':checked'));">
                        <label class="form-check-label" for="showSoftScoresCheckBox">Show soft scores</label>
                    </div>
                    <div class="col-md-3">
                        <div class="md-form">
                            <asp:TextBox ID="showListsDateTextBox" ClientIDMode="Static" placeholder="DD-MM-YYYY - blank for tournament start" CssClass="form-control" runat="server" autofocus="true"></asp:TextBox>
                            <label for="showListsDateTextBox">When lists are visible</label>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="md-form">
                            <asp:TextBox ID="showListsDateTimeTextBox" ClientIDMode="Static" placeholder="DD-MM-YYYY - blank for tournament start" CssClass="form-control" runat="server" autofocus="true">00:00</asp:TextBox>
                            <label for="showListsDateTimeTextBox">Time</label>
                        </div>
                        <asp:CustomValidator runat="server" ID="CustomValidator1" ControlToValidate="showListsDateTimeTextBox" OnServerValidate="ShowListsDateTimeServerValidate" ErrorMessage="Please enter only time e.g. '12:00'" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 form-check align-self-center">
                        <input type="checkbox" class="form-check-input" id="onlineSignupCheckBox" onchange="javascript:$('#OnlineSignupHiddenField').val($('#onlineSignupCheckBox').is(':checked'));">
                        <label class="form-check-label" for="onlineSignupCheckBox">Open for online signup</label>
                    </div>
                    <div class="col-md-3">
                        <div class="md-form">
                            <asp:TextBox ID="onlineSignupStartTextbox" ClientIDMode="Static" placeholder="DD-MM-YYYY - blank for tournament start" CssClass="form-control" runat="server" autofocus="true"></asp:TextBox>
                            <label for="onlineSignupStartTextbox">Online signup start</label>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="md-form">
                            <asp:TextBox ID="onlineSignupStartTimeTextbox" ClientIDMode="Static" placeholder="DD-MM-YYYY - blank for tournament start" CssClass="form-control" runat="server" autofocus="true">00:00</asp:TextBox>
                            <label for="onlineSignupStartTimeTextbox">Time</label>
                        </div>
                        <asp:CustomValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="onlineSignupStartTimeTextbox" OnServerValidate="OnlineSignupStartTimeServerValidate" ErrorMessage="Please enter only time e.g. '12:00'" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 form-check align-self-center">
                        <input type="checkbox" class="form-check-input" id="hideResultsForRoundCheckBox" onchange="javascript:$('#HideResultsForRoundHiddenField').val($('#hideResultsForRoundCheckBox').is(':checked'));">
                        <label class="form-check-label" for="hideResultsForRoundCheckBox">Hide results for current round</label>
                    </div>
                    <div class="col-md-6">
                        <div class="md-form">
                            <asp:TextBox ID="organizerEmailTextBox" ClientIDMode="Static" CssClass="form-control" runat="server" autofocus="true"></asp:TextBox>
                            <label for="organizerEmailTextBox">Organizer email</label>
                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="organizerEmailTextBox" ErrorMessage="Invalid Email Format" ForeColor="Red" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 form-check align-self-center" id="useSecondaryCheckBoxDiv">
                        <input type="checkbox" class="form-check-input" id="useSecondaryCheckBox" onchange="javascript:$('#UseSecondaryHiddenField').val($('#useSecondaryCheckBox').is(':checked'));">
                        <label class="form-check-label" for="useSecondaryCheckBox">Use secondary points</label>
                    </div>
                    <div class="col-md-6">
                    </div>
                </div>
            </div>
        </div>
        <div class="card card-primary mb-2">
            <div class="card-body">
                <h4 class="card-title">About</h4>
                <div class="row">
                    <div class="col-md-6 form-check align-self-center">
                        <input type="checkbox" clientidmode="Static" class="form-check-input" id="useAboutCheckBox" onchange="javascript:$('#UseAboutHiddenField').val($('#useAboutCheckBox').is(':checked'));">
                        <label class="form-check-label" for="useAboutCheckBox">Use About</label>
                    </div>
                </div>
                <div class="row" id="aboutRow">
                    <div class="col-md-2">
                        <asp:Label runat="server" AssociatedControlID="descriptionTextbox">About:</asp:Label>
                    </div>
                    <div class="col-md-10">
                        <script type="text/javascript" src="../Tools/nicEdit/nicEdit.js"></script>
                        <script type="text/javascript">
                            //<![CDATA[
                            bkLib.onDomLoaded(function () {
                                var myInstance = new nicEditor().panelInstance('descriptionTextbox');
                                myInstance.addEvent('blur', function () {
                                    $("#descriptionTextbox").text(new nicEditors.findEditor('descriptionTextbox').getContent());
                                });
                            });
                                //]]>
                        </script>
                        <asp:TextBox ID="descriptionTextbox" ClientIDMode="Static" TextMode="multiline" Style="width: 100%" Rows="10" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <asp:LinkButton ID="okButton" OnClick="okButton_Click" runat="server" Text="Create Tournament" CssClass="btn btn-large btn-primary" />
        </div>
    </div>
    <script type="text/javascript">
        $("#useAboutCheckBox").click(function () {
            if (this.checked) {
                $("#aboutRow").show();
            }
            else {
                $("#aboutRow").hide();
            }
        });
    </script>
</asp:Content>
