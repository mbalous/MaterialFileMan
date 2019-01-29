using FileManager.Core.Annotations;
using MaterialDesignThemes.Wpf;

namespace FileManager
{
    public class FileGridItem : FileSystemGridItem
    {
        public FileGridItem([NotNull] string fileName) : base(fileName)
        {
            string extension = GetExtension(fileName);
            this.Icon = GetIconFromExtension(extension);
        }

        private string GetExtension(string fileName)
        {
            int lastDot = fileName.LastIndexOf('.');
            if (lastDot == -1)
            {
                return string.Empty;
            }

            return fileName.Substring(lastDot);
        }

        private PackIcon GetIconFromExtension(string extension)
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
}
