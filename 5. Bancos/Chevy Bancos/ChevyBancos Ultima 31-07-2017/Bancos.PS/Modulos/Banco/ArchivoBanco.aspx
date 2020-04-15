<%@ Page Title="Bancos - Archivo del Banco" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master"
    AutoEventWireup="true" CodeBehind="ArchivoBanco.aspx.cs" Inherits="Bancos.PS.Modulos.Banco.ArchivoBanco" %>
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
            <%--<td class="CuadranteBotonImagen">
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
                        Definir Estructura del Archivo Plano del Banco
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
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>
                                     <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbTipoProceso" runat="server" Text="Tipo de Proceso:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:DropDownList ID="ddlTipoProceso" runat="server" AutoPostBack="true" 
                                                CssClass="BordeListas" ValidationGroup="1" 
                                                onselectedindexchanged="ddlTipoProceso_SelectedIndexChanged" Height="16px" 
                                                Width="170px" >
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
                                        <td colspan = "5">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9">
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
                                        <td class="EstiloEtiquetas175">
                                            <asp:Button ID="btnCodigoCuenta" runat="server" Text="Código de la Cuenta:" />
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbCodigoCuenta" runat="server" CssClass="BordeControles" MaxLength="5"
                                                Width="40px" ValidationGroup="1" AutoPostBack="true" 
                                                ontextchanged="txbCodigoCuenta_TextChanged" Height="19px" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCodigoCuenta" runat="server" ErrorMessage="Favor digitar el Codigo de la Cuenta!"
                                                ForeColor="Red" ControlToValidate="txbCodigoCuenta" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCodigoCuenta" runat="server" Enabled="True" TargetControlID="rfvCodigoCuenta"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td style="width: 20%" class="EstiloEtiquetas175">
                                            <asp:Label ID="lbNombreCuenta" runat="server" Text="Nombre de la Cuenta:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNombreCuenta" runat="server" CssClass="BordeControles" MaxLength="100"
                                                ReadOnly="true" Height="20px" Width="140px" Enabled="False" ValidationGroup="1"></asp:TextBox>
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
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEsquinasRedondas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDatos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlFormato" CssClass="PanelBordesRedondos" runat="server" Width="99%"  Enabled="False">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">                                     
                                     <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>
                                     <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="Label1" runat="server" Text="Tipo de Archivo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                             <asp:RadioButton ID="rbExcel" runat="server" Text="Excel" GroupName="formato"/>
                                             <asp:RadioButton ID="rbPlano" runat="server" Text="Archivo Plano" 
                                                 GroupName="formato" Checked="True"/>
                                        </td>
                                        <td colspan = "6">
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="Label2" runat="server" Text="Número Hoja Excel:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNumExcel" runat="server" Width="70px" MaxLength="4"></asp:TextBox>  
                                            <asp:MaskedEditExtender ID="meeNumExcel" runat="server" Enabled="True" AutoComplete="false"
                                                InputDirection="RightToLeft" MaskType="Number" Mask="9999" TargetControlID="txbNumExcel">
                                            </asp:MaskedEditExtender>
                                            <asp:MaskedEditValidator ID="mevNumExcel" runat="server" ControlExtender="meeNumExcel"
                                                Enabled="true" EmptyValueMessage="Favor digitar un número!"
                                                ErrorMessage="Favor digitar un número!" InvalidValueMessage="Favor digitar un número!"
                                                Text="*" ForeColor="Red" ControlToValidate="txbNumExcel" ValidationGroup="1"
                                                IsValidEmpty="false" Display="None"></asp:MaskedEditValidator>
                                            <asp:RequiredFieldValidator ID="rfvNumExcel" runat="server" ErrorMessage="*"
                                                ForeColor="Red" ControlToValidate="txbNumExcel" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNumExcel" runat="server" Enabled="True"
                                                TargetControlID="mevNumExcel" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td colspan = "6">
                                        </td>
                                    </tr>                                   
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="Label3" runat="server" Text="Lineas Excluidas al Incio:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNExluidasInicio" runat="server" Width="70px" MaxLength="4"></asp:TextBox>
                                            <asp:MaskedEditExtender ID="meeNExluidasInicio" runat="server" Enabled="True" AutoComplete="false"
                                                InputDirection="RightToLeft" MaskType="Number" Mask="9999" TargetControlID="txbNExluidasInicio">
                                            </asp:MaskedEditExtender>
                                            <asp:MaskedEditValidator ID="mevNExluidasInicio" runat="server" ControlExtender="meeNExluidasInicio"
                                                Enabled="true" EmptyValueMessage="Favor digitar un número!"
                                                ErrorMessage="Favor digitar un número!" InvalidValueMessage="Favor digitar un número!"
                                                Text="*" ForeColor="Red" ControlToValidate="txbNExluidasInicio" ValidationGroup="1"
                                                IsValidEmpty="false" Display="None"></asp:MaskedEditValidator>
                                            <asp:RequiredFieldValidator ID="rfvNExluidasInicio" runat="server" ErrorMessage="*"
                                                ForeColor="Red" ControlToValidate="txbNExluidasInicio" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNExluidasInicio" runat="server" Enabled="True"
                                                TargetControlID="mevNExluidasInicio" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="Label4" runat="server" Text="Lineas Excluidas al Final:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EspaciadoCeldaControl">
                                            <asp:TextBox ID="txbNExluidasFinal" runat="server" Width="70px" MaxLength="4"></asp:TextBox>
                                            <asp:MaskedEditExtender ID="meeNExluidasFinal" runat="server" Enabled="True" AutoComplete="false"
                                                InputDirection="RightToLeft" MaskType="Number" Mask="9999" TargetControlID="txbNExluidasFinal">
                                            </asp:MaskedEditExtender>
                                            <asp:MaskedEditValidator ID="mevNExluidasFinal" runat="server" ControlExtender="meeNExluidasFinal"
                                                Enabled="true" EmptyValueMessage="Favor digitar un número!"
                                                ErrorMessage="Favor digitar un número!" InvalidValueMessage="Favor digitar un número!"
                                                Text="*" ForeColor="Red" ControlToValidate="txbNExluidasFinal" ValidationGroup="1"
                                                IsValidEmpty="false" Display="None"></asp:MaskedEditValidator>
                                            <asp:RequiredFieldValidator ID="rfvNExluidasFinal" runat="server" ErrorMessage="*"
                                                ForeColor="Red" ControlToValidate="txbNExluidasFinal" ValidationGroup="1" Text="*"
                                                SetFocusOnError="true">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceNExluidasFinal" runat="server" Enabled="True"
                                                TargetControlID="mevNExluidasFinal" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        <asp:ImageButton ID="imgBtnAgregarFormato" runat="server" ValidationGroup="1" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                 Width="16px" onclick="imgBtnAgregarFormato_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceFormato" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlFormato" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlArchivo" CssClass="PanelBordesRedondos" runat="server" Width="99%"
                                ScrollBars="None" Enabled="False">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">
                                            Partes del archivo
                                        </td>                                       
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbTipoParte" runat="server" Text="Tipo de parte del archivo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoLinea" CssClass="BordeListas" runat="server" ValidationGroup="2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTipoLinea" runat="server" ErrorMessage="Favor seleccionar la parte del archivo a agregar!"
                                                ForeColor="Red" ControlToValidate="ddlTipoLinea" InitialValue="0" ValidationGroup="2"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTipoLinea" runat="server" Enabled="True" TargetControlID="rfvTipoLinea"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td  style="text-align: right" colspan="5">
                                            <asp:ImageButton ID="imgAgregarTipoLinea" runat="server" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                OnClick="imgAgregarTipoLinea_Click" ValidationGroup="2" Width="16px"/>
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
                                GridLines="Horizontal" OnRowCommand="gvTipoLinea_RowCommand" PageSize="3" 
                                AllowPaging="True" onpageindexchanging="gvTipoLinea_PageIndexChanging">
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
                            <asp:Panel ID="pnlEstructura" CssClass="PanelBordesRedondos" runat="server" 
                                Width="99%" Enabled="False">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">
                                            Campos de la estructura de la l&iacute;nea
                                            <asp:Label ID="lbTLEditando" runat="server" Text="NO HAY LINEA SELECCIONADA"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbOrdenColumna" runat="server" Text="Orden Columna:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
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
                                        <td colspan="5">                                        
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas175">
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
                                        <td class="EstiloEtiquetas175">
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
                                        <td class="EstiloEtiquetas175">
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
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbCaracterRelleno" runat="server" Text="Carácter de Relleno:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCaracterRelleno" runat="server" CssClass="BordeListas" ValidationGroup="3">
                                                <asp:ListItem Value="0">[Seleccione...]</asp:ListItem>
                                                <asp:ListItem Value="ES">Espacio &#39; &#39;</asp:ListItem>
                                                <asp:ListItem Value="CR">Cero &#39;0&#39;</asp:ListItem>
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
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbEsContador" runat="server" Text="¿Es el Campo Contador?:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chbEsContador" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas175">
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
                                        <td class="EstiloEtiquetas175">
                                            <asp:Label ID="lbTieneValor" runat="server" Text="¿Tiene Valor por Defecto?:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chbTieneValor" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="EstiloEtiquetas175">
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
                                        <td class="EstiloEtiquetas175">
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
                                        <td class="EstiloEtiquetas175">
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
                                                Height="16px" ValidationGroup="4" Visible="false" Width="108px">
                                                <asp:ListItem Value="0">[Seleccione...]</asp:ListItem>
                                                <asp:ListItem Value="dd/MM/yyyy">dd/MM/yyyy</asp:ListItem>
                                                <asp:ListItem Value="MM/dd/yyyy">MM/dd/yyyy</asp:ListItem>
                                                <asp:ListItem Value="yyyy/MM/dd">yyyy/MM/dd</asp:ListItem>
                                                <asp:ListItem Value="yyyy/dd/MM">yyyy/dd/MM</asp:ListItem>
                                                <asp:ListItem Value="MM/yyyy/dd">MM/yyyy/dd</asp:ListItem>
                                                <asp:ListItem Value="dd/yyyy/MM">dd/yyyy/MM</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvFormatoFecha" runat="server" ErrorMessage="Favor digitar el formarto de fecha!"
                                                ForeColor="Red" ControlToValidate="ddlFormatoFecha" ValidationGroup="4" Text="*"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFormatoFecha" runat="server" Enabled="True"
                                                TargetControlID="rfvFormatoFecha" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="TopLeft">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td>
                                        <asp:ImageButton ID="imgBtnAgregarCampo" runat="server" ValidationGroup="3" ImageUrl="~/MarcaVisual/iconos/agregar.png"
                                                OnClick="imgBtnAgregarCampo_Click" Width="16px" />
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
                                    <asp:BoundField HeaderText="Tipo de Dato" DataField="pTipoDato"/>                                  
                                    <asp:BoundField HeaderText="Cant. Dec." DataField="pCantidadDecimales" />
                                    <asp:BoundField HeaderText="Formato Fecha" DataField="pFormatoFecha" />
                                    <asp:CheckBoxField HeaderText="Campo Contador" DataField="pEsContador">
                                        <HeaderStyle Width="60px" />
                                    </asp:CheckBoxField>
                                    <asp:CheckBoxField HeaderText="Req. Trans." DataField="pRequiereCambio">
                                        <HeaderStyle Width="60px" />
                                    </asp:CheckBoxField>
                                    <asp:CheckBoxField HeaderText="Tiene Valor por Defecto" 
                                        DataField="pValorPorDefecto">
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
