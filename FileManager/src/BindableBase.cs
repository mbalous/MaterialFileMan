using FileManager.Core.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileManager
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        protected BindableBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets a property to a new value.
        /// If the new value doesn't equal the current one, raises <see cref="PropertyChanged"/>.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="value">New value.</param>
        /// <param name="property">Reference to property.</param>
        /// <param name="propertyName">
        /// Property name. If unused, <see cref="CallerMemberNameAttribute"/> will be used to obtain property name.
        /// </param>
        protected virtual void SetProperty<T>(T value, ref T property, [CallerMemberName] string propertyName = null)
        {
            if (!value.Equals(property))
            {
                property = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}