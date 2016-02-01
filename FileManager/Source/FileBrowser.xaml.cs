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
            get { return _currentDirectoryInfo.FullName; }
            private set
            {
                _currentDirectoryInfo = new DirectoryInfo(value);
                TextBoxPath.Text = _currentDirectoryInfo.FullName;
                List<FileSystemInfo> fileSystemInfos = new List<FileSystemInfo>(20);
                DirectoryInfo directoryInfo = new DirectoryInfo(_currentDirectoryInfo.FullName);
                fileSystemInfos.AddRange(directoryInfo.GetDirectories());
                fileSystemInfos.AddRange(directoryInfo.GetFiles());

                LinkedList<FileSystemInfoView> systemInfoViews = new LinkedList<FileSystemInfoView>();
                foreach (FileSystemInfo systemInfoView in fileSystemInfos)
                {
                    systemInfoViews.AddLast(new FileSystemInfoView(systemInfoView));
                }

                this.ListBoxItems.ItemsSource = systemInfoViews;
            }
        }

        /// <summary>
        /// Create new FileBrowser. 
        /// Current directory is going to be used upon start.
        /// </summary>
        public FileBrowser() : this(Directory.GetCurrentDirectory())
        {
            InitializeComponent();
        }

        public FileBrowser(string currentPath)
        {
            InitializeComponent();
            this.CurrentPath = currentPath;
        }

        private void ButtonGoUp_Click(object sender, RoutedEventArgs e)
        {
            if (CanGoUp())
                GoUp();
        }

        public bool CanGoUp()
        {
            return _currentDirectoryInfo.Parent != null;
        }

        public void GoUp()
        {
            if (!CanGoUp())
                throw new InvalidOperationException();

            CurrentPath = _currentDirectoryInfo.Parent.FullName;
        }

        private void ChangeTextSize_OnClick(object sender, RoutedEventArgs e)
        {
            if (((MenuItem) sender).Equals(MenuItemContextDecreaseTextSize))
            {
                if (this.FontSize <= 3 || this.TextColumnFileName.FontSize <= 3  )
                    return;
                this.FontSize -= 3;
                this.TextColumnFileName.FontSize -= 3;
            }
            else if (((MenuItem) sender).Equals(MenuItemContextIncreaseTextSize))
            {
                this.FontSize += 3;
                this.TextColumnFileName.FontSize += 3;
            }
        }

        private void ListBoxItems_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxItems.SelectedItem == null)
                return;
            FileSystemInfoView selectedItem = (FileSystemInfoView) ListBoxItems.SelectedItem;

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
            if (e.Key == Key.Enter)
            {
                if (Directory.Exists(TextBoxPath.Text))
                {
                    this.CurrentPath = TextBoxPath.Text;
                }
            }
        }
    }
}