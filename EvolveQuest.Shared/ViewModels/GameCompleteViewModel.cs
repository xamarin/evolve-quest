using EvolveQuest.Shared.Helpers;
using EvolveQuest.Shared.Interfaces;
using EvolveQuest.Shared.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvolveQuest.Shared.ViewModels
{
    public class GameCompleteViewModel: BaseViewModel
    {
      
        private IMessageDialog messages;
        private Game game;

        public GameCompleteViewModel()
        {
            messages = ServiceContainer.Resolve<IMessageDialog>();
        }

        public string UUID
        {
            get { return game.UUID; }
        }

        private Prize prize = null;
        public const string PrizePropertyName = "Prize";

        /// <summary>
        /// Gets the current phase
        /// </summary>
        public Prize Prize
        {
            get { return prize; }
            private set { SetProperty(ref prize, value, PrizePropertyName); }
        }

        private ICommand loadGameCommand;

        /// <summary>
        /// Will load the next or current phase
        /// </summary>
        public ICommand LoadGameCommand
        {
            get
            {
                return loadGameCommand ??
                (loadGameCommand = new RelayCommand(() => ExecuteLoadGameCommand()));
            }
        }

        private async Task ExecuteLoadGameCommand()
        {
            if (IsBusy)
                return;

            try
            {
        
                IsBusy = true;

        
                if (game == null)
                {
                    await Task.Run(() =>
                        {
                            game = JsonConvert.DeserializeObject<Game>(FileCache.ReadGameData());
                        });
                }

                Prize = game.Prize;
                OnPropertyChanged("Prize");
            }
            finally
            {
                IsBusy = false;
            }
        }

   



        /// <summary>
        /// Checks the banana.
        /// </summary>
        /// <returns><c>true</c>, if banana was checked, <c>false</c> otherwise.</returns>
        /// <param name="major">Major number of the beacon.</param>
        /// <param name="minor">Minor number of the beacon.</param>
        public void CheckBanana(int major, int minor)
        {
            if (major == 9999 && minor == 1000)
            {
                //Spy monkey found! pop up funny dialog here!
                return;
            }

            if (major == 9999 && major == 9999)
            {
                if (Settings.SecretBeaconFound)
                    return;

                messages.SendMessage("Congratulations!", "You found the top secret hidden beacon and have unlocked a special prize at the end of your adventure!");
                Settings.SecretBeaconFound = true;
                OnPropertyChanged(SecretBananaFoundPropertyName);
                return;
            }

            messages.SendToast("So close! Try another beacon.");
       
        }

        public const string SecretBananaFoundPropertyName = "SecretBananaFound";

        public bool SecretBananaFound
        {
            get { return Settings.SecretBeaconFound; }
        }


        public void CheckBanana(string scan)
        {
            var items = scan.Split(new[] { ',' });

            var major = 0;
            var minor = 0;
            int.TryParse(items[0], out major);
            int.TryParse(items[1], out minor);

            CheckBanana(major, minor);
        }
    }
}
