namespace ProfesoresGuia.Models;

public class Imagen
{
    public int idImagen { get; set; }
    public int idEvidencia { get; set; }
    public String imagen { get; set; }


    public Imagen()
    {
    }

    public Imagen(int idImagen, int idEvidencia, String imagen)
    {
        this.idImagen = idImagen;
        this.idEvidencia = idEvidencia;
        this.imagen = imagen;
    }
    
    public override string ToString()
    {
        return $"ID Imagen: {idImagen}\nID Evidencia: {idEvidencia}\nImagen: {imagen}";
    }
}