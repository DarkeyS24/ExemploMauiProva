namespace MobileApp.Models
{
    public class LoginResponse
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public Usuario? Usuario { get; set; }
        public string PinAutenticacao { get; set; } = string.Empty;
    }
}
