﻿using MaterialDesignThemes.Wpf;
using System.IO;

namespace FileManager
{
    public class FileGridItem : FileSystemGridItem
    {
        public FileGridItem(string fileName, string fullPath) : base(fileName, fullPath)
        {
            string extension = Path.GetExtension(fileName);
            this.Icon = GetIconFromExtension(extension);
        }

        private PackIcon GetIconFromExtension(string extension)
        {
            if (PackIconAssociations.Associations.TryGetValue(extension, out PackIcon icon))
                return icon;
            else
                return PackIconAssociations.DefaultIcon;
        }
    }
}
