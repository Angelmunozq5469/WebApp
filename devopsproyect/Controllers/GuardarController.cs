using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using devopsproyect.Models;
using devopsproyect.Data;

namespace devopsproyect.Controllers
{
    public class GuardarController : Controller
 {






    static string cadena = "Server=tcp:localhost,1433;Initial Catalog=DB_ACCESO;Persist Security Info=False;User ID=sa;Password=Uces6150#;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;"
;
    private readonly ApplicationDbContext _context;

    public GuardarController(ApplicationDbContext context)
    {
        _context = context;
    }


    //GET: Guardar
    public ActionResult Guardar()
    {

        return View();
    }

    //SET: Guardar

    [HttpPost]

    public ActionResult Guardar(Agendamiento cita )
    {

        var token = new GenerateController().GenerateToken();
        bool Guardado;
        string Mensaje;


        if(string.IsNullOrEmpty(cita.Correo))
        {
            ViewData["Mensaje"] = "Verifica el correo.";
    
        }
        else
        {

        }

        using (SqlConnection cn = new SqlConnection (cadena)){
            SqlCommand cmd = new SqlCommand ("sp_AgendamientoGuardar",cn);
            cmd.Parameters.AddWithValue("Nombre", cita.Nombre);
            cmd.Parameters.AddWithValue("Correo", cita.Correo);
            cmd.Parameters.AddWithValue("Numero", cita.Numero);
            cmd.Parameters.AddWithValue("Fecha", cita.Fecha);
            cmd.Parameters.AddWithValue("Horario", cita.Horario);
            cmd.Parameters.AddWithValue("Especialidad",cita.Especialidad);
            cmd.Parameters.AddWithValue("@Emailconfirmacion", false);
            cmd.Parameters.AddWithValue("@TokenUnique", token);
            cmd.Parameters.Add("Guardado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            cmd.ExecuteNonQuery();
            Guardado = Convert.ToBoolean(cmd.Parameters["Guardado"].Value);
            Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
        }
        ViewData["Mensaje"] = Mensaje;
        if (Guardado) {
            cita.TokenUnique = (string)token;
            var emailController = new EmailController(_context);
            emailController.SendEmail(cita.Correo, cita.Fecha, cita.Horario, cita.TokenUnique, cita.Especialidad);
            return RedirectToAction("Index","Home");

        }
        else {
            return View();
        }

    
    }
    }
}


