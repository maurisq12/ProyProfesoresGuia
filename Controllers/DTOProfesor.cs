namespace ProfesoresGuia.Controllers;

public class DTOProfesor
{
    public String codigo { get; set; }
    public String nombreCompleto { get; set; }
    public String correoElectronico { get; set; }
    public String telefonoOficina { get; set; }
    public String telefonoCelular { get; set; }
    public String fotografia { get; set; }
    public String activo { get; set; }

    public DTOProfesor(){}


    public DTOProfesor(String codigo, String nombreCompleto, String correoElectronico, String telefonoOficina, String telefonoCelular, String fotografia, String activo)
    {
        this.codigo = codigo;
        this.nombreCompleto = nombreCompleto;
        this.correoElectronico = correoElectronico;
        this.telefonoOficina = telefonoOficina;
        this.telefonoCelular = telefonoCelular;
        this.fotografia = fotografia;
        this.activo = activo;
    }
    
    public override string ToString()
    {
        return $"Código DTOProfesor: {codigo}\n" +
               $"Nombre completo: {nombreCompleto}\n" +
               $"Correo electrónico: {correoElectronico}\n" +
               $"Teléfono de oficina: {telefonoOficina}\n" +
               $"Teléfono celular: {telefonoCelular}\n" +
               $"Fotografía: {fotografia}\n" +
               $"Activo: {activo}";
    }
}