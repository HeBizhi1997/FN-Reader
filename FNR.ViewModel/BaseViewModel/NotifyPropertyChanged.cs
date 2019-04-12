using System.ComponentModel;

namespace FNR.ViewModel
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///The PropertyChangedEventArgs parameter of the PropertyChanged event specifies the name of the property that changed. 
        ///This value is passed as a parameter to the OnPropertyChanged method.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
