namespace ProfesoresGuia.Models;

public class CentroAcademico
{
    public int idCentro { get; set; }
    public SiglasCentros siglas { get; set; }
    public String nombre { get; set; }
    public int cantidadProfesores { get; set; }

    public CentroAcademico()
    {
    }

    public CentroAcademico(int idCentro, SiglasCentros siglas, String nombre, int cantidadProfesores)
    {
        this.idCentro = idCentro;
        this.siglas = siglas;
        this.nombre = nombre;
        this.cantidadProfesores = cantidadProfesores;
    }
    
    public override string ToString()
    {
        return $"ID Centro: {idCentro}\nSiglas: {siglas}\nNombre: {nombre}\nCantidad de Profesores: {cantidadProfesores}";
    }
}