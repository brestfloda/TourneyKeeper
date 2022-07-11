using TourneyKeeper.DTO.App;
using System;
using System.ComponentModel;
using TourneyKeeper.ViewModels;
using Xamarin.Forms;

namespace TourneyKeeper
{
    public partial class Game : ContentPage, INotifyPropertyChanged
    {
        private static DateTime LastLogin = DateTime.MinValue;

        public Game(GameDTO game)
        {
            InitializeComponent();

            var vm = new GameViewModel(game);
            vm.Navigation = Navigation;
            BindingContext = vm;
        }
    }
}
