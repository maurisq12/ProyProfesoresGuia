using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdminEquipos
{
    public bool agregarProfesorEquipo(int pEquipo, int pProfesor)
    {
        return SingletonDAO.getInstance().agregarProfesorEquipo(pEquipo, pProfesor);
    }

    public bool eliminarProfesorEquipo(int pEquipo, int pProfesor)
    {
        return SingletonDAO.getInstance().eliminarProfesorEquipo(pProfesor, pEquipo);
    }

    public bool definirCoordinador(int pEquipo, String pProfesor)
    {
        return SingletonDAO.getInstance().definirCoordinador(pProfesor, pEquipo);
    }

    public String obtenerDetallesEquipo(int pEquipo)
    {
        EquipoGuia equipo = SingletonDAO.getInstance().getEquipo(pEquipo);
        if (equipo != null)
        {
            return equipo.ToString();
        }

        return "";
    }

    public List<EquipoGuia> obtenerEquipos()
    {
        return SingletonDAO.getInstance().getEquipos();
    }
}