using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Data.SqlClient;
using ProfesoresGuia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

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