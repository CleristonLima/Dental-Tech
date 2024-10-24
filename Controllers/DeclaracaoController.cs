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

        public DeclaracaoController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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

                /*if (id != null)
                {

                    receita = new ReceituarioModel().RetornarPaciente(id);

                }*/

                var paciente = new PacienteModel().ListarTodosPacientes();
                ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

                var medico = new MedicoModel().ListarTodosMedicos();
                ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

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
                    horas.ConsultarNomeMedico();
                    horas.ConsultarNomePaciente();

                    string nomeArquivo = $"{horas.NamePatient}_DeclaracaoHoras.pdf";

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

        [HttpGet]
        public IActionResult DeclaracaoDias(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Prontuario");
            }
            else
            {
                DeclaracaoDiasModel dias = new DeclaracaoDiasModel();

                /*if (id != null)
                {

                    receita = new ReceituarioModel().RetornarPaciente(id);

                }*/

                var paciente = new PacienteModel().ListarTodosPacientes();
                ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

                var medico = new MedicoModel().ListarTodosMedicos();
                ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

                return View(dias);

            }

        }

        [HttpPost]
        public IActionResult DeclaracaoDias(DeclaracaoDiasModel dias)
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
                    dias.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                    dias.GravarDias();
                    dias.ConsultarNomeMedico();
                    dias.ConsultarNomePaciente();

                    string nomeArquivo = $"{dias.NamePatient}_DeclaracaoDias.pdf";

                    return new ViewAsPdf("DeclaracaoDiasPDF", dias)
                    {
                        FileName = nomeArquivo
                    };
                }
            }
        }
    }
}
