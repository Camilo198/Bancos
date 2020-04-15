<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true"
    CodeBehind="DeTablas.aspx.cs" Inherits="Bancos.PS.Modulos.Interpretaciones.DeTablas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
    <table align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnNuevo" runat="server" ImageUrl="~/MarcaVisual/iconos/nuevo.png"
                    Width="16px" ToolTip="Nuevo..." OnClick="imgBtnNuevo_Click" />
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
                        Definir la Reinterpretación de los valores de una tabla entre un banco y asobancaria
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
                        <asp:Panel ID="pnlPrincipal" runat="server" ScrollBars="Auto" Width="100%" Height="620px">
                            <table style="width: 100%" cellpadding="0" cellspacing="2">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlBanco" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                            ScrollBars="None">
                                            <table style="width: 100%" cellpadding="0" cellspacing="2">
                                                <tr>
                                                    <td class="LetraLeyendaColor" colspan="9">
                                                        Banco
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="EspaciadoInicial">
                                                    </td>
                                                    <td class="EstiloEtiquetas">
                                                        <asp:Button ID="btnCodigoBanco" runat="server" Text="Código Banco:" />
                                                    </td>
                                                    <td class="EspaciadoIntermedio">
                                                    </td>
                                                    <td class="EspaciadoCeldaControl">
                                                        <asp:TextBox ID="txbCodigoBanco" runat="server" CssClass="BordeControles" MaxLength="3"
                                                            Width="30px" ValidationGroup="1" AutoPostBack="true" OnTextChanged="txbCodigoBanco_TextChanged"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvCodigoBanco" runat="server" ErrorMessage="Favor digitar el Codigo del Banco!"
                                                            ForeColor="Red" ControlToValidate="txbCodigoBanco" ValidationGroup="1" Text="*"
                                                            SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceCodigoBanco" runat="server" Enabled="True" TargetControlID="rfvCodigoBanco"
                                                            WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:Label ID="lbNombreBanco" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="EspaciadoInicial">
                                                    </td>
                                                    <td class="EstiloEtiquetas">
                                                        <asp:Label ID="lbTabla" runat="server" Text="Tabla:"></asp:Label>
                                                    </td>
                                                    <td class="EspaciadoIntermedio">
                                                    </td>
                                                    <td class="EspaciadoCeldaControl">
                                                        <asp:DropDownList ID="ddlTablaBanco" CssClass="BordeListas" runat="server" ValidationGroup="1">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvTablaBanco" InitialValue="0" runat="server" ErrorMessage="Favor seleccione la tabla que quiere interpretar!"
                                                            ForeColor="Red" ControlToValidate="ddlTablaBanco" ValidationGroup="1" Text="*"
                                                            SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceTablaBanco" runat="server" Enabled="True" TargetControlID="rfvTablaBanco"
                                                            WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                                        </asp:ValidatorCalloutExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:RoundedCornersExtender ID="rceEsquinasRedondas" Radius="3" BorderColor="181, 198, 214"
                                            TargetControlID="pnlBanco" runat="server" Enabled="True">
                                        </asp:RoundedCornersExtender>
                                    </td>
                                    <td class="EspaciadoIntermedio">
                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlAsobancaria" CssClass="PanelBordesRedondos" runat="server" Width="98%"
                                            ScrollBars="None">
                                            <table style="width: 100%" cellpadding="0" cellspacing="2">
                                                <tr>
                                                    <td class="LetraLeyendaColor" colspan="9">
                                                        Asobancaria
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="EspaciadoInicial">
                                                    </td>
                                                    <td class="EstiloEtiquetas">
                                                        <asp:Label ID="Label2" runat="server" Text="Tabla:"></asp:Label>
                                                    </td>
                                                    <td class="EspaciadoIntermedio">
                                                    </td>
                                                    <td class="EspaciadoCeldaControl">
                                                        <asp:DropDownList ID="ddlTablaASO" CssClass="BordeListas" runat="server" ValidationGroup="1">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvTablaASO" InitialValue="0" runat="server" ErrorMessage="Favor seleccione la tabla que quiere interpretar!"
                                                            ForeColor="Red" ControlToValidate="ddlTablaASO" ValidationGroup="1" Text="*"
                                                            SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceTablaASO" runat="server" Enabled="True" TargetControlID="rfvTablaASO"
                                                            WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                            PopupPosition="Left">
                                                        </asp:ValidatorCalloutExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center">
                                                        <asp:ImageButton ID="imgBtnFiltrar" runat="server" ToolTip="Filtar campos." ImageUrl="~/MarcaVisual/iconos/buscar.png"
                                                            OnClick="imgBtnFiltrar_Click" Height="16px" Width="16px" ValidationGroup="1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:RoundedCornersExtender ID="rceAsobancaria" Radius="3" BorderColor="181, 198, 214"
                                            TargetControlID="pnlAsobancaria" runat="server" Enabled="True">
                                        </asp:RoundedCornersExtender>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="pnlEstructura" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                Height="85px">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Interpretar Respuestas de Transacciones del Banco a Asobancaria
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width: 50%">
                                            <asp:Label ID="lbRepuestaBanco" runat="server" Text="Respuesta del Banco"></asp:Label>
                                        </th>
                                        <th class="EspaciadoIntermedio">
                                            =>
                                        </th>
                                        <th style="width: 50%">
                                            <asp:Label ID="lbRespuestaASO" runat="server" Text="Respuesta de Asobancaria"></asp:Label>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center">
                                            <asp:DropDownList ID="ddlRespuestaBanco" CssClass="BordeListas200" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:DropDownList ID="ddlRespuestaAsobancaria" CssClass="BordeListas200" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: center">
                                            <asp:ImageButton ID="imgBtnAgregar" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                OnClick="imgBtnAgregar_Click" ToolTip="Agregar reinterpretación del campo" Width="16px" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEstructura" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlEstructura" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:GridView ID="gvValores" runat="server" AutoGenerateColumns="False" Width="99%"
                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                GridLines="Horizontal" OnRowCommand="gvValores_RowCommand" AllowPaging="True"
                                OnPageIndexChanging="gvValores_PageIndexChanging" PageSize="2">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Eliminar" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                        Text="Eliminar">
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="16px" />
                                    </asp:ButtonField>
                                    <asp:BoundField HeaderText="pValorBanco" DataField="pValorBanco" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="pValorAsobancaria" DataField="pValorAsobancaria" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Código Valor Banco" DataField="pCodigoBanco" />
                                    <asp:BoundField HeaderText="Descripción Valor Banco" DataField="pDescripcionBanco" />
                                    <asp:BoundField HeaderText="Código Valor Asobancaria" DataField="pCodigoAsobancaria" />
                                    <asp:BoundField HeaderText="Descripción Valor Asobancaria" DataField="pDescripcionAsobancaria" />
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
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="110px" OnClick="btnBuscar_Click" />
                                    <asp:Button ID="btnNuevaBusqueda" runat="server" Text="Nueva Búsqueda" Width="110px"
                                        OnClick="btnNuevaBusqueda_Click" />
                                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Resultados" Width="110px"
                                        OnClick="btnLimpiar_Click" Enabled="false" />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Width="110px" />
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
                                                <HeaderStyle Width="30px" />
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
            </asp:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
