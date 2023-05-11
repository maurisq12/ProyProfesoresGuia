namespace ProfesoresGuia.Models;

public class CentroAcademico
{
    public int idCentro { get; set; }
    public SiglasCentros siglas { get; set; }
    public String nombre { get; set; }
    public int cantidadProfesores { get; set; }

    
    public CentroAcademico(int idCentro, SiglasCentros siglas, String nombre, int cantidadProfesores)
    {
        this.idCentro = idCentro;
        this.siglas = siglas;
        this.nombre = nombre;
        this.cantidadProfesores = cantidadProfesores;
    }
}