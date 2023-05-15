using System.Text;

namespace ProfesoresGuia.Models;

public class EquipoGuia
{
    public int idEquipoGuia { get; set; }
    public int anno { get; set; }
    public ProfesorGuia profesorCoordinador { get; set; }
    public List<ProfesorGuia> profesoresGuia { get; set; }
    public List<Estudiante> listaEstudiantes { get; set; }
    public String ultimaModificacion { get; set; }


    public EquipoGuia()
    {
    }

    public EquipoGuia(int idEquipoGuia, int anno, ProfesorGuia profesorCoordinador, List<ProfesorGuia> profesoresGuia, List<Estudiante> listaEstudiantes, string ultimaModificacion)
    {
        this.idEquipoGuia = idEquipoGuia;
        this.anno = anno;
        this.profesorCoordinador = profesorCoordinador;
        this.profesoresGuia = profesoresGuia;
        this.listaEstudiantes = listaEstudiantes;
        this.ultimaModificacion = ultimaModificacion;
    }

    public EquipoGuia(int idEquipoGuia, int anno, ProfesorGuia coordinador)
    {
        this.idEquipoGuia = idEquipoGuia;
        this.anno = anno;
        this.profesorCoordinador = coordinador;
        profesoresGuia = new List<ProfesorGuia>();
        listaEstudiantes = new List<Estudiante>();
        ultimaModificacion = "";
    }


    public void AddProfesorGuia(ProfesorGuia profe)
    {
        profesoresGuia.Add(profe);
    }

    public void RemoveProfesorGuia(ProfesorGuia profe)
    {
        profesoresGuia.Remove(profe);
    }

    public void AddEstudiante(Estudiante estudiante)
    {
        listaEstudiantes.Add(estudiante);
    }

    public void RemoveEstudiante(Estudiante estudiante)
    {
        listaEstudiantes.Remove(estudiante);
    }
    
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"ID Equipo de Guía: {idEquipoGuia}");
        sb.AppendLine($"Año: {anno}");
        sb.AppendLine($"Profesor Coordinador: {profesorCoordinador}");

        sb.AppendLine("Profesores Guía:");
        foreach (var profesor in profesoresGuia)
        {
            sb.AppendLine(profesor.ToString());
        }

        sb.AppendLine("Estudiantes:");
        foreach (var estudiante in listaEstudiantes)
        {
            sb.AppendLine(estudiante.ToString());
        }

        sb.AppendLine($"Última Modificación: {ultimaModificacion}");

        return sb.ToString();
    }
}