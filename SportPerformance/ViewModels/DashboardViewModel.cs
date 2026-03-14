using System;
using System.Data.SQLite;
using SportPerformance.Helpers;

namespace SportPerformance.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private int _totalEntrenamientos;
        public int TotalEntrenamientos
        {
            get => _totalEntrenamientos;
            set { _totalEntrenamientos = value; OnPropertyChanged(); }
        }

        private int _totalEjercicios;
        public int TotalEjercicios
        {
            get => _totalEjercicios;
            set { _totalEjercicios = value; OnPropertyChanged(); }
        }

        private int _totalSeries;
        public int TotalSeries
        {
            get => _totalSeries;
            set { _totalSeries = value; OnPropertyChanged(); }
        }

        private string? _ultimoEntrenamiento;
        public string? UltimoEntrenamiento
        {
            get => _ultimoEntrenamiento;
            set { _ultimoEntrenamiento = value; OnPropertyChanged(); }
        }

        public DashboardViewModel()
        {
            CargarEstadisticas();
        }

        private void CargarEstadisticas()
        {
            using (var conexion = DatabaseHelper.GetConnection())
            {
                string sqlEnt = "SELECT COUNT(*) FROM ENTRENAMIENTOS";
                using (var cmd = new SQLiteCommand(sqlEnt, conexion))
                {
                    TotalEntrenamientos = Convert.ToInt32(cmd.ExecuteScalar());
                }

                string sqlEj = "SELECT COUNT(*) FROM EJERCICIOS";
                using (var cmd = new SQLiteCommand(sqlEj, conexion))
                {
                    TotalEjercicios = Convert.ToInt32(cmd.ExecuteScalar());
                }

                string sqlSer = "SELECT COUNT(*) FROM SERIES";
                using (var cmd = new SQLiteCommand(sqlSer, conexion))
                {
                    TotalSeries = Convert.ToInt32(cmd.ExecuteScalar());
                }

                string sqlUlt = "SELECT MAX(fecha) FROM ENTRENAMIENTOS";
                using (var cmd = new SQLiteCommand(sqlUlt, conexion))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        UltimoEntrenamiento = Convert.ToDateTime(result).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        UltimoEntrenamiento = "Sin registros";
                    }
                }
            }
        }
    }
}