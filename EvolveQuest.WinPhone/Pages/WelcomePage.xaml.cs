using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using EvolveQuest.Shared.ViewModels;
using EvolveQuest.Shared.Helpers;
using EvolveQuest.Shared.Interfaces;

namespace EvolveQuest.WinPhone.Pages
{
    public partial class WelcomePage : PhoneApplicationPage
    {
        WelcomeViewModel viewModel;
        // Constructor
        public WelcomePage()
        {
            InitializeComponent();

            viewModel = this.DataContext as WelcomeViewModel;

            viewModel.PropertyChanged += HandlePropertyChanged;

            this.Loaded += WelcomePage_Loaded;
        }

        void WelcomePage_Loaded(object sender, RoutedEventArgs e)
        {
            var quests = 11.0;
            if (viewModel.Game != null)
                quests = (double)viewModel.Game.Quests.Count - 1;
            var percent = ((double)Settings.CurrentQuest / quests);
            MainScroll.ScrollToVerticalOffset(MainScroll.ScrollableHeight * percent);

        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == WelcomeViewModel.GameLoadedPropertyName)
            {
                if (viewModel.GameLoaded)
                {
                    if (viewModel.Game.Started)
                        NavigateToPhase();
                    else
                        ServiceContainer.Resolve<IMessageDialog>().SendMessage("Evolve Quest", "Your quest begins on October 6th, 2014. Check back soon.");
                }
            }
        }

        private void NavigateToPhase()
        {
            NavigationService.Navigate(new Uri("/Pages/QuestPage.xaml", UriKind.Relative));
        }

        private void AppBarAbout_Click(object sender, EventArgs e)
        {
            Helpers.Messages.ShowAbout();
        }

    }
}