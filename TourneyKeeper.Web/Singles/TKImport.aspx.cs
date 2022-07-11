using System;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Exceptions;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class TKImport : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            warningsLabel.Text = "";

            if (IsPostBack)
            {
                XDocument doc;

                try
                {
                    outputTextBox.Text = "";
                    doc = XDocument.Load(importInput.FileContent);
                    var importedPlayers = doc.Descendants("player").Select(p => new
                    {
                        Name = $"{p.Attribute("name").Value} {p.Attribute("lastname").Value}",
                        T3Nickname = p.Attribute("nickname").Value,
                        City = p.Attribute("city").Value,
                        Army = p.Attribute("army").Value,
                        Team = p.Attribute("team").Value,
                        Paid = p.Attribute("paid").Value,
                    });

                    using (var context = new TourneyKeeperEntities())
                    {
                        var tournament = context.TKTournament.Single(t => t.Id == id);
                        var players = context.TKTournamentPlayer.Where(tp => tp.TournamentId == id);
                        var createdNew = 0;
                        var skipped = 0;
                        var added = 0;

                        foreach (var importedPlayer in importedPlayers)
                        {
                            if (players.Any(p => p.TKPlayer.T3Nickname.Equals(importedPlayer.T3Nickname)))
                            {
                                //player found - do nothing
                                skipped++;
                            }
                            else
                            {
                                //player not found - create if not already existing and then add to tournament
                                var player = context.TKPlayer.SingleOrDefault(p => p.T3Nickname.Equals(importedPlayer.T3Nickname));
                                if (player == null)
                                {
                                    //player not found, create first
                                    var playerManager = new PlayerManager();
                                    player = new TKPlayer
                                    {
                                        Name = importedPlayer.Name,
                                        Password = playerManager.EncodePassword("123"),
                                        Username = importedPlayer.T3Nickname,
                                        T3Nickname = importedPlayer.T3Nickname,
                                        City = importedPlayer.City,
                                        Country = tournament.Country
                                    };
                                    context.TKPlayer.Add(player);
                                    context.SaveChanges();
                                    createdNew++;
                                }
                                var codex = context.TKCodex.SingleOrDefault(c => c.Name.Equals(importedPlayer.Army));
                                var tournamentPlayer = new TKTournamentPlayer
                                {
                                    PlayerId = player.Id,
                                    Active = true,
                                    Paid = importedPlayer.Paid.Equals("yes"),
                                    TournamentId = id,
                                    PrimaryCodexId = codex?.Id,
                                    PlayerName = player.Name,
                                    Club = importedPlayer.Team
                                };
                                context.TKTournamentPlayer.Add(tournamentPlayer);
                                context.SaveChanges();
                                added++;
                            }
                        }

                        outputTextBox.Text = $"New players created: {createdNew}{Environment.NewLine}Added players to tournament: {added}{Environment.NewLine}Skipped players: {skipped}";
                    }
                }
                catch (Exception ex)
                {
                    warningsLabel.Text = ex.Message;
                }
            }
        }
    }
}