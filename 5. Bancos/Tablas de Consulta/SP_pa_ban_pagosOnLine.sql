USE [Bancos]
GO
/****** Object:  StoredProcedure [dbo].[pa_ban_PagosOnLine]    Script Date: 04/05/2020 10:33:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[pa_ban_PagosOnLine]
@pFecLimite date
AS
BEGIN

SELECT 
REF_VENTA,REF_POL,VALOR,CONVERT(DATE,FECHA_TRANSACCION) AS FECHA, REF_VENTA,'' as MEDIO_PAGO_APLICA_SICO,
'' as MEDIO_PAGO_APLICA_VENTAS,'' as CODIGO_AUTORIZACION
FROM [SBOGCHE009V].[CHEVYPLAN].[dbo].[CHP_CONFIRMACION_PAGO] 
where codigo_autorizacion = 'SUCCESS' and reportado = 0 AND
 CONVERT(DATE,fecha_transaccion) < convert(date, GETDATE()) and TIPO_PAGO = 1
UNION ALL
  SELECT REF_VENTA,REF_POL,VALOR,CONVERT(DATE,FECHA_TRANSACCION) AS FECHA,REF_VENTA,
   CASE 
   WHEN MEDIO_PAGO = 10 then '29' --VISA
   WHEN MEDIO_PAGO = 12 then '42' --AMEX 
   WHEN MEDIO_PAGO = 11 then '29' --MASTERCARD 
   WHEN MEDIO_PAGO = 22 then '41' --DINNERS
   end as
   MEDIO_PAGO_APLICA_SICO, 
   CASE 
   WHEN MEDIO_PAGO = 10 then '43' --VISA
   WHEN MEDIO_PAGO = 12 then '45' --AMEX 
   WHEN MEDIO_PAGO = 11 then '43' --MASTERCARD 
   WHEN MEDIO_PAGO = 22 then '44' --DINNERS
   end as
   MEDIO_PAGO_APLICA_VENTAS,
   CODIGO_AUTORIZACION
  FROM [SBOGCHE009V].[CHEVYPLAN].[dbo].[CHP_CONFIRMACION_PAGO_ELIMINADOS] --sv=SBOGCHE009V  db=CHEVYPLAN
  where  
  ISNUMERIC(CUOTAS) > 0 AND
  CONVERT(DATE,fecha_transaccion) < convert(date, GETDATE())
  and reportado = 0 and TIPO_PAGO = 1 and ESTADO_POL = 4
  and len(REF_VENTA) = 11
  order by FECHA desc

END


