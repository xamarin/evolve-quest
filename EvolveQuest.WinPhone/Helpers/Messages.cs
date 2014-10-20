using Coding4Fun.Toolkit.Controls;
using EvolveQuest.Shared.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace EvolveQuest.WinPhone.Helpers
{

    public class Messages : IMessageDialog
    {

        public static void ShowAbout()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var about = new AboutPrompt { Title = "Evolve Quest", VersionNumber = "v1.0" };
                    var xamarin = new AboutPromptItem
                    {
                        AuthorName = "Created In C# with Xamarin \nwith 60%+ Shared Code\n\nCopyright 2014 Xamarin",
                        EmailAddress = "hello@xamarin.com",
                        WebSiteUrl = "http://www.xamarin.com"
                    };

                    var privacy = new AboutPromptItem
                    {
                        AuthorName = "Privacy Policy",
                        WebSiteUrl = "http://www.xamarin.com/privacy"
                    };

                    var tech = new AboutPromptItem
                    {
                        AuthorName = "Technology Use\nZXigng.NET\nCross Platform QR Code Scanning\nCoding4Fun\nXam.PCL Settings\nJson.NET\nWindows Phone Toolkit"
                    };

        
                    about.Show(xamarin, privacy, tech);
                });
        }

        public void SendToast(string toast)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var toastPrompt = new ToastPrompt
                    {
                        Title = "Evolve Quest",
                        Message = toast,
                        ImageSource = new BitmapImage(new Uri("Assets/SmallLogo30.png", UriKind.RelativeOrAbsolute)),
                        MillisecondsUntilHidden = 3000
                    };
                    toastPrompt.Show();
                });
        }

        public void SendMessage(string title, string message)
        {
            SendMessage(title, message, null);
        }

        public void SendMessage(string title, string message, Action completed)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var result = MessageBox.Show(message, title, MessageBoxButton.OK);
                    if (completed != null)
                        completed();
                });
     
        }

        public void EnterTextMessage(string title, string message, Action<string> completed)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var input = new InputPrompt();
                    input.Completed += (sender, args) =>
                    {
                        if (completed != null)
                            completed(args.Result);
                    };
                    input.Title = title;
                    input.Message = message;
                    input.Show();
                });
        }

        public void AskQuestions(string title, Shared.Models.Question question, Action<int> completed)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var messagePrompt = new MessagePrompt
                    {
                        Title = title,
                        IsCancelVisible = true,
                        IsAppBarVisible = false
                    };

                    var listBox = new ListBox() { MaxWidth = 400, MaxHeight = 320 };
                    listBox.ItemsSource = question.Answers.Select(q => q.Text).ToArray();


                    listBox.ItemTemplate = Application.Current.Resources["AnswerItemTemplate"] as DataTemplate;
                    listBox.SelectionMode = SelectionMode.Single;

                    messagePrompt.Body = listBox;


                    messagePrompt.Completed += (sender, e) =>
                    {

                        if (e.PopUpResult != PopUpResult.Ok || listBox.SelectedIndex < 0)
                            return;

                        if (completed != null)
                            completed(listBox.SelectedIndex);
                    };

                    messagePrompt.Show();
                });
        }
    }
}
