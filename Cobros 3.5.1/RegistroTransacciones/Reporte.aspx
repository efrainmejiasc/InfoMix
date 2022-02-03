<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reporte.aspx.cs" Inherits="RegistroTransacciones.Reporte" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">   
  
  
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
          
            <div id="imprimirBuscarPacienteAvni" align="center">
               
                <table width="600" border="0" cellpadding="1" cellspacing="1" class="texto">
                   
                   
                  <tr class="nombre">
                        <td>
                            Año
                        </td>
                        <td colspan="3">
                           <asp:TextBox runat="server" ID="txtanio"></asp:TextBox>
                        </td>
                    </tr>
                     <tr class="nombre">
                        <td>
                            mes
                        </td>
                        <td colspan="3">
                           <asp:DropDownList ID="ddlMes" runat="server">
                                        <asp:ListItem Value="0" Text="Seleccione mes.."></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Enero"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Febrero"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Marzo"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Mayo"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="Junio"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="Julio"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="Septiembre"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="Octubre"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="Noviembre"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="Diciembre"></asp:ListItem>
                                    </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="nombre">
                        <td>
                            Caja
                        </td>
                        <td colspan="3">
                             <asp:DropDownList runat="server" ID="ddlCaja" Style="width: 125px;">
                                        <asp:ListItem Value="0" Text="Seleccione Caja"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Central"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Auxiliar"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Facturas Web"></asp:ListItem>
                                    </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div align="center">
                                <table width="20px" border="0" cellpadding="1" cellspacing="1" class="texto">
                                    <tr align="center">
                                        <td>
                                            <asp:Button ID="btnBuscar" Height="30px" Width="120px" runat="server" Text="Buscar" OnClick="btnBuscar_Click">
                                            </asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnLimpiar" Height="30px" Width="120px" runat="server" Text="Limpiar Datos" OnClick="BtnLimpiar_Click">
                                            </asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnExportar" Height="30px" Width="120px" runat="server" Text="Exportar" OnClick="btnExportar_Click">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr class="td1">
                        <td colspan="4">
                            <div align="center">
                                <table width="50px" border="0" cellpadding="1" cellspacing="1" class="texto">
                                    <tr valign="top" class="td1">
                                        <td>
                                            <table class="ContainerWrapper" border="0" cellpadding="2" cellspacing="0" width="80%">
                                                <tr class="ContainerHeader">
                                                    <td>
                                                        Detalle Reporte
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="MostrarDetalle" style="overflow: auto; display: display">
                                                            <asp:Panel ID="Contenedor" runat="server" Height="300px" Width="650px" ScrollBars="Auto">
                                                                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="GVReportes" runat="server" CellPadding="6" ForeColor="#333333"
                                                                                Width="100%" Font-Size="10px" BorderColor="Lavender" BorderWidth="1px" Font-Names="Arial"
                                                                                Font-Overline="False" ShowFooter="True" ShowHeaderWhenEmpty="True">
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" BorderColor="Lavender" />
                                                                                <RowStyle BackColor="AliceBlue" ForeColor="SteelBlue" />
                                                                                <EditRowStyle BackColor="#999999" />
                                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" BorderStyle="Solid"
                                                                                    BorderWidth="1px" />
                                                                                <PagerStyle BackColor="#5D7B9D" ForeColor="White" HorizontalAlign="Center" BorderColor="Lavender" />
                                                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" BorderColor="Lavender" />
                                                                                <PagerSettings FirstPageText="Principio" LastPageText="Final" NextPageText="Siguiente"
                                                                                    PreviousPageText="Anterior" />
                                                                                <RowStyle Wrap="True" />
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <tr class="GridFooter">
                                                <td colspan="5">
                                                    <div style="float: left">
                                                        Total de cobros:
                                                        <%= GVReportes.Rows.Count%>
                                                    </div>
                                                </td>
                                            </tr>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
