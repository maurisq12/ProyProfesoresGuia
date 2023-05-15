using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class DTOEstudiante
{
    public int idEstudiante { get; set; }
    public String carne { get; set; }
    public String nombreCompleto { get; set; }
    public String correoElectronico { get; set; }
    public String telefonoCelular { get; set; }
    public CentroAcademico centroEstudio { get; set; }


    public DTOEstudiante(int idEstudiante, String carne, String nombreCompleto, String correoElectronico, String telefonoCelular, CentroAcademico centroEstudio)
    {
        this.idEstudiante = idEstudiante;
        this.carne = carne;
        this.nombreCompleto = nombreCompleto;
        this.correoElectronico = correoElectronico;
        this.telefonoCelular = telefonoCelular;
        this.centroEstudio = centroEstudio;
    }
}