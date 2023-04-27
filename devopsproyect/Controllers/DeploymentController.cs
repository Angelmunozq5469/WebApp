using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using devopsproyect.Models;
using devopsproyect.Permisos;
using devopsproyect.Data;
using System.Globalization;

namespace devopsproyect.Controllers;


    public class DeploymentController : Controller
{
    private readonly ApplicationDbContext _db;

    public DeploymentController(ApplicationDbContext db)
    {
        _db = db;
    }

 public IActionResult mostrarcitas()
{
    var citas = _db.Agendamiento.OrderBy(a => a.Especialidad)
                                 .ThenBy(a => a.Fecha)
                                 .ThenBy(a => a.Horario)
                                 .ToList();

    var citasPorEspecialidad = new Dictionary<string, List<Agendamiento>>();
    foreach (var cita in citas)
    {
        if (!citasPorEspecialidad.ContainsKey(cita.Especialidad))
        {
            citasPorEspecialidad[cita.Especialidad] = new List<Agendamiento>();
        }
        citasPorEspecialidad[cita.Especialidad].Add(cita);
    }

    return View(citasPorEspecialidad);
}



    

    //GET

    public IActionResult Edit (int? IdAgendamiento )
    {
        if(IdAgendamiento==null || IdAgendamiento ==0)
        {
            return NotFound();
        }
        var agendamientofromdb = _db.Agendamiento.Find(IdAgendamiento);

        if (agendamientofromdb == null)
        {
            return NotFound();
        }
        return View(agendamientofromdb);
    }




    //POST
    [HttpPost]
    public IActionResult Edit(Agendamiento obj)
    {
        var existingAgendamiento = _db.Agendamiento.Find(obj.IdAgendamiento);
        if (existingAgendamiento == null)
        {
            return NotFound();
        }

        existingAgendamiento.Nombre = obj.Nombre;
        existingAgendamiento.Correo = obj.Correo;
        existingAgendamiento.Numero = obj.Numero;
        existingAgendamiento.Fecha = obj.Fecha;
        existingAgendamiento.Horario = obj.Horario;
        existingAgendamiento.Especialidad = obj.Especialidad;

        _db.SaveChanges();
        return RedirectToAction("mostrarcitas");



    }

    //GET

    public IActionResult Delete (int? IdAgendamiento )
    {
        if(IdAgendamiento==null || IdAgendamiento ==0)
        {
            return NotFound();
        }
        var agendamientofromdb = _db.Agendamiento.Find(IdAgendamiento);

        if (agendamientofromdb == null)
        {
            return NotFound();
        }
        return View(agendamientofromdb);
    }




    //POST
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? IdAgendamiento)
    {
        var obj = _db.Agendamiento.Find(IdAgendamiento);
        if (obj == null)
        {
            return NotFound();
        }

        _db.Agendamiento.Remove(obj);
        _db.SaveChanges();
        return RedirectToAction("mostrarcitas");



    }
    

    

}

