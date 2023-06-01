using System.Text;
using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class DTOActividad
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


    public DTOActividad()
    {
    }

    public DTOActividad(int idActividad, int semana, TipoActividad tipo, string nombre, DateTime fechaHora, List<ProfesorGuia> responsables, DateTime fechaAnuncio, int diasPreviosAnuncio, List<DateTime> recordatorios, Modalidad modalidad, string enlaceRemoto, string afiche, EstadoActividad estado)
    {
        this.idActividad = idActividad;
        this.semana = semana;
        this.tipo = tipo;
        this.nombre = nombre;
        this.fechaHora = fechaHora;
        this.responsables = responsables;
        this.fechaAnuncio = fechaAnuncio;
        this.diasPreviosAnuncio = diasPreviosAnuncio;
        this.recordatorios = recordatorios;
        this.modalidad = modalidad;
        this.enlaceRemoto = enlaceRemoto;
        this.afiche = afiche;
        this.estado = estado;
    }

    public DTOActividad(int idActividad, int semana, TipoActividad tipo, String nombre, DateTime fechaHora, DateTime fechaAnuncio, int diasPreviosAnuncio, Modalidad modalidad, String enlaceRemoto, String afiche, EstadoActividad estado)
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
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"ID DTOActividad: {idActividad}");
        sb.AppendLine($"Semana: {semana}");
        sb.AppendLine($"Tipo: {tipo}");
        sb.AppendLine($"Nombre: {nombre}");
        sb.AppendLine($"Fecha y hora: {fechaHora}");
        sb.AppendLine("Responsables:");
        foreach (var responsable in responsables)
        {
            sb.AppendLine($"- {responsable}");
        }
        sb.AppendLine($"Fecha de anuncio: {fechaAnuncio}");
        sb.AppendLine($"Días previos de anuncio: {diasPreviosAnuncio}");
        sb.AppendLine("Recordatorios:");
        foreach (var recordatorio in recordatorios)
        {
            sb.AppendLine($"- {recordatorio}");
        }
        sb.AppendLine($"Modalidad: {modalidad}");
        sb.AppendLine($"Enlace remoto: {enlaceRemoto}");
        sb.AppendLine($"Afiche: {afiche}");
        sb.AppendLine($"Estado: {estado}");
        return sb.ToString();
    }
}
