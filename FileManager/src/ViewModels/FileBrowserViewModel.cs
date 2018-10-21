using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FileManager
{
    public class FileBrowserViewModel : ViewModelBase
    {
        public string CurrentPath
        {
            get { return this._currentDirectoryInfo.FullName; }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Directory can not be null.", new ArgumentNullException());
                }

                this._currentDirectoryInfo = new DirectoryInfo(value);
                this.TextboxPath = this._currentDirectoryInfo.FullName;
                List<FileSystemInfo> fileSystemInfos = new List<FileSystemInfo>();
                DirectoryInfo directoryInfo = new DirectoryInfo(this._currentDirectoryInfo.FullName);
                fileSystemInfos.AddRange(directoryInfo.GetDirectories());
                fileSystemInfos.AddRange(directoryInfo.GetFiles());

                LinkedList<BrowserItem> systemInfoViews = new LinkedList<BrowserItem>();
                if (CanGoUp)
                {
                    systemInfoViews.AddFirst(new GoUpItem());
                }
                foreach (FileSystemInfo systemInfoView in fileSystemInfos)
                {
                    systemInfoViews.AddLast(new FilesystemItem(systemInfoView));
                }

                this.Items = systemInfoViews;
                OnPropertyChanged();
            }
        }

        private DirectoryInfo _currentDirectoryInfo;


        public FileBrowserViewModel()
        {
            this.CurrentPath = Directory.GetCurrentDirectory();
            this.GoUpButtonClick = ActionCommand.Create((o) =>
            {
                if (CanGoUp) { GoUp(); }
            });
            this.DataGridDoubleClick = ActionCommand.Create((o) =>
            {
                if (this.SelectedItem is GoUpItem)
                {
                    if (CanGoUp)
                    {
                        GoUp();
                        return;
                    }
                }
                else if (this.SelectedItem is FilesystemItem fsItem)
                {
                    if (fsItem.IsDirectory)
                    {
                        this.CurrentPath = fsItem.FullName;
                    }
                    else
                    {
                        Process.Start(fsItem.FullName);
                    }
                }
                else
                {
                    throw new ApplicationException();
                }                
            });
        }

        public bool CanGoUp => this._currentDirectoryInfo.Parent != null;

        public void GoUp()
        {
            if (!CanGoUp) { throw new InvalidOperationException(); }

            this.CurrentPath = this._currentDirectoryInfo.Parent.FullName;
        }

        #region Bindings 

        public ActionCommand GoUpButtonClick { get; private set; }

        public ActionCommand DataGridDoubleClick { get; private set; }

        private LinkedList<BrowserItem> _items;
        public LinkedList<BrowserItem> Items
        {
            get => _items; private set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        private dynamic _selectedItem;
        public dynamic SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private string _textboxPath;
        public string TextboxPath
        {
            get => _textboxPath; set
            {
                _textboxPath = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}