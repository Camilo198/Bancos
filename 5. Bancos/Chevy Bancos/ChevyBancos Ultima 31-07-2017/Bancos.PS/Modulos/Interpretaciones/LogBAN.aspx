<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="LogBAN.aspx.cs" Inherits="Bancos.PS.Modulos.Interpretaciones.LogBAN" %>
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
                        Logs
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
                        <asp:Panel ID="pnlLogs" runat="server" Height="620px" ScrollBars="Auto" 
                            Width="100%">
                            <asp:Panel ID="pnlDatos" runat="server" CssClass="PanelBordesRedondos" 
                                Width="99%">
                                <table cellpadding="0" cellspacing="2" style="width: 100%">
                                    <tr>
                                         <td style="height: 10px" colspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                            &nbsp;</td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lblTipoArchivo" runat="server" Text="Tipo de Archivo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                            &nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoArchivo" runat="server" AutoPostBack="True" 
                                                CssClass="BordeListas"
                                                ValidationGroup="1" Width="180px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTipoArchivo" runat="server" ErrorMessage="Favor seleccionar Tipo de Archivo!"
                                                ForeColor="Red" ControlToValidate="ddlTipoArchivo" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoArchivo" runat="server" Enabled="True" TargetControlID="rfvTipoArchivo"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
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
                                            &nbsp;</td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lblTipoProceso" runat="server" Text="Tipo de Proceso:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                            &nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoProceso" runat="server" AutoPostBack="True" 
                                                CssClass="BordeListas"
                                                ValidationGroup="1" Width="180px" >
                                                 <asp:ListItem Text="[Seleccione]" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Generar Archivo" Value="GEN"></asp:ListItem>
                                                <asp:ListItem Text="Estructura de Archivo" Value="EST"></asp:ListItem>                                                
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTipoProceso" runat="server" ErrorMessage="Favor seleccionar Tipo de Proceso!"
                                                ForeColor="Red" ControlToValidate="ddlTipoProceso" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoProceso" runat="server" Enabled="True" TargetControlID="rfvTipoProceso"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
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
                                        De:
                                            <asp:TextBox ID="txbFechaInicial" runat="server" CssClass="FuenteDDL" 
                                                TabIndex="2" ValidationGroup="1" Width="70px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnFechaInicial" runat="server" ImageAlign="Middle" 
                                                ImageUrl="~/MarcaVisual/iconos/calendario.png" Width="20px" />
                                            <asp:CalendarExtender ID="ceFechaInicial" runat="server" Enabled="True" 
                                                Format="dd/MM/yyyy" PopupButtonID="imgBtnFechaInicial" 
                                                TargetControlID="txbFechaInicial" TodaysDateFormat="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meeFechaInicial" runat="server" 
                                                InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" 
                                                TargetControlID="txbFechaInicial" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                            <asp:RequiredFieldValidator ID="rfvFechaInicial" runat="server" 
                                                ControlToValidate="txbFechaInicial" 
                                                ErrorMessage="Favor digitar o seleccionar del calendario la fecha inicial!" 
                                                ForeColor="Red" SetFocusOnError="true" Text="*" ValidationGroup="1">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFechaInicial" runat="server" 
                                                Enabled="True" HighlightCssClass="Resaltar" TargetControlID="rfvFechaInicial">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                        A:
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
                                                TargetControlID="txbFechaFin" TodaysDateFormat="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meeFechaFin" runat="server" 
                                                InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" 
                                                TargetControlID="txbFechaFin" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnBuscar" runat="server" Text="Cargar" 
                                                onclick="btnBuscar_Click" ValidationGroup="1" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="height: 10px" colspan="4">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceDatos" runat="server" 
                                BorderColor="181, 198, 214" Enabled="True" Radius="3" 
                                TargetControlID="pnlDatos">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:GridView ID="gvLogs"  runat="server" AutoGenerateColumns="False" Width="99%"
                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                GridLines="Horizontal" PageSize="5" 
                                AllowPaging="True" onpageindexchanging="gvLogs_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField HeaderText="FECHA" DataField="FECHA">
                                     <ControlStyle Width="80px" />
                                     <HeaderStyle Width="30px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="USUARIO" DataField="USUARIO">
                                     <ControlStyle Width="100px" />
                                     <HeaderStyle Width="30px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="DETALLE" DataField="DETALLE" >
                                    <ControlStyle Width="200px" />
                                    <HeaderStyle Width="60px" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
<asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
