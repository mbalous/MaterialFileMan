using System.ComponentModel;
using System.IO;
using System.Windows.Markup;
using System.Windows.Navigation;
using MaterialDesignThemes.Wpf;

namespace FileManager
{
    public class FileSystemInfoView
    {
        // TODO: Implement icon property.
        public PackIcon Icon { get; private set; }

        public string Name => _fileSystemInfoItem.Name;

        [Browsable(false)]
        internal string FullName => _fileSystemInfoItem.FullName;

        [Browsable(false)]
        internal bool IsDirectory => IsDirectoryMethod(_fileSystemInfoItem);

        private readonly FileSystemInfo _fileSystemInfoItem;

        public FileSystemInfoView(FileSystemInfo fileSystemInfo)
        {
            this._fileSystemInfoItem = fileSystemInfo;
            this.Icon = null;

            if (IsDirectoryMethod(fileSystemInfo))
            {
                this.Icon = new PackIcon {Kind = PackIconKind.Folder};
            }
            else
            {
                this.Icon = new PackIcon {Kind = PackIconKind.File};
            }
        }

        private bool IsDirectoryMethod(FileSystemInfo fileSystemInfo)
        {
            bool result = (fileSystemInfo.Attributes & FileAttributes.Directory) != 0;
            return result;
        }
    }
}