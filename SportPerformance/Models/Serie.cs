namespace SportPerformance.Models
{
    public class Serie
    {
        public int Id { get; set; } // id_serie [cite: 361]
        public int IdEjercicio { get; set; } // FK a Ejercicio [cite: 362]
        public int IdEntrenamiento { get; set; } // FK a Entrenamiento [cite: 363]
        public double Peso { get; set; } // peso [cite: 364]
        public int Repeticiones { get; set; } // repeticiones [cite: 365]
    }
}