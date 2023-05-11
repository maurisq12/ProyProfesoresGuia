namespace ProfesoresGuia.Models;

public class Justificacion
{
    public int idJustificacion { get; set; }
    public int idActividad { get; set; }
    public String cuerpoJustificacion { get; set; }
    public DateTime fechaJustificacion { get; set; }


    public Justificacion(int idJustificacion, int idActividad, String cuerpoJustificacion, DateTime fechaJustificacion)
    {
        this.idJustificacion = idJustificacion;
        this.idActividad = idActividad;
        this.cuerpoJustificacion = cuerpoJustificacion;
        this.fechaJustificacion = fechaJustificacion;
    }
}