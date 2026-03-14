using System;
using System.Data.SQLite;
using System.IO;

namespace SportPerformance.Helpers
{
    public static class DatabaseHelper
    {
        private const string dbFileName = "SportPerformance.db";
        private static string connectionString = $"Data Source={dbFileName};Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    PRAGMA foreign_keys = ON;
                    CREATE TABLE IF NOT EXISTS EJERCICIOS (
                        id_ejercicio INTEGER PRIMARY KEY AUTOINCREMENT,
                        nombre TEXT NOT NULL,
                        musculo TEXT NOT NULL
                    );
                    CREATE TABLE IF NOT EXISTS ENTRENAMIENTOS (
                        id_entrenamiento INTEGER PRIMARY KEY AUTOINCREMENT,
                        fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
                        comentario TEXT
                    );
                    CREATE TABLE IF NOT EXISTS SERIES (
                        id_serie INTEGER PRIMARY KEY AUTOINCREMENT,
                        id_ejercicio INTEGER NOT NULL,
                        id_entrenamiento INTEGER NOT NULL,
                        peso REAL NOT NULL,
                        repeticiones INTEGER NOT NULL,
                        FOREIGN KEY (id_ejercicio) REFERENCES EJERCICIOS (id_ejercicio) ON DELETE CASCADE,
                        FOREIGN KEY (id_entrenamiento) REFERENCES ENTRENAMIENTOS (id_entrenamiento) ON DELETE CASCADE
                    );";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();

            using (var command = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
            {
                command.ExecuteNonQuery();
            }

            return connection;
        }
    }
}