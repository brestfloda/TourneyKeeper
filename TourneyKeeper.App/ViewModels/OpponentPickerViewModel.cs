using TourneyKeeper.DTO.App;
using System;
using System.Linq;
using System.Windows.Input;
using TourneyKeeper.Common;
using Xamarin.Forms;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;

namespace TourneyKeeper.ViewModels
{
    public class OpponentPickerViewModel : ViewModelBase
    {
        private static string token = null;
        public static bool IsOpen { get; set; } = false;
        public ICommand UpdateCommand { protected set; get; }
        public ICommand SelectOpponentCommand { protected set; get; }
        public GetOpponentsResponseDTO getOpponentsResponse = null;

        public OpponentPickerViewModel(string token) : base()
        {
            IsOpen = true;
            OpponentPickerViewModel.token = token;
            UpdateCommand = new Command(Update);
            SelectOpponentCommand = new Command(SelectOpponent);
        }

        public void Update()
        {
            try
            {
                IsBusy = true;

                var service = new TKREST();
                getOpponentsResponse = service.Post<GetOpponentsDTO, GetOpponentsResponseDTO>(TKREST.GetOpponents, new GetOpponentsDTO { PlayerToken = token });

                if ((getOpponentsResponse?.GameId.HasValue) ?? false)
                {
                    Opponents = getOpponentsResponse.Opponents.ToList();
                    if (SelectedOpponent?.Id != Opponents.SingleOrDefault(o => o.Id == getOpponentsResponse.CurrentOpponent?.Id)?.Id)
                    {
                        SelectedOpponent = Opponents.SingleOrDefault(o => o.Id == getOpponentsResponse.CurrentOpponent?.Id);
                    }
                    Tables = Enumerable.Range(1, getOpponentsResponse.Tables).ToList();
                    if (SelectedTable != getOpponentsResponse.CurrentTable)
                    {
                        SelectedTable = getOpponentsResponse.CurrentTable;
                    }
                    SelectedTable = getOpponentsResponse.CurrentTable;
                }

                IsBusy = false;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string> { { "Message", e.Message }, { "StackTrace", e.StackTrace } });
                IsBusy = false;
            }
        }

        public async void SelectOpponent()
        {
            try
            {
                IsBusy = true;

                var service = new TKREST();

                var response = service.Post<SelectOpponentDTO, ResponseDTO>(TKREST.SelectOpponent, new SelectOpponentDTO
                {
                    GameId = getOpponentsResponse.GameId.Value,
                    OpponentId = SelectedOpponent.Id,
                    PlayerTeam1 = getOpponentsResponse.PlayerTeam1,
                    Table = SelectedTable ?? 0,
                    TournamentPlayerId = getOpponentsResponse.TournamentPlayerId
                });

                MessagingCenter.Send(this, Events.OpponentSelected);

                await Navigation.PopModalAsync();
            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string> { { "Message", e.Message }, { "StackTrace", e.StackTrace } });
                IsBusy = false;
            }
            finally
            {
                IsOpen = false;
            }
        }

        public IList<TournamentPlayerDTO> opponents;
        public IList<TournamentPlayerDTO> Opponents
        {
            get
            {
                return opponents;
            }
            set
            {
                if (opponents != value)
                {
                    opponents = value;
                    OnPropertyChanged();
                }
            }
        }

        public IList<int> tables;
        public IList<int> Tables
        {
            get
            {
                return tables;
            }
            set
            {
                if (tables != value)
                {
                    tables = value;
                    OnPropertyChanged();
                }
            }
        }

        public TournamentPlayerDTO selectedOpponent;
        public TournamentPlayerDTO SelectedOpponent
        {
            get
            {
                return selectedOpponent;
            }
            set
            {
                if (selectedOpponent != value && value != null)
                {
                    selectedOpponent = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? selectedTable;
        public int? SelectedTable
        {
            get
            {
                return selectedTable;
            }
            set
            {
                if (selectedTable != value && value != null)
                {
                    selectedTable = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
