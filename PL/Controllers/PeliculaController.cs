using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class PeliculaController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Peliculas resultWebApi = new ML.Peliculas();
            ML.Peliculas pelicula = new ML.Peliculas();
            resultWebApi.Objects = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");

                var responseTask = client.GetAsync("movie/popular?api_key=7341951ed79858b47785c5602bc19061&language=en-US&page=2");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsAsync<ML.Peliculas>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.results)
                    {
                        ML.Peliculas resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Peliculas>(resultItem.ToString());
                        resultWebApi.Objects.Add(resultItemList);
                    }
                }
                pelicula.PeliculasList = resultWebApi.Objects;
            }
            return View(pelicula);
        }

        [HttpGet]
        public IActionResult GetFavoritos()
        {
            ML.Peliculas resultWebApi = new ML.Peliculas();
            ML.Peliculas pelicula = new ML.Peliculas();
            resultWebApi.Objects = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");

                var responseTask = client.GetAsync("account/19728393/favorite/movies?api_key=7341951ed79858b47785c5602bc19061&language=en-US&page=1&session_id=deeac8d1e4766d5b07b29c74d3d3c1745b6464d5&sort_by=created_at.asc");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsAsync<ML.Peliculas>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.results)
                    {
                        ML.Peliculas resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Peliculas>(resultItem.ToString());
                        resultWebApi.Objects.Add(resultItemList);
                    }
                }
                pelicula.PeliculasList = resultWebApi.Objects;
            }
            return View(pelicula);
        }

        [HttpGet]
        public ActionResult Add(int idPelicula)
        {
            if (idPelicula != null)
            {
                ML.Favorite favorite = new ML.Favorite();
                favorite.media_id = idPelicula;
                favorite.media_type = "movie";
                favorite.favorite = true;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");

                    var postTask = client.PostAsJsonAsync<ML.Favorite>("account/19728393/favorite?session_id=deeac8d1e4766d5b07b29c74d3d3c1745b6464d5&api_key=7341951ed79858b47785c5602bc19061", favorite);
                    postTask.Wait();

                    var resultUsuario = postTask.Result;
                    if (resultUsuario.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se agrego correctamente a la lista de favoritos";


                    }
                    else
                    {
                        ViewBag.Message = "Ocurrio un error al insertar el registro";
                    }

                }
            }

            return View("Modal");
        }

        [HttpGet]
        public ActionResult Delete(int idPelicula)
        {
            if (idPelicula != null)
            {
                ML.Favorite favorite = new ML.Favorite();
                favorite.media_id = idPelicula;
                favorite.media_type = "movie";
                favorite.favorite = false;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");

                    var postTask = client.PostAsJsonAsync<ML.Favorite>("account/19728393/favorite?session_id=deeac8d1e4766d5b07b29c74d3d3c1745b6464d5&api_key=7341951ed79858b47785c5602bc19061", favorite);
                    postTask.Wait();

                    var resultUsuario = postTask.Result;
                    if (resultUsuario.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se Elimino correctamente de la lista de favoritos";


                    }
                    else
                    {
                        ViewBag.Message = "Ocurrio un error al Eliminar:";
                    }

                }
            }

            return View("ModalFavoritos");
        }
    }
}
