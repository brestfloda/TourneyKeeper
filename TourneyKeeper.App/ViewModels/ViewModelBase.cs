using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TourneyKeeper.Common;
using Xamarin.Forms;

namespace TourneyKeeper.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase() : base()
        {
            LogoutCommand = new Command(Logout);
        }

        public INavigation Navigation { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand LogoutCommand { protected set; get; }
        private void Logout()
        {
            var io = DependencyService.Get<ISaveAndLoad>();
            io.DeleteText("player.txt");
            Navigation.PushAsync(new Login());
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
    }
}
