using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using static TourneyKeeper.Common.SharedCode.General;

namespace TourneyKeeper.Web
{
    public partial class MassRoll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void CalcClick(object sender, EventArgs e)
        {
            int _attacks = int.Parse(attacks.Text);
            int _bs = int.Parse(bs.Text);
            int _damage = int.Parse(damage.Text);
            int _hitmodifier = int.Parse(hitmodifier.Text);
            int _towound = int.Parse(towound.Text);
            int.TryParse(save.Text, out int _save);
            int.TryParse(feelnopain.Text, out int _feelnopain);
            RerollTypes _rerolltohit = (RerollTypes)Enum.Parse(typeof(RerollTypes), rerolltohit.SelectedValue);
            RerollTypes _rerolltowound = (RerollTypes)Enum.Parse(typeof(RerollTypes), rerolltowound.SelectedValue);
            RerollTypes _rerollsave = (RerollTypes)Enum.Parse(typeof(RerollTypes), rerollsave.SelectedValue);

            var d = General.MassRoll(_attacks, _bs, _damage, _rerolltohit, _hitmodifier, _towound, _rerolltowound, _save, _rerollsave, _feelnopain);

            hits.Text = d.Hits.ToString("#.##");
            woundinghits.Text = d.Wounds.ToString("#.##");
            aftersaves.Text = d.WoundsAfterSaves.ToString("#.##");
            afterfeelnopain.Text = d.WoundsAfterFnp.ToString("#.##");
        }
    }
}