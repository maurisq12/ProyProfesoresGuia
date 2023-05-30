using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdmRespuestas
{
    public bool realizarRespuesta(Respuesta respuesta)
    {
        return SingletonDAO.getInstance().InsertarRespuesta(respuesta, respuesta.idComentario);
    }

    public int getRespuestasCount()
    {
        return SingletonDAO.getInstance().getRespuestas();
    }

}