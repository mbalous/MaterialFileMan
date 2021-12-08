using FileManager.Core;
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

namespace FileManager.Controls
{
    /// <summary>
    /// Interaction logic for FileBrowser.xaml
    /// </summary>
    public partial class FileBrowser : UserControl
    {
        private readonly DirectoryController _fileSystemWrapper;

        public double ElementSizing
        {
            get { return (double)GetValue(ElementSizingProperty); }
            set { SetValue(ElementSizingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElementSizing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementSizingProperty =
            DependencyProperty.Register(nameof(ElementSizing), typeof(double), typeof(FileBrowser), new PropertyMetadata(12d));

        /// <summary>
        /// Create new FileBrowser. 
        /// Current directory is going to be used upon start.
        /// </summary>
        public FileBrowser() : this(Directory.GetCurrentDirectory())
        {
        }

        public FileBrowser(string currentPath)
        {
            this._fileSystemWrapper = new DirectoryController(currentPath, true);

            this.Unloaded += (sender, e) =>
            {
                this._fileSystemWrapper.Dispose();
            };

            InitializeComponent();   

            GetAndSetItems();

            this._fileSystemWrapper.CurrentPathChanged += (s, e) =>
            {
                GetAndSetItems();
            };

            this._fileSystemWrapper.ChildItemsChanged += (sender, e) =>
            {
                if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Deleted)
                {
                    Application.Current.Dispatcher.Invoke(() => GetAndSetItems());
                }
            };
        }

        private void DataGridItems_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) { return; }
            string newValue = ((TextBox)e.EditingElement).Text;
            if (e.EditingElement.DataContext is FileSystemGridItem fsi)
            {
                // Only invoke rename if the new value is different to the old one
                if (fsi.Text != newValue)
                {
                    this._fileSystemWrapper.RenameChildItem(fsi.Text, newValue);
                }
            }
            else
            {
                //throw new ApplicationException("CAN NOT EDIT NON FILE SYSTEM ITEM");
            }
        }

        private void GetAndSetItems()
        {
            List<GridItem> browserItems = new List<GridItem>();
            if (_fileSystemWrapper.CanTraverseUpwards)
            {
                browserItems.Add(new GoUpItem());
            }

            FileSystemInfo[] childItems = _fileSystemWrapper.ChildItems;
            foreach (FileSystemInfo item in childItems)
            {
                if (IsDirectoryInternal(item))
                {
                    browserItems.Add(new DirectoryGridItem(item.Name));
                }
                else
                {
                    browserItems.Add(new FileGridItem(item.Name));
                }
            }

            this.DataGridItems.ItemsSource = browserItems;
            this.TextBoxPath.Text = this._fileSystemWrapper.CurrentPath;

            bool IsDirectoryInternal(FileSystemInfo fsi)
            {
                bool result = (fsi.Attributes & FileAttributes.Directory) != 0;
                return result;
            }
        }

        private void ButtonGoUp_Click(object sender, RoutedEventArgs e)
        {
            if (this._fileSystemWrapper.CanTraverseUpwards)
            {
                this._fileSystemWrapper.TraverseUpwards();
            }
        }

        private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.DataGridItems.SelectedItem == null)
            {
                return;
            }
            e.Handled = true;

            if (this.DataGridItems.SelectedItem is GoUpItem)
            {
                if (this._fileSystemWrapper.CanTraverseUpwards)
                {
                    this._fileSystemWrapper.TraverseUpwards();
                }
                return;
            }
            else if (this.DataGridItems.SelectedItem is DirectoryGridItem directory)
            {
                this._fileSystemWrapper.EnterChildDirectory(directory.Text);
            }
            else if (this.DataGridItems.SelectedItem is FileGridItem file)
            {
                ProcessStartInfo pci = new ProcessStartInfo(file.Text)
                {
                    UseShellExecute = true,
                };
                try
                {
                    using (var process = Process.Start(pci)) { }
                }
                catch (Win32Exception ex) // Win32Exception is thrown when trying to launch .dll for example
                {
                    DialogError view = new DialogError()
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
                    System.Diagnostics.Debug.WriteLine(ex.Message);
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
                        this._fileSystemWrapper.SetCurrentDirectory(this.TextBoxPath.Text);
                    }
                    else
                    {
                        // TODO: Show popup that the directory does not exist
                    }
                    break;
                case Key.Back:
                    if (this._fileSystemWrapper.CanTraverseUpwards)
                    {
                        this._fileSystemWrapper.TraverseUpwards();
                    }
                    break;
            }
        }

        private void DataGridMenuItemRename(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            DataGridCellInfo targetCell = GetTargetCellInternal<MaterialDesignThemes.Wpf.DataGridTextColumn>(e);
            if (targetCell.Item is FileSystemGridItem fsi)
            {
                this.DataGridItems.CurrentCell = targetCell;
                this.DataGridItems.BeginEdit();
            }
        }

        private void DataGridMenuItemDelete(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            DataGridCellInfo targetCell = GetTargetCellInternal<MaterialDesignThemes.Wpf.DataGridTextColumn>(e);
            if (targetCell.Item is FileSystemGridItem fsi)
            {
                this._fileSystemWrapper.DeleteChildItem(fsi.Text);
            }
        }

        private DataGridCellInfo GetTargetCellInternal<T>(RoutedEventArgs e)
        {
            IList<DataGridCellInfo> cells = this.DataGridItems.SelectedCells;
            DataGridCellInfo targetCell = cells.First(x => x.Column.GetType() == typeof(T));
            return targetCell;
        }
    }
}
