using DentalPlus.Connection;
using DentalPlus.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentalPlus.Controllers
{
    public class PlanoSaudeController : Controller
    {
        private InternetConnection internetConnection = new InternetConnection();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        public PlanoSaudeController(IHttpContextAccessor httpContextAccessor)
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
                ViewBag.ListaPlanos = new PlanoSaudeModel().ListarTodosPlanos();
                return View();
            }
        }

        [HttpGet]
        public IActionResult CadastroPlanoSaude(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "PlanoSaude");
            }
            else
            {
                PlanoSaudeModel plano = new PlanoSaudeModel();

                if (id != null)
                {
                    //Carregar o registro do cliente numa ViewBag
                    plano = new PlanoSaudeModel().RetornarPlano(id);
                }

                return View(plano);
            }

        }

        // Vai receber os dados do medico
        [HttpPost]
        public IActionResult CadastroPlanoSaude(PlanoSaudeModel plano)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "PlanoSaude");
            }
            else
            {
                    plano.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                    plano.Gravar();
                    return RedirectToAction("Index", "PlanoSaude");
            }
        }

        public IActionResult Excluir(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "PlanoSaude");
            }
            else
            {
                ViewData["IdPlan"] = id;
                return View();
            }
        }

        public IActionResult ExcluirPlanoSaude(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "PlanoSaude");
            }
            else
            {
                new PlanoSaudeModel().Excluir(id);
                return View();
            }
        }
    }
}
