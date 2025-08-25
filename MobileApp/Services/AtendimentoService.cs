using MobileApp.Models;

namespace MobileApp.Services
{
    public class AtendimentoService
    {
        private readonly List<Paciente> _pacientes;
        private readonly List<Atendimento> _atendimentos;

        public AtendimentoService()
        {
            _pacientes = new List<Paciente>
            {
                new Paciente { Id = 1, Nome = "Maria", Sobrenome = "Silva" },
                new Paciente { Id = 2, Nome = "João", Sobrenome = "Santos" },
                new Paciente { Id = 3, Nome = "Ana", Sobrenome = "Costa" },
                new Paciente { Id = 4, Nome = "Carlos", Sobrenome = "Oliveira" }
            };

            _atendimentos = new List<Atendimento>
            {
                // Atendimentos de exemplo
                new Atendimento { Id = 1, PacienteId = 1, DataAtendimento = DateTime.Today.AddDays(-5), Status = "Realizado" },
                new Atendimento { Id = 2, PacienteId = 1, DataAtendimento = DateTime.Today.AddDays(-2), Status = "Realizado" },
                new Atendimento { Id = 3, PacienteId = 1, DataAtendimento = DateTime.Today, Status = "Agendado" },
                new Atendimento { Id = 4, PacienteId = 1, DataAtendimento = DateTime.Today.AddDays(3), Status = "Agendado" },
                new Atendimento { Id = 5, PacienteId = 1, DataAtendimento = DateTime.Today.AddDays(7), Status = "Agendado" },
                
                new Atendimento { Id = 6, PacienteId = 2, DataAtendimento = DateTime.Today.AddDays(-3), Status = "Realizado" },
                new Atendimento { Id = 7, PacienteId = 2, DataAtendimento = DateTime.Today.AddDays(1), Status = "Agendado" },
                new Atendimento { Id = 8, PacienteId = 2, DataAtendimento = DateTime.Today.AddDays(5), Status = "Agendado" },
                
                new Atendimento { Id = 9, PacienteId = 3, DataAtendimento = DateTime.Today.AddDays(-1), Status = "Realizado" },
                new Atendimento { Id = 10, PacienteId = 3, DataAtendimento = DateTime.Today.AddDays(2), Status = "Agendado" },
                
                new Atendimento { Id = 11, PacienteId = 4, DataAtendimento = DateTime.Today.AddDays(-4), Status = "Realizado" },
                new Atendimento { Id = 12, PacienteId = 4, DataAtendimento = DateTime.Today.AddDays(4), Status = "Agendado" },
                new Atendimento { Id = 13, PacienteId = 4, DataAtendimento = DateTime.Today.AddDays(10), Status = "Agendado" }
            };
        }

        public List<Paciente> ObterPacientes()
        {
            return _pacientes;
        }

        public List<Atendimento> ObterAtendimentosPorPaciente(int pacienteId)
        {
            return _atendimentos.Where(a => a.PacienteId == pacienteId).ToList();
        }

        public List<Atendimento> ObterAtendimentosPorMes(int pacienteId, DateTime mes)
        {
            var inicioMes = new DateTime(mes.Year, mes.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            return _atendimentos
                .Where(a => a.PacienteId == pacienteId && 
                           a.DataAtendimento >= inicioMes && 
                           a.DataAtendimento <= fimMes)
                .ToList();
        }
    }
}
