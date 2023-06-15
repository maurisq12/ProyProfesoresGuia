
CREATE TABLE Usuario
(
    id INT PRIMARY KEY IDENTITY(1,1),
    correo VARCHAR(100),
    contrasena VARCHAR(100),
    idTipo int FOREIGN KEY REFERENCES TiposUsuario(idTipoUsuario)        
);



CREATE TABLE TiposUsuario(
	idTipoUsuario INT PRIMARY KEY IDENTITY(1,1),
   nombre VARCHAR(40)
);

INSERT INTO TiposUsuario (nombre)
VALUES ('Profesor'),
('ProfesorCoordinador'),
('AsistenteAdministrativo'),
('AsistenteAdministrativoCartago')

CREATE TABLE Profesor (
    codigo varchar(100) PRIMARY KEY,
    nombreCompleto varchar(100),
    correoElectronico varchar(100),
    telefonoOficina varchar(100),
    telefonoCelular varchar(100),
    fotografia varbinary(100),
    activo VARCHAR(20)
);

CREATE TABLE EquipoGuia (
    idEquipoGuia iNT PRIMARY KEY,
    anno int,
    idProfesorCoordinador varchar(100) FOREIGN KEY REFERENCES Profesor(codigo),
    ultimaModificacion varchar(100)
);


CREATE TABLE CentroAcademico (
    idCentroAcademico iNT PRIMARY KEY,
    
    nombre varchar(100),
    cantProfesores INT,
    Siglas varchar(20)
);
    
CREATE TABLE Asistente (
	idAsistente iNT PRIMARY KEY,
	nombre varchar(100),
	correo varchar(100),
	idCentroAcademico int FOREIGN KEY REFERENCES CentroAcademico(idCentroAcademico)
)


CREATE TABLE Estudiante (
    idEstudiante iNT PRIMARY KEY,
    carne varchar(100),
    nombreCompleto varchar(100),
    correoElectronico varchar(100),
    telefonoCelular varchar(100),
    idCentroAcademico int FOREIGN KEY REFERENCES CentroAcademico(idCentroAcademico)
     
);


CREATE TABLE Actividad (
    idActividad iNT IDENTITY (1,1) PRIMARY KEY,
    semana int,
    EstadoActividad varchar(100),
    nombre varchar(100),
    fechaHora date,
    fechaAnuncio date,
    diasPreviosAnuncio int,
    Modalidad varchar(100),
    enlaceRemoto varchar(100),
    afiche varbinary(MAX),
    TipoActividad varchar(100)
 
  
);

CREATE TABLE PlanTrabajo
(
    idPlanTrabajo INT IDENTITY (1,1) PRIMARY KEY,
    idEquipoGuia INT FOREIGN KEY REFERENCES EquipoGuia(idEquipoGuia),
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad)
        
);

CREATE TABLE ActividadXresponsables
(
    idActividadXresponsables INT IDENTITY (1,1) PRIMARY KEY,
    idProfesorGuia VARCHAR(100) FOREIGN KEY REFERENCES Profesor(codigo),
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad)

        
);

CREATE TABLE Recordatorios
(
    idRecordatorios INT IDENTITY (1,1) PRIMARY KEY,
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad),
    fechaRecordatorio DATE
       
);

CREATE TABLE Comentario
(
    idComentario INT IDENTITY (1,1) PRIMARY KEY,
    idProfesorGuia VARCHAR(100) FOREIGN KEY REFERENCES Profesor(codigo),
    fechaHora DATETIME,
    cuerpo VARCHAR(100)
);

CREATE TABLE ActividadXComentario
(
    idActividadXComentario INT IDENTITY (1,1) PRIMARY KEY,
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad),
    idComentario INT FOREIGN KEY REFERENCES Comentario(idComentario)
        
);

CREATE TABLE Respuesta
(
    idRespuesta INT IDENTITY (1,1) PRIMARY KEY,
    idProfesorGuia VARCHAR(100) FOREIGN KEY REFERENCES Profesor(codigo),
    fechaHora DATETIME,
    cuerpo VARCHAR(100),
    idComentario INT FOREIGN KEY REFERENCES Comentario(idComentario)
);
   
CREATE TABLE Evidencias
(
    idEvidencia INT IDENTITY (1,1) PRIMARY KEY,
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad),
    listaAsistencia VARCHAR(100),
    linkGrabacion VARCHAR(100)
        
);

CREATE TABLE Imagen
(
    idImagen INT IDENTITY (1,1) PRIMARY KEY,
    Imagen VARBINARY(MAX)
);

CREATE TABLE EvidenciasXimagen
(
    idEvidenciasXimagen INT IDENTITY (1,1) PRIMARY KEY,
    idImagen INT FOREIGN KEY REFERENCES Imagen(idImagen),
    idEvidencia INT FOREIGN KEY REFERENCES Evidencias(idEvidencia)
       
);

CREATE TABLE Justificacion
(
    idJustificacion INT IDENTITY (1,1) PRIMARY KEY,
    idActividad INT FOREIGN KEY REFERENCES Actividad(idActividad),
    cuerpo VARCHAR(200),
    fecha DATETIME

);

CREATE TABLE Buzon
(
	 idBuzon INT PRIMARY KEY IDENTITY(1,1),
	 idUsuario INT FOREIGN KEY REFERENCES Usuario(id)     
);

CREATE TABLE Servicio
(
	 idServicio INT PRIMARY KEY IDENTITY(1,1),
	 idUsuario INT FOREIGN KEY REFERENCES Usuario(id)     
);


CREATE TABLE Notificacion
(
	 idNotificacion INT PRIMARY KEY IDENTITY(1,1),
	 idBuzon INT FOREIGN KEY REFERENCES Buzon(idBuzon),
    idServicio INT FOREIGN KEY REFERENCES Servicio(idServicio),
    contenido VARCHAR(300),
    fecha DATETIME,
    estado VARCHAR(20)      
);

CREATE TABLE BuzonesXServicio(
	idBuzon INT FOREIGN KEY REFERENCES Buzon(idBuzon),
	idServicio INT FOREIGN KEY REFERENCES Servicio(idServicio)
);
