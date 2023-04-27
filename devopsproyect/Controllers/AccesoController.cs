using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using devopsproyect.Models;

namespace devopsproyect.Controllers
{
    public class AccesoController : Controller
 {

    static string cadena = "Server=tcp:localhost,1433;Initial Catalog=DB_ACCESO;Persist Security Info=False;User ID=sa;Password=Uces6150#;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";
 

    //GET: Acceso
    public ActionResult Login()
    {
        return View();
    }

    public ActionResult Registrar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Registrar(Usuario oUsuario)
    {
        bool registrado;
        string mensaje;

        if (oUsuario.Clave == oUsuario.Confirmarclave)
        {

        }
        else {
            ViewData["Mensaje"] = "Las contraseñas no coinciden";
            return View();

        }

        using (SqlConnection cn = new SqlConnection(cadena)){
            SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario",cn);
            cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
            cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
            cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            cmd.ExecuteNonQuery();
            registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();
        }
        ViewData["Mensaje"] = mensaje;
        if (registrado) {
            return RedirectToAction("Login","Acceso");
        }
        else {
            return View();
        }

    }

    [HttpPost]
    public ActionResult Login(Usuario oUsuario)
    {
        if(string.IsNullOrEmpty(oUsuario.Correo) || string.IsNullOrEmpty(oUsuario.Clave))
        {
            ViewData["Mensaje"] = "Ingrese el correo y la contraseña";
            return View();
        }
        using (SqlConnection cn = new SqlConnection(cadena))
        
        {
            SqlCommand cmd = new SqlCommand ("sp_ValidarUsuario",cn);
            cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
            cmd.Parameters.AddWithValue("Clave",oUsuario.Clave);
            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();

            oUsuario.IdUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());

        }
        

        if (oUsuario.IdUsuario != 0){

            HttpContext.Session.SetString("usuario", oUsuario.Correo);
            return RedirectToAction("Index","Home");
        }
        else {

            ViewData["Mensaje"] = "Usuario no encontrado";
            return View();
        }
        
        }

    }
 }


