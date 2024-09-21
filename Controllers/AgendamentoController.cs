using DentalPlus.Connection;
using DentalPlus.Models;
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
                ViewBag.ListarPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

                var medico = new MedicoModel().ListarTodosMedicos();
                ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

                var consulta = new TipoConsultaModel().ListarTodosTiposConsultas();
                ViewBag.ListaTipoConsultas = new SelectList(consulta, "IdMedicalConsultation", "descMedicalConsultation");

                return View(agendamento);
            }

        }
    }
}
