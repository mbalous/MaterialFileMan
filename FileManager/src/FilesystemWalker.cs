using FileManager.Properties;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace FileManager
{
    internal class FileSystemWalker : INotifyPropertyChanged, IDisposable
    {
        #region Public

        public event EventHandler<string> CurrentPathChanged;

        public string CurrentPath => this._currentDirectoryInfo.FullName;

        public void SetCurrentPath([NotNull] string path)
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

        public void TraverseUpwards()
        {
            if (!CanTraverseUpwards)
            {
                throw new InvalidOperationException("Can't traverse upwards. Current directory has no parent");
            }

            this.SetCurrentPath(this._currentDirectoryInfo.Parent.FullName);
        }

        public FileSystemInfo[] ChildItems => _currentDirectoryInfo.GetFileSystemInfos();

        public FileSystemWalker() : this(Directory.GetCurrentDirectory())
        {
        }

        public FileSystemWalker(string path)
        {
            _watcher = new FileSystemWatcher()
            {
                IncludeSubdirectories = false
            };
            _watcher.Created += _watcher_Created;
            _watcher.Deleted += _watcher_Deleted;
            _watcher.Renamed += _watcher_Renamed;
            SetCurrentPath(path);
        }

        #region IPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            this._watcher?.Dispose();
        }

        #endregion

        #endregion

        private DirectoryInfo _currentDirectoryInfo;

        private FileSystemWatcher _watcher;

        private void _watcher_Created(object sender, FileSystemEventArgs e)
        {
            OnPropertyChanged(nameof(ChildItems));
            System.Diagnostics.Debug.WriteLine("File created...");
        }

        private void _watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            OnPropertyChanged(nameof(ChildItems));
            System.Diagnostics.Debug.WriteLine("File deleted...");
        }

        private void _watcher_Renamed(object sender, RenamedEventArgs e)
        {
            OnPropertyChanged(nameof(ChildItems));
            System.Diagnostics.Debug.WriteLine("File renamed...");
        }
    }
}
