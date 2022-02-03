<%@ Page Title="Requerimientos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Man_Requerimientos.aspx.cs" Inherits="SigFeXML.Man_Requerimientos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="udpanelMain" runat="server">
        <ContentTemplate>
            <div align="center">
                <table border="0" cellpadding="0" cellspacing="0" class="tblPrincipal">
                    <tr>
                        <th class="tituloPrincipal">
                            <asp:Label ID="lblTitulo" runat="server" meta:resourcekey="lblTitulo">MANTENEDOR REQUERIMIENTO</asp:Label>
                        </th>
                    </tr>
                    <%--PANEL FILTRO DE BÚSQUEDA--%>
                    <tr>
                        <td>
                            <cc1:CollapsiblePanelExtender ID="panFiltroBusqueda_CollapsiblePanelExtender" runat="server"
                                TargetControlID="panFiltroBusqueda" Collapsed="true" CollapseControlID="lblFiltroBusqueda"
                                ExpandControlID="lblFiltroBusqueda" CollapsedSize="26" TextLabelID="lblFiltroBusqueda"
                                ImageControlID="imgFiltroBusqueda" ExpandDirection="Vertical" Enabled="True"
                                CollapsedText="Hacer click aqui para abrir filtro de Busqueda..." ExpandedText="Hacer click aqui para cerrar filtro de Busqueda..."
                                SuppressPostBack="True">
                            </cc1:CollapsiblePanelExtender>
                            <asp:Panel ID="panFiltroBusqueda" runat="server" Width="900px" CssClass="pnlFiltro">
                                <table border="0" cellpadding="4" cellspacing="0" class="Table_6_col_filtro">
                                    <tr>
                                        <td colspan="6" class="Filtro_Busqueda">
                                            <asp:Label ID="lblFiltroBusqueda" runat="server" Style="cursor: pointer;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" CssClass="label_Arial_12px">Departamento:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDepartamentob" runat="server" MaxLength="2" CssClass="TextBox_Arial_12px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" CssClass="label_Arial_12px">Sección:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSeccionb" runat="server" MaxLength="2" CssClass="TextBox_Arial_12px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" CssClass="label_Arial_12px">Folio:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFoliob" runat="server" MaxLength="5" CssClass="TextBox_Arial_12px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" CssClass="label_Arial_12px">Cod. Concepto:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlConceptob" runat="server">
                                                <asp:ListItem Selected="True" Value="-1" Text="Seleccione Concepto"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="0701"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="0702"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="0802"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="0899999"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" CssClass="label_Arial_12px">Grupo:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rbnGrupob" runat="server" RepeatDirection="Horizontal" CssClass="label_Arial_12px">
                                                <asp:ListItem Value="1" Text="B"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="S"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="O"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <%-- <asp:TextBox ID="txtGrupob" runat="server" MaxLength="1" CssClass="TextBox_Arial_12px"></asp:TextBox>--%>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" CssClass="label_Arial_12px">Cod. Agrupación:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgrupacionb" runat="server" MaxLength="10" CssClass="TextBox_Arial_12px"></asp:TextBox>
                                        </td>
                                    </tr>
                                  <%--  <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" CssClass="label_Arial_12px">Año:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAnhoB" runat="server" MaxLength="4" CssClass="TextBox_Arial_12px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender runat="server" ID="filteranho" TargetControlID="txtAnhoB" ValidChars="0123456789"/>
                                        </td>
                                    </tr>--%>
                                    <tr style="height: 35px">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Height="30px" OnClick="btnBuscar_Click" />
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" Height="30px" 
                                                onclick="btnLimpiar_Click" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table style="width: 900px;">
                    <tr>
                        <td align="center" colspan="6">
                            <asp:GridView ID="gdvRequerimiento" runat="server" AutoGenerateColumns="False" CellPadding="6"
                                ForeColor="#333333" Width="500px" Font-Size="10px" BorderColor="Lavender" BorderWidth="1px"
                                Font-Names="Arial" Font-Overline="False" ShowHeaderWhenEmpty="True" AllowPaging="True"
                                PagerSettings-Mode="Numeric" OnRowCommand="gdvRequerimiento_RowCommand" OnPageIndexChanging="gdvRequerimiento_PageIndexChanging"
                                DataKeyNames="n2_serial">
                                <%-- <FooterStyle BackColor="Silver" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" ForeColor="White" />--%>
                                <RowStyle BackColor="#F2F2F2" ForeColor="Black" />
                                <AlternatingRowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField HeaderText="Departamento" DataField="n2_id1" />
                                    <asp:BoundField HeaderText="Sección" DataField="n2_id2" />
                                    <asp:BoundField HeaderText="Descripción" DataField="n2_glosa" />
                                    <asp:BoundField HeaderText="Folio" DataField="idreq" />
                                    <asp:BoundField HeaderText="Código Concepto" DataField="idcon" />
                                    <asp:BoundField HeaderText="Grupo" DataField="pf_grupo" />
                                    <asp:BoundField HeaderText="Cod. Agrupación" DataField="idagrupacion" />
                                    <asp:BoundField DataField="anho" HeaderText="Año" />
                                    <asp:ButtonField HeaderText="Editar" CommandName="Editar" Text="&lt;img src='images/btn_editar.gif' border=0/&gt;" />
                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="20px" HeaderText="Eliminar">
                                        <ItemTemplate>
                                            <center>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Eliminar"
                                                    CommandArgument='<%# Eval("n2_serial")%>' Width="16px" ImageUrl="images/btn_delete.gif"
                                                    Text="Eliminar" OnClientClick="return confirm('Esta seguro que desea eliminar el registro?');" />
                                            </center>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <HeaderStyle BackColor="Silver" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Size="12px" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    VerticalAlign="Middle" />
                                <PagerSettings FirstPageText="Principio" LastPageText="Final" NextPageText="Siguiente"
                                    PreviousPageText="Anterior" />
                                <PagerStyle BackColor="Silver" ForeColor="Black" HorizontalAlign="Center" BorderColor="Lavender" />
                                <RowStyle Wrap="True" HorizontalAlign="Center" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <table border="0" cellpadding="8" cellspacing="0" class="Table_980_center">
                        <tr>
                            <td style="width: 147px">
                                <asp:Button ID="lbnAgregar" runat="server" Text="Agregar Nuevo" Height="30px" OnClick="lbnAgregar_Click" />
                            </td>
                            <td style="width: 84px">
                                <asp:Button ID="lbnExportar" runat="server" Text="Exportar" Height="30px" OnClick="lbnExportar_Click" />
                            </td>
                        </tr>
                    </table>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                        <td align="center" colspan="6">
                            <asp:Panel ID="pnlGuardar" runat="server" BackColor="ControlLight" CssClass="puPanel"
                                Width="300px">
                                <table class="Table_Mensaje">
                                    <tr>
                                        <td class="puPanelTituloMensaje" colspan="4">
                                            <asp:Label ID="lblTituloGuardar" runat="server" CssClass="label_Arial_13px_Bold_Mayus"
                                                meta:resourcekey="lblTituloGuardar">GUARDAR</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="width: 456px">
                                            <asp:Label ID="lblCodigo" runat="server" CssClass="label_Arial_12px" meta:resourcekey="lblCodigoIns">Departamento:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDepto" runat="server" Width="120px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlDepto_SelectedIndexChanged" Font-Size="10px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="width: 456px">
                                            <asp:Label ID="lblCodigo2" runat="server" CssClass="label_Arial_12px">Sección:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSeccion" runat="server" Width="120px" AutoPostBack="true" Font-Size="10px"
                                                onselectedindexchanged="ddlSeccion_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="width: 456px">
                                            <asp:Label ID="lblDescripcion" runat="server" CssClass="label_Arial_12px" meta:resourcekey="lblDescripcionIns">Descripción:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="50" CssClass="TextBox_Arial_12px"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="width: 456px">
                                            <asp:Label ID="Label1" runat="server" CssClass="label_Arial_12px">Folio:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFolio" runat="server" MaxLength="5" CssClass="TextBox_Arial_12px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" ValidChars="0123456789"
                                                TargetControlID="txtFolio">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="width: 456px">
                                            <asp:Label ID="Label2" runat="server" CssClass="label_Arial_12px">Cod. Concepto:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlConcepto" runat="server" Width="120px" Font-Size="10px">
                                                <asp:ListItem Selected="True" Value="-1" Text="Seleccione Concepto"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="0701"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="0702"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="0802"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="0899999"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="width: 456px">
                                            <asp:Label ID="Label3" runat="server" CssClass="label_Arial_12px">Grupo</asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rbnGrupo" runat="server" RepeatDirection="Horizontal" CssClass="label_Arial_12px">
                                                <asp:ListItem Value="1" Text="B"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="S"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="O"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <%--<asp:TextBox ID="txtGrupo" runat="server" MaxLength="1" CssClass="TextBox_Arial_12px"></asp:TextBox>--%>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="width: 456px">
                                            <asp:Label ID="Label6" runat="server" CssClass="label_Arial_12px">Cod. Agrupación</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgrupacion" runat="server" MaxLength="10" CssClass="TextBox_Arial_12px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="filtro_agrupacion" runat="server" ValidChars="0123456789"
                                                TargetControlID="txtAgrupacion">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                      <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="width: 456px">
                                            <asp:Label ID="Label11" runat="server" CssClass="label_Arial_12px">Año</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAnho" runat="server" MaxLength="10" CssClass="TextBox_Arial_12px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" ValidChars="0123456789"
                                                TargetControlID="txtAnho">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="lbnGuardar" runat="server" Text="Guardar" Height="30px" OnClick="lbnGuardar_Click" />
                                            <asp:Button ID="lbnEditar" runat="server" Text="Editar" Height="30px" Visible="false"
                                                OnClick="lbnEditar_Click" />
                                        </td>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="lbnCancelar" runat="server" Text="Cancelar" Height="30px" OnClick="lbnCancelar_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <cc1:ModalPopupExtender ID="pGuardar" runat="server" BackgroundCssClass="SegundoPlano"
                                PopupControlID="pnlGuardar" PopupDragHandleControlID="pnlGuardar" TargetControlID="lbnOcultoMensaje2">
                            </cc1:ModalPopupExtender>
                            <asp:LinkButton ID="lbnOcultoMensaje2" runat="server" Style="display: none" />
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                        <td colspan="6">
                            &nbsp;
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                        <td colspan="6">
                            <asp:Panel ID="pnlMensaje" runat="server" CssClass="puPanel" Width="250px">
                                <table border="0" cellpadding="0" cellspacing="0" class="tblMsje">
                                    <tr>
                                        <th style="height: 21px">
                                            <asp:Label ID="lblTituloMensaje" runat="server"></asp:Label>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMensaje" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="lbnMensaje" runat="server" Height="30PX" Text="Aceptar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <cc1:ModalPopupExtender ID="pMensaje" runat="server" BackgroundCssClass="SegundoPlano"
                                OkControlID="lbnMensaje" PopupControlID="pnlMensaje" TargetControlID="lbnOcultoMensaje">
                            </cc1:ModalPopupExtender>
                            <asp:LinkButton ID="lbnOcultoMensaje" runat="server" Style="display: none"></asp:LinkButton>
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lbnExportar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
