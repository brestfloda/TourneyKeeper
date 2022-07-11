using TourneyKeeper.DTO.App;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TourneyKeeper.Common;
using Xamarin.Forms;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;

namespace TourneyKeeper.ViewModels
{
    public class GamesViewModel : ViewModelBase
    {
        private static string token = null;
        public ICommand UpdateCommand { protected set; get; }

        public GamesViewModel(string token) : base()
        {
            GamesViewModel.token = token;
            UpdateCommand = new Command<bool>(Update);
            MessagingCenter.Subscribe<OpponentPickerViewModel>(this, Events.OpponentSelected, (sender) => Update(true));
        }

        public async void Update(bool manual = false)
        {
            try
            {
                IsBusy = true;

                var service = new TKREST();
                var getCurrentGamesResponse = service.Post<GamesDTO, GamesResponseDTO>(TKREST.GetCurrentGames, new GamesDTO { Token = token });

                var newGames = getCurrentGamesResponse != null ?
                    new ObservableCollection<GameListViewModel>(getCurrentGamesResponse.Games.OrderByDescending(g => g.GameId).Select(g => new GameListViewModel
                    {
                        Game = g,
                        Table = $"Table: {g.Table}",
                        Score = $"{g.MyScore}-{g.OpponentScore}",
                        Opponent = $"{g.Opponent}",
                        Round = $"{g.Round}",
                    })) :
                    new ObservableCollection<GameListViewModel>();

                var getOpponentsResponse = service.Post<GetOpponentsDTO, GetOpponentsResponseDTO>(TKREST.GetOpponents, new GetOpponentsDTO { PlayerToken = token });

                Games = newGames;
                GamesVisible = !(newGames.Count > 0);
                IsBusy = false;

                if (((getOpponentsResponse?.GameId.HasValue) ?? false) && (getOpponentsResponse.CurrentOpponent == null || getOpponentsResponse.CurrentOpponent.Id < 0) && !OpponentPickerViewModel.IsOpen)
                {
                    await Navigation.PushModalAsync(new OpponentPicker(token));
                }
                else if (!manual)
                {
                    await Task.Delay(60 * 1000);
                    Update();
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string> { { "Message", e.Message }, { "StackTrace", e.StackTrace } });
                IsBusy = false;
                await Task.Delay(60 * 1000);
                Update(manual);
            }
        }

        public bool gamesVisible;
        public bool GamesVisible
        {
            get
            {
                return gamesVisible;
            }
            set
            {
                if (gamesVisible != value)
                {
                    gamesVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public GameListViewModel selectedGame;
        public GameListViewModel SelectedGame
        {
            get
            {
                return selectedGame;
            }
            set
            {
                selectedGame = value;
                if (value != null)
                {
                    Navigation.PushAsync(new Game(value.Game));
                }
            }
        }

        public ObservableCollection<GameListViewModel> games;
        public ObservableCollection<GameListViewModel> Games
        {
            get
            {
                return games;
            }
            set
            {
                if (games != value)
                {
                    games = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
