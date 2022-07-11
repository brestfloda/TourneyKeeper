using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TourneyKeeper.DTO.Web;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class TKSinglesPairingExport : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            int? round;
            var player = Session["LoggedInUser"] as TKPlayer;
            General.GuardLogin(id, player, this);

            using (var context = new TourneyKeeperEntities())
            {
                round = context.TKGame.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();

                if (!IsPostBack)
                {
                    RoundDropDown.DataSource = Enumerable.Range(1, round.HasValue ? round.Value : 0).Reverse();
                    RoundDropDown.DataBind();

                    BindData(id, round);
                }
            }
        }

        protected void RoundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            int round = int.Parse(RoundDropDown.SelectedValue);

            BindData(id, round);
        }

        private void BindData(int tournamentId, int? round)
        {
            using (var context = new TourneyKeeperEntities())
            {
                IEnumerable<TeamPairingsData> data = context.TKGame
                    .Where(g => g.TournamentId == tournamentId && g.Round == (round.HasValue ? round.Value : 1))
                    .OrderBy(g => g.TableNumber)
                    .Select(g => new TeamPairingsData()
                    {
                        TableNumber = g.TableNumber,
                        Team1Name = g.TKTournamentPlayer.PlayerName,
                        Team2Name = g.TKTournamentPlayer1.PlayerName,
                    });

                StringBuilder sb = new StringBuilder();

                foreach (var d in data)
                {
                    sb.AppendLine($"{d.TableNumber};{d.Team1Name};{d.Team2Name}");
                }

                ExportDataTextBox.Text = sb.ToString();
            }
        }
    }
}