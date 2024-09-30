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

        // Para Cadastro de remedios da clinica
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
                ViewBag.ProdutosEstoqueBaixo = new ProdutosEstoqueModel().VerificarEstoqueBaixo();
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
                try
                {
                    produtosUsar.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                    produtosUsar.SubtrairEstoque();
                    return RedirectToAction("ListaProdutosEstoque", "ProdutosEstoque");
                }
                catch (Exception ex)
                {
                    TempData["ErrorEstoque"] = ex.Message;
                    return RedirectToAction("UsarProduto", new { id = produtosUsar.IdProduct });
                }
            }
        }

        [HttpGet]
        public IActionResult AbastecerProduto(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                ProdutosEstoqueModel produtoAbastecer = new ProdutosEstoqueModel();

                if (id != null)
                {

                    produtoAbastecer = new ProdutosEstoqueModel().RetornarProdutosParaAbastecer(id);
                }

                return View(produtoAbastecer);
            }

        }

        [HttpPost]
        public IActionResult AbastecerProduto(ProdutosEstoqueModel produtosAbastecer)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                produtosAbastecer.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                produtosAbastecer.AbastecerEstoque();
                return RedirectToAction("ListaProdutosEstoque", "ProdutosEstoque");
            }
        }

        public IActionResult ExcluirProdutosOdonto(int id)
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

        public IActionResult ExcluirProdutoEstoque(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                new ProdutosEstoqueModel().ExcluirProdutosEstoque(id);
                return View();
            }
        }

        /*-----------------------------------------------------------------------*/

        // Para Cadastro de remedio em receitas

        [HttpGet]
        public IActionResult ListaRemediosPacientes()
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                ViewBag.ListaRemedios = new ProdutoReceitaModel().ListarTodosRemedios();
                return View();
            }
        }


        [HttpGet]
        public IActionResult CadastroRemedio(int? id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                ProdutoReceitaModel produtoRemedio = new ProdutoReceitaModel();

                if (id != null)
                {

                    produtoRemedio = new ProdutoReceitaModel().RetornarRemedios(id);
                }

                return View(produtoRemedio);
            }

        }

        [HttpPost]
        public IActionResult CadastroRemedio(ProdutoReceitaModel produtosRemedio)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                produtosRemedio.userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuarioLogado");
                produtosRemedio.Gravar();
                return RedirectToAction("ListaRemediosPacientes", "ProdutosEstoque");
            }
        }

        public IActionResult ExcluirProdutoPaciente(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                ViewData["IdProductRevenue"] = id;
                return View();
            }
        }

        public IActionResult ExcluirRemedio(int id)
        {
            if (!VerificarConexaoInternet())
            {
                TempData["ErrorLogin"] = "Sem conexão com a internet. Verifique sua rede e tente novamente.";
                return RedirectToAction("Index", "ProdutosEstoque");
            }
            else
            {
                new ProdutoReceitaModel().ExcluirRemedios(id);
                return View();
            }
        }
    }
}
