
using System.Collections.Generic;


namespace EvolveQuest.Shared.Models
{
    public class Game
    {
        public string UUID { get; set; }

        public List<Quest> Quests { get; set; }

        public Prize Prize { get; set; }

        public bool Started { get; set; }
    }
}
