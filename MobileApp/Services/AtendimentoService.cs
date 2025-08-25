using MobileApp.Models;

namespace MobileApp.Services
{
    public class AtendimentoService
    {
        private readonly List<Paciente> _pacientes;
        private readonly List<Atendimento> _atendimentos;
        private readonly List<Procedimento> _procedimentos;
        private readonly List<Usuario> _cuidadores;
        private readonly List<Avaliacao> _avaliacoes;

        public AtendimentoService()
        {
            _procedimentos = new List<Procedimento>
            {
                new Procedimento { Id = 1, Nome = "Cardiovascular", Descricao = "Exame cardiovascular", Duracao = TimeSpan.FromHours(1) },
                new Procedimento { Id = 2, Nome = "Fisioterapia", Descricao = "Sessão de fisioterapia", Duracao = TimeSpan.FromMinutes(50) },
                new Procedimento { Id = 3, Nome = "Consulta Geral", Descricao = "Consulta médica geral", Duracao = TimeSpan.FromMinutes(30) },
                new Procedimento { Id = 4, Nome = "Medicação", Descricao = "Administração de medicamentos", Duracao = TimeSpan.FromMinutes(15) }
            };

            _cuidadores = new List<Usuario>
            {
                new Usuario { Id = 1, Nome = "Dr. João Silva", Tipo = TipoUsuario.Cuidador },
                new Usuario { Id = 3, Nome = "Dra. Ana Costa", Tipo = TipoUsuario.Cuidador }
            };

            _pacientes = new List<Paciente>
            {
                new Paciente 
                { 
                    Id = 1, 
                    Nome = "Maria", 
                    Sobrenome = "Silva",
                    DataNascimento = new DateTime(1950, 5, 15),
                    Resumo = "Paciente com histórico cardiovascular. Requer cuidados especiais.",
                    FotoUrl = "👵",
                    Telefone = "(11) 99999-1111"
                },
                new Paciente 
                { 
                    Id = 2, 
                    Nome = "João", 
                    Sobrenome = "Santos",
                    DataNascimento = new DateTime(1945, 8, 22),
                    Resumo = "Paciente em recuperação pós-cirúrgica. Fisioterapia regular.",
                    FotoUrl = "👴",
                    Telefone = "(11) 99999-2222"
                },
                new Paciente 
                { 
                    Id = 3, 
                    Nome = "Ana", 
                    Sobrenome = "Costa",
                    DataNascimento = new DateTime(1960, 12, 3),
                    Resumo = "Diabetes controlada. Monitoramento regular necessário.",
                    FotoUrl = "👩",
                    Telefone = "(11) 99999-3333"
                },
                new Paciente 
                { 
                    Id = 4, 
                    Nome = "Carlos", 
                    Sobrenome = "Oliveira",
                    DataNascimento = new DateTime(1955, 3, 10),
                    Resumo = "Hipertensão arterial. Acompanhamento médico constante.",
                    FotoUrl = "👨",
                    Telefone = "(11) 99999-4444"
                }
            };

            _atendimentos = new List<Atendimento>
            {
                // Atendimentos passados
                new Atendimento { Id = 1, PacienteId = 1, CuidadorId = 1, ProcedimentoId = 1, DataAtendimento = DateTime.Today.AddDays(-5), HorarioInicio = new TimeSpan(9, 0, 0), HorarioFim = new TimeSpan(10, 0, 0), Status = "Realizado" },
                new Atendimento { Id = 2, PacienteId = 2, CuidadorId = 3, ProcedimentoId = 2, DataAtendimento = DateTime.Today.AddDays(-3), HorarioInicio = new TimeSpan(14, 0, 0), HorarioFim = new TimeSpan(14, 50, 0), Status = "Realizado" },
                new Atendimento { Id = 3, PacienteId = 3, CuidadorId = 1, ProcedimentoId = 3, DataAtendimento = DateTime.Today.AddDays(-2), HorarioInicio = new TimeSpan(10, 30, 0), HorarioFim = new TimeSpan(11, 0, 0), Status = "Realizado" },
                
                // Atendimento hoje
                new Atendimento { Id = 4, PacienteId = 1, CuidadorId = 1, ProcedimentoId = 1, DataAtendimento = DateTime.Today, HorarioInicio = new TimeSpan(15, 0, 0), HorarioFim = new TimeSpan(16, 0, 0), Status = "Agendado" },
                new Atendimento { Id = 5, PacienteId = 4, CuidadorId = 3, ProcedimentoId = 4, DataAtendimento = DateTime.Today, HorarioInicio = new TimeSpan(8, 0, 0), HorarioFim = new TimeSpan(8, 15, 0), Status = "Agendado" },
                
                // Atendimentos futuros
                new Atendimento { Id = 6, PacienteId = 2, CuidadorId = 1, ProcedimentoId = 2, DataAtendimento = DateTime.Today.AddDays(1), HorarioInicio = new TimeSpan(16, 0, 0), HorarioFim = new TimeSpan(16, 50, 0), Status = "Agendado" },
                new Atendimento { Id = 7, PacienteId = 3, CuidadorId = 3, ProcedimentoId = 3, DataAtendimento = DateTime.Today.AddDays(2), HorarioInicio = new TimeSpan(9, 30, 0), HorarioFim = new TimeSpan(10, 0, 0), Status = "Agendado" },
                new Atendimento { Id = 8, PacienteId = 1, CuidadorId = 1, ProcedimentoId = 1, DataAtendimento = DateTime.Today.AddDays(3), HorarioInicio = new TimeSpan(14, 0, 0), HorarioFim = new TimeSpan(15, 0, 0), Status = "Agendado" },
                new Atendimento { Id = 9, PacienteId = 4, CuidadorId = 3, ProcedimentoId = 4, DataAtendimento = DateTime.Today.AddDays(5), HorarioInicio = new TimeSpan(11, 0, 0), HorarioFim = new TimeSpan(11, 15, 0), Status = "Agendado" },
                new Atendimento { Id = 10, PacienteId = 2, CuidadorId = 1, ProcedimentoId = 2, DataAtendimento = DateTime.Today.AddDays(7), HorarioInicio = new TimeSpan(13, 0, 0), HorarioFim = new TimeSpan(13, 50, 0), Status = "Agendado" }
            };

            _avaliacoes = new List<Avaliacao>
            {
                new Avaliacao { Id = 1, AtendimentoId = 1, Nota = 5, Comentario = "Excelente atendimento!", DataAvaliacao = DateTime.Today.AddDays(-4) },
                new Avaliacao { Id = 2, AtendimentoId = 2, Nota = 4, Comentario = "Muito bom, recomendo.", DataAvaliacao = DateTime.Today.AddDays(-2) }
            };

            // Associar dados relacionados
            AssociarDadosRelacionados();
        }

        private void AssociarDadosRelacionados()
        {
            foreach (var atendimento in _atendimentos)
            {
                atendimento.Paciente = _pacientes.FirstOrDefault(p => p.Id == atendimento.PacienteId);
                atendimento.Cuidador = _cuidadores.FirstOrDefault(c => c.Id == atendimento.CuidadorId);
                atendimento.Procedimento = _procedimentos.FirstOrDefault(p => p.Id == atendimento.ProcedimentoId);
                atendimento.Avaliacao = _avaliacoes.FirstOrDefault(a => a.AtendimentoId == atendimento.Id);
            }
        }

        public List<Paciente> ObterPacientes()
        {
            return _pacientes;
        }

        public List<Procedimento> ObterProcedimentos()
        {
            return _procedimentos;
        }

        public List<Usuario> ObterCuidadores()
        {
            return _cuidadores;
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

        public List<Atendimento> ObterAtendimentosPorData(DateTime data)
        {
            return _atendimentos
                .Where(a => a.DataAtendimento.Date == data.Date)
                .OrderBy(a => a.HorarioInicio)
                .ToList();
        }

        public List<Atendimento> ObterAtendimentosPorDataComFiltros(DateTime data, int? procedimentoId = null, int? cuidadorId = null, int? pacienteId = null)
        {
            var atendimentos = _atendimentos.Where(a => a.DataAtendimento.Date == data.Date);

            if (procedimentoId.HasValue)
                atendimentos = atendimentos.Where(a => a.ProcedimentoId == procedimentoId.Value);

            if (cuidadorId.HasValue)
                atendimentos = atendimentos.Where(a => a.CuidadorId == cuidadorId.Value);

            if (pacienteId.HasValue)
                atendimentos = atendimentos.Where(a => a.PacienteId == pacienteId.Value);

            return atendimentos.OrderBy(a => a.HorarioInicio).ToList();
        }

        public List<Atendimento> ObterAtendimentosFiltrados(int? pacienteId = null, int? procedimentoId = null, int? cuidadorId = null, DateTime? mesAno = null)
        {
            var atendimentos = _atendimentos.AsQueryable();

            if (pacienteId.HasValue)
                atendimentos = atendimentos.Where(a => a.PacienteId == pacienteId.Value);

            if (procedimentoId.HasValue)
                atendimentos = atendimentos.Where(a => a.ProcedimentoId == procedimentoId.Value);

            if (cuidadorId.HasValue)
                atendimentos = atendimentos.Where(a => a.CuidadorId == cuidadorId.Value);

            if (mesAno.HasValue)
            {
                var inicioMes = new DateTime(mesAno.Value.Year, mesAno.Value.Month, 1);
                var fimMes = inicioMes.AddMonths(1).AddDays(-1);
                atendimentos = atendimentos.Where(a => a.DataAtendimento >= inicioMes && a.DataAtendimento <= fimMes);
            }

            return atendimentos.ToList();
        }

        public Task<bool> CancelarAtendimentoAsync(int atendimentoId)
        {
            var atendimento = _atendimentos.FirstOrDefault(a => a.Id == atendimentoId);
            if (atendimento != null && atendimento.PodeSerCancelado)
            {
                atendimento.Status = "Cancelado";
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> AvaliarAtendimentoAsync(int atendimentoId, int nota, string comentario)
        {
            var atendimento = _atendimentos.FirstOrDefault(a => a.Id == atendimentoId);
            if (atendimento != null && atendimento.PodeSerAvaliado)
            {
                var avaliacao = new Avaliacao
                {
                    Id = _avaliacoes.Count + 1,
                    AtendimentoId = atendimentoId,
                    Nota = nota,
                    Comentario = comentario,
                    DataAvaliacao = DateTime.Now
                };
                
                _avaliacoes.Add(avaliacao);
                atendimento.Avaliacao = avaliacao;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
