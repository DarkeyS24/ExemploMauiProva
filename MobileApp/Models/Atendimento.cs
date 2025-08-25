namespace MobileApp.Models
{
    public class Atendimento
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int CuidadorId { get; set; }
        public int ProcedimentoId { get; set; }
        public DateTime DataAtendimento { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public string Status { get; set; } = "Agendado"; // "Agendado", "Realizado", "Cancelado"
        
        // Propriedades navegacionais
        public Paciente? Paciente { get; set; }
        public Usuario? Cuidador { get; set; }
        public Procedimento? Procedimento { get; set; }
        public Avaliacao? Avaliacao { get; set; }
        
        // Propriedades computadas
        public string HorarioFormatado => $"{HorarioInicio:hh\\:mm} - {HorarioFim:hh\\:mm}";
        public bool PodeSerCancelado => DataAtendimento > DateTime.Now && Status == "Agendado";
        public bool PodeSerAvaliado => DataAtendimento < DateTime.Now && Status == "Realizado" && Avaliacao == null;
    }
}
