using System.Text;

namespace ProfesoresGuia.Models;

public class PlanTrabajo
{
    public int idPlanTrabajo { get; set; }
    public EquipoGuia equipo { get; set; }
    public List<Actividad> itinerario { get; set; }


    public PlanTrabajo()
    {
    }

    public PlanTrabajo(int idPlanTrabajo, EquipoGuia equipo, List<Actividad> itinerario)
    {
        this.idPlanTrabajo = idPlanTrabajo;
        this.equipo = equipo;
        this.itinerario = itinerario;
    }

    public PlanTrabajo(int idPlanTrabajo, EquipoGuia equipo)
    {
        this.idPlanTrabajo = idPlanTrabajo;
        this.equipo = equipo;
        itinerario = new List<Actividad>();
    }


    public void AddActividad(Actividad actividad)
    {
        itinerario.Add(actividad);
    }

    public void RemoveActividad(Actividad actividad)
    {
        itinerario.Remove(actividad);
    }
    
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"ID Plan de Trabajo: {idPlanTrabajo}");
        sb.AppendLine($"Equipo de Guía: {equipo}");

        sb.AppendLine("Itinerario:");
        foreach (var actividad in itinerario)
        {
            sb.AppendLine(actividad.ToString());
        }

        return sb.ToString();
    }
}