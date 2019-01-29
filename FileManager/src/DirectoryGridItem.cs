using FileManager.Core.Annotations;
using FileManager.Properties;
using MaterialDesignThemes.Wpf;

namespace FileManager
{
    public class DirectoryGridItem : FileSystemGridItem
    {
        public DirectoryGridItem([NotNull] string directoryName) : base(directoryName)
        {
            this.Icon = new PackIcon { Kind = PackIconKind.Folder };
        }
    }
}
