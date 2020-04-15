<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="EstablecerEquivalencias.aspx.cs" Inherits="Bancos.PS.Modulos.Banco.EstablecerEquivalencias" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
<table align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnNuevo" runat="server" 
                    ImageUrl="~/MarcaVisual/iconos/nuevo.png" Width="16px"
                    ToolTip="Nuevo..." onclick="imgBtnNuevo_Click"   />
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
                        Equivalencias Centrales de Riesgos
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
                        <asp:Panel ID="pnlCentraldeRiesgos" runat="server" ScrollBars="Auto" Width="100%"
                            Height="620px">
                            <asp:Panel ID="pnlTipoArchivo" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                ScrollBars="None">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">                                    
                                    <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas100">
                                            <asp:Label ID="lblTipoArchivo" runat="server" Text="Tipo de Archivo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoArchivo" CssClass="BordeListas" runat="server" 
                                                              ValidationGroup="1" Width="180px" AutoPostBack="True" 
                                                onselectedindexchanged="ddlTipoArchivo_SelectedIndexChanged">                                                
                                            </asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="rfvTipoArchivo" runat="server" ErrorMessage="Favor seleccionar Tipo de Archivo!"
                                                ForeColor="Red" ControlToValidate="ddlTipoArchivo" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoArchivo" runat="server" Enabled="True" TargetControlID="rfvTipoArchivo"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal" colspan="5">
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
                                                Width="40px" ValidationGroup="1" AutoPostBack="true" 
                                                ontextchanged="txbCodigoCuenta_TextChanged"  ></asp:TextBox>
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
                                                ReadOnly="true" Width="140px" Enabled="False" ValidationGroup="1"></asp:TextBox>
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
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas100">
                                            <asp:Label ID="lbTipoLinea" runat="server" Text="Tipo de Linea:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                             <asp:DropDownList ID="ddlTipoLinea" CssClass="BordeListas" runat="server" 
                                                              ValidationGroup="1" Width="170px" AutoPostBack="True" 
                                                 Enabled="False" onselectedindexchanged="ddlTipoLinea_SelectedIndexChanged"> 
                                             </asp:DropDownList> 
                                             <asp:RequiredFieldValidator ID="rfvTipoLinea" runat="server" ErrorMessage="Favor seleccionar el Tipo de Linea!"
                                                ForeColor="Red" ControlToValidate="ddlTipoLinea" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoLinea" runat="server" Enabled="True" TargetControlID="rfvTipoLinea"
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
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="RoundedCornersExtender1" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlTipoArchivo" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlEquivalencias" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                ScrollBars="None">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">
                                            Equivalencias
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>                                   
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas100">
                                            <asp:Label ID="lbCamposLineas" runat="server" Text="Campos de las Lineas:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                             <asp:DropDownList ID="ddlCamposLineas" CssClass="BordeListas" runat="server" 
                                                              ValidationGroup="1" Width="170px" AutoPostBack="True" 
                                                 Enabled="False"> 
                                             </asp:DropDownList> 
                                             <asp:RequiredFieldValidator ID="rfvCamposLineas" runat="server" ErrorMessage="Favor seleccionar el Campo!"
                                                ForeColor="Red" ControlToValidate="ddlCamposLineas" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCamposLineas" runat="server" Enabled="True" TargetControlID="rfvCamposLineas"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr> 
                                     <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas100">
                                            <asp:Label ID="lbTablas" runat="server" Text="Tablas:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                              <asp:DropDownList ID="ddlTablas" CssClass="BordeListas" runat="server" 
                                                              ValidationGroup="1" Width="170px" AutoPostBack="True" 
                                                  Enabled="False" onselectedindexchanged="ddlTablas_SelectedIndexChanged"> 
                                             </asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="rfvTablas" runat="server" ErrorMessage="Favor seleccionar la Tabla!"
                                                ForeColor="Red" ControlToValidate="ddlTablas" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTablas" runat="server" Enabled="True" TargetControlID="rfvTablas"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>                                        
                                         <td class="EspaciadoFinal" colspan="5">
                                        </td>                                     
                                    </tr>  
                                    <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas100">
                                            <asp:Label ID="lbTablasLineas" runat="server" Text="Campos de las Tablas:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                              <asp:DropDownList ID="ddlCamposTablas" CssClass="BordeListas" runat="server" 
                                                              ValidationGroup="2" Width="170px" AutoPostBack="True" 
                                                  Enabled="False"> 
                                             </asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="rfvCamposTablas" runat="server" ErrorMessage="Favor seleccionar la tabla!"
                                                ForeColor="Red" ControlToValidate="ddlCamposTablas" InitialValue="0" ValidationGroup="2"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCamposTablas" runat="server" Enabled="True" TargetControlID="rfvCamposTablas"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>                                        
                                        <td>                                                                                        
                                            <asp:ImageButton ID="imgAgregarEquivalencia" runat="server" 
                                                ImageUrl="~/MarcaVisual/iconos/agregar.png"  
                                                ValidationGroup="1" Width="16px" onclick="imgAgregarEquivalencia_Click" />                                                                              
                                        </td>
                                         <td class="EspaciadoFinal" colspan="5">
                                        </td> 
                                    </tr>                              
                                    <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>         
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="RoundedCornersExtender2" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlEquivalencias" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                               <asp:GridView ID="gvEquivalencias" runat="server" 
                                AutoGenerateColumns="False" Width="99%"
                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                GridLines="Horizontal" PageSize="5" 
                                AllowPaging="True" onrowcommand="gvEquivalencias_RowCommand" 
                                onpageindexchanging="gvEquivalencias_PageIndexChanging">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Editar" ImageUrl="~/MarcaVisual/iconos/editar.png"
                                        Text="Editar">
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="16px" />
                                    </asp:ButtonField>
                                    <asp:ButtonField ButtonType="Image" CommandName="Eliminar" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                        Text="Eliminar">
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="16px" />
                                    </asp:ButtonField>
                                    <asp:BoundField HeaderText="ID" DataField="ID"
                                     ItemStyle-CssClass="OcultarControles"
                                    HeaderStyle-CssClass="OcultarControles">
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="16px" />
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="IDCampoLinea" DataField="IDEA"
                                     ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="16px" />
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Campos de las Lineas" DataField="NOMBREEA">                                    
                                        <ItemStyle Width="150px" />
                                        <ControlStyle Width="16px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="IDTabla" DataField="IDTE" 
                                    ItemStyle-CssClass="OcultarControles"
                                    HeaderStyle-CssClass="OcultarControles">
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="16px" />
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Tabla" DataField="NOMBRETE">
                                        <ItemStyle Width="150px" />
                                        <ControlStyle Width="16px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="IDCampoTabla" DataField="IDCE" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles" >
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="16px" />
                                        <HeaderStyle CssClass="OcultarControles" /> 
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Campos de las Tablas" DataField="DESCRIPCIONCE">
                                        <ItemStyle Width="150px" />
                                        <ControlStyle Width="16px" />                                                                        
                                    </asp:BoundField>                                   
                                </Columns>
                                <HeaderStyle BackColor="#C5C5C6" />
                                <RowStyle HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1pt" />
                                <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
             <asp:Panel ID="pnlBusquedaBanco" CssClass="ContenedorDatos" runat="server" Width="500px"
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
