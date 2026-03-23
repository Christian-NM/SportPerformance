using SportPerformance.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SportPerformance.Repositories
{
    public class EntrenamientoRepository : IRepository<Entrenamiento>
    {
        private readonly string _connectionString = "Data Source=SportPerformance.db;Version=3;";

        public IEnumerable<Entrenamiento> GetAll()
        {
            var lista = new List<Entrenamiento>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT * FROM ENTRENAMIENTOS", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Entrenamiento
                        {
                            Id = Convert.ToInt32(reader["id_entrenamiento"]),
                            Fecha = Convert.ToDateTime(reader["fecha"]),
                            Comentario = reader["comentario"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            return lista;
        }

        public Entrenamiento GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(Entrenamiento entity)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("INSERT INTO ENTRENAMIENTOS (fecha, comentario) VALUES (@fecha, @comentario)", connection))
                {
                    command.Parameters.AddWithValue("@fecha", entity.Fecha.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@comentario", entity.Comentario);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Entrenamiento entity)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("UPDATE ENTRENAMIENTOS SET fecha = @fecha, comentario = @comentario WHERE id_entrenamiento = @id", connection))
                {
                    command.Parameters.AddWithValue("@fecha", entity.Fecha.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@comentario", entity.Comentario);
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
                using (var command = new SQLiteCommand("DELETE FROM ENTRENAMIENTOS WHERE id_entrenamiento = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}