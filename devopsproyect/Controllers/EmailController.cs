using devopsproyect.Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;



namespace devopsproyect.Controllers
{
    public class EmailController:Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailController( ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpPost]
        public  IActionResult SendEmail(string toEmail, string toFecha, TimeSpan toHorario, string toToken, string toEspecialidad)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("clinicacespradocentro@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject="PROGRAMACION DE CITA CLINICA CES";
            email.Headers.Add("X-Priority", "1");
            email.Body= new TextPart(MimeKit.Text.TextFormat.Html) {
                Text = "<html><body><p style=\"font-size:16px;\">TU CITA FUE PROGRAMADA PARA EL  <strong>" + toFecha + "</strong> EN EL HORARIO <strong>" + toHorario + "</strong> CON LA ESPECIALIDAD <strong>" + toEspecialidad + "</strong>.</p><p style=\"font-size:16px;\">POR FAVOR, CONFIRMA TU CITA HACIENDO CLICK EN ESTE ENLACE: <a href=\"https://localhost:7012/confirm?token=" + toToken + "\">CONFIRMAR CITA</a></p></body></html>"
                };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls );
            smtp.Authenticate("clinicacespradocentro@gmail.com","xqewyotmnaxktvtl");
            smtp.Send(email);
            smtp.Disconnect(true);
            return Ok("Mail sent");


        }

        [HttpGet("confirm")]
        public IActionResult ConfirmarCita(string token)
        {
            var appointment = _context.Agendamiento.FirstOrDefault(a => a.TokenUnique == token);

            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Emailconfirmacion = true;
            _context.SaveChanges();

              return Content("<div style='display: flex; justify-content: center; align-items: center; height: 100vh; background-color: #142c4c;'><div style='text-align: center; font-size: 34px; color: #fff; font-family: Arial, sans-serif;'>TU CITA HA SIDO CONFIRMADA, GRACIAS POR CONFIRMAR.</div></div>", "text/html");
        }


    }
}