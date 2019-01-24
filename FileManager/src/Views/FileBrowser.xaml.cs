using FileManager.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileManager.Views
{
    /// <summary>
    /// Interaction logic for FileBrowser.xaml
    /// </summary>
    public partial class FileBrowser : UserControl
    {
        private readonly FileSystemWalker _fileSystemWalker;

        public double ElementSizing
        {
            get { return (double)GetValue(ElementSizingProperty); }
            set { SetValue(ElementSizingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElementSizing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementSizingProperty =
            DependencyProperty.Register("ElementSizing", typeof(double), typeof(FileBrowser), new PropertyMetadata(12d));

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
            this._fileSystemWalker = new FileSystemWalker(currentPath);
            this._fileSystemWalker.CurrentPathChanged += _fileSystemWalker_CurrentPathChanged;
            GetAndSetItems();
        }

        private void _fileSystemWalker_CurrentPathChanged(object sender, string e)
        {
            GetAndSetItems();
        }

        private void GetAndSetItems()
        {
            List<BrowserItem> browserItems = new List<BrowserItem>();
            if (_fileSystemWalker.CanTraverseUpwards)
            {
                browserItems.Add(new GoUpItem());
            }

            FileSystemInfo[] childItems = _fileSystemWalker.ChildItems;
            browserItems.AddRange(childItems.Select(x => new FilesystemItem(x)));

            this.ListBoxItems.ItemsSource = browserItems;
            this.TextBoxPath.Text = this._fileSystemWalker.CurrentPath;
        }

        private void ButtonGoUp_Click(object sender, RoutedEventArgs e)
        {
            if (this._fileSystemWalker.CanTraverseUpwards)
            {
                this._fileSystemWalker.TraverseUpwards();
            }
        }

        private void ListBoxItems_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.ListBoxItems.SelectedItem == null)
            {
                return;
            }
            if (this.ListBoxItems.SelectedItem is GoUpItem)
            {
                if (this._fileSystemWalker.CanTraverseUpwards)
                {
                    this._fileSystemWalker.TraverseUpwards();
                }
                return;
            }

            FilesystemItem selectedItem = (FilesystemItem)this.ListBoxItems.SelectedItem;
            if (selectedItem.IsDirectory)
            {
                this._fileSystemWalker.SetCurrentPath(selectedItem.FullName);
            }
            else
            {
                ProcessStartInfo pci = new ProcessStartInfo(selectedItem.FullName)
                {
                    UseShellExecute = true
                };
                try
                {
                    using (Process.Start(pci)) { }
                }
                catch (Win32Exception ex)
                {
                    DialogErrorView view = new Views.DialogErrorView()
                    {
                        DataContext = new DialogErrorViewModel()
                        {
                            Message = ex.Message
                        }
                    };
                    DialogHost.Show(view);
                }

                catch (Exception ex)
                {

                }
            }
        }

        private void TextBoxPath_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (Directory.Exists(this.TextBoxPath.Text))
                    {
                        this._fileSystemWalker.SetCurrentPath(this.TextBoxPath.Text);
                    }
                    else
                    {
                        // TODO: Show popup that the directory does not exist
                    }
                    break;
                case Key.Back:
                    if (this._fileSystemWalker.CanTraverseUpwards)
                    {
                        this._fileSystemWalker.TraverseUpwards();
                    }
                    break;
            }
        }
    }
}
