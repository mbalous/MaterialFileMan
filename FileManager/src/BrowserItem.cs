using MaterialDesignThemes.Wpf;

namespace FileManager
{
    public abstract class BrowserItem
    {
        public PackIcon Icon { get; protected set; }

        public virtual string Text { get; protected set; }
    }
}