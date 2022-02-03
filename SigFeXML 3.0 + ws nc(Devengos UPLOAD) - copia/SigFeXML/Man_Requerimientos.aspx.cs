using System;
using System.Web.UI.WebControls;
using XML_DATOS;
using XML_Negocio;
using System.Data;
using System.Text;


namespace SigFeXML
{
    public partial class Man_Requerimientos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {               
                CargarGrilla();
                CargarNivel1();
                CargarNivel2();                
            }
        }


        private void CargarNivel1()
        {
            ddlDepto.Items.Clear();
            NegociOlecturaXml bsn = new NegociOlecturaXml();
            DataSet ds = new DataSet();

            ds = bsn.CargarNivel1();
            
            ddlDepto.DataTextField = "n1_glosa";
            ddlDepto.DataValueField = "n1_id1";
            ddlDepto.DataSource = ds.Tables[0];
            ddlDepto.DataBind();
            ddlDepto.Items.Insert(0, new ListItem("[Seleccione Depto..]", "-1"));
            ddlDepto.SelectedValue = "-1";
        }

        private void CargarNivel2()
        {
            ddlSeccion.Items.Clear();
            NegociOlecturaXml bsn = new NegociOlecturaXml();
            DataSet ds = new DataSet();

            ds = bsn.CargarNivel2();

            //ds.Tables[0].Rows.Add(new object[] { -1, "[Seleccione Seccion..]" });
            ddlSeccion.DataTextField = "n2_glosa";
            //ddlSeccion.DataTextField = "n2_id2";
            ddlSeccion.DataValueField = "n2_id2";
            ddlSeccion.DataSource = ds.Tables[0];
            ddlSeccion.DataBind();
            ddlSeccion.Items.Insert(0, new ListItem("[Seleccione Sección..]", "-1"));
            ddlSeccion.SelectedValue = "-1";
        }

        protected void lbnGuardar_Click(object sender, EventArgs e)
        {
            //validar los datos
            if (validaDatosIngreso())
            {
                try
                {
                    DtoManRequerimiento dto = new DtoManRequerimiento();
                    NegociOlecturaXml bsn = new NegociOlecturaXml();
                    DataSet ds = new DataSet();

                    dto.Departamento = ddlDepto.SelectedValue;
                    dto.Seccion = ddlSeccion.SelectedValue;
                    dto.Descripcion = txtDescripcion.Text;
                    dto.Folio = txtFolio.Text;
                    dto.Concepto = ddlConcepto.SelectedItem.Text;
                    dto.Grupo = rbnGrupo.SelectedItem.Text;
                    dto.Agrupacion = txtAgrupacion.Text;
                    dto.Año = txtAnho.Text;

                    //revisar que no registrado el requerimiento
                    ds = bsn.BuscarIdRequerimiento("1", dto.Departamento, dto.Seccion, dto.Grupo, dto.Año);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        lblTituloMensaje.Text = "ERROR";
                        lblMensaje.Text = "El requerimiento ya se encuentra registrado";
                        pMensaje.Show();
                        pGuardar.Show();
                    }//si no esta el requerimiento se guarda
                    else
                    {
                        bsn.GuardarRequerimiento(dto);
                        lblTituloMensaje.Text = "OK";
                        lblMensaje.Text = "El requerimiento se creo correctamente";
                        pMensaje.Show();

                        //limpiar campos
                        ddlDepto.SelectedValue = "-1";
                        ddlSeccion.SelectedValue = "-1";
                        txtDescripcion.Text = "";
                        txtFolio.Text = "";
                        ddlConcepto.SelectedValue = "-1";
                        rbnGrupo.ClearSelection();
                        txtAgrupacion.Text = "";
                        txtAnho.Text = "";
                    }
                }
                catch (Exception ex)
                { 
                
                }
            }
            else
            {
                pGuardar.Show();
            }
        }

        protected void lbnEditar_Click(object sender, EventArgs e)
        {
            if (validaDatosIngreso())
            {
                DtoManRequerimiento dto = new DtoManRequerimiento();
                NegociOlecturaXml bsn = new NegociOlecturaXml();
                DataSet ds = new DataSet();

                try
                {

                    dto.Departamento = ddlDepto.SelectedValue;
                    dto.Seccion = ddlSeccion.SelectedValue;
                    dto.Descripcion = txtDescripcion.Text;
                    dto.Folio = txtFolio.Text;
                    dto.Concepto = ddlConcepto.SelectedItem.Text;
                    dto.Grupo = rbnGrupo.SelectedItem.Text;
                    dto.Agrupacion = txtAgrupacion.Text;
                    dto.Id = (string)Session["Id_Req"];
                    dto.Año = txtAnho.Text;

                    bsn.EditarRequerimiento(dto);
                    lblTituloMensaje.Text = "OK";
                    lblMensaje.Text = "El requerimiento fue actualizado correctamente";
                    pMensaje.Show();

                    //limpiar campos
                    ddlDepto.SelectedValue = "-1";
                    ddlSeccion.SelectedValue = "-1";
                    txtDescripcion.Text = "";
                    txtFolio.Text = "";
                    ddlConcepto.SelectedValue = "-1";
                    rbnGrupo.ClearSelection();
                    txtAgrupacion.Text = "";
                    txtAnho.Text = "";
                }
                catch(Exception ex)
                {
                
                }
            }
            else
            {
                pGuardar.Show();
            }
        }

        protected void lbnCancelar_Click(object sender, EventArgs e)
        {
            pGuardar.Hide();
        }

        protected void lbnExportar_Click(object sender, EventArgs e)
        {           

            gdvRequerimiento.Columns[7].Visible = false; 
            gdvRequerimiento.AllowPaging = false;

            if (Session["_DsBusqueda"] != null)
            {
                DataSet DS = (DataSet)Session["_DsBusqueda"]; 
                string nombre = "Man_Requerimiento_" + DateTime.Now.ToShortDateString();
                    DataTable2Excel(DS.Tables[0], Response, nombre);
                
            }
            else
            {
                lblTituloMensaje.Text = "ERROR";
                lblMensaje.Text = "No hay datos que exportar";
                pMensaje.Show();
            }
           
            gdvRequerimiento.Columns[7].Visible = true;
            gdvRequerimiento.AllowPaging = true;
            
        }      

        protected void gdvRequerimiento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                lblTituloGuardar.Text = "Editar";
                 int rowIndex = Convert.ToInt32(e.CommandArgument);
                 string Id = gdvRequerimiento.DataKeys[rowIndex].Value.ToString();
                Session.Add("Id_Req",Id);
                lbnGuardar.Visible = false;
                lbnEditar.Visible = true;

                ddlDepto.SelectedValue = gdvRequerimiento.Rows[rowIndex].Cells[0].Text.Trim();
                ddlDepto.Enabled = false;
                ddlSeccion.SelectedValue = gdvRequerimiento.Rows[rowIndex].Cells[1].Text.Trim();
                ddlSeccion.Enabled = false;
                txtDescripcion.Text = gdvRequerimiento.Rows[rowIndex].Cells[2].Text.Trim();
                txtFolio.Text = gdvRequerimiento.Rows[rowIndex].Cells[3].Text.Trim();

                string Concepto = gdvRequerimiento.Rows[rowIndex].Cells[4].Text.Trim();
                if (Concepto == "0701")
                {
                    ddlConcepto.SelectedValue = "1";
                }
                else if (Concepto == "0702")
                {
                    ddlConcepto.SelectedValue = "2";
                }
                else if (Concepto == "0802")
                {
                    ddlConcepto.SelectedValue = "3";
                }
                else if(Concepto=="0899999")
                {
                ddlConcepto.SelectedValue="4";
                }

                try
                {
                    rbnGrupo.SelectedValue = rbnGrupo.Items.FindByText(gdvRequerimiento.Rows[rowIndex].Cells[5].Text.Trim()).Value;
                }
                catch(Exception)
                {
                
                }
                txtAgrupacion.Text = gdvRequerimiento.Rows[rowIndex].Cells[6].Text.Trim();
                txtAnho.Text = gdvRequerimiento.Rows[rowIndex].Cells[7].Text.Trim();
                txtAnho.Enabled = false;

                pGuardar.Show();
            }
            else if(e.CommandName =="Eliminar")
            {
                NegociOlecturaXml bsn = new NegociOlecturaXml();
                int Id = Convert.ToInt32(e.CommandArgument);
                bsn.EliminarReq(Id);

                CargarGrilla();

                lblTituloMensaje.Text = "OK";
                lblMensaje.Text = "Se elimino correctamente el requerimiento";
                pMensaje.Show();
            }
        }

        protected void lbnAgregar_Click(object sender, EventArgs e)
        {
            ddlDepto.Enabled = true;
            ddlSeccion.Enabled = true;
            txtAnho.Enabled = true;
            ddlDepto.SelectedValue = "-1";
            ddlSeccion.SelectedValue = "-1";
            txtDescripcion.Text = "";
            txtFolio.Text = "";
            ddlConcepto.SelectedValue = "-1";
            rbnGrupo.ClearSelection();
            txtAgrupacion.Text = "";
            txtAnho.Text = "";
            lblTituloGuardar.Text = "Guardar";
            lbnEditar.Visible = false;
            lbnGuardar.Visible = true;
            pGuardar.Show();
        }

        protected void gdvRequerimiento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvRequerimiento.PageIndex = e.NewPageIndex;
            gdvRequerimiento.DataSource = (DataSet)Session["_DsBusqueda"];
            gdvRequerimiento.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DtoManRequerimiento dto = new DtoManRequerimiento();
            NegociOlecturaXml bsn = new NegociOlecturaXml();

            DataSet ds = new DataSet();          


            if (txtDepartamentob.Text!= string.Empty)
                dto.Departamento = txtDepartamentob.Text;

            if (txtSeccionb.Text!= string.Empty)
                dto.Seccion = txtSeccionb.Text;           

            if(txtFoliob.Text!=string.Empty)
            dto.Folio = txtFoliob.Text;

            if(ddlConceptob.SelectedValue!="-1")
            dto.Concepto = ddlConceptob.SelectedItem.Text;

            if (rbnGrupob.SelectedValue != string.Empty)
                dto.Grupo = rbnGrupob.SelectedItem.Text;

            if(txtAgrupacionb.Text!=string.Empty)
            dto.Agrupacion = txtAgrupacionb.Text;

            //if (txtAnho.Text != string.Empty)
            //    dto.Año = txtAnho.Text;

            ds = bsn.BuscarFiltros(dto);
            
            Session.Add("_DsBusqueda", ds);

            gdvRequerimiento.DataSource = ds;
            gdvRequerimiento.DataBind();  

        }

        private void CargarGrilla()
        {
            DtoManRequerimiento dto = new DtoManRequerimiento();
            NegociOlecturaXml bsn = new NegociOlecturaXml();
            DataSet ds = new DataSet();

            ds = bsn.BuscarFiltros(dto);
            Session.Add("_DsBusqueda", ds);

            gdvRequerimiento.DataSource = ds;
            gdvRequerimiento.DataBind();  
        }

        #region Exportar
        public static void DataTable2Excel(DataTable dt, System.Web.HttpResponse RS, string nombre)
        {
            StringBuilder sbTop = new StringBuilder();
            sbTop.Append("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\"	xmlns:x=\"urn:schemas-microsoft-com:office:excel\" ");
            sbTop.Append("xmlns=\"http://www.w3.org/TR/REC-html40\"><head><meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">");
            sbTop.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content=\"Microsoft Excel 9\"><!--[if gte mso 9]>");
            sbTop.Append("<xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>REQUERIMIENTOS</x:Name><x:WorksheetOptions>");
            sbTop.Append("<x:Selected/><x:ProtectContents>False</x:ProtectContents><x:ProtectObjects>False</x:ProtectObjects>");
            sbTop.Append("<x:ProtectScenarios>False</x:ProtectScenarios></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets>");
            sbTop.Append("<x:ProtectStructure>False</x:ProtectStructure><x:ProtectWindows>False</x:ProtectWindows></x:ExcelWorkbook></xml>");
            sbTop.Append("<![endif]--></head><body><table>");
            string bottom = "</table></body></html>";

            StringBuilder sb = new StringBuilder();
            //Header
            int column = dt.Columns.Count - 2;
            //string header = @"<tr><td height = ""80"" colspan = ""2"" style=""background-color:#BDBDBD;"">";           
            //header += "</td><td colspan = " + @"" + column.ToString() + @""" style=""background-color:#BDBDBD;""><a runat=""server"">Sigfe XML</a>";
            //header += @"<a runat=""server""> " + DateTime.Now.Date.ToLongDateString() + @"<a></td></tr>";

            //sb.Append(header);

            sb = PasarDt2StringBuilder(dt, sb);
            string SSxml = sbTop.ToString() + sb.ToString() + bottom;

            RS.AppendHeader("Content-Type", "application/vnd.ms-excel");
            RS.AppendHeader("content-disposition", "attachment; filename= " + nombre + ".xls");
            RS.ContentEncoding = System.Text.Encoding.Default;
            RS.Charset = "UTF-8";
            RS.Write(SSxml);
        }


        private static StringBuilder PasarDt2StringBuilder(DataTable dt, StringBuilder sb)
        {
            sb.Append("<tr>");
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    if (i != 3)
            //    {
            //        sb.Append(@"<td style=""background-color:SteelBlue;""><a style=""color:White;"" runat=""server"">" + dt.Columns[i].ColumnName + "</a></td>");
            //    }
            //}
            sb.Append(@"<td style=""background-color:SteelBlue;""><a style=""color:White;"" runat=""server"">Departamento</a></td>");
            sb.Append(@"<td style=""background-color:SteelBlue;""><a style=""color:White;"" runat=""server"">Sección</a></td>");
            sb.Append(@"<td style=""background-color:SteelBlue;""><a style=""color:White;"" runat=""server"">Descripción</a></td>");
            sb.Append(@"<td style=""background-color:SteelBlue;""><a style=""color:White;"" runat=""server"">Folio</a></td>");
            sb.Append(@"<td style=""background-color:SteelBlue;""><a style=""color:White;"" runat=""server"">Código Concepto</a></td>");
            sb.Append(@"<td style=""background-color:SteelBlue;""><a style=""color:White;"" runat=""server"">Grupo</a></td>");
            sb.Append(@"<td style=""background-color:SteelBlue;""><a style=""color:White;"" runat=""server"">Agrupación</a></td>");
            sb.Append("</tr>");

            //Items
            for (int x = 0; x < dt.Rows.Count; x++)
            {
                if (x != 3)
                {
                    sb.Append("<tr>");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i != 3)
                        {
                            sb.Append(@"<td style=""background-color:AliceBlue;"">" + dt.Rows[x][i] + "</td>");
                        }

                    }
                    sb.Append("</tr>");
                }
            }
            sb.Append("<tr><td colspan = '" + dt.Columns.Count + "'></td></tr>");
            return sb;
        }
        #endregion

        #region Validaciones

        public bool validaDatosIngreso()
        {
            bool esValido = true;
            string codError = null;

            if (ddlDepto.SelectedValue=="-1")
            {
                esValido = false;
                ddlDepto.Focus();
                codError = "err_Cod";
            }
            else if (ddlSeccion.SelectedValue=="-1")
            {
                esValido = false;
                ddlSeccion.Focus();
                codError = "err_Cod2";
            }
            else if (txtDescripcion.Text.Length == 0)
            {
                esValido = false;
                txtDescripcion.Focus();
                codError = "err_Des";
            }
            else if (txtFolio.Text.Length == 0)
            {
                esValido = false;
                txtFolio.Focus();
                codError = "err_Fol";
            }
            else if (ddlConcepto.SelectedValue == "-1")
            {
                esValido = false;
                ddlConcepto.Focus();
                codError = "err_Con";
            }
            else if (rbnGrupo.SelectedValue == string.Empty)
            {
                
                esValido = false;
                rbnGrupo.Focus();
                codError = "err_Grupo";
            }
            else if (txtAgrupacion.Text.Length == 0)
            {
                esValido = false;
                txtAgrupacion.Focus();
                codError = "err_Agrup";
            }
            else if (txtAnho.Text.Length == 0)
            {
                esValido = false;
                txtAnho.Focus();
                codError = "err_Año";
            }

            InterpretarError(codError, "");
            return esValido;
        }

        private void InterpretarError(string codError, string msjeEx)
        {
            string msjeError = null;

            switch (codError)
            {
                case "err_Cod":
                    msjeError = "Debe ingresar Departamento.";
                    break;

                case "err_Cod2":
                    msjeError = "Debe ingresar Sección.";
                    break;

                case "err_Des":
                    msjeError = "Debe ingresar Descripción.";
                    break;

                case "err_Fol":
                    msjeError = "Debe ingresar Folio.";
                    break;

                case "err_Con":
                    msjeError = "Debe ingresar Concepto.";
                    break;

                case "err_Grupo":
                    msjeError = "Debe ingresar Grupo.";
                    break;

                case "err_Agrup":
                    msjeError = "Debe ingresar Agrupación.";
                    break;

                case "err_Año":
                    msjeError = "Debe ingresar Año.";
                    break;
            }

            if (msjeError != null)
            {
                pMensaje.Show();
                lblTituloMensaje.Text = "ERROR";
                lblMensaje.Text = msjeError;
            }
        }

        #endregion

        protected void ddlDepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSeccion.Items.Clear();
            NegociOlecturaXml bsn = new NegociOlecturaXml();
            DataSet ds = new DataSet();

            ds = bsn.CargarNivel2Filtro(int.Parse(ddlDepto.SelectedValue));

            //ds.Tables[0].Rows.Add(new object[] { -1, "[Seleccione Sección..]" });
            ddlSeccion.DataTextField = "n2_glosa";
            ddlSeccion.DataValueField = "n2_id2";
            ddlSeccion.DataSource = ds.Tables[0];
            ddlSeccion.DataBind();
            ddlSeccion.Items.Insert(0, new ListItem("[Seleccione Sección..]", "-1"));
            ddlSeccion.SelectedValue = "-1";

            pGuardar.Show();
        }

        protected void ddlSeccion_SelectedIndexChanged(object sender, EventArgs e)
        {           
            string Valor = ddlSeccion.SelectedItem.Text;
            Valor= Valor.Replace("._","");
            Valor = Valor.Replace("1", "");
            Valor = Valor.Replace("2", "");
            Valor = Valor.Replace("3", "");
            Valor = Valor.Replace("4", "");
            Valor = Valor.Replace("5", "");
            Valor = Valor.Replace("6", "");
            Valor = Valor.Replace("7", "");
            Valor = Valor.Replace("8", "");
            Valor = Valor.Replace("9", "");
            Valor = Valor.Replace("0", "");
            Valor = Valor.Trim();
            txtDescripcion.Text = Valor;            
           
            pGuardar.Show();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtDepartamentob.Text = "";
            txtSeccionb.Text = "";
            txtFoliob.Text = "";
            ddlConceptob.SelectedValue = "-1";
            rbnGrupob.ClearSelection();
            txtAgrupacionb.Text = "";
        }

      
    }
}