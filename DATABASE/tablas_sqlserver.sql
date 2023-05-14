
CREATE TABLE Profesor (
    codigo varchar(100) PRIMARY KEY,
    nombreCompleto varchar(100),
    correoElectronico varchar(100),
    telefonoOficina varchar(100),
    telefonoCelular varchar(100),
    fotografia varbinary(100),
    activo bit
);

CREATE TABLE EquipoGuia (
    idEquipoGuia int PRIMARY KEY,
    anno int,
    idProfesorCoordinador varchar(100) FOREIGN KEY REFERENCES Profesor(codigo),
    ultimaModificacion varchar(100)
);

CREATE TABLE SiglasCentros (
    idSiglas int PRIMARY KEY,
    Siglas varchar(20)
);

CREATE TABLE CentroAcademico (
    idCentroAcademico int PRIMARY KEY,
    idSiglas int FOREIGN KEY REFERENCES SiglasCentros(idSiglas),
    nombre varchar(100),
    cantProfesores int
);
    

CREATE TABLE Estudiante (
    idEstudiante int PRIMARY KEY,
    carne varchar(100),
    nombreCompleto varchar(100),
    correoElectronico varchar(100),
    telefonoCelular varchar(100),
    idCentroAcademico int FOREIGN KEY REFERENCES CentroAcademico(idCentroAcademico)
     
);

CREATE TABLE EquipoGuiaXprofesores (
    idEquipoGuiaXEstudiantes int PRIMARY KEY,
    idEquipoGuia int FOREIGN KEY REFERENCES EquipoGuia(idEquipoGuia),
    idProfesorGuia varchar(100) FOREIGN KEY REFERENCES Profesor(codigo)
 
);

CREATE TABLE TipoActividad (
    idTipoActividad int PRIMARY KEY,
    TipoActividad varchar(100)
);


CREATE TABLE Modalidad (
    idModalidad int PRIMARY KEY,
    Modalidad varchar(100)
);


CREATE TABLE EstadoActividad (
    idEstadoActividad int PRIMARY KEY,
    EstadoActividad varchar(100)
);

CREATE TABLE Actividad (
    idActividad int PRIMARY KEY,
    semana int,
    idTipoActividad int FOREIGN KEY REFERENCES TipoActividad(idTipoActividad),
    nombre varchar(100),
    fechaHora date,
    fechaAnuncio date,
    diasPreviosAnuncio int,
    idModalidad int FOREIGN KEY REFERENCES Modalidad(idModalidad),
    enlaceRemoto varchar(100),
    afiche varbinary(MAX),
    idEstadoActividad int FOREIGN KEY REFERENCES EstadoActividad(idEstadoActividad)
 
  
);

CREATE TABLE PlanTrabajo
(
    idPlanTrabajo INT PRIMARY KEY,
    idEquipoGuia INT FOREIGN KEY REFERENCES EquipoGuia(idEquipoGuia),
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad)
        
);

CREATE TABLE ActividadXresponsables
(
    idActividadXresponsables INT PRIMARY KEY,
    idProfesorGuia VARCHAR(100) FOREIGN KEY REFERENCES Profesor(codigo),
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad)

        
);

CREATE TABLE Recordatorios
(
    idRecordatorios INT PRIMARY KEY,
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad),
    fechaRecordatorio DATE
       
);

CREATE TABLE Comentario
(
    idComentario INT PRIMARY KEY,
    idProfesorGuia VARCHAR(100) FOREIGN KEY REFERENCES Profesor(codigo),
    fechaHora DATETIME,
    cuerpo VARCHAR(100)
);

CREATE TABLE ActividadXComentario
(
    idActividadXComentario INT PRIMARY KEY,
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad),
    idComentario INT FOREIGN KEY REFERENCES Comentario(idComentario)
        
);

CREATE TABLE Respuesta
(
    idRespuesta INT PRIMARY KEY,
    idProfesorGuia VARCHAR(100) FOREIGN KEY REFERENCES Profesor(codigo),
    fechaHora DATETIME,
    cuerpo VARCHAR(100),
    idComentario INT FOREIGN KEY REFERENCES Comentario(idComentario)
);
   
CREATE TABLE Evidencias
(
    idEvidencia INT PRIMARY KEY,
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad),
    listaAsistencia VARCHAR(100),
    linkGrabacion VARCHAR(100)
        
);

CREATE TABLE Imagen
(
    idImagen INT PRIMARY KEY,
    Imagen VARBINARY(MAX)
);

CREATE TABLE EvidenciasXimagen
(
    idEvidenciasXimagen INT PRIMARY KEY,
    idImagen INT FOREIGN KEY REFERENCES Imagen(idImagen),
    idEvidencia INT FOREIGN KEY REFERENCES Evidencias(idEvidencia)
       
);