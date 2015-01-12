
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EvolveQuest.Shared.Helpers;
using EvolveQuest.Shared.Models;
using System.Windows.Input;
using EvolveQuest.Shared.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace EvolveQuest.Shared.ViewModels
{
    public class QuestViewModel : BaseViewModel
    {
        private IMessageDialog messages;
        private Game game;

        public QuestViewModel()
        {
            messages = ServiceContainer.Resolve<IMessageDialog>();
        }

        public string UUID
        {
            get { return game == null ? string.Empty : game.UUID; }
        }


        public const string NewQuestPropertyName = "NewQuest";
        public const string QuestsPropertyName = "Quests";

        public bool Beacon1Visible { get { return quest != null && quest.Beacons.Count > 0; } }

        public bool Beacon2Visible { get { return quest != null && quest.Beacons.Count > 1; } }

        public bool Beacon3Visible { get { return quest != null && quest.Beacons.Count > 2; } }

        private string extraTaskText = "Enter Code";
        public const string ExtraTaskTextPropertyName = "ExtraTaskText";

        /// <summary>
        /// gets or sets the extra task text to display
        /// </summary>
        public string ExtraTaskText
        {
            get { return extraTaskText; }
            set { SetProperty(ref extraTaskText, value, ExtraTaskTextPropertyName); }
        }

        private bool questComplete;
        public const string QuestCompletePropertyName = "QuestComplete";

        /// <summary>
        /// Gets or sets if the extra task is visible
        /// </summary>
        public bool QuestComplete
        {
            get { return questComplete; }
            set { SetProperty(ref questComplete, value, QuestCompletePropertyName); }
        }

    

        private bool extraTaskVisible;
        public const string ExtraTaskVisiblePropertyName = "ExtraTaskVisible";

        /// <summary>
        /// Gets or sets if the extra task is visible
        /// </summary>
        public bool ExtraTaskVisible
        {
            get { return extraTaskVisible; }
            set { SetProperty(ref extraTaskVisible, value, ExtraTaskVisiblePropertyName); }
        }


        private Quest quest = null;
        public const string QuestPropertyName = "Quest";

        /// <summary>
        /// Gets the current phase
        /// </summary>
        public Quest Quest
        {
            get { return quest; }
            private set { SetProperty(ref quest, value, QuestPropertyName); }
        }

        public const string CompletionDisplayPropertyName = "CompletionDisplay";

        #if WINDOWS_PHONE
    string questDisplay = "quest ";
    bool isWindowsPhone = true;

#else
        string questDisplay = "Quest ";
        bool isWindowsPhone = false;
        #endif
        /// <summary>
        /// Gets the current completion
        /// </summary>
        public string CompletionDisplay
        {
            get
            {
                if (this.GameComplete)
                    return isWindowsPhone ? "complete" : "Complete";

                if (game == null)
                    return string.Empty;

                return questDisplay + (Settings.CurrentQuest + 1) + " of " + game.Quests.Count;
            }
        }

        public string CompletionDisplayShort
        {
            get { return game == null ? string.Empty : (Settings.CurrentQuest + 1) + " of " + game.Quests.Count + " -"; }
        }

        private ICommand loadQuestCommand;

        /// <summary>
        /// Will load the next or current phase
        /// </summary>
        public ICommand LoadQuestCommand
        {
            get
            {
                return loadQuestCommand ??
                (loadQuestCommand = new RelayCommand(() => ExecuteLoadQuestCommand()));
            }
        }

        private async Task ExecuteLoadQuestCommand()
        {
            if (IsBusy)
                return;

            try
            {
        
                IsBusy = true;
                Settings.QuestDone = false;
                QuestComplete = false;
                if (game == null)
                {
                    await Task.Run(() =>
                        {
                            game = JsonConvert.DeserializeObject<Game>(FileCache.ReadGameData());
                        });
                }
                if (Settings.CurrentQuest >= game.Quests.Count)
                {
                    Settings.GameCompleted = true;
                    GameComplete = true;//trigger game complete
                    ExtraTaskVisible = true;
                    Quest = new Quest { Major = -1, Beacons = new List<Beacon>(), Clue = new Clue { Image = "http://blog.xamarin.com/wp-content/uploads/2014/01/evolve-2014.png", Message = "Congratulations, you have completed the Evolve 2014 Quest!" } };
                    OnPropertyChanged(CompletionDisplayPropertyName);
                    OnPropertyChanged("Beacon1Visible");
                    OnPropertyChanged("Beacon2Visible");
                    OnPropertyChanged("Beacon3Visible");
                    return;
                }


                Quest = game.Quests.FirstOrDefault(p => p.Id == Settings.CurrentQuest);
       
                CheckEndQuest();
                OnPropertyChanged(CompletionDisplayPropertyName);
                OnPropertyChanged("Beacon1Visible");
                OnPropertyChanged("Beacon2Visible");
                OnPropertyChanged("Beacon3Visible");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private ICommand extraTaskCommand;

        /// <summary>
        /// When extra task is prompted displays what to do for completion.
        /// </summary>
        public ICommand ExtraTaskCommand
        {
            get { return extraTaskCommand ?? (extraTaskCommand = new RelayCommand(ExecuteExtraTaskCommand)); }
        }

        private void ExecuteExtraTaskCommand()
        {
            if (CodeRequired)
            {
                messages.EnterTextMessage("Enter Code", "Find the code and enter it below!", CheckCode);
            }
            else if (QuestionRequired)
            {
                messages.AskQuestions("Question!", quest.Question, CheckAnswer);
            }
        }




        /// <summary>
        /// Checks the banana.
        /// </summary>
        /// <returns><c>true</c>, if banana was checked, <c>false</c> otherwise.</returns>
        /// <param name="major">Major number of the beacon.</param>
        /// <param name="minor">Minor number of the beacon.</param>
        public void CheckBeacon(int major, int minor)
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
                return;
            }

            bool found = true;
            try
            {
        
                if (quest == null)
                    return;

                found = false;

                if (major != Quest.Major)
                    return;

                foreach (Beacon beacon in Quest.Beacons)
                {
                    if (beacon.Found)
                    {
                        if (beacon.Minor == minor)
                            found = true;

                        continue;
                    }

                    if (beacon.Minor == minor)
                    {
                        beacon.Found = true;
                        OnPropertyChanged(QuestsPropertyName); //refresh bananas!

                        CheckEndQuest();
                        found = true;
                    }
                }
            }
            finally
            {
                if (!found)
                    messages.SendToast("So close! Try another beacon.");
            }

      

        }

        private void CheckEndQuest()
        {
            if (AllBeaconsFound)
            {
                ExtraTaskVisible = true;
                if (CodeRequired || QuestionRequired)
                {

                    QuestComplete = false;
                }
                else
                {
                    QuestComplete = true;
                }
            }
        }

        public void LoadNextLevel()
        {
      
            //move onto the next phase!
            Settings.CurrentQuest++;
            Settings.Beacon1Found = Settings.Beacon2Found = Settings.Beacon3Found = false;
            ExtraTaskVisible = false;
            OnPropertyChanged(NewQuestPropertyName);
            ExecuteLoadQuestCommand();
     
        }

        public void CheckBeacon(string scan)
        {
            var items = scan.Split(new[] { ',' });

            var major = 0;
            var minor = 0;
            int.TryParse(items[0], out major);
            int.TryParse(items[1], out minor);

            CheckBeacon(major, minor);
        }

        public void CheckCode(string code)
        {
            if (!String.Equals(code, quest.Code, StringComparison.CurrentCultureIgnoreCase))
            {
                messages.SendMessage("Wrong Secret Code", "Quick Hint: ask around, someone should have the correct one."); 
                return;
            }
            QuestComplete = true;
        }

        public void CheckAnswer(int answer)
        {
            if (!quest.Question.Answers[answer].IsAnswer)
            {
                messages.SendMessage("Wrong Answer", "Quick Hint: look for the good looking people in the Xamarin shirts. They just may know the answer.");  
                return;
            }

            QuestComplete = true;
        }

        public bool CodeRequired
        {
            get { return !string.IsNullOrWhiteSpace(quest.Code); }
        }

        public bool QuestionRequired
        {
            get { return quest.Question != null; }
        }

        private bool AllBeaconsFound
        {
            get
            {
                return Quest != null && Quest.Beacons.All(beacon => beacon.Found);
            }
        }

        private bool gameComplete;
        public const string GameCompletePropertyName = "GameComplete";

        public bool GameComplete
        {
            get { return gameComplete; }
            set { SetProperty(ref gameComplete, value, GameCompletePropertyName); }
        }
    }
}

