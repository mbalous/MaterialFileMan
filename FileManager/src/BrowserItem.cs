using MaterialDesignThemes.Wpf;

namespace FileManager
{
    public abstract class GridItem : BindableBase
    {
        private string _text;

        public PackIcon Icon { get; protected set; }

        public virtual string Text
        {
            get => _text;
            set => SetProperty(value, ref _text);
        }

        public abstract bool IsEditable { get; }

        protected GridItem()
        {
        }

        protected GridItem(string text) : this()
        {
            this.Text = text;
        }
    }
}