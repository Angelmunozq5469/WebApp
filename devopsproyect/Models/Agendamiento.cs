
using System.ComponentModel.DataAnnotations;

namespace devopsproyect.Models
{
    public class Agendamiento
    {
        [Key]
        public int IdAgendamiento { get; set; }

        public string Nombre { get; set; }

        [Required]
        public string Correo { get; set; }

        public string Numero { get; set; }

        [Required]

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public String Fecha { get; set;}

        public TimeSpan Horario { get; set; }

        public string Especialidad { get; set; }

        public bool Emailconfirmacion {get; set;}

        [MaxLength(100)]
        public string TokenUnique {get; set;}
    }
}
