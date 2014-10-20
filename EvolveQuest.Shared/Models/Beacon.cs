using EvolveQuest.Shared.Helpers;
using Newtonsoft.Json;

namespace EvolveQuest.Shared.Models
{
    public class Beacon : BaseModel
    {
        public int Id { get; set; }

        public int Minor { get; set; }

        [JsonIgnore]
        public bool Found
        {
            get
            {
                switch (Id)
                {
                    case 0:
                        return Settings.Beacon1Found;
                    case 1:
                        return Settings.Beacon2Found;
                    case 2:
                        return Settings.Beacon3Found;
                    default:
                        return false;
                }
            }
            set
            {
                switch (Id)
                {
                    case 0:
                        Settings.Beacon1Found = value;
                        break;
                    case 1:
                        Settings.Beacon2Found = value;
                        break;
                    case 2:
                        Settings.Beacon3Found = value;
                        break;
                }

                OnPropertyChanged("Found");
            }
        }
    }
}

