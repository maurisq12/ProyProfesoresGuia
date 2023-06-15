namespace ProfesoresGuia.Models;

public class Notificacion
{
    public int idNotificacion { get; set; }

    public int idBuzon { get; set; }

    public int emisor { get; set; }

    public String contenido { get; set; }

    public DateTime fecha { get; set; }

    public String estado { get; set;} 


    public Notificacion(){}
}