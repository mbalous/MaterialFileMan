using System.Windows;
using System.Windows.Input;

namespace FileManager
{
    public class SystemMenuItem : Freezable
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
           nameof(Command), typeof(ICommand), typeof(SystemMenuItem), new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(SystemMenuItem));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header), typeof(string), typeof(SystemMenuItem));

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            nameof(Id), typeof(int), typeof(SystemMenuItem));

        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new SystemMenuItem();
        }

        private static void OnCommandChanged(DependencyObject depObject, DependencyPropertyChangedEventArgs e)
        {
            if (depObject is SystemMenuItem systemMenuItem)
            {
                if (e.NewValue != null)
                {
                    systemMenuItem.Command = e.NewValue as ICommand;
                }
            }
        }
    }
}
