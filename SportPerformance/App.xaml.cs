using System;
using System.Windows;
using SportPerformance.Helpers;

namespace SportPerformance
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Sobrescribe el método de inicio para ejecutar la inicialización de la base de datos.
        /// </summary>
        /// <param name="e">Argumentos del evento de inicio.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Ejecución obligatoria de la lógica base de WPF
            base.OnStartup(e);

            try
            {
                // Invocación del motor de persistencia
                DatabaseHelper.InitializeDatabase();
            }
            catch (Exception ex)
            {
                // Notificación visual de error crítico
                MessageBox.Show($"Error crítico al inicializar la base de datos: {ex.Message}",
                                "Error de Persistencia",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                // Finalización del proceso para evitar estados inconsistentes
                Shutdown();
            }
        }
    }
}