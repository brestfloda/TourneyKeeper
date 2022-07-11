using Newtonsoft.Json;
using TourneyKeeper.DTO.App;
using System.Windows.Input;
using TourneyKeeper.Common;
using Xamarin.Forms;

namespace TourneyKeeper.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel() : base()
        {
            LoginCommand = new Command(Login);
        }

        public ICommand LoginCommand { protected set; get; }
        private void Login()
        {
            LoginMessage = "";
            var service = new TKREST();
            var response = service.Post<LoginDTO, LoginResponseDTO>(TKREST.Login, new LoginDTO { Login = Username, Password = Password });
            if (response != null)
            {
                App.PlayerToken = response.Token;
                var io = DependencyService.Get<ISaveAndLoad>();
                io.SaveText("player.txt", JsonConvert.SerializeObject(response));
                Firebase.SendRegistrationToServer(response.Token);
                Navigation.PushAsync(new Games(response.Token));
            }
            else
            {
                LoginMessage = "Login failed. Login/e-mail or password incorrect, please try again";
            }
        }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged();
                }
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }

        private string loginMessage;
        public string LoginMessage
        {
            get
            {
                return loginMessage;
            }
            set
            {
                if (loginMessage != value)
                {
                    loginMessage = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
