using SportPerformance.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SportPerformance.Repositories
{
    public class SerieRepository : IRepository<Serie>
    {
        private readonly string _connectionString = "Data Source=SportPerformance.db;Version=3;";

        public IEnumerable<Serie> GetAll()
        {
            var lista = new List<Serie>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT s.id_serie, s.id_ejercicio, s.id_entrenamiento, s.peso, s.repeticiones,
                           e.nombre as EjercicioNombre, t.fecha as FechaEntrenamiento
                    FROM SERIES s
                    INNER JOIN EJERCICIOS e ON s.id_ejercicio = e.id_ejercicio
                    INNER JOIN ENTRENAMIENTOS t ON s.id_entrenamiento = t.id_entrenamiento";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Serie
                        {
                            Id = Convert.ToInt32(reader["id_serie"]),
                            IdEjercicio = Convert.ToInt32(reader["id_ejercicio"]),
                            IdEntrenamiento = Convert.ToInt32(reader["id_entrenamiento"]),
                            Peso = Convert.ToDouble(reader["peso"]),
                            Repeticiones = Convert.ToInt32(reader["repeticiones"]),
                            EjercicioNombre = reader["EjercicioNombre"].ToString(),
                            EntrenamientoInfo = Convert.ToDateTime(reader["FechaEntrenamiento"]).ToString("dd/MM/yyyy")
                        });
                    }
                }
            }
            return lista;
        }

        public Serie GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(Serie entity)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var cmdPragma = new SQLiteCommand("PRAGMA foreign_keys = ON", connection))
                {
                    cmdPragma.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand("INSERT INTO SERIES (id_ejercicio, id_entrenamiento, peso, repeticiones) VALUES (@id_ejercicio, @id_entrenamiento, @peso, @repeticiones)", connection))
                {
                    command.Parameters.AddWithValue("@id_ejercicio", entity.IdEjercicio);
                    command.Parameters.AddWithValue("@id_entrenamiento", entity.IdEntrenamiento);
                    command.Parameters.AddWithValue("@peso", entity.Peso);
                    command.Parameters.AddWithValue("@repeticiones", entity.Repeticiones);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Serie entity)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("UPDATE SERIES SET id_ejercicio = @id_ejercicio, id_entrenamiento = @id_entrenamiento, peso = @peso, repeticiones = @repeticiones WHERE id_serie = @id", connection))
                {
                    command.Parameters.AddWithValue("@id_ejercicio", entity.IdEjercicio);
                    command.Parameters.AddWithValue("@id_entrenamiento", entity.IdEntrenamiento);
                    command.Parameters.AddWithValue("@peso", entity.Peso);
                    command.Parameters.AddWithValue("@repeticiones", entity.Repeticiones);
                    command.Parameters.AddWithValue("@id", entity.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("DELETE FROM SERIES WHERE id_serie = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}