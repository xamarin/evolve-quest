using System;
using UIKit;
using EvolveQuest.Shared.ViewModels;
using EvolveQuest.Shared.Helpers;
using System.Drawing;
using EvolveQuest.Shared.Interfaces;

namespace EvolveQuest.iOS
{
    partial class WelcomeViewController : UIViewController
    {
        public WelcomeViewController(IntPtr handle)
            : base(handle)
        {
        }


        private WelcomeViewModel viewModel;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBar.BarTintColor = EvolveQuest.Shared.Helpers.Color.Blue.ToUIColor();
            NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
            NavigationController.NavigationBarHidden = true;
            //Custom setup for button stylce
            ButtonLoadGame.Layer.CornerRadius = ButtonAbout.Layer.CornerRadius = 5;

            viewModel = new WelcomeViewModel();
            viewModel.PropertyChanged += HandlePropertyChanged;
        }


        private void GoToGame()
        {
            var storyboard = UIStoryboard.FromName("MainStoryboard", null);
            var vc = storyboard.InstantiateViewController("QuestViewController2") as UIViewController;

            NavigationController.PushViewController(vc, true);
        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case BaseViewModel.IsBusyPropertyName:
                    {
                        ButtonLoadGame.Enabled = ButtonAbout.Enabled = !viewModel.IsBusy;
                        if (viewModel.IsBusy)
                            ProgressBar.StartAnimating();
                        else
                            ProgressBar.StopAnimating();
                    }
                    break;
                case WelcomeViewModel.GameLoadedPropertyName:
                    {
                        if (viewModel.GameLoaded)
                        {
                            if (viewModel.Game.Started)
                                GoToGame();
                            else
                                ServiceContainer.Resolve<IMessageDialog>().SendMessage("Evolve Quest", "Your quest begins on October 6th, 2014. Check back soon.");
                        }
                    }
                    break;
            }
        }



        partial void ButtonLoadGame_TouchUpInside(UIButton sender)
        {

            if (viewModel.GameLoaded && viewModel.Game != null && viewModel.Game.Started)
            {
                GoToGame();
                return;
            }

            viewModel.LoadGameCommand.Execute(null);
        }

        partial void ButtonAbout_TouchUpInside(UIButton sender)
        {
            NavigationController.PushViewController(new AboutViewController(), true);
        }

        private void MoveMap()
        {
            if (Settings.CurrentQuest == 0)
                return;

            var quests = 11.0;
            if (viewModel.Game != null)
                quests = (double)viewModel.Game.Quests.Count - 1;
            var percent = ((double)Settings.CurrentQuest / quests);

            var time = 1.0;
            if (percent > .5)
                time = 1.5;

            UIView.Animate(time, 0, UIViewAnimationOptions.CurveEaseIn,
                () =>
                {
                    MapImage.Frame = GetTargetPositionFromPercent(percent);
                },
                () =>
                {
                });
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            NavigationController.NavigationBarHidden = true;

            MoveMap();
        }


			
        private CoreGraphics.CGRect GetTargetPositionFromPercent(double percentageComplete)
        {
            if (percentageComplete >= 1.0)
            {
                return new CoreGraphics.CGRect(0, (float)-MapImage.Image.Size.Height + View.Frame.Size.Height, MapImage.Frame.Size.Width, View.Frame.Size.Height);
            }
            var height = View.Frame.Size.Height * UIScreen.MainScreen.Scale;
            var position = height * percentageComplete * -1;
            position -= (height * .12);//for top stuff

            if (position * -1 > MapImage.Image.Size.Height - View.Frame.Size.Height)
                position = -MapImage.Image.Size.Height + View.Frame.Size.Height;

            return new CoreGraphics.CGRect(0, (float)position, MapImage.Frame.Size.Width, View.Frame.Size.Height);
        }

    }
}
