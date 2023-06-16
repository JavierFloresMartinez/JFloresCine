using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Net.Mail;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace BL
{
    public class Usuario
    {
        
        public static ML.Result UsuarioGetByUsername(string username)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JfloresCineContext contex = new DL.JfloresCineContext())
                {
                    var RowsAfected = contex.Usuarios.FromSqlRaw($"UsuarioGetByUsername '{username}'").AsEnumerable().FirstOrDefault();

                    result.Object = new object();

                    if (RowsAfected != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = RowsAfected.IdUsuario;
                        usuario.Nombre = RowsAfected.Nombre;
                        usuario.ApellidoPaterno = RowsAfected.ApellidoPaterno;
                        usuario.ApellidoMaterno = RowsAfected.ApellidoMaterno;
                        usuario.Username = RowsAfected.Username;
                        usuario.Contrasenia = RowsAfected.Contrasenia;
                        usuario.CorreoElectronico = RowsAfected.CorreoElectronico;
                        result.Object = usuario;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrió un error al obtener el registros ";
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


        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JfloresCineContext contex = new DL.JfloresCineContext())
                {
                    int RowsAfected = contex.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.Username}', '{usuario.CorreoElectronico}', @Password", new SqlParameter("@Password", usuario.Contrasenia));

                    if (RowsAfected > 0)
                    {
                        result.Correct = true; ;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al ingresar el Nuevo Usuario";
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

        public static ML.Result GetByCorreoElectronico(string email)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JfloresCineContext contex = new DL.JfloresCineContext())
                {
                    var RowsAfected = contex.Usuarios.FromSqlRaw($"UsuarioGetByCorreoElectronico '{email}'").AsEnumerable().FirstOrDefault();

                    result.Object = new object();

                    if (RowsAfected != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = RowsAfected.IdUsuario;
                        usuario.Nombre = RowsAfected.Nombre;
                        usuario.ApellidoPaterno = RowsAfected.ApellidoPaterno;
                        usuario.ApellidoMaterno = RowsAfected.ApellidoMaterno;
                        usuario.Username = RowsAfected.Username;
                        usuario.Contrasenia = RowsAfected.Contrasenia;
                        usuario.CorreoElectronico = RowsAfected.CorreoElectronico;
                        result.Object = usuario;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrió un error al obtener el registros ";
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

        public static ML.Result EnviarEmail(string email, string emailOrigen, string passwordOrigen, string contenidoHTML, string urlNuevoPassword)
        {
            ML.Result result = new ML.Result();
            try
            {
                MailMessage mailMessage = new MailMessage(emailOrigen, email, "Recuperar Contraseña", "<p>Correo para recuperar contraseña</p>");
                mailMessage.IsBodyHtml = true;
                //string contenidoHTML = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Template", "TemplateEmail.html"));
                //string contenidoHTML = System.IO.File.ReadAllText(@"C:\Users\digis\OneDrive\Documents\Javier Flores Martinez\JFloresCine\PL\Views\Usuario\TemplateEmail.html");
                mailMessage.Body = contenidoHTML; 
                string url = urlNuevoPassword + HttpUtility.UrlEncode(email);
                mailMessage.Body = mailMessage.Body.Replace("{url}", url);
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, passwordOrigen);
                
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();

                result.Correct=true;
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }


        public static ML.Result UpdatePassword(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JfloresCineContext contex = new DL.JfloresCineContext())
                {
                    int RowsAfected = contex.Database.ExecuteSqlRaw($"UsuarioUpdatePassword '{usuario.CorreoElectronico}' , @Password", new SqlParameter("@Password", usuario.Contrasenia));

                    if (RowsAfected > 0)
                    {
                        result.Correct = true; ;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al actualizar la nueva contraseña";
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
