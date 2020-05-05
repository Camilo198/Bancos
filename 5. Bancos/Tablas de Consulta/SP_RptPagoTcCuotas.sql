USE [Ventas]
GO

/****** Object:  StoredProcedure [dbo].[RptPagoTcCuotas]    Script Date: 04/05/2020 10:30:40 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[RptPagoTcCuotas] 

@FecDesde VARCHAR(20) = null,
@FecHasta VARCHAR(20) = null, 
@Legalizado char(1) = null,
@NumAutorizacion varchar(50) = null,
@Contrato varchar(10) = null
AS
SET DATEFORMAT  dmy  
IF @Legalizado is null or @Legalizado = '' set @Legalizado= '%'  
IF @FecDesde is null or @FecDesde ='' set @FecDesde= '01/01/1950'  
IF @FecHasta is null or @FecHasta ='' set @FecHasta= '31/12/2100' 
IF @NumAutorizacion is null or @NumAutorizacion ='' set @NumAutorizacion= '%' 
IF @Contrato is null or @Contrato ='' set @Contrato = '%' 

BEGIN 
	DECLARE @query as varchar (max)   
	declare @Servidor varchar(100)
	declare @BaseDatos varchar(100)	
	
select @Servidor = Servidor, @BaseDatos= BaseDatos from BAS_ServidoresBasesDatos where IDServidoresBasesDatos=1

DECLARE @Table table (
CodUnico	varchar(20),
Franquicia	varchar(50),
Cuenta	varchar(50),
FecVale	datetime,
FecProceso	datetime,
FecAbono	datetime,
NumTarjeta	varchar(20),
Hora	varchar(20),
Comprobante	varchar(20),
NumAutorizacion	varchar(20),
Terminal	varchar(20),
ValCompra	numeric,
ValIva	numeric,
ValPropina	numeric,
ValTotal	numeric,
ValComision	numeric,
ValRetIva	numeric,
ValRetIca	numeric,
ValRetFuente numeric,
ValAbono numeric,
TipTarjeta	varchar(50),
Plazo	varchar(20),
PorComision	varchar(20),
Referencia	varchar(20),
Concesionario	varchar(100),
OBSERVACION	varchar(200),
Legalizado	varchar(1),
ESTADO	varchar(1),
Marca	varchar(10)
)


 SET @query = N'
 SELECT DISTINCT 
 PagoTarjetaCredito.CodUnico,PagoTarjetaCredito.Franquicia,PagoTarjetaCredito.Cuenta,PagoTarjetaCredito.FecVale,PagoTarjetaCredito.FecProceso,
 PagoTarjetaCredito.FecAbono,RIGHT(NumTarjeta,4) AS NumTarjeta,PagoTarjetaCredito.Hora,PagoTarjetaCredito.Comprobante,PagoTarjetaCredito.NumAutorizacion,
 PagoTarjetaCredito.Terminal,PagoTarjetaCredito.ValVenta as ValCompra,PagoTarjetaCredito.ValIva,PagoTarjetaCredito.ValPropina, 
 (PagoTarjetaCredito.ValTotal) as ValTotal,PagoTarjetaCredito.ValComision,PagoTarjetaCredito.ValRetIva,PagoTarjetaCredito.ValRetIca,
 PagoTarjetaCredito.ValRetFuente,PagoTarjetaCredito.ValAbono,PagoTarjetaCredito.TipTarjeta, PagoTarjetaCredito.Plazo,PagoTarjetaCredito.PorComision,
 CASE WHEN PagoTarjetaCredito.Legalizado=''N'' --OR PagoTarjetaCredito.Legalizado=''A''
 THEN '''' 
 ELSE 
 Convert(varchar,PagosCuota.Referencia,103) END AS Contrato, 
 CASE WHEN PagoTarjetaCredito.Legalizado=''N'' OR PagoTarjetaCredito.Legalizado=''A'' THEN '''' ELSE
 TEMP_OFIC.OficNombre END AS Concesionario, 
 
 CASE WHEN --PagosCuota.Estado = ''V'' and 
 (PagoTarjetaCredito.Legalizado <> ''A'' and PagoTarjetaCredito.Legalizado=''N'')
 THEN '''' 
 ELSE 
 CASE WHEN Anulaciones.Proceso = ''Boucher'' 
 THEN Anulaciones.Observaciones 
 ELSE A2.Observaciones 
 END
 END AS OBSERVACION,    
 PagoTarjetaCredito.Legalizado,
 CASE WHEN PagoTarjetaCredito.Legalizado=''N'' --OR PagoTarjetaCredito.Legalizado=''A'' 
 --and PagosCuota.Estado =''V'' 
 THEN
 ''''
 ELSE
 PagosCuota.estado
 END
 AS ESTADO,  
CASE WHEN PagosErrados.Proceso IS NULL THEN '''' ELSE ''Errado'' end as Marca    
 FROM PagoTarjetaCredito
 
 LEFT JOIN PagosCuota ON  PagoTarjetaCredito.NumAutorizacion=REPLACE(LTRIM(REPLACE(PagosCuota.NumAutorizacion,''0'','' '')),'' '',''0'') collate SQL_Latin1_General_CP1_CI_AS
 AND PagoTarjetaCredito.ValTotal = (PagosCuota.ValPago+PagosCuota.ValComision+PagosCuota.ValRetFuente+PagosCuota.ValRetIca+PagosCuota.ValRetIva) --Union con la tabla de PagosCuota
 
 LEFT JOIN Cierre ON PagosCuota.Referencia = Cierre.CONTRATO  -- Union con la tabla de cierre
 LEFT JOIN [' + @Servidor +'].'+ @BaseDatos + '.[dbo].[TEMP_OFIC] ON Cierre.CONCESIONA = TEMP_OFIC.OficCodigo  --Union con la tabla de Oficinas
  
 LEFT JOIN Anulaciones ON convert(varchar,PagoTarjetaCredito.NumAutorizacion)  = convert(varchar,Anulaciones.Contrato) collate SQL_Latin1_General_CP1_CI_AS -- Union con la tabla de Anulaciones por baucher
 AND Anulaciones.Observaciones <> '''' and PagoTarjetaCredito.ValVenta = Anulaciones.ValPago
 
 
 LEFT JOIN Anulaciones A2 ON convert(varchar,PagosCuota.Referencia)  = convert(varchar,A2.Contrato) collate SQL_Latin1_General_CP1_CI_AS-- Union con la tabla de Anulaciones por Referencia
 AND A2.Observaciones <> '''' 
 AND A2.ValPago = PagosCuota.ValPago+PagosCuota.ValComision+PagosCuota.ValRetFuente+ PagosCuota.ValRetIca + PagosCuota.ValRetIva  
 
 LEFT JOIN [dbo].[PagosErrados] ON 
 PagosCuota.Referencia = PagosErrados.Referencia AND
 PagosCuota.ValPago+PagosCuota.ValComision+PagosCuota.ValRetFuente+ PagosCuota.ValRetIca + PagosCuota.ValRetIva = PagosErrados.ValorPago AND
 PagosCuota.FecPago = PagosErrados.FechaPago AND
 PagosCuota.ForPago = PagosErrados.ForPago collate SQL_Latin1_General_CP1_CI_AS 

 WHERE PagoTarjetaCredito.NumAutorizacion <> ''0'' '

 
 
	--VALIDACION POR FECHAS
	IF @Legalizado ='%'  and @NumAutorizacion = '%' and @Contrato = '%'
		BEGIN
		SET @query = @query + 'AND CAST(FecVale AS DATE) >= CAST('''+ CONVERT(VARCHAR(20),@FecDesde,103) +''' AS DATE) AND 
								   CAST(FecVale AS DATE) <= CAST('''+ CONVERT(VARCHAR(20),@FecHasta,103) +''' AS DATE)'
		--EXEC (@query)
		--SELECT (@query)
	END
 
	--VALIDACION POR LEGALIZADO  
	IF @Legalizado != '%'
		BEGIN
		SET @query = @query + 'AND  PagoTarjetaCredito.Legalizado  = '''+ @Legalizado +''''
		--EXEC (@query)
		--SELECT (@query)
	END
	
		--VALIDACION NUM AUTORIZACION 
	IF @NumAutorizacion != '%' 
		BEGIN    
		SET @query = @query + 'AND PagoTarjetaCredito.NumAutorizacion =  '''+ @NumAutorizacion +''''
		--EXEC (@query)
		--SELECT (@query)
	END
	
	--VALIDACION POR REFERENCIA 
	IF @Contrato != '%'  
		BEGIN    
		SET @query = @query + 'AND PagosCuota.Referencia =  '''+ @Contrato +''''
		--EXEC (@query)
		--SELECT (@query)
	END

    insert @Table exec(@Query)
    select * from @Table
    select(@query)

 END

 

GO


