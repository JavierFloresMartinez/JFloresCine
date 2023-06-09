using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5181/Api/");

                var responseTask = client.GetAsync("Cine/GetAll");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {

                        ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultItem.ToString());
                        cine.Cines.Add(resultItemList);
                    }
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al hacer la consulta";
                    return View("Modal");
                }

            }
            return View(cine);
        }

        [HttpGet]
        public IActionResult Form(int? idCine)
        {
            ML.Cine cine = new ML.Cine();
           ML.Zona zona = new ML.Zona();
            cine.Zona = new ML.Zona();
            cine.Zona.Zonas = new List<object>();
            zona.Zonas = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5181/Api/");

                var responseTask = client.GetAsync("Zona/GetAll");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {

                        ML.Zona resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Zona>(resultItem.ToString());
                        zona.Zonas.Add(resultItemList);
                    }
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al hacer la consulta";
                    return View("Modal");
                }

            }
            if (idCine == null)
            {
                cine.Zona.Zonas = zona.Zonas;
                return View(cine);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5181/Api/");
                    var responseTask = client.GetAsync("Cine/GetbyId/" + idCine);
                    responseTask.Wait();
                    var resultAPI = responseTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        cine = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(readTask.Result.Object.ToString());
                    }
                    cine.Zona.Zonas = zona.Zonas;
                }
                return View(cine);
            }
        }

        [HttpPost]
        public IActionResult Form(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            if (cine.IdCine == 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5181/Api/");
                    var postTask = client.PostAsJsonAsync<ML.Cine>("Cine/Add", cine);
                    postTask.Wait();

                    var resulUsuario = postTask.Result;
                    if (resulUsuario.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "El Cine se agrego correctamente";

                    }
                    else
                    {
                        ViewBag.Message = "Hubo un error al agregar el Cine" + result.ErrorMessage;
                    }
                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5181/Api/");
                    var postTask = client.PutAsJsonAsync<ML.Cine>("Cine/Update", cine);
                    postTask.Wait();

                    var resultUpdate = postTask.Result;
                    if (resultUpdate.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "El Cine se actualizo correctamente";

                    }
                    else
                    {
                        ViewBag.Message = "Hubo un error al actualizar el Cine" + result.ErrorMessage;
                    }
                }
               
            }
            return View("Modal");
        }

        [HttpGet]
        public ActionResult Delete(int idCine)
        {
            ML.Result resultListProduct = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5181/Api/");


                var postTask = client.GetAsync("Cine/Delete/" + idCine);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Se Elimino con exito";
                    return View("Modal");
                    ;
                }
                else
                {
                    ViewBag.Message = "Error al eliminar";
                    return View("Modal");
                }
            }
        }


        [HttpGet]
        public IActionResult Promedio()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();
            cine.Porcentajes = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5181/Api/");

                var responseTask = client.GetAsync("Cine/GetAll");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {

                        ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultItem.ToString());
                        cine.Cines.Add(resultItemList);
                    }
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al hacer la consulta";
                    return View("Modal");
                }
               
                }

            ML.Result resultPorcentaje = BL.Cine.calcularPorcentaje(cine);
            cine.Porcentajes = resultPorcentaje.Objects;
           

            return View(cine);
        }


      
    }
}
