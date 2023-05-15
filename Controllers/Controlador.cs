using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class Controlador : Controller
{
    private AdmComentarios admComentarios = new AdmComentarios();
    private AdmRespuestas admRespuestas = new AdmRespuestas();
    
    private AdminPlanes admPlanes = new AdminPlanes();
    
    private AdminEquipos admEquipos = new AdminEquipos();
    
    private AdminEstudiantes admEstudiantes = new AdminEstudiantes();
    private AdminProfesores admProfesores = new AdminProfesores();
    
    
    public IActionResult InicioSesion(){
        return View("../Acceso/IniciarSesion");
    }

    public IActionResult Registrar(){
        return View("../Acceso/Registrar");
    }

    public IActionResult InicioAsistente(){
        return View("../Asistente/Inicio");
    }

    public IActionResult nicioProfesor(){
        return View("../Profesor/Inicio");
    }

    public IActionResult CoordinadorProfesoresEquipo(){
        return View("../Coordinador/profesoresEquipo");
    }

    public IActionResult CoordinadorEstudiantesEquipo(){
        return View("../Coordinador/estudiantesEquipo");
    }

    public IActionResult CoordinadorRegistrarProfesor(){
        return View("../Coordinador/registrarProfesor");
    }

    public IActionResult CoordinadorEditarProfesor(){
        return View("../Coordinador/editarProfesor");
    }

    public IActionResult GestProfesores(){
        var todosProfesores = admProfesores.obtenerProfesores();
        ViewBag.profesores = todosProfesores;
        return View("../Coordinador/gestProfesores");
    }

    public IActionResult CoordinadorNuevaActividad(){
        return View("../Coordinador/nuevaActividad");
    }
    





}