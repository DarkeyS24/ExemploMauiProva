using MobileApp.Models;
using MobileApp.Services;
using System.Collections.ObjectModel;

namespace MobileApp.Views
{
    public partial class PacientesDoDiaModal : ContentPage
    {
        private readonly AtendimentoService _atendimentoService;
        private DateTime _dataSelecionada;

        public ObservableCollection<Atendimento> AtendimentosDoDia { get; set; }

        public PacientesDoDiaModal(DateTime dataSelecionada)
        {
            InitializeComponent();
            _atendimentoService = new AtendimentoService();
            _dataSelecionada = dataSelecionada;
            AtendimentosDoDia = new ObservableCollection<Atendimento>();
            
            BindingContext = this;
            CarregarAtendimentosDoDia();
        }

        private void CarregarAtendimentosDoDia()
        {
            LblTitulo.Text = $"Pacientes - {_dataSelecionada:dd/MM/yyyy}";
            
            var atendimentos = _atendimentoService.ObterAtendimentosPorData(_dataSelecionada);
            
            AtendimentosDoDia.Clear();
            foreach (var atendimento in atendimentos)
            {
                AtendimentosDoDia.Add(atendimento);
            }

            if (AtendimentosDoDia.Any())
            {
                CarouselPacientes.ItemsSource = AtendimentosDoDia;
                CarouselPacientes.IsVisible = true;
                IndicatorView.IsVisible = true;
                StackSemPacientes.IsVisible = false;
            }
            else
            {
                CarouselPacientes.IsVisible = false;
                IndicatorView.IsVisible = false;
                StackSemPacientes.IsVisible = true;
            }
        }

        private async void OnFecharClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
