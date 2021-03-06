USE [MINSAL_SURVIH]
GO
/****** Object:  StoredProcedure [dbo].[setXMLCargaMasiva]    Script Date: 10/08/2015 10:39:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sergio Silva A.
-- Create date: 21-10-2014
-- Description:	XML Carga Masiva
-- =============================================
ALTER PROCEDURE [dbo].[setXMLCargaMasiva]
	@xmlCarga XML
AS
BEGIN	
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED 
	--CREAR TABLA TEMPORAL PARA LOS DATOS DEL ARCHIVO EXCEL
	CREATE TABLE #TMPEXCEL(		
		ID							INT					IDENTITY, 					
		CODIGO_IDENTIDAD			VARCHAR(20)			NULL,
		ID_ESTABLECIMIENTO			VARCHAR(6)			NULL,				
		CODIGO_PAIS					VARCHAR(4)			NULL,	 
		RUT_PACIENTE				VARCHAR(12)			NULL,
		DIGITO_VERIFICADOR			VARCHAR(1)			NULL,
		NOMBRE_PACIENTE				VARCHAR(100)		NULL,
		APELLIDO_PATERNO			VARCHAR(100)		NULL,
		APELLIDO_MATERNO			VARCHAR(100)		NULL,
		SEXO						VARCHAR(1)			NULL,
		FECHA_NACIMIENTO			VARCHAR(20)			NULL,
		FECHA_REGISTRO				VARCHAR(20)			NULL,
		FECHA_FALLECIDO				VARCHAR(20)			NULL,
		FALLECIDO					BIT					NULL,
		BORRADO_LOGICO				VARCHAR(1)			NULL,
		DIRECCION_CALLE				VARCHAR(100)		NULL,
		DIRECCION_NUMERO			VARCHAR(25)			NULL,
		DIRECCION_COMPLEMENTO		VARCHAR(256)		NULL,
		CODIGO_COMUNA				VARCHAR(8)			NULL,
		FONO_FIJO_1					VARCHAR(25)			NULL,
		FONO_FIJO_2					VARCHAR(25)			NULL,
		FONO_MOVIL_1				VARCHAR(25)			NULL,
		FONO_MOVIL_2				VARCHAR(25)			NULL,
		CODIGO_POSTAL				VARCHAR(7)			NULL,
		EMAIL						VARCHAR(100)		NULL,
		CODIGO_ISAPRE				VARCHAR(8)			NULL,
		OTRO_ISAPRE					VARCHAR(100)		NULL,
		CODIGO_CONSULTORIO			VARCHAR(10)			NULL,
		AREA_1						VARCHAR(2)			NULL,
		AREA_2						VARCHAR(2)			NULL,
		TIPO_PUERTA					VARCHAR(100)		NULL,
		TIPO_PACIENTE				VARCHAR(100)		NULL,
		MUESTRA_FECHA				VARCHAR(20)			NULL,
		ENVIO_LABORATORIO_FECHA		VARCHAR(20)			NULL,
		ID_USER_CREADOR				VARCHAR(50)			NULL,
		CODIGO_REGISTRO_EXTRA		VARCHAR(35)			NULL,
		CODIGO_LABORATORIO			VARCHAR(8)			NULL,
		RECEPCION_FECHA				VARCHAR(20)			NULL,
		PROCESAMIENTO_FECHA			VARCHAR(20)			NULL,
		REGISTRO_RESULTADO			VARCHAR(3)			NULL,
		REGISTRO_RESULTADO_FECHA	VARCHAR(20)			NULL,
		MENSAJE_PACIENTE			VARCHAR(MAX)		NULL,
		MENSAJE_MUESTRA				VARCHAR(MAX)		NULL,
		ESTADO_GUARDAR				BIT					NULL
	)
	
	-- SE INSERTAN LOS DATOS EN LA TABLA TEMPORAL	
	INSERT INTO #TMPEXCEL(
		CODIGO_IDENTIDAD,
		ID_ESTABLECIMIENTO,				
		CODIGO_PAIS,	 
		RUT_PACIENTE,
		DIGITO_VERIFICADOR,
		NOMBRE_PACIENTE,
		APELLIDO_PATERNO,
		APELLIDO_MATERNO,
		SEXO,
		FECHA_NACIMIENTO,
		FECHA_REGISTRO,
		FECHA_FALLECIDO,
		FALLECIDO,
		BORRADO_LOGICO,
		DIRECCION_CALLE,
		DIRECCION_NUMERO,
		DIRECCION_COMPLEMENTO,
		CODIGO_COMUNA,
		FONO_FIJO_1,
		FONO_FIJO_2,
		FONO_MOVIL_1,
		FONO_MOVIL_2,
		CODIGO_POSTAL,
		EMAIL,
		CODIGO_ISAPRE,
		OTRO_ISAPRE,
		CODIGO_CONSULTORIO,
		AREA_1,
		AREA_2,
		TIPO_PUERTA,	
		TIPO_PACIENTE,
		MUESTRA_FECHA,
		ENVIO_LABORATORIO_FECHA,	
		ID_USER_CREADOR,	
		CODIGO_REGISTRO_EXTRA,
		CODIGO_LABORATORIO,
		RECEPCION_FECHA,
		PROCESAMIENTO_FECHA,	
		REGISTRO_RESULTADO,
		REGISTRO_RESULTADO_FECHA,
		MENSAJE_PACIENTE,
		MENSAJE_MUESTRA,
		ESTADO_GUARDAR
	)
	SELECT 
		lista.elemento.query('./CODIGO_IDENTIDAD').value('.','VARCHAR(20)'),
		lista.elemento.query('./ID_ESTABLECIMIENTO').value('.','VARCHAR(6)'),
		lista.elemento.query('./CODIGO_PAIS').value('.','VARCHAR(4)'),
		CASE WHEN lista.elemento.query('./RUT_PACIENTE').value('.','VARCHAR(12)') IS NULL THEN NULL WHEN lista.elemento.query('./RUT_PACIENTE').value('.','VARCHAR(12)') = '' THEN NULL ELSE lista.elemento.query('./RUT_PACIENTE').value('.','VARCHAR(12)') END,
		CASE WHEN lista.elemento.query('./DIGITO_VERIFICADOR').value('.','VARCHAR(1)') IS NULL THEN NULL WHEN lista.elemento.query('./DIGITO_VERIFICADOR').value('.','VARCHAR(1)') = '' THEN NULL ELSE lista.elemento.query('./DIGITO_VERIFICADOR').value('.','VARCHAR(1)') END,
		lista.elemento.query('./NOMBRE_PACIENTE').value('.','VARCHAR(100)'),
		lista.elemento.query('./APELLIDO_PATERNO').value('.','VARCHAR(100)'),
		lista.elemento.query('./APELLIDO_MATERNO').value('.','VARCHAR(100)'),
		CASE lista.elemento.query('./SEXO').value('.','VARCHAR(1)') 
			WHEN 'M' THEN 0 
			WHEN 'F' THEN 1
			WHEN 'I' THEN 2
			WHEN '' THEN NULL
			WHEN NULL THEN NULL
			ELSE 3 
		END,
		lista.elemento.query('./FECHA_NACIMIENTO').value('.','VARCHAR(25)'),
		lista.elemento.query('./FECHA_REGISTRO').value('.','VARCHAR(25)'),
		lista.elemento.query('./FECHA_FALLECIDO').value('.','VARCHAR(25)'),
		CASE WHEN lista.elemento.query('./FECHA_FALLECIDO').value('.','VARCHAR(25)') IS NULL THEN 0 WHEN lista.elemento.query('./FECHA_FALLECIDO').value('.','VARCHAR(25)') = '' THEN 0 ELSE 1 END,
		lista.elemento.query('./BORRADO_LOGICO').value('.','VARCHAR(1)'),
		lista.elemento.query('./DIRECCION_CALLE').value('.','VARCHAR(100)'),
		lista.elemento.query('./DIRECCION_NUMERO').value('.','VARCHAR(25)'),
		lista.elemento.query('./DIRECCION_COMPLEMENTO').value('.','VARCHAR(256)'),
		lista.elemento.query('./DIRECCION_CODIGO_COMUNA').value('.','VARCHAR(8)'),
		lista.elemento.query('./FONO_FIJO_1').value('.','VARCHAR(25)'),
		lista.elemento.query('./FONO_FIJO_2').value('.','VARCHAR(25)'),
		lista.elemento.query('./FONO_MOVIL_1').value('.','VARCHAR(25)'),
		lista.elemento.query('./FONO_MOVIL_2').value('.','VARCHAR(25)'),
		lista.elemento.query('./CODIGO_POSTAL').value('.','VARCHAR(7)'),
		lista.elemento.query('./EMAIL').value('.','VARCHAR(100)'),
		lista.elemento.query('./CODIGO_ISAPRE').value('.','VARCHAR(8)'),
		lista.elemento.query('./OTRO_ISAPRE').value('.','VARCHAR(100)'),		
		lista.elemento.query('./CODIGO_CONSULTORIO').value('.','VARCHAR(10)'),
		lista.elemento.query('./AREA_1').value('.','VARCHAR(2)'),
		lista.elemento.query('./AREA_2').value('.','VARCHAR(2)'),
		lista.elemento.query('./TIPO_PUERTA').value('.','VARCHAR(100)'),
		lista.elemento.query('./TIPO_PACIENTE').value('.','VARCHAR(100)'),
		lista.elemento.query('./MUESTRA_FECHA').value('.','VARCHAR(25)'),
		lista.elemento.query('./ENVIO_LABORATORIO_FECHA').value('.','VARCHAR(25)'),
		lista.elemento.query('./ID_USER_CREADOR').value('.','VARCHAR(200)'),		
		lista.elemento.query('./CODIGO_REGISTRO_EXTRA').value('.','VARCHAR(35)'),
		lista.elemento.query('./CODIGO_LABORATORIO').value('.','VARCHAR(8)'),
		lista.elemento.query('./RECEPCION_FECHA').value('.','VARCHAR(25)'),
		lista.elemento.query('./PROCESAMIENTO_FECHA').value('.','VARCHAR(25)'),
		lista.elemento.query('./RESGITRO_RESULTADO').value('.','VARCHAR(3)'),
		lista.elemento.query('./REGISTRO_RESULTADO_FECHA').value('.','VARCHAR(25)'),
		NULL,
		NULL,
		NULL
	FROM @xmlCarga.nodes('/ArrayOfXmlCargaMasiva/XmlCargaMasiva') AS lista(elemento) 
	
	--SE DECLARA ENTERO PARA RECORRER LA TABLA
	DECLARE @CANT INT 
	SET @CANT = (SELECT COUNT(*) FROM #TMPEXCEL)
	DECLARE @I INT
	SET @I = 1
	
	--SE RECORRE LA TABLA
	WHILE(@I <= @CANT)	
	BEGIN		
		DECLARE @CODIGO_IDENTIDAD			VARCHAR(20)			
		DECLARE @ID_ESTABLECIMIENTO			VARCHAR(6)							
		DECLARE @CODIGO_PAIS				VARCHAR(4)				 
		DECLARE @RUT_PACIENTE				VARCHAR(12)			
		DECLARE @DIGITO_VERIFICADOR			VARCHAR(1)			
		DECLARE @NOMBRE_PACIENTE			VARCHAR(100)		
		DECLARE @APELLIDO_PATERNO			VARCHAR(100)		
		DECLARE @APELLIDO_MATERNO			VARCHAR(100)		
		DECLARE @SEXO						VARCHAR(2)			
		DECLARE @FECHA_NACIMIENTO			VARCHAR(20)			
		DECLARE @FECHA_REGISTRO				VARCHAR(20)			
		DECLARE @FECHA_FALLECIDO			VARCHAR(20)		
		DECLARE @FALLECIDO					BIT	
		DECLARE @BORRADO_LOGICO				VARCHAR(1)			
		DECLARE @DIRECCION_CALLE			VARCHAR(100)		
		DECLARE @DIRECCION_NUMERO			VARCHAR(25)			
		DECLARE @DIRECCION_COMPLEMENTO		VARCHAR(256)		
		DECLARE @CODIGO_COMUNA				VARCHAR(8)			
		DECLARE @FONO_FIJO_1				VARCHAR(25)			
		DECLARE @FONO_FIJO_2				VARCHAR(25)			
		DECLARE @FONO_MOVIL_1				VARCHAR(25)			
		DECLARE @FONO_MOVIL_2				VARCHAR(25)			
		DECLARE @CODIGO_POSTAL				VARCHAR(7)			
		DECLARE @EMAIL						VARCHAR(100)		
		DECLARE @CODIGO_ISAPRE				VARCHAR(8)			
		DECLARE @OTRO_ISAPRE				VARCHAR(100)		
		DECLARE @CODIGO_CONSULTORIO			VARCHAR(10)			
		DECLARE @AREA_1						VARCHAR(2)			
		DECLARE @AREA_2						VARCHAR(2)	
		DECLARE	@TIPO_PUERTA				VARCHAR(100)		
		DECLARE @TIPO_PACIENTE				VARCHAR(100)		
		DECLARE @MUESTRA_FECHA				VARCHAR(20)			
		DECLARE @ENVIO_LABORATORIO_FECHA	VARCHAR(20)			
		DECLARE @ID_USER_CREADOR			VARCHAR(50)			
		DECLARE @CODIGO_REGISTRO_EXTRA		VARCHAR(35)			
		DECLARE @CODIGO_LABORATORIO			VARCHAR(8)			
		DECLARE @RECEPCION_FECHA			VARCHAR(20)			
		DECLARE @PROCESAMIENTO_FECHA		VARCHAR(20)			
		DECLARE @REGISTRO_RESULTADO			VARCHAR(3)			
		DECLARE @REGISTRO_RESULTADO_FECHA	VARCHAR(20)			
		
		--SE CAPTURAN EN VARIABLES LOS VALORES POR FILA
		SELECT 
			@CODIGO_IDENTIDAD = CODIGO_IDENTIDAD,								
			@ID_ESTABLECIMIENTO	= T.ID_ESTABLECIMIENTO,										
			@CODIGO_PAIS = T.CODIGO_PAIS,							 
			@RUT_PACIENTE = T.RUT_PACIENTE,						
			@DIGITO_VERIFICADOR	= T.DIGITO_VERIFICADOR,				
			@NOMBRE_PACIENTE = T.NOMBRE_PACIENTE,				
			@APELLIDO_PATERNO = T.APELLIDO_PATERNO,				
			@APELLIDO_MATERNO = T.APELLIDO_MATERNO,				
			@SEXO = T.SEXO,								
			@FECHA_NACIMIENTO = T.FECHA_NACIMIENTO,					
			@FECHA_REGISTRO = T.FECHA_REGISTRO,				
			@FECHA_FALLECIDO = T.FECHA_FALLECIDO,	
			@FALLECIDO = T.FALLECIDO,			
			@BORRADO_LOGICO = T.BORRADO_LOGICO,						
			@DIRECCION_CALLE = T.DIRECCION_CALLE,				
			@DIRECCION_NUMERO = T.DIRECCION_NUMERO,				
			@DIRECCION_COMPLEMENTO = T.DIRECCION_COMPLEMENTO,	
			@CODIGO_COMUNA = T.CODIGO_COMUNA,						
			@FONO_FIJO_1 = T.FONO_FIJO_1,							
			@FONO_FIJO_2 = T.FONO_FIJO_2,							
			@FONO_MOVIL_1 = T.FONO_MOVIL_1,			
			@FONO_MOVIL_2 = T.FONO_MOVIL_2,				
			@CODIGO_POSTAL = T.CODIGO_POSTAL,						
			@EMAIL = T.EMAIL,				
			@CODIGO_ISAPRE = T.CODIGO_ISAPRE,						
			@OTRO_ISAPRE = T.OTRO_ISAPRE,				
			@CODIGO_CONSULTORIO = T.CODIGO_CONSULTORIO,					
			@AREA_1 = T.AREA_1,							
			@AREA_2	= T.AREA_2,			
			@TIPO_PUERTA = T.TIPO_PUERTA,					
			@TIPO_PACIENTE = T.TIPO_PACIENTE,				
			@MUESTRA_FECHA = T.MUESTRA_FECHA,				
			@ENVIO_LABORATORIO_FECHA = ENVIO_LABORATORIO_FECHA,	
			@ID_USER_CREADOR = ID_USER_CREADOR,	
			@CODIGO_REGISTRO_EXTRA = T.CODIGO_REGISTRO_EXTRA,
			@CODIGO_LABORATORIO = T.CODIGO_LABORATORIO,
			@RECEPCION_FECHA = T.RECEPCION_FECHA,							
			@PROCESAMIENTO_FECHA = T.PROCESAMIENTO_FECHA,	
			@REGISTRO_RESULTADO	= T.REGISTRO_RESULTADO,
			@REGISTRO_RESULTADO_FECHA = T.REGISTRO_RESULTADO_FECHA			
		FROM
			#TMPEXCEL AS T
		WHERE
			T.ID = @I			
			
		--SE BUSCA NOMBRE DE USUARIO PARA GUARDARLO EN USUARIO_PROCESO
		DECLARE @USERNAME VARCHAR(20)
		SET @USERNAME = (SELECT 
							AU.UserName
						 FROM 
							MINSAL_SURVIH_SEC.dbo.aspnet_Users AS AU
						 WHERE AU.UserId = @ID_USER_CREADOR)
						 		
		--SE VALIDA LOS DATOS DEL PACIENTE
		DECLARE @MENSAJE VARCHAR(MAX)
		DECLARE @ID_PACIENTE BIGINT
		SET @MENSAJE = (SELECT dbo.ValidarDatosPaciente (
										@CODIGO_IDENTIDAD,
										@CODIGO_PAIS,							 
										@RUT_PACIENTE,						
										@DIGITO_VERIFICADOR,				
										@NOMBRE_PACIENTE,				
										@APELLIDO_PATERNO,				
										@APELLIDO_MATERNO,				
										@SEXO,								
										@FECHA_NACIMIENTO,				
										@FECHA_FALLECIDO,						
										@DIRECCION_CALLE,				
										@DIRECCION_NUMERO,				
										@DIRECCION_COMPLEMENTO,	
										@CODIGO_COMUNA,						
										@FONO_FIJO_1,							
										@FONO_FIJO_2,							
										@FONO_MOVIL_1,			
										@FONO_MOVIL_2,				
										@CODIGO_POSTAL,						
										@EMAIL,				
										@CODIGO_ISAPRE,						
										@OTRO_ISAPRE,				
										@CODIGO_CONSULTORIO,					
										@AREA_1,							
										@AREA_2))
										
		-- SI LA VALIDACION ENTREGA MENSAJE SE ACTUALIZA TABLA (CAMPO MENSAJE_PACIENTE) Y SE PASA A LA SIGUIENTE FILA
		IF(@MENSAJE IS NOT NULL)
		BEGIN
			UPDATE
				T
			SET 
				T.MENSAJE_PACIENTE = @MENSAJE,
				T.ESTADO_GUARDAR = 0
			FROM	
				#TMPEXCEL AS T
			WHERE
				ID = @I			
		END
		ELSE
		BEGIN				
			BEGIN TRAN FILA-- INICIO TRANSACCION
				BEGIN TRY
					-- SE INSERTA O SE ACTUALIZA PACIENTE, SE DEVUELVE MENSAJE
					 EXEC dbo.setXMLPacienteCargaMasiva
						@ID_ESTABLECIMIENTO,
						@CODIGO_PAIS,
						@RUT_PACIENTE,
						@DIGITO_VERIFICADOR,	
						@NOMBRE_PACIENTE,
						@APELLIDO_PATERNO,
						@APELLIDO_MATERNO,
						@SEXO,
						@FECHA_NACIMIENTO,
						@FALLECIDO,	
						@FECHA_FALLECIDO,
						@DIRECCION_CALLE,
						@DIRECCION_NUMERO,
						@DIRECCION_COMPLEMENTO,
						@CODIGO_COMUNA,
						@FONO_FIJO_1,
						@FONO_FIJO_2,
						@FONO_MOVIL_1,
						@FONO_MOVIL_2,	
						@CODIGO_POSTAL,
						@EMAIL,
						@CODIGO_ISAPRE,
						@OTRO_ISAPRE,
						@CODIGO_CONSULTORIO,
						@AREA_1,
						@AREA_2,
						@I,
						@ID_USER_CREADOR,
						@USERNAME,
						@MENSAJE OUTPUT,
						@ID_PACIENTE OUTPUT,
						@CODIGO_IDENTIDAD OUTPUT
					
					-- MENSAJE DE ACTUALIZACION O INCERSION DE DATOS PACIENTE
					UPDATE
						T
					SET 
						T.MENSAJE_PACIENTE = @MENSAJE,
						T.ESTADO_GUARDAR = 1
					FROM	
						#TMPEXCEL AS T
					WHERE
						ID = @I
					
					--SE CIERRA CASOS PENDIENTES SI EL PACIENTE ESTA FALLECIDO
					IF(@FALLECIDO = 1)
					BEGIN
						EXEC dbo.setCerrarCasosPendientesFallecido @ID_PACIENTE,@USERNAME
					END
					IF((@MUESTRA_FECHA IS NULL) OR (@MUESTRA_FECHA = '')) 
					BEGIN
						SET @MENSAJE = 'La??Fecha??de??Toma??de??Muestra??esta??vacio.'
						
						UPDATE
							T
						SET 
							T.MENSAJE_MUESTRA = @MENSAJE,
							T.ESTADO_GUARDAR = 0
						FROM	
							#TMPEXCEL AS T
						WHERE
							ID = @I
					END
					ELSE IF(((@MUESTRA_FECHA IS NOT NULL) AND (@MUESTRA_FECHA <> '')) AND (ISDATE(@MUESTRA_FECHA) = 0))
					BEGIN
						SET @MENSAJE = 'La Fecha de Toma de Muestra tiene caracteres invalidos.'
						
						UPDATE
							T
						SET 
							T.MENSAJE_MUESTRA = @MENSAJE,
							T.ESTADO_GUARDAR = 0
						FROM	
							#TMPEXCEL AS T
						WHERE
							ID = @I
					END
					ELSE
					BEGIN
						-- CREAR CODIGO DE REGISTRO MUESTRA
						DECLARE @CR VARCHAR(60)
						DECLARE @CODIGO_ESTABLECIMIENTO VARCHAR(50)
						DECLARE @SECUENCIA_REGISTRO INT
						
						SET @CODIGO_ESTABLECIMIENTO = (SELECT E.CODIGO_ESTABLECIMIENTO FROM ESTABLECIMIENTO AS E WHERE E.ID_ESTABLECIMIENTO = @ID_ESTABLECIMIENTO)
						SET @SECUENCIA_REGISTRO = (SELECT dbo.OBTENER_SECUENCIA_REGISTRO(@ID_ESTABLECIMIENTO))
						SET @CR = (@CODIGO_ESTABLECIMIENTO + '-' + CONVERT(VARCHAR(5),YEAR(@MUESTRA_FECHA)) + '-' + CONVERT(VARCHAR(2),MONTH(@MUESTRA_FECHA)) + '-' + CONVERT(VARCHAR(10),@SECUENCIA_REGISTRO) + CONVERT(VARCHAR(20), CASE WHEN @CODIGO_REGISTRO_EXTRA IS NULL THEN '' ELSE '-' + @CODIGO_REGISTRO_EXTRA END))
						
						--SE VALIDA LOS DATOS DE MUESTRA Y PROCESAMIENTO
						SET @MENSAJE = (SELECT dbo.ValidarDatosMuestra (
															@ID_PACIENTE,
															@ID_ESTABLECIMIENTO,
															@MUESTRA_FECHA,	
															@FECHA_FALLECIDO,
															@FECHA_NACIMIENTO,
															@ENVIO_LABORATORIO_FECHA,
															@CR,	
															@CODIGO_REGISTRO_EXTRA,
															@CODIGO_LABORATORIO,
															@RECEPCION_FECHA,
															@PROCESAMIENTO_FECHA,
															@REGISTRO_RESULTADO,
															@REGISTRO_RESULTADO_FECHA))
													
						-- SI LA VALIDACION ENTREGA MENSAJE SE ACTUALIZA TABLA (CAMPO MENSAJE_MUESTRA) Y SE PASA A LA SIGUIENTE FILA
						IF(@MENSAJE IS NOT NULL)
						BEGIN
							UPDATE
								T
							SET 
								T.MENSAJE_MUESTRA = @MENSAJE,
								T.ESTADO_GUARDAR = 0
							FROM	
								#TMPEXCEL AS T
							WHERE
								ID = @I
							
						END
						ELSE
						BEGIN
							IF((@CODIGO_LABORATORIO IS NULL) OR (@CODIGO_LABORATORIO = ''))
							BEGIN
								--SI LABORATORIO VIENE VACIO EN DOCUMENTO SE HACE LA BUSQUEDA POR ESTABLECIMIENTO DE USUARIO
								SET @CODIGO_LABORATORIO = (SELECT L.CODIGO_LABORATORIO FROM LABORATORIO AS L WHERE L.ID_ESTABLECIMIENTO = @ID_ESTABLECIMIENTO)
							END
							
							
							EXEC dbo.setXMLMuestraCargaMasiva 
											@ID_ESTABLECIMIENTO,
											@MUESTRA_FECHA,
											@ID_PACIENTE,
											@ENVIO_LABORATORIO_FECHA,
											@CR,	
											@SECUENCIA_REGISTRO,
											@ID_USER_CREADOR,
											@CODIGO_REGISTRO_EXTRA,
											@CODIGO_LABORATORIO,
											@RECEPCION_FECHA,
											@PROCESAMIENTO_FECHA,
											@REGISTRO_RESULTADO,
											@REGISTRO_RESULTADO_FECHA,	
											@FECHA_FALLECIDO,
											@FALLECIDO,
											@FECHA_NACIMIENTO,
											@USERNAME,
											@MENSAJE OUTPUT
							
							-- MENSAJE DE ACTUALIZACION O GUARDADO DE MUESTRA PACIENTE
							UPDATE
								T
							SET 
								T.MENSAJE_MUESTRA = @MENSAJE,
								T.ESTADO_GUARDAR = 1
							FROM	
								#TMPEXCEL AS T
							WHERE
								ID = @I					
						END	
					END	
					
					COMMIT TRAN FILA -- SE CONFIRMA TRANSACCION	
				END TRY
				BEGIN CATCH
					ROLLBACK TRAN FILA-- SE CANCELA TRANSACCION
					SET @MENSAJE = 'Error de conexion: ' + ERROR_MESSAGE() + ', ejecutar de nuevo, si el error se mantiene comunicarse con soporte'
					UPDATE
							T
						SET 
							T.MENSAJE_MUESTRA = @MENSAJE,
							T.ESTADO_GUARDAR = 0
						FROM	
							#TMPEXCEL AS T
						WHERE
							ID = @I	
					--SET IMPLICIT_TRANSACTIONS OFF 
				END CATCH				
		END	
				
		SET @I = @I + 1			
	END	
		
	SELECT 
		ID,
		MENSAJE_PACIENTE,
		MENSAJE_MUESTRA,
		ESTADO_GUARDAR
	FROM 
		#TMPEXCEL
	
	DROP TABLE #TMPEXCEL
END

