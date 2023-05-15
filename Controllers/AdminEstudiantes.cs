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

    public List<Estudiante> obtenerEstudiantesAlfabeticoAsc()
    {
        List<Estudiante> estudiantes = SingletonDAO.getInstance().getEstudiantes();
        if (estudiantes != null)
        {
            return estudiantes.OrderBy(est => est.nombreCompleto).ToList();
        }

        return null;
    }
    
    public List<Estudiante> obtenerEstudiantesAlfabeticoDes()
    {
        List<Estudiante> estudiantes = SingletonDAO.getInstance().getEstudiantes();
        if (estudiantes != null)
        {
            return estudiantes.OrderByDescending(est => est.nombreCompleto).ToList();
        }

        return null;
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