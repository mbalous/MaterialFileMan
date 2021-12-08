using System.IO;

namespace FileManager
{
    public abstract class FileSystemGridItem : GridItem
    {
        public string FullPath { get; }

        protected FileSystemGridItem(string name, string fullPath) : base(name)
        {
            this.FullPath = fullPath;
        }

        public static FileSystemGridItem Create(FileSystemInfo fileSystemInfo)
        {
            if (IsDirectory(fileSystemInfo))
            {
                return new DirectoryGridItem(fileSystemInfo.Name, fileSystemInfo.FullName);
            }
            else
            {
                return new FileGridItem(fileSystemInfo.Name, fileSystemInfo.FullName);
            }
        }

        private static bool IsDirectory(FileSystemInfo fsi)
        {
            return (fsi.Attributes & FileAttributes.Directory) != 0;
        }
    }
}
