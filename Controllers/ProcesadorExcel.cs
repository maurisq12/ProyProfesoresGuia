using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

public class ProcesadorExcel
{
    public List<Estudiante> procesarExcel(String ruta)
    {
        List<Estudiante> estudiantes = new List<Estudiante>();

        FileInfo archivo = new FileInfo(ruta);
        using (ExcelPackage excel = new ExcelPackage(archivo))
        {
            ExcelWorksheet hoja = excel.Workbook.Worksheets[0]; // Leer la primera hoja

            int totalFilas = hoja.Dimension.Rows;
            for (int fila = 1; fila <= totalFilas; fila++)
            {
                Estudiante estudiante = new Estudiante();
                estudiante.idEstudiante = Convert.ToInt32(hoja.Cells[fila, 1].Value);
                estudiante.carne = hoja.Cells[fila, 2].Value?.ToString();
                estudiante.nombreCompleto = hoja.Cells[fila, 3].Value?.ToString();
                estudiante.correoElectronico = hoja.Cells[fila, 4].Value?.ToString();
                estudiante.telefonoCelular = hoja.Cells[fila, 5].Value?.ToString();

                int idCentro = Convert.ToInt32(hoja.Cells[fila, 6].Value);
                String nombreCentro = hoja.Cells[fila, 7].Value?.ToString();

                String siglas = hoja.Cells[fila, 8].Value?.ToString();
                SiglasCentros siglasCentros = (SiglasCentros)Enum.Parse(typeof(SiglasCentros), siglas);

                int cantProf = Convert.ToInt32(hoja.Cells[fila, 9].Value);
                estudiante.centroEstudio = new CentroAcademico(idCentro,siglasCentros,nombreCentro,cantProf);
                estudiantes.Add(estudiante);
            }
        }

        return estudiantes;
    }

    public String generarExcelEstudiantes(String ruta)
    {
        
    }


    public String generarExcelPestanas(String ruta)
    {
        
    }
}
