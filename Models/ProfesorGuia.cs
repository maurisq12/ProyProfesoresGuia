namespace ProfesoresGuia.Models;

public class ProfesorGuia
{
    public String codigo { get; set; }
    public String nombreCompleto { get; set; }
    public String correoElectronico { get; set; }
    public String telefonoOficina { get; set; }
    public String telefonoCelular { get; set; }
    public String fotografia { get; set; }
    public bool activo { get; set; }


    public ProfesorGuia(String codigo, String nombreCompleto, String correoElectronico, String telefonoOficina, String telefonoCelular, String fotografia, bool activo)
    {
        this.codigo = codigo;
        this.nombreCompleto = nombreCompleto;
        this.correoElectronico = correoElectronico;
        this.telefonoOficina = telefonoOficina;
        this.telefonoCelular = telefonoCelular;
        this.fotografia = fotografia;
        this.activo = activo;
    }
}