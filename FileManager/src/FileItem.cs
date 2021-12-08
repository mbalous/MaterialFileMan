using MaterialDesignThemes.Wpf;
using System.IO;

namespace FileManager
{
    public class FileItem : FileSystemItem
    {
        public FileItem(string fileName, string fullPath, string humanReadableSize) : base(fileName, fullPath)
        {
            string extension = Path.GetExtension(fileName);
            this.Icon = GetIconFromExtension(extension);
            this.HumanReadableSize = humanReadableSize;
        }

        public string HumanReadableSize { get; }

        private PackIcon GetIconFromExtension(string extension)
        {
            if (PackIconAssociations.Associations.TryGetValue(extension, out PackIcon icon))
                return icon;
            else
                return PackIconAssociations.DefaultIcon;
        }
    }
}
