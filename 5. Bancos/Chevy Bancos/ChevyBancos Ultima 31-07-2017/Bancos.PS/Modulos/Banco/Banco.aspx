<%@ Page Title="Bancos" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master"
    AutoEventWireup="true" CodeBehind="Banco.aspx.cs" Inherits="Bancos.PS.Modulos.Banco.Banco" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
<link href="../../MarcaVisual/estilos/estilos.css" rel="stylesheet" type="text/css" />
     <table align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnNuevo" runat="server" ImageUrl="~/MarcaVisual/iconos/nuevo.png" Width="16px"
                    ToolTip="Nuevo..." OnClick="imgBtnNuevo_Click" />
            </td>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnGuardar" runat="server" 
                    ImageUrl="~/MarcaVisual/iconos/guardar.png" Width="16px"
                    ToolTip="Guardar" OnClick="imgBtnGuardar_Click" ValidationGroup="1" />
            </td>
          <%--  <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/MarcaVisual/iconos/borrar.png" Width="16px"
                    ToolTip="Eliminar" OnClick="imgBtnEliminar_Click" />
                <asp:ConfirmButtonExtender ID="cbeEliminar" runat="server" TargetControlID="imgBtnEliminar"
                    ConfirmText="Esta seguro de eliminar el registro?">
                </asp:ConfirmButtonExtender>
            </td>--%>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">
                        Ingresar Banco
                    </td>
                </tr>
                <tr>
                    <td class="SeparadorSubTitulo">
                    </td>
                </tr>
            </table>
            <table style="width: 100%" class="ColorContenedorDatos" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height: 10px" colspan="3">
                    </td>
                </tr>
                <tr>
                    <td style="width: 10px">
                    </td>
                    <td>
                        <asp:Panel ID="pnlBanco" runat="server" ScrollBars="Auto" Width="100%" Height="620px">
                            
                            <asp:Panel ID="PanelTP" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                     
                                     <tr>
                                        <td style="height: 10px" colspan="7">
                                        </td>
                                    </tr>
                                     <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbTipoProceso" runat="server" Text="Tipo de Proceso:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:DropDownList ID="ddlTipoProceso" runat="server" AutoPostBack="true" 
                                                CssClass="BordeListas" ValidationGroup="1" 
                                                onselectedindexchanged="ddlTipoProceso_SelectedIndexChanged" >
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTipoProceso" runat="server" 
                                                ControlToValidate="ddlTipoProceso" 
                                                ErrorMessage="Favor seleccionar un Proceso!" ForeColor="Red" 
                                                InitialValue="0" SetFocusOnError="true" Text="*" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoProceso" runat="server" Enabled="True" 
                                                HighlightCssClass="Resaltar" PopupPosition="Right" 
                                                TargetControlID="rfvTipoProceso" 
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal" colspan = "4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="7">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceTP" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="PanelTP" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlDatos" CssClass="PanelBordesRedondos" runat="server" 
                                Width="99%" Enabled="False">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbNombreCuenta" runat="server" Text="Cuentas Bancarias:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:DropDownList ID="ddlCuentas" runat="server" AutoPostBack="true" 
                                                CssClass="BordeListas" ValidationGroup="1" Height="16px" Width="170px" 
                                                onselectedindexchanged="ddlCuentas_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td style="width: 20%" class="EstiloEtiquetas">
                                            <asp:Label ID="lbCodigoCuenta1" runat="server" Text="Codigo de la Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbCodigoCuenta" runat="server" CssClass="BordeControles" MaxLength="50"
                                                Width="150px" ValidationGroup="1" Enabled="False"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCodigoCuenta" runat="server" ErrorMessage="Favor digitar el codigo de la cuenta!"
                                                ForeColor="Red" ControlToValidate="txbCodigoCuenta" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCodigoCuenta" runat="server" Enabled="True" TargetControlID="rfvCodigoCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="Left"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td class="EspaciadoInicial">
                                            </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lblNumCuenta" runat="server" Text="Número de Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                            </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNumCuenta" runat="server" CssClass="BordeControles" 
                                                MaxLength="17" Enabled="False"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revNumCuenta" runat="server" 
                                                ControlToValidate="txbNumCuenta" ErrorMessage="Formato Incorrecto!!!" 
                                                ForeColor="Red" ValidationExpression="^\d+$" ValidationGroup="1">*</asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvNumCuenta" runat="server" 
                                                ControlToValidate="txbNumCuenta" ErrorMessage="Favor digitar la Cuenta!" 
                                                ForeColor="Red" SetFocusOnError="true" Text="*" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                            </td>
                                        <td style="width: 20%" class="EstiloEtiquetas">
                                            <asp:Label ID="lblCodBanco" runat="server" Text="Codigo del Banco:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                            </td>
                                        <td class="EspaciadoCeldaControl">
                                             <asp:TextBox ID="txbCodBanco" runat="server" CssClass="BordeControles" 
                                                 MaxLength="50" ValidationGroup="1" Width="150px" Enabled="False"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="rfvCodBanco" runat="server" 
                                                 ControlToValidate="txbCodBanco" 
                                                 ErrorMessage="Favor digitar el Codigo del Banco!" ForeColor="Red" 
                                                 SetFocusOnError="true" Text="*" ValidationGroup="1"></asp:RequiredFieldValidator>
                                             <asp:ValidatorCalloutExtender ID="vceCodBanco" 
                                                 runat="server" Enabled="True" HighlightCssClass="Resaltar" PopupPosition="Left" 
                                                 TargetControlID="rfvCodBanco" 
                                                 WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png">
                                             </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                            </td>
                                    </tr>

                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="Label2" runat="server" Text="Tipo de Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">                                            
                                            <asp:DropDownList ID="ddlTipoCuenta" runat="server" 
                                                CssClass="BordeListas" ValidationGroup="1" Enabled="False">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvTipoCuenta" runat="server" 
                                                ControlToValidate="ddlTipoCuenta" 
                                                ErrorMessage="Favor seleccionar Tipo de Cuenta!" ForeColor="Red" 
                                                InitialValue="0" SetFocusOnError="true" Text="*" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoCuenta" runat="server" Enabled="True" 
                                                HighlightCssClass="Resaltar" PopupPosition="Right" 
                                                TargetControlID="rfvTipoCuenta" 
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png">
                                            </asp:ValidatorCalloutExtender>--%>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoFinal" colspan = "4">
                                        </td>
                                    </tr>
                                   
                                     <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbEstadoE" runat="server" Text="Se Encuentra Activo?:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                             <asp:CheckBox ID="chbEstaActivo" runat="server" 
                                                AutoPostBack="true" />                                            
                                        </td>                                        
                                        <td class="EspaciadoFinal" colspan="4">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEsquinasRedondas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDatos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlRutas" CssClass="PanelBordesRedondos" runat="server" 
                                Width="99%" Enabled="False">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Ruta
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbArchivoBanco" runat="server" Text="Ruta de Entrada:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbArchivoBanco" CssClass="BordeControles" runat="server" Width="400px"
                                                MaxLength="500" ValidationGroup="1">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvArchivoBanco" runat="server" ErrorMessage="Favor digitar la ruta por donde llegaran los archivos!"
                                                ForeColor="Red" ControlToValidate="txbArchivoBanco" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceArchivoBanco" runat="server" Enabled="True"
                                                TargetControlID="rfvArchivoBanco" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="BottomLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbArchivoAsobancaria" runat="server" Text="Ruta Procesados:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbArchivoAsobancaria" CssClass="BordeControles" runat="server"
                                                Width="400px" MaxLength="500" ValidationGroup="1">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvArchivoAsobancaria" runat="server" ErrorMessage="Favor digitar la ruta donde se colocaran los archivos procesados!"
                                                ForeColor="Red" ControlToValidate="txbArchivoAsobancaria" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceArchivoAsobancaria" runat="server" Enabled="True"
                                                TargetControlID="rfvArchivoAsobancaria" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="BottomLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbArchivoEpicor" runat="server" Text="Ruta Epicor:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbArchivoEpicor" CssClass="BordeControles" runat="server"
                                                Width="400px" MaxLength="500" ValidationGroup="1">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvArchivoEpicor" runat="server" ErrorMessage="Favor digitar la ruta donde se colocaran los archivos procesados!"
                                                ForeColor="Red" ControlToValidate="txbArchivoEpicor" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceArchivoEpicor" runat="server" Enabled="True"
                                                TargetControlID="rfvArchivoEpicor" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="BottomLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceRutas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlRutas" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlFTP" CssClass="PanelBordesRedondos" runat="server" 
                                Width="99%" Enabled="False">
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
                                            <asp:RadioButton ID="rbFTP" runat="server" Text="FTP" 
                                                GroupName="OpcionEnvio" 
                                                oncheckedchanged="rb_CheckedChanged"
                                                AutoPostBack="True" Checked="True" Font-Bold="True"/>
                                            <asp:RadioButton ID="rbCorreo" runat="server" 
                                                GroupName="OpcionEnvio" Text="Correo"
                                                oncheckedchanged="rb_CheckedChanged"
                                                AutoPostBack="True" Font-Bold="True"/>
                                        </td>                                      
                                        <td colspan="2">
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
                                        <td class="LetraLeyendaColor" colspan="2">
                                        </td>
                                </tr>                             
                                 <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbUrlFtp" runat="server" Text="Url FTP:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbUrlFTP" CssClass="BordeControles" runat="server"
                                                Width="350px" MaxLength="200" ValidationGroup="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvUrlFTP" runat="server" ErrorMessage="Favor digitar la url del FTP a buscar!"
                                                ForeColor="Red" ControlToValidate="txbUrlFTP" ValidationGroup="10" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceUrlFTP" runat="server" Enabled="True" TargetControlID="rfvUrlFTP"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="BottomLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td colspan="2">
                                            &nbsp;</td>
                                  </tr>
                                  <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbUsuarioFTP" runat="server" Text="Usuario:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbUsuarioFTP" CssClass="BordeControles" runat="server"
                                                Width="350px" MaxLength="150" ValidationGroup="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvUsuarioFTP" runat="server" ErrorMessage="Favor digitar el Usuario para conectarse al FTP!"
                                                ForeColor="Red" ControlToValidate="txbUsuarioFTP" ValidationGroup="10" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceUsuarioFTP" runat="server" Enabled="True" TargetControlID="rfvUsuarioFTP"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="BottomLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbClave" runat="server" Text="Clave:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbClave" CssClass="BordeControles" runat="server"
                                                Width="350px" MaxLength="100" TextMode="Password" ValidationGroup="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvClave" runat="server" ErrorMessage="Favor digitar la contraseña para conectarse al FTP!"
                                                ForeColor="Red" ControlToValidate="txbClave" ValidationGroup="10" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceClave" runat="server" Enabled="True" TargetControlID="rfvClave"
                                                PopupPosition="BottomLeft" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                              
                                  <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Correos
                                        </td>
                                        <td class="LetraLeyendaColor" colspan="2">
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
                                        <asp:TextBox ID="txbRemitente" CssClass="BordeControles" runat="server"
                                                Width="205px" MaxLength="200" ValidationGroup="1"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revRemitente" runat="server" ValidationGroup="1"
                                                ControlToValidate="txbRemitente" ErrorMessage="Formato de Correo Incorrecto!!!" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red">*</asp:RegularExpressionValidator>                                            
                                                <asp:RequiredFieldValidator ID="rfvRemitente" runat ="server" ErrorMessage="Favor digitar el Remitente!"
                                                ForeColor="Red" ControlToValidate="txbRemitente" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>   
                                                <asp:ValidatorCalloutExtender ID="vceRemitente" runat="server" Enabled="True" TargetControlID="revRemitente"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar" PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                              <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" TargetControlID="rfvRemitente"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar" PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                        </td>                                                                        
                                        <td colspan="2">
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
                                        <asp:TextBox ID="txbCorreoControl" CssClass="BordeControles" runat="server"
                                                Width="205px" MaxLength="200" ValidationGroup="2"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revCorreoControl" runat="server" 
                                                ControlToValidate="txbCorreoControl" ErrorMessage="Formato de Correo Incorrecto!!!" ValidationGroup="2" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red">*</asp:RegularExpressionValidator>                                            
                                                <asp:ValidatorCalloutExtender ID="vceCorreoControl" runat="server" Enabled="True" TargetControlID="revCorreoControl"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar" PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                        </td>  
                                        <td>
                                            </td>
                                        <td style="text-align: right">
                                            <asp:ImageButton ID="imgAgregarCorreoControl" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                 Width="16px" onclick="imgAgregarCorreoControl_Click" ValidationGroup="2"/>
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
                                            <asp:ListBox ID="LtbCorreoControl" runat="server" Width="277px" 
                                                CssClass="Bordes" Height="64px"></asp:ListBox>
                                        </td>
                                        <td>
                                            </td>
                                        <td style="text-align: right" valign="top">
                                            <asp:ImageButton  ID="imgBorrarCorreoControl" runat="server" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                                 Width="16px" onclick="imgBorrarCorreoControl_Click"/>
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
                                                Width="205px" MaxLength="200" ValidationGroup="3"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revCorreoEnvio" runat="server" ValidationGroup="3"
                                                ControlToValidate="txbCorreoEnvio" ErrorMessage="Formato de Correo Incorrecto!!!" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red">*</asp:RegularExpressionValidator>                                            
                                            <asp:ValidatorCalloutExtender ID="vceCorreoEnvio" runat="server" Enabled="True" TargetControlID="revCorreoEnvio"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar" PopupPosition="Right">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                         <td>
                                             </td>
                                         <td style="text-align: right">
                                            <asp:ImageButton ID="imgAgregarCorreoEnvio" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                 Width="16px" onclick="imgAgregarCorreoEnvio_Click" ValidationGroup="3"/>
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
                                            <asp:ListBox ID="LtbCorreoEnvio" runat="server" Width="277px" 
                                                CssClass="Bordes" Height="64px" Enabled="False"></asp:ListBox>
                                        </td>
                                        <td>
                                            </td>
                                        <td style="text-align: right" valign="top">
                                            <asp:ImageButton  ID="imgBorrarCorreoEnvio" runat="server" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                                 Width="16px" onclick="imgBorrarCorreoEnvio_Click"/>
                                        </td> 
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>                                    
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceFTP" Radius="3" BorderColor="181, 198, 214" TargetControlID="pnlFTP"
                                runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>

                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <%--<asp:Panel ID="pnlBusquedaBanco" CssClass="ContenedorDatos" runat="server" Width="600px"
                Height="400px" >
                <asp:UpdatePanel ID="upBusquedaBanco" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="BarraSubTitulo">
                                    B&uacute;squeda
                                </td>
                            </tr>
                            <tr>
                                <td class="SeparadorSubTitulo">
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td class="EspaciadoInicial">
                                </td>
                                <td>
                                    <asp:Panel ID="pnlDatosB" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                        <table style="width: 100%" cellpadding="0" cellspacing="2">
                                            <tr>
                                                <td class="EspaciadoInicial">
                                                </td>
                                                <td class="EstiloEtiquetas">
                                                    <asp:Label ID="lbCodigoBanco" runat="server" Text="Código Banco:"></asp:Label>
                                                </td>
                                                <td class="EspaciadoIntermedio">
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txbCodigoBancoB" runat="server" CssClass="BordeControles" MaxLength="3"
                                                        Width="30px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="EspaciadoInicial">
                                                </td>
                                                <td class="EstiloEtiquetas">
                                                    <asp:Label ID="lbNombreBancoB" runat="server" Text="Nombre del Banco:"></asp:Label>
                                                </td>
                                                <td class="EspaciadoIntermedio">
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txbNombreBancoB" runat="server" CssClass="BordeControles" MaxLength="100"
                                                        Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:RoundedCornersExtender ID="rceEsquinasRedondasB" Radius="3" BorderColor="181, 198, 214"
                                        TargetControlID="pnlDatosB" runat="server" Enabled="True">
                                    </asp:RoundedCornersExtender>
                                </td>
                                <td style="width: 130px; text-align: center; vertical-align: bottom">
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="110px" 
                                        OnClick="btnBuscar_Click" UseSubmitBehavior="True" />
                                    <asp:Button ID="btnNuevaBusqueda" runat="server" Text="Nueva Búsqueda" Width="110px"
                                        OnClick="btnNuevaBusqueda_Click" />
                                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Resultados" Width="110px"
                                        OnClick="btnLimpiar_Click" Enabled="false" />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="110px" 
                                        onclick="btnCancelar_Click" UseSubmitBehavior="false"/>
                                </td>
                                <td class="EspaciadoInicial">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" class="SeparadorHorizontal">
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%" cellspacing="2" cellpadding="0">
                            <tr>
                                <td colspan="2">
                                        <asp:GridView ID="gvBusquedaBanco" runat="server" AutoGenerateColumns="False" Width="100%"
                                        AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                        GridLines="Horizontal" OnRowCommand="gvBusquedaBanco_RowCommand" AllowPaging="True"
                                        OnPageIndexChanging="gvBusquedaBanco_PageIndexChanging" PageSize="9">
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" ImageUrl="~/MarcaVisual/iconos/aceptar.png" CommandName="sel"
                                                Text="Seleccionar">
                                                <ItemStyle Width="50px" />
                                                <ControlStyle Width="16px" />
                                            </asp:ButtonField>
                                            <asp:BoundField ItemStyle-Width="80px" HeaderText="Código Banco" DataField="pCodigoBanco">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Nombre Banco" DataField="pNombre" />
                                        </Columns>
                                        <HeaderStyle BackColor="#C5C5C6" />
                                        <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                        <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <div class="BarraEstado">
                            <table class="EstiloBarraEstado">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbEstadoBusquedaCliente" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="gvBusquedaBanco" />
                        <asp:PostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:ModalPopupExtender ID="mpeBusquedaBanco" runat="server" PopupControlID="pnlBusquedaBanco"
                TargetControlID="btnCodigoBanco" BackgroundCssClass="VentanaModal" CancelControlID="btnCancelar">
            </asp:ModalPopupExtender>--%>
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
