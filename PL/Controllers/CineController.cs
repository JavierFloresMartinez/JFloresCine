using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();
            ML.Result result = new ML.Result();
            result = BL.Cine.GetAll();
            if (result.Correct)
            {
                cine.Cines = result.Objects;
            }
            return View(cine);
        }
    }
}
