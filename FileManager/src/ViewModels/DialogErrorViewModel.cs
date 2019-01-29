using System.ComponentModel;
using System.Windows;

namespace FileManager.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="Views.DialogErrorView"/>
    /// </summary>
    class DialogErrorViewModel : ViewModelBase
    {
        private string _message;

        public DialogErrorViewModel()
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                this.Message = Properties.Resources.NoFileAssociation;
            }
        }

        public DialogErrorViewModel(string message)
        {
            this.Message = message;
        }

        public string Message
        {
            get { return _message; }
            set { SetAndRaise(value, ref _message); }
        }
    }
}
