using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class AdmRespuestas
{
    public bool realizarRespuesta(Respuesta respuesta)
    {
        return SingletonDAO.getInstance().InsertarRespuesta(respuesta, respuesta.idComentario);
    }

    public List<Respuesta> getRespuestas()
    {
        return SingletonDAO.getInstance().getRespuestas();
    }

}