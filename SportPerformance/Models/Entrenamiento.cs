using System;

namespace SportPerformance.Models
{
    public class Entrenamiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Comentario { get; set; }
    }
}