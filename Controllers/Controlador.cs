using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class Controlador : Controller
{
    public IActionResult VInicioSesion(){
        return View("../Acceso/IniciarSesion");
    }

    public IActionResult VRegistrar(){
        return View("../Acceso/Registrar");
    }

    public IActionResult VInicioAsistente(){
        return View("../Asistente/Inicio");
    }

    public IActionResult VInicioProfesor(){
        return View("../Profesor/Inicio");
    }

    public IActionResult GestProfesores(){
        return View("../Coordinador/gestProfesores");
    }





}