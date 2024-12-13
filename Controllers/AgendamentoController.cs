﻿using DentalPlus.Connection;
using DentalPlus.Models;
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

        public IActionResult FinalizarAgendamento(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Agendamento");
            }
            else
            {
                ViewData["IdMedConsXPat"] = id;
                return View();
            }
        }

        public IActionResult FinalizarAgendamentoSucesso(AgendamentoModel agendamento, int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Agendamento");
            }
            else
            {
                agendamento.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                agendamento.GravarConsultaRealizada(id);
                return View();
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
        public IActionResult RemarcarAgendamento(int? id)
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
                    agendamento = new AgendamentoModel().RemarcarAgendamento(id);

                    // Preenche o ViewBag com os nomes baseados nos IDs para exibição somente leitura
                    ViewBag.NomePaciente = new PacienteModel().ObterNomePorId(agendamento.IdPatients);
                    ViewBag.NomeMedico = new MedicoModel().ObterNomePorId(agendamento.IdDoctor);
                }

                return View(agendamento);
            }

        }

        [HttpPost]
        public IActionResult RemarcarAgendamento(AgendamentoModel agendamento)
        {

            // Recarrega as listas de Médicos, Pacientes e Consultas para que fiquem disponíveis na View novamente
            var paciente = new PacienteModel().ListarTodosPacientes();
            ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

            var medico = new MedicoModel().ListarTodosMedicos();
            ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

            var consulta = new TipoConsultaModel().ListarTodosTiposConsultas();
            ViewBag.ListaTipoConsultas = new SelectList(consulta, "IdMedicalConsultation", "descMedicalConsultation");

            agendamento.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
            agendamento.GravarRemarcacao();
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

        // Metodo para enviar mensagem

        public IActionResult RedirectToWhatsApp(string phoneNumber, string mensagem)
        {
            string whatsappUrl = $"https://wa.me/{phoneNumber}?text={Uri.EscapeDataString(mensagem)}";
            ViewBag.WhatsAppUrl = whatsappUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnviarMensagem(AgendamentoModel agendamento)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Agendamento");
            }

            // 55 -> DDI do Brasil
            string phoneNumber = "55" + new string(agendamento.PhoneNumber1.Where(char.IsDigit).ToArray());
            ViewBag.NomePaciente = new PacienteModel().ObterNomePorId(agendamento.IdPatients);

            string dataHoraConsulta = new AgendamentoModel().ObterDataeHoraPorId(agendamento.IdMedConsXPat);

            if (string.IsNullOrEmpty(dataHoraConsulta))
            {
                TempData["ErrorMessage"] = "Não é possível enviar a mensagem pois o paciente não está com a consulta com o status de 'Agendado' ou a data do agendamento é anterior a hoje."; //new AgendamentoModel().ErrorMessage;
                return RedirectToAction("EnviarMensagem", new { id = agendamento.IdMedConsXPat });
            }

            if (DateTime.TryParse(dataHoraConsulta, out DateTime dateConsultationStart))
            {
                ViewBag.DataConsulta = dateConsultationStart.ToString("dd/MM/yyyy");
                ViewBag.HorarioConsulta = dateConsultationStart.ToString("HH:mm");
            }

            string mensagem = $"Olá, {ViewBag.NomePaciente}, tudo bem?\n\nPassando para lembrar que a sua consulta está agendada para o dia {ViewBag.DataConsulta} às {ViewBag.HorarioConsulta}.\n\nVocê confirma a sua presença?";

            if (agendamento.MessageType == "WhatsApp")
            {
                agendamento.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                await agendamento.GravarMensagemEnviadaAsync();

                return RedirectToAction("RedirectToWhatsApp", new { phoneNumber, mensagem });
                  
            }

            return View(agendamento);
        }

        public IActionResult ConfirmarAgendamento(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Agendamento");
            }
            else
            {
                ViewData["IdMedConsXPat"] = id;
                return View();
            }
        }

        public IActionResult ConfirmarAgendamentoSucesso(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Agendamento");
            }
            else
            {
                var agendamento = new AgendamentoModel();

                agendamento.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                
                agendamento.AgendamentoConfirmado(id);

                if (!string.IsNullOrEmpty(agendamento.ErrorMessage))
                {
                    TempData["ErrorMessage"] = agendamento.ErrorMessage;
                    return RedirectToAction("ConfirmarAgendamento", "Agendamento");
                }

                return View();
            }
        }
    }

}
