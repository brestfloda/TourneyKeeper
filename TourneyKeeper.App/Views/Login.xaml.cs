using System.ComponentModel;
using TourneyKeeper.ViewModels;
using Xamarin.Forms;

namespace TourneyKeeper
{
    public partial class Login : ContentPage, INotifyPropertyChanged
    {
        public Login()
        {
            InitializeComponent();
            var vm = new LoginViewModel();
            vm.Navigation = Navigation;
            BindingContext = vm;
        }
    }
}
