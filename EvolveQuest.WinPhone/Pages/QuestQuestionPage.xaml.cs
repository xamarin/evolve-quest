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
using EvolveQuest.Shared.Interfaces;

namespace EvolveQuest.WinPhone.Pages
{
    public partial class QuestQuestionPage : PhoneApplicationPage
    {
        public QuestQuestionPage()
        {
            InitializeComponent();

            this.DataContext = QuestPage.ViewModel;
            this.Loaded += QuestQuestionPage_Loaded;
        }

        bool initialized;
        IMessageDialog messages;

        void QuestQuestionPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (initialized)
                return;

            initialized = true;
            messages = ServiceContainer.Resolve<IMessageDialog>();
            Hint.Text = QuestPage.ViewModel.Quest.Question.Text;
            CurrentQuest.Text = QuestPage.ViewModel.CompletionDisplayShort;
        }

        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            if (QuestPage.ViewModel.QuestComplete)
            {
                NavigationService.GoBack();
                return;
            }

            messages.AskQuestions("Question:", QuestPage.ViewModel.Quest.Question, (answer) =>
                {
                    QuestPage.ViewModel.CheckAnswer(answer);
                    if (QuestPage.ViewModel.QuestComplete)
                    {
                        ButtonCancel.Visibility = System.Windows.Visibility.Collapsed;
                        ButtonContinue.Content = "continue";
                        Hint.Text = "Are you ready to continue with the next quest, thrill-seeker?";
                        Awesome.Text = "That's it!";
                        Congrats.Text = "You answered the question correct, noble one.";
                        Settings.QuestDone = true;
                    }
                });
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}