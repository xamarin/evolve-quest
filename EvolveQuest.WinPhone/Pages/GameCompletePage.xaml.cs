using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EvolveQuest.Shared.ViewModels;
using Microsoft.Phone.Tasks;

namespace EvolveQuest.WinPhone.Pages
{
    public partial class GameCompletePage : PhoneApplicationPage
    {
        readonly GameCompleteViewModel viewModel;
        readonly ZXing.Mobile.MobileBarcodeScanner scanner;

        public GameCompletePage()
        {
            InitializeComponent();
            viewModel = this.DataContext as GameCompleteViewModel;
            viewModel.PropertyChanged += HandlePropertyChanged;
     
            Loaded += GameCompletePage_Loaded;
            scanner = new ZXing.Mobile.MobileBarcodeScanner(Deployment.Current.Dispatcher);
        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == BaseViewModel.IsBusyPropertyName)
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = !viewModel.IsBusy;
            }
        }

        void GameCompletePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (viewModel.Prize == null)
                viewModel.LoadGameCommand.Execute(null);
        }

        private void ButtonShare_Click(object sender, RoutedEventArgs e)
        {
            var task = new ShareStatusTask
            {
                Status = "I just completed the #XamarinEvolve Quest and scored an awesome prize!"
            };

            task.Show();
        }

        private async void AppBarScan_Click(object sender, EventArgs e)
        {
      
            var result = await scanner.Scan();

            if (result != null)
            {
                Console.WriteLine("Scanned Barcode: " + result.Text);
                Deployment.Current.Dispatcher.BeginInvoke(() => viewModel.CheckBanana(result.Text));
            }
        }

        private void AppBarAbout_Click(object sender, EventArgs e)
        {
            Helpers.Messages.ShowAbout();
        }
    }
}