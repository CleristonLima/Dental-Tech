using DentalPlus.Connection;
using DentalPlus.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentalPlus.Controllers
{
    public class TipoConsultaController : Controller
    {
        private InternetConnection internetConnection = new InternetConnection();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        public TipoConsultaController(IHttpContextAccessor httpContextAccessor)
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
                ViewBag.ListaTipoConsultas = new TipoConsultaModel().ListarTodosTiposConsultas();
                return View();
            }
        }

        [HttpGet]
        public IActionResult CadastroTipoConsulta(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "TipoConsulta");
            }
            else
            {
                TipoConsultaModel consulta = new TipoConsultaModel();

                if (id != null)
                {

                    consulta = new TipoConsultaModel().RetornarTipoConsulta(id);
                }

                return View(consulta);
            }

        }

        // Vai receber os dados do medico
        [HttpPost]
        public IActionResult CadastroTipoConsulta(TipoConsultaModel consulta)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "TipoConsulta");
            }
            else
            {
                consulta.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                consulta.Gravar();
                return RedirectToAction("Index", "TipoConsulta");
            }
        }

        public IActionResult Excluir(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "TipoConsulta");
            }
            else
            {
                ViewData["IdMedicalConsultation"] = id;
                return View();
            }
        }

        public IActionResult ExcluirTipoConsulta(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "TipoConsulta");
            }
            else
            {
                new TipoConsultaModel().Excluir(id);
                return View();
            }
        }
    }
}
