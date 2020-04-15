<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="BancoPagosErrados.aspx.cs" Inherits="Bancos.PS.Modulos.Banco.BancoPagosErrados" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td class="BarraSubTitulo">
                        Pagos Errados
                    </td>
                </tr>
                <tr>
                    <td class="SeparadorSubTitulo">
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" class="ColorContenedorDatos" 
                style="width: 100%">
                <tr>
                    <td colspan="3" style="height: 10px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 10px">
                    </td>
                    <td>
                        <asp:Panel ID="pnlBanco" runat="server" Height="620px" ScrollBars="Auto" 
                            Width="100%">
                            <asp:Panel ID="pnlRutas" runat="server" CssClass="PanelBordesRedondos" 
                                Width="99%">
                                <table cellpadding="0" cellspacing="2" style="width: 100%">
                                    <tr>
                                        <td class="style1" colspan="4">
                                            Desde
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                            &nbsp;</td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lblBanco" runat="server" Text="Banco:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                            &nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="ddlNombreBanco" runat="server" AutoPostBack="True" 
                                                CssClass="BordeListas" 
                                                OnSelectedIndexChanged="ddlNombreBanco_SelectedIndexChanged" 
                                                ValidationGroup="1" Width="180px">
                                            </asp:DropDownList>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                            &nbsp;</td>
                                        <td class="EstiloEtiquetas175">
                                            &nbsp;</td>
                                        <td class="EspaciadoIntermedio">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:TextBox ID="txbFechaInicial" runat="server" CssClass="FuenteDDL" 
                                                TabIndex="2" ValidationGroup="1" Width="70px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnFechaInicial" runat="server" ImageAlign="Middle" 
                                                ImageUrl="~/MarcaVisual/iconos/calendario.png" Width="20px" />
                                            <asp:CalendarExtender ID="ceFechaInicial" runat="server" Enabled="True" 
                                                Format="dd/MM/yyyy" PopupButtonID="imgBtnFechaInicial" 
                                                TargetControlID="txbFechaInicial">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meeFechaInicial" runat="server" 
                                                InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" 
                                                TargetControlID="txbFechaInicial" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                            <asp:RequiredFieldValidator ID="rfvFechaInicial" runat="server" 
                                                ControlToValidate="txbFechaInicial" 
                                                ErrorMessage="Favor digitar o seleccionar del calendariola fecha inicial!" 
                                                ForeColor="Red" SetFocusOnError="true" Text="*" ValidationGroup="1">
                                    </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFechaInicial" runat="server" 
                                                Enabled="True" HighlightCssClass="Resaltar" TargetControlID="rfvFechaInicial">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbFechaFin" runat="server" CssClass="FuenteDDL" TabIndex="3" 
                                                ValidationGroup="1" Width="70px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnFechaFin" runat="server" ImageAlign="Middle" 
                                                ImageUrl="~/MarcaVisual/iconos/calendario.png" Width="20px" />
                                            <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" 
                                                ControlToValidate="txbFechaFin" 
                                                ErrorMessage="Favor digitar o seleccionar del calendario la fecha final!" 
                                                ForeColor="Red" SetFocusOnError="true" Text="*" ValidationGroup="1">
                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFechaFin" runat="server" Enabled="True" 
                                                HighlightCssClass="Resaltar" TargetControlID="rfvFechaFin">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:CalendarExtender ID="ceFechaFin" runat="server" Enabled="True" 
                                                Format="dd/MM/yyyy" PopupButtonID="imgBtnFechaFin" 
                                                TargetControlID="txbFechaFin">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meeFechaFin" runat="server" 
                                                InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" 
                                                TargetControlID="txbFechaFin" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnCodigoBanco" runat="server" onclick="btnCodigoBanco_Click" 
                                                Text="Cargar" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceRutas" runat="server" 
                                BorderColor="181, 198, 214" Enabled="True" Radius="3" 
                                TargetControlID="pnlRutas">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:GridView ID="gvTipoLinea" runat="server" AllowPaging="True" 
                                AllowSorting="True" AutoGenerateColumns="False" BorderColor="#D0DEF0" 
                                BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal" 
                                OnPageIndexChanging="gvTipoLinea_PageIndexChanging" 
                                OnRowCommand="gvTipoLinea_RowCommand" PageSize="15" Width="99%">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Editar" 
                                        ImageUrl="~/MarcaVisual/iconos/editar.png" Text="Editar">
                                    <ItemStyle Width="30px" />
                                    <ControlStyle Width="16px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="OcultarControles" 
                                        HeaderText="ID" ItemStyle-CssClass="OcultarControles">
                                    <HeaderStyle CssClass="OcultarControles" />
                                    <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CodBanco" HeaderText="Cod. Banco" 
                                        SortExpression="CodBanco">
                                    </asp:BoundField>
                                    <asp:BoundField HeaderStyle-CssClass="OcultarControles" 
                                        HeaderText="Contrato" ItemStyle-CssClass="OcultarControles">
                                    <HeaderStyle CssClass="OcultarControles" />
                                    <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CodError" HeaderText="Cod. Error" 
                                        SortExpression="CodError" />
                                    <asp:BoundField DataField="DescripcionError" HeaderText="Descripción" />
                                    <asp:TemplateField HeaderText="Corregir">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CheckBox3" runat="server" 
                                                Checked='<%# Bind("Corregido") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" 
                                                Checked='<%# Bind("Corregido") %>' Enabled="False" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:CheckBox ID="CheckBox2" runat="server" 
                                        Checked='<%# Bind("Corregido") %>' />
                                </EmptyDataTemplate>
                                <HeaderStyle BackColor="#C5C5C6" />
                                <RowStyle BorderStyle="Solid" BorderWidth="1pt" HorizontalAlign="Center" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlBusquedaBanco" runat="server" CssClass="ContenedorDatos" 
                Height="400px" Width="600px">
                <asp:UpdatePanel ID="upBusquedaBanco" runat="server">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td class="BarraSubTitulo">
                                    Búsqueda
                                </td>
                            </tr>
                            <tr>
                                <td class="SeparadorSubTitulo">
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td class="EspaciadoInicial">
                                </td>
                                <td>
                                    <asp:Panel ID="pnlDatosB" runat="server" CssClass="PanelBordesRedondos" 
                                        Width="99%">
                                        <table cellpadding="0" cellspacing="2" style="width: 100%">
                                            <tr>
                                                <td class="EspaciadoInicial">
                                                </td>
                                                <td class="EstiloEtiquetas">
                                                    <asp:Label ID="lbCodigoBanco" runat="server" Text="Código Banco:"></asp:Label>
                                                </td>
                                                <td class="EspaciadoIntermedio">
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txbCodigoBancoB" runat="server" CssClass="BordeControles" 
                                                        MaxLength="3" Width="30px"></asp:TextBox>
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
                                                    <asp:TextBox ID="txbNombreBancoB" runat="server" CssClass="BordeControles" 
                                                        MaxLength="100" Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:RoundedCornersExtender ID="rceEsquinasRedondasB" runat="server" 
                                        BorderColor="181, 198, 214" Enabled="True" Radius="3" 
                                        TargetControlID="pnlDatosB">
                                    </asp:RoundedCornersExtender>
                                </td>
                                <td style="width: 130px; text-align: center; vertical-align: bottom">
                                    <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" 
                                        Text="Buscar" UseSubmitBehavior="True" Width="110px" />
                                    <asp:Button ID="btnNuevaBusqueda" runat="server" 
                                        OnClick="btnNuevaBusqueda_Click" Text="Nueva Búsqueda" Width="110px" />
                                    <asp:Button ID="btnLimpiar" runat="server" Enabled="false" 
                                        OnClick="btnLimpiar_Click" Text="Limpiar Resultados" Width="110px" />
                                    <asp:Button ID="btnCancelar" runat="server" onclick="btnCancelar_Click" 
                                        Text="Cancelar" UseSubmitBehavior="false" Width="110px" />
                                </td>
                                <td class="EspaciadoInicial">
                                </td>
                            </tr>
                            <tr>
                                <td class="SeparadorHorizontal" colspan="4">
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="2" style="width: 100%">
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvBusquedaBanco" runat="server" AllowPaging="True" 
                                        AllowSorting="True" AutoGenerateColumns="False" BorderColor="#D0DEF0" 
                                        BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal" 
                                        OnPageIndexChanging="gvBusquedaBanco_PageIndexChanging" 
                                        OnRowCommand="gvBusquedaBanco_RowCommand" PageSize="9" Width="100%">
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="sel" 
                                                ImageUrl="~/MarcaVisual/iconos/aceptar.png" Text="Seleccionar">
                                            <ItemStyle Width="50px" />
                                            <ControlStyle Width="16px" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="pCodigoBanco" HeaderText="Código Banco" 
                                                ItemStyle-Width="80px">
                                            <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="pNombre" HeaderText="Nombre Banco" />
                                            <asp:CheckBoxField DataField="pRecFac" HeaderText="Entrada" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>