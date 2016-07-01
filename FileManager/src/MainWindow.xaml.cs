using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace FileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PaletteHelper _paletteHelper;
        
        public MainWindow()
        {
            InitializeComponent();
            this._paletteHelper = new PaletteHelper();
        }

        private void ChangeTextSize_OnClick(object sender, RoutedEventArgs e)
        {
            if (((MenuItem) sender).Equals(this.MenuItemContextDecreaseTextSize))
            {
                this.FileBrowser1.ElementSizing -= 3;
                this.FileBrowser2.ElementSizing -= 3;
            }
            else if (((MenuItem) sender).Equals(this.MenuItemContextIncreaseTextSize))
            {
                this.FileBrowser1.ElementSizing += 3;
                this.FileBrowser2.ElementSizing += 3;
            }
        }

        private void MenuItemColorSwitch_OnClick(object sender, RoutedEventArgs e)
        {
            ResourceDictionary existingResourceDictionary =
                Application.Current.Resources.MergedDictionaries.Where(rd => rd.Source != null).SingleOrDefault(rd =>
                    Regex.Match(rd.Source.OriginalString,
                        @"(\/MaterialDesignThemes.Wpf;component\/Themes\/MaterialDesignTheme\.)((Light)|(Dark))",
                        RegexOptions.Compiled).Success);

            if (existingResourceDictionary == null)
            {
                throw new ApplicationException($"{nameof(existingResourceDictionary)} equals null.");
            }

            this._paletteHelper.SetLightDark(!existingResourceDictionary.Source.ToString().ToLower().Contains("dark"));
        }
    }
}
