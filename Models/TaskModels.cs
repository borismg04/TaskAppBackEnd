namespace Models
{
    public class TaskModels
    {
        public int Id { get; set; }
        public string? Tarea { get; set; }
        public string? Status { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? Fecha { get; set; }
        public string? UserGestion { get; set; }
        public string? Prioridad { get; set; }
    }
}
