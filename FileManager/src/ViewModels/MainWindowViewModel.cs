﻿using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace FileManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly PaletteHelper _paletteHelper;

        public ActionCommand DecreaseTextSizeCommand { get; private set; }
        public ActionCommand IncreaseTextSizeCommand { get; private set; }
        public ActionCommand SwitchColorSchemeCommand { get; private set; }

        private double _fontSize = 12;
        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            this._paletteHelper = new PaletteHelper();

            DecreaseTextSizeCommand = ActionCommand.Create((o) =>
            {
                this.FontSize -= 2;
                Debug.WriteLine($"FontSize set to {this.FontSize}");
            });

            IncreaseTextSizeCommand = ActionCommand.Create((o) =>
            {
                this.FontSize += 2;
                Debug.WriteLine($"FontSize set to {this.FontSize}");
            });

            SwitchColorSchemeCommand = ActionCommand.Create(SwitchColorScheme);
        }

        private void SwitchColorScheme(object obj)
        {

            ResourceDictionary existingResourceDictionary = Application.Current.Resources.MergedDictionaries
                .Where(rd => rd.Source != null)
                .SingleOrDefault(rd =>
                    Regex.Match(rd.Source.OriginalString, @"(\/MaterialDesignThemes.Wpf;component\/Themes\/MaterialDesignTheme\.)((Light)|(Dark))",
                    RegexOptions.Compiled).Success);

            if (existingResourceDictionary == null)
            {
                throw new ApplicationException($"{nameof(existingResourceDictionary)} equals null.");
            }

            this._paletteHelper.SetLightDark(!existingResourceDictionary.Source.ToString().ToLower().Contains("dark"));
        }
    }
}