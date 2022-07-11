using RestSharp;
using System;
using System.Collections.Generic;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Common.Managers
{
    public class NotificationManager
    {
        private static void NotifyFirebase(string appNotificationToken, string title, string body)
        {
#if !DEBUG
            var client = new RestClient("https://fcm.googleapis.com/");
            var notifyRequest = new FirebaseNotification
            {
                to = appNotificationToken,
                notification = new FirebaseNotificatonData
                {
                    body = body,
                    title = title
                }
            };

            var request = new RestRequest("fcm/send", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"key={"AAAAR22Whu8:APA91bEKSXgHMXLBFPNP2Blgqa_DMSsqoVhfmVJJrY_n6E4Dm50rIcRJNHWq7Les3IDzSMsWs9t1NTXAWiSkzjzRoqnXfIEcG1-2M2LJkP963v8TILGVnVYxu_VmoJ1Y67InoTHWOOf9"}");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(notifyRequest);
            client.Execute(request);
#endif
        }

        internal static void NotifyNewGame(IEnumerable<TKGame> games)
        {
            try
            {
                foreach (var game in games)
                {
                    if (game.TKTournamentPlayer.TKPlayer.AppNotificationToken != null)
                    {
                        NotifyFirebase(game.TKTournamentPlayer.TKPlayer.AppNotificationToken,
                            $"New game at {game.TKTournament.Name}",
                            $"Opponent: {game.TKTournamentPlayer1.PlayerName} Table: {game.TableNumber}");
                    }
                    if (game.TKTournamentPlayer1.TKPlayer.AppNotificationToken != null)
                    {
                        NotifyFirebase(game.TKTournamentPlayer1.TKPlayer.AppNotificationToken,
                            $"New game at {game.TKTournament.Name}",
                            $"Opponent: {game.TKTournamentPlayer.PlayerName} Table: {game.TableNumber}");
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
            }
        }

        internal static void NotifyNewTeamMatch(string tournamentName, string opponent, string table, string appNotificationToken)
        {
            try
            {
                NotifyFirebase(appNotificationToken,
                    $"New match at {tournamentName}",
                    $"Opponent: {opponent} Table: {table}");
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
            }
        }

        internal static void NotifyTeamMatchSwap(string tournamentName, string opponent, string table, string appNotificationToken)
        {
            try
            {
                NotifyFirebase(appNotificationToken,
                    $"Teams swapped at {tournamentName}",
                    $"New opponent: {opponent} Table: {table}");
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
            }
        }

        internal static void NotifySwap(IEnumerable<TKGame> games)
        {
            try
            {
                foreach (var game in games)
                {
                    if (game.TKTournamentPlayer.TKPlayer.AppNotificationToken != null)
                    {
                        NotifyFirebase(game.TKTournamentPlayer.TKPlayer.AppNotificationToken,
                            $"Players swapped at {game.TKTournament.Name}",
                            $"New opponent: {game.TKTournamentPlayer1.PlayerName} Table: {game.TableNumber}");
                    }
                    if (game.TKTournamentPlayer1.TKPlayer.AppNotificationToken != null)
                    {
                        NotifyFirebase(game.TKTournamentPlayer1.TKPlayer.AppNotificationToken,
                            $"Players swapped at {game.TKTournament.Name}",
                            $"New opponent: {game.TKTournamentPlayer.PlayerName} Table: {game.TableNumber}");
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
            }
        }
    }

    internal class FirebaseNotification
    {
        public string to { get; set; }
        public FirebaseNotificatonData notification { get; set; }
    }

    internal class FirebaseNotificatonData
    {
        internal FirebaseNotificatonData() { sound = "Enabled"; }
        public string body { get; set; }
        public string title { get; set; }
        public string sound { get; set; }
    }
}
