using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using SigFeXML.cl.Devengo;
using XML_DATOS;
using XML_Negocio;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;

namespace SigFeXML
{
    
    public partial class XML : System.Web.UI.Page
    {
        static StreamWriter _sw;
        string _mensajeXlm = "";
      

        private readonly NegociOlecturaXml _negocio = new NegociOlecturaXml();
       
        
        protected void Page_Load(object sender, EventArgs e)
        {          
            if (Session["UsuarioLogueado"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            txtError.Visible = false;
            lblMensajeError.Visible = false;
           
            if (!IsPostBack)
            {
                txtEjercicio.Text = DateTime.Now.Year.ToString();

                ddlPeriodo.SelectedValue = string.Format("{0:MM}", DateTime.Now);

                txtEjercicio.Enabled = DateTime.Now.Month == 1;//solo en enero se pueden cargar documentos de años anteriores

                divBuscarporFolio.Visible = false;
                pnlMensaje.Visible = true;

                RbBuscarporFecha.Checked = true; 
            }
        }

        #region Funciones principales

        private void BuscarPorFolio(string folios)
        {
            txtError.Text = "";
            txtError.Visible = false;
            DataSet dsTotal = new DataSet();
            EliminarArchivoTxt();
            string[] parts = Regex.Split(folios, ",");
            int index = 0;

            foreach (var part in parts)
                // recorre los folios que se ingresan, separandolos de a uno para realizar una busqueda uno a uno
            {
                txtError.Visible = false;
                txtError.Text = "";
                DataSet ds = new DataSet();

                #region ASIGNO TIPO DE DOCUMENTO (crp, e, f)

                string tipoDocumento = "";

                if (rdCRP.Checked)
                {
                    tipoDocumento = "CRP";
                    ds = _negocio.BuscarBoletaxfolio(part, tipoDocumento);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int anio = DateTime.Parse(ds.Tables[0].Rows[0]["fecemi"].ToString()).Year;
                        if (anio == DateTime.Now.Year ||
                            (DateTime.Now.Month == 1 && (DateTime.Now.Year - 1) == anio))
                            //solo se pueden cargar facturas del mismo año o rezagadas de diciembre en enero
                        {
                            dsTotal.Merge(ds);
                        }
                    }
                }
                else if (rdE.Checked)
                {
                    tipoDocumento = "E";
                    ds = _negocio.BuscarDatosxFolio(part, tipoDocumento);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int anio = DateTime.Parse(ds.Tables[0].Rows[0]["fecemi"].ToString()).Year;
                        if (anio == DateTime.Now.Year ||
                            (DateTime.Now.Month == 1 && (DateTime.Now.Year - 1) == anio))
                        {
                            dsTotal.Merge(ds);
                        }
                    }
                }
                else if (rdF.Checked)
                {
                    tipoDocumento = "F";
                    ds = _negocio.BuscarDatosxFolio(part, tipoDocumento);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int anio = DateTime.Parse(ds.Tables[0].Rows[0]["fecemi"].ToString()).Year;
                        if (anio == DateTime.Now.Year ||
                            (DateTime.Now.Month == 1 && (DateTime.Now.Year - 1) == anio))
                        {
                            dsTotal.Merge(ds);
                        }
                    }
                }
                else if (rdNotaC.Checked)
                {
                    try
                    {
                        ds = _negocio.BuscarDatosxFolio(part, "C");

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (QuitarCaracteres(ds.Tables[0].Rows[0][10].ToString()) != "0")
                                //las notas de credito con valor 0 no se consideran
                            {
                                dsTotal.Merge(ds);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        txtError.Visible = true;
                        txtError.Text = e.ToString();
                    }
                }

                #endregion

                index++;
            }

            if (dsTotal.Tables.Count > 0 && dsTotal.Tables[0].Rows.Count > 0)
            {
                if (ValidarXml(dsTotal))
                {
                    GenerarXml(dsTotal, "Folios");
                }
                else
                {
                    btnVer.Visible = true;
                    lblMensaje.Text = "Error al generar archivos, consulte log de errores para su detalle";
                    pMensaje.Show();
                }
            }
            else
            {
                lblMensaje.Text = rdNotaC.Checked ? "No se encontraron datos.(Notas de crédito sin valor no son consideradas)" : "No se encontraron datos.";

                pMensaje.Show();
            }
        }

        public void BuscarPorFecha(string fechaInicio)
        {
            EliminarArchivoTxt();

            txtError.Visible = false;
            txtError.Text = "";

            DataSet Ds = new DataSet();

            #region ASIGNO TIPO DE DOCUMENTO (crp, e, f)

            string tipoDocumento = ""; //tipo documento (crp, e, f)

            if (rdCRP.Checked)
            {
                tipoDocumento = "CRP";
                try
                {
                    Ds = _negocio.BuscarBoletaFechaTipo(txtFechaInicio.Text, tipoDocumento);
                }
                catch (Exception e)
                {
                    txtError.Visible = true;
                    txtError.Text = e.ToString();
                }

            }
            else if (rdE.Checked)
            {
                tipoDocumento = "E";

                try
                {

                    Ds = _negocio.BuscarFacturaFechaTipoDoc(txtFechaInicio.Text, tipoDocumento); //busca facturas en facman solamente para no repetir las facturas
                }
                catch (Exception e)
                {
                    txtError.Visible = true;
                    txtError.Text = e.ToString();
                }
            }
            else if (rdF.Checked)
            {
                tipoDocumento = "F";
                try
                {

                    Ds = _negocio.BuscarFacturaFechaTipoDoc(txtFechaInicio.Text, tipoDocumento); //busca facturas en facman solamente para no repetir las facturas
                }
                catch (Exception e)
                {

                    txtError.Visible = true;
                    txtError.Text = e.ToString();

                }
            }
            else if (rdNotaC.Checked)
            {
                try
                {

                    Ds = _negocio.BuscarFacturaFechaTipoDoc(txtFechaInicio.Text, "C");
                }
                catch (Exception e)
                {

                    txtError.Visible = true;
                    txtError.Text = e.ToString();
                }
            }

            #endregion

            if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            {
                if (ValidarXml(Ds))
                {
                    GenerarXml(Ds, "Fechas");
                }
                else
                {
                    btnVer.Visible = true;
                    lblMensaje.Text = "Error al generar archivos, consulte log de errores para su detalle";
                    pMensaje.Show();
                }
            }
            else
            {
                lblMensaje.Text = "No se encontraron datos.";
                pMensaje.Show();
            }
        }

        private void GenerarXml(DataSet ds, string documento)
        {
            #region variables

            int sumatoriaValorFactura = 0;
            int valornetosum = 0;
            int valorexcentosum = 0;
            int despachosum = 0;
            int descuentosum = 0;
            int iva = 0;

            DtoXml dto = new DtoXml();
            DataTable dt = new DataTable("Archivos");
            dt.Columns.Add("Nombre", typeof (string));
            int count = 0; //contador para separar en 30 las transaccionesb (quedo en 5 transacciones)
            int countvariable = 0;
            string FechaFolios = "";

            int contadorDSDOCUPRE = 0;

            string rutCliente = "";
            string dVrutCliente = "";
            string conceptoReq = "";
            int contadorCorrelativoDocumentype = 1;
            List<long[]> listDevengo = new List<long[]>(); //en esta lista se guarda la respuesta del ws

            #endregion

            DevengoType.dev.DevengoType3 docDevengo = DevengoType.dev.DevengoType3.CreateDocument();
            soap.soap2.soap3 docSoap = soap.soap2.soap3.CreateDocument();

            DevengoType.dev.DevengoType2 ptroDevengo = docDevengo.Devengo.Append();
            DevengoType.dev.CabeceraArchivoType ptroCabeceraArchivoType = ptroDevengo.cabeceraArchivoType2.Append();

            int contadortotal = ds.Tables[0].Rows.Count;

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                #region Foreach FACMAN

                int sumaMontototalNetoOrigen = 0;

                if (item[3].ToString() == "R")
                {
                    dto.TipoDoc = "CRP";
                }
                else
                {
                    dto.TipoDoc = item[3].ToString();
                }
                

                

                dto.FechaEmision = item[5].ToString();
                dto.PfGrupo = item[13].ToString();

                #region Asigno Valores de tipo de documento

                if (dto.TipoDoc == "F") //factura afecta
                {
                    dto.CodDocumento = "0101";
                    dto.Impuesto = "true";
                    dto.Cabecera = "FA";
                    dto.TipoTansaccion = "1";
                }
                else if (dto.TipoDoc == "CRP") //boleta
                {
                    
                    /*
                    DateTime fechaControl = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["controlBoletas"]);
                    DateTime fechaEmision = DateTime.Parse(dto.FechaEmision);

                    if (fechaEmision >= fechaControl)//si la fecha de emision es mayor igual a la fecha de control, son documentos electronicos
                    {
                        dto.CodDocumento = "6060";//codigo de boleta electronica
                    }
                    else
                    {
                        dto.CodDocumento = "0900";
                    }
                     * DESCOMENTAR CUANDO SE PASE BOLETAS ELECTRONICAS A PRODUCCION
                     * 
                     */

                    dto.CodDocumento = "0900";
                    
                    dto.Impuesto = "false";
                    dto.Cabecera = "BV";
                    dto.TipoTansaccion = "1";
                    dto.Iva = "0";
                }
                else if (dto.TipoDoc == "E") //factura excenta
                {
                    dto.CodDocumento = "0201";
                    dto.Impuesto = "false";
                    dto.Cabecera = "FE";
                    dto.TipoTansaccion = "1";
                    dto.Iva = "0";
                }
                else if (dto.TipoDoc == "C") //Nota Credito
                {
                    dto.TipoAjuste = "7";
                    dto.FolRelDoc = item[7].ToString();
                    dto.TipRelDoc = item[12].ToString();

                    DataSet interSigfe = _negocio.BuscarInterSigfe(dto.TipRelDoc, int.Parse(dto.FolRelDoc));
                    //se busca en la tabla intersigfe el folio asignado en Sigfe 2 al documento

                    if (interSigfe != null && interSigfe.Tables[0].Rows.Count > 0)
                        //se valida que la factura este cargada en sigfe
                    {
                        dto.FolioSigfe = interSigfe.Tables[0].Rows[0]["folio_sigfe"].ToString();
                        dto.FechaCumplimiento =
                            CambiaFormatoFecha(interSigfe.Tables[0].Rows[0]["fecha_cumplimiento"].ToString());
                        dto.PfGrupo = interSigfe.Tables[0].Rows[0]["pf_grupo"].ToString();
                    }

                    listDevengo = WsObtenerDevengo(int.Parse(dto.FolioSigfe), int.Parse(txtEjercicio.Text),
                                                   dto.TipRelDoc, dto.FolRelDoc);

                    foreach (long[] str in listDevengo)
                    {
                        dto.CorrelativoDocumento = str[0].ToString(CultureInfo.InvariantCulture);
                        break;
                    }

                    dto.CodDocumento = "0401";
                    dto.Impuesto = item[12].ToString() == "F" ? "true" : "false";

                    dto.Cabecera = "NC";
                    dto.TipoTansaccion = "2";
                    dto.Iva = "0";
                }

                if (dto.CodDocumento != "0900") //&& dto.CodDocumento != "6060") DESCOMENTAR CUANDO SE PASE BOLETAS ELECTRONICAS A PRODUCCION
                {
                    dto.FechaVencimiento = item[6].ToString();
                    dto.FechaVencimiento = CambiaFormatoFecha(dto.FechaVencimiento);
                }

                if (dto.CodDocumento != "0401")
                {
                    dto.FechaCumplimiento = item[6].ToString();
                    dto.FechaCumplimiento = CambiaFormatoFecha(dto.FechaCumplimiento);
                }

                dto.CodigoFactura = item[0].ToString();
                dto.RutFactura = item[1].ToString();
                dto.DvFactura = item[2].ToString();

                dto.Folio = item[4].ToString();

                if (contadortotal == ds.Tables[0].Rows.Count)
                {
                    FechaFolios = item[5].ToString();
                    FechaFolios = FechaFolios.Replace(" ", "").Replace("-", "").Replace(":", "").Trim();
                }
                dto.FechaEmision = CambiaFormatoFecha(dto.FechaEmision);

                dto.ValorNeto = QuitarCaracteres(item[8].ToString());
                dto.ValorExento = QuitarCaracteres(item[9].ToString());
                dto.ValorFactura = QuitarCaracteres(item[10].ToString());
                dto.ValorNetoF = QuitarCaracteres(item[11].ToString());

                if (dto.PfGrupo == "C")
                {
                    dto.PfGrupo = "O";
                }

                if (item[14].ToString() != "")
                {
                    dto.Iva = QuitarCaracteres(item[14].ToString());
                }

                if (item[15].ToString() != "")
                {
                    dto.Descuento = QuitarCaracteres(item[15].ToString());
                }

                if (item[16].ToString() != "")
                {
                    dto.Despacho = QuitarCaracteres(item[16].ToString());
                }

                //Sumatoria de todas las facturas
                if (dto.ValorNeto != "")
                {
                    valornetosum = int.Parse(dto.ValorNeto);
                }

                if (dto.ValorExento != "")
                {
                    valorexcentosum = int.Parse(dto.ValorExento);
                }

                if (dto.Despacho != "")
                {
                    despachosum = int.Parse(dto.Despacho);
                }

                if (dto.Descuento != "")
                {
                    descuentosum = int.Parse(dto.Descuento);
                }

                sumaMontototalNetoOrigen = (valornetosum +
                                            valorexcentosum +
                                            despachosum) - descuentosum;


                sumatoriaValorFactura = sumatoriaValorFactura + ((
                                                                     valornetosum +
                                                                     valorexcentosum +
                                                                     despachosum
                                                                 ) - descuentosum);

                #endregion

                DataSet DSdoc = new DataSet();
                DataSet DSNivel2x = new DataSet();
                DataSet DSCliente = new DataSet();

                DSdoc = dto.TipoDoc == "CRP"
                            ? _negocio.BuscarDocuPreBoletaxFolioTipo(dto.Folio, "CRP")
                            : _negocio.BuscarDocuPrexFolioTipo(dto.Folio, dto.TipoDoc);

                DSCliente = _negocio.BuscarRutCliente(dto.RutFactura);

                if (DSCliente.Tables[0].Rows.Count > 0)
                {
                    rutCliente = VerificarLargo(DSCliente.Tables[0].Rows[0][4].ToString());
                    dVrutCliente = DSCliente.Tables[0].Rows[0][5].ToString();
                }

                #region transacciontype

                DevengoType.dev.TransaccionType ptroTransaccionType = ptroDevengo.transaccionType2.Append();
                if (dto.FolioSigfe.Equals(""))
                {
                    ptroTransaccionType.folioOriginal.Append();
                }
                else
                {
                    ptroTransaccionType.folioOriginal.Append().Value = dto.FolioSigfe;
                }
                ptroTransaccionType.tipoTransaccion.Append().Value = dto.TipoTansaccion;
                ptroTransaccionType.idTransferencia.Append().Value = dto.CodigoFactura;
                ptroTransaccionType.titulo.Append().Value = dto.Cabecera + "_" + dto.Folio + "_" + dto.FechaEmision;
                ptroTransaccionType.descripcionTransaccionDevengo.Append().Value = dto.Cabecera + "_" + dto.Folio + "_" +
                                                                                   dto.FechaEmision;

                if (dto.TipoAjuste.Equals(""))
                {
                    ptroTransaccionType.tipoAjuste.Append();
                }
                else
                {
                    ptroTransaccionType.tipoAjuste.Append().Value = dto.TipoAjuste;
                }
                ptroTransaccionType.devengoTED.Append().Value = false;

                #endregion

                DevengoType.dev.DocumentoType ptroDocumentoType = ptroTransaccionType.documentoType2.Append();

                #region CombinacionCatalogoContableType

                DevengoType.dev.CombinacionCatalogoContableType ptroCombinacionCatalogoContableType =
                    ptroTransaccionType.combinacionCatalogoContableType2.Append();
                ptroCombinacionCatalogoContableType.codigo.Append();
                ptroCombinacionCatalogoContableType.valor.Append();

                #endregion

                if (DSdoc.Tables[0].Rows.Count > 0 && DSdoc != null)
                {
                    string cuentaD = "";
                    string cuentaH = "";
                    string montoH = "";
                    string idReq = "";
                    string correlativoReq = "";
                    string codigoImpuesto = "";
                    string montoBase = "";

                    string fechaActual = CambiaFormatoFecha(DateTime.Now.AddMonths(2).ToString());
                    int sumaMontoCumplimiento = 0;

                    DevengoType.dev.ImputacionAGlosasType ptroImputacionAGlosasType = null;
                    DevengoType.dev.CampoVariableType ptroCampoVariableType = null;
                    DevengoType.dev.ImpuestoType ptroImpuestoType = null;
                    DevengoType.dev.PrincipalType ptroPrincipalType = null;

                    bool esAnterior = false;

                    if (dto.TipoDoc == "C") //si es nota de credito, hay que buscar el folio sigfe
                    {
                        foreach (var dato in listDevengo)
                        {
                            if (dato[3] == 1210) //si es traspaso de años anteriores va a la cuenta 1210
                            {
                                esAnterior = true;
                                conceptoReq = "1210"; //cuenta de devengos atrasados
                                correlativoReq = dato[2].ToString(CultureInfo.InvariantCulture);

                                //como es traspaso de años anteriores, 
                                //todo queda en un unico correlativo, esto es, se suman las cuentas y van todas al requerimiento 20 "institucional"

                            }
                        }
                    }

                    if (dto.TipoDoc == "C" && esAnterior)
                    {
                        #region Nota de creditos Anulando documentos anteriores

                        #region documentType

                        dto.FechaVencimiento = "";

                        #region DocumentType

                        if (dto.CorrelativoDocumento.Equals(""))
                            ptroDocumentoType.correlativoDocumento.Append();
                        else
                            ptroDocumentoType.correlativoDocumento.Append().Value = dto.CorrelativoDocumento;
                        //solo para las notas de crédito, es el folio de sigfe

                        ptroDocumentoType.tipoDocumento.Append().Value = dto.CodDocumento;
                        ptroDocumentoType.descripcionDocumentoDevengo.Append().Value = dto.Cabecera + "_" + dto.Folio +
                                                                                       "_" + dto.FechaEmision;
                        ptroDocumentoType.numeroDocumentoDevengo.Append().Value = dto.Folio;
                        ptroDocumentoType.fechaDocumentoDevengo.Append().Value = dto.FechaEmision;


                        /*CAMBIO SOLICITADO POR DIPRES, EL TAG DEBE IR VACIO 26-10-2015*/
                        /*VOLVER A SACAR LA FECHA 02-03-2016*/
                        //if (dto.TipoDoc == "E" || dto.TipoDoc == "F")
                        //{
                        //    ptroDocumentoType.fechaRecepcionConforme.Append().Value = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(dto.FechaEmision));//CAMBIO PEDIDO A ÚLTIMA HORA, SE DEJA EN DURAZNO  
                        //}
                        //else
                        //{
                            ptroDocumentoType.fechaRecepcionConforme.Append();
                        //}
                        /***************************************************************/

                        
                        

                        ptroDocumentoType.monedaDocumentoDevengo.Append().Value = "CLP";

                        ptroDocumentoType.fechaTipoCambio.Append();
                        ptroDocumentoType.contabilizacionImpuesto.Append().Value = bool.Parse(dto.Impuesto);
                        ptroDocumentoType.montoTotalNetoOrigen.Append().Value = sumaMontototalNetoOrigen;

                        #endregion

                        montoBase = "";

                        if (dto.ValorNeto != "")
                        {
                            sumaMontoCumplimiento = int.Parse(dto.ValorNeto);
                        }

                        if (dto.ValorExento != "")
                        {
                            sumaMontoCumplimiento = sumaMontoCumplimiento + int.Parse(dto.ValorExento);
                        }

                        if (dto.Despacho != "")
                        {
                            sumaMontoCumplimiento = sumaMontoCumplimiento + int.Parse(dto.Despacho);
                        }

                        if (dto.Descuento != "")
                        {
                            sumaMontoCumplimiento = sumaMontoCumplimiento - int.Parse(dto.Descuento);
                        }

                        contadorCorrelativoDocumentype++;

                        #endregion

                        ptroImputacionAGlosasType = ptroDocumentoType.imputacionAGlosasType2.Append();
                        ptroCampoVariableType =
                            ptroDocumentoType.camposVariablesType2.Append().campoVariableType2.Append();
                        ptroImpuestoType = ptroDocumentoType.impuestoType2.Append();

                        ptroPrincipalType = ptroDocumentoType.principalType2.Append();
                        ptroPrincipalType.rut.Append().Value = rutCliente + "-" + dVrutCliente;
                        ptroPrincipalType.institucionTED.Append();

                        #region requerimientoCompromisoType

                        DevengoType.dev.RequerimientoCompromisoType ptroRequerimientoCompromisoType =
                            ptroPrincipalType.requerimientoCompromisoType2.Append();

                        ptroRequerimientoCompromisoType.correlativoRequerimientoCompromiso.Append().Value =
                            long.Parse(correlativoReq);


                        ptroRequerimientoCompromisoType.proceso.Append().Value = "01";
                        ptroRequerimientoCompromisoType.folio.Append();

                        #region CombinacionType

                        DevengoType.dev.CombinacionType ptroCombinacionType =
                            ptroRequerimientoCompromisoType.combinacionType2.Append();

                        #region ConceptoPresupuestarioType

                        DevengoType.dev.ConceptoPresupuestarioType ptrConceptoPresupuestarioType =
                            ptroCombinacionType.conceptoPresupuestarioType2.Append();
                        ptrConceptoPresupuestarioType.concepto.Append().Value = conceptoReq;
                        ptrConceptoPresupuestarioType.montoNeto.Append().Value = decimal.Parse(dto.ValorFactura);

                        #region cuentaContable

                        DevengoType.dev.CuentaContableType ptroCuentaContableType =
                            ptrConceptoPresupuestarioType.cuentaContable.Append();
                        ptroCuentaContableType.codigoCuentaDebe.Append().Value = System.Configuration.ConfigurationManager.AppSettings["codigoCuentaDebe"];//codigo de cuenta debe atrasados
                        ptroCuentaContableType.codigoCuentaHaber.Append().Value = System.Configuration.ConfigurationManager.AppSettings["codigoCuentaHaber"];//codigo de cuenta haber atrasados
                        ptroCuentaContableType.montoCuentaDebe.Append().Value = decimal.Parse(dto.ValorFactura);
                        ptroCuentaContableType.montoCuentaHaber.Append().Value = decimal.Parse(dto.ValorFactura);

                        #endregion

                        #region CatalogosType

                        DevengoType.dev.CatalogosType ptroCatalogosType = ptroCombinacionType.catalogosType2.Append();
                        ptroCatalogosType.catalogo.Append().Value = "iniciativaInversion";
                        ptroCatalogosType.valor.Append().Value = "00";

                        #endregion

                        #endregion

                        #endregion

                        #endregion

                        #endregion
                    }
                    else
                    {
                        for (int j = 0; j < DSdoc.Tables[0].Rows.Count; j++)
                        {
                            #region DOCU_PRE

                            DtoDocuPre dtoDocuPre = new DtoDocuPre();

                            dtoDocuPre.Emision = DSdoc.Tables[0].Rows[j][5].ToString();
                            dtoDocuPre.Emision = CambiaFormatoFecha(dtoDocuPre.Emision);
                            dtoDocuPre.FolioDoc = DSdoc.Tables[0].Rows[j][16].ToString();
                            dtoDocuPre.Cuenta = DSdoc.Tables[0].Rows[j][18].ToString();
                            dtoDocuPre.Depto = QuitarCaracteres(DSdoc.Tables[0].Rows[j][20].ToString());
                            dtoDocuPre.Seccion = QuitarCaracteres(DSdoc.Tables[0].Rows[j][21].ToString());
                            dtoDocuPre.Dh = DSdoc.Tables[0].Rows[j][23].ToString();
                            dtoDocuPre.Origen = DSdoc.Tables[0].Rows[j][24].ToString();
                            dtoDocuPre.NumSerial = DSdoc.Tables[0].Rows[j][25].ToString();
                            dtoDocuPre.PrsgSerial = DSdoc.Tables[0].Rows[j][26].ToString();

                            if (dtoDocuPre.Depto != "" && dtoDocuPre.Seccion != "")
                            {

                                DSNivel2x = _negocio.BuscarIdRequerimiento("1", dtoDocuPre.Depto, dtoDocuPre.Seccion,
                                                                           dto.PfGrupo, txtEjercicio.Text);

                                if (DSNivel2x.Tables[0].Rows.Count > 0 && DSNivel2x != null)
                                {
                                    dtoDocuPre.Dh = DSdoc.Tables[0].Rows[j][23].ToString();
                                    dtoDocuPre.IdReq = DSNivel2x.Tables[0].Rows[0]["idreq"].ToString();

                                    if (dtoDocuPre.Dh == "D")
                                    {
                                        idReq = dtoDocuPre.IdReq;
                                    }

                                    dtoDocuPre.FolioNivel2X = DSNivel2x.Tables[0].Rows[0]["idcon"].ToString();
                                    conceptoReq = dtoDocuPre.FolioNivel2X;
                                    dtoDocuPre.CorrelativoReq = DSNivel2x.Tables[0].Rows[0]["idagrupacion"].ToString();
                                    correlativoReq = dtoDocuPre.CorrelativoReq;

                                    if (dto.TipoDoc == "C") //si es nota de credito, hay que buscar el folio sigfe
                                    {
                                        foreach (var dato in listDevengo)
                                        {
                                            if (dtoDocuPre.IdReq == dato[1].ToString(CultureInfo.InvariantCulture))
                                            //si es el mismo departamento
                                            {
                                                correlativoReq = dato[2].ToString(CultureInfo.InvariantCulture);
                                                //se asigna el correlativo requerimiento correspondiente

                                                break;
                                            }
                                        }
                                    }
                                    
                                }
                            }

                            dtoDocuPre.Cuenta = DSdoc.Tables[0].Rows[j][18].ToString();
                            dtoDocuPre.Monto = DSdoc.Tables[0].Rows[j][22].ToString();


                            string montoCorrecto = QuitarCaracteres(dtoDocuPre.Monto);

                            dtoDocuPre.Dh = DSdoc.Tables[0].Rows[j][23].ToString();

                            if (dtoDocuPre.Dh == "D")
                            {
                                cuentaD = dtoDocuPre.Cuenta;
                            }
                            else if (dtoDocuPre.Dh == "H")
                            {
                                cuentaH = dtoDocuPre.Cuenta;
                                montoH = montoCorrecto;
                            }

                            if (dto.TipoDoc == "C") //Cuando es nota de credito el folio va vacio
                            {
                                idReq = "";
                                //correlativoReq = dto.RequerimientoOriginal;
                            }

                            contadorDSDOCUPRE++;

                            if (j == 0) //tiene que escribir solo una vez
                            {
                                #region documentType

                                if (dto.TipoDoc == "C")
                                {
                                    dto.FechaVencimiento = "";

                                }
                                else
                                {
                                    dto.CorrelativoDocumento = "";
                                }

                                #region DocumentType

                                if (dto.CorrelativoDocumento.Equals(""))
                                    ptroDocumentoType.correlativoDocumento.Append();
                                else
                                    ptroDocumentoType.correlativoDocumento.Append().Value = dto.CorrelativoDocumento;
                                //solo para las notas de crédito, es el folio de sigfe

                                ptroDocumentoType.tipoDocumento.Append().Value = dto.CodDocumento;
                                ptroDocumentoType.descripcionDocumentoDevengo.Append().Value = dto.Cabecera + "_" +
                                                                                               dtoDocuPre.FolioDoc + "_" +
                                                                                               dto.FechaEmision;
                                ptroDocumentoType.numeroDocumentoDevengo.Append().Value = dto.Folio;
                                ptroDocumentoType.fechaDocumentoDevengo.Append().Value = dtoDocuPre.Emision;


                                /*CAMBIO SOLICITADO POR DIPRES, EL TAG DEBE IR VACIO 26-10-2015*/
                                //if (dto.TipoDoc == "E" || dto.TipoDoc == "F")
                                //{
                                //    ptroDocumentoType.fechaRecepcionConforme.Append().Value = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(dto.FechaEmision));//CAMBIO PEDIDO A ÚLTIMA HORA, SE DEJA EN DURAZNO  
                                //}
                                //else
                                //{
                                    ptroDocumentoType.fechaRecepcionConforme.Append();
                                //}
                                
                                /***************************************************************/

                                /*DIPRES SOLICITA LLENAR EL TAG NUEVAMENTE ¬¬ 25-02-2016*/
                                //ptroDocumentoType.fechaRecepcionConforme.Append().Value = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(dto.FechaEmision));//CAMBIO PEDIDO A ÚLTIMA HORA, SE DEJA EN DURAZNO


                                ptroDocumentoType.monedaDocumentoDevengo.Append().Value = "CLP";

                                ptroDocumentoType.fechaTipoCambio.Append();
                                ptroDocumentoType.contabilizacionImpuesto.Append().Value = bool.Parse(dto.Impuesto);
                                ptroDocumentoType.montoTotalNetoOrigen.Append().Value = sumaMontototalNetoOrigen;

                                #endregion

                                if (dto.TipoDoc == "F")
                                {
                                    codigoImpuesto = "1"; //si es f 1 si no ""
                                }

                                if (dto.TipoDoc == "E" || dto.TipoDoc == "C")
                                {
                                    montoBase = "";
                                }
                                else if (dto.TipoDoc == "CRP")
                                {
                                    montoBase = "";
                                }
                                else
                                {
                                    montoBase =
                                        (int.Parse(dto.ValorNeto) + int.Parse(dto.ValorExento) + int.Parse(dto.Despacho))
                                            .ToString(CultureInfo.InvariantCulture);
                                }


                                if (dto.ValorNeto != "")
                                {
                                    sumaMontoCumplimiento = int.Parse(dto.ValorNeto);
                                }

                                if (dto.ValorExento != "")
                                {
                                    sumaMontoCumplimiento = sumaMontoCumplimiento + int.Parse(dto.ValorExento);
                                }

                                if (dto.Despacho != "")
                                {
                                    sumaMontoCumplimiento = sumaMontoCumplimiento + int.Parse(dto.Despacho);
                                }

                                if (dto.Descuento != "")
                                {
                                    sumaMontoCumplimiento = sumaMontoCumplimiento - int.Parse(dto.Descuento);
                                }

                                contadorCorrelativoDocumentype++;

                                #endregion

                                ptroImputacionAGlosasType = ptroDocumentoType.imputacionAGlosasType2.Append();
                                ptroCampoVariableType =
                                    ptroDocumentoType.camposVariablesType2.Append().campoVariableType2.Append();
                                ptroImpuestoType = ptroDocumentoType.impuestoType2.Append();

                                ptroPrincipalType = ptroDocumentoType.principalType2.Append();
                                ptroPrincipalType.rut.Append().Value = rutCliente + "-" + dVrutCliente;
                                ptroPrincipalType.institucionTED.Append();
                            }

                            if (j != 0 && (j%2 != 0))
                            {
                                #region requerimientoCompromisoType

                                DevengoType.dev.RequerimientoCompromisoType ptroRequerimientoCompromisoType =
                                    ptroPrincipalType.requerimientoCompromisoType2.Append();

                                ptroRequerimientoCompromisoType.correlativoRequerimientoCompromiso.Append().Value =
                                    long.Parse(correlativoReq);


                                ptroRequerimientoCompromisoType.proceso.Append().Value = "01";
                                ptroRequerimientoCompromisoType.folio.Append().Value = idReq;

                                #region CombinacionType

                                DevengoType.dev.CombinacionType ptroCombinacionType =
                                    ptroRequerimientoCompromisoType.combinacionType2.Append();

                                #region ConceptoPresupuestarioType

                                DevengoType.dev.ConceptoPresupuestarioType ptrConceptoPresupuestarioType =
                                    ptroCombinacionType.conceptoPresupuestarioType2.Append();
                                ptrConceptoPresupuestarioType.concepto.Append().Value = conceptoReq;
                                ptrConceptoPresupuestarioType.montoNeto.Append().Value = decimal.Parse(montoH);

                                #region cuentaContable

                                DevengoType.dev.CuentaContableType ptroCuentaContableType =
                                    ptrConceptoPresupuestarioType.cuentaContable.Append();
                                ptroCuentaContableType.codigoCuentaDebe.Append().Value = cuentaD;
                                ptroCuentaContableType.codigoCuentaHaber.Append().Value = cuentaH;
                                ptroCuentaContableType.montoCuentaDebe.Append().Value = decimal.Parse(montoH);
                                ptroCuentaContableType.montoCuentaHaber.Append().Value = decimal.Parse(montoH);

                                #endregion

                                #region CatalogosType

                                DevengoType.dev.CatalogosType ptroCatalogosType =
                                    ptroCombinacionType.catalogosType2.Append();
                                ptroCatalogosType.catalogo.Append().Value = "iniciativaInversion";
                                ptroCatalogosType.valor.Append().Value = "00";

                                #endregion

                                #endregion

                                #endregion

                                #endregion
                            }

                            #endregion
                        }
                    }

                    #region imputacionAGlosasType

                    ptroImputacionAGlosasType.programaPresupuestario.Append();
                    ptroImputacionAGlosasType.numeroGlosaPresupuestaria.Append();
                    ptroImputacionAGlosasType.letraGlosaPresupuestaria.Append();
                    ptroImputacionAGlosasType.valorImputacion.Append();

                    #endregion

                    #region campoVariableType

                    ptroCampoVariableType.codigo.Append();
                    ptroCampoVariableType.valor.Append();

                    #endregion

                    #region impuestotype

                    ptroImpuestoType.codigoImpuesto.Append().Value = codigoImpuesto;
                    if (montoBase.Equals(""))
                        ptroImpuestoType.montoBase.Append();
                    else
                        ptroImpuestoType.montoBase.Append().Value = montoBase;

                    #endregion

                    #region CumplimientoType

                    DevengoType.dev.CumplimientoType ptroCumplimientoType = ptroPrincipalType.cumplimientoType2.Append();
                    ptroCumplimientoType.rutPrincipalRelacionado.Append().Value = rutCliente + "-" + dVrutCliente;

                    ptroCumplimientoType.fechaCumplimiento.Append().Value = dto.CodDocumento == "0401"
                                                                                ? dto.FechaCumplimiento
                                                                                : fechaActual;

                    ptroCumplimientoType.montoCumplimiento.Append().Value = sumaMontoCumplimiento;

                    #endregion
                }


                count++;
                contadortotal--;

                if (count == 5 || contadortotal == 0)
                {
                    #region cabeceraArchivoType

                    string cantidadTransacciones = "";

                    if (ds.Tables[0].Rows.Count > 5 && contadortotal < 5 && count < 5) //si es el ultimo xml
                    {
                        int ultimaTransaccion = (ds.Tables[0].Rows.Count)%5;

                        cantidadTransacciones = ultimaTransaccion.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (ds.Tables[0].Rows.Count > 5)
                    {
                        cantidadTransacciones = "5";
                    }
                    else
                    {
                        cantidadTransacciones = ds.Tables[0].Rows.Count.ToString(CultureInfo.InvariantCulture);
                    }


                    Random r = new Random();
                    string aleatorio = r.Next(999).ToString(CultureInfo.InvariantCulture);


                    string fecha = "";
                    fecha = string.Format("{0:yy-MM-dd hh:mm:ss}", DateTime.Now); //fecha actual para id de envío
                    fecha = fecha.Replace("-", "").Replace(":", "").Replace(" ", "").Trim();
                    string id = fecha + aleatorio + countvariable;

                    ptroCabeceraArchivoType.ejercicio.Append().Value = int.Parse(txtEjercicio.Text);
                    ptroCabeceraArchivoType.periodo.Append().Value = ddlPeriodo.SelectedValue;
                    ptroCabeceraArchivoType.proceso.Append().Value = "0301";
                    ptroCabeceraArchivoType.identificacionEnvio.Append().Value = long.Parse(id);
                    ptroCabeceraArchivoType.montoTotal.Append().Value = sumatoriaValorFactura;
                    ptroCabeceraArchivoType.cantidadTransacciones.Append().Value = int.Parse(cantidadTransacciones);
                    ptroCabeceraArchivoType.usuarioGenerador.Append().Value = (string) Session["UsuarioLogueado"];
                    ptroCabeceraArchivoType.usuarioAprobador.Append().Value = (string) Session["UsuarioLogueado"];

                    DevengoType.dev.InformacionInstitucionType ptroInformacionInstitucionType =
                        ptroCabeceraArchivoType.informacionInstitucionType2.Append();
                    ptroInformacionInstitucionType.partida.Append().Value = "16";
                    ptroInformacionInstitucionType.capitulo.Append().Value = "04";
                    ptroInformacionInstitucionType.areaTransaccional.Append().Value = "001";

                    #endregion

                    countvariable++;

                    count = 0;

                    string fechaHrActual = "";
                    
                    fechaHrActual = txtFechaInicio.Text != string.Empty ? string.Format("{0:dd-MM-yyyy}", txtFechaInicio.Text) : string.Format("{0:dd-MM-yyyy}", DateTime.Now);

                    fechaHrActual = fechaHrActual.Replace(" ", "").Replace("-", "").Replace(":", "").Trim();

                    string path = "";
                    string nombreArchivo = "";

                    if (documento.Equals("Folios"))
                    {
                        
                        path =
                            System.Web.HttpContext.Current.Server.MapPath(@"LogSigfe2\" + txtEjercicio.Text + @"\" +
                                                                          FechaFolios + @"\" + dto.TipoDoc +
                                                                          @"_Rezagados\");

                        nombreArchivo = String.Format("{0}_{1}_{2}_Rezagado-{3}.xml", fechaHrActual, dto.CodigoFactura,
                                                      dto.TipoDoc, countvariable);
                    }
                    else
                    {
                        path =
                            System.Web.HttpContext.Current.Server.MapPath(@"LogSigfe2\" + txtEjercicio.Text + @"\" +
                                                                          fechaHrActual + @"\" + dto.TipoDoc + @"\");

                        nombreArchivo = fechaHrActual + "_" + dto.CodigoFactura + "_" + dto.TipoDoc + "-" +
                                        countvariable + ".xml";
                    }

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string ruta = path + nombreArchivo;

                    soap.soap2.Envelope ptroEnv = docSoap.Envelope2.Append();
                    soap.soap2.Header ptroHeader = ptroEnv.Header2.Append();
                    soap.soap2.Body ptroBody = ptroEnv.Body2.Append();

                    string xmlDevengo =
                        docDevengo.SaveToString(true).Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

                    ptroBody.Node.InnerXml = xmlDevengo;

                    docSoap.SaveToFile(ruta, true, "utf-8");

                    DataRow row = dt.NewRow();
                    row["Nombre"] = nombreArchivo;
                    dt.Rows.Add(row);

                    sumatoriaValorFactura = 0;

                    valornetosum = 0;
                    valorexcentosum = 0;
                    despachosum = 0;
                    descuentosum = 0;
                    iva = 0;

                    docDevengo = null;
                    docSoap = null;
                    ptroDevengo = null;
                    ptroCabeceraArchivoType = null;

                    docDevengo = DevengoType.dev.DevengoType3.CreateDocument();
                    docSoap = soap.soap2.soap3.CreateDocument();
                    ptroDevengo = docDevengo.Devengo.Append();
                    ptroCabeceraArchivoType = ptroDevengo.cabeceraArchivoType2.Append();

                    if (contadortotal == 0)
                    {
                        Session.Add("Ruta", path);
                        lblMensaje.Text = "Archivos generados correctamente";
                        pMensaje.Show();

                        gdvArchivos.Visible = true;
                        gdvArchivos.DataSource = dt;
                        gdvArchivos.DataBind();

                        dt = null;
                        dt = new DataTable();

                    }
                }

                #endregion
            }
        }

        #endregion

        #region Validaciones

        private bool ValidarXml(DataSet ds)
        {
            bool res = true;

            foreach (DataRow item in ds.Tables[0].Rows)
            {

                #region variables
                int valornetosum = 0;
                int valorexcentosum = 0;
                int despachosum = 0;
                int descuentosum = 0;
                DtoXml dto = new DtoXml();
                string errorGral = "";
                string rutCliente = "";
                string dVrutCliente = "";
                #endregion

                #region Foreach FACMAN

                int sumaMontototalNetoOrigen = 0;
                int sumaMontosH = 0;

                dto.TipoDoc = item[3].ToString();

                #region Asigno Valores de tipo de documento

                if (rdCRP.Checked)//Boleta
                {
                    dto.TipoDoc = "CRP";
                }
                else if (rdE.Checked)//Factura excenta
                {
                    dto.TipoDoc = "E";
                }
                else if (rdF.Checked)//Factura afecta
                {
                    dto.TipoDoc = "F";
                }
                else if (rdNotaC.Checked)//Nota Credito
                {
                    dto.TipoDoc = "C";
                }

                dto.CodigoFactura = item[0].ToString();
                dto.RutFactura = item[1].ToString();
                dto.DvFactura = item[2].ToString();
                dto.Folio = item[4].ToString();
               
                dto.ValorNeto = QuitarCaracteres(item[8].ToString());
                dto.ValorExento = QuitarCaracteres(item[9].ToString());
                dto.ValorFactura = QuitarCaracteres(item[10].ToString());
                dto.PfGrupo = item[13].ToString();

                if (dto.TipoDoc == "C")//si es nota de crédito, se toma el grupo desde la factura
                {
                    dto.FolRelDoc = item[7].ToString();
                    dto.TipRelDoc = item[12].ToString();
                }

                if (item[14].ToString() != "")
                {
                    dto.Iva = QuitarCaracteres(item[14].ToString());
                }

                if (item[15].ToString() != "")
                {
                    dto.Descuento = QuitarCaracteres(item[15].ToString());
                }

                if (item[16].ToString() != "")
                {
                    dto.Despacho = QuitarCaracteres(item[16].ToString());
                }

                #endregion

                if (dto.ValorNeto != "")
                {
                    valornetosum = int.Parse(dto.ValorNeto);
                }

                if (dto.ValorExento != "")
                {
                    valorexcentosum = int.Parse(dto.ValorExento);
                }

                if (dto.Despacho != "")
                {
                    despachosum = int.Parse(dto.Despacho);
                }

                if (dto.Descuento != "")
                {
                    descuentosum = int.Parse(dto.Descuento);
                }
                sumaMontototalNetoOrigen = ((
                    valornetosum +
                    valorexcentosum +
                    despachosum
                    ) - descuentosum);

                DataSet DsDoc = new DataSet();
                DataSet DSNivel2x = new DataSet();
                DataSet DSCliente = new DataSet();

                DsDoc = dto.TipoDoc == "CRP" ? _negocio.BuscarDocuPreBoletaxFolioTipo(dto.Folio, dto.TipoDoc) : _negocio.BuscarDocuPrexFolioTipo(dto.Folio, dto.TipoDoc);

                DSCliente = _negocio.BuscarRutCliente(dto.RutFactura);

                if (DSCliente.Tables[0].Rows.Count > 0)
                {
                    rutCliente = VerificarLargo(DSCliente.Tables[0].Rows[0][4].ToString());

                    dVrutCliente = DSCliente.Tables[0].Rows[0][5].ToString() == "K" ? DSCliente.Tables[0].Rows[0][5].ToString().ToLower() : DSCliente.Tables[0].Rows[0][5].ToString();
                }

                string rutClienteConDv = rutCliente + "-" + dVrutCliente;

                if (!ValidaRut(rutCliente, dVrutCliente))
                {
                    errorGral = "El rut " + rutClienteConDv + ", de la factura " + dto.Folio + " esta erroneo.";
                    const string nombreArchivo = "ErroresXML.txt";
                    Escribeylee(errorGral, nombreArchivo);
                    res = false;
                }

                #region VALIDACION RUT EN WS
                /*ESTE WS NO ESTA HABILITADO AÚN*/
                //if (!WSBuscarPersona(rutclienteconDv))
                //{
                //    ERRORGral = "El rut " + rutclienteconDv + ", de la factura " + dto.Folio + " no se encuentra registrado en Banco Personas.";
                //    string nombrearchivo = "ErroresXML.txt";
                //    Escribeylee(ERRORGral, nombrearchivo);
                //    res = false;
                //}

                #endregion

                #endregion

                bool resagada = false;
                if (dto.TipoDoc == "C")
                {
                    DataSet interSigfe = _negocio.BuscarInterSigfe(dto.TipRelDoc, int.Parse(dto.FolRelDoc));
                    //se busca en la tabla intersigfe el folio asignado en Sigfe 2 al documento

                    if (interSigfe != null && interSigfe.Tables[0].Rows.Count > 0)
                    //se valida que la factura este cargada en sigfe
                    {
                        dto.FolioSigfe = interSigfe.Tables[0].Rows[0]["folio_sigfe"].ToString();
                        dto.FechaCumplimiento =
                            CambiaFormatoFecha(interSigfe.Tables[0].Rows[0]["fecha_cumplimiento"].ToString());
                        dto.PfGrupo = interSigfe.Tables[0].Rows[0]["pf_grupo"].ToString();//si es nota de credito, se considera el grupo de la factura, para evitar errores.
                    }

                    if (!string.IsNullOrEmpty(dto.FolioSigfe))
                    {
                        var listDevengo = WsObtenerDevengo(int.Parse(dto.FolioSigfe), int.Parse(txtEjercicio.Text),
                                                       dto.TipRelDoc, dto.FolRelDoc);

                        if (listDevengo.Count > 0 && listDevengo.Any(dato => dato[3] == 1210))
                        {
                            resagada = true;
                        }
                        else if (dto.TipoDoc == "C" && (listDevengo == null || listDevengo.Count == 0))
                        {
                            errorGral = "No se ha cargado la factura en Sigfe 2 o el Web Service 'Obtener Devengo'" +
                                        " no esta funcionando. Nota de credito folio: " + dto.Folio;
                            const string nombreArchivo = "ErroresXML.txt";
                            Escribeylee(errorGral, nombreArchivo);
                            res = false;
                        }
                    }
                    else
                    {
                        errorGral = "No se ha cargado la factura en Sigfe 2" +
                                       " .Factura: " + dto.TipRelDoc+ " Folio: "+dto.FolRelDoc;
                        const string nombreArchivo = "ErroresXML.txt";
                        Escribeylee(errorGral, nombreArchivo);
                        res = false;
                    }
                }

                if (dto.PfGrupo == "C")
                {
                    dto.PfGrupo = "O";
                }

                if (DsDoc.Tables[0].Rows.Count > 0 && DsDoc != null)//si es una factura resagada, no sirve la validación pues cambiaron las cuentas 
                {
                    for (int j = 0; j < DsDoc.Tables[0].Rows.Count; j++)
                    {
                        #region DOCU_PRE
                        DtoDocuPre dtoDocuPre = new DtoDocuPre();

                        dtoDocuPre.Cuenta = DsDoc.Tables[0].Rows[j][18].ToString();
                        dtoDocuPre.Depto = QuitarCaracteres(DsDoc.Tables[0].Rows[j][20].ToString());
                        dtoDocuPre.Seccion = QuitarCaracteres(DsDoc.Tables[0].Rows[j][21].ToString());

                        if (DsDoc.Tables[0].Rows[j][23].ToString().ToLower() == "h")
                        {
                            sumaMontosH = sumaMontosH + int.Parse(QuitarCaracteres(DsDoc.Tables[0].Rows[j][22].ToString()));
                        }

                        string cuentaSolicitada = "";
                        cuentaSolicitada = BuscarCuentaContable(dtoDocuPre.Cuenta.Trim());//busca en cuenta

                        /*************************************Validación de cuenta*********************************/
                        if (cuentaSolicitada == "")
                        {
                            const string nombreArchivo = "ErroresXML.txt";
                            errorGral = "Error: la cuenta " + dtoDocuPre.Cuenta.Trim() + " de la factura " + dto.TipoDoc + " " + dto.Folio + " no existe en el plan de cuentas.";
                            Escribeylee(errorGral, nombreArchivo);
                            res = false;
                        }
                        /******************************************************************************************/

                        if (dtoDocuPre.Depto != "" && dtoDocuPre.Seccion != "")
                        {
                            /*************************************Validación de identificador de requerimiento*********************************/

                            if (!resagada)//si es nota de credito rezagada, va siempre a "cuentas por cobrar", por eso no se valida
                            {
                                switch (dto.PfGrupo)
                                {
                                    case "S": dto.Cuenta = "11507";//servicios
                                        break;

                                    case "O":
                                        dto.Cuenta = "11508";//otros
                                        break;

                                    case "B":
                                        dto.Cuenta = "11507";//bienes
                                        break;

                                }

                                if (dtoDocuPre.Cuenta != dto.Cuenta)//validar las cuentas
                                {
                                    _mensajeXlm = "Error: Las cuentas no coinciden. Cuenta factura: " + dto.Cuenta + ", cuenta en docu_pre  " + dtoDocuPre.Cuenta +
                                                     " factura " + dto.TipoDoc + " " + dto.Folio + ".";
                                    const string nombreArchivo = "ErroresXML.txt";
                                    errorGral = _mensajeXlm;
                                    Escribeylee(errorGral, nombreArchivo);
                                    res = false;
                                }


                                DSNivel2x = _negocio.BuscarIdRequerimiento("1", dtoDocuPre.Depto, dtoDocuPre.Seccion,
                                                                           dto.PfGrupo, txtEjercicio.Text);

                                if (DSNivel2x == null || DSNivel2x.Tables.Count <= 0 || DSNivel2x.Tables[0].Rows.Count == 0)
                                {
                                    _mensajeXlm = "Error: No se encontró el identificador del requerimiento del departamento "
                                                  + dtoDocuPre.Depto + ",sección " + dtoDocuPre.Seccion + " y grupo " +
                                                  dto.PfGrupo +
                                                  " para la factura " + dto.TipoDoc + " " + dto.Folio + ".";
                                    const string nombreArchivo = "ErroresXML.txt";
                                    errorGral = _mensajeXlm;
                                    Escribeylee(errorGral, nombreArchivo);
                                    res = false;
                                }
                            }
                            /****************************************************************************************************************/

                        }
                        #endregion
                    }

                    /*************************************Validación de montos************************************************/
                    if (sumaMontototalNetoOrigen != sumaMontosH)//montos de facman vs montos de docu_pre
                    {
                        #region SI NO COINCIDEN LOS MONTOS 

                        //si la factura no tiene el mismo monto total se genera un error                       
                        const string nombreArchivo = "ErroresXML.txt";
                        errorGral = "Error: los montos de la factura " + dto.Folio + " no cuadran.";
                        Escribeylee(errorGral, nombreArchivo);
                        res = false;
                        #endregion
                    }
                    /****************************************************************************************************************/
                }
            }
            return res;
        }

        private bool ValidaAño(string año)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                int convert = DateTime.ParseExact(año, "yyyy", provider).Year;
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool ValidaDatosIngreso()
        {
            #region FUNCION VALIDA DATOS INGRESADOS

            bool esValido = true;
            string codError = null;
            CultureInfo provider = CultureInfo.InvariantCulture;


            if (txtEjercicio.Text.Length == 0)
            {
                esValido = false;
                txtEjercicio.Focus();
                codError = "err_Ejer";
            }
            else if (!ValidaAño(txtEjercicio.Text))
            {
                esValido = false;
                txtEjercicio.Focus();
                codError = "err_Ejer1";
            }
            else if (DateTime.ParseExact(txtEjercicio.Text, "yyyy", provider).Year > DateTime.Now.Year)
            {
                esValido = false;
                txtEjercicio.Focus();
                codError = "err_Ejer2";
            }
            else if (ddlPeriodo.SelectedValue == "0")
            {
                esValido = false;
                ddlPeriodo.Focus();
                codError = "err_Per";
            }
            else if (!rdE.Checked && !rdCRP.Checked && !rdF.Checked && !rdNotaC.Checked)
            {
                esValido = false;
                rdE.Focus();
                codError = "err_TipDoc";
            }
            else if (RbBuscarporFecha.Checked == false && buscarporfolio.Checked == false)
            {
                esValido = false;
                rdE.Focus();
                codError = "err_TipoBusc";
            }
            else if (RbBuscarporFecha.Checked && txtFechaInicio.Text.Length == 0)
            {
                esValido = false;
                txtFechaInicio.Focus();
                codError = "err_Fecha";
            }
            else if (RbBuscarporFecha.Checked && !IsDate(txtFechaInicio.Text))
            {
                esValido = false;
                txtFechaInicio.Focus();
                codError = "err_Vfecha";
            }
            else if (buscarporfolio.Checked && txtXML.Text.Length == 0)
            {
                esValido = false;
                txtXML.Focus();
                codError = "err_Fo";
            }


            InterpretarError(codError);
            return esValido;

            #endregion
        }

        private void InterpretarError(string codError)
        {
            #region Funcion Interpreta error

            string msjeError = null;

            switch (codError)
            {
                case "err_Ejer":
                    msjeError = "Debe ingresar Ejercicio.";
                    break;
                case "err_Ejer1":
                    msjeError = "Error en el formato ingresado.";
                    break;
                case "err_Ejer2":
                    msjeError = "El año ingresado es mayor al actual.";
                    break;
                case "err_Per":
                    msjeError = "Debe seleccionar Periodo.";
                    break;
                case "err_TipDoc":
                    msjeError = "Debe ingresar Tipo de Documento.";
                    break;
                case "err_TipoBusc":
                    msjeError = "Debe ingresar Tipo Búsqueda.";
                    break;
                case "err_Fecha":
                    msjeError = "Debe ingresar Fecha.";
                    break;
                case "err_Vfecha":
                    msjeError = "El formato de la fecha no es válido.";
                    break;
                case "err_Fo":
                    msjeError = "Debe ingresar al menos un Folio.";
                    break;
            }

            if (msjeError != null)
            {
                lblMensaje.Text = msjeError;
                pMensaje.Show();
            }
            #endregion
        }

        #endregion

        #region botones y eventos

        protected void BtnPAceptar_Click(object sender, EventArgs e)
        {
            txtError.Text = "";
            txtError.Visible = false;
            lblMensajeError.Visible = false;
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            const string fileArchivoName = "ErroresXML.txt";
            string fileName = Server.MapPath(@"Log\") + fileArchivoName;

            FileInfo file = new FileInfo(fileName);
            if (file.Exists)
            {
                Response.Clear();
                Response.HeaderEncoding = System.Text.Encoding.Default;
                Response.AppendHeader("Content-Disposition", "attachment; filename=ErroresXML.txt");
                Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
                Response.ContentType = "text/plain";// "Application/msword";
                Response.TransmitFile(file.FullName);
                Response.End();
            }
        }

        protected void RbBuscarporFecha_CheckedChanged(object sender, EventArgs e)
        {
            divBuscarporFolio.Visible = false;
            divBuscarporFecha.Visible = true;
            btnGenerarXml.Enabled = true;
            txtFechaInicio.Focus();
        }

        protected void buscarporfolio_CheckedChanged(object sender, EventArgs e)
        {
            divBuscarporFolio.Visible = true;
            divBuscarporFecha.Visible = false;
            btnGenerarXml.Enabled = true;
            txtXML.Focus();
        }

        protected void btnGenerarXml_Click(object sender, EventArgs e)
        {
            btnVer.Visible = false;

            if (ValidaDatosIngreso())
            {
                if (buscarporfolio.Checked)
                {
                    BuscarPorFolio(txtXML.Text);
                }
                else
                {
                    BuscarPorFecha(txtFechaInicio.Text);
                }

                _mensajeXlm = "";
            }
        }

        protected void gdvArchivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Descargar")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                string fileArchivoName = gdvArchivos.Rows[rowIndex].Cells[1].Text;

                string fileName = "";

                if (Session["Ruta"] != null)
                {
                    fileName = (string)Session["Ruta"] + fileArchivoName;
                }

                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    Response.Clear();
                    Response.HeaderEncoding = System.Text.Encoding.Default;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileArchivoName);
                    Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
                    Response.ContentType = "text/plain";// "Application/msword";
                    Response.TransmitFile(file.FullName);
                    Response.End();
                }
            }
        }

        #endregion

        #region funciones
        
        private List<long[]> WsObtenerDevengo(int folioDevengo, int anio, string tipoDoc, string folio)
        {
            string Rezagadas = System.Configuration.ConfigurationManager.AppSettings["rezagadas"];
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(ValidateServerCertificate);
            bool encontrado = false;
            devengo servicio = new devengo();
            SolicitudDeObtencionDeDevengoMsgType solicitud = new SolicitudDeObtencionDeDevengoMsgType();

            List<long[]> listaCorrelativos = new List<long[]>();
            solicitud.partida = "16";
            solicitud.capitulo = "04";
            solicitud.areaTransaccional = "001";
            solicitud.ejercicio = anio;
            solicitud.folio = folioDevengo;

            try
            {

                RespuestaObtencionDeDevengoMsgType respuesta = servicio.obtenerDevengo(solicitud);

                if (respuesta != null)
                {
                    long cuenta = 0;
                    if (respuesta.devengo.titulo.Equals(Rezagadas))
                    {
                        cuenta = 1210;
                    }

                    foreach (var doc in respuesta.devengo.documentos)
                        //hay que recorrer para el caso de traspasos de años anteriores
                    {
                        string tipoDocSigfe = "";

                        switch (doc.tipo)
                        {
                            case "0201": //fe
                                tipoDocSigfe = "E";
                                break;

                            case "0101": //fa
                                tipoDocSigfe = "F";
                                break;

                            case "0900": //boleta
                                tipoDocSigfe = "B";
                                break;
                        }

                        if (tipoDocSigfe == tipoDoc && doc.numero == folio) //si es el mismo documento
                        {
                            foreach (var pri in doc.principales)
                            {
                                foreach (var tra in pri.transaccionesPrevias)
                                {
                                    long[] dev = new long[4];

                                    dev[0] = doc.idDocumentoAjustado; //correlativo de documento
                                    dev[1] = tra.folioCombinacion;
                                        //folio a buscar en nivel 2x para saber el departamento y seccion
                                  //  dev[2] = tra.agrupacionesDeImputacionesACatalogos.folioAgrupacionDeReferencia;
                                        //correlativo requeremiento compromiso
                                    dev[3] = cuenta;

                                    listaCorrelativos.Add(dev);
                                    encontrado = true;

                                }
                            }
                        }
                        if (encontrado)
                        {
                            break;
                        }

                    }
                }

            }
            catch (Exception)
            {
              
            }

            return listaCorrelativos;
        }

        private string VerificarLargo(string valor)
        {
            string valor2 = "";

            if(valor.Length < 7)
            {
                int total = 0;
                total = 7 - valor.Length;

                for (int i = 0; total > i; i++)
                {
                    valor2 = valor2 + "0";
                }

                valor2 = valor2 + valor;

                return valor2;
            }
            
            return valor;
        }

        private bool IsDate(string fecha)
        {
            try
            {
                DateTime.Parse(fecha);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public void Download(string path)
        {
            FileInfo toDownload = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(path));

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
            System.Web.HttpContext.Current.Response.AddHeader("Content-Length", toDownload.Length.ToString(CultureInfo.InvariantCulture));
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.WriteFile(path);
            System.Web.HttpContext.Current.Response.End();
        }

        private string QuitarCaracteres(string valor)
        {
            string valor2 = "";
            for (int i = 0; valor.Length > i; i++)
            {
                if (valor[i] == '.' || valor[i] == ',')
                {
                    return valor2;
                }
                else
                {
                    valor2 = valor2 + valor[i];
                }
            }

            return valor2;
        }

        public string BuscarCuentaContable(string cuenta)
        {
            DataSet ds = new DataSet();
            string cuentaContable = "";

            if (cuenta != "")
            {
                ds = _negocio.BuscarCuentaContable(cuenta);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cuentaContable = ds.Tables[0].Rows[0]["codigo"].ToString();
                }
            }
            return cuentaContable;
        }

        public static bool ValidaRut(string rut, string dv)
        {
            try
            {
                rut = rut.Replace(".", "");
                rut = rut.Replace(",", "");
                rut = rut.Replace("-", "");
                rut = rut.Replace(" ", "");
                int rutverif = int.Parse(rut);
                int contador = 2;
                int acumulador = 0;

                while (rutverif != 0)
                {
                    int multiplo = (rutverif % 10) * contador;
                    acumulador = acumulador + multiplo;
                    rutverif = rutverif / 10;
                    contador = contador + 1;

                    if (contador == 8)
                    {
                        contador = 2;
                    }
                }

                int digito = 11 - (acumulador % 11);
                string rutDigito = digito.ToString(CultureInfo.InvariantCulture).Trim();

                if (digito == 10)
                {
                    rutDigito = "K";
                }

                if (digito == 11)
                {
                    rutDigito = "0";
                }

                if (rutDigito.ToUpper() != dv.ToUpper().Trim())
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Escribeylee(string error, string fileName)
        {

            string directorio = System.Web.HttpContext.Current.Server.MapPath(@"Log\");

            if (File.Exists(directorio + fileName))
            {
                _sw = File.AppendText(directorio + fileName);
                _sw.Write(error);
                _sw.Write("-------------------");
                _sw.WriteLine();
            }
            else
            {
                _sw = File.CreateText(directorio + fileName);

                _sw.Write(error);
                _sw.Write("------------------- ");
                _sw.WriteLine();
            }
            _sw.Close();
        }

        private void EliminarArchivoTxt()
        {
            if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(@"Log\ErroresXML.txt")))
            {
                try
                {
                    File.Delete(System.Web.HttpContext.Current.Server.MapPath(@"Log\ErroresXML.txt"));
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private string CambiaFormatoFecha(string fecha)
        {
            string fechaNueva = "";
            if (fecha != "")
            {
                fechaNueva = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(fecha));
            }
            return fechaNueva;
        }

        //valida los certificados web (los deja en true siempre)
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion
    }
}