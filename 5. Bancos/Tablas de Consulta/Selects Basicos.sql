use Bancos
select * from tb_BAN_BANCO where TIPO_PROCESO like 'POL_' and LugPago=66

  SELECT * FROM tb_BAN_PAGOS WHERE Recaudo='T' 

  select * from tb_BAN_ESTRUCTURA_ARCHIVO where Configuracion=66
--update tb_BAN_PAGOS set Disponibilidad=0
--delete from tb_BAN_RECAUDOS where FechaProceso>='18/04/2020'
select * from tb_BAN_PAGOS where Disponibilidad=1
select * delete from tb_BAN_RECAUDOS where FechaProceso>='21/04/2020'
select * from tb_BAN_PAGOS where Disponibilidad=1

use VentasPruebas
select *  from PagosCuota where NumAutorizacion='680359'       
select * from PagoTarjetaCredito  where NumAutorizacion='680359' order by FecProceso desc
use PaginaWeb_DR
select * from CHP_CONFIRMACION_PAGO
select * from CHP_CONFIRMACION_PAGO_ELIMINADOS

