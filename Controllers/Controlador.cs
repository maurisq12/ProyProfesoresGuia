using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProfesoresGuia.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ProfesoresGuia.Controllers;

public class Controlador : Controller
{
    //Inicio de sesión
    
    public IActionResult InicioSesion(){
        return View("../Acceso/IniciarSesion");
    }

    public IActionResult nuevoRegistro(){
        
        return View("../Acceso/RegistroExito");
    }

    public IActionResult Registrar(){
        return View("../Acceso/Registrar");
    }

    [HttpPost]
    public async Task<IActionResult> IniciarSesion(){
        ClaimsIdentity auntent= await SingletonDAO.getInstance().sesionUsuario(Request.Form["correo"],Request.Form["contrasena"]);
        Console.WriteLine("Validando información de usuario...");
        if(auntent==null){
            ViewBag.credenciales="incorrectos";
            return View("../Acceso/IniciarSesion");
        }
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(auntent),new AuthenticationProperties());
        switch(auntent.Claims.Where(c => c.Type == ClaimTypes.Role)
                   .Select(c => c.Value).SingleOrDefault()){                
            case "Profesor":
            case "ProfesorCoordinador":   
                return View("../Profesor/Inicio");
            case "AsistenteAdministrativo":
            case "AsistenteAdministrativoCartago":
                return View("../Asistente/Inicio");            
        }
        return View("../Acceso/IniciarSesion");
    }

    public async Task<IActionResult> CerrarSesion(){
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return View("../Acceso/IniciarSesion");
    }

    

    //Mostrar páginas

    //------------------------------------------------------------------ASISTENTE-----------------------------------------------------------------

    public IActionResult InicioAsistente(){
        return View("../Asistente/Inicio");
    }

    public IActionResult AsistenteEstudiantes(){
        ViewBag.estudiantes= new List<Estudiante>();
        return View("../Asistente/estudiantesSede");
    }

    public IActionResult AsistenteCargarEstudiantes(){
        return View("../Asistente/cargarEstudiantes");
    }

    public IActionResult AsistenteProfesores(){
        ViewBag.profesores= new List<ProfesorGuia>();
        return View("../Asistente/gestProfesores");
    }

    public IActionResult AsistenteEditarProfesor(){
        //ViewBag.actividad= new Actividad();
        return View("../Asistente/editarProfesor");
    }

    public IActionResult AsistenteRegistrarProfesor(){
        return View("../Asistente/registrarProfesor");
    }
    
    public IActionResult AsistenteProfesoresEquipo(){
        ViewBag.profesores= new List<ProfesorGuia>();
        return View("../Asistente/profesoresEquipo");
    }

    public IActionResult AsistenteEstudiantesEquipo(){
        ViewBag.estudiantes= new List<ProfesorGuia>();
        return View("../Asistente/estudiantesEquipo");
    }

    public IActionResult AsistenteProximaActividad(){
        //ViewBag.actividad= new Actividad();
        return View("../Asistente/proximaActividad");
    }

    public IActionResult AsistentePlanTrabajo(){
        //ViewBag.actividad= new Actividad();
        return View("../Asistente/planTrabajo");
    }

    public IActionResult AsistenteDetallesActividad(){
        //ViewBag.actividad= new Actividad();
        return View("../Asistente/detallesActividad");
    }










    public IActionResult InicioProfesor(){
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
        return View("../Profesor/editarEstudiante");
    }

    /*public IActionResult GestProfesores(){
        var todosProfesores = admProfesores.obtenerProfesores();
        ViewBag.profesores = todosProfesores;
        return View("../Coordinador/gestProfesores");
    }*/

    public IActionResult CoordinadorNuevaActividad(){
        return View("../Coordinador/nuevaActividad");
    }

    public IActionResult ProfesorPlanTrabajo(){
        return View("../Profesor/planTrabajo");
    }

    public IActionResult ProfesorActividadDetalles(){
        return View("../Coordinador/actividadRealizada");
    }

    public IActionResult ProfesorComentariosActividad(){
        return View("../Profesor/detallesActividad");
    }




    





}