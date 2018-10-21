﻿using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.IO;

namespace FileManager
{
    public sealed class FilesystemItem : BrowserItem
    {
        public override string Text => this._fileSystemInfoItem.Name;

        [Browsable(false)]
        public string FullName => this._fileSystemInfoItem.FullName;

        [Browsable(false)]
        public bool IsDirectory => IsDirectoryMethod(this._fileSystemInfoItem);

        private readonly FileSystemInfo _fileSystemInfoItem;

        public FilesystemItem(FileSystemInfo fileSystemInfo)
        {
            this._fileSystemInfoItem = fileSystemInfo;
            Icon = null;

            if (IsDirectoryMethod(fileSystemInfo))
            {
                Icon = new PackIcon { Kind = PackIconKind.Folder };
            }
            else
            {
                Icon = new PackIcon { Kind = PackIconKind.File };
            }
        }

        private bool IsDirectoryMethod(FileSystemInfo fileSystemInfo)
        {
            bool result = (fileSystemInfo.Attributes & FileAttributes.Directory) != 0;
            return result;
        }
    }
}