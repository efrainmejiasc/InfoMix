<%@ Page Title="SigfeXML" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="XML.aspx.cs" Inherits="SigFeXML.XML" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="udpanelMain" runat="server">
        <ContentTemplate>
            <div id="DivMensajeOk" runat="server" visible="false">
                <tr>
                    <td>
                        <div style="border: thin groove #FFFFFF; background-color: #CCFFCC">
                            <img src="images/Select.png" />
                            <asp:Label ID="lnlMensajeOk" runat="server" Font-Bold="True" Font-Size="Small">Archivos generados correctamente</asp:Label>
                        </div>
                    </td>
                </tr>
            </div>
           
            <br />
            <tr>
                <td>
                    <asp:Label ID="lblMensajeError" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <br />
            <br />
            <div align="left">
                <div style="position: relative; width: 600px;">
                    <div align="left">
                        <b>Datos Requeridos para generar XML</b>
                    </div>
                    <br />
                    <div style="position: relative; float: left; width: 300px;">
                        <div align="left" style="position: relative; width: 120px; float: left">
                            Ejercicio
                        </div>
                        <div align="left" style="position: relative; width: 180px; float: left">
                            <asp:TextBox runat="server" ID="txtEjercicio"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderEjercicio" runat="server"
                                ValidChars="0123456789" TargetControlID="txtEjercicio">
                            </cc1:FilteredTextBoxExtender>
                        </div>
                    </div>
                    <div style="position: relative; float: left; width: 300px; top: 0px; left: 0px; height: 39px;">
                        <div align="left" style="position: relative; width: 120px; float: left">
                            Periodo
                        </div>
                        <div align="left" style="position: relative; width: 180px; float: left">
                            <asp:DropDownList ID="ddlPeriodo" runat="server">
                                <asp:ListItem Value="0" Text="Seleccione periodo.."></asp:ListItem>
                                <asp:ListItem Value="01" Text="Enero"></asp:ListItem>
                                <asp:ListItem Value="02" Text="Febrero"></asp:ListItem>
                                <asp:ListItem Value="03" Text="Marzo"></asp:ListItem>
                                <asp:ListItem Value="04" Text="Abril"></asp:ListItem>
                                <asp:ListItem Value="05" Text="Mayo"></asp:ListItem>
                                <asp:ListItem Value="06" Text="Junio"></asp:ListItem>
                                <asp:ListItem Value="07" Text="Julio"></asp:ListItem>
                                <asp:ListItem Value="08" Text="Agosto"></asp:ListItem>
                                <asp:ListItem Value="09" Text="Septiembre"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Octubre"></asp:ListItem>
                                <asp:ListItem Value="11" Text="Noviembre"></asp:ListItem>
                                <asp:ListItem Value="12" Text="Diciembre"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div style="clear: both">
                        </div>
                    </div>
                    <br />
                    <br />
                    <div style="position: relative; width: 600px;">
                        <div align="left">
                            <b>Seleccion de tipo de documento</b>
                        </div>
                        <div style="position: relative; float: left; width: 600px;">
                            <div align="left" style="position: relative; width: 150px; float: left">
                                <asp:RadioButton runat="server" ID="rdE" GroupName="tipodoc"
                                    Text="E(Exento)"  />
                            </div>
                            <div align="left" style="position: relative; width: 150px; float: left">
                                <asp:RadioButton runat="server" ID="rdCRP" GroupName="tipodoc"
                                    Text="CRP(Boletas)" />
                            </div>
                            <div align="left" style="position: relative; width: 150px; float: left">
                                <asp:RadioButton runat="server" ID="rdF" GroupName="tipodoc"
                                    Text="F(Factura Afecta)"  />
                            </div>
                            <div align="left" style="position: relative; width: 150px; float: left">
                                <asp:RadioButton  runat="server" ID="rdNotaC" GroupName="tipodoc"
                                    Text="Nota de Crédito" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div style="position: relative; float: left; width: 920px;">
                <tr align="left">
                    <td align="left">
                        <asp:Label runat="server" Visible="False" ID="txtError" Style="color: red;" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </div>
            <br />
            <br />
            <tr>
                <td>
                    <asp:RadioButton runat="server" ID="buscarporfolio" GroupName="RbBuscar" Text="Generar XML ingresando manualmente los Folio"
                        OnCheckedChanged="buscarporfolio_CheckedChanged" AutoPostBack="true" />
                </td>
            </tr>
            <br />
            <tr>
                <td>
                    <asp:RadioButton runat="server" ID="RbBuscarporFecha" GroupName="RbBuscar" Text="Generar XML ingresando Fecha"
                        OnCheckedChanged="RbBuscarporFecha_CheckedChanged" AutoPostBack="true" />
                </td>
            </tr>
            <br />
            <br />
            <br />
            <div align="left">
                <div style="position: relative; width: 600px;">
                    <div id="divBuscarporFecha" runat="server" style="position: relative; float: left;
                        width: 300px;">
                        <div align="left" style="position: relative; width: 120px; float: left">
                            Fecha
                        </div>
                        <div align="left" style="position: relative; width: 180px; float: left">
                            <span>
                                <asp:TextBox runat="server" ID="txtFechaInicio" Width="70px"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender0" runat="server" ValidChars="0123456789-"
                                    TargetControlID="txtFechaInicio">
                                </cc1:FilteredTextBoxExtender>
                                <asp:Image ID="Image" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    Width="16px" />
                            </span>
                            <cc1:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" TargetControlID="txtFechaInicio"
                                PopupButtonID="Image" OnClientDateSelectionChanged="checkDate" Format="dd-MM-yyyy"
                                PopupPosition="TopRight">
                            </cc1:CalendarExtender>
                        </div>
                    </div>
                    <br />
                    <br />
                </div>
            </div>
            <div id="divBuscarporFolio" runat="server" style="position: relative; width: 920px;
                top: 0px; left: 0px;">
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="txtXML" TextMode="MultiLine" Rows="4" Style="width: 920px;"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderXML" runat="server" ValidChars="0123456789,"
                            TargetControlID="txtXML">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
            </div>
            <br />
            <%--<div id="divNotaCredito" align="center" runat="server" visible="false">
                <br />
                <div align="center">
                    <asp:Button ID="btnBuscarNotasCredito" Height="30px" runat="server" Text="Buscar Nota de Crédito"
                        OnClick="btnBuscarNotasCredito_Click" />
                </div>
                <br />
                <asp:GridView ID="GrillaNotaCredito" runat="server" AutoGenerateColumns="False" CellPadding="6"
                    ForeColor="#333333" Width="800px" Font-Size="10px" BorderColor="Lavender" BorderWidth="1px"
                    Font-Names="Arial" Font-Overline="False" ShowFooter="True" ShowHeaderWhenEmpty="True">
                    <FooterStyle BackColor="Silver" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                    <RowStyle BackColor="#F2F2F2" ForeColor="Black" />
                    <Columns>
                        <asp:BoundField HeaderText="Folio Sigfe" DataField="folio_Sigfe" />
                        <asp:TemplateField HeaderText="Correlativo Documento">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCorrelativoDocumento" runat="server" Width="100px"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="FilteredtxtCorrelativoDocumento" runat="server"
                                ValidChars="0123456789" TargetControlID="txtCorrelativoDocumento">
                            </cc1:FilteredTextBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <HeaderStyle BackColor="Silver" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                        Font-Size="12px" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                        VerticalAlign="Middle" />
                    <AlternatingRowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                    <PagerSettings FirstPageText="Principio" LastPageText="Final" NextPageText="Siguiente"
                        PreviousPageText="Anterior" />
                    <RowStyle Wrap="True" HorizontalAlign="Center" />
                </asp:GridView>
            </div>--%>
                <div align="center">
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnGenerarXml" Height="30px" Text="Generar" OnClick="btnGenerarXml_Click" />
                    </td>
                    <td>
                    </td>
                </tr>
            </div>
            <br />
            <br />
            <div align="center">
                <tr>
                    <td>
                        <asp:Button ID="btnVer" runat="server" Text="Ver errores" Visible="false" Height="30px"
                            OnClick="btnVer_Click" />
                    </td>
                </tr>
            </div>
            <br/>
            <div align="center">
                <asp:GridView ID="gdvArchivos" runat="server" AutoGenerateColumns="False" CellPadding="6"
                    ForeColor="#333333" Width="500px" Font-Size="10px" BorderColor="Lavender" BorderWidth="1px"
                    Font-Names="Arial" Font-Overline="False" ShowFooter="True" ShowHeaderWhenEmpty="True" 
                    onrowcommand="gdvArchivos_RowCommand">
                    <FooterStyle BackColor="Silver" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                    <RowStyle BackColor="#F2F2F2" ForeColor="Black" />
                    <Columns>
                        <asp:ButtonField CommandName="Descargar" HeaderText="Descargar" Text="&lt;img src=&quot;../images/downloads.png&quot; width=&quot;14px&quot; height=&quot;14px&quot;&gt;" />
                        
                        <asp:BoundField HeaderText="Nombre Archivo" DataField="Nombre" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <HeaderStyle BackColor="Silver" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                        Font-Size="12px" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                        VerticalAlign="Middle" />
                    <AlternatingRowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                    <PagerSettings FirstPageText="Principio" LastPageText="Final" NextPageText="Siguiente"
                        PreviousPageText="Anterior" />
                    <RowStyle Wrap="True" HorizontalAlign="Center" />
                </asp:GridView>
            </div>
           
            <br />
            <asp:Panel ID="pnlMensaje" runat="server" CssClass="puPanel" BorderStyle="Solid"
                Width="350px">
                <table border="0" cellpadding="0" cellspacing="0" class="tblMsje">
                    <tr>
                        <th style="height: 21px">
                            <asp:Label ID="lblTituloMensaje" runat="server"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td style="background-color: White; width: 250px">
                            <asp:Label ID="lblMensaje" runat="server" Width="345px" Font-Bold="True" Text=""
                                Font-Size="9pt" BackColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Button ID="BtnPAceptar" runat="server" Text="Aceptar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="pMensaje" runat="server" BackgroundCssClass="SegundoPlano"
                OkControlID="BtnPAceptar" PopupControlID="pnlMensaje" TargetControlID="Ocultomensaje"
                Enabled="True" DynamicServicePath="">
            </cc1:ModalPopupExtender>
            <asp:LinkButton ID="Ocultomensaje" runat="server" Style="display: none"></asp:LinkButton>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnVer" />
             <asp:PostBackTrigger ControlID="gdvArchivos" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
