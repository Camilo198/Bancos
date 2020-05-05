USE [Ventas]
GO

/****** Object:  StoredProcedure [dbo].[UpdateLegalizacionesPagoCuota]    Script Date: 04/05/2020 10:13:20 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[UpdateLegalizacionesPagoCuota]

AS

UPDATE [dbo].[PagosCuota] SET CodBanco = CASE 
--Fiducia 1
WHEN PagosCuota.CodBanco = 43 THEN 29 
WHEN PagosCuota.CodBanco = 45 THEN 42
WHEN PagosCuota.CodBanco = 44 THEN 41
END, 
ValPago = Valpago - (PagoTarjetaCredito.ValComision+PagoTarjetaCredito.ValRetFuente+PagoTarjetaCredito.ValRetIca+PagoTarjetaCredito.ValRetIva),
ValComision = PagoTarjetaCredito.ValComision,
ValRetFuente = PagoTarjetaCredito.ValRetFuente,
ValRetIca = PagoTarjetaCredito.ValRetIca,
ValRetIva = PagoTarjetaCredito.ValRetIva,
Legalizado = 'S'
FROM  PagoTarjetaCredito 
WHERE 
PagosCuota.CodBanco = PagoTarjetaCredito.CodBanco and
PagosCuota.ValPago =PagoTarjetaCredito.ValTotal  and 
format(PagosCuota.FecPago,'dd/MM/yyyy')  = format( PagoTarjetaCredito.FecVale,'dd/MM/yyyy') and 
REPLACE(LTRIM(REPLACE(PagosCuota.NumAutorizacion,'0',' ')),' ','0') = PagoTarjetaCredito.NumAutorizacion collate SQL_Latin1_General_CP1_CI_AS and 
PagoTarjetaCredito.Legalizado = 'N' and 
PagosCuota.ForPago = 'TARJETA' AND 
PagosCuota.Legalizado = 'N' AND 
PagosCuota.Estado = 'V'

UPDATE [dbo].[PagoTarjetaCredito] SET PagoTarjetaCredito.Legalizado = 'S'
FROM PagosCuota 
WHERE 
PagoTarjetaCredito.ValTotal  = PagosCuota.ValPago+PagosCuota.ValComision+PagosCuota.ValRetFuente+ PagosCuota.ValRetIca + PagosCuota.ValRetIva  and 
format(PagosCuota.FecPago,'dd/MM/yyyy')  = format( PagoTarjetaCredito.FecVale,'dd/MM/yyyy')   and
PagoTarjetaCredito.NumAutorizacion = REPLACE(LTRIM(REPLACE(PagosCuota.NumAutorizacion,'0',' ')),' ','0') collate SQL_Latin1_General_CP1_CI_AS and PagoTarjetaCredito.Legalizado = 'N' and PagosCuota.ForPago = 'TARJETA'
AND PagosCuota.Legalizado = 'S' AND PagosCuota.Estado = 'V'
RETURN @@rowcount

   


GO


