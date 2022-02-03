<%@ Page Language="C#" AutoEventWireup="true" Title="Login" CodeBehind="Login.aspx.cs" Inherits="SigFeXML.Login"
    MasterPageFile="~/Login.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="upPrincipal" runat="server">
        <ContentTemplate>
            <div align="center">
                <br />
                <asp:Label ID="lblError" runat="server" CssClass="lblError" Visible="False"></asp:Label>
                <br />
                <br />
                <table border="0" cellpadding="0" cellspacing="0" class="tblLogin" style="height: 184px;
                    width: 315px">
                    <tr>
                        <th colspan="4" class="division">
                            &nbsp;
                        </th>
                    </tr>
                    <tr height="30px">
                        <td colspan="4" align="center" style="background-color: #CE1B1B;">
                            <asp:Label ID="lblControlAcceso" runat="server" CssClass="titulo" meta:resourcekey="lblControlAcceso">Control Acceso</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="4" class="division">
                            &nbsp;
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="imgFlecha" runat="server" ImageUrl="images/flecha_01.jpg" />
                        </td>
                        <td class="CasillaCampoNormal">
                            <asp:Label ID="lblUsuario" runat="server" meta:resourcekey="lblUsuario" CssClass="label_Arial_12px_Bold">Usuario:</asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtUser" runat="server" Width="100px" SkinID="TextBoxNormal"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="division">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="images/flecha_01.jpg" />
                        </td>
                        <td class="CasillaCampoNormal">
                            <asp:Label ID="lblContrasena" runat="server" meta:resourcekey="lblContrasena" CssClass="label_Arial_12px_Bold">Contraseña</asp:Label>
                            &nbsp;&nbsp;
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" Width="100px" SkinID="TextBoxNormal"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 13px" class="division">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 13px">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnIngresar"  runat="server" 
                            Height="30px" Width="100px" Text="Ingresar" OnClick="btnIngresar_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <asp:Label ID="lblVersion" runat="server" CssClass="lblFooter" meta:resourcekey="lblVersion">Version 3.0</asp:Label>
                <%--PANEL MENSAJE--%>
                <asp:Panel ID="pnlMensaje" runat="server" CssClass="puPanel" BorderStyle="Solid"
                    Width="350px">
                    <table border="0" cellpadding="0" cellspacing="0" class="tblMsje">
                        <tr>
                            <th style="height: 21px">
                                <asp:Label ID="lblTituloMensaje" runat="server">Error</asp:Label>
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
                                <asp:Button ID="BtnPAceptar" runat="server" Text="Aceptar" OnClick="BtnPAceptar_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <cc1:ModalPopupExtender ID="PopupMensaje" runat="server" BackgroundCssClass="SegundoPlano"
                    OkControlID="lblMensaje" PopupControlID="pnlMensaje" PopupDragHandleControlID="pnlMensaje"
                    TargetControlID="Ocultomensaje" Enabled="True" DynamicServicePath="">
                </cc1:ModalPopupExtender>
                <asp:LinkButton ID="Ocultomensaje" runat="server" Style="display: none" meta:resourcekey="lbnOcultoMensajeResource1"></asp:LinkButton>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
