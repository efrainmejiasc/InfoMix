using System;
using System.Data.Common;
using System.Data;
using XML_DATOS;


namespace Informix.Domain
{
    public class ProcedureIfx
    {
        /// <summary>
        /// Atributo que retorna Error
        /// </summary>
        /// <returns></returns>
        public string MensajeError;

        public ProcedureIfx()
        {
            this.MensajeError = string.Empty;
        }

        private static DbDataReader _dr;
        private static readonly BaseDatosIFX Bd = new BaseDatosIFX();

        #region Metodos Informix   
    

        public DataSet BuscarFacmanFechaTipo(string fecha, string tipoDoc)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE buscarfacman2(@fecha,@tipo_doc)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@fecha", fecha);
                bd.AsignarParametroCadena("@tipo_doc", tipoDoc);
                using (_dr = bd.EjecutarConsulta())
                {
                    //if (!_dr.HasRows)
                    //{
                    //    if (_dr.Read()) coderror = _dr.GetInt32(0);
                    //    if (coderror != 0) this.MensajeError = "error execute procedure buscarfacman2";
                    //    return (ds);
                    //}
                    //else
                    //{             
                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Facman" });

                        ds.Tables[0].Columns[0].ColumnName = "cod_factura";
                        ds.Tables[0].Columns[1].ColumnName = "rutfac";
                        ds.Tables[0].Columns[2].ColumnName = "dvfac";
                        ds.Tables[0].Columns[3].ColumnName = "tipo_doc";
                        ds.Tables[0].Columns[4].ColumnName = "folio";
                        ds.Tables[0].Columns[5].ColumnName = "fecemi";
                        ds.Tables[0].Columns[6].ColumnName = "fecven";
                        ds.Tables[0].Columns[7].ColumnName = "folrel_doc";
                        ds.Tables[0].Columns[8].ColumnName = "valor_neto";
                        ds.Tables[0].Columns[9].ColumnName = "valor_exento";
                        ds.Tables[0].Columns[10].ColumnName = "valor_factura";
                        ds.Tables[0].Columns[11].ColumnName = "valor_netof";
                        ds.Tables[0].Columns[12].ColumnName = "tiprel_doc";
                        ds.Tables[0].Columns[13].ColumnName = "pf_grupo";
                        ds.Tables[0].Columns[14].ColumnName = "valor_iva";
                        ds.Tables[0].Columns[15].ColumnName = "valor_descto";
                        ds.Tables[0].Columns[16].ColumnName = "valor_despacho";
                    //}
                  
                }
               
                return (ds);
            }
            catch (Exception)
            {
                return ds=null;
            }            
        }

        public DataSet BuscarPorNumeroFolioTipo(string folio, string tipoDoc)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE buscarfoltipFacman(@folio,@tipo_doc)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@folio", folio);
                bd.AsignarParametroCadena("@tipo_doc", tipoDoc);
                using (_dr = bd.EjecutarConsulta())
                {                  

                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Facman" });

                        ds.Tables[0].Columns[0].ColumnName = "cod_factura";
                        ds.Tables[0].Columns[1].ColumnName = "rutfac";
                        ds.Tables[0].Columns[2].ColumnName = "dvfac";
                        ds.Tables[0].Columns[3].ColumnName = "tipo_doc";
                        ds.Tables[0].Columns[4].ColumnName = "folio";
                        ds.Tables[0].Columns[5].ColumnName = "fecemi";
                        ds.Tables[0].Columns[6].ColumnName = "fecven";
                        ds.Tables[0].Columns[7].ColumnName = "folrel_doc";
                        ds.Tables[0].Columns[8].ColumnName = "valor_neto";
                        ds.Tables[0].Columns[9].ColumnName = "valor_exento";
                        ds.Tables[0].Columns[10].ColumnName = "valor_factura";
                        ds.Tables[0].Columns[11].ColumnName = "valor_netof";
                        ds.Tables[0].Columns[12].ColumnName = "tiprel_doc";                    
                   
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet BuscarPorNumeroFolio(string folio, string tipoDoc)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE buscarfoliofacman(@folio,@tipo_doc)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@folio", folio);
                bd.AsignarParametroCadena("@tipo_doc", tipoDoc);
                using (_dr = bd.EjecutarConsulta())
                {                  

                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Facman" });

                        ds.Tables[0].Columns[0].ColumnName = "cod_factura";
                        ds.Tables[0].Columns[1].ColumnName = "rutfac";
                        ds.Tables[0].Columns[2].ColumnName = "dvfac";
                        ds.Tables[0].Columns[3].ColumnName = "tipo_doc";
                        ds.Tables[0].Columns[4].ColumnName = "folio";
                        ds.Tables[0].Columns[5].ColumnName = "fecemi";
                        ds.Tables[0].Columns[6].ColumnName = "fecven";
                        ds.Tables[0].Columns[7].ColumnName = "folrel_doc";
                        ds.Tables[0].Columns[8].ColumnName = "valor_neto";
                        ds.Tables[0].Columns[9].ColumnName = "valor_exento";
                        ds.Tables[0].Columns[10].ColumnName = "valor_factura";
                        ds.Tables[0].Columns[11].ColumnName = "valor_netof";
                        ds.Tables[0].Columns[12].ColumnName = "tiprel_doc";
                        ds.Tables[0].Columns[13].ColumnName = "pf_grupo";
                        ds.Tables[0].Columns[14].ColumnName = "valor_iva";
                        ds.Tables[0].Columns[15].ColumnName = "valor_descto";
                        ds.Tables[0].Columns[16].ColumnName = "valor_despacho";
                  
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }       

        public DataSet BuscarFacmanDocu_PrexFolioTipoDoc(string folio, string tipoDoc)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE bxml_foltipdohab(@folio,@tipo_doc)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@folio", folio);
                bd.AsignarParametroCadena("@tipo_doc", tipoDoc);
                using (_dr = bd.EjecutarConsulta())
                {                                

                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "DocuPre" });

                        ds.Tables[0].Columns[0].ColumnName = "cod_factura";
                        ds.Tables[0].Columns[1].ColumnName = "rutfac";
                        ds.Tables[0].Columns[2].ColumnName = "dvfac";
                        ds.Tables[0].Columns[3].ColumnName = "tipo_doc";
                        ds.Tables[0].Columns[4].ColumnName = "folio";
                        ds.Tables[0].Columns[5].ColumnName = "fecemi";
                        ds.Tables[0].Columns[6].ColumnName = "fecven";
                        ds.Tables[0].Columns[7].ColumnName = "folrel_doc";
                        ds.Tables[0].Columns[8].ColumnName = "valor_neto";
                        ds.Tables[0].Columns[9].ColumnName = "valor_exento";
                        ds.Tables[0].Columns[10].ColumnName = "valor_factura";
                        ds.Tables[0].Columns[11].ColumnName = "valor_netof";
                        ds.Tables[0].Columns[12].ColumnName = "tiprel_doc";
                        ds.Tables[0].Columns[13].ColumnName = "ent_codente";
                        ds.Tables[0].Columns[14].ColumnName = "cod_oficina";
                        ds.Tables[0].Columns[15].ColumnName = "tipo_docdocupre";
                        ds.Tables[0].Columns[16].ColumnName = "folio_doc";

                        ds.Tables[0].Columns[17].ColumnName = "emision";
                        ds.Tables[0].Columns[18].ColumnName = "cuenta";
                        ds.Tables[0].Columns[19].ColumnName = "ccosto";
                        ds.Tables[0].Columns[20].ColumnName = "depto";
                        ds.Tables[0].Columns[21].ColumnName = "seccion";
                        ds.Tables[0].Columns[22].ColumnName = "monto";
                        ds.Tables[0].Columns[23].ColumnName = "dh";
                        ds.Tables[0].Columns[24].ColumnName = "origen";
                        ds.Tables[0].Columns[25].ColumnName = "num_serial";
                        ds.Tables[0].Columns[26].ColumnName = "prsg_serial";
                  
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet BuscarFacmanFechaTipoDoc(string fecha, string tipoDoc)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE bxml_foltipdohab(@fecha,@tipo_doc)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@fecha", fecha);
                bd.AsignarParametroCadena("@tipo_doc", tipoDoc);
                using (_dr = bd.EjecutarConsulta())
                {                  

                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Facman" });

                        ds.Tables[0].Columns[0].ColumnName = "cod_factura";
                        ds.Tables[0].Columns[1].ColumnName = "rutfac";
                        ds.Tables[0].Columns[2].ColumnName = "dvfac";
                        ds.Tables[0].Columns[3].ColumnName = "tipo_doc";
                        ds.Tables[0].Columns[4].ColumnName = "folio";
                        ds.Tables[0].Columns[5].ColumnName = "fecemi";
                        ds.Tables[0].Columns[6].ColumnName = "fecven";
                        ds.Tables[0].Columns[7].ColumnName = "folrel_doc";
                        ds.Tables[0].Columns[8].ColumnName = "valor_neto";
                        ds.Tables[0].Columns[9].ColumnName = "valor_exento";
                        ds.Tables[0].Columns[10].ColumnName = "valor_factura";
                        ds.Tables[0].Columns[11].ColumnName = "valor_netof";
                        ds.Tables[0].Columns[12].ColumnName = "tiprel_doc";
                        ds.Tables[0].Columns[13].ColumnName = "ent_codente";
                        ds.Tables[0].Columns[14].ColumnName = "cod_oficina";
                        ds.Tables[0].Columns[15].ColumnName = "tipo_doc";
                        ds.Tables[0].Columns[16].ColumnName = "folio_doc";

                        ds.Tables[0].Columns[17].ColumnName = "emision";
                        ds.Tables[0].Columns[18].ColumnName = "cuenta";
                        ds.Tables[0].Columns[19].ColumnName = "ccosto";
                        ds.Tables[0].Columns[20].ColumnName = "depto";
                        ds.Tables[0].Columns[21].ColumnName = "seccion";
                        ds.Tables[0].Columns[22].ColumnName = "monto";
                        ds.Tables[0].Columns[23].ColumnName = "dh";                       
                 
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet BuscarIdReq(string ente, string depto, string seccion, string pfGrupo, string anho)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE bxml_dptosecc(@ente,@depto,@seccion,@pf_grupo, @Anho)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@ente", ente);
                bd.AsignarParametroCadena("@depto", depto);
                bd.AsignarParametroCadena("@seccion", seccion);
                bd.AsignarParametroCadena("@pf_grupo", pfGrupo);
                bd.AsignarParametroCadena("@Anho", anho);
                using (_dr = bd.EjecutarConsulta())
                {
                   
                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Nivel2x" });

                        ds.Tables[0].Columns[0].ColumnName = "n2_ente";
                        ds.Tables[0].Columns[1].ColumnName = "n2_id1";
                        ds.Tables[0].Columns[2].ColumnName = "n2_id2";
                        ds.Tables[0].Columns[3].ColumnName = "idreq";
                        ds.Tables[0].Columns[4].ColumnName = "pf_grupo";
                        ds.Tables[0].Columns[5].ColumnName = "n2_glosa";
                        ds.Tables[0].Columns[6].ColumnName = "idcon";
                        ds.Tables[0].Columns[7].ColumnName = "idagrupacion";
                                         
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public DataSet BuscarRutCliente(string rutFactura)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE bxml_rutcliente(@rutFactura)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@rutFactura", rutFactura);               
                using (_dr = bd.EjecutarConsulta())
                { 
                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Cliente" });

                        ds.Tables[0].Columns[0].ColumnName = "clte_origen";
                        ds.Tables[0].Columns[1].ColumnName = "rut";
                        ds.Tables[0].Columns[2].ColumnName = "digito";
                        ds.Tables[0].Columns[3].ColumnName = "nombre";
                        ds.Tables[0].Columns[4].ColumnName = "clte_rut";
                        ds.Tables[0].Columns[5].ColumnName = "clte_digito";
                        ds.Tables[0].Columns[6].ColumnName = "privpub";
                        ds.Tables[0].Columns[7].ColumnName = "cod_estbl";

                   
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet BuscarBoletaFechaTipo(string fecha, string tipoDoc)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            
                const string sql = "EXECUTE PROCEDURE buscarbolet2(@fecha,@tipo_doc)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@fecha", fecha);
                bd.AsignarParametroCadena("@tipo_doc", tipoDoc);
                using (_dr = bd.EjecutarConsulta())
                {              
                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Cmrc" });

                        ds.Tables[0].Columns[0].ColumnName = "cod_boleta";
                        ds.Tables[0].Columns[1].ColumnName = "rutboleta";
                        ds.Tables[0].Columns[2].ColumnName = "dvboleta";
                        ds.Tables[0].Columns[3].ColumnName = "tipo_doc";
                        ds.Tables[0].Columns[4].ColumnName = "folio";
                        ds.Tables[0].Columns[5].ColumnName = "fecemi";
                        ds.Tables[0].Columns[6].ColumnName = "fecven";
                        ds.Tables[0].Columns[7].ColumnName = "folrel_doc";
                        ds.Tables[0].Columns[8].ColumnName = "valor_neto";
                        ds.Tables[0].Columns[9].ColumnName = "valor_exento";
                        ds.Tables[0].Columns[10].ColumnName = "valor_boleta";
                        ds.Tables[0].Columns[11].ColumnName = "valor_netof";
                        ds.Tables[0].Columns[12].ColumnName = "tiprel_doc";
                        ds.Tables[0].Columns[13].ColumnName = "pf_grupo";

                        ds.Tables[0].Columns[14].ColumnName = "valor_iva";
                        ds.Tables[0].Columns[15].ColumnName = "valor_descto";
                        ds.Tables[0].Columns[16].ColumnName = "valor_despacho";
                       
                   
                }
                return (ds);
        }


        public DataSet BuscarBoletaFolio(string folio, string tipoDoc)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE buscarboletfolio(@folio,@tipo_doc)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@folio", folio);
                bd.AsignarParametroCadena("@tipo_doc", tipoDoc);
                using (_dr = bd.EjecutarConsulta())
                {                 
                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Cmrc" });

                        ds.Tables[0].Columns[0].ColumnName = "cod_boleta";//serial_comrec
                        ds.Tables[0].Columns[1].ColumnName = "rutboleta";//cmrc_rutcli
                        ds.Tables[0].Columns[2].ColumnName = "dvboleta";//cmrc_dvcli
                        ds.Tables[0].Columns[3].ColumnName = "tipo_doc";//"R"
                        ds.Tables[0].Columns[4].ColumnName = "folio";//cmrc_folio
                        ds.Tables[0].Columns[5].ColumnName = "fecemi";//cmrc_fecemi
                        ds.Tables[0].Columns[6].ColumnName = "fecven";//cmrc_fecemi
                        ds.Tables[0].Columns[7].ColumnName = "folrel_doc";//boleta
                        ds.Tables[0].Columns[8].ColumnName = "valor_neto";//cmrc_valor
                        ds.Tables[0].Columns[9].ColumnName = "valor_exento";//0
                        ds.Tables[0].Columns[10].ColumnName = "valor_boleta";//cmrc_valor
                        ds.Tables[0].Columns[11].ColumnName = "valor_netof";//cmrc_valor
                        ds.Tables[0].Columns[12].ColumnName = "tiprel_doc";//""
                        ds.Tables[0].Columns[13].ColumnName = "pf_grupo";// pf_grupo
                        ds.Tables[0].Columns[14].ColumnName = "valor_iva";//0
                        ds.Tables[0].Columns[15].ColumnName = "valor_descto";//0
                        ds.Tables[0].Columns[16].ColumnName = "valor_despacho";//0
                   
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet BuscarBoletaDocu_PrexFolioTipoDoc(string folio, string tipoDoc)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE bxml_foltipdobol(@folio,@tipo_doc)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@folio", folio);
                bd.AsignarParametroCadena("@tipo_doc", tipoDoc);
                using (_dr = bd.EjecutarConsulta())
                {                  

                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "DocuPre" });

                        ds.Tables[0].Columns[0].ColumnName = "cod_boleta";
                        ds.Tables[0].Columns[1].ColumnName = "rutboleta";
                        ds.Tables[0].Columns[2].ColumnName = "dvboleta";
                        ds.Tables[0].Columns[3].ColumnName = "tipo_doc";
                        ds.Tables[0].Columns[4].ColumnName = "folio";
                        ds.Tables[0].Columns[5].ColumnName = "fecemi";
                        ds.Tables[0].Columns[6].ColumnName = "fecven";
                        ds.Tables[0].Columns[7].ColumnName = "folrel_doc";
                        ds.Tables[0].Columns[8].ColumnName = "valor_neto";
                        ds.Tables[0].Columns[9].ColumnName = "valor_exento";
                        ds.Tables[0].Columns[10].ColumnName = "valor_boleta";
                        ds.Tables[0].Columns[11].ColumnName = "valor_netof";
                        ds.Tables[0].Columns[12].ColumnName = "tiprel_doc";


                        ds.Tables[0].Columns[13].ColumnName = "ent_codente";
                        ds.Tables[0].Columns[14].ColumnName = "cod_oficina";
                        ds.Tables[0].Columns[15].ColumnName = "tipo_docdocupre";
                        ds.Tables[0].Columns[16].ColumnName = "folio_doc";
                        ds.Tables[0].Columns[17].ColumnName = "emision";
                        ds.Tables[0].Columns[18].ColumnName = "cuenta";
                        ds.Tables[0].Columns[19].ColumnName = "ccosto";
                        ds.Tables[0].Columns[20].ColumnName = "depto";

                        ds.Tables[0].Columns[21].ColumnName = "seccion";
                        ds.Tables[0].Columns[22].ColumnName = "monto";
                        ds.Tables[0].Columns[23].ColumnName = "dh";
                        ds.Tables[0].Columns[24].ColumnName = "origen";

                        ds.Tables[0].Columns[25].ColumnName = "num_serial";
                        ds.Tables[0].Columns[26].ColumnName = "prsg_serial";
                   
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet BuscarCtaContable(string cuenta)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE bxml_ctacont(@cuenta)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@cuenta", cuenta);
               
                using (_dr = bd.EjecutarConsulta())
                {
                                 

                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Cuenta" });

                        ds.Tables[0].Columns[0].ColumnName = "codigo";
                        ds.Tables[0].Columns[1].ColumnName = "descripcion";
                      
                  
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        /*****************************************Procedimientos Mantenedor******************************************/
        public void InsertarRequerimientos(DtoManRequerimiento dto)
        {
            BaseDatosIFX bd = Bd.Connect();

            try
            {
                const string sql = "EXECUTE PROCEDURE InsertarReq(@id1,@id2,@glosa,@req,@con,@grupo,@agrupacion,@Anho)";
                bd.CrearComando(sql);

                bd.AsignarParametroCadena("@id1", dto.Departamento);
                bd.AsignarParametroCadena("@id2", dto.Seccion);
                bd.AsignarParametroCadena("@glosa", dto.Descripcion);
                bd.AsignarParametroCadena("@req", dto.Folio);
                bd.AsignarParametroCadena("@con", dto.Concepto);
                bd.AsignarParametroCadena("@grupo", dto.Grupo);
                bd.AsignarParametroCadena("@agrupacion", dto.Agrupacion);
                bd.AsignarParametroCadena("@Anho", dto.Año);
                _dr = bd.EjecutarConsulta();
            }
            catch (Exception e)
            {
             
            }
        }

        public void EditarRequerimientos(DtoManRequerimiento dto)
        {
            BaseDatosIFX bd = Bd.Connect();

            try
            {
                const string sql = "EXECUTE PROCEDURE EditarReq(@id1,@id2,@glosa,@req,@con,@grupo,@agrupacion,@Id,@Anho)";
                bd.CrearComando(sql);

                bd.AsignarParametroCadena("@id1", dto.Departamento);
                bd.AsignarParametroCadena("@id2", dto.Seccion);
                bd.AsignarParametroCadena("@glosa", dto.Descripcion);
                bd.AsignarParametroCadena("@req", dto.Folio);
                bd.AsignarParametroCadena("@con", dto.Concepto);
                bd.AsignarParametroCadena("@grupo", dto.Grupo);
                bd.AsignarParametroCadena("@agrupacion", dto.Agrupacion);
                bd.AsignarParametroCadena("@Id", dto.Id);
                bd.AsignarParametroCadena("@Anho", dto.Año);
                _dr = bd.EjecutarConsulta();
            }
            catch (Exception e)
            {

            }
        }

        public DataSet BuscarReq(DtoManRequerimiento dto)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE BuscarReq(@id1,@id2@grupo)";
                bd.CrearComando(sql);

               
                    bd.AsignarParametroCadena("@id1", dto.Departamento);               
                    bd.AsignarParametroCadena("@id2", dto.Seccion);              
                    bd.AsignarParametroCadena("@grupo", dto.Grupo);
              
                using (_dr = bd.EjecutarConsulta())
                {
                 
                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Nivel2x" });

                        ds.Tables[0].Columns[0].ColumnName = "n2_id1";
                        ds.Tables[0].Columns[1].ColumnName = "n2_id2";
                        ds.Tables[0].Columns[6].ColumnName = "pf_grupo";                      

                 
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet BuscarFiltros(DtoManRequerimiento dto)
        {            
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE BuscarFiltros(@id1,@id2,@glosa,@req,@con,@grupo,@agrupacion)";
                bd.CrearComando(sql);

                if (dto.Departamento == null)
                    bd.AsignarParametroNulo("@id1");
                else
                    bd.AsignarParametroCadena("@id1", dto.Departamento);

                if (dto.Seccion == null)
                    bd.AsignarParametroNulo("@id2");
                else
                    bd.AsignarParametroCadena("@id2", dto.Seccion);

                if (dto.Descripcion == null)
                    bd.AsignarParametroNulo("@glosa");
                else
                    bd.AsignarParametroCadena("@glosa", dto.Descripcion);

                if (dto.Folio == null)
                    bd.AsignarParametroNulo("@req");
                else
                    bd.AsignarParametroCadena("@req", dto.Folio);

                if (dto.Concepto == null)
                    bd.AsignarParametroNulo("@con");
                else
                    bd.AsignarParametroCadena("@con", dto.Concepto);

                if (dto.Grupo == null)
                    bd.AsignarParametroNulo("@grupo");
                else
                    bd.AsignarParametroCadena("@grupo", dto.Grupo);

                if (dto.Agrupacion == null)
                    bd.AsignarParametroNulo("@agrupacion");
                else
                    bd.AsignarParametroCadena("@agrupacion", dto.Agrupacion);
                //if (dto.Año == null)
                //    bd.AsignarParametroNulo("@Anho");
                //else
                //    bd.AsignarParametroCadena("@Anho", dto.Año);
                using (_dr = bd.EjecutarConsulta())
                {                   
                        ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Nivel2x" });

                        
                        ds.Tables[0].Columns[0].ColumnName = "n2_id1";
                        ds.Tables[0].Columns[1].ColumnName = "n2_id2";
                        ds.Tables[0].Columns[2].ColumnName = "n2_glosa";
                        ds.Tables[0].Columns[3].ColumnName = "n2_serial";
                        ds.Tables[0].Columns[4].ColumnName = "idreq";
                        ds.Tables[0].Columns[5].ColumnName = "idcon";
                        ds.Tables[0].Columns[6].ColumnName = "pf_grupo";                        
                        ds.Tables[0].Columns[7].ColumnName = "idagrupacion";
                        ds.Tables[0].Columns[8].ColumnName = "anho";
                  
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public DataSet CargarNivel1()
        {
           
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE CargarNivel1()";
                //const string sql = "EXECUTE PROCEDURE CargarNivel1V2()";
                bd.CrearComando(sql);

                using (_dr = bd.EjecutarConsulta())
                {
                    ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Nivel1" });

                    ds.Tables[0].Columns[0].ColumnName = "n1_id1";
                    ds.Tables[0].Columns[1].ColumnName = "n1_glosa";
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet CargarNivel2()
        {

            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE CargarNivel2()";
                bd.CrearComando(sql);

                using (_dr = bd.EjecutarConsulta())
                {
                    ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Nivel2" });

                    ds.Tables[0].Columns[0].ColumnName = "n2_id2";
                    ds.Tables[0].Columns[1].ColumnName = "n2_glosa";
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet CargarNivel2Filtro(int id)
        {

            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE CargarNivel2Filtro(@id_nivel1)";
                bd.CrearComando(sql);

                bd.AsignarParametroEntero("@id_nivel1", id);

                using (_dr = bd.EjecutarConsulta())
                {
                    ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Nivel2" });

                    ds.Tables[0].Columns[0].ColumnName = "n2_id2";
                    ds.Tables[0].Columns[1].ColumnName = "n2_glosa";
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet CargarNivel2FiltroGlosa(int id)
        {

            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE CargarGlosaNivel2(@id_nivel1)";
                bd.CrearComando(sql);

                bd.AsignarParametroEntero("@id_nivel2", id);

                using (_dr = bd.EjecutarConsulta())
                {
                    ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Nivel2" });
                    
                    ds.Tables[0].Columns[1].ColumnName = "n2_glosa";
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void EliminarReq(int id)
        {            
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE Eliminar2x(@id_Nivel2x)";
                bd.CrearComando(sql);

                bd.AsignarParametroEntero("@id_Nivel2x", id);

                _dr = bd.EjecutarConsulta();
                
            }
            catch (Exception)
            {
              
            }
        }
        /************************************************************************************************************/


        public DataSet BuscarInterSigfe(string tipRelDoc, int folRelDoc)
        {
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                string sql = "EXECUTE PROCEDURE buscarinterfac(@folio,@tipodoc)";
                
                bd.CrearComando(sql);

                bd.AsignarParametroCadena("@tipodoc", tipRelDoc);
                bd.AsignarParametroEntero("@folio", folRelDoc);

                using (_dr = bd.EjecutarConsulta())
                {
                    ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "intersigfe" });

                    ds.Tables[0].Columns[0].ColumnName = "pf_grupo";
                    ds.Tables[0].Columns[1].ColumnName = "folio_sigfe";
                    ds.Tables[0].Columns[2].ColumnName = "fecha_cumplimiento";
                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public DataSet BuscarNotaCreditoSigfe(string folio)
        {
            DataSet ds = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
            try
            {
                const string sql = "EXECUTE PROCEDURE buscarncsigfe(@folio)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@folio", folio);
                using (_dr = bd.EjecutarConsulta())
                {

                    ds.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Facman" });

                    ds.Tables[0].Columns[0].ColumnName = "cod_factura";
                    ds.Tables[0].Columns[1].ColumnName = "rutfac";
                    ds.Tables[0].Columns[2].ColumnName = "dvfac";
                    ds.Tables[0].Columns[3].ColumnName = "tipo_doc";
                    ds.Tables[0].Columns[4].ColumnName = "folio";
                    ds.Tables[0].Columns[5].ColumnName = "fecemi";
                    ds.Tables[0].Columns[6].ColumnName = "fecven";
                    ds.Tables[0].Columns[7].ColumnName = "folrel_doc";
                    ds.Tables[0].Columns[8].ColumnName = "valor_neto";
                    ds.Tables[0].Columns[9].ColumnName = "valor_exento";
                    ds.Tables[0].Columns[10].ColumnName = "valor_factura";
                    ds.Tables[0].Columns[11].ColumnName = "valor_netof";
                    ds.Tables[0].Columns[12].ColumnName = "tiprel_doc";
                    ds.Tables[0].Columns[13].ColumnName = "pf_grupo";
                    ds.Tables[0].Columns[14].ColumnName = "valor_iva";
                    ds.Tables[0].Columns[15].ColumnName = "valor_descto";
                    ds.Tables[0].Columns[16].ColumnName = "valor_despacho";
                    ds.Tables[0].Columns[17].ColumnName = "folio_sigfe";
                    ds.Tables[0].Columns[18].ColumnName = "fecha_cumplimiento";

                }
                return (ds);
            }
            catch (Exception)
            {
                return null;
            }
        }       
    }
}
