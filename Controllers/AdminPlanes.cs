using DocumentFormat.OpenXml.Office2010.ExcelAc;
using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdminPlanes
{
    
    /*public PlanTrabajo consultarPlan()
    {
        PlanTrabajo p = new PlanTrabajo();
        p.idPlanTrabajo = 1;
        p.equipo = return SingletonDAO.getInstance().getEquiposGuia();
        p.itinerario = consultarActividades();
        
        return p;
    }*/

    public Actividad consultarProxActividad()
    {
        return SingletonDAO.getInstance().getProximaActividad();
    }

    public bool agregarActividadPlan(Actividad pActividad)
    {
        return SingletonDAO.getInstance().InsertarActividad( pActividad);
    }

    public bool activarPublicacion(int idActividad)
    {
        return SingletonDAO.getInstance().activarPublicacion(idActividad);
    }

    public bool marcarCancelada(int idActividad, String justificacion, DateTime fecha)
    {
        return SingletonDAO.getInstance().marcarCancelada(idActividad, justificacion, fecha);
    }
    
    public bool marcarRealizada(Evidencia evidencia, int idActividad)
    {
        return true;//SingletonDAO.getInstance().marcarRealizada(evidencia,int idActividad);
    }

    public Actividad consultarActividad(int idActividad)
    {
        return SingletonDAO.getInstance().getActividadXid(idActividad);
    }

    public List<Actividad> consultarActividades()
    {
        return SingletonDAO.getInstance().getActividades();
    }

    public List<Evidencia> getEvidencias()
    {
        return null;//SingletonDAO.getInstance().getEvidencias();
    }

    public List<Imagen> getImagenes()
    {
        return null;//SingletonDAO.getInstance().getImagenes();
    }
}
