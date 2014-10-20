using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EvolveQuest.Shared.Helpers;

namespace EvolveQuest.WinPhone.Pages
{
    public partial class QuestCompletionPage : PhoneApplicationPage
    {
        public QuestCompletionPage()
        {
            InitializeComponent();
            this.Loaded += QuestCompletionPage_Loaded;
        }

        void QuestCompletionPage_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentQuest.Text = QuestPage.ViewModel.CompletionDisplayShort;
            Settings.QuestDone = true;
        }

        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}