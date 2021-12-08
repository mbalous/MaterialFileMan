using MaterialDesignThemes.Wpf;

namespace FileManager
{
    public class DirectoryItem : FileSystemItem
    {
        public DirectoryItem(string directoryName, string fullPath) : base(directoryName, fullPath)
        {
            this.Icon = new PackIcon { Kind = PackIconKind.Folder };
        }
    }
}
