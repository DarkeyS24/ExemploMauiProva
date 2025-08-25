namespace MobileApp.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Resumo { get; set; } = string.Empty;
        public string FotoUrl { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        
        public string NomeCompleto => $"{Nome} {Sobrenome}";
        public int Idade => DateTime.Now.Year - DataNascimento.Year - (DateTime.Now.DayOfYear < DataNascimento.DayOfYear ? 1 : 0);
        public string DataNascimentoFormatada => DataNascimento.ToString("dd/MM/yyyy");
    }
}
