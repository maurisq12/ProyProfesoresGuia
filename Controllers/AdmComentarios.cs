using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdmComentarios
{
    public bool realizarComentario(Comentario comentario, int pActividad)
    {
        return SingletonDAO.getInstance().InsertarComentario(comentario, pActividad);
    }

    public List<Comentario> getComentarios()
    {
        return SingletonDAO.getInstance().getComentarios();
    }

}