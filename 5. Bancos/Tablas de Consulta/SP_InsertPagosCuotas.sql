USE [Ventas]
GO
/****** Object:  StoredProcedure [dbo].[InsertPagosCuotas]    Script Date: 04/05/2020 10:27:19 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
		
								
CREATE procedure [dbo].[InsertPagosCuotas]
			
		    @CodBanco int
           ,@FecPago datetime
           ,@ValPago numeric(18,2)
           ,@ForPago varchar(10)
           ,@Contrato numeric(10,0)
           ,@Estado char(1)
           ,@UsuProceso varchar(30)
           ,@ContAnterior numeric(10,0) 
           ,@UsuModifico varchar(30) = null
           ,@FecModifico datetime = null
           ,@ValComision numeric(18,2) = null
           ,@ValRetFuente numeric(18,2) = null
           ,@ValRetIva numeric(18,2) = null
           ,@ValRetIca numeric(18,2) = null
           ,@NumAutorizacion varchar(20)
           ,@HoraPago char(4) = null
           ,@NumLote numeric(18,0) = null
           ,@Legalizado char(1)
           ,@ValPagoAnt numeric(18, 2) = null
           ,@FecPagoAnt datetime = null
           ,@CodBancoAnt int = null
		   
          
AS
declare @EstadoMigracion Varchar(1) ='P'
INSERT INTO [PagosCuota]
           ([CodBanco]
           ,[FecPago]
           ,[ValPago]
           ,[ForPago]
           ,[Referencia]
		   ,[ContAnterior]
           ,[Estado]
           ,[UsuProceso]
           ,[FecProceso]
           ,[ValComision]
           ,[ValRetFuente]
           ,[ValRetIva]
           ,[ValRetIca]
           ,[NumAutorizacion]
           ,[HoraPago]
           ,[NumLote]
           ,[Legalizado]
		   ,[EstMigracion])
     VALUES
   
           (@CodBanco,
@FecPago,
@ValPago,
@ForPago,
@Contrato,
@ContAnterior,
@Estado,
@UsuProceso,
GETDATE(), 
@ValComision,
@ValRetFuente,
@ValRetIva,
@ValRetIca,
@NumAutorizacion,
@HoraPago,
@NumLote,
@Legalizado
,@EstadoMigracion
)
return @@rowcount
