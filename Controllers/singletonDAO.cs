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


     public bool insertarProfesor(ProfesorGuia p)
    {
        SingletonDB basedatos = SingletonDB.getInstance();
        basedatos.getConnection().Open();
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
    
    
    public bool insertarCentroAcademico(CentroAcademico c)
    {
        
        SingletonDB basedatos = SingletonDB.getInstance();
        basedatos.getConnection().Open();        

        
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
        
        basedatos.getConnection().Open();        

        
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
    public void insertarEquipoGuia(EquipoGuia eq)
    {
    
    string storedProcedure = "insertar_equipoGuia";
    SingletonDB basedatos = SingletonDB.getInstance();
    basedatos.getConnection().Open(); 

    
    
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
    basedatos.getConnection().Open();
        SqlCommand command = new SqlCommand("consultar_profesor", basedatos.getConnection());
        //command.CommandType = CommandType.StoredProcedure;
        
        
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

   public List<Estudiante> getEstudiantes()
    {
        var estudiantes = new List<Estudiante>();
        SingletonDB basedatos = SingletonDB.getInstance();
        basedatos.getConnection().Open();        

        
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
    basedatos.getConnection().Open();   
    string storedProcedure = "consultar_equipoGuia";

    SqlCommand command = new SqlCommand(storedProcedure, basedatos.getConnection());
    //command.CommandType = CommandType.StoredProcedure;

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
