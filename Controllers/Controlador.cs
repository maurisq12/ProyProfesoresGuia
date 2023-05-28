using System.Diagnostics;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using ProfesoresGuia.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ProfesoresGuia.Controllers;

public class Controlador : Controller
{
    private AdmComentarios admComentarios = new AdmComentarios();
    private AdmRespuestas admRespuestas = new AdmRespuestas();
    
    private AdminPlanes admPlanes = new AdminPlanes();
    
    private AdminEquipos admEquipos = new AdminEquipos();
    
    private AdminEstudiantes admEstudiantes = new AdminEstudiantes();
    private AdminProfesores admProfesores = new AdminProfesores();

    private ProcesadorExcel pExcel = new ProcesadorExcel();
    
    
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





    
    /// 
    /// 
    ///         Asistente Administrativa
    /// 
    ///
    
    public IActionResult agregarProfesor()
    {
        return View();           // se va a la pantalla para ingresar la informacion del profesor
    }
    
    public IActionResult agregarProfesorConf()
    {
        //   agrega el profesor a la base de datos
        int num_prof = admProfesores.obtenerProfesores().Count + 1;
        string sede = Request.Form["sede"];
        DTOProfesor profe = new DTOProfesor(sede + "-" + ((num_prof < 10) ? "0" : "") + num_prof, Request.Form["nombre"], Request.Form["correo"], Request.Form["telefonoOficina"], Request.Form["telefonoCelular"], Request.Form["fotografia"], "Activo", (SiglasCentros)Enum.Parse(typeof(SiglasCentros), sede.ToUpper()));
        admProfesores.agregarProfesor(profe);
        //   regresa a la pantalla anterior
        return View(se devuelve);
    }
    

    

    
    
   
    
    
    
    
    
    
    
    
    

    
    public IActionResult editarProfesor()
    {
        //   obtiene info del profe
        ProfesorGuia profe = admProfesores.obtenerProfesores().FirstOrDefault(p => p.codigo == Request.Query["codigo"]);
        ViewBag.Profesor = profe;
        
        //  se va a la pantalla para editar la info del profe
        return View();
    }
    
    [HttpPost]
    public IActionResult editarProfesorConf()
    {
        //   obtiene info del profe
        ProfesorGuia profeGuia = (ProfesorGuia)ViewBag.Profesor;
        DTOProfesor profe = new DTOProfesor(profeGuia.codigo, Request.Form["nombre"], Request.Form["correo"], Request.Form["telefonoOficina"], Request.Form["telefonoCelular"], Request.Form["fotografia"], Request.Form["activo"], profeGuia.sede);
        admProfesores.editarProfesor(profe);

        
        return View(se devuelve);
    }

    [HttpPost]
    public IActionResult definirCoordinador()
    {
        ProfesorGuia profeGuia = (ProfesorGuia)ViewBag.Profesor;
        admEquipos.definirCoordinador(1, profeGuia.codigo);
        return View(se devuelve);
    }
    
    public IActionResult editarSusDatosProfesor()
    {
        //   obtiene info del profe
        ProfesorGuia profe = admProfesores.obtenerProfesores().FirstOrDefault(p => p.codigo == Request.Query["codigo"]);
        ViewBag.Profesor = profe;
        
        //  se va a la pantalla para editar la info del profe
        return View();
    }
    
    [HttpPost]
    public IActionResult editarSusDatosProfesorConf()
    {
        //   obtiene info del profe
        ProfesorGuia profeGuia = (ProfesorGuia)ViewBag.Profesor;
        DTOProfesor profe = new DTOProfesor(profeGuia.codigo, Request.Form["nombre"], Request.Form["correo"], Request.Form["telefonoOficina"], Request.Form["telefonoCelular"], Request.Form["fotografia"], profeGuia.activo, profeGuia.sede);
        admProfesores.editarProfesor(profe);

        
        return View(se devuelve);
    }
    
    public IActionResult modificarEstudiante()
    {
        //   obtiene estudiante
        Estudiante est = admEstudiantes.obtenerEstudiantes().FirstOrDefault(e => e.idEstudiante == Request.Query["id"]);
        ViewBag.Estudiante = est;
        
        //  se va a la pantalla para editar la info del estudiante
        return View();
    }
    
    [HttpPost]
    public IActionResult modificarEstudianteConf()
    {
        Estudiante est = (Estudiante)ViewBag.Estudiante;
        DTOEstudiante estudiante = new DTOEstudiante(est.idEstudiante, est.carne, Request.Form["nombre"], Request.Form["correo"], Request.Form["telefonoCelular"], admEstudiantes.getCentros().FirstOrDefault(c => c.nombre == Request.Form["Centro"]));
        admEstudiantes.modificarEstudiante(est.idEstudiante, estudiante);

        
        return View(se devuelve);
    }
    
    public IActionResult activarPublicacion()
    {
        Actividad act = (Actividad)ViewBag.Actividad;
        admPlanes.activarPublicacion(act.idActividad);
        return View(se devuelve);
    }
    
    
    
    
    
    
    
    
    public IActionResult consultarProfesores()
    {
        ViewBag.Profesores = admProfesores.obtenerProfesores();
        return View();
    }
    
    public IActionResult consultarProfesor()
    {
        ViewBag.Profesor = admProfesores.obtenerProfesores().FirstOrDefault(p => p.codigo == Request.Form["codigo"]);
        return View(el de consultarProfesores());
    }

   
    
    public IActionResult consultarProximaActividad()
    {
        ViewBag.Actividad = admPlanes.consultarProxActividad(1);
        return View();
    }

  

    public IActionResult consultarEstudiantesCentro()
    {
        //  obtiene estudiantes del centro y despliega su pantalla
        ViewBag.Estudiantes = admEstudiantes.consultarEstudiantesCentro(Int32.Parse(Request.Query["id"]));
        return View();
    }
    
    public IActionResult consultarEstudiantes()
    {
        //   obtiene estudiantes y despliega su pantalla
        ViewBag.Estudiantes = admEstudiantes.obtenerEstudiantes();
        return View();
    }

    public IActionResult consultarComentarios()
    {
        int idActividad = Int32.Parse(Request.Query["id"]);
        List<Comentario> comentarios = admComentarios.consultarComentarios(idActividad);
        List<Respuesta> respuestas = new List<Respuesta>();
        List<Respuesta> found = new List<Respuesta>();
        foreach (var comentario in comentarios)
        {
            found = admRespuestas.consultarRespuestas(comentario.idComentario);
            if (found != null && found.Count > 0)
            {
                respuestas = respuestas.Union(found).ToList();
            }
        }
        
        ViewBag.IdActividad = idActividad;
        ViewBag.Comentarios = comentarios;
        ViewBag.Respuestas = respuestas;
        return View();
    }

    public IActionResult consultarPlanConComentarios()
    {
        ViewBag.Plan = admPlanes.consultarPlan(1);
        return View();
    }
    
    public IActionResult consultarPlanSinComentarios()
    {
        ViewBag.Plan = admPlanes.consultarPlan(1);
        return View();
    }
    
    public IActionResult consultarPlanCoord()
    {
        ViewBag.Plan = admPlanes.consultarPlan(1);
        return View();
    }
    
    public IActionResult consultarActividad()
    {
        ViewBag.Actividad = admPlanes.consultarActividad(Int32.Parse(Request.Query["id"]));
        return View();
    }

    public IActionResult marcarCancelada()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult marcarCanceladaJustificacion()
    {
        Actividad act = (Actividad)ViewBag.Actividad;
        admPlanes.marcarCancelada(act.idActividad, Request.Form["justificacion"], DateTime.Now);
       return View(se devuelve);
    }
   
    public IActionResult realizarComentario()
    {
        int idActividad = (int)ViewBag.IdActividad;
        admComentarios.realizarComentario(idActividad, Request.Form["comentario"]);
        return View(el de consultarComentarios);
    }
   
   
   
   
   
   
   
    
    
    
    
    
    
    
   public IActionResult agregarActividad()
   {
       ViewBag.Plan = admPlanes.consultarPlan(1);
       return View();
   }
    
   public IActionResult agregarActividadConf()
   {
       int idAct = admPlanes.consultarActividades().Count + 1;
       string recordatorios = Request.Form["recordatorios"];
       string tipo = Request.Form["tipo"];
       string modalidad = Request.Form["modalidad"];
       string estado = Request.Form["estado"];
       
       string[] responsables = string.Join("", Request.Form["responsables"]).Split(new[] { ". ", "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
       List<ProfesorGuia> profes = admProfesores.obtenerProfesores();
       List<ProfesorGuia> responsablesList = new List<ProfesorGuia>();

       ProfesorGuia found = null;
       foreach (var profe in responsables)
       {
           found = profes.FirstOrDefault(p => p.nombreCompleto == profe);
           if (found is not null)
           {
               responsablesList.Add(found);
           }
       }
       
       DTOActividad act = new DTOActividad(idAct, Int32.Parse(Request.Form["semana"]), (TipoActividad)Enum.Parse(typeof(TipoActividad), tipo.ToUpper()), 
           Request.Form["nombre"], DateTime.Parse(Request.Form["fechaActividad"] + " " + Request.Form["horaActividad"]), 
           responsablesList, DateTime.Parse(Request.Form["fechaAnuncio"] + " " + Request.Form["horaAnuncio"]), 
           Int32.Parse(Request.Form["diasPreviosAnuncio"]), 
           recordatorios.Split(new[] { ". ", "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => DateTime.Parse(s.Trim())).ToList(), 
           (Modalidad)Enum.Parse(typeof(Modalidad), modalidad.ToUpper()), Request.Form["enlace"], Request.Form["afiche"], 
           (EstadoActividad)Enum.Parse(typeof(EstadoActividad), estado.ToUpper()));

       admPlanes.agregarActividadPlan((PlanTrabajo)ViewBag.Plan, act);
       return View(se devuelve);
   }

   public IActionResult marcarRealizada()
   {
       return View();
   }
    
   [HttpPost]
   public IActionResult marcarRealizadaEvidencia()
   {
       Actividad act = (Actividad)ViewBag.Actividad;
       string[] imagenes = string.Join("", Request.Form["imagenes"]).Split(new[] { ". ", "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
       List<Imagen> imagenesList = new List<Imagen>();

       foreach (var imagen in imagenes)
       {
           imagenesList.Add(new Imagen(, , imagen));
       }
       
       admPlanes.marcarRealizada(new Evidencia(, act.idActividad, imagenesList, Request.Form["asistencias"], Request.Form["linkGrabacion"]));
       return View(se devuelve);
   }
    
   public IActionResult realizarRespuesta()
   {
       ViewBag.IdComentario = Int32.Parse(Request.Query["idComentario"]);
       return View();
   }
    
   public IActionResult realizarRespuestaConf()
   {
       admRespuestas.realizarRespuesta((int)ViewBag.IdComentario, Request.Form["respuesta"]);
       return View(al de consultarComentarios);
   }
   
   public IActionResult cambiarContrasena()
   {
       return View();
   }
    
   [HttpPost]
   public IActionResult cambiarContrasenaConf()
   {
       String correo = Request.Form["correo"];
       String contrasena = Request.Form["contrasena"];
       SingletonDAO.getInstance().cambiarContrasena(correo, contrasena);
       return View(se va a inicio sesion);
   }
   
   
   
   
   
   
   
   
    
   

   public IActionResult cargarEstudiantes()
   {
       return View();
   }
    
   public IActionResult cargarEstudiantesConf()
   {
       admEstudiantes.convertirEstudiantes(Request.Form["ruta"]);
       return View(se devuelve);
   }
   
   public IActionResult generarExcelEstudiantes()
   {
       return View();
   }
   
   public IActionResult generarExcelEstudiantesSede()
   {
       //// no se si es asi
       return View();
   }
   
   public IActionResult generarExcelEstudiantesSedeConf()
   {
       pExcel.generarExcelEstudiantes(Request.Form["ruta"]);
       return View(se devuelve);
   }
    
    public IActionResult generarExcelPestanas()
    {
        //// no se si es asi
        return View();
    }
    
    public IActionResult generarExcelPestanasConf()
    {
        pExcel.generarExcelPestanas(Request.Form["ruta"]);
        return View(se devuelve);
    }
    
    
    
 /*   public IActionResult cargarEstudiantesCentro(String pRuta, int pCentro)
    {
        
    }
    
    public IActionResult cargarEstudiantesPestanas(String pRuta)
    {
        
    }*/



}