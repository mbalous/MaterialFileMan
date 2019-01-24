using MaterialDesignThemes.Wpf;
using System.Collections.Generic;

namespace FileManager
{
    static class PackIconAssociations
    {
        public static IReadOnlyDictionary<string, PackIcon> Associations;
        public static PackIcon DefaultIcon;

        static PackIconAssociations()
        {
            Associations = new Dictionary<string, PackIcon>()
            {
                { ".exe", new PackIcon() {Kind = PackIconKind.Application } },
                { ".dll", new PackIcon() {Kind = PackIconKind.SettingsOutline } },
                { ".jpg", new PackIcon() {Kind = PackIconKind.Image } },
                { ".jpeg", new PackIcon() {Kind = PackIconKind.Image } },
                { ".png", new PackIcon() {Kind = PackIconKind.Image } },
                { ".doc", new PackIcon() {Kind = PackIconKind.FileWord } },
                { ".docx", new PackIcon() {Kind = PackIconKind.FileWord } },
                { ".pdf", new PackIcon() {Kind = PackIconKind.FilePdf } },
                { ".txt", new PackIcon() {Kind = PackIconKind.FileDocumentBox } },
                { ".rar", new PackIcon() {Kind = PackIconKind.ZipBox } },
                { ".7z", new PackIcon() {Kind = PackIconKind.ZipBox } },
                { ".zip", new PackIcon() {Kind = PackIconKind.ZipBox } },
            };

            DefaultIcon = new PackIcon() { Kind = PackIconKind.File };
        }
    }
}
