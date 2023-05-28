using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdmComentarios
{
    public bool realizarComentario(int pActividad, String pComentario)
    {
        return SingletonDAO.getInstance().realizarComentario(pActividad, pComentario);
    }

    public List<Comentario> consultarComentarios(int idActividad)
    {
        
    }
}