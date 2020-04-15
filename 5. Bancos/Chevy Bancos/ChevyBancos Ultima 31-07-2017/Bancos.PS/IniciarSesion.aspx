<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IniciarSesion.aspx.cs" Inherits="Bancos.PS.IniciarSesion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Validando Usuario...</title>
    <link href="MarcaVisual/estilos/estilos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var anchoPantalla = screen.width;
        var altoPantalla = screen.height;

        x = (anchoPantalla - 900) / 2;
        y = (altoPantalla - 700) / 2;
        propiedades = "width=900, height=700, top=" + y + ", left=" + x + ", resize=no, status=no, toolbar=no, menubar=no, location=no, scrollbars=no";
        window.open("Modulos/MenuPrincipal.aspx", "", propiedades);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
