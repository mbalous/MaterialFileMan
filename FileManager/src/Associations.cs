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
                { ".dll", new PackIcon() {Kind = PackIconKind.FileCog } },
                { ".jpg", new PackIcon() {Kind = PackIconKind.FileImage } },
                { ".jpeg", new PackIcon() {Kind = PackIconKind.FileImage } },
                { ".png", new PackIcon() {Kind = PackIconKind.FileImage } },
                { ".webp", new PackIcon() {Kind = PackIconKind.FileImage } },
                { ".bmp", new PackIcon() {Kind = PackIconKind.FileImage } },
                { ".doc", new PackIcon() {Kind = PackIconKind.FileWord } },
                { ".docx", new PackIcon() {Kind = PackIconKind.FileWord } },
                { ".pdf", new PackIcon() {Kind = PackIconKind.FilePdfBox } },
                { ".txt", new PackIcon() {Kind = PackIconKind.FileDocument } },
                { ".log", new PackIcon() {Kind = PackIconKind.FileDocument } },
                { ".rar", new PackIcon() {Kind = PackIconKind.ZipBox } },
                { ".7z", new PackIcon() {Kind = PackIconKind.ZipBox } },
                { ".zip", new PackIcon() {Kind = PackIconKind.ZipBox } },
                { ".mp4", new PackIcon() {Kind = PackIconKind.FileVideo } },
            };

            DefaultIcon = new PackIcon() { Kind = PackIconKind.File };
        }
    }
}
