using DocumentFormat.OpenXml.Office2010.ExcelAc;
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
        return SingletonDAO.getInstance().activarPublicacion(idActividad);
    }

    public bool marcarCancelada(int idActividad, String justificacion, DateTime fecha)
    {
        return SingletonDAO.getInstance().marcarCancelada(idActividad, justificacion, fecha);
    }
    
    public bool marcarRealizada(Evidencia evidencia)
    {
        return SingletonDAO.getInstance().marcarRealizada(evidencia);
    }

    public Actividad consultarActividad(int idActividad)
    {
        return SingletonDAO.getInstance().consultarActividad(idActividad);
    }

    public List<Actividad> consultarActividades()
    {
        return SingletonDAO.getInstance().consultarActividades();
    }

    public List<Evidencia> getEvidencias()
    {
        return SingletonDAO.getInstance().getEvidencias();
    }

    public List<Imagen> getImagenes()
    {
        return SingletonDAO.getInstance().getImagenes();
    }
}