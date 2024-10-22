using DentalPlus.Connection;
using DentalPlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

namespace DentalPlus.Controllers
{
    public class DeclaracaoController : Controller
    {
        private InternetConnection internetConnection = new InternetConnection();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        // Processo para gerar a declaração de horas
        public IActionResult DeclaracaoHoras()
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
        public IActionResult DeclaracaoHoras(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Prontuario");
            }
            else
            {
                DeclaracaoHorasModel horas = new DeclaracaoHorasModel();

                var paciente = new PacienteModel().ListarTodosPacientes();
                ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

                return View(horas);

            }

        }

        [HttpPost]
        public IActionResult DeclaracaoHoras(DeclaracaoHorasModel horas)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Prontuario");
            }
            else
            {
                if (!VerificarConexaoInternet())
                {
                    TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                    return RedirectToAction("Index", "Prontuario");
                }
                else
                {
                    horas.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                    horas.GravarHoras();
                    horas.ConsultarNomePaciente();

                    string nomeArquivo = $"{horas.NamePatient}_Prontuario.pdf";

                    return new ViewAsPdf("DeclaracaoHorasPDF", horas)
                    {
                        FileName = nomeArquivo
                    };
                }
            }
        }

        // Processo para gerar a declaração de dias
        public IActionResult DeclaracaoDias()
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
    }
}
