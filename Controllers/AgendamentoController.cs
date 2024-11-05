using DentalPlus.Connection;
using DentalPlus.Models;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalPlus.Controllers
{
    public class AgendamentoController : Controller
    {
        private InternetConnection internetConnection = new InternetConnection();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        public AgendamentoController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                ViewBag.ListaAgendamento = new AgendamentoModel().ListarTodosAgendamentos();
                ViewBag.ListaAgendamentoAmanha = new AgendamentoModel().ListarTodasConsultasAmanha();
                return View();
            }
        }

        /* public IActionResult CadastroAgendamento()
         {
             var agendamentoModel = new AgendamentoModel();

             DateTime dataSelecionada = DateTime.Now; // Altere conforme a data selecionada pelo usuário
             string idDoctor = "id_doctor_exemplo"; // ID do médico selecionado

             var horariosOcupados = agendamentoModel.ObterHorariosOcupados(dataSelecionada, idDoctor);

             // Logica para determinar os horarios disponiveis entre os horariosOcupados
             // Crie uma lista de horários disponíveis e passe para a ViewBag

             List<DateTime> horariosDisponiveis = agendamentoModel.ObterHorariosDisponiveis(dataSelecionada, idDoctor);
             ViewBag.HorariosDisponiveis = horariosDisponiveis;

             return View(agendamentoModel);
         }*/

        [HttpGet]
        public IActionResult CadastroAgendamento(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Agendamento");
            }
            else
            {
                AgendamentoModel agendamento = new AgendamentoModel();

                if (id != null)
                {

                    agendamento = new AgendamentoModel().RetornarAgendamento(id);
                }

                var paciente = new PacienteModel().ListarTodosPacientes();
                ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

                var medico = new MedicoModel().ListarTodosMedicos();
                ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

                var consulta = new TipoConsultaModel().ListarTodosTiposConsultas();
                ViewBag.ListaTipoConsultas = new SelectList(consulta, "IdMedicalConsultation", "descMedicalConsultation");

                return View(agendamento);
            }

        }

        [HttpPost]
        public IActionResult CadastroAgendamento(AgendamentoModel agendamento)
        {

            if (string.IsNullOrEmpty(agendamento.IdMedConsXPat) &&
                !agendamento.HorarioDisponivel(agendamento.DateConsultationStart, agendamento.DateConsultationFinish, agendamento.IdDoctor))
            {
                TempData["ErrorHorario"] = agendamento.ErrorMessage;

                // Retorna a View mantendo os valores de Paciente e Médico, mas redefinindo o horário.
                ModelState.Remove("DateConsultationStart");
                ModelState.Remove("DateConsultationFinish");

                // Recarrega as listas de Médicos, Pacientes e Consultas para que fiquem disponíveis na View novamente
                var paciente = new PacienteModel().ListarTodosPacientes();
                ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

                var medico = new MedicoModel().ListarTodosMedicos();
                ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

                var consulta = new TipoConsultaModel().ListarTodosTiposConsultas();
                ViewBag.ListaTipoConsultas = new SelectList(consulta, "IdMedicalConsultation", "descMedicalConsultation");

                return View(agendamento);
            }

            agendamento.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
            agendamento.Gravar();
            return RedirectToAction("Index", "Agendamento");

        }

        [HttpGet]
        public IActionResult CancelamentoAgendamento(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Agendamento");
            }
            else
            {
                AgendamentoModel agendamento = new AgendamentoModel();

                if (id != null)
                {
                    agendamento = new AgendamentoModel().RetornarAgendamentoParaCancelar(id);

                    // Preenche o ViewBag com os nomes baseados nos IDs para exibição somente leitura
                    ViewBag.NomePaciente = new PacienteModel().ObterNomePorId(agendamento.IdPatients);
                    ViewBag.NomeMedico = new MedicoModel().ObterNomePorId(agendamento.IdDoctor);
                    ViewBag.NomeConsulta = new TipoConsultaModel().ObterDescricaoPorId(agendamento.IdMedicalConsultation);
                }

                return View(agendamento);
            }

        }

        [HttpPost]
        public IActionResult CancelamentoAgendamento(AgendamentoModel agendamento)
        {

            // Recarrega as listas de Médicos, Pacientes e Consultas para que fiquem disponíveis na View novamente
            var paciente = new PacienteModel().ListarTodosPacientes();
            ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

            var medico = new MedicoModel().ListarTodosMedicos();
            ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

            var consulta = new TipoConsultaModel().ListarTodosTiposConsultas();
            ViewBag.ListaTipoConsultas = new SelectList(consulta, "IdMedicalConsultation", "descMedicalConsultation");

            agendamento.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
            agendamento.GravarCancelamento();
            return RedirectToAction("Index", "Agendamento");

        }

        [HttpGet]
        public IActionResult ConsultasRealizadas()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                ViewBag.ListaConsultasRealizadas = new AgendamentoModel().ListarTodasConsultasRealizadas();
                return View();
            }
        }

        [HttpGet]
        public IActionResult AgendamentosCancelados()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                ViewBag.ListaConsultasCanceladas = new AgendamentoModel().ListarTodasConsultasCanceladas();
                return View();
            }
        }

        [HttpGet]
        public IActionResult EnviarMensagem(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Agendamento");
            }
            else
            {
                AgendamentoModel agendamento = new AgendamentoModel();

                if (id != null)
                {
                    agendamento = new AgendamentoModel().RetornarPacienteParaEnviarMensagem(id);

                    // Preenche o ViewBag com os nomes baseados nos IDs para exibição somente leitura
                    ViewBag.NomePaciente = new PacienteModel().ObterNomePorId(agendamento.IdPatients);
                    ViewBag.Telefone1 = new PacienteModel().ObterTelefone1Paciente(agendamento.IdPatients);
                    ViewBag.Telefone2 = new PacienteModel().ObterTelefone2Paciente(agendamento.IdPatients);

                    ViewBag.DataConsulta = agendamento.DateConsultationStart.ToString("dd/MM/yyyy");
                    ViewBag.HorarioConsulta = agendamento.DateConsultationStart.ToString("HH:mm");
                }
                return View(agendamento);
            }
        }

        [HttpPost]
        public IActionResult EnviarMensagem(AgendamentoModel agendamento)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Agendamento");
            }

            string phoneNumber = agendamento.PhoneNumber1;
            phoneNumber = "+55" + new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Obter o nome do paciente
            ViewBag.NomePaciente = new PacienteModel().ObterNomePorId(agendamento.IdPatients);

            // Obter data e hora da consulta usando o método ObterDataeHoraPorId
            string dataHoraConsulta = new AgendamentoModel().ObterDataeHoraPorId(agendamento.IdMedConsXPat);

            if (!string.IsNullOrEmpty(dataHoraConsulta) && DateTime.TryParse(dataHoraConsulta, out DateTime dateConsultationStart))
            {
                ViewBag.DataConsulta = dateConsultationStart.ToString("dd/MM/yyyy");
                ViewBag.HorarioConsulta = dateConsultationStart.ToString("HH:mm");
            }

            // Criar mensagem com data e horário
            string mensagem = $"Boa tarde, {ViewBag.NomePaciente}! Sua consulta está agendada para o dia {ViewBag.DataConsulta} às {ViewBag.HorarioConsulta}. Você confirma a sua presença?";

            if (agendamento.StatusConsultation == "WhatsApp")
            {
                // Redireciona para o WhatsApp com a mensagem
                string whatsappUrl = $"https://wa.me/{phoneNumber}?text={Uri.EscapeDataString(mensagem)}";
                return Redirect(whatsappUrl);
            }

            return View(agendamento);
        }
    }

}
