using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EvolveQuest.Shared.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        protected void SetProperty<T>(
            ref T backingStore, T value,
            string propertyName,
            Action onChanged = null)
        {


            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;


            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }


        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
