namespace SportPerformance.Models
{
    public class Serie
    {
        public int Id { get; set; }
        public int IdEjercicio { get; set; }
        public int IdEntrenamiento { get; set; }
        public double Peso { get; set; }
        public int Repeticiones { get; set; }
        public string? EjercicioNombre { get; set; }
        public string? EntrenamientoInfo { get; set; }
    }
}