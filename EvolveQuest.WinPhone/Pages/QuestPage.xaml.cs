using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EvolveQuest.Shared.ViewModels;
using EvolveQuest.Shared.Helpers;
using System.Collections.Generic;

namespace EvolveQuest.WinPhone.Pages
{
    public partial class QuestPage : PhoneApplicationPage
    {
        public static QuestViewModel ViewModel{ get; set; }

        readonly ZXing.Mobile.MobileBarcodeScanner scanner;
        ZXing.Mobile.MobileBarcodeScanningOptions options = new ZXing.Mobile.MobileBarcodeScanningOptions();

        public QuestPage()
        {
            InitializeComponent();
            ViewModel = this.DataContext as QuestViewModel;

            ViewModel.PropertyChanged += HandlePropertyChanged;

            this.Loaded += PhasePage_Loaded;
            scanner = new ZXing.Mobile.MobileBarcodeScanner(Deployment.Current.Dispatcher);
            options.PossibleFormats = new List<ZXing.BarcodeFormat>()
            { 
                ZXing.BarcodeFormat.QR_CODE
            };
        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == QuestViewModel.GameCompletePropertyName)
            {
                //if(ViewModel.GameComplete)
                //  NavigateToGameComplete();
            }
            else if (e.PropertyName == BaseViewModel.IsBusyPropertyName)
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = !ViewModel.IsBusy;
            }
        }

        private void NavigateToGameComplete()
        {
            NavigationService.Navigate(new Uri("/Pages/GameCompletePage.xaml", UriKind.Relative));
        }

        void PhasePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Quest == null)
                ViewModel.LoadQuestCommand.Execute(null);
            else if (ViewModel.QuestComplete && Settings.QuestDone)
                ViewModel.LoadNextLevel();
        }

        private void AppBarScan_Click(object sender, EventArgs e)
        {
            scanner.Scan(options).ContinueWith((result) =>
                {
                    if (result.Result == null)
                        return;

                    Console.WriteLine("Scanned Barcode: " + result.Result.Text);
                    Deployment.Current.Dispatcher.BeginInvoke(() => ViewModel.CheckBeacon(result.Result.Text));
                });
        }

        private void AppBarAbout_Click(object sender, EventArgs e)
        {
            Helpers.Messages.ShowAbout();
        }

        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.GameComplete)
            {
                NavigateToGameComplete();
                return;
            }

            if (ViewModel.CodeRequired)
                NavigationService.Navigate(new Uri("/Pages/QuestCodePage.xaml", UriKind.Relative));
            else if (ViewModel.QuestionRequired)
                NavigationService.Navigate(new Uri("/Pages/QuestQuestionPage.xaml", UriKind.Relative));
            else if (ViewModel.QuestComplete)
                NavigationService.Navigate(new Uri("/Pages/QuestCompletionPage.xaml", UriKind.Relative));
        }
    }
}