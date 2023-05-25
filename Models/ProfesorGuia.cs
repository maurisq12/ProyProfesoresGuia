namespace ProfesoresGuia.Models;

public class ProfesorGuia
{
    public String codigo { get; set; }
    public String nombreCompleto { get; set; }
    public String correoElectronico { get; set; }
    public String telefonoOficina { get; set; }
    public String telefonoCelular { get; set; }
    public String fotografia { get; set; }
    public String activo { get; set; }
    public SiglasCentros sede { get; set; }

    public ProfesorGuia(){}


    public ProfesorGuia(String codigo, String nombreCompleto, String correoElectronico, String telefonoOficina, String telefonoCelular, String fotografia, String activo,SiglasCentros sede)
    {
        this.codigo = codigo;
        this.nombreCompleto = nombreCompleto;
        this.correoElectronico = correoElectronico;
        this.telefonoOficina = telefonoOficina;
        this.telefonoCelular = telefonoCelular;
        this.fotografia = fotografia;
        this.activo = activo;
        this.sede = sede;
    }
    
    public override string ToString()
    {
        return $"Código: {codigo}\nNombre: {nombreCompleto}\nCorreo Electrónico: {correoElectronico}\nTeléfono Oficina: {telefonoOficina}\nTeléfono Celular: {telefonoCelular}\nFotografía: {fotografia}\nActivo: {activo}\nSede: "+ sede.ToString();
    }
}
