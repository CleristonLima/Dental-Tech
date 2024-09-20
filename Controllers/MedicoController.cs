using DentalPlus.Connection;
using DentalPlus.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentalPlus.Controllers
{
    public class MedicoController : Controller
    {
        private InternetConnection internetConnection = new InternetConnection();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        public MedicoController(IHttpContextAccessor httpContextAccessor)
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
                ViewBag.ListaMedicos = new MedicoModel().ListarTodosMedicos();
                return View();
            }
        }

        [HttpGet]
        public IActionResult CadastroMedico(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Medico");
            }
            else
            {
                MedicoModel medico = new MedicoModel();

                if (id != null)
                {
                    
                    medico = new MedicoModel().RetornarMedico(id);
                }

                return View(medico);
            }

        }

        // Vai receber os dados do medico
        [HttpPost]
        public IActionResult CadastroMedico(MedicoModel medico)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Medico");
            }
            else
            {
                try
                {
                    medico.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                    medico.Gravar();
                    return RedirectToAction("Index", "Medico");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CRM_CRO", ex.Message);
                    return View(medico);
                }
            }
        }

        public IActionResult Excluir(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Medico");
            }
            else
            {
                ViewData["IdDoctor"] = id;
                return View();
            }
        }

        public IActionResult ExcluirMedico(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Medico");
            }
            else
            {
                new MedicoModel().Excluir(id);
                return View();
            }
        }
    }
}
