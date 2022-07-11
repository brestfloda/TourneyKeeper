using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;

namespace TourneyKeeper.Web
{
    public partial class TKShowRanking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                if (!IsPostBack)
                {
                    GameSystemDropDown.DataSource = context.TKGameSystem.OrderBy(r => r.Name).ToList();
                    GameSystemDropDown.DataBind();
                    GameSystemDropDown.SelectedValue = "1";

                    CountryDropdown.DataSource = context.TKRankingType.Where(rt => !rt.Name.Equals("international")).OrderBy(r => r.Name).GroupBy(g => g.Name).Select(g2 => g2.FirstOrDefault()).ToList();
                    CountryDropdown.DataBind();
                    CountryDropdown.SelectedValue = "Denmark";

                    BindRankings(context, 1, "Denmark");
                }
            }
        }

        protected void GameSystemDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
                int gameSystem = int.Parse(GameSystemDropDown.SelectedValue);
                string country = CountryDropdown.SelectedValue.ToString();

                BindRankings(context, gameSystem, country);
            }
        }

        protected void CountryDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
                int gameSystem = int.Parse(GameSystemDropDown.SelectedValue);
                string country = CountryDropdown.SelectedValue.ToString();

                BindRankings(context, gameSystem, country);
            }
        }

        private void BindRankings(TourneyKeeperEntities context, int gameSystem, string country)
        {
            TKRankingType rankingType = context.TKRankingType.SingleOrDefault(r => r.GameSystemId == gameSystem && r.Name == country);

            if (rankingType == null)
            {
                return;
            }

            IEnumerable<RankingGridViewData> rankingData = context.TKRanking.Where(r => r.TKRankingType.Id == rankingType.Id)
                .OrderBy(r => r.Rank).ToList().Select(tp => new RankingGridViewData()
                {
                    Placement = tp.Rank.Value,
                    PlayerName = string.Format("{0} ({1}/{2}/{3})", tp.PlayerName, tp.TKPlayer.Wins, tp.TKPlayer.Losses, tp.TKPlayer.Draws),
                    Points = (int)tp.Points,
                    LatestGame = tp.LatestGame.HasValue ? tp.LatestGame.Value.ToLongDateString() : "",
                    PlayerId = tp.PlayerId
                }).ToList();
            RankingGridView.DataSource = rankingData;
            RankingGridView.DataBind();
        }
    }

    public class RankingGridViewData
    {
        public int PlayerId { get; set; }
        public int Placement { get; set; }
        public string PlayerName { get; set; }
        public int Points { get; set; }
        public string LatestGame { get; set; }
    }
}