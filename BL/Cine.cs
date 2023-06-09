using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Cine
    {
        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JfloresCineContext contex = new DL.JfloresCineContext())
                {
                    int RowsAfected = contex.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}', '{cine.Direccion}', {cine.Zona.IdZona}, {cine.Venta}, {cine.Latitud}, {cine.Longitud}");

                    if (RowsAfected > 0)
                    {
                        result.Correct = true; ;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al ingresar el cine";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }


        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JfloresCineContext contex = new DL.JfloresCineContext())
                {
                    int RowsAfected = contex.Database.ExecuteSqlRaw($"CineUpdate {cine.IdCine}, '{cine.Nombre}', '{cine.Direccion}', {cine.Zona.IdZona}, {cine.Venta}, {cine.Latitud}, {cine.Longitud}");

                    if (RowsAfected > 0)
                    {
                        result.Correct = true; ;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al Actualizar el cine";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result Delete(int idCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JfloresCineContext contex = new DL.JfloresCineContext())
                {
                    int RowsAfected = contex.Database.ExecuteSqlRaw($"CineDelete {idCine}");

                    if (RowsAfected > 0)
                    {
                        result.Correct = true; ;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al Elimar el cine";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JfloresCineContext contex = new DL.JfloresCineContext())
                {
                    var RowsAfected = contex.Cines.FromSqlRaw("CineGetAll").ToList();

                    result.Objects = new List<object>();

                    if (contex != null)
                    {
                        foreach (var obj in RowsAfected)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.IdCine = obj.IdCine;
                            cine.Nombre = obj.Nombre;
                            cine.Direccion = obj.Direccion;
                            cine.Venta = (int)obj.Ventas;
                            cine.Zona = new ML.Zona();
                            cine.Zona.IdZona = (int)obj.IdZona;
                            cine.Zona.Nombre = obj.Zona;
                            cine.Latitud = (float)obj.Latitud;
                            cine.Longitud = (float)obj.Longitud;

                            result.Objects.Add(cine);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron registros.";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result GetById(int idCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JfloresCineContext contex = new DL.JfloresCineContext())
                {
                    var RowsAfected = contex.Cines.FromSqlRaw($"CineGetById {idCine}").AsEnumerable().FirstOrDefault();
                    
                    result.Object = new object();

                    if (RowsAfected != null)
                    {
                        ML.Cine cine = new ML.Cine();
                        cine.IdCine = RowsAfected.IdCine;
                        cine.Nombre = RowsAfected.Nombre;
                        cine.Direccion = RowsAfected.Direccion;
                        cine.Venta = (int)RowsAfected.Ventas;
                        cine.Zona = new ML.Zona();
                        cine.Zona.IdZona = (int)RowsAfected.IdZona;
                        cine.Zona.Nombre = RowsAfected.Zona;
                        cine.Latitud = (float)RowsAfected.Latitud;
                        cine.Longitud = (float)RowsAfected.Longitud;
                        result.Object = cine;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrió un error al obtener el registros en la tabla Cine";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result calcularPorcentaje(ML.Cine cine)
        {
            ML.Cine cineNorte = new ML.Cine();
            ML.Cine cineSur = new ML.Cine();
            ML.Cine cineEste = new ML.Cine();
            ML.Cine cineOeste = new ML.Cine();
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();
            List<object> norteList = new List<object>();
            List<object> surList = new List<object>();
            List<object> esteList = new List<object>();
            List<object> oesteList = new List<object>();
            float promedioNorte = 0;
            float promedioSur = 0;
            float promedioEste = 0;
            float promedioOeste = 0;
            foreach (ML.Cine cinePromedio in cine.Cines)
            {
                if (cinePromedio.Zona.Nombre == "Norte")
                {
                    ML.Cine cineResult = new ML.Cine();
                    cineResult.Zona = new ML.Zona();
                    cineResult.Zona.Nombre = cinePromedio.Zona.Nombre;
                    cineResult.Venta = cinePromedio.Venta;
                    norteList.Add(cineResult);
                }
                if (cinePromedio.Zona.Nombre == "Sur")
                {
                    ML.Cine cineResult = new ML.Cine();
                    cineResult.Zona = new ML.Zona();
                    cineResult.Zona.Nombre = cinePromedio.Zona.Nombre;
                    cineResult.Venta = cinePromedio.Venta;
                    surList.Add(cineResult);
                }
                if (cinePromedio.Zona.Nombre == "Este")
                {
                    ML.Cine cineResult = new ML.Cine();
                    cineResult.Zona = new ML.Zona();
                    cineResult.Zona.Nombre = cinePromedio.Zona.Nombre;
                    cineResult.Venta = cinePromedio.Venta;
                    esteList.Add(cineResult);
                }
                if (cinePromedio.Zona.Nombre == "Oeste")
                {
                    ML.Cine cineResult = new ML.Cine();
                    cineResult.Zona = new ML.Zona();
                    cineResult.Zona.Nombre = cinePromedio.Zona.Nombre;
                    cineResult.Venta = cinePromedio.Venta;
                    oesteList.Add(cineResult);
                }
            }

            foreach (ML.Cine norte in norteList)
            {
                cineNorte.Zona = new ML.Zona();
                promedioNorte = promedioNorte + norte.Venta;
                cineNorte.Zona.Nombre = norte.Zona.Nombre;
            }
            
            promedioNorte = (norteList.Count / promedioNorte) * 100;
            cineNorte.Porcentaje = promedioNorte;
            result.Objects.Add(cineNorte);
            
            foreach (ML.Cine sur in surList)
            {
                cineSur.Zona = new ML.Zona();
                promedioSur = promedioSur + sur.Venta;
                cineSur.Zona.Nombre = sur.Zona.Nombre;
            }
            promedioSur = (surList.Count / promedioSur) * 100;
            cineSur.Porcentaje = promedioSur;
            result.Objects.Add(cineSur);

            foreach (ML.Cine este in esteList)
            {
                cineEste.Zona = new ML.Zona();
                promedioEste = promedioEste + este.Venta;
                cineEste.Zona.Nombre = este.Zona.Nombre;
            }
            promedioEste = (esteList.Count / promedioEste) * 100;
            cineEste.Porcentaje = promedioEste;
            result.Objects.Add(cineEste);

            foreach (ML.Cine oeste in oesteList)
            {
                cineOeste.Zona = new ML.Zona();
                promedioOeste = promedioOeste + oeste.Venta;
                cineOeste.Zona.Nombre = oeste.Zona.Nombre;
            }
            promedioOeste = (oesteList.Count / promedioOeste) * 100;
            cineOeste.Porcentaje = promedioOeste;
            result.Objects.Add(cineOeste);


            return result;
        }


    }

}