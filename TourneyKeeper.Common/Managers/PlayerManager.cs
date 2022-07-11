using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Common.Managers
{
    public class PlayerManager : IManager<TKPlayer>
    {
        public IList<int> GetTournamentIds(int playerId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKTournamentPlayer.Where(tp => tp.PlayerId == playerId).Select(tp => tp.TournamentId).ToList();
            }
        }

        public IList<TKPlayer> Search(int tournamentId, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return new List<TKPlayer>();
            }

            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var result = context.TKPlayer
                    .Where(p => p.Name.Contains(searchString) || p.Email.Contains(searchString))
                    .Where(p => !context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournamentId).Select(tp => tp.PlayerId).Contains(p.Id))
                    .OrderBy(p => p.Name)
                    .ToList();

                return result;
            }
        }

        public IList<TKPlayer> SearchOrganizer(int tournamentId, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return new List<TKPlayer>();
            }

            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var result = context.TKPlayer
                    .Where(p => p.Name.Contains(searchString) || p.Email.Contains(searchString))
                    .Where(p => !context.TKOrganizer.Where(tp => tp.TournamentId == tournamentId).Select(tp => tp.PlayerId).Contains(p.Id))
                    .OrderBy(p => p.Name)
                    .ToList();

                return result;
            }
        }

        public IList<int> GetOrganizerTournamentIds(int playerId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKOrganizer.Where(tp => tp.PlayerId == playerId).Select(tp => tp.TournamentId).ToList();
            }
        }

        public int GetPlayerId(string token)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKPlayer.SingleOrDefault(p => p.Token == token).Id;
            }
        }

        public string EncodePassword(string password)
        {
            SHA512 sha = SHA512.Create();
            byte[] newData = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
            string newHash = Encoding.ASCII.GetString(newData);

            return newHash;
        }

        public TKPlayer Logon(string token)
        {
            using (var context = new TourneyKeeperEntities())
            {
                TKPlayer tmp = context.TKPlayer.SingleOrDefault(p => p.Token.Equals(token));

                if (tmp != null)
                {
                    if (tmp.Token == null)
                    {
                        tmp.Token = Guid.NewGuid().ToString();
                        context.SaveChanges();
                    }
                    tmp.LastLoggedIn = DateTime.Now;
                    context.SaveChanges();
                }

                return tmp;
            }
        }

        public TKPlayer Logon(string username, string password)
        {
            using (var context = new TourneyKeeperEntities())
            {
                SHA512 sha = SHA512.Create();
                byte[] dbData = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
                string dbHash = Encoding.ASCII.GetString(dbData);

                TKPlayer tmp = context.TKPlayer.SingleOrDefault(p =>
                    ((p.Username.Equals(username) || p.Email.Equals(username)) && p.Password.Equals(dbHash)) ||
                    ((p.Username.Equals(username) || p.Email.Equals(username)) && p.Password == "" && password == ""));

                if (tmp != null)
                {
                    if (tmp.Token == null)
                    {
                        tmp.Token = Guid.NewGuid().ToString();
                        context.SaveChanges();
                    }
                    tmp.LastLoggedIn = DateTime.Now;
                    context.SaveChanges();
                }

                return tmp;
            }
        }

        private string RandomString(int size)
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public void ResetPassword(string email)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var tmpPlayer = context.TKPlayer.SingleOrDefault(p => p.Email.Equals(email));
                if (tmpPlayer == null)
                {
                    throw new Exception("E-mail not found");
                }

                string newPassword = RandomString(6);

                tmpPlayer.Password = EncodePassword(newPassword);

                context.SaveChanges();

                var emailManager = new EmailManager();
                try
                {
                    emailManager.SendNewPassword(email, newPassword);
                }
                catch (Exception e)
                {
                    LogManager.LogError($"{email} - {e.Message}");
                    throw new Exception("There was a problem sending your new password");
                }
            }
        }

        public int[] GetJoinedFutureTournamentIds(int id)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var tmp = context.TKTournamentPlayer
                    .Where(tp => tp.PlayerId == id &&
                        DbFunctions.AddDays(tp.TKTournament.TournamentEndDate, 5) > DateTime.Now &&
                        tp.TKTournament.UseAbout &&
                        tp.TKTournament.Active == true)
                    .Select(tp => tp.TournamentId)
                    .ToArray();
                return tmp;
            }
        }

        public TKPlayer Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TKPlayer entity, string token)
        {
            throw new NotImplementedException();
        }

        public void SetNotificationToken(string playerToken, string notificationToken)
        {
            using (var context = new TourneyKeeperEntities())
            {
                TKPlayer tmp = context.TKPlayer.SingleOrDefault(p => p.Token.Equals(playerToken));

                if (tmp != null)
                {
                    tmp.AppNotificationToken = notificationToken;
                    context.SaveChanges();
                }
            }
        }
    }
}
