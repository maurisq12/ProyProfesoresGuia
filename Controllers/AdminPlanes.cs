using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdminPlanes
{
    public PlanTrabajo consultarPlan(int pPlan)
    {
        return SingletonDAO.getInstance().getPlan(pPlan);
    }

    public Actividad consultarProxActividad(int pPlan)
    {
        return SingletonDAO.getInstance().getProximaActividad(pPlan);
    }

    public bool agregarActividadPlan(PlanTrabajo pId, DTOActividad pActividad)
    {
        return SingletonDAO.getInstance().agregarActividadPlan(pId, pActividad);
    }

    public bool activarPublicacion(int idActividad)
    {
        
    }

    public bool marcarCancelada(int idActividad, String justificacion, DateTime fecha)
    {
        
    }

    public Actividad consultarActividad(int idActividad)
    {
        
    }

    public List<Actividad> consultarActividades()
    {
        
    }
}