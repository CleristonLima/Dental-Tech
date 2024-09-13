using DentalPlus.Models;
using DentalPlus.Uteis;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DentalPlus.Connection;

namespace DentalPlus.Controllers
{
    public class HomeController : Controller
    {
        private InternetConnection internetConnection = new InternetConnection();

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        public IActionResult Menu()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return View();
            }

            // Para realizar o logout
            if (id != null && id == 0)
            {
                HttpContext.Session.SetString("IdUsuarioLogado", string.Empty);
                HttpContext.Session.SetString("NomeUsuarioLogado", string.Empty);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return View();
            }


            if (ModelState.IsValid || true)
            {
                bool loginOk = login.ValidarLogin();

                if (loginOk)
                {
                    // Para gravar o login e a senha no sistema
                    HttpContext.Session.SetString("IdUsuarioLogado", login.Id);
                    HttpContext.Session.SetString("NomeUsuarioLogado", login.NamePeople);
                    return RedirectToAction("Menu", "Home");
                }
                else
                {
                    TempData["ErrorLogin"] = login.ErrorMessage;
                }
            }

            return View();
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
