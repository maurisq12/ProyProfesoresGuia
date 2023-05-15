
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SpreadsheetLight;
using System.Data;
using OfficeOpenXml;

namespace SampleNamespace
{  




public enum SiglasCentros
{
    CA,
    SJ,
    LI,
    AL,
    SC
}
public class CentroAcademico
{
    public int idCentro { get; set; }
    public SiglasCentros siglas { get; set; }
    public String nombre { get; set; }
    public int cantidadProfesores { get; set; }

    public CentroAcademico(){

        this.idCentro = 0;
        this.siglas = SiglasCentros.AL;
        this.nombre = "";
        this.cantidadProfesores = 0;
    }
    public CentroAcademico(int idCentro, SiglasCentros siglas, String nombre, int cantidadProfesores)
    {
        this.idCentro = idCentro;
        this.siglas = siglas;
        this.nombre = nombre;
        this.cantidadProfesores = cantidadProfesores;
    }
    public override string ToString()
    {
        return $"ID Centro: {idCentro}\n" +
               $"Siglas: {siglas}\n" +
               $"Nombre: {nombre}\n" +
               $"Cantidad de Profesores: {cantidadProfesores}\n";
    }
}


public class Estudiante
{
    public int idEstudiante { get; set; }
    public String carne { get; set; }
    public String nombreCompleto { get; set; }
    public String correoElectronico { get; set; }
    public String telefonoCelular { get; set; }
    public CentroAcademico centroEstudio { get; set; }

    public Estudiante()
    {
        this.idEstudiante = 0;
        this.carne = "";
        this.nombreCompleto = "";
        this.correoElectronico = "";
        this.telefonoCelular = "";
        this.centroEstudio = new CentroAcademico();
    }
    public Estudiante(int idEstudiante, String carne, String nombreCompleto, String correoElectronico, String telefonoCelular, CentroAcademico centroEstudio)
    {
        this.idEstudiante = idEstudiante;
        this.carne = carne;
        this.nombreCompleto = nombreCompleto;
        this.correoElectronico = correoElectronico;
        this.telefonoCelular = telefonoCelular;
        this.centroEstudio = centroEstudio;
    }
    public void crearExcel() 
    {
       string pathFile = AppDomain.CurrentDomain.BaseDirectory + "miArchivo.xlsx";

            SLDocument oSLDocument = new SLDocument();
            DataTable table = new DataTable();

            //Columnas artículos en venta
            table.Columns.Add("Id", typeof(int));

            table.Columns.Add( "Nombre", typeof(string));
            table.Columns.Add("Precio", typeof(double));

            //registros de artículos
            table.Rows.Add(1, "paleta", 5.50);
            table.Rows.Add(2, "papas", 12);

            table.Rows.Add(3, "galletas", 10);

            oSLDocument .ImportDataTable(1, 1, table, true);
            oSLDocument .SaveAs (pathFile); 
    }

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

    public override string ToString()
    {
        return $"ID: {idEstudiante}\n" +
               $"Carne: {carne}\n" +
               $"Nombre Completo: {nombreCompleto}\n" +
               $"Correo Electrónico: {correoElectronico}\n" +
               $"Teléfono Celular: {telefonoCelular}\n" +
               $"Centro de Estudio: {centroEstudio}\n";
    }
}   


}