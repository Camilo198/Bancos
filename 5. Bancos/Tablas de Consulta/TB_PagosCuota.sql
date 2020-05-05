USE [Ventas]
GO

/****** Object:  Table [dbo].[PagosCuota]    Script Date: 04/05/2020 10:23:34 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PagosCuota](
	[CodBanco] [int] NULL,
	[FecPago] [datetime] NULL,
	[ValPago] [numeric](18, 2) NULL,
	[ForPago] [varchar](10) NULL,
	[Referencia] [numeric](10, 0) NULL,
	[Estado] [char](1) NULL,
	[UsuProceso] [varchar](30) NULL,
	[FecProceso] [datetime] NULL,
	[ContAnterior] [numeric](10, 0) NULL,
	[UsuModifico] [varchar](30) NULL,
	[FecModifico] [datetime] NULL,
	[ValComision] [numeric](18, 2) NULL,
	[ValRetFuente] [numeric](18, 2) NULL,
	[ValRetIva] [numeric](18, 2) NULL,
	[ValRetIca] [numeric](18, 2) NULL,
	[NumAutorizacion] [varchar](20) NULL,
	[HoraPago] [char](4) NULL,
	[NumLote] [numeric](18, 0) NULL,
	[Legalizado] [char](1) NULL,
	[EstMigracion] [varchar](1) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


