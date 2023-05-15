namespace ProfesoresGuia.Models;

public class EquipoGuia
{
    public int idEquipoGuia { get; set; }
    public int anno { get; set; }
    public ProfesorGuia profesorCoordinador { get; set; }
    public List<ProfesorGuia> profesoresGuia { get; set; }
    public List<Estudiante> listaEstudiantes { get; set; }
    public String ultimaModificacion { get; set; }


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
}