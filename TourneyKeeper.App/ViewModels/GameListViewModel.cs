using TourneyKeeper.DTO.App;

namespace TourneyKeeper.ViewModels
{
    public class GameListViewModel : ViewModelBase
    {
        public GameDTO Game { get; set; }

        public GameListViewModel() : base()
        {
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

        private string score;
        public string Score
        {
            get
            {
                return score;
            }
            set
            {
                if (score != value)
                {
                    score = value;
                    OnPropertyChanged();
                }
            }
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

        private string round;
        public string Round
        {
            get
            {
                return round;
            }
            set
            {
                if (round != value)
                {
                    round = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
