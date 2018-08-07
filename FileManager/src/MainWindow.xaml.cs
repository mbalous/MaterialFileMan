using System.Windows;
using System.Windows.Controls;

namespace FileManager
{
    /// <summary>
    /// Interaction logic for <see cref="MainWindow"/>
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}
