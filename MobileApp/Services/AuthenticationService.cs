using MobileApp.Models;

namespace MobileApp.Services
{
    public class AuthenticationService
    {
        private readonly List<Usuario> _usuarios;
        private readonly Random _random;

        public AuthenticationService()
        {
            _random = new Random();
            _usuarios = new List<Usuario>
            {
                new Usuario 
                { 
                    Id = 1, 
                    Login = "joao.silva", 
                    Senha = "123456", 
                    Nome = "João Silva", 
                    Tipo = TipoUsuario.Cuidador,
                    DataCriacao = DateTime.Now.AddMonths(-6)
                },
                new Usuario 
                { 
                    Id = 2, 
                    Login = "maria.santos", 
                    Senha = "123456", 
                    Nome = "Maria Santos", 
                    Tipo = TipoUsuario.Paciente,
                    DataCriacao = DateTime.Now.AddMonths(-3)
                },
                new Usuario 
                { 
                    Id = 3, 
                    Login = "ana.costa", 
                    Senha = "123456", 
                    Nome = "Ana Costa", 
                    Tipo = TipoUsuario.Cuidador,
                    DataCriacao = DateTime.Now.AddMonths(-4)
                },
                new Usuario 
                { 
                    Id = 4, 
                    Login = "carlos.oliveira", 
                    Senha = "123456", 
                    Nome = "Carlos Oliveira", 
                    Tipo = TipoUsuario.Paciente,
                    DataCriacao = DateTime.Now.AddMonths(-2)
                }
            };
        }

        public async Task<LoginResponse> AutenticarAsync(string login, string senha)
        {
            // Simular delay de rede
            await Task.Delay(1000);

            var usuario = _usuarios.FirstOrDefault(u => 
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) && 
                u.Senha == senha && 
                u.Ativo);

            if (usuario == null)
            {
                return new LoginResponse
                {
                    Sucesso = false,
                    Mensagem = "Login ou senha inválidos."
                };
            }

            // Gerar PIN aleatório de 3 dígitos
            var pin = _random.Next(100, 1000).ToString();

            // Atualizar último login
            usuario.UltimoLogin = DateTime.Now;

            return new LoginResponse
            {
                Sucesso = true,
                Mensagem = "Login realizado com sucesso!",
                Usuario = usuario,
                PinAutenticacao = pin
            };
        }

        public bool ValidarPin(string pinInformado, string pinCorreto)
        {
            return pinInformado == pinCorreto;
        }

        public async Task SalvarUsuarioLogadoAsync(Usuario usuario)
        {
            await SecureStorage.SetAsync("usuario_id", usuario.Id.ToString());
            await SecureStorage.SetAsync("usuario_login", usuario.Login);
            await SecureStorage.SetAsync("usuario_nome", usuario.Nome);
            await SecureStorage.SetAsync("usuario_tipo", ((int)usuario.Tipo).ToString());
            await SecureStorage.SetAsync("mantenha_conectado", "true");
        }

        public async Task<Usuario?> ObterUsuarioSalvoAsync()
        {
            try
            {
                var mantenhaConectado = await SecureStorage.GetAsync("mantenha_conectado");
                if (mantenhaConectado != "true")
                    return null;

                var usuarioId = await SecureStorage.GetAsync("usuario_id");
                var usuarioLogin = await SecureStorage.GetAsync("usuario_login");
                var usuarioNome = await SecureStorage.GetAsync("usuario_nome");
                var usuarioTipo = await SecureStorage.GetAsync("usuario_tipo");

                if (string.IsNullOrEmpty(usuarioId) || string.IsNullOrEmpty(usuarioLogin))
                    return null;

                return new Usuario
                {
                    Id = int.Parse(usuarioId),
                    Login = usuarioLogin,
                    Nome = usuarioNome ?? string.Empty,
                    Tipo = (TipoUsuario)int.Parse(usuarioTipo ?? "0")
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task LimparUsuarioSalvoAsync()
        {
            SecureStorage.RemoveAll();
        }

        public async Task DesabilitarMantenhaConectadoAsync()
        {
            await SecureStorage.SetAsync("mantenha_conectado", "false");
        }
    }
}
