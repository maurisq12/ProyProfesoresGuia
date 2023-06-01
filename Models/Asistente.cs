namespace ProfesoresGuia.Models;

public class Asistente
{
    public String id { get; set; }
    public String nombreCompleto { get; set; }
    public String correoElectronico { get; set; }
    public int idCentro { get; set; }



    public Asistente(){}


    public Asistente(String id, String nombreCompleto, String correoElectronico,int idCentro)
    {
        this.id = id;
        this.nombreCompleto = nombreCompleto;
        this.correoElectronico = correoElectronico;
        this.idCentro=idCentro;
    }
    
    public override string ToString()
    {
        return $"ID: {id}\nNombre Completo: {nombreCompleto}\nCorreo Electrónico: {correoElectronico}";
    }
}