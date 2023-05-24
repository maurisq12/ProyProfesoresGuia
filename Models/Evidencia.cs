using System.Text;

namespace ProfesoresGuia.Models;

public class Evidencia
{
    public int idEvidencia { get; set; }
    public int idActividad { get; set; }
    public List<Imagen> imagenes { get; set; }
    public String listaAsistencia { get; set; }
    public String linkGrabacion { get; set; }


    public Evidencia()
    {
    }
    

    public Evidencia(int idEvidencia, int idActividad, List<Imagen> imagenes, String listaAsistencia, String linkGrabacion)
    {
        this.idEvidencia = idEvidencia;
        this.idActividad = idActividad;
        this.imagenes = imagenes;
        this.listaAsistencia = listaAsistencia;
        this.linkGrabacion = linkGrabacion;
    }



    public void AddImagen(Imagen imagen)
    {
        imagenes.Add(imagen);
    }

    public void RemoveImagen(Imagen imagen)
    {
        imagenes.Remove(imagen);
    }
    
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"ID Evidencia: {idEvidencia}");
        sb.AppendLine($"ID Actividad: {idActividad}");
        sb.AppendLine($"Lista de Asistencia: {listaAsistencia}");
        sb.AppendLine($"Link de Grabación: {linkGrabacion}");

        sb.AppendLine("Imágenes:");
        foreach (var imagen in imagenes)
        {
            sb.AppendLine(imagen.ToString());
        }

        return sb.ToString();
    }
}