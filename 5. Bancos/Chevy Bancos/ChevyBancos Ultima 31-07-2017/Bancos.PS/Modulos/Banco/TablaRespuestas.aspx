<%@ Page Title="Bancos - Tablas de Respuestas" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master"
    AutoEventWireup="true" CodeBehind="TablaRespuestas.aspx.cs" Inherits="Bancos.PS.Modulos.Banco.TablaRespuestas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <script language="javascript" type="text/javascript">
        function validarCaracteres(textareaControl, maxlength, idSpan) {
            if (textareaControl.value.length > maxlength) {
                textareaControl.value = textareaControl.value.substring(0, maxlength);
            }
            document.getElementById(idSpan).innerHTML = textareaControl.value.length + ' / <b>' + maxlength + '</b>';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
    <table align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnNuevo" runat="server" ImageUrl="~/MarcaVisual/iconos/nuevo.png" Width="16px"
                    ToolTip="Nuevo..." OnClick="imgBtnNuevo_Click" />
            </td>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnGuardar" runat="server" ImageUrl="~/MarcaVisual/iconos/guardar.png"
                    ToolTip="Guardar" ValidationGroup="1" OnClick="imgBtnGuardar_Click" />
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
                        Definir Respuestas de Transacciones
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
                            <asp:Panel ID="pnlDatos" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                ScrollBars="None">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
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
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbNombreTabla" runat="server" Text="Nombre de la Tabla:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNombreTabla" runat="server" CssClass="BordeControles" MaxLength="100"
                                                ValidationGroup="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvNombreTabla" runat="server" ErrorMessage="Favor digitar el nombre de la tabla!"
                                                ForeColor="Red" ControlToValidate="txbNombreTabla" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNombreTabla" runat="server" Enabled="True" TargetControlID="rfvNombreTabla"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbEsAsobancaria" runat="server" Text="¿Es tabla para asobancaria?:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:CheckBox ID="chbEsAsobancaria" AutoPostBack="true" runat="server" OnCheckedChanged="chbEsAsobancaria_CheckedChanged" />
                                        </td>
                                        <td colspan="4" style="text-align: right">
                                            <asp:ImageButton ID="imgBtnFiltrar" runat="server" ImageUrl="~/MarcaVisual/iconos/buscar.png" Width="16px"
                                                OnClick="imgBtnFiltrar_Click" ToolTip="Buscar" />
                                            <asp:ImageButton ID="imgBtnAddTabla" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png" Width="16px"
                                                ValidationGroup="1" OnClick="imgBtnAddTabla_Click" ToolTip="Agregar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEsquinasRedondas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDatos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:GridView ID="gvTablas" runat="server" AutoGenerateColumns="False" Width="99%"
                                AllowPaging="true" AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid"
                                BorderWidth="1px" GridLines="Horizontal" OnRowCommand="gvTablas_RowCommand" PageSize="3"
                                OnPageIndexChanging="gvTablas_PageIndexChanging">
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
                                    <asp:BoundField HeaderText="OID" DataField="pOid" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Código Banco" DataField="pBanco">
                                        <HeaderStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Tabla" DataField="pNombre"></asp:BoundField>
                                    <asp:CheckBoxField DataField="pEsAsobancaria" HeaderText="Es Asobancaria">
                                        <HeaderStyle Width="60px" />
                                    </asp:CheckBoxField>
                                </Columns>
                                <HeaderStyle BackColor="#C5C5C6" />
                                <RowStyle HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1pt" />
                                <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" />
                            </asp:GridView>
                            <br />
                            <asp:Panel ID="pnlEstructura" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="8">
                                            Campos de la estructura de la l&iacute;nea
                                            <asp:Label ID="lbTLEditando" runat="server" Text="NO HAY TABLA SELECCIONADA"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbCodigo" runat="server" Text="Código Causal:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbCodigo" CssClass="BordeControles" ValidationGroup="3" MaxLength="5"
                                                runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" ErrorMessage="Favor digitar el código de la causal!"
                                                ForeColor="Red" ControlToValidate="txbCodigo" ValidationGroup="3" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCodigo" runat="server" Enabled="True" TargetControlID="rfvCodigo"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbPrenotificacion" runat="server" Text="Prenotificación:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPrenotificacion" CssClass="BordeListas" runat="server">
                                                <asp:ListItem Text="[Seleccione]" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Si" Value="SI"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="NO"></asp:ListItem>
                                                <asp:ListItem Text="No Aplica" Value="N/A"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvPrenotificacion" runat="server" ErrorMessage="Favor seleccionar si requiere prenotificación!"
                                                ForeColor="Red" ControlToValidate="ddlPrenotificacion" InitialValue="0" ValidationGroup="3"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vcePrenotificacion" runat="server" Enabled="True"
                                                TargetControlID="rfvPrenotificacion" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbTransDebito" runat="server" Text="Transacción Débito:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTransDebito" CssClass="BordeListas" runat="server">
                                                <asp:ListItem Text="[Seleccione]" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Si" Value="SI" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="NO"></asp:ListItem>
                                                <asp:ListItem Text="No Aplica" Value="N/A"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTransDebito" runat="server" ErrorMessage="Favor seleccionar si es de transacción débito!"
                                                ForeColor="Red" ControlToValidate="ddlTransDebito" InitialValue="0" ValidationGroup="3"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTransDebito" runat="server" Enabled="True" TargetControlID="rfvTransDebito"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar" PopupPosition="Left">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbDesc" runat="server" Text="Descripción Estándar:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txbDesc" CssClass="Bordes" Width="420px" TextMode="MultiLine" ValidationGroup="3"
                                                runat="server" Height="50px"></asp:TextBox>
                                            <span id="spnDesc"></span>
                                            <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ErrorMessage="Favor digitar la descripción estándar!"
                                                ForeColor="Red" ControlToValidate="txbDesc" ValidationGroup="3" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceDesc" runat="server" Enabled="True" TargetControlID="rfvDesc"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="Left">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbDetalleAdicional" runat="server" Text="Detalle Adicional:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txbDetalleAdicional" CssClass="Bordes" Width="420px" runat="server"
                                                TextMode="MultiLine" Height="50px"></asp:TextBox>
                                            <span id="spnDetalleAdicional"></span>
                                            <%--
                                            <asp:RequiredFieldValidator ID="rfvDetalleAdicional" runat="server" ErrorMessage="Favor digitar el detalle adicional!"
                                                ForeColor="Red" ControlToValidate="txbDetalleAdicional" ValidationGroup="3" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceDetalleAdicional" runat="server" Enabled="True" TargetControlID="rfvDetalleAdicional"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar" PopupPosition="Left">
                                            </asp:ValidatorCalloutExtender>
                                            --%>
                                        </td>
                                        <td>
                                        <td colspan="8" style="text-align: right">
                                            <asp:ImageButton ID="imgBtnAddRespuesta" ToolTip="Almacenar Respuesta" runat="server" Width="16px"
                                                ValidationGroup="3" ImageUrl="~/MarcaVisual/iconos/agregar.png" OnClick="imgBtnAddRespuesta_Click" />
                                        </td>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEstructura" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlEstructura" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:GridView ID="gvRespuestas" runat="server" AutoGenerateColumns="False" Width="99%"
                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                GridLines="Horizontal" OnRowCommand="gvRespuestas_RowCommand" AllowPaging="True"
                                OnPageIndexChanging="gvRespuestas_PageIndexChanging" PageSize="2">
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
                                    <asp:BoundField HeaderText="OID" DataField="pOid" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Tabla" DataField="pTabla" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Código Causal" DataField="pCausal">
                                        <HeaderStyle Width="65px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Descripción Estándar" DataField="pDescripcionEstandar">
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Detalle Adicional" DataField="pDetalleAdicional"></asp:BoundField>
                                    <asp:BoundField HeaderText="Prenotificación" DataField="pPrenotificacion"></asp:BoundField>
                                    <asp:BoundField HeaderText="Transacción Débito" DataField="pTransaccionDebito">
                                        <HeaderStyle Width="65px" />
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
            </asp:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
