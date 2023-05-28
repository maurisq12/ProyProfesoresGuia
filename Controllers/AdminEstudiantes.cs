using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdminEstudiantes
{
    public List<Estudiante> obtenerEstudiantesEquipo(int pEquipo)
    {
        return SingletonDAO.getInstance().getEstudiantesEquipo(pEquipo);
    }

    public bool convertirEstudiantes(String ruta)
    {
        
    }

    public List<Estudiante> obtenerEstudiantes()
    {
        return SingletonDAO.getInstance().getEstudiantes();
    }
    

    public List<Estudiante> consultarEstudiantesCentro(int pCentro)
    {
        return SingletonDAO.getInstance().getEstudiantesSede(pCentro);
    }

    public bool modificarEstudiante(int pId, DTOEstudiante pEstudiante)
    {
        Estudiante est = new Estudiante(pEstudiante.idEstudiante, pEstudiante.carne, pEstudiante.nombreCompleto, 
            pEstudiante.correoElectronico, pEstudiante.telefonoCelular, pEstudiante.centroEstudio);
        return SingletonDAO.getInstance().modificarEstudiante(pId, est);
    }
}