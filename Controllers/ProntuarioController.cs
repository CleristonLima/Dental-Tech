using DentalPlus.Connection;
using DentalPlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalPlus.Controllers
{
    public class ProntuarioController : Controller
    {

        private InternetConnection internetConnection = new InternetConnection();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        public ProntuarioController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Index()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                return View();
            }
        }

        // Receituario
        public IActionResult Receituario()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Prontuario");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Receituario(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Prontuario");
            }
            else
            {
                ReceituarioModel receita = new ReceituarioModel();

                if (id != null)
                {

                    //receita = new ReceituarioModel().RetornarPaciente(id);
                    var paciente = new PacienteModel().ListarTodosPacientes();
                    ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

                    
                }

                return View(receita);

            }

        }

        // Vai receber os dados do paciente
        [HttpPost]
        public IActionResult Receituario(ReceituarioModel receita)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Prontuario");
            }
            else
            {
                try
                {
                    receita.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                   // receita.Gravar();
                    return RedirectToAction("Receituario", "Prontuario");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CPF_CNPJ", ex.Message);
                    return View(receita);
                }
            }
        }
    }
   
}
