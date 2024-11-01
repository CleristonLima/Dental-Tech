using DentalPlus.Connection;
using DentalPlus.Models;
using DocumentFormat.OpenXml.EMMA;
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
            /*if (!ModelState.IsValid)
            {
                // Retorna a view com os erros de validação padrão
                return View(agendamento);
            }*/

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

                    var paciente = new PacienteModel().ListarTodosPacientes();
                    ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

                    var medico = new MedicoModel().ListarTodosMedicos();
                    ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

                    var consulta = new TipoConsultaModel().ListarTodosTiposConsultas();
                    ViewBag.ListaTipoConsultas = new SelectList(consulta, "IdMedicalConsultation", "descMedicalConsultation");
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
    }
}
