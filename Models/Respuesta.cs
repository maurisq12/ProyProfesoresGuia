namespace ProfesoresGuia.Models;

public class Respuesta
{
    public int idRespuesta { get; set; }
    public int idComentario { get; set; }
    public ProfesorGuia emisor { get; set; }
    public DateTime fechaHora { get; set; }
    public string cuerpo { get; set; }


    public Respuesta()
    {
    }

    public Respuesta(int idRespuesta, int idComentario, ProfesorGuia emisor, DateTime fechaHora, String cuerpo)
    {
        this.idRespuesta = idRespuesta;
        this.idComentario = idComentario;
        this.emisor = emisor;
        this.fechaHora = fechaHora;
        this.cuerpo = cuerpo;
    }
    
    public override string ToString()
    {
        return $"ID Respuesta: {idRespuesta}\nID Comentario: {idComentario}\nEmisor: {emisor}\nFecha y Hora: {fechaHora}\nCuerpo: {cuerpo}";
    }
}