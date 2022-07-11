using System.ComponentModel;
using TourneyKeeper.ViewModels;
using Xamarin.Forms;

namespace TourneyKeeper
{
    public partial class OpponentPicker : ContentPage, INotifyPropertyChanged
    {
        public OpponentPicker(string token)
        {
            InitializeComponent();

            var vm = new OpponentPickerViewModel(token);
            vm.Navigation = Navigation;
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext != null)
            {
                ((OpponentPickerViewModel)BindingContext).Update();
            }
        }
    }
}
