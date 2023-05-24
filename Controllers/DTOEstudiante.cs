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


    public DTOEstudiante()
    {
    }

    public DTOEstudiante(int idEstudiante, String carne, String nombreCompleto, String correoElectronico, String telefonoCelular, CentroAcademico centroEstudio)
    {
        this.idEstudiante = idEstudiante;
        this.carne = carne;
        this.nombreCompleto = nombreCompleto;
        this.correoElectronico = correoElectronico;
        this.telefonoCelular = telefonoCelular;
        this.centroEstudio = centroEstudio;
    }
    
    public override string ToString()
    {
        return $"ID DTOEstudiante: {idEstudiante}\n" +
               $"Carne: {carne}\n" +
               $"Nombre completo: {nombreCompleto}\n" +
               $"Correo electrónico: {correoElectronico}\n" +
               $"Teléfono celular: {telefonoCelular}\n" +
               $"Centro de estudio: {centroEstudio}";
    }
}