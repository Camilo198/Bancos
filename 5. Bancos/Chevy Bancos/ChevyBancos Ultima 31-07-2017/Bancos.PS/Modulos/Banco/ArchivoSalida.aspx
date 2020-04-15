<%@ Page Title="Bancos - Archivo de Asobancaria Salida" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master"
    AutoEventWireup="true" CodeBehind="ArchivoSalida.aspx.cs" Inherits="Bancos.PS.Modulos.Banco.ArchivoSalida" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">
                        Enviar y Generar Archivos de Salida
                    </td>
                </tr>
                <tr>
                    <td class="SeparadorSubTitulo">
                    </td>
                </tr>
            </table>
            <table style="width: 100%; height: 100%;" class="ColorContenedorDatos" cellpadding="0"
                cellspacing="0">
                <tr>
                    <td style="height: 10px" colspan="3">
                    </td>
                </tr>
                <tr>
                    <td style="width: 10px">
                    </td>
                    <td>
                        <asp:Panel ID="pnlInformacionBanco" runat="server" ScrollBars="Auto" Width="100%"
                            Height="620px">
                            <asp:Panel ID="pnlBanco" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                ScrollBars="None">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Banco
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lblTipoArchivo" runat="server" Text="Tipo de Archivo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoArchivo" CssClass="BordeListas" runat="server" 
                                                              ValidationGroup="1" Width="180px" AutoPostBack="True" 
                                                onselectedindexchanged="ddlTipoArchivo_SelectedIndexChanged">
                                                <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="ABF1">Asobancaria</asp:ListItem>
                                                <asp:ListItem Value="TRF1">Telefono Rojo</asp:ListItem>
                                            </asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="rfvTipoArchivo" runat="server" ErrorMessage="Favor seleccionar Tipo de Archivo!"
                                                ForeColor="Red" ControlToValidate="ddlTipoArchivo" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoArchivo" runat="server" Enabled="True" TargetControlID="rfvTipoArchivo"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>                                  
                                    </tr>                                   
                                    <tr>
                                        <td style="height: 10px" colspan="6">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lblCuenta" runat="server" Text="Nombre de la Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlNombreCuenta" CssClass="BordeListas" runat="server" 
                                                ValidationGroup="1" AutoPostBack="True" 
                                                OnSelectedIndexChanged="ddlNombreCuenta_SelectedIndexChanged" Width="180px" 
                                                Enabled="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>                                  
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="RoundedCornersExtender1" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlBanco" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlRutas" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                ScrollBars="None">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Rutas
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lblArchivoSalidaAso" runat="server" Text="Ruta Salida:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbArchivoSalidaAso" CssClass="BordeControles" runat="server" ValidationGroup="1"
                                                Enabled="False" Width="360px"></asp:TextBox>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>                                     
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="RoundedCornersExtender2" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlRutas" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlFtp" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                ScrollBars="None">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td style="height: 10px" colspan="6">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lblOpcionE" runat="server" Text="Enviar Información por:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbFTP" runat="server" Text="FTP" GroupName="OpcionEnvio" OnCheckedChanged="rb_CheckedChanged"
                                                AutoPostBack="True" Enabled="False" />
                                            <asp:RadioButton ID="rbCorreo" runat="server" GroupName="OpcionEnvio" Text="Correo"
                                                OnCheckedChanged="rb_CheckedChanged" AutoPostBack="True" Enabled="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="6">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            FTP
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="6">
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lblUrlFtp" runat="server" Text="Url FTP:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbUrlFtp" CssClass="BordeControles" runat="server" ValidationGroup="10"
                                                Width="360px" Enabled="False"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvUrlFtp" runat="server" ErrorMessage="Favor digitar la url del FTP a buscar!"
                                                ForeColor="Red" ControlToValidate="txbUrlFtp" ValidationGroup="10" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceUrlFtp" runat="server" Enabled="True" TargetControlID="rfvUrlFtp"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="BottomLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lblUsuarioFtp" runat="server" Text="Usuario FTP:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbUsuarioFtp" CssClass="BordeControles" runat="server" ValidationGroup="10"
                                                Width="360px" Enabled="False"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvUsuarioFtp" runat="server" ErrorMessage="Favor digitar el Usuario para conectarse al FTP!"
                                                ForeColor="Red" ControlToValidate="txbUsuarioFTP" ValidationGroup="10" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceUsuarioFtp" runat="server" Enabled="True" TargetControlID="rfvUsuarioFtp"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="BottomLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lblClaveFtp" runat="server" Text="Clave FTP:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbClaveFtp" CssClass="BordeControles" runat="server" ValidationGroup="10"
                                                Width="360px" Enabled="False" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvClaveFtp" runat="server" ErrorMessage="Favor digitar la contraseña para conectarse al FTP!"
                                                ForeColor="Red" ControlToValidate="txbClaveFtp" ValidationGroup="10" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceClaveFtp" runat="server" Enabled="True" TargetControlID="rfvClaveFtp"
                                                PopupPosition="BottomLeft" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="6">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Correos
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="6">
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="LblRemitente" runat="server" Text="Correo Remitente:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbRemitente" CssClass="BordeControles" runat="server" Width="360px"
                                                MaxLength="200" ValidationGroup="1" Enabled="False"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revRemitente" runat="server" ValidationGroup="1"
                                                ControlToValidate="txbRemitente" ErrorMessage="Formato de Correo Incorrecto!!!"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red">*</asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvRemitente" runat="server" ErrorMessage="Favor digitar el Remitente!"
                                                ForeColor="Red" ControlToValidate="txbRemitente" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceRemitente" runat="server" Enabled="True" TargetControlID="revRemitente"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                TargetControlID="rfvRemitente" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbCorreoControl" runat="server" Text="Correos Control:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbCorreoControl" CssClass="BordeControles" Enabled="False" runat="server"
                                                Width="360px" MaxLength="200" ValidationGroup="2"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revCorreoControl" runat="server" ValidationGroup="2"
                                                ControlToValidate="txbCorreoControl" ErrorMessage="Formato de Correo Incorrecto!!!"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCorreoControl" runat="server" Enabled="True"
                                                TargetControlID="revCorreoControl" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td style="text-align: right" class="style5">
                                            <asp:ImageButton ID="imgAgregarCorreoControl" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                Width="16px" OnClick="imgAgregarCorreoControl_Click" ValidationGroup="2" />
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EspaciadoIntermedio" colspan="2">
                                        </td>
                                        <td>
                                            <asp:ListBox ID="LtbCorreoControl" runat="server" Width="360px" CssClass="Bordes"
                                                Height="64px" Enabled="False"></asp:ListBox>
                                        </td>
                                        <td style="text-align: right" valign="top">
                                            <asp:ImageButton ID="imgBorrarCorreoControl" runat="server" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                                Width="16px" OnClick="imgBorrarCorreoControl_Click" />
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="LbCorreoEnvio" runat="server" Text="Correos Envio Información:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbCorreoEnvio" CssClass="BordeControles" Enabled="False" runat="server"
                                                Width="360px" MaxLength="200" ValidationGroup="3"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revCorreoEnvio" runat="server" ValidationGroup="3"
                                                ControlToValidate="txbCorreoEnvio" ErrorMessage="Formato de Correo Incorrecto!!!"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red">*</asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCorreoEnvio" runat="server" Enabled="True" TargetControlID="revCorreoEnvio"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:ImageButton ID="imgAgregarCorreoEnvio" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                Width="16px" OnClick="imgAgregarCorreoEnvio_Click" ValidationGroup="3" />
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EspaciadoIntermedio" colspan="2">
                                        </td>
                                        <td>
                                            <asp:ListBox ID="LtbCorreoEnvio" runat="server" Width="360px" CssClass="Bordes" Height="64px"
                                                Enabled="False"></asp:ListBox>
                                        </td>
                                        <td style="text-align: right" valign="top">
                                            <asp:ImageButton ID="imgBorrarCorreoEnvio" runat="server" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                                Width="16px" OnClick="imgBorrarCorreoEnvio_Click" />
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="6">
                                        </td>
                                    </tr>
                                    <tr>                                        
                                        <td colspan="6" style="text-align: center">
                                            <asp:Button ID="btnEnviar" runat="server" Text="Enviar Archivo" Width="116px"
                                                OnClick="btnEnviar_Click" Height="31px" Enabled="False" ValidationGroup="1" UseSubmitBehavior="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="6">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="RoundedCornersExtender4" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlFtp" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                AssociatedUpdatePanelID="upContenido">
                <ProgressTemplate>
                <div class="contenedor">
                            <div class="centrado">
                                <div class="contenido" style="width: 100px; height: 20px">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/MarcaVisual/iconos/loading.gif"
                                     Height="20px" Width="100px" ImageAlign="Middle" />
                                </div>
                            </div>
                 </div>                   
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
