using System;
using System.Data.SqlClient;
using ProfesoresGuia.Models;

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
    
    
    public void getVersion()
    {
        SingletonDB basedatos = SingletonDB.getInstance();

        basedatos.getConnection().Open();
    

        string sql = "SELECT version()";

        SqlCommand cmd = new SqlCommand(sql, basedatos.getConnection());

        var version = cmd.ExecuteScalar().ToString();
        Console.WriteLine($"PostgreSQL version: {version}");

        basedatos.getConnection().Close();
    }

    public int validacion(string pCorreo, string pContrasena){
        SingletonDB basedatos = SingletonDB.getInstance();
        basedatos.getConnection().Open();

        string query= "SELECT id FROM Usuario WHERE correo=@pCorreo and contrasena = @pContrasena";

        SqlCommand cmd = new SqlCommand(query,basedatos.getConnection());
        cmd.Parameters.AddWithValue("@pCorreo",pCorreo);
        cmd.Parameters.AddWithValue("@pContrasena",pContrasena);
        int tipo=0;
        
        using (SqlDataReader dr = cmd.ExecuteReader()){
            while(dr.Read()){
                tipo= Int32.Parse(dr["id"].ToString());
            }
        }
        if (tipo==0){
            basedatos.getConnection().Close();
            return 0;
        }
        basedatos.getConnection().Close();
        return tipo;        
    }

    public ProfesorGuia getProfesorCorreo(string pCorreo){
        SingletonDB basedatos = SingletonDB.getInstance();
        basedatos.getConnection().Open();
        ProfesorGuia objeto = new ProfesorGuia();

        string query= "SELECT codigo,nombreCompleto,correoElectronico,telefonoOficina, telefonoCelular, fotografia, activo FROM Profesor WHERE correoElectronico=@pCorreo";

        SqlCommand cmd = new SqlCommand(query,basedatos.getConnection());
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
        return objeto;
    }

    public bool sesionUsuario(string pCorreo, string pContrasena){
        int result = validacion(pCorreo,pContrasena);
        if (result==0){
            return false;
        }
        if (result==1 || result==2){
            ProfesorGuia profe = getProfesorCorreo(pCorreo);
            Console.WriteLine(profe.nombreCompleto);
        }
        if (result==3 || result==4){
            Coordinador coord = getCoordinadorCorreo(pCorreo);
        }
        return true;
    }




    public List<ProfesorGuia> getTodosProfesores(){
        var listaResultado = new List<ProfesorGuia>();
        SingletonDB basedatos = SingletonDB.getInstance();
        basedatos.getConnection().Open();        

        string query= "SELECT codigo, nombreCompleto,telefonoCelular, telefonoOficina, correoElectronico, activo FROM Profesor  ";        

        SqlCommand cmd = new SqlCommand(query,basedatos.getConnection());
        
        using (SqlDataReader dr = cmd.ExecuteReader()){
            while(dr.Read()){
                ProfesorGuia objeto = new ProfesorGuia(){
                    codigo=dr["codigo"].ToString(),
                    nombreCompleto= dr["nombreCompleto"].ToString(),
                    telefonoCelular= dr["telefonoCelular"].ToString(),
                    telefonoOficina= dr["telefonoOficina"].ToString(),
                    //fotografia= dr["fotografia"].ToString(),
                    correoElectronico=dr["correoElectronico"].ToString()};
                    if (Convert.ToBoolean((dr["activo"].ToString())))
                        objeto.activo="Activo";
                    else
                        objeto.activo="Inactivo";
                listaResultado.Add(objeto);
            }
        }
        basedatos.getConnection().Close();
        return listaResultado; 

    }






}