using SportPerformance.Helpers;
using SportPerformance.Models;
using SportPerformance.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SportPerformance.ViewModels
{
    public class SeriesViewModel : BaseViewModel
    {
        private readonly IRepository<Serie> _serieRepository;
        private readonly IRepository<Ejercicio> _ejercicioRepository;
        private readonly IRepository<Entrenamiento> _entrenamientoRepository;

        private ObservableCollection<Serie> _listaSeries = new();
        private ObservableCollection<Ejercicio> _listaEjercicios = new();
        private ObservableCollection<Entrenamiento> _listaEntrenamientos = new();

        private Serie? _serieSeleccionada;
        private Ejercicio? _ejercicioSeleccionado;
        private Entrenamiento? _entrenamientoSeleccionado;
        private double _nuevoPeso;
        private int _nuevasRepeticiones;

        public ObservableCollection<Serie> ListaSeries
        {
            get => _listaSeries;
            set { _listaSeries = value; OnPropertyChanged(nameof(ListaSeries)); }
        }

        public ObservableCollection<Ejercicio> ListaEjercicios
        {
            get => _listaEjercicios;
            set { _listaEjercicios = value; OnPropertyChanged(nameof(ListaEjercicios)); }
        }

        public ObservableCollection<Entrenamiento> ListaEntrenamientos
        {
            get => _listaEntrenamientos;
            set { _listaEntrenamientos = value; OnPropertyChanged(nameof(ListaEntrenamientos)); }
        }

        public Serie? SerieSeleccionada
        {
            get => _serieSeleccionada;
            set
            {
                _serieSeleccionada = value;
                OnPropertyChanged(nameof(SerieSeleccionada));
                if (_serieSeleccionada != null)
                {
                    NuevoPeso = _serieSeleccionada.Peso;
                    NuevasRepeticiones = _serieSeleccionada.Repeticiones;
                    EjercicioSeleccionado = ListaEjercicios.FirstOrDefault(e => e.IdEjercicio == _serieSeleccionada.IdEjercicio);
                    EntrenamientoSeleccionado = ListaEntrenamientos.FirstOrDefault(en => en.Id == _serieSeleccionada.IdEntrenamiento);
                }
            }
        }

        public Ejercicio? EjercicioSeleccionado
        {
            get => _ejercicioSeleccionado;
            set { _ejercicioSeleccionado = value; OnPropertyChanged(nameof(EjercicioSeleccionado)); }
        }

        public Entrenamiento? EntrenamientoSeleccionado
        {
            get => _entrenamientoSeleccionado;
            set { _entrenamientoSeleccionado = value; OnPropertyChanged(nameof(EntrenamientoSeleccionado)); }
        }

        public double NuevoPeso
        {
            get => _nuevoPeso;
            set { _nuevoPeso = value; OnPropertyChanged(nameof(NuevoPeso)); }
        }

        public int NuevasRepeticiones
        {
            get => _nuevasRepeticiones;
            set { _nuevasRepeticiones = value; OnPropertyChanged(nameof(NuevasRepeticiones)); }
        }

        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }

        public SeriesViewModel()
        {
            _serieRepository = new SerieRepository();
            _ejercicioRepository = new EjercicioRepository();
            _entrenamientoRepository = new EntrenamientoRepository();

            GuardarCommand = new RelayCommand(Guardar, PuedeGuardar);
            EliminarCommand = new RelayCommand(Eliminar, (p) => SerieSeleccionada != null);

            CargarDatos();
        }

        private void CargarDatos()
        {
            ListaSeries = new ObservableCollection<Serie>(_serieRepository.GetAll());
            ListaEjercicios = new ObservableCollection<Ejercicio>(_ejercicioRepository.GetAll());
            ListaEntrenamientos = new ObservableCollection<Entrenamiento>(_entrenamientoRepository.GetAll());
        }

        private bool PuedeGuardar(object? p) => EjercicioSeleccionado != null && EntrenamientoSeleccionado != null;

        private void Guardar(object? p)
        {
            var serie = new Serie
            {
                IdEjercicio = EjercicioSeleccionado!.IdEjercicio,
                IdEntrenamiento = EntrenamientoSeleccionado!.Id,
                Peso = NuevoPeso,
                Repeticiones = NuevasRepeticiones
            };

            if (SerieSeleccionada == null)
            {
                _serieRepository.Add(serie);
            }
            else
            {
                serie.Id = SerieSeleccionada.Id;
                _serieRepository.Update(serie);
            }

            CargarDatos();
        }

        private void Eliminar(object? p)
        {
            if (SerieSeleccionada != null)
            {
                _serieRepository.Delete(SerieSeleccionada.Id);
                CargarDatos();
            }
        }
    }
}