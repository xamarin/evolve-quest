using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EvolveQuest.Shared.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        private bool isBusy;
        /// <summary>
        /// Gets or sets if the view is busy.
        /// </summary>
        public const string IsBusyPropertyName = "IsBusy";

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                NotBusy = !value;
                SetProperty(ref isBusy, value, IsBusyPropertyName);
            }
        }

        private bool notBusy = true;
        /// <summary>
        /// Gets or sets if the view is busy.
        /// </summary>
        public const string NotBusyPropertyName = "NotBusy";

        public bool NotBusy
        {
            get { return notBusy; }
            set { SetProperty(ref notBusy, value, NotBusyPropertyName); }
        }

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
