using MobileApp.Models;
using MobileApp.Services;
using System.Collections.ObjectModel;

namespace MobileApp.Views
{
    public partial class MeusAtendimentosPage : ContentPage
    {
        private readonly AuthenticationService _authService;
        private readonly AtendimentoService _atendimentoService;
        private DateTime _mesAtual;
        private DateTime? _dataSelecionada;
        private int _usuarioLogadoId = 2; // Simulando usuário Maria Santos

        public ObservableCollection<Procedimento> Procedimentos { get; set; }
        public ObservableCollection<Usuario> Cuidadores { get; set; }
        public ObservableCollection<Atendimento> AtendimentosDodia { get; set; }
        
        private Procedimento? _procedimentoSelecionado;
        public Procedimento? ProcedimentoSelecionado
        {
            get => _procedimentoSelecionado;
            set
            {
                _procedimentoSelecionado = value;
                OnPropertyChanged();
                AtualizarCalendario();
            }
        }

        private Usuario? _cuidadorSelecionado;
        public Usuario? CuidadorSelecionado
        {
            get => _cuidadorSelecionado;
            set
            {
                _cuidadorSelecionado = value;
                OnPropertyChanged();
                AtualizarCalendario();
            }
        }

        public MeusAtendimentosPage()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            _atendimentoService = new AtendimentoService();
            _mesAtual = DateTime.Now;
            
            Procedimentos = new ObservableCollection<Procedimento>();
            Cuidadores = new ObservableCollection<Usuario>();
            AtendimentosDodia = new ObservableCollection<Atendimento>();

            BindingContext = this;
            CarregarDados();
        }

        private void CarregarDados()
        {
            // Carregar procedimentos
            Procedimentos.Clear();
            Procedimentos.Add(new Procedimento { Id = 0, Nome = "Todos os Procedimentos" });
            foreach (var proc in _atendimentoService.ObterProcedimentos())
            {
                Procedimentos.Add(proc);
            }

            // Carregar cuidadores
            Cuidadores.Clear();
            Cuidadores.Add(new Usuario { Id = 0, Nome = "Todos os Cuidadores" });
            foreach (var cuidador in _atendimentoService.ObterCuidadores())
            {
                Cuidadores.Add(cuidador);
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
            tapGesture.Tapped += (sender, e) => OnDiaClicado(dia.Data);
            frame.GestureRecognizers.Add(tapGesture);
            
            return frame;
        }

        private void OnDiaClicado(DateTime data)
        {
            _dataSelecionada = data;
            CarregarAtendimentosDoDia(data);
        }

        private void CarregarAtendimentosDoDia(DateTime data)
        {
            var procedimentoId = ProcedimentoSelecionado?.Id == 0 ? null : ProcedimentoSelecionado?.Id;
            var cuidadorId = CuidadorSelecionado?.Id == 0 ? null : CuidadorSelecionado?.Id;
            
            var atendimentos = _atendimentoService.ObterAtendimentosPorDataComFiltros(
                data, procedimentoId, cuidadorId, _usuarioLogadoId);

            AtendimentosDodia.Clear();
            foreach (var atendimento in atendimentos.OrderByDescending(a => a.DataAtendimento).ThenBy(a => a.HorarioInicio))
            {
                AtendimentosDodia.Add(atendimento);
            }

            LblTituloDetalhes.Text = $"Atendimentos do Dia - {data:dd/MM/yyyy}";
            StackDetalhesAtendimentos.IsVisible = true;

            if (AtendimentosDodia.Any())
            {
                CollectionAtendimentos.ItemsSource = AtendimentosDodia;
                CollectionAtendimentos.IsVisible = true;
                StackSemAtendimentos.IsVisible = false;
            }
            else
            {
                CollectionAtendimentos.IsVisible = false;
                StackSemAtendimentos.IsVisible = true;
            }
        }

        private List<DiaCalendario> ObterDiasDoCalendario()
        {
            var dias = new List<DiaCalendario>();
            var primeiroDiaDoMes = new DateTime(_mesAtual.Year, _mesAtual.Month, 1);
            var ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddDays(-1);

            // Calcular o primeiro dia a ser exibido (início da semana)
            var diaSemanaInicio = (int)primeiroDiaDoMes.DayOfWeek;
            var ajusteDiaInicio = diaSemanaInicio == 0 ? 6 : diaSemanaInicio - 1;
            var dataInicio = primeiroDiaDoMes.AddDays(-ajusteDiaInicio);

            // Calcular o último dia a ser exibido (fim da semana)
            var diaSemanaFim = (int)ultimoDiaDoMes.DayOfWeek;
            var ajusteDiaFim = diaSemanaFim == 0 ? 0 : 7 - diaSemanaFim;
            var dataFim = ultimoDiaDoMes.AddDays(ajusteDiaFim);

            var dataAtual = DateTime.Today;
            var procedimentoId = ProcedimentoSelecionado?.Id == 0 ? null : ProcedimentoSelecionado?.Id;
            var cuidadorId = CuidadorSelecionado?.Id == 0 ? null : CuidadorSelecionado?.Id;

            var atendimentosDoMes = _atendimentoService.ObterAtendimentosFiltrados(
                _usuarioLogadoId, procedimentoId, cuidadorId, _mesAtual);

            for (var data = dataInicio; data <= dataFim; data = data.AddDays(1))
            {
                var ehDoMesAtual = data.Month == _mesAtual.Month && data.Year == _mesAtual.Year;
                var temAtendimento = atendimentosDoMes.Any(a => a.DataAtendimento.Date == data.Date);

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

        private async void OnSairClicked(object sender, EventArgs e)
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
            StackDetalhesAtendimentos.IsVisible = false;
        }

        private void OnProximoMesClicked(object sender, EventArgs e)
        {
            _mesAtual = _mesAtual.AddMonths(1);
            AtualizarMesAno();
            AtualizarCalendario();
            StackDetalhesAtendimentos.IsVisible = false;
        }

        private void OnLimparFiltrosClicked(object sender, EventArgs e)
        {
            PickerProcedimento.SelectedIndex = 0;
            PickerCuidador.SelectedIndex = 0;
            StackDetalhesAtendimentos.IsVisible = false;
        }

        private async void OnCancelarAtendimentoClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int atendimentoId)
            {
                var result = await DisplayAlert("Cancelar Atendimento", 
                    "Tem certeza que deseja cancelar este atendimento?", 
                    "Sim", "Não");

                if (result)
                {
                    var sucesso = await _atendimentoService.CancelarAtendimentoAsync(atendimentoId);
                    if (sucesso)
                    {
                        await DisplayAlert("Sucesso", "Atendimento cancelado com sucesso!", "OK");
                        if (_dataSelecionada.HasValue)
                        {
                            CarregarAtendimentosDoDia(_dataSelecionada.Value);
                            AtualizarCalendario();
                        }
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Não foi possível cancelar o atendimento.", "OK");
                    }
                }
            }
        }

        private async void OnAvaliarAtendimentoClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int atendimentoId)
            {
                // Solicitar nota
                var notaResult = await DisplayPromptAsync("Avaliar Atendimento", 
                    "Digite uma nota de 1 a 5 estrelas:", 
                    "OK", "Cancelar", "5", 1, Keyboard.Numeric);

                if (string.IsNullOrEmpty(notaResult) || !int.TryParse(notaResult, out int nota) || nota < 1 || nota > 5)
                {
                    await DisplayAlert("Erro", "Por favor, digite uma nota válida entre 1 e 5.", "OK");
                    return;
                }

                // Solicitar comentário
                var comentario = await DisplayPromptAsync("Avaliar Atendimento", 
                    "Digite um comentário (opcional):", 
                    "OK", "Cancelar", "", -1, Keyboard.Text);

                var sucesso = await _atendimentoService.AvaliarAtendimentoAsync(atendimentoId, nota, comentario ?? "");
                if (sucesso)
                {
                    await DisplayAlert("Sucesso", "Avaliação registrada com sucesso!", "OK");
                    if (_dataSelecionada.HasValue)
                    {
                        CarregarAtendimentosDoDia(_dataSelecionada.Value);
                    }
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível registrar a avaliação.", "OK");
                }
            }
        }
    }
}
