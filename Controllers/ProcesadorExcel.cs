using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using SpreadsheetLight;
using System.Data;
using OfficeOpenXml;
using ProfesoresGuia.Models;

namespace ProfesoresGuia.Controllers;

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
    excel.SaveAs(archivo);
    }

    public List<Estudiante> LeerEstudiantesDesdeExcel(string rutaArchivo)
    {
        List<Estudiante> estudiantes = new List<Estudiante>();

        FileInfo archivo = new FileInfo(rutaArchivo);
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
}
