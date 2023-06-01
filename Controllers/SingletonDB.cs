using System;
using System.Data;
using System.Data.SqlClient;

namespace ProfesoresGuia.Controllers;

public class SingletonDB
{
    private static SqlConnection con;
    
    private static SingletonDB instance;

    private SingletonDB(){
        string connectionString = "Server=compuguias.mssql.somee.com;User Id=maurisq12cr_SQLLogin_1;Password=vmwxcnu5pt;Database=compuguias;MultipleActiveResultSets=true";
        con = new SqlConnection(connectionString);
    }

    public static SingletonDB getInstance(){
        if (instance==null){
            lock(typeof(SingletonDB))
            instance= new SingletonDB();
        } 
        return instance;
    }

    public SqlConnection getConnection(){
        return con;
    }
    
    
    private static void establecer()
    {
        
    }
    
    public bool IsConnectionOpen()
    {
       return con.State == ConnectionState.Open;
    }


}
