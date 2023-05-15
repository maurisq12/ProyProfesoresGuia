
namespace ProfesoresGuia.Models;

public class Actividad
{
    public int idActividad { get; set; }
    public int semana { get; set; }
    public TipoActividad tipo { get; set; }
    public String nombre { get; set; }
    public DateTime fechaHora { get; set; }
    public List<ProfesorGuia> responsables { get; set; }
    public DateTime fechaAnuncio { get; set; }
    public int diasPreviosAnuncio { get; set; }
    public List<DateTime> recordatorios { get; set; }
    public Modalidad modalidad { get; set; }
    public String enlaceRemoto { get; set; }
    public String afiche { get; set; }
    public EstadoActividad estado { get; set; }
    public List<Comentario> listaComentarios { get; set; }

    
    
    public Actividad(int idActividad, int semana, TipoActividad tipo, String nombre, DateTime fechaHora, DateTime fechaAnuncio, int diasPreviosAnuncio, Modalidad modalidad, String enlaceRemoto, String afiche, EstadoActividad estado)
    {
        this.idActividad = idActividad;
        this.semana = semana;
        this.tipo = tipo;
        this.nombre = nombre;
        this.fechaHora = fechaHora;
        responsables = new List<ProfesorGuia>();
        this.fechaAnuncio = fechaAnuncio;
        this.diasPreviosAnuncio = diasPreviosAnuncio;
        recordatorios = new List<DateTime>();
        this.modalidad = modalidad;
        this.enlaceRemoto = enlaceRemoto;
        this.afiche = afiche;
        this.estado = estado;
        listaComentarios = new List<Comentario>();
    }
    

    
    
    public void AddProfeGuia(ProfesorGuia profe)
    {
        responsables.Add(profe);
    }

    public void RemoveProfeGuia(ProfesorGuia profe)
    {
        responsables.Remove(profe);
    }
    
    public void AddRecordarotio(DateTime recordatorio)
    {
        recordatorios.Add(recordatorio);
    }

    public void RemoveRecordarotio(DateTime recordatorio)
    {
        recordatorios.Remove(recordatorio);
    }
    
    public void AddComentario(Comentario comentario)
    {
        listaComentarios.Add(comentario);
    }

    public void RemoveComentario(Comentario comentario)
    {
        listaComentarios.Remove(comentario);
    }
}