using FileManager.Core.Annotations;
using FileManager.Properties;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileManager
{
    public abstract class GridItem : INotifyPropertyChanged
    {
        private string _text;

        public PackIcon Icon { get; protected set; }

        public virtual string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        protected GridItem()
        {

        }

        protected GridItem(string text)
        {
            this.Text = text;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}