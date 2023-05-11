namespace ProfesoresGuia.Models;

public class PlanTrabajo
{
    public int idPlanTrabajo { get; set; }
    public EquipoGuia equipo { get; set; }
    public List<Actividad> itinerario { get; set; }


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
}