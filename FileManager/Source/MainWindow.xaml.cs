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
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChangeTextSize_OnClick(object sender, RoutedEventArgs e)
        {
            if (((MenuItem) sender).Equals(MenuItemContextDecreaseTextSize))
            {
                this.FileBrowser1.ElementSizing -= 3;
                this.FileBrowser2.ElementSizing -= 3;
            }
            else if (((MenuItem) sender).Equals(MenuItemContextIncreaseTextSize))
            {
                this.FileBrowser1.ElementSizing += 3;
                this.FileBrowser2.ElementSizing += 3;
            }
        }

        private void MenuItemColorSwitch_OnClick(object sender, RoutedEventArgs e)
        {
            ResourceDictionary existingResourceDictionary = Application.Current.Resources.MergedDictionaries
               .Where(rd => rd.Source != null)
               .SingleOrDefault(rd => Regex.Match(rd.Source.OriginalString, @"(\/MaterialDesignThemes.Wpf;component\/Themes\/MaterialDesignTheme\.)((Light)|(Dark))", RegexOptions.Compiled).Success);
            if (existingResourceDictionary == null)
                throw new ApplicationException($"{nameof(existingResourceDictionary)} equals null.");

            if (existingResourceDictionary.Source.ToString().ToLower().Contains("dark"))
                new PaletteHelper().SetLightDark(false);
            else
                new PaletteHelper().SetLightDark(true);
        }
    }
}