using System.IO;

namespace FileManager
{
    public abstract class FileSystemItem : GridItem
    {
        public string FullPath { get; }

        protected FileSystemItem(string name, string fullPath) : base(name)
        {
            this.FullPath = fullPath;
        }

        public static FileSystemItem Create(FileSystemInfo fileSystemInfo)
        {
            if (IsDirectory(fileSystemInfo))
            {
                return new DirectoryItem(fileSystemInfo.Name, fileSystemInfo.FullName);
            }
            else
            {
                return new FileItem(fileSystemInfo.Name, fileSystemInfo.FullName);
            }
        }

        private static bool IsDirectory(FileSystemInfo fsi)
        {
            return (fsi.Attributes & FileAttributes.Directory) != 0;
        }
    }
}
