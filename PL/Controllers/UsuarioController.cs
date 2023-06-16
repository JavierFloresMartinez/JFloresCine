using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Runtime;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private IHostingEnvironment environment;
        private IConfiguration configuration;
        public UsuarioController(IHostingEnvironment _environment, IConfiguration _configuration)
        {
            environment = _environment;
            configuration = _configuration;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            ML.Usuario usuario = new ML.Usuario();
            var bcrip = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
            var passwordHash = bcrip.GetBytes(20);
            ML.Result result = BL.Usuario.UsuarioGetByUsername(username);
            if (result.Correct)
            {
                usuario = (ML.Usuario)result.Object;
                if (usuario.Contrasenia.SequenceEqual(passwordHash))
                {
                    return RedirectToAction("Index", "Home", usuario);
                }
                else
                {
                    ViewBag.Message = "Error contraseña Incorrecta";
                    return View("Modal");
                }

            }
            else
            {
                ViewBag.Message = "Error credenciales Incorrectas";
                return View("Modal");
            }


        }

        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Form(ML.Usuario usuario, string password, string paswrd)
        {
            if (password == paswrd)
            {
                var bcrip = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
                var passwordHash = bcrip.GetBytes(20);
                usuario.Contrasenia = passwordHash;
                ML.Result result = BL.Usuario.Add(usuario);
                if (result.Correct)
                {
                    ViewBag.Message = "Se ingreso Correctamente";
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error" + result.ErrorMessage;
                }
            }
            else
            {
                ViewBag.Message = "Contraseñas No Iguales";
            }

            return View("Modal");
        }


        [HttpGet]
        public IActionResult CambiarPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambiarPassword(string email)
        {

            ML.Result result = BL.Usuario.GetByCorreoElectronico(email);
            if (result.Correct)
            {
                string emailOrigen = configuration["EmailOrigen"];
                string passwordOrigen = configuration["PasswordOrigen"];
                string contenidoHTML = System.IO.File.ReadAllText(Path.Combine(environment.ContentRootPath, "wwwroot", "Template", "TemplateEmail.html"));
                string urlNuevoPassword = configuration["Url"];
                result = BL.Usuario.EnviarEmail(email, emailOrigen, passwordOrigen, contenidoHTML, urlNuevoPassword);
                if (result.Correct)
                {
                    ViewBag.Modal = "show";
                    ViewBag.Message  = "Se ha enviado un correo de confirmación al correo electronico " + email;
                }
            }
            else
            {
                ViewBag.Modal = "show";
                ViewBag.Message = "Correo no valido Ingrese correo electronico valido";
            }

            return View("Modal");
        }


        [HttpGet]
        public ActionResult NuevoPassword(string email)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.CorreoElectronico = email;
            
            return View(usuario);
        }
        [HttpPost]
        public ActionResult NuevoPassword(string email, string password, string psword)
        {
            if (password == psword)
            {
                var bcrip = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
                var passwordHash = bcrip.GetBytes(20);
                ML.Usuario usuario = new ML.Usuario();
                usuario.Contrasenia = passwordHash;
                usuario.CorreoElectronico = email;
                ML.Result result = BL.Usuario.UpdatePassword(usuario);
                if (result.Correct)
                {
                    ViewBag.Modal = "show";
                    ViewBag.Message = "Se actualizo correctamente la contraseña";
                }
                else
                {
                    ViewBag.Modal = "show";
                    ViewBag.Messsage = "Ocurrio un error " + result.ErrorMessage;
                }
                return View("Modal");
            }
            else
            {
                ViewBag.Modal = "show";
                ViewBag.Message = "Contraseñas Incorrectas";
                return View("ModalRecuperarPassword");
                
            }
           
        }
    }
}
