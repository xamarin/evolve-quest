using EvolveQuest.Shared.Helpers;
using EvolveQuest.Shared.Interfaces;
using EvolveQuest.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvolveQuest.Shared.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
      

        private Random random;
        IMessageDialog messages;

        public WelcomeViewModel()
        {
            random = new Random();
            messages = ServiceContainer.Resolve<IMessageDialog>();
            var game = FileCache.ReadGameData();
            GameLoaded = !string.IsNullOrWhiteSpace(game);
            if (GameLoaded)
                Game = JsonConvert.DeserializeObject<Game>(game);
        }

			

        private bool gameLoaded = false;
        public const string GameLoadedPropertyName = "GameLoaded";

        public bool GameLoaded
        {
            get { return gameLoaded; }
            set
            {
                gameLoaded = value;
                OnPropertyChanged(GameLoadedPropertyName);
            }
        }

        public Game Game { get; set; }

        private ICommand loadGameCommand;

        /// <summary>
        /// Will load the next or current phase
        /// </summary>
        public ICommand LoadGameCommand
        {
            get
            {
                return loadGameCommand ??
                (loadGameCommand = new RelayCommand(() => ExecuteLoadGameCommand(), () => NotBusy));
            }
        }

        private const string GameUrl = "http://evolvequest.azurewebsites.net/game{0}.json";

        #if TESTER
      public async Task ExecuteLoadGameCommand(int game = 0)

#else
        public async Task ExecuteLoadGameCommand()
#endif
      {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
#if TESTER
          var url = string.Format(GameUrl, game);
#else
                var url = string.Format(GameUrl, random.Next(0, 3));
                //var url = string.Format(GameUrl, 0);
#endif
                #if __IOS__
				var client = new HttpClient(new ModernHttpClient.NativeMessageHandler());
                #else
                var client = new HttpClient();
                #endif
                client.Timeout = new TimeSpan(0, 0, 15);
                var result = await client.GetStringAsync(url);
                Game = JsonConvert.DeserializeObject<Game>(result);

                await FileCache.SaveGameDataAsync(result);
                GameLoaded = true;
                Xamarin.Insights.Track("GameStarted");
            }
            catch (Exception ex)
            {
                Xamarin.Insights.Report(ex);
                messages.SendMessage("Not all who wander are lost...", "But you might be. Looks like you were unable to load Evolve Quest because you dropped the connection. Please check if you have reception and try again.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
