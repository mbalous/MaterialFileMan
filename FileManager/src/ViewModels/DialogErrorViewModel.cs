using System.ComponentModel;
using System.Windows;

namespace FileManager.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="Views.DialogErrorView"/>
    /// </summary>
    class DialogErrorViewModel : BindableBase
    {
        private string _message;

        public DialogErrorViewModel()
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                this.Message = "No application is associated with the specified file for this operation.";
            }
        }

        public DialogErrorViewModel(string message)
        {
            this.Message = message;
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(value, ref _message); }
        }
    }
}