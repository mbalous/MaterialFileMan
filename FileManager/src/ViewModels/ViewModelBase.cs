using FileManager.Properties;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileManager.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetAndRaise<T>(T value, ref T property, [CallerMemberName] string propertyName = null)
        {
            if (!value.Equals(property))
            {
                property = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}