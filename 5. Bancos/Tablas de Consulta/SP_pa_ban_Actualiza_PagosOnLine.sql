USE [Bancos]
GO
/****** Object:  StoredProcedure [dbo].[pa_ban_Actualiza_PagosOnLine]    Script Date: 04/05/2020 10:43:39 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[pa_ban_Actualiza_PagosOnLine]
AS
BEGIN
--PRODUCCION
UPDATE [SBOGCHE009V].[CHEVYPLAN].[dbo].[CHP_CONFIRMACION_PAGO] 
SET reportado = 1, Lote = convert(varchar,getdate(),112)
WHERE codigo_autorizacion = 'SUCCESS' and reportado = 0 AND
CONVERT(DATE,fecha_transaccion) <  convert(date, GETDATE()) and TIPO_PAGO = 1

--NUEVO

UPDATE [SBOGCHE009V].[CHEVYPLAN].[dbo].[CHP_CONFIRMACION_PAGO_ELIMINADOS] 
SET reportado = 1, Lote = convert(varchar,getdate(),112)
where  
  ISNUMERIC(CUOTAS) > 0 AND
  CONVERT(DATE,fecha_transaccion) < convert(date, GETDATE())
  and reportado = 0 and TIPO_PAGO = 1 and ESTADO_POL = 4
  and len(REF_VENTA) = 11


END

