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
                    int RowsAfected = contex.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}', '{cine.Direccion}', {cine.Zona.IdZona}, {cine.Venta}");

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
                    int RowsAfected = contex.Database.ExecuteSqlRaw($"CineUpdate {cine.IdCine}, '{cine.Nombre}', '{cine.Direccion}', {cine.Zona.IdZona}, {cine.Venta}");

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
                        cine.Nombre = RowsAfected.Zona;
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




    }

}