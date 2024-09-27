using DentalPlus.Connection;
using DentalPlus.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentalPlus.Controllers
{
    public class ProdutosEstoqueController : Controller
    {
        private InternetConnection internetConnection = new InternetConnection();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool VerificarConexaoInternet()
        {
            return internetConnection.VerificarConexaoInternet();
        }

        public ProdutosEstoqueController(IHttpContextAccessor httpContextAccessor)
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

        [HttpGet]
        public IActionResult ListaProdutosEstoque()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                ViewBag.ListaProdutosEstoque = new ProdutosEstoqueModel().ListarTodosProdutosEstoque();
                return View();
            }
        }

        [HttpGet]
        public IActionResult CadastroProdutosEstoque(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                ProdutosEstoqueModel produtoEstoque = new ProdutosEstoqueModel();

                if (id != null)
                {

                    produtoEstoque = new ProdutosEstoqueModel().RetornarProdutosEstoque(id);
                }

                return View(produtoEstoque);
            }

        }

        [HttpPost]
        public IActionResult CadastroProdutosEstoque(ProdutosEstoqueModel produtosEstoque)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                produtosEstoque.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                produtosEstoque.Gravar();
                return RedirectToAction("ListaProdutosEstoque", "ProdutosEstoque");
            }
        }

        [HttpGet]
        public IActionResult UsarProduto(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                ProdutosEstoqueModel produtoUsar = new ProdutosEstoqueModel();

                if (id != null)
                {

                    produtoUsar = new ProdutosEstoqueModel().RetornarProdutosParaUsar(id);
                }

                return View(produtoUsar);
            }

        }

        [HttpPost]
        public IActionResult UsarProduto(ProdutosEstoqueModel produtosUsar)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                produtosUsar.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                produtosUsar.SubtrairEstoque();
                return RedirectToAction("ListaProdutosEstoque", "ProdutosEstoque");
            }
        }

        [HttpPost]
        public IActionResult ExcluirProdutosEstoque(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                ViewData["IdProduct"] = id;
                return View();
            }
        }
    }
}
