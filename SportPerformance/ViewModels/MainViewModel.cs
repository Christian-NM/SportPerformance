using System.Windows.Input;
using SportPerformance.Helpers;

namespace SportPerformance.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string? _tituloApp;
        public string? TituloApp
        {
            get => _tituloApp;
            set { _tituloApp = value; OnPropertyChanged(); }
        }

        private object? _vistaActual;
        public object? VistaActual
        {
            get => _vistaActual;
            set { _vistaActual = value; OnPropertyChanged(); }
        }

        public ICommand MostrarDashboardCommand { get; }
        public ICommand MostrarEjerciciosCommand { get; }
        public ICommand MostrarEntrenamientosCommand { get; }
        public ICommand MostrarSeriesCommand { get; }

        public MainViewModel()
        {
            TituloApp = "SportPerformance - Gestión de Entrenamiento";

            MostrarDashboardCommand = new RelayCommand(o => VistaActual = new DashboardViewModel());
            MostrarEjerciciosCommand = new RelayCommand(o => VistaActual = new EjerciciosViewModel());
            MostrarEntrenamientosCommand = new RelayCommand(o => VistaActual = new EntrenamientosViewModel());
            MostrarSeriesCommand = new RelayCommand(o => VistaActual = new SeriesViewModel());

            // Establecer el Dashboard como vista inicial al abrir el programa
            VistaActual = new DashboardViewModel();
        }
    }
}