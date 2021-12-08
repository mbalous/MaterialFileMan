using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FileManager.Core
{
    public class DirectoryController : IDisposable
    {
        #region Public

        public event EventHandler<string> CurrentPathChanged;

        public string CurrentPath => this._currentDirectoryInfo.FullName;

        /// <summary>
        /// Sets current directory.
        /// </summary>
        /// <param name="path">
        /// Path that points to directory.
        /// If passsed a file path, parent directory will be used.
        /// </param>
        /// <exception cref="ArgumentNullException" />
        public void SetCurrentDirectory([NotNull] string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException();
            }

            this._currentDirectoryInfo = new DirectoryInfo(path);
            this._watcher.Path = this._currentDirectoryInfo.FullName;
            this.CurrentPathChanged?.Invoke(this, this._currentDirectoryInfo.FullName);
        }

        public bool CanTraverseUpwards => this._currentDirectoryInfo.Parent != null;

        /// <summary>
        /// Traverses the <see cref="DirectoryController"/> instance into parent directory.
        /// Check the property <see cref="CanTraverseUpwards"/> if the traversal is possible.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public void TraverseUpwards()
        {
            if (!CanTraverseUpwards)
            {
                throw new InvalidOperationException("Can't traverse upwards. Current directory has no parent");
            }

            this.SetCurrentDirectory(this._currentDirectoryInfo.Parent.FullName);
        }

        /// <summary>
        /// Enters a child directory.
        /// </summary>
        /// <param name="directory">Directory to enter. Full name is not necessary.</param>
        public void EnterChildDirectory([NotNull] string directory)
        {
            string newDirectory = Path.Combine(this._currentDirectoryInfo.FullName, directory);
            if (!Directory.Exists(newDirectory))
            {
                throw new InvalidOperationException("Directory does not exist", new DirectoryNotFoundException());
            }

            this.SetCurrentDirectory(newDirectory);
        }

        public void RenameChildItem([NotNull] string oldPath, [NotNull] string newName)
        {
            bool isFile;
            if (!(isFile = File.Exists(oldPath)) && !Directory.Exists(oldPath))
                throw new ArgumentException($"\"{oldPath}\" does not exist.", nameof(oldPath));

            string fullTargetPath = Path.Combine(this._currentDirectoryInfo.FullName, newName);

            if (oldPath == fullTargetPath)
                return;

            if (isFile)
                File.Move(oldPath, fullTargetPath);
            else
                Directory.Move(oldPath, fullTargetPath);

            Debug.WriteLine($"Renamed \"{oldPath}\" to \"{fullTargetPath}\".");
        }

        public void DeleteChildItem([NotNull] string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                Debug.WriteLine($"Deleted file \"{fullPath}\".");
            }

            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                Debug.WriteLine($"Deleted directory \"{fullPath}\".");
            }
        }

        public event EventHandler<FileSystemEventArgs> ChildItemsChanged;

        public FileSystemInfo[] GetChildItems()
        {
            return _currentDirectoryInfo.GetFileSystemInfos("*", this._enumerationOptions);
        }

        /// <summary>
        /// Initializes new instance with <see cref="Directory.GetCurrentDirectory"/>.
        /// </summary>
        public DirectoryController() : this(Directory.GetCurrentDirectory())
        {
        }

        public DirectoryController(string path, bool useFileWatcher = false)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Path does not point to valid directory.", nameof(path));
            }

            this._watcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = false,
                EnableRaisingEvents = true,
            };

            _watcher.Created += Watcher_Created;
            _watcher.Deleted += Watcher_Deleted;
            _watcher.Renamed += Watcher_Renamed;
            SetCurrentDirectory(path);
        }

        public void Dispose()
        {
            this._watcher?.Dispose();
        }

        #endregion

        private readonly EnumerationOptions _enumerationOptions = new EnumerationOptions() { ReturnSpecialDirectories = false };

        private DirectoryInfo _currentDirectoryInfo;

        private FileSystemWatcher _watcher;

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            this.ChildItemsChanged?.Invoke(sender, e);
            System.Diagnostics.Debug.WriteLine("File created...");
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            this.ChildItemsChanged?.Invoke(sender, e);
            System.Diagnostics.Debug.WriteLine("File deleted...");
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            this.ChildItemsChanged?.Invoke(sender, e);
            System.Diagnostics.Debug.WriteLine("File renamed...");
        }
    }
}
