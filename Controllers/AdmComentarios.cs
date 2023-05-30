using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdmComentarios
{
    public bool realizarComentario(Comentario comentario, int pActividad)
    {
        return SingletonDAO.getInstance().InsertarComentario(comentario, pActividad);
    }

    public int getComentariosCount()
    {
        return SingletonDAO.getInstance().getCountComentarios();
    }

}