<%@ Page Title="BS - Cadenas de Conexión y Otros" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master"
    AutoEventWireup="true" CodeBehind="CadenaCX.aspx.cs" Inherits="Bancos.PS.Modulos.Administracion.CadenaCX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <script language="javascript" type="text/javascript">
    function validarCaracteres(textAreaControl, maxTam)
    {
        if (textAreaControl.value.length > maxTam)
        {
            textAreaControl.value = textAreaControl.value.substring(0, maxTam);
            alert("Debe ingresar hasta un máximo de " + maxTam + " carácteres!!!");
        }
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
    <table align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnHome" runat="server" ImageUrl="~/MarcaVisual/iconos/home.png" Width="16px"
                    ToolTip="Ir a la página principal" PostBackUrl="~/Modulos/MenuPrincipal.aspx"/>
            </td>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnGuardar" runat="server" ImageUrl="~/MarcaVisual/iconos/guardar.png" Width="16px"
                    ToolTip="Guardar" ValidationGroup="1" OnClick="imgBtnGuardar_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <table style="width: 100%" cellpadding="0" cellspacing="0">
        <tr>
            <td class="BarraSubTitulo">
                Cadenas de Conexión
            </td>
        </tr>
        <tr>
            <td class="SeparadorSubTitulo">
            </td>
        </tr>
    </table>
    <table class="ContenedorDatos" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="3" style="height: 10px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            <asp:Panel ID="pnlCadenas" runat="server" ScrollBars="Auto" Width="100%" Height="620px">
                                <asp:Panel ID="pnlDatos" runat="server" CssClass="PanelBordesRedondos" Width="99%">
                                    <table cellpadding="0" cellspacing="2">
                                        <tr>
                                            <td class="LetraLeyendaColor" colspan="4">
                                                Configuraciones Globales
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial">
                                            </td>
                                            <td style="text-align: right; vertical-align: top">
                                                <asp:Label ID="lbCadenaCX" runat="server" Font-Size="X-Small" Text="Cadena de Conexión:"></asp:Label>
                                            </td>
                                            <td class="EspaciadoIntermedio">
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txbCadenaCX" runat="server" CssClass="EstiloTextoMultilinea" Height="50px"
                                                    TextMode="MultiLine" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial">
                                            </td>
                                            <td style="text-align: right; vertical-align: middle">
                                                <asp:Label ID="lbServidorDA" Font-Size="X-Small" runat="server" Text="Servidor Directorio Activo:"></asp:Label>
                                            </td>
                                            <td class="EspaciadoIntermedio">
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txbServidorDA" runat="server" CssClass="BordeControles" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial">
                                            </td>
                                            <td style="text-align: right; vertical-align: middle">
                                                <asp:Label ID="lbDominio" runat="server" Font-Size="X-Small" Text="Dominio:"></asp:Label>
                                            </td>
                                            <td class="EspaciadoIntermedio">
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txbDominio" runat="server" CssClass="BordeControles" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:RoundedCornersExtender ID="rceEsquinasRedondas" runat="server" BorderColor="181, 198, 214"
                                    Enabled="True" Radius="3" TargetControlID="pnlDatos">
                                </asp:RoundedCornersExtender>
                                <br />
                                <asp:Panel ID="pnlConfigMail" runat="server" CssClass="PanelBordesRedondos" Width="370px">
                                    <table cellpadding="0" cellspacing="2">
                                        <tr>
                                            <td class="LetraLeyendaColor" colspan="4">
                                                Configuraciones para Correo
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial">
                                            </td>
                                            <td style="vertical-align: middle" class="EstiloEtiquetas">
                                                <asp:Label ID="lbServidor" runat="server" Font-Size="X-Small" Text="Servidor Correo:"></asp:Label>
                                            </td>
                                            <td class="EspaciadoIntermedio">
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txbServidor" runat="server" Width="200px" CssClass="BordeControles"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial">
                                            </td>
                                            <td style="vertical-align: middle" class="EstiloEtiquetas">
                                                <asp:Label ID="lbCorreo" runat="server" Font-Size="X-Small" Text="Correo de Distribución:"></asp:Label>
                                            </td>
                                            <td class="EspaciadoIntermedio">
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txbCorreo" runat="server" Width="200px" CssClass="BordeControles"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:RoundedCornersExtender ID="rceConfigMail" runat="server" BorderColor="181, 198, 214"
                                    Enabled="True" Radius="3" TargetControlID="pnlConfigMail">
                                </asp:RoundedCornersExtender>
                                <br />
                                <asp:Panel ID="pnlUsuario" runat="server" CssClass="PanelBordesRedondos" Width="300px">
                                    <table cellpadding="0" cellspacing="2">
                                        <tr>
                                            <td class="LetraLeyendaColor" colspan="4">
                                                Usuario Administrador
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial">
                                            </td>
                                            <td class="EstiloEtiquetas">
                                                <asp:Label ID="lbNombreUsuario" runat="server" Font-Size="X-Small" Text="Usuario:"></asp:Label>
                                            </td>
                                            <td class="EspaciadoIntermedio">
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txbUsuario" runat="server" CssClass="BordeControles"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="EspaciadoInicial">
                                            </td>
                                            <td class="EstiloEtiquetas">
                                                <asp:Label ID="lbClave" runat="server" Font-Size="X-Small" Text="Contraseña:"></asp:Label>
                                            </td>
                                            <td class="EspaciadoIntermedio">
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txbClave" runat="server" CssClass="BordeControles" TextMode="Password"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:RoundedCornersExtender ID="rceUsuarios" runat="server" BorderColor="181, 198, 214"
                                    Enabled="True" Radius="3" TargetControlID="pnlUsuario">
                                </asp:RoundedCornersExtender>
                            </asp:Panel>
                        </td>
                        <td style="width: 10px">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
