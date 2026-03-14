using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Input;
using SportPerformance.Helpers;
using SportPerformance.Models;

namespace SportPerformance.ViewModels
{
    public class SerieDisplay
    {
        public int Id { get; set; }
        public int IdEntrenamiento { get; set; }
        public int IdEjercicio { get; set; }
        public string? EntrenamientoInfo { get; set; }
        public string? EjercicioNombre { get; set; }
        public double Peso { get; set; }
        public int Repeticiones { get; set; }
    }

    public class SeriesViewModel : BaseViewModel
    {
        public ObservableCollection<SerieDisplay> ListaSeries { get; set; }
        public ObservableCollection<Entrenamiento> ListaEntrenamientos { get; set; }
        public ObservableCollection<Ejercicio> ListaEjercicios { get; set; }

        private Entrenamiento? _entrenamientoSeleccionado;
        public Entrenamiento? EntrenamientoSeleccionado
        {
            get => _entrenamientoSeleccionado;
            set { _entrenamientoSeleccionado = value; OnPropertyChanged(); }
        }

        private Ejercicio? _ejercicioSeleccionado;
        public Ejercicio? EjercicioSeleccionado
        {
            get => _ejercicioSeleccionado;
            set { _ejercicioSeleccionado = value; OnPropertyChanged(); }
        }

        private double _peso;
        public double Peso
        {
            get => _peso;
            set { _peso = value; OnPropertyChanged(); }
        }

        private int _repeticiones;
        public int Repeticiones
        {
            get => _repeticiones;
            set { _repeticiones = value; OnPropertyChanged(); }
        }

        private SerieDisplay? _serieSeleccionada;
        public SerieDisplay? SerieSeleccionada
        {
            get => _serieSeleccionada;
            set
            {
                _serieSeleccionada = value;
                if (_serieSeleccionada != null)
                {
                    EntrenamientoSeleccionado = ListaEntrenamientos.FirstOrDefault(e => e.Id == _serieSeleccionada.IdEntrenamiento);
                    EjercicioSeleccionado = ListaEjercicios.FirstOrDefault(e => e.Id == _serieSeleccionada.IdEjercicio);
                    Peso = _serieSeleccionada.Peso;
                    Repeticiones = _serieSeleccionada.Repeticiones;
                }
                OnPropertyChanged();
            }
        }

        public ICommand GuardarSerieCommand { get; }
        public ICommand EliminarSerieCommand { get; }
        public ICommand ActualizarSerieCommand { get; }

        public SeriesViewModel()
        {
            ListaSeries = new ObservableCollection<SerieDisplay>();
            ListaEntrenamientos = new ObservableCollection<Entrenamiento>();
            ListaEjercicios = new ObservableCollection<Ejercicio>();

            GuardarSerieCommand = new RelayCommand(EjecutarGuardar, PuedeGuardar);
            EliminarSerieCommand = new RelayCommand(EjecutarEliminar, PuedeEliminar);
            ActualizarSerieCommand = new RelayCommand(EjecutarActualizar, PuedeActualizar);

            CargarListasDesplegables();
            CargarSeries();
        }

        private bool PuedeGuardar(object? obj)
        {
            return EntrenamientoSeleccionado != null && EjercicioSeleccionado != null && Peso > 0 && Repeticiones > 0 && SerieSeleccionada == null;
        }

        private void EjecutarGuardar(object? obj)
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "INSERT INTO SERIES (id_ejercicio, id_entrenamiento, peso, repeticiones) VALUES (@idEj, @idEnt, @peso, @reps)";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@idEj", EjercicioSeleccionado!.Id);
                    comando.Parameters.AddWithValue("@idEnt", EntrenamientoSeleccionado!.Id);
                    comando.Parameters.AddWithValue("@peso", Peso);
                    comando.Parameters.AddWithValue("@reps", Repeticiones);
                    comando.ExecuteNonQuery();
                }
            }
            LimpiarFormulario();
            CargarSeries();
        }

        private bool PuedeEliminar(object? obj)
        {
            return SerieSeleccionada != null;
        }

        private void EjecutarEliminar(object? obj)
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "DELETE FROM SERIES WHERE id_serie = @id";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", SerieSeleccionada!.Id);
                    comando.ExecuteNonQuery();
                }
            }
            LimpiarFormulario();
            CargarSeries();
        }

        private bool PuedeActualizar(object? obj)
        {
            return SerieSeleccionada != null && EntrenamientoSeleccionado != null && EjercicioSeleccionado != null && Peso > 0 && Repeticiones > 0;
        }

        private void EjecutarActualizar(object? obj)
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE SERIES SET id_ejercicio = @idEj, id_entrenamiento = @idEnt, peso = @peso, repeticiones = @reps WHERE id_serie = @id";
                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@idEj", EjercicioSeleccionado!.Id);
                    comando.Parameters.AddWithValue("@idEnt", EntrenamientoSeleccionado!.Id);
                    comando.Parameters.AddWithValue("@peso", Peso);
                    comando.Parameters.AddWithValue("@reps", Repeticiones);
                    comando.Parameters.AddWithValue("@id", SerieSeleccionada!.Id);
                    comando.ExecuteNonQuery();
                }
            }
            LimpiarFormulario();
            CargarSeries();
        }

        private void LimpiarFormulario()
        {
            EntrenamientoSeleccionado = null;
            EjercicioSeleccionado = null;
            Peso = 0;
            Repeticiones = 0;
            SerieSeleccionada = null;
        }

        private void CargarListasDesplegables()
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sqlEj = "SELECT id_ejercicio, nombre, musculo FROM EJERCICIOS";
                using (var cmd = new SQLiteCommand(sqlEj, conexion))
                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        ListaEjercicios.Add(new Ejercicio { Id = lector.GetInt32(0), Nombre = lector.GetString(1), GrupoMuscular = lector.GetString(2) });
                    }
                }

                string sqlEnt = "SELECT id_entrenamiento, fecha, comentario FROM ENTRENAMIENTOS ORDER BY fecha DESC";
                using (var cmd = new SQLiteCommand(sqlEnt, conexion))
                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        ListaEntrenamientos.Add(new Entrenamiento { Id = lector.GetInt32(0), Fecha = lector.GetDateTime(1), Comentario = lector.IsDBNull(2) ? string.Empty : lector.GetString(2) });
                    }
                }
            }
        }

        private void CargarSeries()
        {
            ListaSeries.Clear();
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT s.id_serie, s.id_entrenamiento, s.id_ejercicio, en.fecha, ej.nombre, s.peso, s.repeticiones
                    FROM SERIES s
                    INNER JOIN ENTRENAMIENTOS en ON s.id_entrenamiento = en.id_entrenamiento
                    INNER JOIN EJERCICIOS ej ON s.id_ejercicio = ej.id_ejercicio
                    ORDER BY s.id_serie DESC";

                using (var comando = new SQLiteCommand(sql, conexion))
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        ListaSeries.Add(new SerieDisplay
                        {
                            Id = lector.GetInt32(0),
                            IdEntrenamiento = lector.GetInt32(1),
                            IdEjercicio = lector.GetInt32(2),
                            EntrenamientoInfo = lector.GetDateTime(3).ToString("dd/MM/yyyy"),
                            EjercicioNombre = lector.GetString(4),
                            Peso = lector.GetDouble(5),
                            Repeticiones = lector.GetInt32(6)
                        });
                    }
                }
            }
        }
    }
}