using DentalPlus.Connection;
using DentalPlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

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

        // Prontuario
        public IActionResult ProntuarioInicial()
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
        public IActionResult ProntuarioInicial(int? id)
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
                                        
                }

                var paciente = new PacienteModel().ListarTodosPacientes();
                ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

                var medico = new MedicoModel().ListarTodosMedicos();
                ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

                return View(receita);

            }

        }

        // Vai receber os dados do paciente
        [HttpPost]
        public IActionResult ProntuarioInicial(ReceituarioModel receita)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Prontuario");
            }
            else
            {
                receita.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                receita.Gravar();
                receita.ConsultarNomePaciente();
                receita.ConsultarNomeMedico();

                string nomeArquivo = $"{receita.NamePatient}_Prontuario.pdf";

                return new ViewAsPdf("ProntuarioPDF", receita)
                {
                    FileName = nomeArquivo
                };
            }
        }

        /*public IActionResult ProntuarioPDF(ReceituarioModel receita)
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
        }*/

        public IActionResult Receita(string medicoId, string pacienteId)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Prontuario");
            }

            var receita = new ReceituarioModel
            {
                IdDoctor = medicoId,
                IdPatients = pacienteId
            };

            // Consultar o nome do médico e do paciente
            receita.ConsultarNomeMedico();
            receita.ConsultarNomePaciente();

            receita.ConsultarRemedio();

            // Retornar os dados de produtos disponíveis para a receita
            var produtos = new ProdutoReceitaModel().ListarTodosRemedios(); // Exemplo de função para listar produtos
            ViewBag.ProdutosReceita = new SelectList(produtos, "IdProductRevenue", "NameProduct");

            var paciente = new PacienteModel().ListarTodosPacientes();
            ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

            var medico = new MedicoModel().ListarTodosMedicos();
            ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

            return View(receita);
        }

        [HttpGet]
        public IActionResult ReceitaMedica(int? id)
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

                }

                var medicamentos = new ProdutoReceitaModel().ListarTodosRemedios();
                ViewBag.ListaMedicamentos = new SelectList(medicamentos, "IdProductRevenue", "NameProduct");

                return View(receita);

            }

        }

        [HttpPost]
        public IActionResult ReceitaMedica(ReceituarioModel receita)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Prontuario");
            }

            // Salvar a receita no banco de dados
            receita.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
            receita.GravarRemedios();

            // Redirecionar após o salvamento
            return RedirectToAction("Index", "Prontuario");
        }
    }
   
}
