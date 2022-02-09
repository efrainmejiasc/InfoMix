using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using RegistroTransacciones.t03.Estado;
using RegistroTransacciones.t03.contabilidad;
using Negocio;
using System.Data;
using Datos;
using CabeceraTransaccion = RegistroTransacciones.t03.contabilidad.CabeceraTransaccion;
using IdentificacionInstitucion = RegistroTransacciones.t03.contabilidad.IdentificacionInstitucion;
using TicketDeAtencionMsgType = RegistroTransacciones.t03.contabilidad.TicketDeAtencionMsgType;
using System.Threading;


namespace RegistroTransacciones
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                txtEjercicio.Text = DateTime.Now.Year.ToString();
                txtEjercicio.Enabled = DateTime.Now.Month == 1;
                ddlPeriodo.SelectedValue = string.Format("{0:MM}", DateTime.Now);
                CalendarExtender4.EndDate = DateTime.Now;
            }

           
            lblMensajeError.Visible = false;
            //btnVer.Visible = false;
            DivMensajeOk.Visible = false;
        }

        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private List<decimal> AgregarCuentas(RespuestaObtencionDeTransaccionContableMsgType respuesta, string tipo_doc, string folio, bool electronica)
        {
            List<decimal> lista = new List<decimal>();

            switch (tipo_doc)
            {

                case "E": //fe
                    tipo_doc = "0201";
                    break;

                case "F": //fa
                    tipo_doc = "0101";
                    break;

                case "CRP":

                    tipo_doc = "0900";
                    if (electronica)
                    {
                        tipo_doc = "6060";
                    }
                    
                    break;
            }


            foreach (var t in respuesta.transaccionContabilidad.agrupacionesDeImputacionesACuentasContables)
            {
                foreach (ImputacionACuentaContable i in t.imputacionesACuentasContables)
                {
                    foreach (CarteraFinanciera cartera in i.carterasFinancieras)
                    {
                        if (tipo_doc == cartera.codigoTipoDocumento && folio == cartera.numeroDocumento)
                        {

                            foreach (CumplimientoDeCartera cumplimientos in cartera.cumplimientosDeCartera)
                            {
                                foreach (
                                    AgrupacionDeConceptosDeContabilidad agrupa in
                                        cumplimientos.agrupacionesDeConceptosDeContabilidad)
                                    //se repite tantas veces como cuentas tenga 1+
                                {
                                    lista.Add(agrupa.debe);
                                }
                            }

                            return lista;
                        }
                    }
                }
            }
            return lista;
        }

        private void WsEstadoCarga(Dto_DetRecau dto, DateTime fechaLog)
        {
            ServicePointManager.ServerCertificateValidationCallback =
               new RemoteCertificateValidationCallback(ValidateServerCertificate);
            var estado = new SolicitudDeEstadoDeTransaccionesMsgType();
            var servicio = new EstadoDeTransaccionBindingQSService();
            var respuesta = new RespuestaDeEstadoDeTransaccionesMsgType();
            Bsn_DetRecau bsn = new Bsn_DetRecau();
            
            estado.areaTransaccional = "001";
            estado.partida = "16";
            estado.ejercicio = int.Parse(txtEjercicio.Text);
            estado.capitulo = "04";
            estado.codigoTicket = dto.Nro_Ticket;
            dto.FechaProce = string.Format("{0:yyyy-MM-dd H:mm:ss}", DateTime.Now);

            try
            {
                respuesta = servicio.obtenerEstadoDeTransacciones(estado);

                if (respuesta != null)
                {
                    dto.Estado = respuesta.estado;

                    if (dto.Estado == "FINALIZADO_SIN_ERRORES" 
                        || dto.Estado == "EN_PROCESO" 
                        || dto.Estado == "RECEPCIONADO")//si se registra correctamente, consultar a la tabla de control
                    {
                        
                        dto.Procesado = "1";
                        if (dto.Estado == "EN_PROCESO" || dto.Estado == "RECEPCIONADO")
                        {
                            Escribeylee("Cobro en proceso, consulte por ticket N°: " + dto.Nro_Ticket, fechaLog);
                        }
                        else
                        {
                            Escribeylee("Cobro registrado correctamente, N° Ticket: " + dto.Nro_Ticket, fechaLog); 
                        }
                        
                        try
                        {
                            if (ValidaEstado(dto) == string.Empty)
                            {
                                bsn.InsertarProcesados(dto);
                            }
                            else
                            {
                                bsn.ActualizarProcesados(dto);
                            }
                        }
                        catch (Exception)
                        {
                            Escribeylee("error al guardar en procesados", fechaLog);
                        }

                    }
                    else
                    {
                        dto.Estado = "FINALIZADO_CON_ERRORES";
                        Escribeylee(String.Format("Error: Error al registrar la transacción con código de ticket {0}", dto.Nro_Ticket), fechaLog);
                        foreach (EstadoDeTransaccion est in respuesta.detalles)
                        {
                            foreach (DetalleFalla detf in est.errores)
                            {
                                Escribeylee("Detalle: " + detf.descripcion, fechaLog);
                            }
                        }
                        dto.Procesado = "0";
                        if (ValidaEstado(dto) == string.Empty)
                        {
                            bsn.InsertarProcesados(dto);
                        }
                        else
                        {
                            bsn.ActualizarProcesados(dto);
                        }
                    }
                }
                else
                {
                    dto.Estado = "FINALIZADO_CON_ERRORES";
                    dto.Procesado = "0";
                    if (ValidaEstado(dto) == string.Empty)
                    {
                        bsn.InsertarProcesados(dto);
                    }
                    else
                    {
                        bsn.ActualizarProcesados(dto);
                    }
                    Escribeylee(String.Format("Error: Error al registrar la transacción con código de ticket {0}", dto.Nro_Ticket), fechaLog);
                }
            }
            catch (Exception ex)
            {
                dto.Estado = "FINALIZADO_CON_ERRORES";
                dto.Procesado = "0";
                if (ValidaEstado(dto) == string.Empty)
                {
                    bsn.InsertarProcesados(dto);
                }
                else
                {
                    bsn.ActualizarProcesados(dto);
                }
                Escribeylee(String.Format("Error: Error al registrar la transacción con código de ticket {0}", dto.Nro_Ticket), fechaLog);
            }
        }

        private RespuestaObtencionDeTransaccionContableMsgType BuscarTransaccion(int folioSigfe)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(ValidateServerCertificate);

            var solicitud = new SolicitudDeObtencionDeTransaccionContableMsgType();
            var respuesta = new RespuestaObtencionDeTransaccionContableMsgType();
            contabilidad servicio = new contabilidad();

            solicitud.partida = "16";
            solicitud.capitulo = "04";
            solicitud.areaTransaccional = "001";
           
            solicitud.ejercicio = int.Parse(txtEjercicio.Text);
            solicitud.folio = folioSigfe;

            respuesta = servicio.obtenerTransaccionContable(solicitud);

            return respuesta;

        }

        private List<string> BuscarTransaccion(int folioSigfe, string tipo_doc, string folio)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(ValidateServerCertificate);

            var solicitud = new SolicitudDeObtencionDeTransaccionContableMsgType();
            var respuesta = new RespuestaObtencionDeTransaccionContableMsgType();
            contabilidad servicio = new contabilidad();
            List<string> lista = new List<string>();

            solicitud.partida = "16";
            solicitud.capitulo = "04";
            solicitud.areaTransaccional = "001";
            solicitud.ejercicio = int.Parse(txtEjercicio.Text);
            solicitud.folio = folioSigfe;

            respuesta = servicio.obtenerTransaccionContable(solicitud);

            if (respuesta != null)
            {
                switch (tipo_doc)
                {

                    case "E": //fe
                        tipo_doc = "0201";
                        break;

                    case "F": //fa
                        tipo_doc = "0101";
                        break;

                    case "CRP": //boleta
                        tipo_doc = "0900";
                        break;
                }

                //if (respuesta.transaccionContabilidad.descripcion == "Traspaso de devengo anterior a ejercicio 2015")
                //{
                //    return null;
                //}

                foreach (TransaccionContabilidadAgrupacion t  in respuesta.transaccionContabilidad.agrupacionesDeImputacionesACuentasContables)
                {
                    
                    foreach (ImputacionACuentaContable i in t.imputacionesACuentasContables)
                    {
                        string cuenta = "";
                        cuenta = i.codigoCuenta;
                       
                            foreach (CarteraFinanciera cartera in i.carterasFinancieras)
                            {
                                if (tipo_doc == cartera.codigoTipoDocumento && folio == cartera.numeroDocumento)
                                {
                                    foreach (CumplimientoDeCartera cumplimiento in cartera.cumplimientosDeCartera)
                                    {
                                        foreach (
                                            AgrupacionDeConceptosDeContabilidad agrupa in
                                                cumplimiento.agrupacionesDeConceptosDeContabilidad)
                                        {
                                            lista.Add(cuenta);
                                            lista.Add(agrupa.idAgrupacionRelacionada.ToString());
                                            return lista;                                           

                                        }
                                    }
                                }
                            }                       
                    }
                }
            }

            return lista;
        }

        private void RealizarCobro(Dto_DetRecau dto, DateTime fechaLog)
        {
            bool esValido = true;
            var bsn = new Bsn_DetRecau();
            DateTime fechaControl = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["controlBoletas"]);
            DateTime fechaEmision = DateTime.Parse(dto.Fecha_regi);

            if (dto.Tipo_Doc == "CRP" && (fechaEmision >= fechaControl))//si la fecha de emision es mayor igual a la fecha de control, son documentos electronicos
            {
                dto.Electronica = true;

                dto.Boleta = bsn.BuscarCmrcBoleta(dto.Folio);
            }

            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(ValidateServerCertificate);

            TicketDeAtencionMsgType ticket = new TicketDeAtencionMsgType();

            SolicitudDeRegistroDeTransaccionesContablesMsgType registro =
                new SolicitudDeRegistroDeTransaccionesContablesMsgType();
            registro.cabecera = new CabeceraTransaccion();

            CabeceraTransaccion cabecera = registro.cabecera;

            registro.detalles = new TransaccionContabilidad[1]; //siempre se registrará de a una transacción

            #region cabecera

            cabecera.ejercicio = int.Parse(txtEjercicio.Text);
            cabecera.periodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            cabecera.institucion = new IdentificacionInstitucion();

            cabecera.institucion.partida = "16"; //valor fijo
            cabecera.institucion.capitulo = "04"; //valor fijo
            cabecera.institucion.areaTransaccional = "001"; //valor fijo 
            cabecera.proceso = "05"; //fijo

            #endregion

            #region transaccion

            TransaccionContabilidad[] transaccionContabilidad =
                new TransaccionContabilidad[1]; //puede tener mas de una transaccion, nosotros usaremos siempre una

            transaccionContabilidad[0] = new TransaccionContabilidad();

            transaccionContabilidad[0].descripcion = String.Format("WS_Cobro_ISP_{0}_{1}.xml", dto.Electronica ? "BEE" : dto.Tipo_Doc, dto.Folio);
          
            transaccionContabilidad[0].id = dto.Indice;//id del asiento en el sistema vertical
            transaccionContabilidad[0].informacionDeTransaccion =
                new TransaccionContabilidadInformacionDeTransaccion();
            TransaccionContabilidadInformacionDeTransaccion informacion =
                transaccionContabilidad[0].informacionDeTransaccion;
            var informacionCreacion =
                new TransaccionContabilidadInformacionDeTransaccionInformacionDeCreacion();

            informacionCreacion.procesoFuente = "2"; //2._ tesoreria 5._ contabilidad
            informacionCreacion.tipoMovimiento = "2"; // 1._ economico 2._ financiero
            informacionCreacion.tipoOperacion = "0101"; //valor fijo si es tesoreria

            string TipoDoc = "";
            if (dto.Tipo_Doc == "CRP")
            {
                TipoDoc = "Boleta";

                if (dto.Electronica)
                {
                    TipoDoc = "Boleta_Electronica";
                }
            }
            else if (dto.Tipo_Doc == "E")
            {
                TipoDoc = "Factura_Exenta";
            }
            else if (dto.Tipo_Doc == "F")
            {
                TipoDoc = "Factura_Afecta";
            }

            string Caja = "";

            if (dto.Cod_caja == 1)
            {
                Caja = "C1";
            }
            else if (dto.Cod_caja == 2)
            {
                Caja = "C2";
            }
            else if (dto.Cod_caja == 3)
            {
                Caja = "FW";
            }

            transaccionContabilidad[0].titulo = String.Format("{0}_{1}_{2}_{3}", Caja,
                                                              string.Format("{0:dd-MM-yyyy}",
                                                              DateTime.Parse(dto.Fecha_regi)), dto.Electronica ? "BEE" : TipoDoc,
                                                              dto.Folio); //fecha, factura web , folio

            informacion.Item = informacionCreacion;

            #endregion

            

            TransaccionContabilidadAgrupacion[] transaccion =
                new TransaccionContabilidadAgrupacion[1];

            /***********se repite tantas veces como folios vengan en el pago, se hará de una transacción*********/
            transaccion[0] = new TransaccionContabilidadAgrupacion();
            /****************************************************************************************************/

            //dto.Tipo_Doc = dsCorrel.Tables[0].Rows[0]["tp_docpaga"].ToString();//tipo de documento
            //dto.Folio = int.Parse(dsCorrel.Tables[0].Rows[0]["folio_docpaga"].ToString());//folio de la factura
            //dto.Valor_docpaga = decimal.Parse(QuitarCaracteres(dsCorrel.Tables[0].Rows[i]["valor_docpaga"].ToString()));//valor pagado

            #region Folio Sigfe

            DataSet dsSigfe = new DataSet();

            try
            {
                dsSigfe = bsn.BuscarFolioSigfe(dto.Tipo_Doc == "CRP" ? "B" : dto.Tipo_Doc, dto.Folio);

                if (dsSigfe == null || dsSigfe.Tables.Count == 0 && dsSigfe.Tables[0].Rows.Count == 0)
                    //si no encuentra datos, es por que esta registrado como documento de negocio
                {
                    dsSigfe = bsn.BuscarFolioSigfe("N", dto.Folio);
                }

                if (dsSigfe != null && dsSigfe.Tables.Count > 0 && dsSigfe.Tables[0].Rows.Count > 0)
                {
                    if (dsSigfe.Tables[0].Rows[0][0].ToString() != string.Empty)
                    {
                        dto.Folio_Sigfe = int.Parse(dsSigfe.Tables[0].Rows[0][0].ToString());
                        if (dsSigfe.Tables[0].Rows[0][1].ToString() != string.Empty)
                        {
                            dto.FechaCumplimiento = DateTime.Parse(dsSigfe.Tables[0].Rows[0][1].ToString());
                        }
                        else
                        {
                            esValido = false;
                            Escribeylee("Error: No se encontro la fecha de cumplimiento", fechaLog);
                        }
                    }
                    else
                    {
                        esValido = false;
                        Escribeylee("Error: No se ha cargado el devengo o hay problemas con el sincronizador", fechaLog);
                    }
                }
                else
                {
                    esValido = false;
                    Escribeylee("Error: No se encontro el folio Sigfe", fechaLog);
                }
            }
            catch (Exception)
            {
                esValido = false;
                Escribeylee("Error: Problemas al buscar el folio Sigfe", fechaLog);

            }

            #endregion

            Bsn_Docupre bsnDocupre = new Bsn_Docupre();
            DataSet dsDocupre = new DataSet();

            try
            {
                dsDocupre = bsnDocupre.BuscarDocupre(dto.Folio, dto.Tipo_Doc);
            }
            catch (Exception)
            {
                esValido = false;
                Escribeylee("Error: no se encontraron las cuentas", fechaLog);
            }

            if (dsDocupre != null && dsDocupre.Tables[0].Rows.Count > 0)
            {
                DataSet dsMedioPago = new DataSet();

                try
                {
                    dsMedioPago = bsn.BuscarMediosPago(dto); //buscar todos los medios de pago
                }
                catch (Exception)
                {
                    esValido = false;
                    Escribeylee("Error: No se encontro el monto del medio de pago", fechaLog);
                }

                transaccionContabilidad[0].agrupacionesDeImputacionesACuentasContables =
                    new TransaccionContabilidadAgrupacion[1];

                ImputacionACuentaContable[] imputacion =
                    new ImputacionACuentaContable[(dsDocupre.Tables[0].Rows.Count) + (dsMedioPago.Tables[0].Rows.Count)];

                /*
                *mínimo son 2, 
                * la primera registra el detalle de la/las cuentas (puede ser más de 1)
                * se le suma la cantidad de cuentas                 
                */

                #region Cuentas

                if (dsMedioPago != null && dsMedioPago.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsMedioPago.Tables[0].Rows.Count; j++)
                    {
                        CarteraBancaria[] carteraBancaria = new CarteraBancaria[1];
                        carteraBancaria[0] = new CarteraBancaria();
                        imputacion[j] = new ImputacionACuentaContable();

                        dto.Tp_medpago = dsMedioPago.Tables[0].Rows[j]["tp_medpago"].ToString();
                        decimal valormediopago = decimal.Parse(dsMedioPago.Tables[0].Rows[j]["valor_medpago"].ToString());
                        dto.Valor_Medpago = DistrubuirCuentas(dto.Cod_caja, dto.Fecha_regi, dto.Nro_correl,
                                                              valormediopago, dto.Folio); //calcular porcentaje//

                        if (dto.Tp_medpago == "CHD") //cheque
                        {
                            dto.Tp_medpago = "Deposito Documento";
                            carteraBancaria[0].cuentaBancaria = "1";
                            carteraBancaria[0].medioDePago = dto.Tp_medpago;
                            carteraBancaria[0].rutBanco = "66666666-6";
                            imputacion[j].codigoCuenta = "11101";
                        }
                        else if (dto.Tp_medpago == "EFE") //efectivo
                        {
                            dto.Tp_medpago = "Efectivo";
                            carteraBancaria[0].cuentaBancaria = "1";
                            carteraBancaria[0].medioDePago = dto.Tp_medpago;
                            carteraBancaria[0].rutBanco = "66666666-6";
                            imputacion[j].codigoCuenta = "11101";
                        }
                        else if (dto.Tp_medpago == "VV") //vale vista
                        {
                            dto.Tp_medpago = "Efectivo"; //no esta la opción de vale vista
                            //carteraBancaria[0].cuentaBancaria = "9507256";
                            carteraBancaria[0].cuentaBancaria = "1";
                            carteraBancaria[0].medioDePago = dto.Tp_medpago;
                            carteraBancaria[0].rutBanco = "66666666-6";
                            imputacion[j].codigoCuenta = "11101";
                        }
                        else if (dto.Tp_medpago == "PPI") //transferencia
                        {
                            dto.Tp_medpago = "TEF";
                            carteraBancaria[0].cuentaBancaria = "00000000100"; //nueva cuenta 2015
                            carteraBancaria[0].medioDePago = dto.Tp_medpago;
                            carteraBancaria[0].rutBanco = "66666666-6";
                            imputacion[j].codigoCuenta = "11902";
                        }
                        else if (dto.Tp_medpago == "DBA") //deposito bancario
                        {
                            dto.Tp_medpago = "Deposito Efectivo";
                            carteraBancaria[0].cuentaBancaria = "00009507256"; //cuenta nueva
                            carteraBancaria[0].medioDePago = dto.Tp_medpago;
                            carteraBancaria[0].rutBanco = "97030000-7";
                            imputacion[j].codigoCuenta = "11102";
                        }
                        else if (dto.Tp_medpago == "AJU")
                        {
                            Escribeylee(
                                "Error: Los ajustes no son considerados por la aplicación, cobro no registrado",
                                fechaLog);
                            esValido = false;
                        }

                        carteraBancaria[0].debe = dto.Valor_Medpago;
                            //dto.Valor_docpaga; //monto total debitado de la cuenta
                        carteraBancaria[0].fechaDocumento = DateTime.Parse(dto.Fecha_regi); //AAAA-MM-DD
                        carteraBancaria[0].folioContabilidad = dto.Folio_Sigfe;
                        carteraBancaria[0].folioContabilidadSpecified = true;
                        carteraBancaria[0].haber = 0; //Monto acreditado en la cuenta

                        carteraBancaria[0].numeroDocumento = dto.Electronica ? dto.Boleta.ToString() : dto.Folio.ToString();

                        imputacion[j].montoDebe = dto.Valor_Medpago;
                        imputacion[j].montoHaber = 0;
                        imputacion[j].carterasBancarias = carteraBancaria;
                    }
                }
                else
                {
                    esValido = false;
                    Escribeylee("Error: No se encontro el monto del medio de pago", fechaLog);
                }

                #endregion

                /*********ciclo que se repetira tantas veces como cuentas tenga la factura*********/
                if (dsDocupre.Tables[0].Rows.Count > 0)
                {
                    #region Recorre respuesta WS

                    int k = 0;
                    RespuestaObtencionDeTransaccionContableMsgType transacciones =
                        new RespuestaObtencionDeTransaccionContableMsgType();
                    List<decimal> lista = new List<decimal>();
                    try
                    {
                        if (dto.Folio_Sigfe != -1)
                        {
                            transacciones = BuscarTransaccion(dto.Folio_Sigfe);

                            if (transacciones == null)
                            {
                                esValido = false;
                                Escribeylee("Error: No se encontraron los datos del documento en Sigfe", fechaLog);
                            }
                            else
                            {
                                lista = AgregarCuentas(transacciones, dto.Tipo_Doc, dto.Folio.ToString(), dto.Electronica);
                                if (lista.Count == 0) //si no encuentra nada es por que no se sincronizo el folio
                                {
                                    esValido = false;
                                    Escribeylee("Error: No se encontraron los datos del documento en Sigfe", fechaLog);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Escribeylee("Sin respuesta del WS para buscar transacción", fechaLog);
                        esValido = false;
                    }
                    
                    string doc = "";
                    switch (dto.Tipo_Doc)
                    {

                        case "E": //fe
                            doc = "0201";
                            break;

                        case "F": //fa
                            doc = "0101";
                            break;

                        case "CRP": //boleta

                            doc = "0900";

                            if (dto.Electronica)
                            {
                                doc = "6060";
                            }
                            
                            break;
                    }

                    bool generado = false;

                    if (esValido)
                    {
                        foreach (
                            var t in transacciones.transaccionContabilidad.agrupacionesDeImputacionesACuentasContables)
                        {
                            foreach (ImputacionACuentaContable i in t.imputacionesACuentasContables)
                            {
                                Dto_DocuPre dtoDocuPre = new Dto_DocuPre();
                                List<decimal> porcentajedebe = CalcularPorcentaje(lista, i.montoDebe, dto.Valor_docpaga);

                                dtoDocuPre.Cuenta = i.codigoCuenta;

                                foreach (CarteraFinanciera cartera in i.carterasFinancieras)
                                {
                                    if (generado)
                                    {
                                        break;
                                    }

                                    if (doc == cartera.codigoTipoDocumento &&
                                        dto.Folio.ToString() == cartera.numeroDocumento)
                                    {
                                        foreach (CumplimientoDeCartera cumplimientos in cartera.cumplimientosDeCartera)
                                        {
                                            foreach (
                                                AgrupacionDeConceptosDeContabilidad agrupa in
                                                    cumplimientos.agrupacionesDeConceptosDeContabilidad)
                                                //se repite tantas veces como cuentas tenga 1+
                                            {
                                                #region completar datos

                                                dto.Id_Agrupacion = int.Parse(agrupa.idAgrupacionRelacionada.ToString());


                                                dtoDocuPre.Monto = porcentajedebe.Count == 0 ? 0: porcentajedebe[k];
                                                //decimal.Parse(dsDocupre.Tables[0].Rows[k]["monto"].ToString());
                                                dtoDocuPre.Rut_Fac = dsDocupre.Tables[0].Rows[k][1].ToString();
                                                try
                                                {
                                                    string rut = BuscarCliente(dto.Folio, dto.Tipo_Doc);


                                                    if (rut == string.Empty)
                                                    {
                                                        esValido = false;
                                                        Escribeylee("Error: No se encontro el cliente para la cuenta " +
                                                                    dtoDocuPre.Cuenta, fechaLog);
                                                    }
                                                    else
                                                    {
                                                        dtoDocuPre.Rut_Cliente = rut;
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    esValido = false;
                                                    Escribeylee("Error: No se encontro el cliente para la cuenta " +
                                                                dtoDocuPre.Cuenta, fechaLog);
                                                }

                                                CarteraFinanciera[] carteraFinanciera = new CarteraFinanciera[1];
                                                carteraFinanciera[0] = new CarteraFinanciera();

                                                CumplimientoDeCartera[] cumplimiento = new CumplimientoDeCartera[1];
                                                cumplimiento[0] = new CumplimientoDeCartera();
                                                carteraFinanciera[0].cumplimientosDeCartera = cumplimiento;

                                                AgrupacionDeConceptosDeContabilidad[] agrupacion =
                                                    new AgrupacionDeConceptosDeContabilidad[1];
                                                agrupacion[0] = new AgrupacionDeConceptosDeContabilidad();
                                                cumplimiento[0].agrupacionesDeConceptosDeContabilidad = agrupacion;

                                                agrupacion[0].debe = 0;
                                                agrupacion[0].haber = dtoDocuPre.Monto;
                                                agrupacion[0].monedaPresupuestaria = MonedaPresupuestaria.NACIONAL;

                                                agrupacion[0].idAgrupacionRelacionada = dto.Id_Agrupacion;
                                                agrupacion[0].idAgrupacionRelacionadaSpecified = true;

                                                cumplimiento[0].debe = 0;
                                                cumplimiento[0].fecha = dto.FechaCumplimiento; //AAAA-MM-DD
                                                cumplimiento[0].haber = dtoDocuPre.Monto;
                                                cumplimiento[0].rutDestinatario = dtoDocuPre.Rut_Cliente;

                                                carteraFinanciera[0].debe = 0;
                                                carteraFinanciera[0].fechaDocumento = DateTime.Parse(dto.Fecha_regi);
                                                //AAAA-MM-DD
                                                carteraFinanciera[0].folioContabilidad = Convert.ToInt64(dto.Folio_Sigfe.ToString());
                                                carteraFinanciera[0].folioContabilidadSpecified = true;
                                                carteraFinanciera[0].haber = dtoDocuPre.Monto; //valor de la cuenta
                                                carteraFinanciera[0].rut = dtoDocuPre.Rut_Cliente;

                                                imputacion[k + dsMedioPago.Tables[0].Rows.Count] =
                                                    new ImputacionACuentaContable();
                                                //es + 1 por que la 0 es el detalle

                                                imputacion[k + dsMedioPago.Tables[0].Rows.Count].codigoCuenta =
                                                    dtoDocuPre.Cuenta;
                                                imputacion[k + dsMedioPago.Tables[0].Rows.Count].montoDebe = 0;
                                                imputacion[k + dsMedioPago.Tables[0].Rows.Count].montoHaber =
                                                    dtoDocuPre.Monto;

                                                imputacion[k + dsMedioPago.Tables[0].Rows.Count].carterasFinancieras =
                                                    carteraFinanciera;

                                                #endregion

                                                k++;
                                            }

                                            generado = true;
                                        }
                                    }
                                }
                            }
                        } //
                    }
                    //}

                    #endregion
                }

                /*************************tag obligatorio, va vacio *******************/

                ImputacionACatalogoPropioDeContabilidad[] catalogoPropio =
                    new ImputacionACatalogoPropioDeContabilidad[1];
                catalogoPropio[0] = new ImputacionACatalogoPropioDeContabilidad();

                transaccion[0].imputacionesACatalogosPropios = catalogoPropio;

                /**********************************************************************/

                transaccion[0].imputacionesACuentasContables = imputacion;
            }
            else
            {
                esValido = false;
                Escribeylee("Error: No se encontraron cuentas", fechaLog);
            }

            transaccionContabilidad[0].agrupacionesDeImputacionesACuentasContables = transaccion;

            registro.detalles = transaccionContabilidad;

            if (esValido)
            {
                contabilidad servicio = new contabilidad();

                ticket = servicio.registrarTransaccionesContables(registro);
                //retorna el numero de ticket en el caso que sea correcta la transacción

                if (ticket != null)
                {
                    dto.Nro_Ticket = ticket.codigo;
                    try
                    {
                        Thread.Sleep(60000); //espera 60 segundos
                        WsEstadoCarga(dto, fechaLog);

                    }
                    catch (Exception)
                    {
                        Escribeylee("Error: No se registro el cobro", fechaLog);
                    }
                }
                else //si el ticket viene vacio es por que hay un error en el xml enviado
                {
                    Escribeylee("Error: No se pudo enviar el cobro.", fechaLog);
                }
            }
            else
            {
                dto.Estado = "FINALIZADO_CON_ERRORES";
                dto.Procesado = "0";
                //Escribeylee("Error: No se pueden registrar cobros del año anterior");
                if (ValidaEstado(dto) == string.Empty)
                {
                    bsn.InsertarProcesados(dto);
                }
                else
                {
                    bsn.ActualizarProcesados(dto);
                }
            }
        }

        private string BuscarCliente(int folio, string tipodoc)
        {
            DataSet ds = new DataSet();
            Bsn_Docupre bsn = new Bsn_Docupre();
            string rut = "";
            string dv = "";
            string rutfac = "";

            if(tipodoc == "CRP")//boletas
            {
                ds = bsn.BuscarRutBoleta(folio);
            }
            else
            {
                ds = bsn.BuscarRutFactura(folio, tipodoc);
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                rutfac = ds.Tables[0].Rows[0][0].ToString();
            }

            ds = bsn.BuscarRutCliente(rutfac);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                rut = ds.Tables[0].Rows[0][0].ToString();
                dv = ds.Tables[0].Rows[0][1].ToString().ToLower();

                if (rut.Length < 7)
                {
                    while (rut.Length < 7)
                    {
                        rut =  "0" + rut;
                    }
                }

                return string.Format("{0}-{1}", rut, dv);
            }

            return string.Empty;
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtEjercicio.Text = "";
            txtFolio.Text = "";
            txtEjercicio.Text = "";
            ddlCaja.SelectedValue = "0";
            ddlPeriodo.SelectedValue = "0";
            ddlTipoDoc.SelectedValue = "0";
        }

        protected void btnPagar_Click(object sender, EventArgs e)
        {
            if (ValidaDatos())
            {
                //EliminarArchivoTxt();
                DateTime fechaLog = DateTime.Now;
                Session.Add("FechaLog", fechaLog);
                string fecha = txtFecha.Text;
                string folio = txtFolio.Text;
                string TipoDoc = ddlTipoDoc.SelectedValue;
                int caja = int.Parse(ddlCaja.SelectedValue);
                decimal Sumadoc, SumaPagos = 0;
                bool Pec = false;
                Bsn_DetRecau bsn = new Bsn_DetRecau();
                DataSet ds = new DataSet();

                if (rbnselccion.SelectedValue == "0")
                {
                    ds = bsn.BuscarDocumentosDetRecau(fecha, caja); //busca todas las transacciones de la caja
                }
                else //busqueda por folios
                {
                    foreach (var item in folio.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    {
                        DataSet ds2 = new DataSet();

                        for (int i = 1; i <= 3; i++)
                        {

                            ds2 = bsn.BuscarDetRecauFolio(item, TipoDoc, i.ToString());

                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                ds.Merge(ds2);
                                //break;
                            }
                        }
                    }
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        Dto_DetRecau dto = new Dto_DetRecau();

                        if (rbnselccion.SelectedValue == "0")
                        {
                            dto.Cod_caja = int.Parse(ddlCaja.SelectedValue);
                            dto.Fecha_regi = txtFecha.Text;
                            dto.Nro_correl = int.Parse(ds.Tables[0].Rows[j]["nro_correl"].ToString());
                            dto.Tipo_Doc = ds.Tables[0].Rows[j]["tp_docpaga"].ToString(); //tipo de documento
                            dto.Folio = int.Parse(ds.Tables[0].Rows[j]["folio_docpaga"].ToString());
                            dto.Fecha_docpaga = ds.Tables[0].Rows[j]["fecha_docpaga"].ToString();
                            //folio de la factura
                            dto.Valor_docpaga =
                                decimal.Parse(QuitarCaracteres(ds.Tables[0].Rows[j]["valor_docpaga"].ToString()));
                            //valor pagado
                            dto.Indice = ds.Tables[0].Rows[j]["indice"].ToString();
                            dto.Tp_medpago = ds.Tables[0].Rows[j]["tp_medpago"].ToString();
                        }
                        else
                        {
                            dto.Cod_caja = int.Parse(ds.Tables[0].Rows[j]["cod_caja"].ToString());
                            dto.Fecha_regi = string.Format("{0:dd-MM-yyyy}",
                                                           DateTime.Parse(
                                                               ds.Tables[0].Rows[j]["fecha_regi"].ToString()));
                            dto.Nro_correl = int.Parse(ds.Tables[0].Rows[j]["nro_correl"].ToString());
                            dto.Tipo_Doc = ds.Tables[0].Rows[j]["tp_docpaga"].ToString(); //tipo de documento
                            dto.Folio = int.Parse(ds.Tables[0].Rows[j]["folio_docpaga"].ToString());
                            //folio de la factura
                            dto.Valor_docpaga =
                                decimal.Parse(QuitarCaracteres(ds.Tables[0].Rows[j]["valor_docpaga"].ToString()));
                            //valor pagado
                            dto.Indice = ds.Tables[0].Rows[j]["indice"].ToString();
                            dto.Fecha_docpaga = ds.Tables[0].Rows[j]["fecha_docpaga"].ToString();
                            dto.Tp_medpago = ds.Tables[0].Rows[j]["tp_medpago"].ToString();

                            if (DateTime.Parse(dto.Fecha_regi) < DateTime.Parse("01-01-2014"))
                            {
                                lblMensajeError.Visible = true;
                                lblMensajeError.Text =
                                    "El folio ingresado tiene fecha de registro menor a 01-01-2014";
                                break;
                            }
                        }

                        if (j == 0) //validación de los montos, solo se hace la primera vez
                        {
                            EscribeInicio("Inicio Log: ", fechaLog);

                            DataSet ds2 = new DataSet();
                            ds2 = bsn.ValidarTransaccion(fecha, dto.Cod_caja);

                            if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                            {
                                Sumadoc = 0;
                                SumaPagos = 0;
                                try
                                {
                                    Sumadoc = decimal.Parse(ds2.Tables[0].Rows[0]["sumadoc"].ToString());
                                    SumaPagos = decimal.Parse(ds2.Tables[0].Rows[0]["sumapago"].ToString());
                                }
                                catch (Exception ex)
                                {

                                }

                                Escribelinea(fechaLog);
                                if (rbnselccion.SelectedValue == "0")
                                {
                                    Escribeylee("Total Caja " + dto.Cod_caja + ": " + Sumadoc, fechaLog);
                                    Escribelinea(fechaLog);
                                    Escribelinea(fechaLog);
                                }
                            }

                            #region Validar pagos

                            if (fecha == "")
                            {
                                fecha = string.Format("{0:dd-MM-yyyy}", DateTime.Parse(dto.Fecha_regi));
                            }

                            #endregion

                        }

                        if (dto.Tipo_Doc != "SD")
                        {
                            Escribeylee("Factura: " + dto.Tipo_Doc, fechaLog);
                            Escribeylee("Folio: " + dto.Folio, fechaLog);
                            if (dto.Tp_medpago != "AJU")
                            {
                                //bool cobroatrasado = EsAnterior(dto.Tipo_Doc, dto.Folio);

                                //if (!cobroatrasado)
                                //{

                                //string estadotran = ValidaTransaccion(dto);

                                DataSet dspro = bsn.BuscarTicket(dto);

                                if (dspro != null && dspro.Tables[0].Rows.Count > 0)
                                {
                                    dto.Estado = dspro.Tables[0].Rows[0]["estado"].ToString().Trim();
                                    dto.Procesado = dspro.Tables[0].Rows[0]["procesado"].ToString().Trim();
                                    if (dspro.Tables[0].Rows[0]["cod_ticket"].ToString().Trim() != "")
                                    {
                                        dto.Nro_Ticket =
                                            long.Parse(dspro.Tables[0].Rows[0]["cod_ticket"].ToString().Trim());
                                    }
                                }

                                if (dto.Estado == "EN_PROCESO")
                                {
                                    Escribeylee("Actualizando estado...", fechaLog);
                                    dto.Estado = WsEstadoCarga2(dto, fechaLog);

                                }

                                if (dto.Estado == "FINALIZADO_CON_ERRORES" ||
                                    dto.Estado == "RECEPCIONADO" ||
                                    dto.Estado == "TERMINADO" ||
                                    dto.Estado == string.Empty)
                                {
                                    RealizarCobro(dto, fechaLog);
                                }
                                else if (dto.Estado == "FINALIZADO_SIN_ERRORES")
                                {
                                    Escribeylee(
                                        "Error: No se puede registrar dos veces el mismo cobro (FINALIZADO SIN ERRORES)",
                                        fechaLog);
                                }
                                Escribelinea(fechaLog);
                            }
                            else
                            {
                                Escribeylee(
                                    "Error: Los ajustes no son considerados por la aplicación, cobro no registrado",
                                    fechaLog);
                                Escribelinea(fechaLog);
                            }
                        }
                    }

                    EscribePie("Fin Log: ", fechaLog);
                    DivMensajeOk.Visible = true;
                }
                else
                {
                    lblMensajeError.Visible = true;
                    lblMensajeError.Text = "No se encontraron cobros pendientes.";
                }
            }
        }

        private decimal DistrubuirCuentas(int caja, string fecha, int correl, decimal valormediopago, int folio)
        {
            Bsn_DetRecau bsn = new Bsn_DetRecau();
            DataSet ds = new DataSet();
            List<decimal> lista = new List<decimal>();
            decimal totalsuma = 0;//suma de los porcentajes
            decimal suma = 0;
            int i = 0;
            int pos = 0;

            ds = bsn.BuscarDetRecau(fecha, caja, correl);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    suma = suma + decimal.Parse(item[9].ToString());//suma total de lo que se esta pagando
                }

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    decimal porcentaje = 0;
                    decimal resultado = 0;
                    decimal valordocpaga = decimal.Parse(item[9].ToString());

                    porcentaje = ((valordocpaga * 100) / suma);//calculo el porcentaje
                    resultado = Math.Round((valormediopago * porcentaje)  / 100);

                    if(int.Parse(item[6].ToString()) == folio)
                    {
                        pos = i;
                    }

                    lista.Add(resultado);
                    totalsuma = totalsuma + resultado;
                    i++;
                }

                if (totalsuma != valormediopago) //si quedaron algunos decimales se redondea le agregan al último monto
                {
                    decimal redondeo = 0;
                    if (totalsuma > valormediopago)
                    {
                        redondeo = totalsuma - valormediopago;
                        lista[lista.Count - 1] = lista[lista.Count - 1] - redondeo;
                    }
                    else
                    {
                        redondeo = valormediopago - totalsuma;
                        lista[lista.Count - 1] = lista[lista.Count - 1] + redondeo;
                    }
                }
            }

            return lista[pos];
        }

        private List<decimal> CalcularPorcentaje(List<decimal> cuentas, decimal debe, decimal paga)
        {
            var totales = new List<decimal>();
            decimal totalsuma = 0;//suma de los porcentajes
            decimal suma = 0;

            try
            {
                foreach (var monto in cuentas)
                {
                    suma = suma + monto;//suma total de lo que se esta pagando
                }

                foreach (var monto in cuentas)
                {

                    decimal porcentaje = 0;
                    decimal resultado = 0;

                    porcentaje = ((monto * 100) / suma);
                    resultado = Math.Round(paga * (porcentaje / 100));

                    totales.Add(resultado);

                    totalsuma = totalsuma + resultado;

                }

                if (totalsuma != paga) //si quedaron algunos decimales se redondea la ultima cuenta
                {
                    decimal redondeo = 0;
                    if (totalsuma > paga)
                    {
                        redondeo = totalsuma - paga;
                        totales[totales.Count - 1] = totales[totales.Count - 1] - redondeo;
                    }
                    else
                    {
                        redondeo = paga - totalsuma;
                        totales[totales.Count - 1] = totales[totales.Count - 1] + redondeo;
                    }
                }
            }
            catch (Exception)
            {
              
            }
            

            return totales;
        }

        private string ValidaTransaccion(Dto_DetRecau dto)
        {
            Bsn_DetRecau bsn = new Bsn_DetRecau();
            DataSet ds = new DataSet();

            ds = bsn.BuscarProcesados(dto);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                //if (ds.Tables[0].Rows[0]["estado"].ToString().Trim() == "FINALIZADO_SIN_ERRORES")
                //{
                ds.Tables[0].Rows[0]["cod_tiket"].ToString().Trim();
                    return ds.Tables[0].Rows[0]["estado"].ToString().Trim();
                //}
            }

            return string.Empty;
        }

        private string ValidaEstado(Dto_DetRecau dto)
        {
            Bsn_DetRecau bsn = new Bsn_DetRecau();
            DataSet ds = new DataSet();

            ds = bsn.BuscarProcesados(dto);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["folio_doc"].ToString().Trim();
            }

            return string.Empty;
        }

        private string QuitarCaracteres(string valor)
        {
            string Valor2 = "";
            for (int i = 0; valor.Length > i; i++)
            {
                if (valor[i] == '.' || valor[i] == ',')
                {
                    return Valor2;
                }
                else
                {
                    Valor2 = Valor2 + valor[i];
                }
            }

            return Valor2;
        }

        public bool ValidaDatos()
        {
            bool esValido = true;
            string codError = null;

            if (txtEjercicio.Text == string.Empty)
            {
                esValido = false;
                txtEjercicio.Focus();
                codError = "err_ejer";
            }
            else if (int.Parse(txtEjercicio.Text) < 2014)
            {
                esValido = false;
                txtEjercicio.Focus();
                codError = "err_ejer2";
            }
            else if (ddlPeriodo.SelectedValue == "0")
            {
                esValido = false;
                ddlPeriodo.Focus();
                codError = "err_per";
            }
            else if (rbnselccion.SelectedValue == "0" && ddlCaja.SelectedValue == "0")
            {
                esValido = false;
                ddlCaja.Focus();
                codError = "err_caj";
            }
            else if (rbnselccion.SelectedValue == "0" && txtFecha.Text == string.Empty)
            {
                esValido = false;
                txtFecha.Focus();
                codError = "err_fec";
            }
            else if (rbnselccion.SelectedValue == "0" && txtFecha.Text != string.Empty)
            {
                if (DateTime.Parse(txtFecha.Text) < DateTime.Parse("01-01-2014"))
                {
                    esValido = false;
                    txtFecha.Focus();
                    codError = "err_fec2";
                }
            }
            else if (rbnselccion.SelectedValue == "1" && txtFolio.Text == string.Empty)
            {
                esValido = false;
                txtFolio.Focus();
                codError = "err_fol";
            }
            else if (rbnselccion.SelectedValue == "1" && ddlTipoDoc.SelectedValue == "0")
            {
                esValido = false;
                txtFolio.Focus();
                codError = "err_doc";
            }

            InterpretarError(codError);
            return esValido;
        }

        private void InterpretarError(string codError)
        {
            string msjeError = null;

            switch (codError)
            {
                case "err_ejer":
                    msjeError = "Debe ingresar Ejercicio.";
                    break;

                case "err_ejer2":
                    msjeError = "El Ejercicio debe ser mayor o igual a 2014.";
                    break;

                case "err_per":
                    msjeError = "Debe seleccionar Periodo.";
                    break;

                case "err_caj":
                    msjeError = "Debe seleccionar Caja.";
                    break;

                case "err_vfec":
                    msjeError = "Fecha inválida.";
                    break;

                case "err_fec":
                    msjeError = "Debe ingresar Fecha.";
                    break;

                case "err_fec2":
                    msjeError = "La fecha debe ser mayor o igual a 01-01-2014.";
                    break;

                case "err_fol":
                    msjeError = "Debe ingresar Folio.";
                    break;

                case "err_doc":
                    msjeError = "Debe seleccionar tipo de documento.";
                    break;
            }

            if (msjeError != null)
            {
                lblMensajeError.Visible = true;
                lblMensajeError.Text = msjeError;
            }
        }

        protected void rbnselccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbnselccion.SelectedValue == "0")
            {
                divfecha.Visible = true;
                divfolio.Visible = false;
                divcaja.Visible = true;
                divTipoDoc.Visible = false;
                txtEjercicio.Text = "";
                ddlPeriodo.SelectedValue = "0";
                ddlCaja.SelectedValue = "0";
                ddlTipoDoc.SelectedValue = "0";
                txtFolio.Text = "";
                txtFecha.Text = "";
                
            }
            else
            {
                divfolio.Visible = true;
                divfecha.Visible = false;
                divcaja.Visible = false;
                divTipoDoc.Visible = true;
                txtEjercicio.Text = "";
                ddlPeriodo.SelectedValue = "0";
                ddlCaja.SelectedValue = "0";
                ddlTipoDoc.SelectedValue = "0";
                txtFolio.Text = "";
                txtFecha.Text = "";
                
            }

            txtEjercicio.Text = DateTime.Now.Year.ToString();
            txtEjercicio.Enabled = DateTime.Now.Month == 1;
            ddlPeriodo.SelectedValue = string.Format("{0:MM}", DateTime.Now);
        }

        private void Escribeylee(string error, DateTime fecha)
        {
            string Directorio = HttpContext.Current.Server.MapPath(@"Log\");
            Dto_Usuario dto = new Dto_Usuario();
            dto = (Dto_Usuario)Session["UsuarioLogueado"];
            string FILE_NAME = String.Format("Log_{0}_{1}.txt", fecha.ToString().Replace(":", "").Replace("/", "-"), dto.Rut);
            
            StreamWriter sw;

            if (File.Exists(Directorio + FILE_NAME))
            {
                sw = File.AppendText(Directorio + FILE_NAME);

                sw.WriteLine(error);
            }
            else
            {
                sw = File.CreateText(Directorio + FILE_NAME);

                sw.WriteLine(error);
            }
            sw.Close();
        }

        private void Escribelinea(DateTime fecha)
        {
            string Directorio = HttpContext.Current.Server.MapPath(@"Log\");
            Dto_Usuario dto = new Dto_Usuario();
            dto = (Dto_Usuario)Session["UsuarioLogueado"];
            string FILE_NAME = String.Format("Log_{0}_{1}.txt", fecha.ToString().Replace(":", "").Replace("/", "-"), dto.Rut);

            StreamWriter sw;

            if (File.Exists(Directorio + FILE_NAME))
            {
                sw = File.AppendText(Directorio + FILE_NAME);

                sw.WriteLine("--------------------------------------------------------");
            }
            else
            {
                sw = File.CreateText(Directorio + FILE_NAME);

                sw.WriteLine("--------------------------------------------------------");
            }
            sw.Close();
        }

        private void EscribeInicio(string texto, DateTime fecha)
        {
           
            string Directorio = HttpContext.Current.Server.MapPath(@"Log\");
            Dto_Usuario dto = new Dto_Usuario();
            dto = (Dto_Usuario)Session["UsuarioLogueado"];   
            string FILE_NAME = String.Format("Log_{0}_{1}.txt", fecha.ToString().Replace(":", "").Replace("/","-"),dto.Rut);

            StreamWriter sw;

            if (File.Exists(Directorio + FILE_NAME))
            {
                sw = File.AppendText(Directorio + FILE_NAME);

                sw.WriteLine("--------------------------------------------------------");
                sw.WriteLine(String.Format(" {0}  : {1}", texto, DateTime.Now));
                sw.WriteLine("--------------------------------------------------------");
            }
            else
            {
                sw = File.CreateText(Directorio + FILE_NAME);

                sw.WriteLine("--------------------------------------------------------");
                sw.WriteLine(String.Format(" {0}  : {1}", texto, DateTime.Now));
                sw.WriteLine("--------------------------------------------------------");
            }
            sw.Close();
        }

        private void EscribePie(string texto, DateTime fecha)
        {
            string Directorio = HttpContext.Current.Server.MapPath(@"Log\");
            Dto_Usuario dto = new Dto_Usuario();
            dto = (Dto_Usuario)Session["UsuarioLogueado"];
            string FILE_NAME = String.Format("Log_{0}_{1}.txt", fecha.ToString().Replace(":", "").Replace("/", "-"), dto.Rut);

            StreamWriter sw;

            if (File.Exists(Directorio + FILE_NAME))
            {
                sw = File.AppendText(Directorio + FILE_NAME);

                sw.WriteLine("--------------------------------------------------------");
                sw.WriteLine(String.Format(" {0}  : {1}", texto, DateTime.Now));
                sw.WriteLine("--------------------------------------------------------");
            }
            else
            {
                sw = File.CreateText(Directorio + FILE_NAME);

                sw.WriteLine("--------------------------------------------------------");
                sw.WriteLine(String.Format(" {0}  : {1}", texto, DateTime.Now));
                sw.WriteLine("--------------------------------------------------------");
            }
            sw.Close();
        }

        private void EliminarArchivoTxt()
        {   // Delete a file by using File class static method...
            string Directorio = HttpContext.Current.Server.MapPath(@"Log\");
            Dto_Usuario dto = new Dto_Usuario();
            dto = (Dto_Usuario)Session["UsuarioLogueado"];   
            string FILE_NAME = String.Format("Log_{0}_{1}.txt", String.Format("{0:dd-MM-yyyy}", DateTime.Now),dto.Rut);

            if (File.Exists(Directorio + FILE_NAME))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    File.Delete(Directorio + FILE_NAME);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fecha = (DateTime)Session["FechaLog"];
                Descargar(fecha);
            }
            catch (Exception)
            {

                lblMensajeError.Visible = true;
                lblMensajeError.Text = "Problemas al tratar de descargar el log.";
            }
           
        }

        private void Descargar(DateTime fecha)
        {
            Dto_Usuario dto = new Dto_Usuario();
            dto = (Dto_Usuario)Session["UsuarioLogueado"];
            string FILE_ARCHIVO_NAME = String.Format("Log_{0}_{1}.txt", fecha.ToString().Replace(":", "").Replace("/", "-"), dto.Rut);
            string FILE_NAME = Server.MapPath(@"Log\") + FILE_ARCHIVO_NAME;

            FileInfo file = new FileInfo(FILE_NAME);
            if (file.Exists)
            {
                Response.Clear();
                Response.HeaderEncoding = System.Text.Encoding.Default;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FILE_ARCHIVO_NAME);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "text/plain";// "Application/msword";
                Response.TransmitFile(file.FullName);
                Response.End();
            }
        }

        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime.Parse(txtFecha.Text);
            }
            catch (Exception)
            {
                txtFecha.Text = "";
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            if (ValidaDatos())
            {
                //EliminarArchivoTxt();
                string fecha = txtFecha.Text;
                string folio = txtFolio.Text;
                DateTime fechaLog = DateTime.Now;
                Session.Add("FechaLog", fechaLog);
                string TipoDoc = ddlTipoDoc.SelectedValue;
                int caja = int.Parse(ddlCaja.SelectedValue);
                
                Bsn_DetRecau bsn = new Bsn_DetRecau();
                DataSet ds = new DataSet();


                
                if (rbnselccion.SelectedValue == "0")
                {
                    ds = bsn.BuscarDocumentosDetRecau(fecha, caja); //busca todas las transacciones de la caja
                }
                else
                {
                    foreach (var item in folio.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    {
                        DataSet ds2 = new DataSet();
                        for (int i = 1; i <= 3; i++)
                        {

                            ds2 = bsn.BuscarDetRecauFolio(item, TipoDoc, i.ToString());

                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                ds.Merge(ds2);
                                break;
                            }
                        }
                    }

                    //for (int i = 1; i <= 3; i++)
                    //{

                    //    ds = bsn.BuscarDetRecauFolio(folio, TipoDoc, i.ToString());

                    //    if (ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        break;
                    //    }
                    //}
                }
               
                if (ds != null && ds.Tables[0].Rows.Count > 0) 
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        
                        Dto_DetRecau dto = new Dto_DetRecau();
                        dto.Indice = ds.Tables[0].Rows[j]["indice"].ToString();

                        if (rbnselccion.SelectedValue == "0")//fecha
                        {
                            dto.Cod_caja = int.Parse(ddlCaja.SelectedValue);
                            dto.Nro_correl = int.Parse(ds.Tables[0].Rows[j]["nro_correl"].ToString());
                            dto.Tipo_Doc = ds.Tables[0].Rows[j]["tp_docpaga"].ToString(); //tipo de documento
                            dto.Folio = int.Parse(ds.Tables[0].Rows[j]["folio_docpaga"].ToString());
                        }
                        else
                        {
                            dto.Cod_caja = int.Parse(ds.Tables[0].Rows[j]["cod_caja"].ToString());
                            dto.Fecha_regi = string.Format("{0:dd-MM-yyyy}",DateTime.Parse(ds.Tables[0].Rows[j]["fecha_regi"].ToString()));
                            dto.Nro_correl = int.Parse(ds.Tables[0].Rows[j]["nro_correl"].ToString());
                            dto.Tipo_Doc = ds.Tables[0].Rows[j]["tp_docpaga"].ToString(); //tipo de documento
                            dto.Folio = int.Parse(ds.Tables[0].Rows[j]["folio_docpaga"].ToString());
                        }

                        if (j == 0)
                        {
                            EscribeInicio("Inicio Log: ", fechaLog);
                        }
                        
                       
                        if (dto.Tipo_Doc != "SD")
                        {
                            DataSet dspro = new DataSet();

                            dspro = bsn.BuscarTicket(dto);
                         

                            if (dspro != null && dspro.Tables[0].Rows.Count > 0)
                            {
                                   
                                    dto.Estado = dspro.Tables[0].Rows[0]["estado"].ToString().Trim(); //CAMBIAR J POR 0
                                    dto.Procesado = dspro.Tables[0].Rows[0]["procesado"].ToString().Trim();

                                    if (dspro.Tables[0].Rows[0]["cod_ticket"].ToString().Trim() != "")
                                    {
                                        dto.Nro_Ticket =
                                            long.Parse(dspro.Tables[0].Rows[0]["cod_ticket"].ToString().Trim());
                                    }

                                    if (dto.Estado != "FINALIZADO_SIN_ERRORES")
                                    {
                                        Escribeylee("Factura: " + dto.Tipo_Doc, fechaLog);
                                        Escribeylee("Folio: " + dto.Folio, fechaLog);
                                        if (dto.Nro_Ticket != -1)
                                        {
                                            WsEstadoCarga2(dto, fechaLog);
                                        }
                                        else
                                        {
                                            dto.Estado = "FINALIZADO_CON_ERRORES";
                                            dto.Procesado = "0";
                                            try
                                            {
                                                bsn.ActualizarProcesados(dto);
                                            }
                                            catch (Exception)
                                            {
                                                Escribeylee(String.Format("Error: Error al actualizar la transacción con código de ticket {0}", dto.Nro_Ticket), fechaLog);
                                               
                                            }

                                            Escribeylee("El cobro no esta registrado..", fechaLog);
                                        }

                                        Escribelinea(fechaLog);
                                    }
                                    //else
                                    //{
                                    //    Escribeylee("El cobro esta registrado sin problemas");   
                                    //}
                              
                            }
                            //else
                            //{
                            //    Escribeylee("El cobro no esta registrado");
                            //}
                            
                            
                        }
                    }

                    EscribePie("Fin Log: ", fechaLog);
                    DivMensajeOk.Visible = true;
                    //btnVer.Visible = true;
                }
                else
                {
                    lblMensajeError.Visible = true;
                    lblMensajeError.Text = "No se encontraron cobros pendientes.";
                }
            }
        }

        private string WsEstadoCarga2(Dto_DetRecau dto, DateTime fechaLog)
        {
            ServicePointManager.ServerCertificateValidationCallback =
               new RemoteCertificateValidationCallback(ValidateServerCertificate);

            var estado = new SolicitudDeEstadoDeTransaccionesMsgType();
            var servicio = new EstadoDeTransaccionBindingQSService();
            var respuesta = new RespuestaDeEstadoDeTransaccionesMsgType();
            Bsn_DetRecau bsn = new Bsn_DetRecau();

            estado.areaTransaccional = "001";
            estado.partida = "16";
            estado.ejercicio = int.Parse(txtEjercicio.Text);
            estado.capitulo = "04";
            estado.codigoTicket = dto.Nro_Ticket;
            dto.FechaProce = string.Format("{0:yyyy-MM-dd H:mm:ss}", DateTime.Now);

            try
            {
                respuesta = servicio.obtenerEstadoDeTransacciones(estado);

                if (respuesta != null)
                {
                    dto.Estado = respuesta.estado;

                    if (dto.Estado == "FINALIZADO_SIN_ERRORES"
                        || dto.Estado == "EN_PROCESO"
                        || dto.Estado == "RECEPCIONADO")//si se registra correctamente, consultar a la tabla de control
                    {

                        dto.Procesado = "1";
                        if (dto.Estado == "EN_PROCESO" || dto.Estado == "RECEPCIONADO")
                        {
                            Escribeylee("Cobro en proceso, N° Ticket: " + dto.Nro_Ticket, fechaLog);
                        }
                        else
                        {
                            Escribeylee("Cobro registrado correctamente, N° Ticket: " + dto.Nro_Ticket, fechaLog);
                        }

                        try
                        {
                          bsn.ActualizarProcesados(dto);
                        }
                        catch (Exception)
                        {
                            Escribeylee("error al guardar en procesados", fechaLog);
                        }
                        return dto.Estado;

                    }
                    else
                    {
                        //dto.Estado = "FINALIZADO_CON_ERRORES";
                        dto.Procesado = "0";
                        Escribeylee(String.Format("Error: Error al consultar la transacción con código de ticket {0}..", dto.Nro_Ticket), fechaLog);
                        foreach (EstadoDeTransaccion est in respuesta.detalles)
                        {
                            foreach (DetalleFalla detf in est.errores)
                            {
                                Escribeylee("Detalle: " + detf.descripcion, fechaLog);
                            }
                        }
                        dto.Procesado = "0";
                        try
                        {
                            bsn.ActualizarProcesados(dto);
                        }
                        catch (Exception ex)
                        {

                            Escribeylee(String.Format("Error: Error al actualizar la transacción con código de ticket {0}", dto.Nro_Ticket), fechaLog);
                        }

                        return dto.Estado;

                    }
                }
                else
                {
                    Escribeylee(String.Format("Error: Sin respuesta del WS para la transacción, código de ticket {0}", dto.Nro_Ticket), fechaLog);
                    return "";
                }
            }
            catch (Exception ex)//al recibir una respuesta con errores, por definición de esquemas no es capaz de parsear correctamente la respuesta a xml
            {
                dto.Estado = "FINALIZADO_CON_ERRORES";
                dto.Procesado = "0";
                bsn.ActualizarProcesados(dto);
                Escribeylee(String.Format("Error: Error al consultar la transacción con código de ticket {0}", dto.Nro_Ticket), fechaLog);
                return "";
            }
        }
    }
}
