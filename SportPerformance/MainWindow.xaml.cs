using System.Windows;
using SportPerformance.ViewModels;

namespace SportPerformance
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Vinculamos la lógica del ViewModel con esta ventana
            this.DataContext = new MainViewModel();
        }
    }
}