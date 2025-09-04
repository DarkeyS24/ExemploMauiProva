# Ponte Dourada - Aplicativo Móvel

## Sistema de Atendimento para Cuidadores e Pacientes

### 🔐 Funcionalidades de do dia Clicado

Com o atendimento criar os scampos PossivelCancelar e PossivelAvaliar com o valor data

        public bool PodeSerCancelado => DataAtendimento > DateTime.Now && Status == "Agendado";
        public bool PodeSerAvaliado => DataAtendimento < DateTime.Now && Status == "Realizado" && Avaliacao == null;

Para apresentar o valor da nota dependera de um conversor que calcule se a avaliçao é nula no campo IsVisible

 <!-- Avaliação (se existir) -->
 <StackLayout Grid.Row="1" Grid.Column="0" Grid.ColumnSpan="2" 
            IsVisible="{Binding Avaliacao, Converter={StaticResource IsNotNullConverter}}"
            Orientation="Horizontal" 
            Spacing="10" 
            Margin="0,10,0,0">
     <Label Text="⭐" FontSize="16" />
     <Label Text="{Binding Avaliacao.Nota}" FontSize="14" FontAttributes="Bold" />
     <Label Text="{Binding Avaliacao.Comentario}" FontSize="12" TextColor="#6C757D" />
 </StackLayout>

<!-- Criar o calendario utilizando combobox Plan B -->

        private readonly HttpClient http;
        private List<Atendimento> ateds;
        
        public MainPage(HttpClient http)
        {
            InitializeComponent();
            this.http = http;
            SetCombo();
        }
        
        private async void SetCombo()
        {
            var response = await http.GetAsync("Atendimentos");
            if (response.IsSuccessStatusCode)
            {
        var content = await response.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize < List < Atendimento>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

        if (list.Any())
        {
            ateds = new List<Atendimento>(list.Where(l => l.DataHora.Year == DateTime.Now.Year).ToList());  
            pickerTxt.ItemsSource = list.Where(l => l.DataHora.Year == DateTime.Now.Year).Select(l => l.DataHora.ToString("dd/MM/yyyy")).ToList();
            pickerTxt.SelectedIndex = 0;
        }
    }
        }

        private void IndexChanged(object sender, EventArgs e)
        {
            tipoData.Text = ateds[pickerTxt.SelectedIndex].DataHora < DateTime.Now ? "Passada": ateds[pickerTxt.SelectedIndex].DataHora == DateTime.Now ?  "Atual" : "Futura";
            paciente.Text = ateds[pickerTxt.SelectedIndex].Idoso.IdNavigation.Nome;
            tipo.Text = ateds[pickerTxt.SelectedIndex].Procedimento.Nome;
            profissional.Text = ateds[pickerTxt.SelectedIndex].Cuidador.IdNavigation.Nome;
        }

        
<!-- Class converter-->
          public class IsNotNullConverter : IValueConverter
         {
             public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
             {
                 return value != null;
             }

     public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
     {
         throw new NotImplementedException();
     }
 }

#### **Login com Dupla Autenticação**
1. **Primeira Camada**: Login e senha do usuário
2. **Segunda Camada**: PIN aleatório de 3 dígitos gerado automaticamente

#### **Funcionalidade "Mantenha-me Conectado"**
- Permite que o usuário permaneça logado mesmo após fechar o aplicativo
- Os dados são armazenados de forma segura no dispositivo
- Na próxima abertura do app, o usuário é automaticamente direcionado para sua tela inicial

### 🔧 Como Reverter a Funcionalidade "Mantenha-me Conectado"

Existem **3 formas** de desabilitar ou reverter a funcionalidade de login automático:

#### **Método 1: Através do Botão de Configurações (Recomendado)**
1. Faça login no aplicativo normalmente
2. Na tela principal (Calendário ou Meus Atendimentos), clique no ícone **⚙️** no canto superior direito
3. Confirme a ação quando perguntado se deseja desabilitar a funcionalidade
4. A partir da próxima abertura do app, será necessário fazer login novamente

#### **Método 2: Não Marcar a Opção na Próxima Vez**
1. Ao fazer login novamente, simplesmente **não marque** a caixa "Mantenha-me conectado"
2. Isso substituirá a configuração anterior

### 🎯 Fluxo de Navegação

#### **Para Cuidadores**
Login → PIN → Calendário de Atendimentos

#### **Para Pacientes**
Login → PIN → Meus Atendimentos

### 🛡️ Segurança

- As senhas são validadas através de um serviço de autenticação
- O PIN de dupla autenticação é gerado aleatoriamente a cada login
- Os dados de "mantenha-me conectado" são armazenados usando `Preferences.Default` do .NET MAUI
- Possibilidade de logout e limpeza de dados salvos a qualquer momento
