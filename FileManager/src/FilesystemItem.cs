using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FileManager
{
    public abstract class FileSystemItem : GridItem
    {
        public string FullPath { get; }

        protected FileSystemItem(string name, string fullPath) : base(name)
        {
            this.FullPath = fullPath;
        }

        public unsafe static FileSystemItem Create(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo is FileInfo fileInfo)
            {
                const uint buffer_size = 50;
                fixed (char* buffer = new char[buffer_size])
                {
                    var result = Windows.Win32.PInvoke.StrFormatByteSize(fileInfo.Length, buffer, buffer_size);
                    return new FileItem(fileSystemInfo.Name, fileSystemInfo.FullName, new string(buffer));
                }

            }
            else
            {
                return new DirectoryItem(fileSystemInfo.Name, fileSystemInfo.FullName);
            }
        }

        public override bool IsEditable => true;

        private static bool IsDirectory(FileSystemInfo fsi)
        {
            return (fsi.Attributes & FileAttributes.Directory) != 0;
        }
    }
}
