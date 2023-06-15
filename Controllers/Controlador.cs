using System.Diagnostics;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using ProfesoresGuia.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Text;

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

    [HttpPost]
    public async Task<IActionResult> nuevoRegistroAsync(){
        Console.WriteLine("1");
        int num_prof = admProfesores.obtenerProfesores().Count + 1;
        string sede = Request.Form["sede"];
        DTOProfesor profe = new DTOProfesor(sede + "-" + ((num_prof < 10) ? "0" : "") + num_prof, Request.Form["nombre"], Request.Form["correo"], Request.Form["telefonoOficina"], Request.Form["telefonoCelular"], null, "Activo", (SiglasCentros)Enum.Parse(typeof(SiglasCentros), sede.ToUpper()));
        //   agrega el profesor a la base de datos
        if (HttpContext.Request.Form.Files.GetFile("fotografia") != null && HttpContext.Request.Form.Files.GetFile("fotografia").Length > 0){
            var ms = new MemoryStream();
            await HttpContext.Request.Form.Files.GetFile("fotografia").CopyToAsync(ms);
             profe.fotografia = ms.ToArray();
        }
        admProfesores.agregarProfesor(profe, Request.Form["contrasena"]);
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
        Console.WriteLine("Credenciales correctos");
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
        var todosEstudiantes = admEstudiantes.obtenerEstudiantes();
        ViewBag.estudiantes = todosEstudiantes;
        return View("../Asistente/estudiantesSede");
    }

    public IActionResult AsistenteCargarEstudiantes(){
        return View("../Asistente/cargarEstudiantes");
    }

    public IActionResult AsistenteCargarEstudiantesConf(){
        pExcel.LeerEstudiantesDesdeExcel(AppDomain.CurrentDomain.BaseDirectory + "estudiantes.xlsx");
        return AsistenteEstudiantes();
    }

    public IActionResult AsistenteProfesores(){
        var todosProfesores = admProfesores.obtenerProfesores();
        ViewBag.profesores = todosProfesores;
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
        var todosProfesores = admProfesores.obtenerProfesores();
        ViewBag.profesores = todosProfesores;
        return View("../Asistente/profesoresEquipo");
    }

    public IActionResult AsistenteEstudiantesEquipo(){
        var todosEstudiantes = admEstudiantes.obtenerEstudiantes();
        ViewBag.estudiantes = todosEstudiantes;
        return View("../Asistente/estudiantesEquipo");
    }

    public IActionResult AsistenteProximaActividad(){
        var act = admPlanes.consultarProxActividad();
        ViewBag.actividad= act;
        return View("../Asistente/proximaActividad");
    }

    public IActionResult AsistentePlanTrabajo(){
        var plan= admPlanes.consultarActividades();
        ViewBag.actividades = plan;
        return View("../Asistente/planTrabajo");
    }

    public IActionResult AsistenteDetallesActividad(){
        Actividad actv = admPlanes.consultarActividad(Int32.Parse(Request.Query["id"]));
        ViewBag.actividad = actv;
        return View("../Asistente/detallesActividad");
    }

    public IActionResult GestProfesores(){
        var todosProfesores = admProfesores.obtenerProfesores();
        ViewBag.profesores = todosProfesores;
        return View("../Asistente/gestProfesores");
    }

//------------------------------------------------------------------PROFE GUIA (Compartidas con Coordinador)-----------------------------------------------------------------
    
    
    public IActionResult ProfesorEstudiantesSede(){
        var miSede=(User.Claims.Where(x=> x.Type == ClaimTypes.Locality).SingleOrDefault().Value); 
        var estudiantes = admEstudiantes.obtenerEstudiantes();
        List<Estudiante> estudiantesFiltrados = estudiantes.Where(estudiante => estudiante.centroEstudio.siglas == (SiglasCentros)Enum.Parse(typeof(SiglasCentros), miSede.ToUpper())).ToList();
        ViewBag.estudiantes = estudiantesFiltrados;
        return View("../Profesor/estudiantesSede");
    }

    public IActionResult generarExcelEstudiantes()
   {
       return View("../Profesor/generarExcel");
   }
   
   [HttpPost]
   public IActionResult generarExcelEstudiantesConf()
   {
    if(Request.Form["opc"]=="MiSede"){
        var miSede=(User.Claims.Where(x=> x.Type == ClaimTypes.Locality).SingleOrDefault().Value);
        var listaEstudiantes = admEstudiantes.obtenerEstudiantes();
        pExcel.EscribirEstudiantesEnExcelXsede(listaEstudiantes,(SiglasCentros)Enum.Parse(typeof(SiglasCentros), miSede.ToUpper()),AppDomain.CurrentDomain.BaseDirectory + "estudiantesD.xlsx");
    }
    else{

        var listaEstudiantes = admEstudiantes.obtenerEstudiantes();
        pExcel.EscribirEstudiantesEnExcelSeparados(listaEstudiantes,AppDomain.CurrentDomain.BaseDirectory + "estudiantesD.xlsx");
    }
    return View("../Profesor/generarExcel");
   }

    public IActionResult modificarEstudiante()
    {
        //   obtiene estudiante
        Estudiante est = admEstudiantes.obtenerEstudiantes().FirstOrDefault(e => e.idEstudiante == Int32.Parse(Request.Query["id"]));
        ViewBag.Estudiante = est;
        
        //  se va a la pantalla para editar la info del estudiante
        return View("../Profesor/editarEstudiante");
    }
    
    [HttpPost]
    public IActionResult modificarEstudianteConf()
    {
        Console.WriteLine("El id es "+Int32.Parse(Request.Form["sede"]));
        DTOEstudiante estudiante = new DTOEstudiante(Int32.Parse(Request.Form["idE"]), Request.Form["carne"], Request.Form["nombre"], Request.Form["correo"], Request.Form["telefonoCelular"], admEstudiantes.getCentro(Int32.Parse(Request.Form["sede"])));
        admEstudiantes.modificarEstudiante(estudiante);

        return ProfesorEstudiantesSede();
    }

    public IActionResult ProfesorEstudiantesTodos(){
        var todosEstudiantes = admEstudiantes.obtenerEstudiantes();
        ViewBag.estudiantes = todosEstudiantes;
        return View("../Profesor/todosEstudiantes");
    }

    public IActionResult ProfesorProfesoresEquipo(){
        var todosProfesores = admProfesores.obtenerProfesores();
        ViewBag.profesores = todosProfesores;
        return View("../Profesor/profesoresEquipo");
    }

    public IActionResult ProfesorEstudiantesEquipo(){
        var todosEstudiantes = admEstudiantes.obtenerEstudiantes();
        ViewBag.estudiantes = todosEstudiantes;
        return View("../Profesor/estudiantesEquipo");
    }

    public IActionResult ProfesorPlanTrabajo(){
        var plan= admPlanes.consultarActividades();
        ViewBag.actividades = plan;

        if(User.Claims.Where(x=> x.Type == ClaimTypes.Role).SingleOrDefault().Value=="ProfesorCoordinador")
        return View("../Coordinador/planTrabajo");
        else
        return View("../Profesor/planTrabajo");
    }

    public IActionResult ProfesorDetallesActividad(){
        Actividad actv = admPlanes.consultarActividad(Int32.Parse(Request.Query["id"]));
        ViewBag.actividad = actv;
        if(User.Claims.Where(x=> x.Type == ClaimTypes.Role).SingleOrDefault().Value=="ProfesorCoordinador")
        return View("../Coordinador/detallesActividad");
        else
        return View("../Profesor/detallesActividad");

    }



//PROFESOR GUIA COORDINADOR

    public IActionResult InicioProfesor(){
        return View("../Profesor/Inicio");
    }

    

    

    public IActionResult CoordinadorNuevaActividad(){
        return View("../Coordinador/nuevaActividad");
    }

    public IActionResult ProfesorActividadDetalles(){
        return View("../Coordinador/actividadRealizada");
    }

    public IActionResult ProfesorComentariosActividad(){
        return View("../Profesor/detallesActividad");
    }

    public IActionResult coordinadorEvidencias(){
        return View("../Coordinador/ActividadRealizada");
    }

    public IActionResult CoordinadorPlanTrabajo(){
        var plan= admPlanes.consultarActividades();
        ViewBag.actividades = plan;
        return View("../Coordinador/planTrabajo");
    }

    public IActionResult CoordinadorDetallesActividad(){
        Actividad actv = admPlanes.consultarActividad(Int32.Parse(Request.Query["id"]));
        ViewBag.actividad = actv;
        return View("../Coordinador/detallesActividad");
    }







    
    //////////////////////////////////////////////////                                           //////////////////////////////////////////////////
    //////////////////////////////////////////////////                                           //////////////////////////////////////////////////
    //////////////////////////////////////////////////         Asistente Administrativa          //////////////////////////////////////////////////
    //////////////////////////////////////////////////                                           //////////////////////////////////////////////////
    //////////////////////////////////////////////////                                           //////////////////////////////////////////////////

    
    public IActionResult agregarProfesor()
    {
        return View("../Asistente/registrarProfesor");           // se va a la pantalla para ingresar la informacion del profesor
    }
    
    public async Task<IActionResult> agregarProfesorConfAsync()
    {
        int num_prof = admProfesores.obtenerProfesores().Count + 1;
        string sede = Request.Form["sede"];
        DTOProfesor profe = new DTOProfesor(sede + "-" + ((num_prof < 10) ? "0" : "") + num_prof, Request.Form["nombre"], Request.Form["correo"], Request.Form["telefonoOficina"], Request.Form["telefonoCelular"], null, "Activo", (SiglasCentros)Enum.Parse(typeof(SiglasCentros), sede.ToUpper()));
        //   agrega el profesor a la base de datos
        if (HttpContext.Request.Form.Files.GetFile("fotografia") != null && HttpContext.Request.Form.Files.GetFile("fotografia").Length > 0){
            var ms = new MemoryStream();
            await HttpContext.Request.Form.Files.GetFile("fotografia").CopyToAsync(ms);
             profe.fotografia = ms.ToArray();
        }
        admProfesores.agregarProfesor(profe, Request.Form["contrasena"]);
        //   regresa a la pantalla anterior
        return consultarProfesoresAsistente();
    }
    
    
    public IActionResult cargarEstudiantes()
    {
        return View("../Asistente/cargarEstudiantes");
    }
    
    public IActionResult cargarEstudiantesConf()
    {
        List<Estudiante> estudiantes = pExcel.LeerEstudiantesDesdeExcel(Request.Form["ruta"]);
        admEstudiantes.agregarEstudiantes(estudiantes);
        return consultarEstudiantesCentroAsistente();
    }
    
    
    public IActionResult editarProfesor()
    {
        //   obtiene info del profe
        ProfesorGuia profe = admProfesores.obtenerProfesores().FirstOrDefault(p => p.codigo == Request.Query["codigo"]);
        ViewBag.Profesor = profe;
       // TempData["Profesor"] = profe;
        
        //  se va a la pantalla para editar la info del profe
        return View("../Asistente/editarProfesor");
    }
    
    [HttpPost]
    public async Task<IActionResult> editarProfesorConfAsync()
    {
        //   obtiene info del profe
        Console.WriteLine("La sede es: "+Request.Form["sede"]);
        DTOProfesor profe = new DTOProfesor(Request.Form["codigo"], Request.Form["nombre"], Request.Form["correo"], Request.Form["telefonoOficina"], Request.Form["telefonoCelular"], null, Request.Form["activo"], Enum.Parse<SiglasCentros>(Request.Form["sede"].ToString()));
        profe.activo=Request.Form["activo"];
        if (HttpContext.Request.Form.Files.GetFile("fotografia") != null && HttpContext.Request.Form.Files.GetFile("fotografia").Length > 0){
            var ms = new MemoryStream();
            await HttpContext.Request.Form.Files.GetFile("fotografia").CopyToAsync(ms);
             profe.fotografia = ms.ToArray();
        }
        else{
            profe.fotografia= Encoding.ASCII.GetBytes(Request.Form["fotografiaV"]);
        }
        return consultarProfesoresAsistente();
    }

    
    [HttpPost]
    public IActionResult definirCoordinador()
    {
        ProfesorGuia profeGuia = (ProfesorGuia)ViewBag.Profesor;
        admEquipos.definirCoordinador(1, profeGuia.codigo);
        return consultarProfesoresAsistente();
    }



/*
    public IActionResult consultarPlanSinComentarios()
    {
        ViewBag.Plan = admPlanes.consultarPlan();
        return View("../Asistente/planTrabajo");
    }*/
    
    
    public IActionResult consultarProfesoresAsistente()
    {
        ViewBag.Profesores = admProfesores.obtenerProfesores();
        return View("../Asistente/profesoresEquipo");
    }
    
    public IActionResult consultarProfesorAsistente()
    {
        ViewBag.Profesor = admProfesores.obtenerProfesores().FirstOrDefault(p => p.codigo == Request.Form["codigo"]);
        return View("../Asistente/profesoresEquipo");
    }
    
    /*
    public IActionResult consultarActividadAsistente()
    {
        ViewBag.actividad = admPlanes.consultarActividad(Int32.Parse(Request.Query["id"]));
        return View("../Asistente/detallesActividad");
    }*/
    

    //////////////////////////////////////////////////                                //////////////////////////////////////////////////
    //////////////////////////////////////////////////                                //////////////////////////////////////////////////
    //////////////////////////////////////////////////         Profesor Guia          //////////////////////////////////////////////////
    //////////////////////////////////////////////////                                //////////////////////////////////////////////////
    //////////////////////////////////////////////////                                //////////////////////////////////////////////////

   
    public IActionResult editarSusDatosProfesor()
    {
        //   obtiene info del profe
        ProfesorGuia profe = admProfesores.obtenerProfesores().FirstOrDefault(p => p.codigo == Request.Query["codigo"]);
        ViewBag.Profesor = profe;
        
        //  se va a la pantalla para editar la info del profe
        return View("../Profesor/editarProfesor");
    }
    
    [HttpPost]
    public async Task<IActionResult> editarSusDatosProfesorConfAsync()
    {
        //   obtiene info del profe
        ProfesorGuia profeGuia = (ProfesorGuia)ViewBag.Profesor;
        DTOProfesor profe = new DTOProfesor(profeGuia.codigo, Request.Form["nombre"], Request.Form["correo"], Request.Form["telefonoOficina"], Request.Form["telefonoCelular"], null, profeGuia.activo, profeGuia.sede);
        admProfesores.editarProfesor(profe);

        if (HttpContext.Request.Form.Files.GetFile("fotografia") != null && HttpContext.Request.Form.Files.GetFile("fotografia").Length > 0){
            var ms = new MemoryStream();
            await Request.Form.Files.GetFile("fotografia").CopyToAsync(ms);
             profe.fotografia = ms.ToArray();
        }

        
        return View("../Profesor/Inicio");
    }
    
    
    
    /*
    public IActionResult consultarPlanConComentarios()
    {
        ViewBag.Plan = admPlanes.consultarPlan();
        return View("../Profesor/planTrabajo");
    }
    */
    
    
    
    public IActionResult consultarProfesoresProfesorGuia()
    {
        ViewBag.Profesores = admProfesores.obtenerProfesores();
        return View("../Profesor/profesoresEquipo");
    }
    
    
    
    public IActionResult consultarProfesorProfesorGuia()
    {
        ViewBag.Profesor = admProfesores.obtenerProfesores().FirstOrDefault(p => p.codigo == Request.Form["codigo"]);
        return View("../Profesor/profesoresEquipo");
    }
    
    
    
    /*
    public IActionResult consultarActividadProfesorGuia()
    {
        ViewBag.Actividad = admPlanes.consultarActividad(Int32.Parse(Request.Query["id"]));
        return View("../Profesor/detallesActividad");
    }*/
    
    
    
    
    
    
    //////////////////////////////////////////////////                              //////////////////////////////////////////////////
    //////////////////////////////////////////////////                              //////////////////////////////////////////////////
    //////////////////////////////////////////////////         Coordinador          //////////////////////////////////////////////////
    //////////////////////////////////////////////////                              //////////////////////////////////////////////////
    //////////////////////////////////////////////////                              //////////////////////////////////////////////////

    public IActionResult agregarActividad(){
        ViewBag.listProfes = admProfesores.obtenerProfesores();
        return View("../Coordinador/nuevaActividad");
    }
    
    public async Task<IActionResult> agregarActividadConfAsync()
    {
        int idAct = admPlanes.consultarActividades().Count + 1;
        string recordatorios = Request.Form["recordatorios"];
        string tipo = Request.Form["tipo"];
        string modalidad = Request.Form["modalidad"];
        string estado = Request.Form["estado"];
        //// revisar formato de las listas
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
       
        Actividad act = new Actividad(idAct, Int32.Parse(Request.Form["semana"]), (TipoActividad)Enum.Parse(typeof(TipoActividad), tipo.ToUpper()), 
            Request.Form["nombre"], DateTime.Parse(Request.Form["fechahoraActividad"]), 
            responsablesList, DateTime.Parse(Request.Form["fechahoraAnuncio"]), 
            Int32.Parse(Request.Form["diasPreviosAnuncio"]), 
            recordatorios.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s => DateTime.Parse(s.Trim())).ToList(), 
            (Modalidad)Enum.Parse(typeof(Modalidad), modalidad.ToUpper()), Request.Form["enlace"], 
            EstadoActividad.PLANEADA, new List<Comentario>());

        if (HttpContext.Request.Form.Files.GetFile("fotografia") != null && HttpContext.Request.Form.Files.GetFile("fotografia").Length > 0){
            var ms = new MemoryStream();
            await HttpContext.Request.Form.Files.GetFile("afiche").CopyToAsync(ms);
            act.afiche = ms.ToArray();
        }

        

        admPlanes.agregarActividadPlan(act);
        return CoordinadorPlanTrabajo();
    }

    public IActionResult activarPublicacion()
    {
        int idAct=Int32.Parse(Request.Query["id"]);
        admPlanes.activarPublicacion(idAct);
        return ProfesorPlanTrabajo();
    }

    public IActionResult marcarCancelada()
    {
        ViewBag.act = Request.Query["id"];
        Console.WriteLine("Pasando un : "+Request.Query["id"]);
        return View("../Coordinador/CancelarActividad");
    }
    
    [HttpPost]
    public IActionResult marcarCanceladaJustificacion()
    {
        admPlanes.marcarCancelada(Int32.Parse(Request.Form["idAct"]), Request.Form["obs"], DateTime.Now);
        return CoordinadorPlanTrabajo();
    }

    public IActionResult marcarRealizada()
    {
        return View("../Coordinador/ActividadRealizada");
    }

    [HttpPost]
    public void pruebaImg(){
        var imgs1 = HttpContext.Request.Form.Files["lista"];
        var imgs2 = HttpContext.Request.Form.Files["pruebas"];

    }



    /*
    [HttpPost]
    public IActionResult marcarRealizadaEvidencia()
    {
        Actividad act = (Actividad)ViewBag.Actividad;
        string[] imagenes = string.Join("", Request.Form["imagenes"]).Split(new[] { ". ", "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
        List<Imagen> imagenesList = new List<Imagen>();
        int idEvidencia = admPlanes.getEvidencias().Count + 1;
        int idImagen = admPlanes.getImagenes().Count + 1;
        //// revisar formato de las listas
        foreach (var imagen in imagenes)
        {
            imagenesList.Add(new Imagen(idImagen, idEvidencia, imagen));
            idImagen++;
        }
       
        admPlanes.marcarRealizada(new Evidencia(idEvidencia, act.idActividad, imagenesList, Request.Form["asistencias"], Request.Form["linkGrabacion"]));
        return consultarPlanCoord();
    }*/
    
    

    
    public IActionResult consultarProfesoresCoordinador()
    {
        ViewBag.Profesores = admProfesores.obtenerProfesores();
        return View("../Coordinador/profesoresEquipo");
    }


    
    
    
    public IActionResult consultarProfesorCoordinador()
    {
        ViewBag.Profesor = admProfesores.obtenerProfesores().FirstOrDefault(p => p.codigo == Request.Form["codigo"]);
        return View("../Coordinador/profesoresEquipo");
    }
    

    
    
    
    //////////////////////////////////////////////////                                 //////////////////////////////////////////////////
    //////////////////////////////////////////////////                                 //////////////////////////////////////////////////
    //////////////////////////////////////////////////         Mas de un user          //////////////////////////////////////////////////
    //////////////////////////////////////////////////                                 //////////////////////////////////////////////////
    //////////////////////////////////////////////////                                 //////////////////////////////////////////////////

    
    
    // Profe Guia, Coordinador
    [HttpPost]
    public void realizarComentario()
    {
        int idActividad = Int32.Parse(Request.Form["idActividadc"]);
        String idProfesor = "1-04";//Int32.Parse(User.Claims.Where(x=> x.Type == ClaimTypes.NameIdentifier).SingleOrDefault().Value);
        int idComentario = admComentarios.getComentariosCount() + 1;
        ProfesorGuia profe = admProfesores.obtenerProfesores().FirstOrDefault(p => p.codigo == idProfesor);
        admComentarios.realizarComentario(new Comentario(idComentario, profe, DateTime.Now, Request.Form["comentario"]), idActividad);
        
    }
    

    //// revisar view de respuesta
    public void realizarRespuestaConf()
    {
        int idRespuesta = admRespuestas.getRespuestasCount() + 1;
        int idComentario = Int32.Parse(Request.Form["idComentarioR"]);
        String idProfesor =  "1-04";//Int32.Parse(User.Claims.Where(x=> x.Type == ClaimTypes.NameIdentifier).SingleOrDefault().Value);
        ProfesorGuia profe = admProfesores.obtenerProfesores().FirstOrDefault(p => p.codigo == idProfesor);
        admRespuestas.realizarRespuesta(new Respuesta(idRespuesta, idComentario, profe, DateTime.Now, Request.Form["respuesta"]));
        
    }
    
    
    
    
    
    
    
    // Profesor Guia, Coordinador
    
    
    
    
    
    
    
    // Asistente, Profe Guia, Coordinador
    public IActionResult cambiarContrasena()
    {
        return View();/// no se
    }
    //// revisar view de cambiar contrasena
    [HttpPost]
    public IActionResult cambiarContrasenaConf()
    {
        String correo = Request.Form["correo"];
        String contrasena = Request.Form["contrasena"];
        SingletonDAO.getInstance().cambiarContrasena(correo, contrasena);
        return View("../Acceso/IniciarSesion");
    }
    
    
    


    // Asistente
    public IActionResult consultarEstudiantesCentroAsistente()
    {
        //  obtiene estudiantes del centro y despliega su pantalla
        ViewBag.Estudiantes = admEstudiantes.consultarEstudiantesCentro(Int32.Parse(Request.Query["id"]));
        return View("../Asistente/estudiantesSede");
    }
    
    
    
    
    // Profe Guia, Coordinador
    public IActionResult consultarEstudiantesCentroProfeGuiaCoordinador()
    {
        //  obtiene estudiantes del centro y despliega su pantalla
        ViewBag.Estudiantes = admEstudiantes.consultarEstudiantesCentro(Int32.Parse(Request.Query["id"]));
        return View("../Profesor/estudiantesSede");
    }
    

    // Asistente
    public IActionResult consultarEstudiantesAsistente()
    {
        //   obtiene estudiantes y despliega su pantalla
        ViewBag.Estudiantes = admEstudiantes.obtenerEstudiantes();
        return View("../Asistente/estudiantesEquipo");
    }
    
   
    // Profe Guia, Coordinador
    public IActionResult consultarEstudiantesProfeGuiaCoordinador()
    {
        //   obtiene estudiantes y despliega su pantalla
        ViewBag.Estudiantes = admEstudiantes.obtenerEstudiantes();
        return View("../Profesor/estudiantesEquipo");
    }
    


    //////////////////////////////////////////////////////////////////////////////////////////////
    //                                             TERCER PROYECTO
    //////////////////////////////////////////////////////////////////////////////////////////////

    public IActionResult InicioEstudiante(){
        return View("../Estudiante/Inicio");
    }

    public int getCantNot(){
        int id= 1;
        var nots = SingletonDAO.getInstance().getNotificaciones(id).Where(n => n.estado == "NO_LEIDA");
        return nots.Count();
    }

    public IActionResult miInformacion(){
        var est=admEstudiantes.obtenerEstudiantes().FirstOrDefault(p => p.idEstudiante == 1);
        ViewBag.estudiante=est;
        //Int32.Parse(User.Claims.Where(x=> x.Type == ClaimTypes.NameIdentifier).SingleOrDefault().Value);
        return View("../Estudiante/miInformacion");
    }

    public IActionResult editarInformacionEstudiante(){
        var est=admEstudiantes.obtenerEstudiantes().FirstOrDefault(p => p.idEstudiante == 1);
        ViewBag.estudiante=est;
        //Int32.Parse(User.Claims.Where(x=> x.Type == ClaimTypes.NameIdentifier).SingleOrDefault().Value);
        return View("../Estudiante/editarEstudiante");
    }

    public IActionResult calendarioEstudiante(){
        var plan= admPlanes.consultarActividadesPlaneadas();
        ViewBag.actividades = plan;
        return View("../Estudiante/calendario");
    }

    public IActionResult EstudianteProximaActividad(){
        var act = admPlanes.consultarProxActividad();
        ViewBag.actividad= act;
        return View("../Estudiante/proximaActividad");
    }

    public IActionResult EstudianteDetallesActividad(){
        ViewBag.Actividad = admPlanes.consultarActividad(Int32.Parse(Request.Query["id"]));
        return View("../Estudiante/detallesActividad");
    }

    public IActionResult EstudianteMisChats(){
        ViewBag.chats= SingletonDAO.getInstance().getMisChats();
        return View("../Estudiante/misChats");
    }

    public IActionResult verChat(){
        int idChat=Int32.Parse(Request.Query["id"]);
        ViewBag.elChat=SingletonDAO.getInstance().getMisChats().FirstOrDefault(c => c.id == idChat);
        ViewBag.mensajes=SingletonDAO.getInstance().getMensajesChat(idChat);
        ViewBag.yo=1;

        return View("../Estudiante/unChat");
    }

    [HttpPost]
    public void enviarMensaje(string pSala, string pMensaje){
        int idUsuario =1;//Int32.Parse(User.Claims.Where(x=> x.Type == ClaimTypes.NameIdentifier).SingleOrDefault().Value);
        SingletonDAO.getInstance().enviarMensajeChat(Int32.Parse(pSala),idUsuario,pMensaje);
    }

    public IActionResult notificaciones(){
        int id= 1;//Int32.Parse(Request.Query["id"]);
        ViewBag.notificaciones= SingletonDAO.getInstance().getNotificaciones(id);
        return View("../Estudiante/misNotificaciones");
    }

    public IActionResult unaNotificacion(){
        int id= 1;//Int32.Parse(Request.Query["id"]);
        int idNot = Int32.Parse(Request.Query["id"]);
        ViewBag.notificacion = SingletonDAO.getInstance().getNotificaciones(id).FirstOrDefault(n => n.idNotificacion == idNot);
        SingletonDAO.getInstance().marcarLeida(idNot);
        return View("../Estudiante/unaNotificacion");
    }

    public IActionResult marcarNoLeida(int idNot){
        SingletonDAO.getInstance().marcarNoLeida(idNot);
        return notificaciones();
    }

    public IActionResult eliminarNotificacion(int idNot){
        SingletonDAO.getInstance().eliminarNot(idNot);
        return notificaciones();
    }

    public IActionResult darBajaServicio(int idBuzon, int idServicio){
        SingletonDAO.getInstance().desafiliarServicio(idBuzon,idServicio);
        return notificaciones();
    }

    public IActionResult eliminarTodasNotificaciones(){
        var nots = SingletonDAO.getInstance().getNotificaciones(1);
        foreach (var una in nots){
            SingletonDAO.getInstance().eliminarNot(una.idNotificacion);
        }
        return notificaciones();
    }

    public IActionResult EstudianteModificarInformacion(){
        var nuevoTelefono = Request.Form["telefonoCelular"];
        var idEstudiante = Int32.Parse(Request.Form["idEstudiante"]);
        admEstudiantes.modificarEstudianteTel(nuevoTelefono,idEstudiante);
        return miInformacion();
    }








     

    

    
    
}