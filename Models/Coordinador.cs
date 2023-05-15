namespace ProfesoresGuia.Models;

public class Coordinador
{
    public String id { get; set; }
    public String nombreCompleto { get; set; }
    public String correoElectronico { get; set; }



    public Coordinador(){}


    public Coordinador(String id, String nombreCompleto, String correoElectronico)
    {
        this.id = id;
        this.nombreCompleto = nombreCompleto;
        this.correoElectronico = correoElectronico;
    }
    
    public override string ToString()
    {
        return $"ID: {id}\nNombre Completo: {nombreCompleto}\nCorreo Electrónico: {correoElectronico}";
    }
}