using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows.Input;
using SportPerformance.Helpers;
using SportPerformance.Models;

namespace SportPerformance.ViewModels
{
    public class EntrenamientosViewModel : BaseViewModel
    {
        public ObservableCollection<Entrenamiento> ListaEntrenamientos { get; set; }

        private DateTime _nuevaFecha = DateTime.Now;
        public DateTime NuevaFecha
        {
            get => _nuevaFecha;
            set { _nuevaFecha = value; OnPropertyChanged(); }
        }

        private string? _nuevoComentario;
        public string? NuevoComentario
        {
            get => _nuevoComentario;
            set { _nuevoComentario = value; OnPropertyChanged(); }
        }

        private Entrenamiento? _entrenamientoSeleccionado;
        public Entrenamiento? EntrenamientoSeleccionado
        {
            get => _entrenamientoSeleccionado;
            set
            {
                _entrenamientoSeleccionado = value;
                if (_entrenamientoSeleccionado != null)
                {
                    NuevaFecha = _entrenamientoSeleccionado.Fecha;
                    NuevoComentario = _entrenamientoSeleccionado.Comentario;
                }
                OnPropertyChanged();
            }
        }

        public ICommand GuardarEntrenamientoCommand { get; }
        public ICommand EliminarEntrenamientoCommand { get; }
        public ICommand ActualizarEntrenamientoCommand { get; }

        public EntrenamientosViewModel()
        {
            ListaEntrenamientos = new ObservableCollection<Entrenamiento>();
            GuardarEntrenamientoCommand = new RelayCommand(EjecutarGuardar, PuedeGuardar);
            EliminarEntrenamientoCommand = new RelayCommand(EjecutarEliminar, PuedeEliminar);
            ActualizarEntrenamientoCommand = new RelayCommand(EjecutarActualizar, PuedeActualizar);
            CargarEntrenamientos();
        }

        private bool PuedeGuardar(object? obj)
        {
            return EntrenamientoSeleccionado == null;
        }

        private void EjecutarGuardar(object? obj)
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "INSERT INTO ENTRENAMIENTOS (fecha, comentario) VALUES (@fecha, @comentario)";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@fecha", NuevaFecha.ToString("yyyy-MM-dd HH:mm:ss"));
                    comando.Parameters.AddWithValue("@comentario", NuevoComentario ?? string.Empty);
                    comando.ExecuteNonQuery();
                }
            }
            LimpiarFormulario();
            CargarEntrenamientos();
        }

        private bool PuedeEliminar(object? obj)
        {
            return EntrenamientoSeleccionado != null;
        }

        private void EjecutarEliminar(object? obj)
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "DELETE FROM ENTRENAMIENTOS WHERE id_entrenamiento = @id";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", EntrenamientoSeleccionado!.Id);
                    comando.ExecuteNonQuery();
                }
            }
            LimpiarFormulario();
            CargarEntrenamientos();
        }

        private bool PuedeActualizar(object? obj)
        {
            return EntrenamientoSeleccionado != null;
        }

        private void EjecutarActualizar(object? obj)
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE ENTRENAMIENTOS SET fecha = @fecha, comentario = @comentario WHERE id_entrenamiento = @id";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@fecha", NuevaFecha.ToString("yyyy-MM-dd HH:mm:ss"));
                    comando.Parameters.AddWithValue("@comentario", NuevoComentario ?? string.Empty);
                    comando.Parameters.AddWithValue("@id", EntrenamientoSeleccionado!.Id);
                    comando.ExecuteNonQuery();
                }
            }
            LimpiarFormulario();
            CargarEntrenamientos();
        }

        private void LimpiarFormulario()
        {
            NuevaFecha = DateTime.Now;
            NuevoComentario = string.Empty;
            EntrenamientoSeleccionado = null;
        }

        private void CargarEntrenamientos()
        {
            ListaEntrenamientos.Clear();
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT id_entrenamiento, fecha, comentario FROM ENTRENAMIENTOS ORDER BY fecha DESC";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            ListaEntrenamientos.Add(new Entrenamiento
                            {
                                Id = lector.GetInt32(0),
                                Fecha = lector.GetDateTime(1),
                                Comentario = lector.IsDBNull(2) ? string.Empty : lector.GetString(2)
                            });
                        }
                    }
                }
            }
        }
    }
}