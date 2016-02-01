using System.ComponentModel;
using System.IO;
using MaterialDesignThemes.Wpf;

namespace FileManager
{
    public struct FileSystemInfoView
    {
        // TODO: Implement icon property.
        //[DisplayName("")]
        //public PackIcon Icon { get; private set; }

        public string Name => _fileSystemInfoItem.Name;

        [Browsable(false)]
        internal string FullName => _fileSystemInfoItem.FullName;

        [Browsable(false)]
        internal bool IsDirectory => IsDirectoryMethod(_fileSystemInfoItem);

        private readonly FileSystemInfo _fileSystemInfoItem;

        public FileSystemInfoView(FileSystemInfo fileSystemInfo)
        {
            this._fileSystemInfoItem = fileSystemInfo;
            PackIcon packIcon;

            if (IsDirectoryMethod(fileSystemInfo))
            {
                packIcon = new PackIcon {Kind = PackIconKind.Folder};
            }
            else
            {
                packIcon = new PackIcon {Kind = PackIconKind.File};
            }

            // TODO: Implement icon.
        }

        private bool IsDirectoryMethod(FileSystemInfo fileSystemInfo)
        {
            return (fileSystemInfo.Attributes & FileAttributes.Directory) != 0;
        }
    }
}