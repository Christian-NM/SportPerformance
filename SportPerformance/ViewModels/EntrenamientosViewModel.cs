using SportPerformance.Helpers;
using SportPerformance.Models;
using SportPerformance.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SportPerformance.ViewModels
{
    public class EntrenamientosViewModel : BaseViewModel
    {
        private readonly IRepository<Entrenamiento> _entrenamientoRepository;
        private ObservableCollection<Entrenamiento> _listaEntrenamientos;
        private Entrenamiento _entrenamientoSeleccionado;
        private DateTime _nuevaFecha = DateTime.Now;
        private string _nuevoComentario = string.Empty;

        public ObservableCollection<Entrenamiento> ListaEntrenamientos
        {
            get => _listaEntrenamientos;
            set { _listaEntrenamientos = value; OnPropertyChanged(nameof(ListaEntrenamientos)); }
        }

        public Entrenamiento EntrenamientoSeleccionado
        {
            get => _entrenamientoSeleccionado;
            set
            {
                _entrenamientoSeleccionado = value;
                OnPropertyChanged(nameof(EntrenamientoSeleccionado));
                if (_entrenamientoSeleccionado != null)
                {
                    NuevaFecha = _entrenamientoSeleccionado.Fecha;
                    NuevoComentario = _entrenamientoSeleccionado.Comentario;
                }
            }
        }

        public DateTime NuevaFecha
        {
            get => _nuevaFecha;
            set { _nuevaFecha = value; OnPropertyChanged(nameof(NuevaFecha)); }
        }

        public string NuevoComentario
        {
            get => _nuevoComentario;
            set { _nuevoComentario = value; OnPropertyChanged(nameof(NuevoComentario)); }
        }

        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }

        public EntrenamientosViewModel()
        {
            _entrenamientoRepository = new EntrenamientoRepository();
            _listaEntrenamientos = new ObservableCollection<Entrenamiento>();

            GuardarCommand = new RelayCommand(Guardar);
            EliminarCommand = new RelayCommand(Eliminar, (p) => EntrenamientoSeleccionado != null);

            CargarEntrenamientos();
        }

        private void CargarEntrenamientos()
        {
            var datos = _entrenamientoRepository.GetAll();
            ListaEntrenamientos.Clear();
            foreach (var item in datos) ListaEntrenamientos.Add(item);
        }

        private void Guardar(object parameter)
        {
            if (EntrenamientoSeleccionado == null)
            {
                _entrenamientoRepository.Add(new Entrenamiento { Fecha = NuevaFecha, Comentario = NuevoComentario });
            }
            else
            {
                EntrenamientoSeleccionado.Fecha = NuevaFecha;
                EntrenamientoSeleccionado.Comentario = NuevoComentario;
                _entrenamientoRepository.Update(EntrenamientoSeleccionado);
            }
            CargarEntrenamientos();
        }

        private void Eliminar(object parameter)
        {
            if (EntrenamientoSeleccionado != null)
            {
                _entrenamientoRepository.Delete(EntrenamientoSeleccionado.Id);
                CargarEntrenamientos();
            }
        }
    }
}