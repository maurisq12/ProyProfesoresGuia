using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Data.SqlClient;
using ProfesoresGuia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Data;

namespace ProfesoresGuia.Controllers;

public class SingletonDAO
{    
    private static SingletonDAO instance;

    private SingletonDAO(){
    }

    public static SingletonDAO getInstance(){
        if (instance==null){
            instance= new SingletonDAO();
        } 
        return instance;
    }
    
    
    //Funciones de inicio de sesión y cerrado de sesión


    public int validacion(string pCorreo, string pContrasena){
        SingletonDB basedatos = SingletonDB.getInstance();
        basedatos.getConnection().Open();

        string query= "SELECT idTipo FROM Usuario WHERE correo=@pCorreo and contrasena = @pContrasena";

        SqlCommand cmd = new SqlCommand(query,basedatos.getConnection());
        cmd.Parameters.AddWithValue("@pCorreo",pCorreo);
        cmd.Parameters.AddWithValue("@pContrasena",pContrasena);
        int tipo=0;
        
        using (SqlDataReader dr = cmd.ExecuteReader()){
            while(dr.Read()){
                tipo= Int32.Parse(dr["idTipo"].ToString());
            }
        }
        basedatos.getConnection().Close();
        return tipo;        
    }

    public async Task<ClaimsIdentity> sesionUsuario(string pCorreo, string pContrasena){
        int result = validacion(pCorreo,pContrasena);
        Console.WriteLine("Es un usuario de tipo: "+result);
        if (result==0){
            return null;
        }
        else if (result==1 || result==2){
            Console.WriteLine("Retorno de claims para profesor");
            ProfesorGuia profe = getProfesorCorreo(pCorreo);
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, profe.codigo.ToString()),
                    new Claim(ClaimTypes.Email, profe.correoElectronico)                    
            };
            if(result==1)
                claims.Add(new Claim(ClaimTypes.Role, "Profesor"));
            else
                claims.Add(new Claim(ClaimTypes.Role, "ProfesorCoordinador"));
            var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            return claimsIdentity;
        }
        else if (result==3 || result==4){
            Coordinador coord = getCoordinadorCorreo(pCorreo);
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, coord.id.ToString()),
                    new Claim(ClaimTypes.Email, coord.correoElectronico)                    
            };
            if(result==1)
                claims.Add(new Claim(ClaimTypes.Role, "Profesor"));
            else
                claims.Add(new Claim(ClaimTypes.Role, "ProfesorCoordinador"));
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            return claimsIdentity;
            
        }
        return null;
    }
    
    public bool cambiarContrasena(String correo, String contrasena)
    {
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        string query= "UPDATE Usuario SET contrasena = @pContrasena WHERE correo=@pCorreo";
        SqlCommand command = new SqlCommand(query, basedatos.getConnection());
        command.CommandType = System.Data.CommandType.Text;

        command.Parameters.AddWithValue("@pCorreo",correo);
        command.Parameters.AddWithValue("@pContrasena",contrasena);
        
        try
        {
            command.ExecuteNonQuery();
            basedatos.getConnection().Close();
            return true;
            
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error al cambiar la contrasena: " + ex.Message);
            basedatos.getConnection().Close();
            return false;
        }  
    }
    

    //

    public ProfesorGuia getProfesorCorreo(string pCorreo){
        SingletonDB basedatos = SingletonDB.getInstance();
        basedatos.getConnection().Open();
        ProfesorGuia objeto = new ProfesorGuia();

        string query= "SELECT codigo,nombreCompleto,correoElectronico,telefonoOficina, telefonoCelular, fotografia, activo FROM Profesor WHERE correoElectronico=@pCorreo";

        SqlCommand cmd = new SqlCommand(query,basedatos.getConnection());
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@pCorreo",pCorreo);
        
        using (SqlDataReader dr = cmd.ExecuteReader()){
            while(dr.Read()){
                objeto.codigo=dr["codigo"].ToString();
                objeto.nombreCompleto=dr["nombreCompleto"].ToString();
                objeto.correoElectronico=dr["correoElectronico"].ToString();
                objeto.telefonoOficina=dr["telefonoOficina"].ToString();
                objeto.telefonoCelular=dr["telefonoCelular"].ToString();
                objeto.fotografia=dr["fotografia"].ToString();
                if (Convert.ToBoolean((dr["activo"].ToString())))
                        objeto.activo="Activo";
                    else
                        objeto.activo="Inactivo";
            }
        }
        basedatos.getConnection().Close();
        return objeto;
    }

    public Coordinador getCoordinadorCorreo(string pCorreo){
        SingletonDB basedatos = SingletonDB.getInstance();
        basedatos.getConnection().Open();
        Coordinador objeto = new Coordinador();

        string query= "SELECT id,nombreCompleto,correoElectronico FROM Coordinador WHERE correo=@pCorreo";

        SqlCommand cmd = new SqlCommand(query,basedatos.getConnection());
        cmd.Parameters.AddWithValue("@pCorreo",pCorreo);
        
        using (SqlDataReader dr = cmd.ExecuteReader()){
            while(dr.Read()){
                objeto.id=dr["id"].ToString();
                objeto.nombreCompleto=dr["nombreCompleto"].ToString();
                objeto.correoElectronico=dr["correoElectronico"].ToString();
            }
        }
        basedatos.getConnection().Close();
        return objeto;
    }


    public int getCountComentarios(){
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("SELECT COUNT(idComentario) FROM Comentario", basedatos.getConnection());
        //SqlDataReader readerComentarios = command.ExecuteReader();
    
            int count = (int)command.ExecuteScalar();

        return count;
    }
    public int getRespuestas(){
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("SELECT COUNT(idRespuesta) FROM Respuesta", basedatos.getConnection());
        //SqlDataReader readerComentarios = command.ExecuteReader();
    
            int count = (int)command.ExecuteScalar();

        return count;
    }
    public bool InsertarRespuesta(Respuesta r, int idComentario)
    {   
        //se supone que el profesor ya esta ingresado en la tablas profesores
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("insertar_respuesta", basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@respuesta_id", r.idRespuesta);
            command.Parameters.AddWithValue("@profesor_guia_id", r.emisor.codigo);
            command.Parameters.AddWithValue("@fecha_hora", r.fechaHora);
            command.Parameters.AddWithValue("@cuerpo_respuesta", r.cuerpo);
            command.Parameters.AddWithValue("@comentario_id", idComentario);
        try
        {
            command.ExecuteNonQuery();
            basedatos.getConnection().Close();
            return true;
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
            basedatos.getConnection().Close();
            return false;
        }       
    }
    public bool InsertarComentario(Comentario c, int idActividad)
    {   
        //se supone que el profesor ya esta ingresado en la tablas profesores
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("insertar_comentario", basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@profesor_guia_id", c.emisor.codigo);
            command.Parameters.AddWithValue("@idActividad", idActividad);
            command.Parameters.AddWithValue("@fecha_hora", c.fechaHora);
            command.Parameters.AddWithValue("@comentario_cuerpo", c.cuerpo);
           
        try
        {
            command.ExecuteNonQuery();
            basedatos.getConnection().Close();

            foreach (Respuesta r in c.listaRespuestas){
                InsertarRespuesta(r,c.idComentario);

            }
            return true;
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
            basedatos.getConnection().Close();
            return false;
        }       
    }
   public bool InsertarResponsables(ProfesorGuia p, int idActividad)
    {
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("insertar_ActividadXresponsables", basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@profesor_guia_id", p.codigo);
            command.Parameters.AddWithValue("@actividad_id", idActividad);
           
        try
        {
            command.ExecuteNonQuery();
            basedatos.getConnection().Close();
            return true;
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
            basedatos.getConnection().Close();
            return false;
        }       
    }
    public bool InsertarRecordatorios (DateTime r, int idActividad)
    {
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("insertar_recordatorio", basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@fecha_recordatorio", r);
            command.Parameters.AddWithValue("@actividad_id", idActividad);
        try
        {
            command.ExecuteNonQuery();
            basedatos.getConnection().Close();
            return true;
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
            basedatos.getConnection().Close();
            return false;
        }       
    }
    public bool InsertarActividad(Actividad a)
    {
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("insertar_actividad", basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;
            
            
            command.Parameters.AddWithValue("@id_actividad", a.idActividad);
            command.Parameters.AddWithValue("@semana_num", a.semana);
            command.Parameters.AddWithValue("@tipo_actividad", a.tipo.ToString());
            command.Parameters.AddWithValue("@nombre", a.nombre);
            
            command.Parameters.AddWithValue("@fecha_hora", a.fechaHora);
            
            command.Parameters.AddWithValue("@fecha_anuncio", a.fechaAnuncio);
            command.Parameters.AddWithValue("@dias_previos_anuncio", a.diasPreviosAnuncio);
            command.Parameters.AddWithValue("@modalidad", a.modalidad.ToString());
            command.Parameters.AddWithValue("@enlace_remoto", a.enlaceRemoto);
            command.Parameters.AddWithValue("@afiche_bytea", new byte[0]); // Se asume que el afiche es una imagen convertida a byte array
            command.Parameters.AddWithValue("@estado_actividad", a.estado.ToString());
            
        try
        {
            command.ExecuteNonQuery();
            basedatos.getConnection().Close();
            foreach (ProfesorGuia p in a.responsables){
                InsertarResponsables(p,a.idActividad);
            }
            foreach (DateTime r in a.recordatorios){
                InsertarRecordatorios(r,a.idActividad);
            }
            foreach (Comentario c in a.listaComentarios){
                InsertarComentario(c,a.idActividad);
            }
            return true;
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
            basedatos.getConnection().Close();
            return false;
        }       
    }
    public CentroAcademico getCentro(int idCentro){
        SingletonDB basedatos = SingletonDB.getInstance();
        CentroAcademico centroAcademico = new CentroAcademico();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }        
        SqlCommand command = new SqlCommand("consultar_centroAcademico" ,basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@in_idCentroAcademico", idCentro);

        using (SqlDataReader reader = command.ExecuteReader()){
        if (reader.Read()){
        
        centroAcademico.idCentro = Convert.ToInt32(reader["idCentroAcademico"]);

        centroAcademico.siglas = (SiglasCentros)Enum.Parse(typeof(SiglasCentros),reader["Siglas"].ToString()); 
        centroAcademico.nombre = reader["nombre"].ToString();
        centroAcademico.cantidadProfesores = Convert.ToInt32(reader["cantProfesores"]);
        }}
        return centroAcademico;
    }
    public List<Estudiante> getEstudiantesCentro(int idCentro){
        List<Estudiante> estudiantes = new List<Estudiante>(); 
        estudiantes = getEstudiantes();
        List<Estudiante> estudiantesFiltrados = estudiantes.Where(e => e.centroEstudio.idCentro == idCentro).ToList();
        return estudiantesFiltrados;
    }
    public Actividad getActividadXid(int id)
    {
     Actividad actividad = new Actividad();
     SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("consultar_actividades_xid", basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@id_actividad", id);

        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            
            actividad.idActividad = (int)reader["idActividad"];
            actividad.semana = (int)reader["semana"];
            actividad.nombre = (string)reader["nombre"];
            actividad.fechaHora = (DateTime)reader["fechaHora"];
            actividad.fechaAnuncio = (DateTime)reader["fechaAnuncio"];
            actividad.diasPreviosAnuncio = (int)reader["diasPreviosAnuncio"];
            actividad.enlaceRemoto = (string)reader["enlaceRemoto"];
            actividad.afiche = reader["afiche"].ToString();
            actividad.estado = (EstadoActividad)Enum.Parse(typeof(EstadoActividad),(string)reader["EstadoActividad"]);
            actividad.modalidad = (Modalidad)Enum.Parse(typeof(Modalidad),(string)reader["Modalidad"]);
            actividad.tipo = (TipoActividad)Enum.Parse(typeof(TipoActividad),(string)reader["TipoActividad"]);
            actividad.responsables = new List<ProfesorGuia>();
            actividad.recordatorios = new List<DateTime>();
            actividad.listaComentarios = new List<Comentario>();
            SqlCommand commandResponsables = new SqlCommand("consultar_ActividadXresponsables", basedatos.getConnection());
            commandResponsables.CommandType = System.Data.CommandType.StoredProcedure;
            commandResponsables.Parameters.AddWithValue("@idActividad", id);
            SqlDataReader readerResponsables = commandResponsables.ExecuteReader();
            

            while (readerResponsables.Read()){
                ProfesorGuia profesor = new ProfesorGuia();
                profesor.codigo = readerResponsables["codigo"].ToString();
                profesor.nombreCompleto = readerResponsables["nombrecompleto"].ToString();
                profesor.correoElectronico = readerResponsables["correoelectronico"].ToString();
                profesor.telefonoOficina = readerResponsables["telefonooficina"].ToString();
                profesor.telefonoCelular = readerResponsables["telefonocelular"].ToString();
                profesor.fotografia = (byte[])readerResponsables["fotografia"];
                profesor.activo = readerResponsables["activo"].ToString();
                    
                actividad.responsables.Add(profesor);
                
            }
            readerResponsables.Close();

            SqlCommand commandRecordatorios = new SqlCommand("consultar_recordatorio", basedatos.getConnection());
            commandRecordatorios.CommandType = System.Data.CommandType.StoredProcedure;
            commandRecordatorios.Parameters.AddWithValue("@idActividad", id);

            SqlDataReader readerRecordatorios = commandRecordatorios.ExecuteReader();
            while (readerRecordatorios.Read()){

                actividad.recordatorios.Add((DateTime)readerRecordatorios["fechaRecordatorio"]);
            }
            readerRecordatorios.Close();

            SqlCommand commandComentarios = new SqlCommand("consultar_comentarios", basedatos.getConnection());
            commandComentarios.CommandType = System.Data.CommandType.StoredProcedure;
            commandComentarios.Parameters.AddWithValue("@idActividad", id);

            SqlDataReader readerComentarios = commandComentarios.ExecuteReader();
        
            while (readerComentarios.Read()){
                Comentario c = new Comentario();
                c.listaRespuestas = new List<Respuesta>();
                c.idComentario = (Int32)readerComentarios["idComentario"];
                   

                c.emisor = getProfesorXcodigo((String)readerComentarios["idProfesorGuia"]);
            
                c.cuerpo = (String)readerComentarios["cuerpo"];
                c.fechaHora = (DateTime)readerComentarios["fechaHora"];

                SqlCommand commandRespuestas = new SqlCommand("consultar_respuestas", basedatos.getConnection());
                commandRespuestas.CommandType = System.Data.CommandType.StoredProcedure;
                commandRespuestas.Parameters.AddWithValue("@id_comentario", id);

                SqlDataReader readerRespuestas = commandRespuestas.ExecuteReader();
                while (readerRespuestas.Read()){
                    Respuesta r = new Respuesta(); 
                    r.idRespuesta = (Int32)readerRespuestas["idRespuesta"];
                    r.fechaHora = (DateTime)readerRespuestas["fechaHora"];
                    r.cuerpo = (String)readerRespuestas["cuerpo"];
                      
                    r.emisor = getProfesorXcodigo((String)readerRespuestas["idProfesorGuia"]);
                 
                    c.listaRespuestas.Add(r);  
                    
                }

                actividad.listaComentarios.Add(c);
            }

        }

        reader.Close();
        basedatos.getConnection().Close();
        return actividad;
    } 
    public List<Actividad> getActividades()
   {
    List<Actividad> actividades = new List<Actividad>();

     SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("consultar_actividades", basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;

        SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Actividad actividad = new Actividad();
            actividad.idActividad = (int)reader["idActividad"];
            actividad.semana = (int)reader["semana"];
            actividad.nombre = (string)reader["nombre"];
            actividad.fechaHora = (DateTime)reader["fechaHora"];
            actividad.fechaAnuncio = (DateTime)reader["fechaAnuncio"];
            actividad.diasPreviosAnuncio = (int)reader["diasPreviosAnuncio"];
            actividad.enlaceRemoto = (string)reader["enlaceRemoto"];
            actividad.afiche = reader["afiche"].ToString();
            actividad.estado = (EstadoActividad)Enum.Parse(typeof(EstadoActividad),(string)reader["EstadoActividad"]);
            actividad.modalidad = (Modalidad)Enum.Parse(typeof(Modalidad),(string)reader["Modalidad"]);
            actividad.tipo = (TipoActividad)Enum.Parse(typeof(TipoActividad),(string)reader["TipoActividad"]);
            actividad.responsables = new List<ProfesorGuia>();
            actividad.recordatorios = new List<DateTime>();
            actividad.listaComentarios = new List<Comentario>();
            SqlCommand commandResponsables = new SqlCommand("consultar_ActividadXresponsables", basedatos.getConnection());
            commandResponsables.CommandType = System.Data.CommandType.StoredProcedure;
            commandResponsables.Parameters.AddWithValue("@idActividad", actividad.idActividad);
            SqlDataReader readerResponsables = commandResponsables.ExecuteReader();
            

            while (readerResponsables.Read()){
                ProfesorGuia profesor = new ProfesorGuia();
                profesor.codigo = readerResponsables["codigo"].ToString();
                profesor.nombreCompleto = readerResponsables["nombrecompleto"].ToString();
                profesor.correoElectronico = readerResponsables["correoelectronico"].ToString();
                profesor.telefonoOficina = readerResponsables["telefonooficina"].ToString();
                profesor.telefonoCelular = readerResponsables["telefonocelular"].ToString();
                profesor.fotografia = (byte[])readerResponsables["fotografia"];
                profesor.activo = readerResponsables["activo"].ToString();
                    
                actividad.responsables.Add(profesor);
                
            }
            readerResponsables.Close();

            SqlCommand commandRecordatorios = new SqlCommand("consultar_recordatorio", basedatos.getConnection());
            commandRecordatorios.CommandType = System.Data.CommandType.StoredProcedure;
            commandRecordatorios.Parameters.AddWithValue("@idActividad", actividad.idActividad);

            SqlDataReader readerRecordatorios = commandRecordatorios.ExecuteReader();
            while (readerRecordatorios.Read()){

                actividad.recordatorios.Add((DateTime)readerRecordatorios["fechaRecordatorio"]);
            }
            readerRecordatorios.Close();

            SqlCommand commandComentarios = new SqlCommand("consultar_comentarios", basedatos.getConnection());
            commandComentarios.CommandType = System.Data.CommandType.StoredProcedure;
            commandComentarios.Parameters.AddWithValue("@idActividad", actividad.idActividad);

            SqlDataReader readerComentarios = commandComentarios.ExecuteReader();
        
            while (readerComentarios.Read()){
                Comentario c = new Comentario();
                c.listaRespuestas = new List<Respuesta>();
                c.idComentario = (Int32)readerComentarios["idComentario"];
                   

                c.emisor = getProfesorXcodigo((String)readerComentarios["idProfesorGuia"]);
       
                c.cuerpo = (String)readerComentarios["cuerpo"];
                c.fechaHora = (DateTime)readerComentarios["fechaHora"];

                SqlCommand commandRespuestas = new SqlCommand("consultar_respuestas", basedatos.getConnection());
                commandRespuestas.CommandType = System.Data.CommandType.StoredProcedure;
                commandRespuestas.Parameters.AddWithValue("@id_comentario", c.idComentario);

                SqlDataReader readerRespuestas = commandRespuestas.ExecuteReader();
                while (readerRespuestas.Read()){
                    Respuesta r = new Respuesta(); 
                    r.idRespuesta = (Int32)readerRespuestas["idRespuesta"];
                    r.fechaHora = (DateTime)readerRespuestas["fechaHora"];
                    r.cuerpo = (String)readerRespuestas["cuerpo"];
                      
                    r.emisor = getProfesorXcodigo((String)readerRespuestas["idProfesorGuia"]);
                   
                    c.listaRespuestas.Add(r);  
                    
                }

                actividad.listaComentarios.Add(c);
            }

            actividades.Add(actividad);
        }

        reader.Close();
        basedatos.getConnection().Close();
        return actividades;
    } 
   
    public bool insertarProfesor(ProfesorGuia p)
    {
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("insertar_profesor", basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@in_codigo", p.codigo);
            command.Parameters.AddWithValue("@in_nombreCompleto", p.nombreCompleto);
            command.Parameters.AddWithValue("@in_correoElectronico", p.correoElectronico);
            command.Parameters.AddWithValue("@in_telefonoOficina", p.telefonoOficina);
            command.Parameters.AddWithValue("@in_telefonoCelular", p.telefonoCelular);
            command.Parameters.AddWithValue("@in_fotografia", p.fotografia);
        
            command.Parameters.AddWithValue("@in_activo", p.activo);
        try
        {
                    
            
            command.ExecuteNonQuery();
            basedatos.getConnection().Close();
            return true;
            
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
            basedatos.getConnection().Close();
            return false;
        }  
    }
    public bool modificarProfesor(ProfesorGuia p)
    {
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("actualizar_profesor", basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@in_codigo", p.codigo);
            command.Parameters.AddWithValue("@in_nombreCompleto", p.nombreCompleto);
            command.Parameters.AddWithValue("@in_correoElectronico", p.correoElectronico);
            command.Parameters.AddWithValue("@in_telefonoOficina", p.telefonoOficina);
            command.Parameters.AddWithValue("@in_telefonoCelular", p.telefonoCelular);
            command.Parameters.AddWithValue("@in_fotografia", p.fotografia);
        
            command.Parameters.AddWithValue("@in_activo", p.activo);
        try
        {
            command.ExecuteNonQuery();
            basedatos.getConnection().Close();
            return true;
            
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
            basedatos.getConnection().Close();
            return false;
        }  
    }
    
    public bool insertarCentroAcademico(CentroAcademico c)
    {
        
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }        

        
        string storedProcedure = "insertar_centroAcademico";
        SqlCommand commandCentro = new SqlCommand(storedProcedure ,basedatos.getConnection());
        commandCentro.CommandType = System.Data.CommandType.StoredProcedure;

            //commandCentro.CommandType = CommandType.StoredProcedure;
            commandCentro.Parameters.AddWithValue("@in_idCentroAcademico", c.idCentro);
            commandCentro.Parameters.AddWithValue("@in_Siglas", c.siglas.ToString());
            commandCentro.Parameters.AddWithValue("@in_nombre", c.nombre);
            commandCentro.Parameters.AddWithValue("@in_cantProfesores", c.cantidadProfesores);
           try
        {
                    
            commandCentro.ExecuteNonQuery();
            basedatos.getConnection().Close();
            return true;
            
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
            basedatos.getConnection().Close();
            return false;
        }
             
    }
    public bool insertarEstudiante(Estudiante e)
    {
        this.insertarCentroAcademico(e.centroEstudio);
        SingletonDB basedatos = SingletonDB.getInstance();
        
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }        

        
        string storedProcedure = "insertar_estudiante";
        SqlCommand command = new SqlCommand(storedProcedure ,basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;

            
            command.Parameters.AddWithValue("@in_idEstudiante", e.idEstudiante);
            command.Parameters.AddWithValue("@in_carne", e.carne);
            command.Parameters.AddWithValue("@in_nombreCompleto", e.nombreCompleto);
            command.Parameters.AddWithValue("@in_correoElectronico", e.correoElectronico);
            command.Parameters.AddWithValue("@in_telefonoCelular", e.telefonoCelular);
            command.Parameters.AddWithValue("@in_idCentroAcademico", e.centroEstudio.idCentro);
            
            
        try
        {     
            command.ExecuteNonQuery();
            basedatos.getConnection().Close();
            return true;
            
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
            basedatos.getConnection().Close();
            return false;
        }
             
    }
    public bool modificarEstudiante(Estudiante e)
    {
        this.insertarCentroAcademico(e.centroEstudio);
        SingletonDB basedatos = SingletonDB.getInstance();
        
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }        

        
        string storedProcedure = "actualizar_estudiante";
        SqlCommand command = new SqlCommand(storedProcedure ,basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;

            
            command.Parameters.AddWithValue("@in_idEstudiante", e.idEstudiante);
            command.Parameters.AddWithValue("@in_carne", e.carne);
            command.Parameters.AddWithValue("@in_nombreCompleto", e.nombreCompleto);
            command.Parameters.AddWithValue("@in_correoElectronico", e.correoElectronico);
            command.Parameters.AddWithValue("@in_telefonoCelular", e.telefonoCelular);
            command.Parameters.AddWithValue("@in_idCentroAcademico", e.centroEstudio.idCentro);
            
            
        try
        {     
            command.ExecuteNonQuery();
            basedatos.getConnection().Close();
            return true;
            
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
            basedatos.getConnection().Close();
            return false;
        }
             
    }
    public void insertarEquipoGuia(EquipoGuia eq)
{
    
    string storedProcedure = "insertar_equipoGuia";
    SingletonDB basedatos = SingletonDB.getInstance();
    if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        } 

    
    
    SqlCommand command = new SqlCommand(storedProcedure, basedatos.getConnection());
    command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@in_idEquipoGuia", eq.idEquipoGuia);
        command.Parameters.AddWithValue("@in_anno", eq.anno);
        command.Parameters.AddWithValue("@in_idProfesorCoordinador", eq.profesorCoordinador.codigo);
        command.Parameters.AddWithValue("@in_ultimaModificacion", eq.ultimaModificacion);

        try
        {
        
            command.ExecuteNonQuery();
            Console.WriteLine("Equipo Guía insertado correctamente.");
            basedatos.getConnection().Close();
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Número de error para violación de clave primaria duplicada
            {
                Console.WriteLine("Error: ID duplicado. No se insertó el registro.");
            }
            else
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
            }
             basedatos.getConnection().Close();
        }
    }
    public List<ProfesorGuia> getTodosProfesores(){

    List<ProfesorGuia> profesores = new List<ProfesorGuia>();
    
    SingletonDB basedatos = SingletonDB.getInstance();
    if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }
        SqlCommand command = new SqlCommand("consultar_profesor", basedatos.getConnection());
        command.CommandType = System.Data.CommandType.StoredProcedure;
        
        
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read()){
            ProfesorGuia profesor = new ProfesorGuia();
            
                profesor.codigo = reader["codigo"].ToString();
                profesor.nombreCompleto = reader["nombrecompleto"].ToString();
                profesor.correoElectronico = reader["correoelectronico"].ToString();
                profesor.telefonoOficina = reader["telefonooficina"].ToString();
                profesor.telefonoCelular = reader["telefonocelular"].ToString();
                profesor.fotografia = (byte[])reader["fotografia"];
                profesor.activo = reader["activo"].ToString();

            
            profesores.Add(profesor);
        }
    }
    basedatos.getConnection().Close();
    return profesores;
    }
    public ProfesorGuia getProfesorPorcodigo(String codigo){

        ProfesorGuia profesor = new ProfesorGuia();
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }

            SqlCommand command = new SqlCommand("consultar_profesor_codigo", basedatos.getConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@codigo_Profesor",codigo);
            
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read()){
                
                    profesor.codigo = reader["codigo"].ToString();
                    profesor.nombreCompleto = reader["nombrecompleto"].ToString();
                    profesor.correoElectronico = reader["correoelectronico"].ToString();
                    profesor.telefonoOficina = reader["telefonooficina"].ToString();
                    profesor.telefonoCelular = reader["telefonocelular"].ToString();
                    profesor.fotografia = (byte[])reader["fotografia"];
                    profesor.activo = reader["activo"].ToString();
                    }
            }
        basedatos.getConnection().Close();
        return profesor;
    }
    private ProfesorGuia getProfesorXcodigo(String codigo){

        ProfesorGuia profesor = new ProfesorGuia();
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }

            SqlCommand command = new SqlCommand("consultar_profesor_codigo", basedatos.getConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@codigo_Profesor",codigo);
            
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read()){
                
                    profesor.codigo = reader["codigo"].ToString();
                    profesor.nombreCompleto = reader["nombrecompleto"].ToString();
                    profesor.correoElectronico = reader["correoelectronico"].ToString();
                    profesor.telefonoOficina = reader["telefonooficina"].ToString();
                    profesor.telefonoCelular = reader["telefonocelular"].ToString();
                    profesor.fotografia = (byte[])reader["fotografia"];
                    profesor.activo = reader["activo"].ToString();
                    }
            }
  
        return profesor;
    }

   public List<Estudiante> getEstudiantes()
    {
        var estudiantes = new List<Estudiante>();
        SingletonDB basedatos = SingletonDB.getInstance();
        if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }        

        
        string storedProcedure = "consultar_estudiante";
        SqlCommand cmd = new SqlCommand(storedProcedure ,basedatos.getConnection());


        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Estudiante estudiante = new Estudiante();

                estudiante.idEstudiante = Convert.ToInt32(reader["idEstudiante"]);
                estudiante.carne = reader["carne"].ToString();
                estudiante.nombreCompleto = reader["nombreCompleto"].ToString();
                estudiante.correoElectronico = reader["correoElectronico"].ToString();
                estudiante.telefonoCelular = reader["telefonoCelular"].ToString();

                CentroAcademico centroAcademico = new CentroAcademico();
                centroAcademico.idCentro = Convert.ToInt32(reader["idCentroAcademico"]);

                centroAcademico.siglas = (SiglasCentros)Enum.Parse(typeof(SiglasCentros),reader["Siglas"].ToString()); 
                centroAcademico.nombre = reader["nombreCentroAcademico"].ToString();
                centroAcademico.cantidadProfesores = Convert.ToInt32(reader["cantProfesores"]);

                estudiante.centroEstudio = centroAcademico;

                estudiantes.Add(estudiante);
                
            }
                basedatos.getConnection().Close();
        }
            
        
       
        return estudiantes;
    }
    
    
    public List<EquipoGuia> getEquiposGuia()
    {
    var equiposGuia = new List<EquipoGuia>();
    SingletonDB basedatos = SingletonDB.getInstance();
    if (basedatos.IsConnectionOpen() == false){
            basedatos.getConnection().Open();
        }   
    string storedProcedure = "consultar_equipoGuia";

    SqlCommand command = new SqlCommand(storedProcedure, basedatos.getConnection());
    command.CommandType = System.Data.CommandType.StoredProcedure;

        try
        {
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                EquipoGuia equipoGuia = new EquipoGuia();
                equipoGuia.idEquipoGuia = Convert.ToInt32(reader["idEquipoGuia"]);
                equipoGuia.anno = Convert.ToInt32(reader["anno"]);
                equipoGuia.ultimaModificacion = reader["ultimaModificacion"].ToString();

                ProfesorGuia coordinador = new ProfesorGuia();
                coordinador.codigo = reader["idProfesorCoordinador"].ToString();
                coordinador.nombreCompleto = reader["nombreCompleto"].ToString();
                coordinador.correoElectronico = reader["correoElectronico"].ToString();
                coordinador.telefonoOficina = reader["telefonoOficina"].ToString();
                coordinador.telefonoCelular = reader["telefonoCelular"].ToString();
                coordinador.fotografia = (byte[])reader["fotografia"];
                coordinador.activo = reader["activo"].ToString();

                equipoGuia.profesorCoordinador= coordinador;

                equiposGuia.Add(equipoGuia);
            }

            reader.Close();
            basedatos.getConnection().Close();
            return equiposGuia;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error al obtener los equipos guía: " + ex.Message);
            basedatos.getConnection().Close();
            return equiposGuia;
            
        }
    }



}
