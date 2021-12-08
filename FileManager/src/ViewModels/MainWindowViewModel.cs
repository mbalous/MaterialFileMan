using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace FileManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Commands
#pragma warning disable CS0649

        public ActionCommand _decreaseTextSizeCommand;
        public ActionCommand DecreaseTextSizeCommand
        {
            get
            {
                return _decreaseTextSizeCommand ?? ActionCommand.Create((o) =>
                {
                    this.FontSize -= 2;
                });
            }
        }

        private ActionCommand _increaseTextSizeCommand;
        public ActionCommand IncreaseTextSizeCommand
        {
            get
            {
                return _increaseTextSizeCommand ?? ActionCommand.Create((o) =>
                {
                    this.FontSize += 2;
                });
            }
        }

        private ActionCommand _switchColorSchemeCommand;
        public ActionCommand SwitchColorSchemeCommand
        {
            get
            {
                return _switchColorSchemeCommand ?? ActionCommand.Create(SwitchColorScheme);
            }
        }

        private double _fontSize = 12;
        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                SetAndRaise(value, ref _fontSize);
                Debug.WriteLine($"FontSize set to {this.FontSize}");
            }
        }

#pragma warning restore CS0649
        #endregion

        public MainWindowViewModel()
        {
            this._paletteHelper = new PaletteHelper();
        }

        private readonly PaletteHelper _paletteHelper;

        private void SwitchColorScheme(object obj)
        {
            // This code retreives the current color scheme
            System.Collections.Generic.IEnumerable<ResourceDictionary> existingResourceDictionary = Application.Current.Resources.MergedDictionaries.Where(rd => rd.Source != null);
            ResourceDictionary target = existingResourceDictionary.SingleOrDefault(rd =>
                Regex.Match(rd.Source.OriginalString, @"(\/MaterialDesignThemes.Wpf;component\/Themes\/MaterialDesignTheme\.)((Light)|(Dark))",
                RegexOptions.Compiled).Success);

            if (existingResourceDictionary == null)
            {
                throw new ApplicationException($"{nameof(existingResourceDictionary)} equals null.");
            }

            // If the current collor scheme is set to dark,
            // then the method 'SetLightDark' will swap it for light.
            bool isDark = target.Source.ToString().ToLower().Contains("dark");
            var theme = this._paletteHelper.GetTheme();
            //this._paletteHelper.SetLightDark(!isDark);            
        }
    }
}
