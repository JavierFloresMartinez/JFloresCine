using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            ML.Usuario usuario = new ML.Usuario();
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Login(ML.Usuario usuario)
        {
           
            return View("Modal");
        }

        [HttpGet]
        public IActionResult Form()
        {
            ML.Usuario usuario = new ML.Usuario();

            return View(usuario);
        }

        [HttpPost]
        public IActionResult Form(ML.Usuario usuario)
        {
            return View("Modal");
        }

    }
}
