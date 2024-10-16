﻿using DentalPlus.Connection;
using DentalPlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DentalPlus.Controllers
{
    public class FinanceiroController : Controller
    {
        private InternetConnection internetConnection = new InternetConnection();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        public FinanceiroController(IHttpContextAccessor httpContextAccessor)
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

        public IActionResult Caixa()
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
        public IActionResult Caixa(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Financeiro");
            }
            else
            {
                FinanceiroModel financeiro = new FinanceiroModel();

                var medico = new MedicoModel().ListarTodosMedicos();
                ViewBag.ListaMedicos = new SelectList(medico, "IdDoctor", "NomeComCRM");

                var paciente = new PacienteModel().ListarTodosPacientes();
                ViewBag.ListaPacientes = new SelectList(paciente, "IdPatients", "NomeComCpf");

                var valorConsulta = new TipoConsultaModel().ListarTodosTiposConsultas();
                ViewBag.ListaConsulta = new SelectList(valorConsulta, "IdMedicalConsultation", "descMedicalConsultation");

                var pagamento = new PagamentoModel().ListarTodosPagamentos();
                ViewBag.ListaPagamento = new SelectList(pagamento, "IdPayment", "DescPayment");

                return View(financeiro);
            }

        }

        [HttpPost]
        public IActionResult Caixa(FinanceiroModel financeiro, string VendasJson)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Financeiro");
            }
            else
            {
                // Adiciona o ID do usuário logado
                financeiro.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");

                // Grava a venda principal
                financeiro.Gravar();

                return RedirectToAction("Caixa");

            }
        }

        // Vai pegar o valor das consultas
        public IActionResult GetConsultaPreco(string idConsulta)
        {
            var model = new FinanceiroModel();
            var preco = model.GetConsultaPreco(idConsulta);
            return Json(preco);
        }

        // Contas a pagar

        public IActionResult ContasPagar()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                ViewBag.ListaContas = new ContasPagarModel().ListarTodosContas();
                return View();
            }
        }
        [HttpGet]
        public IActionResult CadastroContas(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Financeiro");
            }
            else
            {
                ContasPagarModel conta = new ContasPagarModel();

                if (id != null)
                {
                    conta = new ContasPagarModel().RetornarConta(id);
                }

                return View(conta);
            }

        }

        // Vai receber os dados do medico
        [HttpPost]
        public IActionResult CadastroContas(ContasPagarModel conta)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Financeiro");
            }
            else
            {
                conta.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                conta.Gravar();
                return RedirectToAction("ContasPagar", "Financeiro");
            }
        }

        public IActionResult Excluir(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Financeiro");
            }
            else
            {
                ViewData["IdAccountPay"] = id;
                return View();
            }
        }

        public IActionResult ExcluirContasPagar(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "Financeiro");
            }
            else
            {
                new ContasPagarModel().Excluir(id);
                return View();
            }
        }

        // Relatórios

        public IActionResult Relatorio()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Menu", "Home");
            }
            
                return View(); 
        }
    }
}
