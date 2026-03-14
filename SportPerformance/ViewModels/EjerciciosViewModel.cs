using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows.Input;
using SportPerformance.Helpers;
using SportPerformance.Models;

namespace SportPerformance.ViewModels
{
    public class EjerciciosViewModel : BaseViewModel
    {
        public ObservableCollection<Ejercicio> ListaEjercicios { get; set; }

        private string? _nuevoNombre;
        public string? NuevoNombre
        {
            get => _nuevoNombre;
            set { _nuevoNombre = value; OnPropertyChanged(); }
        }

        private string? _nuevoMusculo;
        public string? NuevoMusculo
        {
            get => _nuevoMusculo;
            set { _nuevoMusculo = value; OnPropertyChanged(); }
        }

        private Ejercicio? _ejercicioSeleccionado;
        public Ejercicio? EjercicioSeleccionado
        {
            get => _ejercicioSeleccionado;
            set
            {
                _ejercicioSeleccionado = value;
                if (_ejercicioSeleccionado != null)
                {
                    NuevoNombre = _ejercicioSeleccionado.Nombre;
                    NuevoMusculo = _ejercicioSeleccionado.GrupoMuscular;
                }
                OnPropertyChanged();
            }
        }

        public ICommand GuardarEjercicioCommand { get; }
        public ICommand EliminarEjercicioCommand { get; }
        public ICommand ActualizarEjercicioCommand { get; }

        public EjerciciosViewModel()
        {
            ListaEjercicios = new ObservableCollection<Ejercicio>();
            GuardarEjercicioCommand = new RelayCommand(EjecutarGuardar, PuedeGuardar);
            EliminarEjercicioCommand = new RelayCommand(EjecutarEliminar, PuedeEliminar);
            ActualizarEjercicioCommand = new RelayCommand(EjecutarActualizar, PuedeActualizar);
            CargarEjercicios();
        }

        private bool PuedeGuardar(object? obj)
        {
            return !string.IsNullOrWhiteSpace(NuevoNombre) && !string.IsNullOrWhiteSpace(NuevoMusculo) && EjercicioSeleccionado == null;
        }

        private void EjecutarGuardar(object? obj)
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "INSERT INTO EJERCICIOS (nombre, musculo) VALUES (@nombre, @musculo)";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", NuevoNombre);
                    comando.Parameters.AddWithValue("@musculo", NuevoMusculo);
                    comando.ExecuteNonQuery();
                }
            }
            LimpiarFormulario();
            CargarEjercicios();
        }

        private bool PuedeEliminar(object? obj)
        {
            return EjercicioSeleccionado != null;
        }

        private void EjecutarEliminar(object? obj)
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "DELETE FROM EJERCICIOS WHERE id_ejercicio = @id";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", EjercicioSeleccionado!.Id);
                    comando.ExecuteNonQuery();
                }
            }
            LimpiarFormulario();
            CargarEjercicios();
        }

        private bool PuedeActualizar(object? obj)
        {
            return EjercicioSeleccionado != null && !string.IsNullOrWhiteSpace(NuevoNombre) && !string.IsNullOrWhiteSpace(NuevoMusculo);
        }

        private void EjecutarActualizar(object? obj)
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE EJERCICIOS SET nombre = @nombre, musculo = @musculo WHERE id_ejercicio = @id";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", NuevoNombre);
                    comando.Parameters.AddWithValue("@musculo", NuevoMusculo);
                    comando.Parameters.AddWithValue("@id", EjercicioSeleccionado!.Id);
                    comando.ExecuteNonQuery();
                }
            }
            LimpiarFormulario();
            CargarEjercicios();
        }

        private void LimpiarFormulario()
        {
            NuevoNombre = string.Empty;
            NuevoMusculo = string.Empty;
            EjercicioSeleccionado = null;
        }

        private void CargarEjercicios()
        {
            ListaEjercicios.Clear();
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT * FROM EJERCICIOS";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            ListaEjercicios.Add(new Ejercicio
                            {
                                Id = lector.GetInt32(0),
                                Nombre = lector.GetString(1),
                                GrupoMuscular = lector.GetString(2)
                            });
                        }
                    }
                }
            }
        }
    }
}