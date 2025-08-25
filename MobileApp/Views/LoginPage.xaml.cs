using MobileApp.Models;
using MobileApp.Services;

namespace MobileApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly AuthenticationService _authService;
        private LoginResponse? _loginResponse;

        public LoginPage()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            
            // Verificar se há usuário salvo ao inicializar
            _ = VerificarUsuarioSalvoAsync();
        }

        private async Task VerificarUsuarioSalvoAsync()
        {
            try
            {
                var usuarioSalvo = await _authService.ObterUsuarioSalvoAsync();
                if (usuarioSalvo != null)
                {
                    // Usuário mantido conectado, navegar direto para a tela principal
                    await NavegarParaTelaInicialAsync(usuarioSalvo);
                }
            }
            catch
            {
                // Em caso de erro, limpar dados salvos e continuar normal
                await _authService.LimparUsuarioSalvoAsync();
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var login = EntryLogin.Text?.Trim();
            var senha = EntrySenha.Text?.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
            {
                await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
                return;
            }

            // Mostrar loading
            BtnLogin.IsEnabled = false;
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            try
            {
                _loginResponse = await _authService.AutenticarAsync(login, senha);

                if (_loginResponse.Sucesso)
                {
                    // Login bem-sucedido, mostrar formulário de PIN
                    MostrarFormularioPin();
                }
                else
                {
                    await DisplayAlert("Erro de Autenticação", _loginResponse.Mensagem, "OK");
                }
            }
            catch
            {
                await DisplayAlert("Erro", "Ocorreu um erro durante o login. Tente novamente.", "OK");
            }
            finally
            {
                // Esconder loading
                BtnLogin.IsEnabled = true;
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
            }
        }

        private void MostrarFormularioPin()
        {
            if (_loginResponse?.Usuario == null) return;

            // Esconder formulário de login
            LoginForm.IsVisible = false;
            
            // Mostrar formulário de PIN
            PinForm.IsVisible = true;
            LblPinGerado.Text = _loginResponse.PinAutenticacao;
            
            // Limpar campo PIN
            EntryPin.Text = string.Empty;
            
            // Focar no campo PIN
            EntryPin.Focus();
        }

        private void OnVoltarPinClicked(object sender, EventArgs e)
        {
            // Voltar para formulário de login
            PinForm.IsVisible = false;
            LoginForm.IsVisible = true;
            
            // Limpar dados do login anterior
            _loginResponse = null;
        }

        private async void OnConfirmarPinClicked(object sender, EventArgs e)
        {
            var pinInformado = EntryPin.Text?.Trim();

            if (string.IsNullOrWhiteSpace(pinInformado))
            {
                await DisplayAlert("Erro", "Por favor, digite o PIN.", "OK");
                return;
            }

            if (_loginResponse?.Usuario == null)
            {
                await DisplayAlert("Erro", "Erro interno. Faça login novamente.", "OK");
                OnVoltarPinClicked(sender, e);
                return;
            }

            if (_authService.ValidarPin(pinInformado, _loginResponse.PinAutenticacao))
            {
                // PIN correto, verificar se deve salvar usuário
                if (CheckBoxMantenhaConectado.IsChecked)
                {
                    await _authService.SalvarUsuarioLogadoAsync(_loginResponse.Usuario);
                }

                // Navegar para tela principal
                await NavegarParaTelaInicialAsync(_loginResponse.Usuario);
            }
            else
            {
                await DisplayAlert("Erro", "PIN incorreto. Tente novamente.", "OK");
                EntryPin.Text = string.Empty;
                EntryPin.Focus();
            }
        }

        private async Task NavegarParaTelaInicialAsync(Usuario usuario)
        {
            try
            {
                // Navegar de acordo com o tipo de usuário
                string rota = usuario.Tipo switch
                {
                    TipoUsuario.Cuidador => "//CalendarioAtendimentos",
                    TipoUsuario.Paciente => "//MeusAtendimentos", // Será criada posteriormente
                    _ => "//MainPage"
                };

                await Shell.Current.GoToAsync(rota);
            }
            catch
            {
                // Se a rota não existir, ir para a página principal
                await Shell.Current.GoToAsync("//MainPage");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Limpar formulários ao aparecer
            EntryLogin.Text = string.Empty;
            EntrySenha.Text = string.Empty;
            EntryPin.Text = string.Empty;
            CheckBoxMantenhaConectado.IsChecked = false;
            
            // Garantir que o formulário de login está visível
            LoginForm.IsVisible = true;
            PinForm.IsVisible = false;
            
            // Limpar dados anteriores
            _loginResponse = null;
        }
    }
}
