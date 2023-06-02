using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    public class ZonaController : Controller
    {
        [HttpGet]
        [Route("Api/Zona/GetAll")]
        public IActionResult GetAll()
        {

            ML.Result result = BL.Zona.GetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}
