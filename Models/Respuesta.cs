namespace ProfesoresGuia.Models;

public class Respuesta
{
    public int idRespuesta { get; set; }
    public ProfesorGuia emisor { get; set; }
    public DateTime fechaHora { get; set; }
    public string cuerpo { get; set; }


    public Respuesta(int idRespuesta, ProfesorGuia emisor, DateTime fechaHora, String cuerpo)
    {
        this.idRespuesta = idRespuesta;
        this.emisor = emisor;
        this.fechaHora = fechaHora;
        this.cuerpo = cuerpo;
    }
}