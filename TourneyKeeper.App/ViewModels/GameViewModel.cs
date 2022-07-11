using Newtonsoft.Json;
using TourneyKeeper.DTO.App;
using System.Threading.Tasks;
using System.Windows.Input;
using TourneyKeeper.Common;
using Xamarin.Forms;
using System;

namespace TourneyKeeper.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private GameDTO dto = null;

        public GameViewModel(GameDTO dto) : base()
        {
            Status = "";
            ShowStatus = false;

            UpdateCommand = new Command(Update);

            Opponent = dto.Opponent;
            Table = dto.Table?.ToString();
            MyScore = dto.MyScore.ToString();
            MySecondaryScore = dto.MySecondaryScore.ToString();
            OpponentScore = dto.OpponentScore.ToString();
            OpponentSecondaryScore = dto.OpponentSecondaryScore.ToString();
            UseSecondaryPoints = dto.UseSecondaryPoints;

            this.dto = dto;
        }

        public ICommand UpdateCommand { protected set; get; }
        private async void Update()
        {
            Status = "";
            ShowStatus = false;

            var io = DependencyService.Get<ISaveAndLoad>();
            var tmp = io.LoadText("player.txt");
            var playerInfo = JsonConvert.DeserializeObject<LoginResponseDTO>(tmp);

            var updateDto = new UpdateGameDTO
            {
                GameId = dto.GameId,
                MyScore = GetScore(MyScore),
                MySecondaryScore = GetScore(MySecondaryScore),
                OpponentScore = GetScore(OpponentScore),
                OpponentSecondaryScore = GetScore(OpponentSecondaryScore),
                Token = playerInfo.Token
            };

            var service = new TKREST();
            var response = service.Post<UpdateGameDTO, UpdateGameResponseDTO>(TKREST.UpdateGame, updateDto);

            Status = response.Status ? "Game updated" : response.Message;
            ShowStatus = true;

            if (response.Status)
            {
                await Task.Delay(1500);
                try
                {
                    await Navigation.PopAsync();
                }
                catch
                {
                    //spis den, han har nok trykket på back
                }
            }
        }

        private int GetScore(string myScore)
        {
            if (string.IsNullOrEmpty(myScore))
            {
                return 0;
            }
            if (int.TryParse(myScore, out int result))
            {
                return result;
            }
            return 0;
        }

        private string opponent;
        public string Opponent
        {
            get
            {
                return opponent;
            }
            set
            {
                if (opponent != value)
                {
                    opponent = value;
                    OnPropertyChanged();
                }
            }
        }

        private string table;
        public string Table
        {
            get
            {
                return table;
            }
            set
            {
                if (table != value)
                {
                    table = value;
                    OnPropertyChanged();
                }
            }
        }

        private string myScore;
        public string MyScore
        {
            get
            {
                return myScore;
            }
            set
            {
                if (myScore != value)
                {
                    myScore = value;
                    OnPropertyChanged();
                }
            }
        }

        private string mySecondaryScore;
        public string MySecondaryScore
        {
            get
            {
                return mySecondaryScore;
            }
            set
            {
                if (mySecondaryScore != value)
                {
                    mySecondaryScore = value;
                    OnPropertyChanged();
                }
            }
        }

        private string opponentScore;
        public string OpponentScore
        {
            get
            {
                return opponentScore;
            }
            set
            {
                if (opponentScore != value)
                {
                    opponentScore = value;
                    OnPropertyChanged();
                }
            }
        }

        private string opponentSecondaryScore;
        public string OpponentSecondaryScore
        {
            get
            {
                return opponentSecondaryScore;
            }
            set
            {
                if (opponentSecondaryScore != value)
                {
                    opponentSecondaryScore = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool useSecondaryPoints;
        public bool UseSecondaryPoints
        {
            get
            {
                return useSecondaryPoints;
            }
            set
            {
                if (useSecondaryPoints != value)
                {
                    useSecondaryPoints = value;
                    OnPropertyChanged();
                }
            }
        }

        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool showStatus;
        public bool ShowStatus
        {
            get
            {
                return showStatus;
            }
            set
            {
                if (showStatus != value)
                {
                    showStatus = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
