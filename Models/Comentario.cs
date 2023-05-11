namespace ProfesoresGuia.Models;

public class Comentario
{
    public int idComentario { get; set; }
    public ProfesorGuia emisor { get; set; }
    public DateTime fechaHora { get; set; }
    public String cuerpo { get; set; }
    public List<Respuesta> listaRespuestas { get; set; }


    public Comentario(int idComentario, ProfesorGuia emisor, DateTime fechaHora, String cuerpo)
    {
        this.idComentario = idComentario;
        this.emisor = emisor;
        this.fechaHora = fechaHora;
        this.cuerpo = cuerpo;
        listaRespuestas = new List<Respuesta>();
    }


    public void AddRespuesta(Respuesta respuesta)
    {
        listaRespuestas.Add(respuesta);
    }

    public void RemoveRespuesta(Respuesta respuesta)
    {
        listaRespuestas.Remove(respuesta);
    }
}