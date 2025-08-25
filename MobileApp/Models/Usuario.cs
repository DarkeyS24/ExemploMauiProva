namespace MobileApp.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public TipoUsuario Tipo { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? UltimoLogin { get; set; }
    }

    public enum TipoUsuario
    {
        Paciente,
        Cuidador
    }
}
