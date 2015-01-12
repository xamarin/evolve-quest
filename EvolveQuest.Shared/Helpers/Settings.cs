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


        const string QuestDoneKey = "quest_done";
        static readonly bool QuestDoneDefault = false;

        const string Quest1Key = "quest1";
        static readonly bool Quest1Default = false;


        const string Quest2Key = "quest2";
        static readonly bool Quest2Default = false;

        const string Quest3Key = "quest3";
        static readonly bool Quest3Default = false;

        const string SecretBeaconFoundKey = "secret_beacon";
        static readonly bool SecretBananaFoundDefault = false;

        const string CurrentQuestKey = "current_quest_number";
        static readonly int CurrenQuestDefault = 0;

        const string GameCompleteKey = "game_complete";
        static readonly bool GameCompleteDefault = false;

        #endregion

        public static int CurrentQuest
        {
            get
            {
                return AppSettings.GetValueOrDefault(CurrentQuestKey, CurrenQuestDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CurrentQuestKey, value);
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
                AppSettings.AddOrUpdateValue(QuestDoneKey, value);

                if (value)
                {
                    #if !__UNIFIED__
                    Xamarin.Insights.Track("QuestCompleted", new Dictionary<string, string>
                        {
                            { "Quest", CurrentQuest.ToString() },
                            { "Date", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() }
                        });
                    #endif
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
                    #if !__UNIFIED__
                    Xamarin.Insights.Track("GameComplete", new Dictionary<string, string>
                        {
                            { "Date", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() }
                        });
                    #endif
                }

                AppSettings.AddOrUpdateValue(GameCompleteKey, value);

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
                AppSettings.AddOrUpdateValue(SecretBeaconFoundKey, value);
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
                AppSettings.AddOrUpdateValue(Quest1Key, value);
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
                AppSettings.AddOrUpdateValue(Quest2Key, value);
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
                AppSettings.AddOrUpdateValue(Quest3Key, value);
            }
        }

    }
}