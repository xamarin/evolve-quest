using Refractored.Xam.Settings;

// Helpers/Settings.cs
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;
using System;
using System.Collections.Generic;

namespace EvolveQuest.Shared.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants


        private const string QuestDoneKey = "quest_done";
        private static readonly bool QuestDoneDefault = false;

        private const string Quest1Key = "quest1";
        private static readonly bool Quest1Default = false;


        private const string Quest2Key = "quest2";
        private static readonly bool Quest2Default = false;

        private const string Quest3Key = "quest3";
        private static readonly bool Quest3Default = false;

        private const string SecretBeaconFoundKey = "secret_beacon";
        private static readonly bool SecretBananaFoundDefault = false;

        private const string CurrentQuestKey = "current_quest_number";
        private static readonly int CurrenQuestDefault = 0;

        private const string GameCompleteKey = "game_complete";
        private static readonly bool GameCompleteDefault = false;

        #endregion

        public static int CurrentQuest
        {
            get
            {
                return AppSettings.GetValueOrDefault(CurrentQuestKey, CurrenQuestDefault);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue(CurrentQuestKey, value))
                    AppSettings.Save();
            }
        }

        public static bool QuestDone
        {
            get
            {
                return AppSettings.GetValueOrDefault(QuestDoneKey, QuestDoneDefault);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue(QuestDoneKey, value))
                    AppSettings.Save();

                if (value)
                {
                    Xamarin.Insights.Track("QuestCompleted", new Dictionary<string, string>
                        {
                            { "Quest", CurrentQuest.ToString() },
                            { "Date", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() }
                        });
                }
            }
        }

        public static bool GameCompleted
        {
            get
            {
                return AppSettings.GetValueOrDefault(GameCompleteKey, GameCompleteDefault);
            }
            set
            {

                if (!GameCompleted && value)
                {
                    Xamarin.Insights.Track("GameComplete", new Dictionary<string, string>
                        {
                            { "Date", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() }
                        });
                }

                if (AppSettings.AddOrUpdateValue(GameCompleteKey, value))
                    AppSettings.Save();

            }
        }

        public static bool SecretBeaconFound
        {
            get
            {
                return AppSettings.GetValueOrDefault(SecretBeaconFoundKey, SecretBananaFoundDefault);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue(SecretBeaconFoundKey, value))
                    AppSettings.Save();
            }
        }


        public static bool Beacon1Found
        {
            get
            {
                return AppSettings.GetValueOrDefault(Quest1Key, Quest1Default);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue(Quest1Key, value))
                    AppSettings.Save();
            }
        }

        public static bool Beacon2Found
        {
            get
            {
                return AppSettings.GetValueOrDefault(Quest2Key, Quest2Default);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue(Quest2Key, value))
                    AppSettings.Save();
            }
        }

        public static bool Beacon3Found
        {
            get
            {
                return AppSettings.GetValueOrDefault(Quest3Key, Quest3Default);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue(Quest3Key, value))
                    AppSettings.Save();
            }
        }

    }
}