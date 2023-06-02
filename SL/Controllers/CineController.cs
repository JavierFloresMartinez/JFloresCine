using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    public class CineController : Controller
    {

        [HttpGet]
        [Route("Api/Cine/GetAll")]
        public IActionResult GetAll()
        {
           
            ML.Result result = BL.Cine.GetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

      

        [HttpPost]
        [Route("Api/Cine/Add")]
        public IActionResult Add([FromBody] ML.Cine cine)
        {
            ML.Result result = BL.Cine.Add(cine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.ErrorMessage);
            }
        }

        [HttpGet]
        [Route("Api/Cine/Delete/{idCine}")]
        public IActionResult Delete(int idCine)
        {
           
        
            ML.Result result = BL.Cine.Delete(idCine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.ErrorMessage);
            }
        }

        [HttpGet]
        [Route("Api/Cine/GetbyId/{id}")]
        public IActionResult GetById(int id)
        {
         
            var result = BL.Cine.GetById(id);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPut]
        [Route("api/Cine/update")]
        public IActionResult Put([FromBody] ML.Cine cine)
        {
            var result = BL.Cine.Update(cine);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

   
    }
}
