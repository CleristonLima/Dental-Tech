using DentalPlus.Connection;
using DentalPlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalPlus.Controllers
{
    public class PacienteController : Controller
    {
        private InternetConnection internetConnection = new InternetConnection();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        public PacienteController(IHttpContextAccessor httpContextAccessor)
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
                ViewBag.ListaPacientes = new PacienteModel().ListarTodosPacientes();

                var aniversariantes = new PacienteModel().ListarTodosPacientesAniversariantes();
                ViewBag.PacientesAniversariantes = aniversariantes;

                return View();
            }
        }

        [HttpGet]
        public IActionResult CadastroPaciente(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Paciente");
            }
            else
            {
                PacienteModel paciente = new PacienteModel();

                if (id != null)
                {
                    
                    paciente = new PacienteModel().RetornarPaciente(id);
                }

                var plano = new PlanoSaudeModel().ListarTodosPlanos();
                ViewBag.ListaPlanos = new SelectList(plano, "IdPlan", "PlanoComCobertura");

                var estado = new EstadoModel().ListarTodosEstados();
                ViewBag.ListaEstado = new SelectList(estado, "IdUf", "codeUf");

                return View(paciente);
            }

        }

        // Vai receber os dados do paciente
        [HttpPost]
        public IActionResult CadastroPaciente(PacienteModel paciente)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Paciente");
            }
            else
            {
                try
                {
                    paciente.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                    paciente.Gravar();
                    return RedirectToAction("Index", "Paciente");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CPF_CNPJ", ex.Message);
                    return View(paciente);
                }
            }
        }

        public IActionResult Excluir(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Paciente");
            }
            else
            {
                ViewData["IdPatients"] = id;
                return View();
            }
        }

        public IActionResult ExcluirPaciente(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Paciente");
            }
            else
            {
                new PacienteModel().Excluir(id);
                return View();
            }
        }

        //

        [HttpGet]
        public IActionResult Aniversariantes()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                ViewBag.ListaPacienteAniversariantes = new PacienteModel().ListarTodosPacientesAniversariantes();
                return View();
            }
        }
    }
}
