<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="RegistroTransacciones._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="udpanelMain" runat="server">
        <ContentTemplate>
             <div id="DivMensajeOk" runat="server" visible="false">
                <tr>
                    <td>
                        <div style="border: thin groove #FFFFFF; background-color: #CCFFCC">
                            <img src="images/Select.png" />
                            <asp:Label ID="lnlMensajeOk" runat="server" Font-Bold="True" Font-Size="Small">Proceso finalizado, consulte el log para más detalles</asp:Label>
                        </div>
                    </td>
                </tr>
            </div>
            <tr>
                <td>
                    <asp:Label ID="lblMensajeError" runat="server" Visible="False" Font-Bold="True" ForeColor="Red" Height="40px"></asp:Label>
                </td>
            </tr>
            <div align="left">
                <div style="position: relative; width: 750px;">
                    <div align="left">
                        <b>Seleccione metodo de proceso:</b>
                    </div>
                    <br />
                    <div id="divfechas" runat="server">
                        <div style="position: relative; float: left; width: 750px;">
                            <asp:RadioButtonList runat="server" ID="rbnselccion" RepeatDirection="Horizontal"
                                AutoPostBack="True" 
                                onselectedindexchanged="rbnselccion_SelectedIndexChanged">
                                <asp:ListItem Value="0" Text="Fecha" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Folio"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div style="position: relative; float: left; width: 750px;">
                            <div style="position: relative; float: left; width: 300px;">
                                <div align="left" style="position: relative; width: 120px; float: left">
                                    Ejercicio
                                </div>
                                <div align="left" style="position: relative; width: 180px; float: left">
                                    <asp:TextBox runat="server" ID="txtEjercicio" Width="80px" MaxLength="4"></asp:TextBox>
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
                            <div style="position: relative; float: left; width: 300px;" runat="server" id="divcaja">
                                <div align="left" style="position: relative; width: 120px; float: left">
                                    Caja
                                </div>
                                <div align="left" style="position: relative; width: 180px; float: left">
                                    <asp:DropDownList runat="server" ID="ddlCaja" Style="width: 125px;">
                                        <asp:ListItem Value="0" Text="Seleccione Caja"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Central"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Auxiliar"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Facturas Web"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div style="position: relative; float: left; width: 300px;" runat="server" ID="divfecha">
                                <div align="left" style="position: relative; width: 120px; float: left">
                                    Fecha
                                </div>
                                <div align="left" style="position: relative; width: 180px; float: left">
                                    <span>
                                        <asp:TextBox runat="server" ID="txtFecha" Width="70px" AutoPostBack="True" MaxLength="10" 
                                        ontextchanged="txtFecha_TextChanged"></asp:TextBox>
                                      <%--  <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" ValidChars="0123456789-"
                                            TargetControlID="txtFecha">
                                        </cc1:FilteredTextBoxExtender>--%>
                                      <%--  <cc1:MaskedEditExtender TargetControlID="txtFecha" Mask="99/99/9999" MaskType="Date"
                                         ID="Maskededitextender2" OnFocusCssClass="MaskedEditFocus" runat="server" />--%>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                            Width="16px" />
                                    </span>
                                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtFecha"
                                        PopupButtonID="Image1"  Format="dd-MM-yyyy"
                                        PopupPosition="TopRight">
                                    </cc1:CalendarExtender>
                                </div>
                            </div>
                            <div style="position: relative; float: left; width: 300px;" id="divfolio" runat="server" Visible="False">
                                <div align="left" style="position: relative; width: 120px; float: left">
                                    Tipo Documento
                                </div>
                                <div align="left" style="position: relative; width: 180px; float: left">
                                       <span>
                                        <asp:DropDownList runat="server" ID="ddlTipoDoc">
                                            <asp:ListItem Value="0" Text="Seleccione Tipo Doc" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="E" Text="Factura Exenta"></asp:ListItem>
                                            <asp:ListItem Value="F" Text="Factura Afecta"></asp:ListItem>
                                            <asp:ListItem Value="CRP" Text="Boleta"></asp:ListItem>
                                        </asp:DropDownList>
                                        
                                    </span>
                                </div>
                            </div>
                            <div style="position: relative; float: left; width: 300px;" id="divTipoDoc" runat="server" Visible="False">
                                <div align="left" style="position: relative; width: 120px; float: left">
                                    Folio
                                </div>
                                <div align="left" style="position: relative; width: 180px; float: left">
                                
                                     <span>
                                        <asp:TextBox runat="server" ID="txtFolio" Height="100px" Width="300px" TextMode="MultiLine" placeholder="Separe con una coma si ingresa más de un folio"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" ValidChars="0123456789," 
                                            TargetControlID="txtFolio">
                                        </cc1:FilteredTextBoxExtender>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="position: relative; float: left; width: 920px; height: 30px;" runat="server">
            </div>
            <div style="position: relative; float: left; width: 920px;">
                <div style="position: relative; float: left; width: 250px; height: 40px;" align="center">
                    <asp:Button ID="btnPagar" runat="server" Text="Generar Cobro" Height="30px" Width="120px"
                        OnClick="btnPagar_Click" />
                </div>
               <%-- <div id="Div2" style="position: relative; float: left; width: 50px; top: 0px; left: 0px;
                    height: 17px; height: 40px;" align="right" runat="server">
                </div>--%>
                <div style="position: relative; float: left; width: 250px; height: 40px;" align="center">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar Ticket" 
                        Height="30px" Width="120px" onclick="btnConsultar_Click"
                       />
                </div>
                  <div style="position: relative; float: left; width: 250px; height: 40px;" align="center">
                      <asp:Button ID="btnVer" runat="server" Text="Descargar Log" Height="30px" 
                          Width="120px" onclick="btnVer_Click"
                         />
                </div>
                 <div style="position: relative; float: left; width: 120px; height: 40px;" align="center">
                      <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" Height="30px" Width="70px"
                        OnClick="btnLimpiar_Click" />
                 
                </div>
            </div>
            <br />
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="btnVer" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
