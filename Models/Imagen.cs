namespace ProfesoresGuia.Models;

public class Imagen
{
    public int idImagen { get; set; }
    public int idEvidencia { get; set; }
    public Byte[] imagen { get; set; }


    public Imagen()
    {
    }

    public Imagen(int idImagen, int idEvidencia, Byte[] imagen)
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