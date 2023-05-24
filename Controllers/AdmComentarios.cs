namespace ProfesoresGuia.Controllers;

public class AdmComentarios
{
    public bool realizarRespuesta(int pComentario, String pRespuesta)
    {
        return SingletonDAO.getInstance().realizarRespuesta(pComentario, pRespuesta);
    }
}