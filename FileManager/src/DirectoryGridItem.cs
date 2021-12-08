using MaterialDesignThemes.Wpf;

namespace FileManager
{
    public class DirectoryGridItem : FileSystemGridItem
    {
        public DirectoryGridItem(string directoryName, string fullPath) : base(directoryName, fullPath)
        {
            this.Icon = new PackIcon { Kind = PackIconKind.Folder };
        }
    }
}
