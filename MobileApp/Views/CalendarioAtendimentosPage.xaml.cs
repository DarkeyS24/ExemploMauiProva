using MobileApp.Models;
using MobileApp.Services;
using System.Collections.ObjectModel;

namespace MobileApp.Views
{
    public partial class CalendarioAtendimentosPage : ContentPage
    {
        private readonly AtendimentoService _atendimentoService;
        private readonly AuthenticationService _authService;
        private DateTime _mesAtual;
        private ObservableCollection<Paciente> _pacientes;
        private Paciente? _pacienteSelecionado;
        private List<Atendimento> _atendimentosDoMes;

        public ObservableCollection<Paciente> Pacientes
        {
            get => _pacientes;
            set
            {
                _pacientes = value;
                OnPropertyChanged();
            }
        }

        public Paciente? PacienteSelecionado
        {
            get => _pacienteSelecionado;
            set
            {
                _pacienteSelecionado = value;
                OnPropertyChanged();
                AtualizarCalendario();
            }
        }

        public CalendarioAtendimentosPage()
        {
            InitializeComponent();
            _atendimentoService = new AtendimentoService();
            _authService = new AuthenticationService();
            _mesAtual = DateTime.Now;
            _pacientes = new ObservableCollection<Paciente>();
            _atendimentosDoMes = new List<Atendimento>();

            BindingContext = this;
            CarregarDados();
        }

        private void CarregarDados()
        {
            var pacientes = _atendimentoService.ObterPacientes();
            Pacientes.Clear();
            foreach (var paciente in pacientes)
            {
                Pacientes.Add(paciente);
            }

            if (Pacientes.Any())
            {
                PacienteSelecionado = Pacientes.First();
                PickerPaciente.SelectedItem = PacienteSelecionado;
            }

            AtualizarMesAno();
            AtualizarCalendario();
        }

        private void AtualizarMesAno()
        {
            LblMesAno.Text = _mesAtual.ToString("MMMM yyyy");
        }

        private void AtualizarCalendario()
        {
            if (PacienteSelecionado == null) return;

            _atendimentosDoMes = _atendimentoService.ObterAtendimentosPorMes(PacienteSelecionado.Id, _mesAtual);
            CriarCalendario();
        }

        private void CriarCalendario()
        {
            GridCalendario.Children.Clear();
            GridCalendario.RowDefinitions.Clear();
            GridCalendario.ColumnDefinitions.Clear();

            // Configurar colunas (7 dias da semana)
            for (int i = 0; i < 7; i++)
            {
                GridCalendario.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            var diasDoMes = ObterDiasDoCalendario();
            var totalLinhas = (int)Math.Ceiling(diasDoMes.Count / 7.0);

            // Configurar linhas
            for (int i = 0; i < totalLinhas; i++)
            {
                GridCalendario.RowDefinitions.Add(new RowDefinition { Height = 60 });
            }

            // Preencher o calendário
            for (int i = 0; i < diasDoMes.Count; i++)
            {
                var dia = diasDoMes[i];
                var linha = i / 7;
                var coluna = i % 7;

                var frame = CriarFrameDia(dia);
                Grid.SetRow(frame, linha);
                Grid.SetColumn(frame, coluna);
                GridCalendario.Children.Add(frame);
            }
        }

        private Frame CriarFrameDia(DiaCalendario dia)
        {
            var frame = new Frame
            {
                BackgroundColor = dia.CorFundo,
                BorderColor = Colors.LightGray,
                CornerRadius = 5,
                Padding = 5,
                Margin = 2,
                HasShadow = false
            };

            var label = new Label
            {
                Text = dia.Numero,
                TextColor = dia.CorTexto,
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            if (dia.TemAtendimento)
            {
                label.FontAttributes = FontAttributes.Bold;
            }

            frame.Content = label;
            
            // Adicionar gesture recognizer para clique
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (sender, e) => await OnDiaClicado(dia.Data);
            frame.GestureRecognizers.Add(tapGesture);
            
            return frame;
        }

        private async Task OnDiaClicado(DateTime data)
        {
            var modal = new PacientesDoDiaModal(data);
            await Navigation.PushModalAsync(modal);
        }

        private List<DiaCalendario> ObterDiasDoCalendario()
        {
            var dias = new List<DiaCalendario>();
            var primeiroDiaDoMes = new DateTime(_mesAtual.Year, _mesAtual.Month, 1);
            var ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddDays(-1);

            // Calcular o primeiro dia a ser exibido (início da semana)
            var diaSemanaInicio = (int)primeiroDiaDoMes.DayOfWeek;
            var ajusteDiaInicio = diaSemanaInicio == 0 ? 6 : diaSemanaInicio - 1; // Ajustar para começar na segunda
            var dataInicio = primeiroDiaDoMes.AddDays(-ajusteDiaInicio);

            // Calcular o último dia a ser exibido (fim da semana)
            var diaSemanaFim = (int)ultimoDiaDoMes.DayOfWeek;
            var ajusteDiaFim = diaSemanaFim == 0 ? 0 : 7 - diaSemanaFim;
            var dataFim = ultimoDiaDoMes.AddDays(ajusteDiaFim);

            var dataAtual = DateTime.Today;

            for (var data = dataInicio; data <= dataFim; data = data.AddDays(1))
            {
                var ehDoMesAtual = data.Month == _mesAtual.Month && data.Year == _mesAtual.Year;
                var temAtendimento = _atendimentosDoMes.Any(a => a.DataAtendimento.Date == data.Date);

                var tipoData = data.Date < dataAtual ? TipoData.Passada :
                              data.Date == dataAtual ? TipoData.Atual :
                              TipoData.Futura;

                var corTexto = ehDoMesAtual ? Colors.Black : Colors.Gray;
                var corFundo = Colors.Transparent;

                if (temAtendimento)
                {
                    corFundo = tipoData switch
                    {
                        TipoData.Passada => Color.FromArgb("#4CAF50"), // Verde
                        TipoData.Atual => Color.FromArgb("#FF9800"),   // Laranja
                        TipoData.Futura => Color.FromArgb("#2196F3"),  // Azul
                        _ => Colors.Transparent
                    };
                    corTexto = Colors.White;
                }

                dias.Add(new DiaCalendario
                {
                    Data = data,
                    EhDoMesAtual = ehDoMesAtual,
                    TemAtendimento = temAtendimento,
                    TipoData = tipoData,
                    CorFundo = corFundo,
                    CorTexto = corTexto
                });
            }

            return dias;
        }

        private async void OnVoltarClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Login");
        }

        private async void OnDesconectarClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Desconectar", 
                "Deseja desabilitar a funcionalidade 'mantenha-me conectado'?", 
                "Sim", "Cancelar");

            if (result)
            {
                await _authService.DesabilitarMantenhaConectadoAsync();
                await DisplayAlert("Sucesso", "Funcionalidade desabilitada. Na próxima vez será necessário fazer login novamente.", "OK");
            }
        }

        private void OnMesAnteriorClicked(object sender, EventArgs e)
        {
            _mesAtual = _mesAtual.AddMonths(-1);
            AtualizarMesAno();
            AtualizarCalendario();
        }

        private void OnProximoMesClicked(object sender, EventArgs e)
        {
            _mesAtual = _mesAtual.AddMonths(1);
            AtualizarMesAno();
            AtualizarCalendario();
        }
    }
}
