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
    public partial class QuestCodePage : PhoneApplicationPage
    {
        public QuestCodePage()
        {
            InitializeComponent();
            this.DataContext = QuestPage.ViewModel;
            this.Loaded += QuestCodePage_Loaded;
        }

        void QuestCodePage_Loaded(object sender, RoutedEventArgs e)
        {

            CurrentQuest.Text = QuestPage.ViewModel.CompletionDisplayShort;

            if (!string.IsNullOrWhiteSpace(QuestPage.ViewModel.Quest.CodeHint))
                Hint.Text = QuestPage.ViewModel.Quest.CodeHint;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            if (QuestPage.ViewModel.QuestComplete)
            {
                NavigationService.GoBack();
                return;
            }

            QuestPage.ViewModel.ExtraTaskText = Code.Text.Trim();
            QuestPage.ViewModel.CheckCode(QuestPage.ViewModel.ExtraTaskText);
            if (QuestPage.ViewModel.QuestComplete)
            {
                ButtonCancel.Visibility = Code.Visibility = System.Windows.Visibility.Collapsed;
                ButtonContinue.Content = "continue";
                Hint.Text = "Are you ready to continue with the next quest, thrill-seeker?";
                Awesome.Text = "That's it!";
                Congrats.Text = "You entered the correct code.";
                Settings.QuestDone = true;
            }
        }
    }
}