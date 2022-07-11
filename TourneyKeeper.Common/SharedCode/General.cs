using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common.Managers;
using System.Data.Entity;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TourneyKeeper.Common.SharedCode
{
    public static class General
    {
        private static void GetControlList<T>(ControlCollection controlCollection, List<T> resultCollection) where T : Control
        {
            foreach (Control control in controlCollection)
            {
                if (control is T)
                    resultCollection.Add((T)control);

                if (control.HasControls())
                    GetControlList(control.Controls, resultCollection);
            }
        }

        public static T GetParam<T>(string name)
        {
            try
            {
                var o = HttpContext.Current.Request[name];
                if (o != null)
                {
                    if(typeof(T) == typeof(int))
                    {
                        if(int.TryParse(o, out int result))
                        {
                            return (T)Convert.ChangeType(o, typeof(T));
                        }
                    }
                    else if (o is T)
                    {
                        return (T)Convert.ChangeType(o, typeof(T));
                    }
                }
            }
            catch
            {
                HttpContext.Current.Response.Redirect("/");
            }

            HttpContext.Current.Response.Redirect("/");

            Type type = typeof(T).MakeGenericType();
            return (T)Activator.CreateInstance(type);
        }

        public static T FindControl<T>(ControlCollection controlCollection, string id) where T : Control
        {
            List<T> hyperLinks = new List<T>();
            GetControlList(controlCollection, hyperLinks);
            return hyperLinks.SingleOrDefault(h => h.ID == id);
        }

        public static void RedirectToFrontIfNotReady(TKTournament tournament)
        {
            if (tournament.OnlineSignup && tournament.OnlineSignupStart > DateTime.Now)
            {
                HttpContext.Current.Response.Redirect(string.Format("/Shared/TKSignupTicker.aspx?id={0}", tournament.Id), true);
                HttpContext.Current.Response.Flush();
            }
        }

        public static void ShowOrHideContent(int tournamentId, TKPlayer player, PlaceHolder signupPlaceHolder, PlaceHolder signoutPlaceHolder, PlaceHolder signupContent, PlaceHolder adminPlaceHolder, PlaceHolder aboutPlaceHolder, HyperLink itcExport, Literal titleLiteral, Literal tournamentNameLiteral, Literal contactLiteral)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var tournament = context.TKTournament
                    .Include(t => t.TKSinglesScoringSystem)
                    .Single(t => t.Id == tournamentId);

                titleLiteral.Text = $"TourneyKeeper";
                tournamentNameLiteral.Text = $"{tournament.Name.Substring(0, Math.Min(tournament.Name.Length, 30))}";
                var contactInfo = string.IsNullOrEmpty(tournament.OrganizerEmail) ? "admin@tourneykeeper.net" : tournament.OrganizerEmail;
                contactLiteral.Text = $"Contact: <a href='mailto: {contactInfo}'>{contactInfo}</a>";

                if (tournament.OnlineSignup)
                {
                    if (player == null)
                    {
                        signupContent.Visible = false;
                    }
                    else if (player != null)
                    {
                        var isSignedUp = tournament.TKTournamentPlayer.Any(p => p.PlayerId == player.Id);
                        signupPlaceHolder.Visible = !isSignedUp && tournament.TournamentDate.AddDays(1) > DateTime.Now;
                        signoutPlaceHolder.Visible = isSignedUp && tournament.TournamentDate.AddDays(1) > DateTime.Now;

                        if (!signupPlaceHolder.Visible && !signoutPlaceHolder.Visible)
                        {
                            signupContent.Visible = false;
                        }
                    }
                }
                else
                {
                    signupContent.Visible = false;
                }

                adminPlaceHolder.Visible = IsAdminOrOrganizer(player, tournamentId);

                if (tournament.UseAbout)
                {
                    aboutPlaceHolder.Visible = true;
                }

                if (itcExport != null && !tournament.TKSinglesScoringSystem.Name.Equals("ITC", StringComparison.InvariantCultureIgnoreCase))
                {
                    itcExport.Visible = false;
                }
            }
        }

        public static bool IsAdminOrOrganizer(TKPlayer player, int tournamentId)
        {
            return player != null && (player.IsAdmin || player.IsPlayerOrganizer(tournamentId));
        }

        public static bool IsAdminOrOrganizer(string token, int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var player = context.TKPlayer.SingleOrDefault(p => p.Token == token);
                if (player == null)
                {
                    return false;
                }
                if (player.IsAdmin)
                {
                    return true;
                }

                var res = context.TKOrganizer.SingleOrDefault(o => o.PlayerId == player.Id && o.TournamentId == tournamentId);

                return res != null ? true : false;
            }
        }

        public static void GuardLogin(int tournamentId, TKPlayer player, Page page)
        {
            if (player != null)
            {
                if (!(player.IsPlayerOrganizer(tournamentId) || player.IsAdmin))
                {
                    page.Response.Redirect("/");
                }
            }
            else
            {
                page.Response.Redirect("/");
            }
        }

        public enum RerollTypes
        {
            FullReroll,
            Reroll1s,
            NoReroll
        }

        public struct Damage
        {
            public float Hits { get; set; }
            public float Wounds { get; set; }
            public float WoundsAfterSaves { get; set; }
            public float WoundsAfterFnp { get; set; }
        }

        public static Damage MassRoll(
            int attacks,
            int bs,
            int damage,
            RerollTypes rerollToHit,
            int hitModifier,
            int toWound,
            RerollTypes rerollToWound,
            int save,
            RerollTypes rerollSave,
            int fnp)
        {
            bs = Math.Max(bs, 2);
            bs = Math.Min(bs, 6);
            float hits = attacks * (7.0f - (Math.Max(Math.Min(bs - hitModifier, 6), 2))) / 6.0f;
            switch (rerollToHit)
            {
                case RerollTypes.FullReroll:
                    hits += (attacks - hits) * ((Math.Min(6.0f + hitModifier, 6.0f)) / 6.0f) * (7.0f - (Math.Max(Math.Min(bs - hitModifier, 6), 2))) / 6.0f;
                    break;
                case RerollTypes.Reroll1s:
                    hits += attacks * (1.0f / 6.0f) * (7.0f - (Math.Max(Math.Min(bs - hitModifier, 6), 2))) / 6.0f;
                    break;
                case RerollTypes.NoReroll:
                default:
                    break;
            }
            float wounds = hits * (7.0f - (Math.Max(Math.Min(toWound, 6), 2))) / 6.0f;
            switch (rerollToWound)
            {
                case RerollTypes.FullReroll:
                    wounds += (hits - wounds) * (7.0f - (Math.Max(Math.Min(toWound, 6), 2))) / 6.0f;
                    break;
                case RerollTypes.Reroll1s:
                    wounds += hits * (1.0f / 6.0f) * (7.0f - (Math.Max(Math.Min(toWound, 6), 2))) / 6.0f;
                    break;
                case RerollTypes.NoReroll:
                default:
                    break;
            }
            float woundsAfterSaves = wounds * (1.0f - (7.0f - (Math.Max(Math.Min(save, 6), 2))) / 6.0f);
            switch (rerollToWound)
            {
                case RerollTypes.FullReroll:
                    woundsAfterSaves -= (wounds - woundsAfterSaves) * (1.0f - (7.0f - (Math.Max(Math.Min(save, 6), 2))) / 6.0f);
                    break;
                case RerollTypes.Reroll1s:
                    woundsAfterSaves -= wounds * (1.0f / 6.0f) * (1.0f - (7.0f - (Math.Max(Math.Min(save, 6), 2))) / 6.0f);
                    break;
                case RerollTypes.NoReroll:
                default:
                    break;
            }
            woundsAfterSaves = save == 0 ? wounds : woundsAfterSaves;
            float woundsAfterFnp = fnp == 0 ? woundsAfterSaves : woundsAfterSaves * (1.0f - (float)Math.Pow((7.0f - (Math.Max(Math.Min(fnp, 6), 2))) / 6.0f, damage));

            return new Damage
            {
                Hits = hits,
                Wounds = wounds,
                WoundsAfterFnp = woundsAfterFnp,
                WoundsAfterSaves = woundsAfterSaves
            };
        }
    }
}