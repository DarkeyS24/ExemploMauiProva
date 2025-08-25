namespace MobileApp.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public int AtendimentoId { get; set; }
        public int Nota { get; set; } // 1 a 5 estrelas
        public string Comentario { get; set; } = string.Empty;
        public DateTime DataAvaliacao { get; set; }
    }
}
