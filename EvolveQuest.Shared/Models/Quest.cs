
using System.Collections.Generic;

namespace EvolveQuest.Shared.Models
{
    public class Quest
    {

        public int Id { get; set; }

        public List<Beacon> Beacons { get; set; }

        public int Major { get; set; }

        public Clue Clue { get; set; }

        public string Code { get; set; }

        public string CodeHint { get; set; }

        public Question Question { get; set; }
    }
}

