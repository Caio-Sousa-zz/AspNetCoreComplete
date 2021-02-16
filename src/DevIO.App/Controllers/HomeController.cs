using DevIO.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DevIO.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelError = new ErrorViewModel();

            switch (id)
            {
                case 500:
                    modelError.Message = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso supporte.";
                    modelError.Title = "Ocorreu um erro";
                    break;
                case 404:
                    modelError.Message = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso supporte.";
                    modelError.Title = "Ops! Página não encontrada";
                    break;
                case 403:
                    modelError.Message = "Você não tem permissão para fazer isto.";
                    modelError.Title = "Acesso Negado";
                    break;
                default:
                    return StatusCode(404);
            }

            modelError.RequestId = id;

            return View(modelError);
        }
    }
}
