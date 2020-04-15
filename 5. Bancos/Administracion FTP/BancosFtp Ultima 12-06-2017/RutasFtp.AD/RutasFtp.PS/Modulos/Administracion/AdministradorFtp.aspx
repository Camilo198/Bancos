<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="AdministradorFtp.aspx.cs" Inherits="RutasFtp.PS.Modulos.Administracion.AdministradorFtp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
<table align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnNuevo" runat="server" 
                    ImageUrl="~/MarcaVisual/iconos/nuevo.png" Width="16px"
                    ToolTip="Nuevo..." onclick="imgBtnNuevo_Click"/>
            </td>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnGuardar" runat="server" 
                    ImageUrl="~/MarcaVisual/iconos/guardar.png" Width="16px"
                    ToolTip="Guardar" ValidationGroup="1" onclick="imgBtnGuardar_Click"/>
            </td>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/MarcaVisual/iconos/borrar.png" Width="16px"
                    ToolTip="Eliminar" OnClick="imgBtnEliminar_Click" />
                <asp:ConfirmButtonExtender ID="cbeEliminar" runat="server" TargetControlID="imgBtnEliminar"
                    ConfirmText="Esta seguro de eliminar el registro?">
                </asp:ConfirmButtonExtender>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
<asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">
                        Administración Ftp
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
                        <asp:Panel ID="pnlAdmonFtp" runat="server" ScrollBars="Auto" Width="100%" 
                            Height="647px">    
                            <br />
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
                                                InitialValue="0" SetFocusOnError="true" Text="*" ValidationGroup="1">
                                            </asp:RequiredFieldValidator>
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
                            <asp:Panel ID="pnlFTP" CssClass="PanelBordesRedondos" runat="server" 
                                Width="99%" Enabled="False">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">

                                <tr>
                                        <td style="height: 10px" colspan="6">
                                        </td>
                                </tr>                              
                                <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            FTP
                                        </td>
                                        <td class="LetraLeyendaColor">
                                        </td>
                                 </tr> 
                                 <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                 </tr>
                                 <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Button ID="btnCodigoCuenta" runat="server" Text="Código de la Cuenta:" />
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbCodigoCuenta" runat="server" CssClass="BordeControles" MaxLength="5"
                                                Width="40px" ValidationGroup="1" AutoPostBack="true" OnTextChanged="txbCodigoCuenta_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCodigoCuenta" runat="server" ErrorMessage="Favor digitar el Codigo de la Cuenta!"
                                                ForeColor="Red" ControlToValidate="txbCodigoCuenta" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCodigoCuenta" runat="server" Enabled="True" TargetControlID="rfvCodigoCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td style="width: 20%" class="EstiloEtiquetas">
                                            <asp:Label ID="lbNombreCuenta" runat="server" Text="Nombre de la Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNombreCuenta" runat="server" CssClass="BordeControles" MaxLength="100"
                                                ValidationGroup="1" ReadOnly="true" Width="150px" Enabled="False"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvNombreCuenta" runat="server" ErrorMessage="Favor digitar el Nombre de la Cuenta!"
                                                ForeColor="Red" ControlToValidate="txbNombreCuenta" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNombreCuenta" runat="server" Enabled="True" TargetControlID="rfvNombreCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>

                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9">
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
                                        <td colspan="5">
                                            <asp:TextBox ID="txbUrlFTP" CssClass="BordeControles" runat="server"
                                                Width="400px" MaxLength="200" ValidationGroup="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvUrlFTP" runat="server" ErrorMessage="Favor digitar la url del FTP a buscar!"
                                                ForeColor="Red" ControlToValidate="txbUrlFTP" ValidationGroup="1" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceUrlFTP" runat="server" Enabled="True" TargetControlID="rfvUrlFTP"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="BottomLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
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
                                        <td colspan="5">
                                            <asp:TextBox ID="txbUsuarioFTP" CssClass="BordeControles" runat="server"
                                                Width="400px" MaxLength="150" ValidationGroup="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvUsuarioFTP" runat="server" ErrorMessage="Favor digitar el Usuario para conectarse al FTP!"
                                                ForeColor="Red" ControlToValidate="txbUsuarioFTP" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceUsuarioFTP" runat="server" Enabled="True" TargetControlID="rfvUsuarioFTP"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="BottomLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
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
                                        <td colspan="5">
                                            <asp:TextBox ID="txbClave" CssClass="BordeControles" runat="server"
                                                Width="400px" MaxLength="100" TextMode="Password" ValidationGroup="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvClave" runat="server" ErrorMessage="Favor digitar la contraseña para conectarse al FTP!"
                                                ForeColor="Red" ControlToValidate="txbClave" ValidationGroup="1" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceClave" runat="server" Enabled="True" TargetControlID="rfvClave"
                                                PopupPosition="BottomLeft" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="Label1" runat="server" Text="FTP Seguro?:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td colspan="5">
                                            <asp:CheckBox ID="chbFTPSeguro" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="Label2" runat="server" Text="Prefijo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txbPrefijo" CssClass="BordeControles" runat="server"
                                                Width="100px" MaxLength="50" ></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="Label3" runat="server" Text="Formato:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txbFormato" CssClass="BordeControles" runat="server"
                                                Width="100px" MaxLength="5" ></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>                                     
                                    <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>  
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbRutaArchivo" runat="server" Text="Ruta destino:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txbRutaSalida" CssClass="BordeControles" runat="server"
                                                Width="400px" MaxLength="500" ValidationGroup="1">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvRutaSalida" runat="server" ErrorMessage="Favor digitar la ruta donde se colocaran los archivos procesados!"
                                                ForeColor="Red" ControlToValidate="txbRutaSalida" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceRutaSalida" runat="server" Enabled="True"
                                                TargetControlID="rfvRutaSalida" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="BottomLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                         <td>
                                        </td>
                                    </tr>     
                                     <tr>
                                        <td style="height: 10px" colspan="9">
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
              <asp:Panel ID="pnlBusquedaBanco" CssClass="ContenedorDatos" runat="server" Width="500px"
                Height="400px">
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
                                                    <asp:Label ID="lbCodigoCuentaB" runat="server" Text="Código de la Cuenta:"></asp:Label>
                                                </td>
                                                <td class="EspaciadoIntermedio">
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txbCodigoCuentaB" runat="server" CssClass="BordeControles" MaxLength="5"
                                                        Width="40px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="EspaciadoInicial">
                                                </td>
                                                <td class="EstiloEtiquetas">
                                                    <asp:Label ID="lbNombreCuentaB" runat="server" Text="Nombre de la Cuenta:"></asp:Label>
                                                </td>
                                                <td class="EspaciadoIntermedio">
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txbNombreCuentaB" runat="server" CssClass="BordeControles" MaxLength="100"
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
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="110px" OnClick="btnBuscar_Click" />
                                    <asp:Button ID="btnNuevaBusqueda" runat="server" Text="Nueva Búsqueda" Width="110px"
                                        OnClick="btnNuevaBusqueda_Click" />
                                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Resultados" Width="110px"
                                        OnClick="btnLimpiar_Click" Enabled="false" />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="110px" 
                                        onclick="btnCancelar_Click" />
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
                                            <asp:BoundField ItemStyle-Width="80px" HeaderText="Código de la Cuenta" DataField="pIdCuentaBanco">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Nombre de la Cuenta" DataField="pNombreCuenta" />
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
                TargetControlID="btnCodigoCuenta" BackgroundCssClass="VentanaModal" CancelControlID="btnCancelar">
            </asp:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
<asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
