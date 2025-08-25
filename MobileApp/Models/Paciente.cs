namespace MobileApp.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string NomeCompleto => $"{Nome} {Sobrenome}";
    }
}
