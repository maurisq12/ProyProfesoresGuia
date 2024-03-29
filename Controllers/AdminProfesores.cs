using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdminProfesores
{
    public bool agregarProfesor(DTOProfesor pProfesor, string pContrasena)
    {
        ProfesorGuia profe = new ProfesorGuia(pProfesor.codigo, pProfesor.nombreCompleto, pProfesor.correoElectronico,
            pProfesor.telefonoOficina, pProfesor.telefonoCelular, pProfesor.fotografia, pProfesor.activo, pProfesor.sede);
        return SingletonDAO.getInstance().insertarProfesor(profe,pContrasena);
    }

    public bool editarProfesor(DTOProfesor pProfesor)
    {
        ProfesorGuia profe = new ProfesorGuia(pProfesor.codigo, pProfesor.nombreCompleto, pProfesor.correoElectronico,
            pProfesor.telefonoOficina, pProfesor.telefonoCelular, pProfesor.fotografia, pProfesor.activo, pProfesor.sede);
        return SingletonDAO.getInstance().modificarProfesor(profe);
    }

    /*public bool modificarEstadoProfesor(int pProfesor, String pEstado)
    {
        return SingletonDAO.getInstance().modificarEstadoProfesor(pProfesor, pEstado);
    }

    public List<ProfesorGuia> obtenerProfesoresSede(int pSede)
    {
        return SingletonDAO.getInstance().getProfesoresSede(pSede);
    }

    public List<ProfesorGuia> obtenerProfesoresEquipo(int pEquipo)
    {
        return SingletonDAO.getInstance().getProfesoresEquipo(pEquipo);
    }*/

    public List<ProfesorGuia> obtenerProfesores()
    {
        return SingletonDAO.getInstance().getTodosProfesores();
    }
}