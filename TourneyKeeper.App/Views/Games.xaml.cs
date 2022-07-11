using System.ComponentModel;
using TourneyKeeper.ViewModels;
using Xamarin.Forms;

namespace TourneyKeeper
{
    public partial class Games : ContentPage, INotifyPropertyChanged
    {
        public Games(string token)
        {
            InitializeComponent();

            var vm = new GamesViewModel(token);
            vm.Navigation = Navigation;
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext != null)
            {
                ((GamesViewModel)BindingContext).Update();
            }

            if (GamesListView != null)
            {
                GamesListView.ClearValue(ListView.SelectedItemProperty);
            }
        }
    }
}
