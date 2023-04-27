using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using devopsproyect.Models;
using devopsproyect.Permisos;
using devopsproyect.Data;
using System.Globalization;

namespace devopsproyect.Controllers;



    [ValidarSesion]
    public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
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


    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

         public IActionResult CerrarSesion()
    {
        HttpContext.Session.Remove("usuario");
        return RedirectToAction("Login", "Acceso");
    }  




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
