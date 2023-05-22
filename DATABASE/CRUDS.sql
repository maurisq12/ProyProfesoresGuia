CREATE TABLE Usuarios(
	idUsuario iNT IDENTITY (1,1) PRIMARY KEY,
	correoElectronico varchar(100),
	contrasenna varchar(100),
	rol varchar(100)
);

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
    idEquipoGuia iNT IDENTITY (1,1) PRIMARY KEY,
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

CREATE TABLE EquipoGuiaXprofesores (
    idEquipoGuiaXEstudiantes iNT IDENTITY (1,1) PRIMARY KEY,
    idEquipoGuia int FOREIGN KEY REFERENCES EquipoGuia(idEquipoGuia),
    idProfesorGuia varchar(100) FOREIGN KEY REFERENCES Profesor(codigo)
 
);

CREATE TABLE TipoActividad (
    idTipoActividad iNT IDENTITY (1,1) PRIMARY KEY,
    TipoActividad varchar(100)
);


CREATE TABLE Modalidad (
    idModalidad iNT IDENTITY (1,1) PRIMARY KEY,
    Modalidad varchar(100)
);


CREATE TABLE EstadoActividad (
    idEstadoActividad iNT IDENTITY (1,1) PRIMARY KEY,
    EstadoActividad varchar(100)
);

CREATE TABLE Actividad (
    idActividad iNT IDENTITY (1,1) PRIMARY KEY,
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
       
); ;
END;


--------UPDATE--------------------
CREATE OR ALTER PROCEDURE actualizar_equipoGuia
    @in_idEquipoGuia int, 
    @in_anno int, 
    @in_idProfesorCoordinador varchar(100), 
    @in_ultimaModificacion varchar(100)
AS 
BEGIN
    UPDATE EquipoGuia SET anno = @in_anno, idProfesorCoordinador = @in_idProfesorCoordinador, ultimaModificacion = @in_ultimaModificacion 
    WHERE idEquipoGuia = @in_idEquipoGuia;
END;


--------DELETE--------------------
CREATE OR ALTER PROCEDURE eliminar_equipoGuia
    @in_idEquipoGuia int
AS 
BEGIN
    DELETE FROM EquipoGuia WHERE idEquipoGuia = @in_idEquipoGuia;
END;

-------CRUD SiglasCentros------------------------
--------CREATE--------------------

CREATE OR ALTER PROCEDURE insertar_siglasCentro (
@in_idSiglas INTEGER,
@in_Siglas VARCHAR(20)
)
AS
BEGIN
INSERT INTO SiglasCentros (idSiglas, Siglas)
VALUES (@in_idSiglas, @in_Siglas)
END;

--------READ--------------------

CREATE OR ALTER PROCEDURE consultar_siglasCentro
AS
BEGIN
SELECT * FROM SiglasCentros;
END;

--------UPDATE--------------------

CREATE OR ALTER PROCEDURE actualizar_siglasCentro (
	@in_idSiglas INTEGER,
	@in_Siglas VARCHAR(20)
)
AS
BEGIN
	UPDATE SiglasCentros SET Siglas = @in_Siglas
	WHERE idSiglas = @in_idSiglas;
END;

--------DELETE--------------------

CREATE OR ALTER PROCEDURE eliminar_siglasCentro (
@in_idSiglas INTEGER
)
AS
BEGIN
DELETE FROM SiglasCentros WHERE idSiglas = @in_idSiglas;
END;

-------CRUD CentroAcademico------------------------
--------CREATE--------------------
CREATE OR ALTER PROCEDURE insertar_centroAcademico
	@in_idCentroAcademico INT,
	@in_nombre VARCHAR(100),
	@in_cantProfesores INT
AS
BEGIN
INSERT INTO CentroAcademico (idCentroAcademico, nombre, cantProfesores)
VALUES (@in_idCentroAcademico, @in_nombre, @in_cantProfesores);
END;

--------READ----------------------
CREATE OR ALTER PROCEDURE consultar_centroAcademico
AS
BEGIN
SELECT * FROM CentroAcademico;
END;

--------UPDATE--------------------
CREATE OR ALTER PROCEDURE actualizar_centroAcademico
	@in_idCentroAcademico INT,
	@in_idSiglas INT,
	@in_nombre VARCHAR(100),
	@in_cantProfesores INT
AS
BEGIN
UPDATE CentroAcademico SET
idSiglas = @in_idSiglas,
nombre = @in_nombre,
cantProfesores = @in_cantProfesores
WHERE idCentroAcademico = @in_idCentroAcademico;
END;

--------DELETE--------------------
CREATE OR ALTER PROCEDURE eliminar_centroAcademico
@in_idCentroAcademico INT
AS
BEGIN
DELETE FROM CentroAcademico WHERE idCentroAcademico = @in_idCentroAcademico;
END;

-------CRUD Estudiante------------------------
--------CREATE--------------------
CREATE OR ALTER PROCEDURE insertar_estudiante
	@in_idEstudiante int,
	@in_carne varchar(100),
	@in_nombreCompleto varchar(100),
	@in_correoElectronico varchar(100),
	@in_telefonoCelular varchar(100),
	@in_idCentroAcademico int
AS
BEGIN
INSERT INTO Estudiante (idEstudiante, carne, nombreCompleto, correoElectronico, telefonoCelular, idCentroAcademico)
VALUES (@in_idEstudiante, @in_carne, @in_nombreCompleto, @in_correoElectronico, @in_telefonoCelular, @in_idCentroAcademico);
END;

--------READ----------------------
CREATE PROCEDURE consultar_estudiante
AS
BEGIN
    SELECT E.idEstudiante, E.carne, E.nombreCompleto, E.correoElectronico, E.telefonoCelular, 
           CA.idCentroAcademico, CA.idSiglas, CA.nombre AS nombreCentroAcademico, CA.cantProfesores,
           SC.idSiglas, SC.Siglas
    FROM Estudiante E
    INNER JOIN CentroAcademico CA ON E.idCentroAcademico = CA.idCentroAcademico
    INNER JOIN SiglasCentros SC ON CA.idSiglas = SC.idSiglas;
END;

--------UPDATE--------------------
CREATE OR ALTER PROCEDURE actualizar_estudiante
	@in_idEstudiante int,
	@in_carne varchar(100),
	@in_nombreCompleto varchar(100),
	@in_correoElectronico varchar(100),
	@in_telefonoCelular varchar(100),
	@in_idCentroAcademico int
AS
BEGIN
UPDATE Estudiante SET
carne = @in_carne,
nombreCompleto = @in_nombreCompleto,
correoElectronico = @in_correoElectronico,
telefonoCelular = @in_telefonoCelular,
idCentroAcademico = @in_idCentroAcademico
WHERE idEstudiante = @in_idEstudiante;
END;

--------DELETE--------------------
CREATE OR ALTER PROCEDURE eliminar_estudiante
@in_idEstudiante int
AS
BEGIN
DELETE FROM Estudiante WHERE idEstudiante = @in_idEstudiante;
END;

-------CRUD EquipoGuiaXprofesores------------------------
--------CREATE--------------------
CREATE PROCEDURE insertar_EquipoGuiaXprofesores
@in_idEquipoGuiaXEstudiantes int,
@in_idEquipoGuia int,
@in_idProfesorGuia varchar(100)
AS
BEGIN
INSERT INTO EquipoGuiaXprofesores (idEquipoGuiaXEstudiantes,idEquipoGuia,idProfesorGuia)
VALUES (@in_idEquipoGuiaXEstudiantes,@in_idEquipoGuia,@in_idProfesorGuia);
END;

--------READ----------------------
CREATE PROCEDURE consultar_EquipoGuiaXprofesores
AS
BEGIN
SELECT * FROM EquipoGuiaXprofesores;
END;

--------UPDATE--------------------
CREATE PROCEDURE actualizar_EquipoGuiaXprofesores
@in_idEquipoGuiaXEstudiantes int,
@in_idEquipoGuia int,
@in_idProfesorGuia varchar(100)
AS
BEGIN
UPDATE EquipoGuiaXprofesores SET
idEquipoGuia = @in_idEquipoGuia,
idProfesorGuia = @in_idProfesorGuia
WHERE idEquipoGuiaXEstudiantes = @in_idEquipoGuiaXEstudiantes;
END;

--------DELETE--------------------
CREATE PROCEDURE eliminar_EquipoGuiaXprofesores
@in_idEquipoGuiaXEstudiantes int
AS
BEGIN
DELETE FROM EquipoGuiaXprofesores WHERE idEquipoGuiaXEstudiantes = @in_idEquipoGuiaXEstudiantes;
END;

-------CRUD TipoActividad------------------------
--------CREATE--------------------
CREATE OR ALTER PROCEDURE insertar_TipoActividad(
@in_id_tipo_actividad int,
@in_tipo_actividad varchar(100)
)
AS
BEGIN
INSERT INTO TipoActividad (idTipoActividad, TipoActividad)
VALUES (@in_id_tipo_actividad, @in_tipo_actividad);
END;

--------READ----------------------
CREATE OR ALTER PROCEDURE consultar_TipoActividad
AS
BEGIN
SELECT * FROM TipoActividad;
END;

--------UPDATE----------------------
CREATE OR ALTER PROCEDURE actualizar_TipoActividad(
@in_id_tipo_actividad int,
@in_tipo_actividad varchar(100)
)
AS
BEGIN
UPDATE TipoActividad SET
TipoActividad = @in_tipo_actividad
WHERE idTipoActividad = @in_id_tipo_actividad;
END;

--------DELETE----------------------
CREATE OR ALTER PROCEDURE eliminar_TipoActividad(
@in_id_tipo_actividad int
)
AS
BEGIN
DELETE FROM TipoActividad WHERE idTipoActividad = @in_id_tipo_actividad;
END;

-------CRUD Modalidad------------------------
--------CREATE---------------------
CREATE OR ALTER PROCEDURE insertar_modalidad
@id_modalidad INT,
@modalidad_texto TEXT
AS
BEGIN
INSERT INTO Modalidad (idModalidad,Modalidad) VALUES (@id_modalidad,@modalidad_texto);
COMMIT;
END;

--------READ-----------------------
CREATE OR ALTER PROCEDURE consultar_modalidad
AS
BEGIN
SELECT * FROM Modalidad;
END;

--------UPDATE---------------------
CREATE OR ALTER PROCEDURE actualizar_modalidad
@id_modalidad INT,
@modalidad_texto TEXT
AS
BEGIN
UPDATE Modalidad SET Modalidad = @modalidad_texto WHERE idModalidad = @id_modalidad;
COMMIT;
END;

--------DELETE----------------------
CREATE OR ALTER PROCEDURE borrar_modalidad
@id_modalidad INT
AS
BEGIN
DELETE FROM Modalidad WHERE idModalidad = @id_modalidad;
COMMIT;
END;


-------CRUD EstadoActividad------------------------
--------CREATE---------------------
CREATE OR ALTER PROCEDURE insertar_EstadoActividad
@id_EstadoActividad INT,
@estado_actividad_texto VARCHAR(100)
AS
BEGIN
INSERT INTO EstadoActividad (idEstadoActividad, EstadoActividad)
VALUES (@id_EstadoActividad, @estado_actividad_texto);
END;

--------READ-----------------------
CREATE OR ALTER PROCEDURE consultar_EstadoActividad
AS
BEGIN
SELECT * FROM EstadoActividad;
END;

--------UPDATE---------------------
CREATE OR ALTER PROCEDURE actualizar_EstadoActividad
@id_estado_actividad INT,
@estado_actividad_texto VARCHAR(100)
AS
BEGIN
UPDATE EstadoActividad
SET EstadoActividad = @estado_actividad_texto
WHERE idEstadoActividad = @id_estado_actividad;
END;

--------DELETE----------------------
CREATE OR ALTER PROCEDURE borrar_EstadoActividad
@id_estado_actividad INT
AS
BEGIN
DELETE FROM EstadoActividad
WHERE idEstadoActividad = @id_estado_actividad;
END;

-------CRUD Actividad------------------------
--------CREATE---------------------
CREATE OR ALTER PROCEDURE insertar_actividad
@id_actividad INT,
@semana_num INT,
@tipo_actividad_id INT,
@nombre_texto TEXT,
@fecha_hora DATETIME,
@fecha_anuncio DATE,
@dias_previos_anuncio INT,
@modalidad_id INT,
@enlace_remoto_texto TEXT,
@afiche_bytea VARBINARY(MAX),
@estado_actividad_id INT
AS
BEGIN
INSERT INTO Actividad (idActividad, semana, idTipoActividad, nombre, fechaHora, fechaAnuncio, diasPreviosAnuncio, idModalidad, enlaceRemoto, afiche, idEstadoActividad)
VALUES (@id_actividad, @semana_num, @tipo_actividad_id, @nombre_texto, @fecha_hora, @fecha_anuncio, @dias_previos_anuncio, @modalidad_id, @enlace_remoto_texto, @afiche_bytea, @estado_actividad_id);
END;

--------READ-----------------------
CREATE PROCEDURE consultar_actividades AS
BEGIN
SELECT * FROM Actividad;
END;

--------UPDATE---------------------
CREATE OR ALTER PROCEDURE actualizar_actividad
@id_actividad INT,
@semana_num INT,
@tipo_actividad_id INT,
@nombre_texto TEXT,
@fecha_hora DATETIME,
@fecha_anuncio DATE,
@dias_previos_anuncio INT,
@modalidad_id INT,
@enlace_remoto_texto TEXT,
@afiche_bytea VARBINARY(MAX),
@estado_actividad_id INT
AS
BEGIN
UPDATE Actividad
SET semana = @semana_num,
idTipoActividad = @tipo_actividad_id,
nombre = @nombre_texto,
fechaHora = @fecha_hora,
fechaAnuncio = @fecha_anuncio,
diasPreviosAnuncio = @dias_previos_anuncio,
idModalidad = @modalidad_id,
enlaceRemoto = @enlace_remoto_texto,
afiche = @afiche_bytea,
idEstadoActividad = @estado_actividad_id
WHERE idActividad = @id_actividad;
END;

--------DELETE----------------------
CREATE OR ALTER PROCEDURE borrar_actividad
@id_actividad INT
AS
BEGIN
DELETE FROM Actividad WHERE idActividad = @id_actividad;
END;

-------CRUD PlanTrabajo------------------------
--------CREATE---------------------
CREATE OR ALTER PROCEDURE insertar_planTrabajo
@planTrabajo_id INT,
@equipo_guia_id INT,
@actividad_id INT
AS
BEGIN
INSERT INTO PlanTrabajo (idPlanTrabajo, idEquipoGuia, idActividad)
VALUES (@planTrabajo_id, @equipo_guia_id, @actividad_id);
COMMIT;
END;

--------READ-----------------------
CREATE OR ALTER PROCEDURE consultar_planTrabajo
AS
BEGIN
SELECT * FROM PlanTrabajo;
END;

--------UPDATE---------------------
CREATE OR ALTER PROCEDURE actualizar_planTrabajo
@plan_trabajo_id INT,
@equipo_guia_id INT,
@actividad_id INT
AS
BEGIN
UPDATE PlanTrabajo
SET idEquipoGuia = @equipo_guia_id,
idActividad = @actividad_id
WHERE idPlanTrabajo = @plan_trabajo_id;
COMMIT;
END;

--------DELETE---------------------
CREATE OR ALTER PROCEDURE borrar_planTrabajo
@plan_trabajo_id INT
AS
BEGIN
DELETE FROM PlanTrabajo WHERE idPlanTrabajo = @plan_trabajo_id;
COMMIT;
END;

-------CRUD ActividadXresponsables------------------------
--------CREATE---------------------
CREATE OR ALTER PROCEDURE insertar_ActividadXresponsables
@actividad_responsable_id INT,
@profesor_guia_id VARCHAR(100),
@actividad_id INT
AS
BEGIN
INSERT INTO ActividadXresponsables (idActividadXresponsables, idProfesorGuia, idActividad)
VALUES (@actividad_responsable_id, @profesor_guia_id, @actividad_id);
END;

--------READ-----------------------
CREATE OR ALTER PROCEDURE consultar_ActividadXresponsables AS
BEGIN
SELECT * FROM ActividadXresponsables;
END;

--------UPDATE---------------------
CREATE OR ALTER PROCEDURE actualizar_ActividadXresponsables(
@actividad_responsable_id INT,
@profesor_guia_id VARCHAR(100),
@actividad_id INT
) AS
BEGIN
UPDATE ActividadXresponsables
SET idProfesorGuia = @profesor_guia_id,
idActividad = @actividad_id
WHERE idActividadXresponsables = @actividad_responsable_id;
END;

--------DELETE---------------------
CREATE OR ALTER PROCEDURE borrar_ActividadXresponsables(
@actividad_responsable_id INT
) AS
BEGIN
DELETE FROM ActividadXresponsables WHERE idActividadXresponsables = @actividad_responsable_id;
END;

-------CRUD Recordatorios------------------------
--------CREATE---------------------
CREATE OR ALTER PROCEDURE insertar_recordatorio(
@id_recordatorio INT,
@actividad_id INT,
@fecha_recordatorio DATE
) AS
BEGIN
INSERT INTO Recordatorios (idRecordatorios, idActividad, fechaRecordatorio)
VALUES (@id_recordatorio, @actividad_id, @fecha_recordatorio);
COMMIT;
END;

--------READ-----------------------
CREATE OR ALTER PROCEDURE consultar_recordatorio AS
BEGIN
SELECT * FROM Recordatorios;
END;

--------UPDATE---------------------
CREATE OR ALTER PROCEDURE actualizar_recordatorio(
@recordatorio_id INT,
@actividad_id INT,
@fecha_recordatorio DATE
) AS
BEGIN
UPDATE Recordatorios
SET idActividad = @actividad_id,
fechaRecordatorio = @fecha_recordatorio
WHERE idRecordatorios = @recordatorio_id;
COMMIT;
END;

--------DELETE---------------------
CREATE OR ALTER PROCEDURE borrar_recordatorio(
@recordatorio_id INT
) AS
BEGIN
DELETE FROM Recordatorios WHERE idRecordatorios = @recordatorio_id;
COMMIT;
END;

-------CRUD Comentario------------------------
--------CREATE---------------------
CREATE PROCEDURE insertar_comentario (
@profesor_guia_id VARCHAR(100),
@fecha_hora DATETIME,
@comentario_cuerpo VARCHAR(MAX)
)
AS
BEGIN
INSERT INTO Comentario (idProfesorGuia, fechaHora, cuerpo)
VALUES (@profesor_guia_id, @fecha_hora, @comentario_cuerpo);
COMMIT;
END;

--------READ-----------------------
CREATE PROCEDURE consultar_comentarios
AS
BEGIN
SELECT * FROM Comentario;
END;

--------UPDATE---------------------
CREATE PROCEDURE actualizar_comentario (
@comentario_id INT,
@profesor_guia_id VARCHAR(100),
@fecha_hora DATETIME,
@comentario_cuerpo VARCHAR(MAX)
)
AS
BEGIN
UPDATE Comentario
SET idProfesorGuia = @profesor_guia_id,
fechaHora = @fecha_hora,
cuerpo = @comentario_cuerpo
WHERE idComentario = @comentario_id;
COMMIT;
END;

--------DELETE---------------------
CREATE PROCEDURE borrar_comentario (
@comentario_id INT
)
AS
BEGIN
DELETE FROM Comentario WHERE idComentario = @comentario_id;
COMMIT;
END;

-------CRUD ActividadXComentario------------------------
--------CREATE---------------------
CREATE OR ALTER PROCEDURE insertar_actividadxcomentario
@id_actividadxcomentario int,
@actividad_id int,
@comentario_id int
AS
BEGIN
INSERT INTO ActividadXComentario (idActividadXComentario, idActividad, idComentario)
VALUES (@id_actividadxcomentario, @actividad_id, @comentario_id);
END;

--------READ-----------------------
CREATE OR ALTER PROCEDURE consultar_actividadesxcomentarios
AS
BEGIN
SELECT * FROM ActividadXComentario;
END;

--------UPDATE---------------------
CREATE OR ALTER PROCEDURE actualizar_actividadxcomentario
@id_actividadxcomentario int,
@actividad_id int,
@comentario_id int
AS
BEGIN
UPDATE ActividadXComentario
SET idActividad = @actividad_id,
idComentario = @comentario_id
WHERE idActividadXComentario = @id_actividadxcomentario;
END;

--------DELETE---------------------
CREATE OR ALTER PROCEDURE borrar_actividadxcomentario
@actividadxcomentario_id int
AS
BEGIN
DELETE FROM ActividadXComentario WHERE idActividadXComentario = @actividadxcomentario_id;
END;

-------CRUD Respuesta------------------------
--------CREATE---------------------
CREATE OR ALTER PROCEDURE insertar_respuesta(
@respuesta_id INT,
@profesor_guia_id TEXT,
@fecha_hora DATE,
@cuerpo_respuesta TEXT,
@comentario_id INT
) AS
BEGIN
INSERT INTO Respuesta (idRespuesta, idProfesorGuia, fechaHora, cuerpo, idComentario)
VALUES (@respuesta_id, @profesor_guia_id, @fecha_hora, @cuerpo_respuesta, @comentario_id);
COMMIT;
END;

--------READ-----------------------
CREATE OR ALTER PROCEDURE consultar_respuestas AS
BEGIN
SELECT * FROM Respuesta;
END;

--------UPDATE---------------------
CREATE OR ALTER PROCEDURE actualizar_respuesta(
@respuesta_id INT,
@profesor_guia_id TEXT,
@fecha_hora DATE,
@cuerpo_respuesta TEXT,
@comentario_id INT
) AS
BEGIN
UPDATE Respuesta
SET idProfesorGuia = @profesor_guia_id, fechaHora = @fecha_hora, cuerpo = @cuerpo_respuesta, idComentario = @comentario_id
WHERE idRespuesta = @respuesta_id;
COMMIT;
END;

--------DELETE---------------------
CREATE OR ALTER PROCEDURE borrar_respuesta(
@respuesta_id INT
) AS
BEGIN
DELETE FROM Respuesta WHERE idRespuesta = @respuesta_id;
COMMIT;
END;

-------CRUD Evidencias------------------------
--------CREATE---------------------
CREATE OR ALTER PROCEDURE insertar_evidencia
@in_idEvidencia INT,
@in_idActividad INT,
@in_listaAsistencia TEXT,
@in_linkGrabacion TEXT
AS
BEGIN
INSERT INTO Evidencias (idEvidencia, idActividad, listaAsistencia, linkGrabacion)
VALUES (@in_idEvidencia, @in_idActividad, @in_listaAsistencia, @in_linkGrabacion);
COMMIT;
END;

--------READ-----------------------
CREATE OR ALTER PROCEDURE consultar_evidencias
AS
BEGIN
SELECT * FROM Evidencias;
END;

--------UPDATE---------------------
CREATE OR ALTER PROCEDURE actualizar_evidencias
@in_idEvidencia INT,
@in_idActividad INT,
@in_listaAsistencia TEXT,
@in_linkGrabacion TEXT
AS
BEGIN
UPDATE Evidencias
SET idEvidencia = @in_idEvidencia,
idActividad = @in_idActividad,
listaAsistencia = @in_listaAsistencia,
linkGrabacion = @in_linkGrabacion
WHERE idEvidencia = @in_idEvidencia;
COMMIT;
END;

--------DELETE---------------------
CREATE OR ALTER PROCEDURE borrar_evidencia
@in_idEvidencia INT
AS
BEGIN
DELETE FROM Evidencias WHERE idEvidencia = @in_idEvidencia;
COMMIT;
END;

-------CRUD Imagen------------------------
--------CREATE---------------------
CREATE PROCEDURE insertar_imagen
@in_idImagen int,
@in_Imagen varbinary(MAX)
AS
BEGIN
INSERT INTO Imagen (idImagen, Imagen)
VALUES (@in_idImagen, @in_Imagen);
END;

--------READ-----------------------
CREATE PROCEDURE consultar_Imagen
AS
BEGIN
SELECT * FROM Imagen;
END;

--------UPDATE---------------------
CREATE PROCEDURE actualizar_imagen
@in_idImagen int,
@in_Imagen varbinary(MAX)
AS
BEGIN
UPDATE Imagen
SET Imagen = @in_Imagen
WHERE idImagen = @in_idImagen;
END;

--------DELETE---------------------
CREATE PROCEDURE borrar_imagen
@in_idImagen int
AS
BEGIN
DELETE FROM Imagen WHERE idImagen = @in_idImagen;
END;

-------CRUD EvidenciasXimagen------------------------
--------CREATE---------------------
CREATE OR ALTER PROCEDURE insertar_evidenciasXimagen(
@in_idEvidenciasXimagen INT,
@in_idImagen int,
@in_idEvidencia INT
) AS
BEGIN
INSERT INTO [EvidenciasXimagen] (idEvidenciasXimagen, idImagen, idEvidencia)
VALUES (@in_idEvidenciasXimagen, @in_idImagen, @in_idEvidencia);
END;

--------READ-----------------------
CREATE OR ALTER PROCEDURE consultar_evidenciasXimagen AS
BEGIN
SELECT * FROM [EvidenciasXimagen];
END;

--------UPDATE---------------------
CREATE OR ALTER PROCEDURE actualizar_evidenciasXimagen(
@in_idEvidenciasXimagen INT,
@in_idImagen int,
@in_idEvidencia INT
) AS
BEGIN
UPDATE [EvidenciasXimagen]
SET idImagen = @in_idImagen, idEvidencia = @in_idEvidencia
WHERE idEvidenciasXimagen = @in_idEvidenciasXimagen;
END;

--------DELETE---------------------
CREATE OR ALTER PROCEDURE borrar_evidenciasXimagen(
@in_idEvidenciasXimagen INT
) AS
BEGIN
DELETE FROM [EvidenciasXimagen] WHERE idEvidenciasXimagen = @in_idEvidenciasXimagen;
END;
