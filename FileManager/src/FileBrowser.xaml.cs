using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace FileManager
{
    /// <summary>
    /// Interaction logic for FileBrowser.xaml
    /// </summary>
    public partial class FileBrowser : UserControl
    {
        private DirectoryInfo _currentDirectoryInfo;

        public string CurrentPath
        {
            get { return this._currentDirectoryInfo.FullName; }
            private set
            {
                this._currentDirectoryInfo = new DirectoryInfo(value);
                this.TextBoxPath.Text = this._currentDirectoryInfo.FullName;
                List<FileSystemInfo> fileSystemInfos = new List<FileSystemInfo>();
                DirectoryInfo directoryInfo = new DirectoryInfo(this._currentDirectoryInfo.FullName);
                fileSystemInfos.AddRange(directoryInfo.GetDirectories());
                fileSystemInfos.AddRange(directoryInfo.GetFiles());

                LinkedList<BrowserItem> systemInfoViews = new LinkedList<BrowserItem>();
                if (CanGoUp())
                {
                    systemInfoViews.AddFirst(new GoUpItem());
                }
                foreach (FileSystemInfo systemInfoView in fileSystemInfos)
                {
                    systemInfoViews.AddLast(new FileSystemInfoItem(systemInfoView));
                }

                this.ListBoxItems.ItemsSource = systemInfoViews;
            }
        }

        public double ElementSizing
        {
            get { return (double)GetValue(ElementSizingProperty); }
            set
            {
                SetValue(ElementSizingProperty, value);
                _dataContext.ElementSizing = value;
            }
        }

        // Using a DependencyProperty as the backing store for ElementSizing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementSizingProperty =
            DependencyProperty.Register("ElementSizing", typeof(double), typeof(FileBrowser), new PropertyMetadata(12d));

        private FileBrowserViewModel _dataContext;

        /// <summary>
        /// Create new FileBrowser. 
        /// Current directory is going to be used upon start.
        /// </summary>
        public FileBrowser() : this(Directory.GetCurrentDirectory())
        {
        }

        public FileBrowser(string currentPath)
        {
            InitializeComponent();
            _dataContext = new FileBrowserViewModel();
            this.CurrentPath = currentPath;
        }

        private void ButtonGoUp_Click(object sender, RoutedEventArgs e)
        {
            if (CanGoUp())
            {
                GoUp();
            }
        }

        public bool CanGoUp()
        {
            return this._currentDirectoryInfo.Parent != null;
        }

        public void GoUp()
        {
            if (!CanGoUp())
            {
                throw new InvalidOperationException();
            }

            this.CurrentPath = this._currentDirectoryInfo.Parent.FullName;
        }

        private void ListBoxItems_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.ListBoxItems.SelectedItem == null)
            {
                return;
            }
            if (this.ListBoxItems.SelectedItem is GoUpItem)
            {
                if (CanGoUp())
                {
                    GoUp();
                }
                return;
            }

            FileSystemInfoItem selectedItem = (FileSystemInfoItem)this.ListBoxItems.SelectedItem;
            if (selectedItem.IsDirectory)
            {
                this.CurrentPath = selectedItem.FullName;
            }
            else
            {
                Process.Start(selectedItem.FullName);
            }
        }

        private void TextBoxPath_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (Directory.Exists(this.TextBoxPath.Text))
                    {
                        this.CurrentPath = this.TextBoxPath.Text;
                    }
                    break;
                case Key.Back:
                    if (CanGoUp())
                    {
                        GoUp();
                    }
                    break;
            }
        }
    }
}