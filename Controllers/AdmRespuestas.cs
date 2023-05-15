namespace ProfesoresGuia.Controllers;

public class AdmRespuestas
{
    public bool realizarRespuesta(int pComentario, String pRespuesta)
    {
        return SingletonDAO.getInstance().realizarRespuesta(pComentario, pRespuesta);
    }
}