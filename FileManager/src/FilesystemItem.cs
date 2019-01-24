using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.IO;

namespace FileManager
{
    /// <summary>
    /// Represents a file system item.
    /// Can be either file or directory.
    /// Execute method <see cref="IsDirectory"/> to find out.
    /// </summary>
    public sealed class FilesystemItem : BrowserItem
    {
        public override string Text => this._fileSystemInfoItem.Name;

        [Browsable(false)]
        public string FullName => this._fileSystemInfoItem.FullName;

        [Browsable(false)]
        public bool IsDirectory => IsDirectoryInternal(this._fileSystemInfoItem);

        private readonly FileSystemInfo _fileSystemInfoItem;

        public FilesystemItem(FileSystemInfo fileSystemInfo)
        {
            this._fileSystemInfoItem = fileSystemInfo;
            Icon = null;

            if (IsDirectoryInternal(fileSystemInfo))
            {
                Icon = new PackIcon { Kind = PackIconKind.Folder };
            }
            else
            {
                string extension = GetExtension(fileSystemInfo);
                this.Icon = GetIconFromExtension(extension);
            }

            PackIcon GetIconFromExtension(string extension)
            {
                if (PackIconAssociations.Associations.TryGetValue(extension, out PackIcon icon))
                {
                    return icon;
                }
                else
                {
                    return PackIconAssociations.DefaultIcon;
                }
            }
        }

        private string GetExtension(FileSystemInfo fsi)
        {
            int lastDot = fsi.FullName.LastIndexOf('.');
            if (lastDot == -1)
            {
                return string.Empty;
            }

            return fsi.FullName.Substring(lastDot);

        }

        private bool IsDirectoryInternal(FileSystemInfo fsi)
        {
            bool result = (fsi.Attributes & FileAttributes.Directory) != 0;
            return result;
        }
    }
}
