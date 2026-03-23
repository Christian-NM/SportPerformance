using SportPerformance.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SportPerformance.Repositories
{
    public class EjercicioRepository : IRepository<Ejercicio>
    {
        private readonly string _connectionString = "Data Source=SportPerformance.db;Version=3;";

        public IEnumerable<Ejercicio> GetAll()
        {
            var lista = new List<Ejercicio>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT * FROM EJERCICIOS", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Ejercicio
                        {
                            IdEjercicio = Convert.ToInt32(reader["id_ejercicio"]),
                            Nombre = reader["nombre"].ToString(),
                            Musculo = reader["musculo"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public Ejercicio GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(Ejercicio entity)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("INSERT INTO EJERCICIOS (nombre, musculo) VALUES (@nombre, @musculo)", connection))
                {
                    command.Parameters.AddWithValue("@nombre", entity.Nombre);
                    command.Parameters.AddWithValue("@musculo", entity.Musculo);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Ejercicio entity)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("UPDATE EJERCICIOS SET nombre = @nombre, musculo = @musculo WHERE id_ejercicio = @id", connection))
                {
                    command.Parameters.AddWithValue("@nombre", entity.Nombre);
                    command.Parameters.AddWithValue("@musculo", entity.Musculo);
                    command.Parameters.AddWithValue("@id", entity.IdEjercicio);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("DELETE FROM EJERCICIOS WHERE id_ejercicio = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}