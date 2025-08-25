namespace MobileApp.Models
{
    public class Atendimento
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataAtendimento { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public string Status { get; set; } = "Agendado"; // "Agendado", "Realizado", "Cancelado"
    }
}
