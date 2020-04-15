<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true"
 CodeBehind="HistorialArchivosSalida.aspx.cs" Inherits="Bancos.PS.Modulos.Interpretaciones.HistorialArchivosSalida" %>

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
                        Generar Archivos de Salida
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
                        <asp:Panel ID="pnlAsobancaria" runat="server" ScrollBars="Auto" Width="100%" Height="620px">
                           <asp:Panel ID="pnlBanco" CssClass="PanelBordesRedondos" runat="server" Width="99%" ScrollBars="None">
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
                           <asp:Panel ID="pnlArchivo" CssClass="PanelBordesRedondos" runat="server" Width="99%" ScrollBars="None">
                            <table style="width: 100%" cellpadding="0" cellspacing="2">
                            <tr>
                               <td class="LetraLeyendaColor" colspan="4">
                                    Archivos
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
                                   <asp:Label ID="lblArchivoSalidaAso" runat="server" Text="Ruta Salida:"></asp:Label>
                                   </td>
                                   <td class="EspaciadoIntermedio">
                                   </td>
                                   <td>
                                   <asp:TextBox ID="txbArchivoSalida" CssClass="BordeControles" runat="server" 
                                           ValidationGroup="1" Width="360px" Enabled="False"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvArchivoSalida" runat="server" ErrorMessage="Favor digitar la ruta donde se colocaran los archivos procesados!"
                                                ForeColor="Red" ControlToValidate="txbArchivoSalida" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceArchivoSalida" runat="server" Enabled="True"
                                                TargetControlID="rfvArchivoSalida" WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png"
                                                HighlightCssClass="Resaltar" PopupPosition="BottomLeft">
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
                               <asp:Label ID="lblFecha" runat="server" Text="Fechas de los Archivos:"></asp:Label>
                               </td>
                               <td class="EspaciadoIntermedio">
                               </td>
                               <td>
                                <asp:DropDownList ID="ddlFechas" CssClass="BordeListas" runat="server" 
                                                ValidationGroup="1" 
                                                AutoPostBack="True" Enabled="False" 
                                       onselectedindexchanged="ddlFechas_SelectedIndexChanged" Width="180px">
                                </asp:DropDownList>
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
                               <asp:Label ID="lblConsecutivo" runat="server" Text="Consecutivo del Archivo:"></asp:Label>
                               </td>
                               <td class="EspaciadoIntermedio">
                               </td>
                               <td>
                                <asp:DropDownList ID="ddlConsecutivo" CssClass="BordeListas" runat="server" 
                                                ValidationGroup="1" 
                                                AutoPostBack="True" Enabled="False" 
                                       onselectedindexchanged="ddlConsecutivo_SelectedIndexChanged" Width="90px" 
                                       Height="18px">
                                </asp:DropDownList>
                               </td>
                               <td class="EspaciadoFinal">
                               </td>
                            </tr>   
                             <tr>
                                <td style="height: 10px" colspan="6">
                                </td>
                            </tr>   
                             <tr>                                       
                                        <td colspan ="6" style="text-align: center">
                                            <asp:Button ID="btnGenerar" runat="server" Text="Generar Archivo" 
                                                Width="116px" Height="31px" Enabled="False"
                                                ValidationGroup="1" onclick="btnGenerar_Click"/>
                                        </td>                                        
                             </tr> 
                             <tr>
                                <td style="height: 10px" colspan="6">
                                </td>
                            </tr>                         
                            </table>                           
                           </asp:Panel>
                           <asp:RoundedCornersExtender ID="RoundedCornersExtender2" Radius="3" BorderColor="181, 198, 214"
                                               TargetControlID="pnlArchivo" runat="server" Enabled="True">
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
                                <div class="contenido">
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
