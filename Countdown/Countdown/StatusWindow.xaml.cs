using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Countdown
{
    /// <summary>
    /// Interaction logic for StatusWindow.xaml
    /// </summary>
    public partial class StatusWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StatusWindow()
        {
            InitializeComponent();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Escape && WindowState == WindowState.Maximized && WindowStyle == WindowStyle.None)
            {
                Close();
            }

            base.OnKeyUp(e);
        }

        public void ShowData(DateTime roundEnd)
        {
            DataContext = this;

            new Thread(() =>
            {
                TimeSpan zeroSpan = new TimeSpan();
                while (timeLeft > zeroSpan)
                {
                    TimeLeft = roundEnd.Subtract(DateTime.Now);
                    Thread.Sleep(500);
                }
            }).Start();
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private TimeSpan timeLeft = TimeSpan.MaxValue;
        public TimeSpan TimeLeft
        {
            get
            {
                return timeLeft;
            }
            set
            {
                timeLeft = value;
                NotifyPropertyChanged();
            }
        }
    }
}
