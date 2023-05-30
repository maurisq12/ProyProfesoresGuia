using DocumentFormat.OpenXml.Office2010.ExcelAc;
using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdminEstudiantes
{
   /* public List<Estudiante> obtenerEstudiantesEquipo(int pEquipo)
    {
        return SingletonDAO.getInstance().getEstudiantesEquipo(pEquipo);
    }*/

    public List<Estudiante> obtenerEstudiantes()
    {
        return SingletonDAO.getInstance().getEstudiantes();
    }

    public bool agregarEstudiantes(List<Estudiante> estudiantes)
    {
        foreach (var estudiante in estudiantes)
        {
            SingletonDAO.getInstance().insertarEstudiante(estudiante);
        }

        return true;
    }

    public List<Estudiante> consultarEstudiantesCentro(int pCentro)
    {
        return SingletonDAO.getInstance().getEstudiantesCentro(pCentro);
    }

    public bool modificarEstudiante(DTOEstudiante pEstudiante)
    {
        Estudiante est = new Estudiante(pEstudiante.idEstudiante, pEstudiante.carne, pEstudiante.nombreCompleto, 
            pEstudiante.correoElectronico, pEstudiante.telefonoCelular, pEstudiante.centroEstudio);
        return SingletonDAO.getInstance().modificarEstudiante(est);
    }

    public CentroAcademico getCentro(int idCentro)
    {
        return SingletonDAO.getInstance().getCentro(idCentro);
    }
}