using DentalPlus.Connection;
using DentalPlus.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentalPlus.Controllers
{
    public class ConfiguracoesController : Controller
    {
        private InternetConnection internetConnection = new InternetConnection();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        public ConfiguracoesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Processo de cadastro de perfil

        public IActionResult Menu()
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

        [HttpGet]
        public IActionResult Perfil()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return View();
            }
            else
            {
                ViewBag.ListaPerfis = new PerfilModel().ListarTodosPerfis();
                return View();
            }
        }

        [HttpGet]
        public IActionResult CadastroPerfil(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Perfil", "Configuracoes");
            }
            else
            {
                PerfilModel perfil = new PerfilModel();

                if (id != null)
                {
                    //Carregar o registro do cliente numa ViewBag
                    perfil = new PerfilModel().RetornarPerfil(id);
                }

                return View(perfil);
            }

        }

        // Vai receber os dados de perfil
        [HttpPost]
        public IActionResult CadastroPerfil(PerfilModel perfil)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Perfil", "Configuracoes");
            }
            else
            {
                perfil.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                perfil.Gravar();
                return RedirectToAction("Perfil");
            }
        }

        public IActionResult Excluir(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Perfil", "Configuracoes");
            }
            else
            {
                ViewData["IdProfile"] = id;
                return View();
            }
        }

        public IActionResult ExcluirPerfil(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Perfil", "Configuracoes");
            }
            else
            {
                new PerfilModel().Excluir(id);
                return View();
            }
        }

        // Na parte abaixo e para cadastro de logins


        public IActionResult Acesso()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return View();
            }
            else
            {
                ViewBag.ListaAcessos = new AcessoModel().ListarTodosAcessos();
                return View();
            }
        }
    }
}
