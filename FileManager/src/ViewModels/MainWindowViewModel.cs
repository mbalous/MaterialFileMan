using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;

namespace FileManager.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Commands

        public ActionCommand<int> ChangeTextSizeCommand { get; }
        public ActionCommand SwitchColorSchemeCommand { get; }

        private double _fontSize = 12;
        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                SetProperty(Math.Abs(value), ref _fontSize);
                Debug.WriteLine($"FontSize set to {this.FontSize}");
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            this._paletteHelper = new PaletteHelper();

            this.ChangeTextSizeCommand = new ActionCommand<int>(ChangeTextSize);
            this.SwitchColorSchemeCommand = new ActionCommand(SwitchColorScheme);
        }

        private void ChangeTextSize(int param)
        {
            this.FontSize += param;
        }

        private readonly PaletteHelper _paletteHelper;

        private bool _isDark = false;

        private void SwitchColorScheme(object obj)
        {
            ITheme theme = this._paletteHelper.GetTheme();
            IBaseTheme baseTheme = _isDark ?  new MaterialDesignLightTheme() : new MaterialDesignDarkTheme();
            _isDark = !_isDark;
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }
    }
}
