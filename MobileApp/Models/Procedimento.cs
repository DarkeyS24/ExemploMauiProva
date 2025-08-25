namespace MobileApp.Models
{
    public class Procedimento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public TimeSpan Duracao { get; set; }
    }
}
