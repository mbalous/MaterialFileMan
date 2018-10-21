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
        public double ElementSizing
        {
            get { return (double)GetValue(ElementSizingProperty); }
            set { SetValue(ElementSizingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElementSizing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementSizingProperty =
            DependencyProperty.Register("ElementSizing", typeof(double), typeof(FileBrowser), new PropertyMetadata(12d));

        private FileBrowserViewModel _dataContext => this.DataContext as FileBrowserViewModel;

        public FileBrowser()
        {
            InitializeComponent();
            this.DataContext = new FileBrowserViewModel();
        }        

        private void TextBoxPath_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (Directory.Exists(this.TextBoxPath.Text))
                    {
                        _dataContext.CurrentPath = this.TextBoxPath.Text;
                    }
                    break;
                case Key.Back:
                    _dataContext.GoUpButtonClick.Execute();
                    break;
            }
        }
    }
}