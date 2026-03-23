using System;
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

        private string _nuevoNombre = string.Empty;
        public string NuevoNombre
        {
            get => _nuevoNombre;
            set { _nuevoNombre = value; OnPropertyChanged(); }
        }

        private string _nuevoMusculo = string.Empty;
        public string NuevoMusculo
        {
            get => _nuevoMusculo;
            set { _nuevoMusculo = value; OnPropertyChanged(); }
        }

        private Ejercicio _ejercicioSeleccionado;
        public Ejercicio EjercicioSeleccionado
        {
            get => _ejercicioSeleccionado;
            set
            {
                _ejercicioSeleccionado = value;
                OnPropertyChanged();

                // Al seleccionar un ejercicio, cargamos sus datos en los cuadros de texto para poder editar o eliminar
                if (_ejercicioSeleccionado != null)
                {
                    NuevoNombre = _ejercicioSeleccionado.Nombre;
                    NuevoMusculo = _ejercicioSeleccionado.Musculo;
                }
            }
        }

        public ICommand GuardarEjercicioCommand { get; set; }
        public ICommand ActualizarEjercicioCommand { get; set; }
        public ICommand EliminarEjercicioCommand { get; set; }

        public EjerciciosViewModel()
        {
            ListaEjercicios = new ObservableCollection<Ejercicio>();

            // INICIALIZACIÓN DE COMANDOS
            GuardarEjercicioCommand = new RelayCommand(GuardarEjercicio, p => !string.IsNullOrEmpty(NuevoNombre));
            ActualizarEjercicioCommand = new RelayCommand(ActualizarEjercicio, p => EjercicioSeleccionado != null);
            EliminarEjercicioCommand = new RelayCommand(EliminarEjercicio, p => EjercicioSeleccionado != null);

            CargarEjercicios();
        }

        private void CargarEjercicios()
        {
            try
            {
                ListaEjercicios.Clear();
                using (var conexion = DatabaseHelper.GetConnection())
                {
                    string consulta = "SELECT * FROM EJERCICIOS";
                    using (var cmd = new SQLiteCommand(consulta, conexion))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListaEjercicios.Add(new Ejercicio
                            {
                                IdEjercicio = Convert.ToInt32(reader["id_ejercicio"]),
                                Nombre = reader["nombre"].ToString(),
                                Musculo = reader["musculo"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar ejercicios: " + ex.Message);
            }
        }

        private void GuardarEjercicio(object p)
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string consulta = "INSERT INTO EJERCICIOS (nombre, musculo) VALUES (@nom, @mus)";
                using (var cmd = new SQLiteCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@nom", NuevoNombre);
                    cmd.Parameters.AddWithValue("@mus", NuevoMusculo);
                    cmd.ExecuteNonQuery();
                }
            }
            LimpiarYRefrescar();
        }

        private void ActualizarEjercicio(object p)
        {
            if (EjercicioSeleccionado == null) return;
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string consulta = "UPDATE EJERCICIOS SET nombre=@nom, musculo=@mus WHERE id_ejercicio=@id";
                using (var cmd = new SQLiteCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@nom", NuevoNombre);
                    cmd.Parameters.AddWithValue("@mus", NuevoMusculo);
                    cmd.Parameters.AddWithValue("@id", EjercicioSeleccionado.IdEjercicio);
                    cmd.ExecuteNonQuery();
                }
            }
            LimpiarYRefrescar();
        }

        private void EliminarEjercicio(object p)
        {
            if (EjercicioSeleccionado == null) return;
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string consulta = "DELETE FROM EJERCICIOS WHERE id_ejercicio = @id";
                using (var cmd = new SQLiteCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", EjercicioSeleccionado.IdEjercicio);
                    cmd.ExecuteNonQuery();
                }
            }
            LimpiarYRefrescar();
        }

        private void LimpiarYRefrescar()
        {
            NuevoNombre = string.Empty;
            NuevoMusculo = string.Empty;
            EjercicioSeleccionado = null;
            CargarEjercicios();
        }
    }
}