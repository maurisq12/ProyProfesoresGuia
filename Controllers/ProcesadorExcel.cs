using System;

using System.Text;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using SpreadsheetLight;
using System.Data;
using OfficeOpenXml;
using ProfesoresGuia.Models;



public class ProcesadorExcel
{
     public void EscribirEstudiantesEnExcel(List<Estudiante> estudiantes, string rutaArchivo)
    {
    // Crear un nuevo archivo de Excel
        ExcelPackage excel = new ExcelPackage();
        var hoja = excel.Workbook.Worksheets.Add("Estudiantes");
    
        // Escribir estudiantes
        int fila = 1;
        foreach (Estudiante estudiante in estudiantes)
        {
            hoja.Cells[fila, 1].Value = estudiante.idEstudiante;
            hoja.Cells[fila, 2].Value = estudiante.carne;
            hoja.Cells[fila, 3].Value = estudiante.nombreCompleto;
            hoja.Cells[fila, 4].Value = estudiante.correoElectronico;
            hoja.Cells[fila, 5].Value = estudiante.telefonoCelular;
            
            hoja.Cells[fila, 6].Value = estudiante.centroEstudio.idCentro;
            hoja.Cells[fila, 7].Value = estudiante.centroEstudio.nombre;
            hoja.Cells[fila, 8].Value = estudiante.centroEstudio.siglas;
             hoja.Cells[fila, 9].Value = estudiante.centroEstudio.cantidadProfesores;
            fila++;
    }

    // Guardar el archivo de Excel
    FileInfo archivo = new FileInfo(rutaArchivo);
    Console.WriteLine(rutaArchivo);
    excel.SaveAs(archivo);
    }

   public List<Estudiante> LeerEstudiantesDesdeExcel(string rutaArchivo)
{
    List<Estudiante> estudiantes = new List<Estudiante>();

    FileInfo archivo = new FileInfo(rutaArchivo);
    using (ExcelPackage excel = new ExcelPackage(archivo))
    {
        foreach (ExcelWorksheet hoja in excel.Workbook.Worksheets) // Iterar sobre todas las hojas
        {
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
                estudiante.centroEstudio = new CentroAcademico(idCentro, siglasCentros, nombreCentro, cantProf);
                estudiantes.Add(estudiante);
            }
        }
    }

    return estudiantes;
}

    public void EscribirEstudiantesEnExcelSeparados(List<Estudiante> estudiantes, string rutaArchivo)
{
    // Crear un nuevo archivo de Excel
    ExcelPackage excel = new ExcelPackage();
    
    foreach (Estudiante estudiante in estudiantes)
    {
        CentroAcademico centroAcademico = estudiante.centroEstudio;
        string nombreHoja = centroAcademico.nombre + "_" + centroAcademico.idCentro;

        // Verificar si la hoja ya existe en el libro de Excel
        var hojaExistente = excel.Workbook.Worksheets.FirstOrDefault(sheet => sheet.Name == nombreHoja);

        ExcelWorksheet hoja;
        if (hojaExistente != null)
        {
            // Si la hoja ya existe, usar la hoja existente en lugar de crear una nueva
            hoja = hojaExistente;
        }
        else
        {
            // Si la hoja no existe, crear una nueva hoja con el nombre único
            hoja = excel.Workbook.Worksheets.Add(nombreHoja);
            
            // Escribir encabezados de columna en la nueva hoja
            hoja.Cells[1, 1].Value = "ID Estudiante";
            hoja.Cells[1, 2].Value = "Carne";
            hoja.Cells[1, 3].Value = "Nombre Completo";
            hoja.Cells[1, 4].Value = "Correo Electrónico";
            hoja.Cells[1, 5].Value = "Teléfono Celular";
            hoja.Cells[1, 6].Value = "ID Centro";
            hoja.Cells[1, 7].Value = "Nombre Centro";
            hoja.Cells[1, 8].Value = "Siglas Centro";
            hoja.Cells[1, 9].Value = "Cantidad Profesores";
        }

        // Calcular la última fila ocupada en la hoja
        int ultimaFila = hoja.Dimension?.End.Row ?? 1;

        // Escribir datos del estudiante en la siguiente fila de la hoja
        hoja.Cells[ultimaFila + 1, 1].Value = estudiante.idEstudiante;
        hoja.Cells[ultimaFila + 1, 2].Value = estudiante.carne;
        hoja.Cells[ultimaFila + 1, 3].Value = estudiante.nombreCompleto;
        hoja.Cells[ultimaFila + 1, 4].Value = estudiante.correoElectronico;
        hoja.Cells[ultimaFila + 1, 5].Value = estudiante.telefonoCelular;
        hoja.Cells[ultimaFila + 1, 6].Value = centroAcademico.idCentro;
        hoja.Cells[ultimaFila + 1, 7].Value = centroAcademico.nombre;
        hoja.Cells[ultimaFila + 1, 8].Value = centroAcademico.siglas;
        hoja.Cells[ultimaFila + 1, 9].Value = centroAcademico.cantidadProfesores;
    }

    // Guardar el archivo de Excel
    FileInfo archivo = new FileInfo(rutaArchivo);
    excel.SaveAs(archivo);
}

public void EscribirEstudiantesEnExcelXsede(List<Estudiante> estudiantes, SiglasCentros sede, string rutaArchivo)
    {
        // Filtrar estudiantes por sede
        List<Estudiante> estudiantesFiltrados = estudiantes.Where(estudiante => estudiante.centroEstudio.siglas == sede).ToList();

        // Crear un nuevo archivo de Excel
        ExcelPackage excel = new ExcelPackage();
        var hoja = excel.Workbook.Worksheets.Add("Estudiantes");

        // Escribir estudiantes
        int fila = 1;
        foreach (Estudiante estudiante in estudiantesFiltrados)
        {
            hoja.Cells[fila, 1].Value = estudiante.idEstudiante;
            hoja.Cells[fila, 2].Value = estudiante.carne;
            hoja.Cells[fila, 3].Value = estudiante.nombreCompleto;
            hoja.Cells[fila, 4].Value = estudiante.correoElectronico;
            hoja.Cells[fila, 5].Value = estudiante.telefonoCelular;

            hoja.Cells[fila, 6].Value = estudiante.centroEstudio.idCentro;
            hoja.Cells[fila, 7].Value = estudiante.centroEstudio.nombre;
            hoja.Cells[fila, 8].Value = estudiante.centroEstudio.siglas;
            hoja.Cells[fila, 9].Value = estudiante.centroEstudio.cantidadProfesores;
            fila++;
        }

        // Guardar el archivo de Excel
        FileInfo archivo = new FileInfo(rutaArchivo);
        Console.WriteLine(rutaArchivo);
        excel.SaveAs(archivo);
    }
}



