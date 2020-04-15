<%@ Page Title="Bancos - Archivo de Asobancaria" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master"
    AutoEventWireup="true" CodeBehind="EstructuraArchivos.aspx.cs" Inherits="Bancos.PS.Modulos.Banco.EstructuraArchivos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
    <table align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnNuevo" runat="server" ImageUrl="~/MarcaVisual/iconos/nuevo.png" Width="16px"
                    ToolTip="Nuevo..." OnClick="imgBtnNuevo_Click" />
            </td>
           <%-- <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnGuardar" runat="server" ImageUrl="~/MarcaVisual/iconos/guardar.png" Width="16px"
                    ToolTip="Guardar" ValidationGroup="1" OnClick="imgBtnGuardar_Click" />
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
                        Definir Estructura del Archivo
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
                        <asp:Panel ID="pnlArchivoAsoban" runat="server" ScrollBars="Auto" Width="100%" Height="620px">
                        <asp:Panel ID="pnlTipoArchivo" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                ScrollBars="None">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                     <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Función del archivo
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="Label1" runat="server" Text="Tipo archivo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoArchivo" CssClass="BordeListas" runat="server" 
                                                ValidationGroup="2" 
                                                onselectedindexchanged="ddlTipoArchivo_SelectedIndexChanged"
                                                AutoPostBack="True">
                                               <%-- <asp:ListItem>[Seleccione]</asp:ListItem>
                                                <asp:ListItem>Recaudo</asp:ListItem>
                                                <asp:ListItem>Facturación</asp:ListItem>
                                                <asp:ListItem>Telefono Rojo</asp:ListItem>
                                                <asp:ListItem>Pagos Online</asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTipoArchivo" runat="server" ErrorMessage="Favor seleccionar Tipo de Archivo!"
                                                ForeColor="Red" ControlToValidate="ddlTipoArchivo" InitialValue="0" ValidationGroup="2"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoArchivo" runat="server" Enabled="True" TargetControlID="rfvTipoArchivo"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                </table>
                                
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="RoundedCornersExtender1" Radius="3" BorderColor="181, 198, 214"
                            TargetControlID="pnlTipoArchivo" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlArchivo" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                ScrollBars="None">                                                            

                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Partes del archivo
                                        </td>
                                        <td rowspan="2" style="text-align: right">
                                            <asp:ImageButton ID="imgAgregarTipoLinea" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                OnClick="imgAgregarTipoLinea_Click" ValidationGroup="2" Width="16px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbTipoParte" runat="server" Text="Tipo de parte del archivo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoLinea" CssClass="BordeListas" runat="server" 
                                                ValidationGroup="2" Enabled="False">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTipoLinea" runat="server" ErrorMessage="Favor seleccionar la parte del archivo a agregar!"
                                                ForeColor="Red" ControlToValidate="ddlTipoLinea" InitialValue="0" ValidationGroup="2"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoLinea" runat="server" Enabled="True" TargetControlID="rfvTipoLinea"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceArchivo" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlArchivo" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:GridView ID="gvTipoLinea" runat="server" AutoGenerateColumns="False" Width="99%"
                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                GridLines="Horizontal" OnRowCommand="gvTipoLinea_RowCommand" PageSize="3" AllowPaging="True"
                                OnPageIndexChanging="gvTipoLinea_PageIndexChanging" OnSelectedIndexChanged="gvTipoLinea_SelectedIndexChanged">
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
                                    <asp:BoundField HeaderText="OID" DataField="OID" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Tipo_Linea" DataField="Tipo_Linea" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Archivo_Plano" DataField="Archivo_Plano" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Nombre" DataField="NOMBRE"></asp:BoundField>
                                </Columns>
                                <HeaderStyle BackColor="#C5C5C6" />
                                <RowStyle HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1pt" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" />
                            </asp:GridView>
                            <br />
                            <asp:Panel ID="pnlEstructura" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="8">
                                            Campos de la estructura de la l&iacute;nea
                                            <asp:Label ID="lbTLEditando" runat="server" Text="NO HAY LINEA SELECCIONADA"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbOrdenColumna" runat="server" Text="Orden Columna:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbOrdenColumna" CssClass="BordeControles" runat="server" ValidationGroup="3"></asp:TextBox>
                                            <asp:MaskedEditExtender ID="meeOrdenColumna" runat="server" Enabled="True" AutoComplete="false"
                                                InputDirection="RightToLeft" MaskType="Number" Mask="99999" TargetControlID="txbOrdenColumna">
                                            </asp:MaskedEditExtender>
                                            <asp:MaskedEditValidator ID="mevOrdenColumna" runat="server" ControlExtender="meeOrdenColumna"
                                                Enabled="true" EmptyValueMessage="Favor digitar el orden de columna del campo!"
                                                ErrorMessage="Favor digitar la OrdenColumna del campo!" InvalidValueMessage="Favor digitar el orden de columna del campo!"
                                                Text="*" ForeColor="Red" ControlToValidate="txbOrdenColumna" ValidationGroup="3"
                                                IsValidEmpty="false" Display="None"></asp:MaskedEditValidator>
                                            <asp:RequiredFieldValidator ID="rfvOrdenColumna" runat="server" ErrorMessage="*"
                                                ForeColor="Red" ControlToValidate="txbOrdenColumna" ValidationGroup="3" Text="*"
                                                SetFocusOnError="true">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceOrdenColumna" runat="server" Enabled="True"
                                                TargetControlID="mevOrdenColumna" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbNombreCampo" runat="server" Text="Nombre del Campo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNombreCampo" CssClass="BordeControles" ValidationGroup="3" MaxLength="50"
                                                runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvNombreCampo" runat="server" ErrorMessage="Favor digitar el Nombre del Campo!"
                                                ForeColor="Red" ControlToValidate="txbNombreCampo" ValidationGroup="3" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNombreCampo" runat="server" Enabled="True" TargetControlID="rfvNombreCampo"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbAlineacion" runat="server" Text="Alineación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:DropDownList ID="ddlAlineacion" runat="server" CssClass="BordeListas" ValidationGroup="3">
                                                <asp:ListItem Value="0">[Seleccione...]</asp:ListItem>
                                                <asp:ListItem Value="izq">Izquierda</asp:ListItem>
                                                <asp:ListItem Value="der">Derecha</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvAlineacion" runat="server" ErrorMessage="Favor seleccionar la alineacion del campo!"
                                                ForeColor="Red" ControlToValidate="ddlAlineacion" InitialValue="0" ValidationGroup="3"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceAlineacion" runat="server" Enabled="True" TargetControlID="rfvAlineacion"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbLongitud" runat="server" Text="Longitud:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbLongitud" CssClass="BordeControles" runat="server" ValidationGroup="3"></asp:TextBox>
                                            <asp:MaskedEditExtender ID="meeLongitud" runat="server" Enabled="True" AutoComplete="false"
                                                InputDirection="RightToLeft" MaskType="Number" Mask="99999" TargetControlID="txbLongitud">
                                            </asp:MaskedEditExtender>
                                            <asp:MaskedEditValidator ID="mevLongitud" runat="server" ControlExtender="meeLongitud"
                                                Enabled="true" EmptyValueMessage="Favor digitar la longitud del campo!" ErrorMessage="Favor digitar la longitud del campo!"
                                                InvalidValueMessage="Favor digitar la longitud del campo!" Text="*" ForeColor="Red"
                                                ControlToValidate="txbLongitud" ValidationGroup="3" IsValidEmpty="false" Display="None"></asp:MaskedEditValidator>
                                            <asp:RequiredFieldValidator ID="rfvLongitud" runat="server" ErrorMessage="*" ForeColor="Red"
                                                ControlToValidate="txbLongitud" ValidationGroup="3" Text="*" SetFocusOnError="true">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceLongitud" runat="server" Enabled="True" TargetControlID="mevLongitud"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar"
                                                PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbCaracterRelleno" runat="server" Text="Carácter de Relleno:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCaracterRelleno" runat="server" CssClass="BordeListas" ValidationGroup="3">
                                                <asp:ListItem Value="0">[Seleccione...]</asp:ListItem>
                                                <asp:ListItem Value="ES">Espacio</asp:ListItem>
                                                <asp:ListItem Value="CR">Cero</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCaracterRelleno" runat="server" ErrorMessage="Favor seleccionar el carácter de relleno!"
                                                ForeColor="Red" ControlToValidate="ddlCaracterRelleno" InitialValue="0" ValidationGroup="3"
                                                Text="*" SetFocusOnError="true">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCaracterRelleno" runat="server" Enabled="True"
                                                TargetControlID="rfvCaracterRelleno" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbEsContador" runat="server" Text="¿Es el Campo Contador?:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chbEsContador" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbRequiereCambio" runat="server" Text="¿Requiere Transformación?:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chbRequiereCambio" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbTieneValor" runat="server" Text="¿Tiene Valor por Defecto?:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chbTieneValor" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbValor" runat="server" Text="Valor:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbValor" CssClass="BordeControles" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbTipoDato" runat="server" Text="Tipo Dato:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoDato" runat="server" CssClass="BordeListas" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlTipoDato_SelectedIndexChanged" ValidationGroup="3">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTipoDato" runat="server" ErrorMessage="Favor seleccionar el tipo de dato!"
                                                ForeColor="Red" ControlToValidate="ddlTipoDato" InitialValue="0" ValidationGroup="3"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoDato" runat="server" Enabled="True" TargetControlID="rfvTipoDato"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas">
                                            <asp:Label ID="lbCDoFF" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txbCantidadDecimales" CssClass="BordeControles" runat="server" Visible="false"
                                                ValidationGroup="4"></asp:TextBox>
                                            <asp:TextBoxWatermarkExtender ID="tbweCantidadDecimales" WatermarkText="Ejemplo: 3"
                                                WatermarkCssClass="MarcaAgua" runat="server" Enabled="True" TargetControlID="txbCantidadDecimales">
                                            </asp:TextBoxWatermarkExtender>
                                            <asp:RequiredFieldValidator ID="rfvCantidadDecimales" runat="server" ErrorMessage="Favor digitar la cantidad de decimales!"
                                                ForeColor="Red" ControlToValidate="txbCantidadDecimales" ValidationGroup="4"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCantidadDecimales" runat="server" Enabled="True"
                                                TargetControlID="rfvCantidadDecimales" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>

                                            <asp:DropDownList ID="ddlFormatoFecha" runat="server" CssClass="BordeListas" 
                                                ValidationGroup="4" Height="16px" Width="108px" Visible="false">
                                                <asp:ListItem Value="0">[Seleccione...]</asp:ListItem>
                                                <asp:ListItem Value="dd/MM/yyyy">dd/MM/yyyy</asp:ListItem>
                                                <asp:ListItem Value="MM/dd/yyyy">MM/dd/yyyy</asp:ListItem>
                                                <asp:ListItem Value="yyyy/MM/dd">yyyy/MM/dd</asp:ListItem>
                                                <asp:ListItem Value="yyyy/dd/MM">yyyy/dd/MM</asp:ListItem>
                                                <asp:ListItem Value="MM/yyyy/dd">MM/yyyy/dd</asp:ListItem>
                                                <asp:ListItem Value="dd/yyyy/MM">dd/yyyy/MM</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvFormatoFecha" runat="server" ErrorMessage="Favor digitar el formato de fecha!"
                                                ForeColor="Red" ControlToValidate="ddlFormatoFecha" ValidationGroup="4" Text="*"
                                                SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFormatoFecha" runat="server" Enabled="True"
                                                TargetControlID="rfvFormatoFecha" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                            <%--<asp:TextBox ID="txbFormatoFecha" CssClass="BordeControles" ValidationGroup="4" runat="server"
                                                Visible="false"></asp:TextBox>
                                            <asp:TextBoxWatermarkExtender ID="txbwFormatoFecha" WatermarkText="Ejemplo: yyyyMMdd"
                                                WatermarkCssClass="MarcaAgua" runat="server" Enabled="True" TargetControlID="txbFormatoFecha">
                                            </asp:TextBoxWatermarkExtender>
                                            <asp:RequiredFieldValidator ID="rfvFormatoFecha" runat="server" ErrorMessage="Favor digitar el formarto de fecha!"
                                                ForeColor="Red" ControlToValidate="txbFormatoFecha" ValidationGroup="4" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFormatoFecha" runat="server" Enabled="True"
                                                TargetControlID="rfvFormatoFecha" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>--%>

                                        </td>
                                        <td>
                                        <asp:ImageButton ID="imgBtnAgregarCampo" runat="server" ValidationGroup="3" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                OnClick="imgBtnAgregarCampo_Click" Width="20px" Height="16px" />    
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEstructura" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlEstructura" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:GridView ID="gvCampos" runat="server" AutoGenerateColumns="False" Width="99%"
                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                GridLines="Horizontal" OnRowCommand="gvCampos_RowCommand" AllowPaging="True"
                                OnPageIndexChanging="gvCampos_PageIndexChanging" PageSize="3">
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
                                    <asp:BoundField HeaderText="OC" DataField="pOrdenColumna" HeaderStyle-Width="60px">
                                        <HeaderStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Nombre Campo" DataField="pNombreCampo"></asp:BoundField>
                                    <asp:BoundField HeaderText="Long." DataField="pLongitud" HeaderStyle-Width="60px">
                                        <HeaderStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Car. de Relleno" DataField="pCaracterRelleno" HeaderStyle-Width="70px">
                                        <HeaderStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Alineación" DataField="pAlineacion" HeaderStyle-Width="70px">
                                        <HeaderStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Tipo de Dato" DataField="pTipoDato" />
                                    <asp:BoundField HeaderText="Cant. Dec." DataField="pCantidadDecimales" />
                                    <asp:BoundField HeaderText="Formato Fecha" DataField="pFormatoFecha" />
                                    <asp:CheckBoxField HeaderText="Campo Contador" DataField="pEsContador">
                                        <HeaderStyle Width="60px" />
                                    </asp:CheckBoxField>
                                    <asp:CheckBoxField HeaderText="Req. Trans." DataField="pRequiereCambio">
                                        <HeaderStyle Width="60px" />
                                    </asp:CheckBoxField>
                                    <asp:CheckBoxField HeaderText="Tiene Valor Por Defecto" DataField="pValorPorDefecto">
                                        <HeaderStyle Width="60px" />
                                    </asp:CheckBoxField>
                                    <asp:BoundField HeaderText="Valor" DataField="pValor" />
                                </Columns>
                                <HeaderStyle BackColor="#C5C5C6" />
                                <RowStyle HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1pt" />
                                <PagerSettings Mode="NumericFirstLast" />
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
