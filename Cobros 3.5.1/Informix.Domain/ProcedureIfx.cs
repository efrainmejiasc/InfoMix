using System;
using System.Data.Common;
using System.Data;
using Datos;

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


        public DataSet BuscarFacmanFechaTipo(string fecha, string tipo_doc)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            const string sql = "EXECUTE PROCEDURE buscarfacman2(@fecha,@tipo_doc)";
            bd.CrearComando(sql);
            bd.AsignarParametroCadena("@fecha", fecha);
            bd.AsignarParametroCadena("@tipo_doc", tipo_doc);
            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] {"Facman"});

                DS.Tables[0].Columns[0].ColumnName = "cod_factura";
                DS.Tables[0].Columns[1].ColumnName = "rutfac";
                DS.Tables[0].Columns[2].ColumnName = "dvfac";
                DS.Tables[0].Columns[3].ColumnName = "tipo_doc";
                DS.Tables[0].Columns[4].ColumnName = "folio";
                DS.Tables[0].Columns[5].ColumnName = "fecemi";
                DS.Tables[0].Columns[6].ColumnName = "fecven";
                DS.Tables[0].Columns[7].ColumnName = "folrel_doc";
                DS.Tables[0].Columns[8].ColumnName = "valor_neto";
                DS.Tables[0].Columns[9].ColumnName = "valor_exento";
                DS.Tables[0].Columns[10].ColumnName = "valor_factura";
                DS.Tables[0].Columns[11].ColumnName = "valor_netof";
                DS.Tables[0].Columns[12].ColumnName = "tiprel_doc";
                DS.Tables[0].Columns[13].ColumnName = "pf_grupo";
                DS.Tables[0].Columns[14].ColumnName = "valor_iva";
                DS.Tables[0].Columns[15].ColumnName = "valor_descto";
                DS.Tables[0].Columns[16].ColumnName = "valor_despacho";

            }
            bd.Desconectar();
            return (DS);
        }

        public DataSet BuscarPorNumeroFolioTipo(string folio, string tipo_doc)
        {            
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
          
                const string sql = "EXECUTE PROCEDURE buscarfoltipFacman(@folio,@tipo_doc)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@folio", folio);
                bd.AsignarParametroCadena("@tipo_doc", tipo_doc);
                using (_dr = bd.EjecutarConsulta())
                {                  

                        DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Facman" });

                        DS.Tables[0].Columns[0].ColumnName = "cod_factura";
                        DS.Tables[0].Columns[1].ColumnName = "rutfac";
                        DS.Tables[0].Columns[2].ColumnName = "dvfac";
                        DS.Tables[0].Columns[3].ColumnName = "tipo_doc";
                        DS.Tables[0].Columns[4].ColumnName = "folio";
                        DS.Tables[0].Columns[5].ColumnName = "fecemi";
                        DS.Tables[0].Columns[6].ColumnName = "fecven";
                        DS.Tables[0].Columns[7].ColumnName = "folrel_doc";
                        DS.Tables[0].Columns[8].ColumnName = "valor_neto";
                        DS.Tables[0].Columns[9].ColumnName = "valor_exento";
                        DS.Tables[0].Columns[10].ColumnName = "valor_factura";
                        DS.Tables[0].Columns[11].ColumnName = "valor_netof";
                        DS.Tables[0].Columns[12].ColumnName = "tiprel_doc";                    
                   
                }
                bd.Desconectar();
                return (DS);
        }

        public DataSet BuscarDetRecau(string fecha, string caja)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
           
                string sql = "select distinct nro_correl from detrecau ";
                sql += " where cod_oficina = 1 and cod_caja = @caja and fecha_regi = @fecha ";
                sql += " order by nro_correl asc";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@fecha", fecha);
                bd.AsignarParametroCadena("@caja", caja);
                
                using (_dr = bd.EjecutarConsulta())
                {
                    DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "detrecau" });
                }
                bd.Desconectar();
                return (DS);
          
        }

        public DataSet BuscarTodosDetRecau(string fecha, int caja)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select * from detrecau";
            sql +=
                " where cod_oficina = 1 and cod_caja = @caja and fecha_regi = @fecha and folio_docpaga is not null";
            sql += " order by nro_correl, tp_docpaga desc";
            bd.CrearComando(sql);
            bd.AsignarParametroCadena("@fecha", fecha);
            bd.AsignarParametroEntero("@caja", caja);
            //bd.AsignarParametroEntero("@correl", correl);

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] {"detrecau"});
            }
            bd.Desconectar();
            return (DS);
        }

        public
            DataSet ValidarTransaccion(string fecha, int caja)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
           
                string sql = "execute procedure validartransaccion(@caja,@fecha)";
                
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@fecha", fecha);
                bd.AsignarParametroEntero("@caja", caja);
                

                using (_dr = bd.EjecutarConsulta())
                {
                    DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "detrecau" });

                    DS.Tables[0].Columns[0].ColumnName = "sumadoc";
                    DS.Tables[0].Columns[1].ColumnName = "sumapago";
                }
                bd.Desconectar();
                return (DS);
          
        }
       
        #endregion

        public DataSet BuscarDocupre(int folio, string tipo_doc)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
          
                string sql = "select emision, cuenta, ccosto, depto, ";
                sql += "seccion, sum(monto) as monto, prsg_serial, num_serial, dh from docu_pre ";
                sql += "where tipo_doc = @tipo_doc and folio_doc = @folio and origen <> 'I' and dh = 'D'";
                sql += "group by emision, cuenta, ccosto, depto, seccion, prsg_serial, num_serial, dh";
                //const string sql = "EXECUTE PROCEDURE bxml_foltipdohab(@folio,@tipo_doc)";
                bd.CrearComando(sql);

                //bd.AsignarParametroCadena("@ccosto", "");
                bd.AsignarParametroCadena("@folio", folio.ToString());
                bd.AsignarParametroCadena("@tipo_doc", tipo_doc);

                using (_dr = bd.EjecutarConsulta())
                {
                    DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "docu_pre" });
                }
                bd.Desconectar();
                return (DS);
        }

        public DataSet BuscarMediosPago(Dto_DetRecau dto)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select tp_medpago, sum(valor_medpago) as valor_medpago ";
                sql += "from detrecau ";
                sql += "where cod_oficina = 1 and cod_caja = @caja and fecha_regi = @fecha ";
                sql += "and nro_correl = @correl and folio_docpaga is null ";//and valor_medpago = @valor
                sql += "group by tp_medpago";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@fecha", dto.Fecha_regi);
                bd.AsignarParametroEntero("@caja", dto.Cod_caja);
                bd.AsignarParametroEntero("@correl", dto.Nro_correl);
                //bd.AsignarParametroCadena("@valor", dto.Valor_docpaga.ToString());

                using (_dr = bd.EjecutarConsulta())
                {
                    DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "detrecau" });
                }
                bd.Desconectar();
                return (DS);
        }

        public DataSet BuscarTipoMedioPago(Dto_DetRecau dto)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
           
                string sql = "select first 1 tp_medpago, nro_medpago ";
                sql += "from detrecau ";
                sql += "where cod_oficina = 1 and cod_caja = @caja and fecha_regi = @fecha ";
                sql += "and nro_correl = @correl and folio_docpaga is null";//and valor_medpago = @valor
                sql += "order by tp_medpago asc";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@fecha", dto.Fecha_regi);
                bd.AsignarParametroEntero("@caja", dto.Cod_caja);
                bd.AsignarParametroEntero("@correl", dto.Nro_correl);
                //bd.AsignarParametroCadena("@valor", dto.Valor_docpaga.ToString());

                using (_dr = bd.EjecutarConsulta())
                {
                    DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "detrecau" });
                }
                bd.Desconectar();
                return (DS);
        }

        public DataSet BuscarRutCliente(string Rut_Fac)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
           
                string sql = "select clte_rut,clte_digito ";
                sql += "from cliente ";
                sql += "where rut = @Rut_Fac";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@Rut_Fac", Rut_Fac);
                
                using (_dr = bd.EjecutarConsulta())
                {
                    DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "Cliente" });

                }
                bd.Desconectar();
                return (DS);
        }

        public DataSet BuscarFolioSigfe(string tipo_doc, int folio)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();
          
                string sql = "select folio_sigfe, fecha_cumplimiento ";
                sql += "from intersigfe ";
                sql += "where tipo_doc = @tipo_doc and folio = @folio";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@tipo_doc", tipo_doc);
                bd.AsignarParametroEntero("@folio", folio);

                using (_dr = bd.EjecutarConsulta())
                {
                    DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "intersigfe" });
                }
                bd.Desconectar();
                return (DS);
        }

        public DataSet BuscarDetRecauFolio(string folio, string TipoDoc, string codcaja)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select *  from detrecau ";
            sql += "where cod_oficina = 1 and cod_caja = @codcaja and fecha_regi >= '01-01-2014' and ";
            sql += "tp_docpaga = @TipoDoc and folio_docpaga = @folio ";

            bd.CrearComando(sql);
            bd.AsignarParametroCadena("@folio", folio);
            bd.AsignarParametroCadena("@TipoDoc", TipoDoc);
            bd.AsignarParametroCadena("@codcaja",codcaja);

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] {"detrecau"});
            }
            bd.Desconectar();
            return (DS);
        }
       
        public void InsertarProcesados(Dto_DetRecau dto)
        {
            BaseDatosIFX bd = Bd.Connect();

            string sql = "insert into detrecau_proce ";
            sql += "(cod_caja, nro_correl, tp_docpaga, folio_doc, procesado, estado, cod_ticket, indice, fecha_proce) ";
            sql += "values (@caja, @correl, @tp_doc, @folio, @procesado, @estado, @ticket, @indice, @fechaproce)";

            bd.CrearComando(sql);
            bd.AsignarParametroEntero("@caja", dto.Cod_caja);
            bd.AsignarParametroEntero("@correl", dto.Nro_correl);
            bd.AsignarParametroEntero("@folio", dto.Folio);
            bd.AsignarParametroCadena("@tp_doc", dto.Tipo_Doc);
            bd.AsignarParametroCadena("@procesado", dto.Procesado);
            bd.AsignarParametroCadena("@estado", dto.Estado);
            bd.AsignarParametroCadena("@ticket", dto.Nro_Ticket.ToString());
            bd.AsignarParametroCadena("@indice", dto.Indice);
            bd.AsignarParametroCadena("@fechaproce", dto.FechaProce);

            _dr = bd.EjecutarConsulta();
            bd.Desconectar();
            
        }

        public void ActualizaProcesados(Dto_DetRecau dto)
        {
            BaseDatosIFX bd = Bd.Connect();

            string sql = "update detrecau_proce ";
            sql += "set estado = @estado, procesado = @procesado, cod_ticket = @ticket, fecha_proce = @fechaproce";
            sql += "where cod_caja = @caja and nro_correl = @correl and ";
            sql += "tp_docpaga = @tp_doc and folio_doc = @folio and indice = @indice";


            bd.CrearComando(sql);
            bd.AsignarParametroEntero("@caja", dto.Cod_caja);
            bd.AsignarParametroEntero("@correl", dto.Nro_correl);
            bd.AsignarParametroEntero("@folio", dto.Folio);
            bd.AsignarParametroCadena("@tp_doc", dto.Tipo_Doc);
            bd.AsignarParametroCadena("@procesado", dto.Procesado);
            bd.AsignarParametroCadena("@estado", dto.Estado);
            bd.AsignarParametroCadena("@ticket", dto.Nro_Ticket.ToString());
            bd.AsignarParametroCadena("@indice", dto.Indice);
            bd.AsignarParametroCadena("@fechaproce", dto.FechaProce);

            _dr = bd.EjecutarConsulta();
            bd.Desconectar();
        }

        public DataSet BuscarProcesados(Dto_DetRecau dto)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select *  from detrecau_proce ";
            sql += "where cod_caja = @caja and nro_correl = @correl and  tp_docpaga = @tp_doc and folio_doc = @folio and indice = @indice";
            sql += " order by estado desc";
            
            bd.CrearComando(sql);
            bd.AsignarParametroEntero("@caja", dto.Cod_caja);
            bd.AsignarParametroEntero("@correl", dto.Nro_correl);
            bd.AsignarParametroEntero("@folio", dto.Folio);
            bd.AsignarParametroCadena("@tp_doc", dto.Tipo_Doc);
            bd.AsignarParametroCadena("@indice", dto.Indice);

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "detrecau" });
            }
            bd.Desconectar();
            return (DS);
        }

        public DataSet BuscarRutFactura(int folio, string tipodoc)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select rutfac  from facman ";
            sql += "where tipo_doc = @tipodoc and folio = @folio";

            bd.CrearComando(sql);
            bd.AsignarParametroEntero("@folio", folio);
            bd.AsignarParametroCadena("@tipodoc", tipodoc);

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "facman" });
            }
            bd.Desconectar();
            return (DS);
        }

        public DataSet BuscarRutBoleta(int folio)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select cmrc_rutcli, cmrc_dvcli  from cmrc ";
            sql += "where cmrc_folio = @folio";

            bd.CrearComando(sql);
            bd.AsignarParametroEntero("@folio", folio);
           

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "cmrc" });
            }
            bd.Desconectar();
            return (DS);
        }

        public DataSet BuscarUsuario(string clave)
        {
            DataSet DS = new DataSet();
            Bd.Desconectar();
            BaseDatosIFX bd = Bd.Connect();
          
                const string sql = "EXECUTE PROCEDURE buscarusuario(@clave)";
                bd.CrearComando(sql);
                bd.AsignarParametroCadena("@clave", clave);
                using (_dr = bd.EjecutarConsulta())
                {
                    DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "users" });

                    DS.Tables[0].Columns[0].ColumnName = "rut_user";
                    DS.Tables[0].Columns[1].ColumnName = "dv_user";
                    DS.Tables[0].Columns[2].ColumnName = "nombre_user";
                    DS.Tables[0].Columns[3].ColumnName = "apellido_user";
                    DS.Tables[0].Columns[4].ColumnName = "ini_user";
                    DS.Tables[0].Columns[5].ColumnName = "cargo_user";
                    DS.Tables[0].Columns[6].ColumnName = "password";
                    DS.Tables[0].Columns[7].ColumnName = "ubicacion";

                }
                bd.Desconectar();
                return (DS);
          
        }

        public DataSet BuscarTicket(Dto_DetRecau dto)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select cod_ticket, estado, procesado from detrecau_proce ";
            sql += "where tp_docpaga = @tipo_doc and folio_doc = @folio and indice = @indice";

            bd.CrearComando(sql);
            bd.AsignarParametroEntero("@folio", dto.Folio);
            bd.AsignarParametroCadena("@tipo_doc", dto.Tipo_Doc);
            bd.AsignarParametroCadena("@indice", dto.Indice);

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "detrecau_proce" });
            }
            bd.Desconectar();
            return (DS);
        }

        public DataSet BuscarReporteDetRecau(int caja, string mes, string anio)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select fecha_regi, folio_docpaga, fecha_docpaga, valor_docpaga";
            sql += " from detrecau";
            sql +=
                " where cod_oficina = 1 and cod_caja = @caja and year(fecha_regi) = @anio and month(fecha_regi) = @mes and folio_docpaga is not null";
           
            bd.CrearComando(sql);
            bd.AsignarParametroCadena("@anio", anio);
            bd.AsignarParametroCadena("@mes", mes);
            bd.AsignarParametroEntero("@caja", caja);
            //bd.AsignarParametroEntero("@correl", correl);
            
            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "detrecau" });
            }
            bd.Desconectar();
            return (DS);
        }

        public DataSet ValidarMontosTransaccion(string fecha, int caja, int correl)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "execute procedure validatransaccion2(@caja,@fecha,@correl)";

            bd.CrearComando(sql);
            bd.AsignarParametroCadena("@fecha", fecha);
            bd.AsignarParametroEntero("@caja", caja);
            bd.AsignarParametroEntero("@correl", correl);

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "detrecau" });

                DS.Tables[0].Columns[0].ColumnName = "sumadoc";
                DS.Tables[0].Columns[1].ColumnName = "sumapago";
            }
            bd.Desconectar();
            return (DS);
        }

        public DataSet BuscarCorrelDetRecau(string fecha, int caja, int correl)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select * from detrecau";
            sql +=
                " where cod_oficina = 1 and cod_caja = @caja and fecha_regi = @fecha and nro_correl = @correl and folio_docpaga is not null";
            sql += " order by nro_correl, tp_docpaga desc";
            bd.CrearComando(sql);
            bd.AsignarParametroCadena("@fecha", fecha);
            bd.AsignarParametroEntero("@caja", caja);
            bd.AsignarParametroEntero("@correl", correl);

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "detrecau" });
            }
            bd.Desconectar();
            return (DS);
        }

        public DataSet BuscarCmrc(int folio)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select cmrc_fecemi from cmrc";
            sql +=" where cmrc_folio = @folio";
            bd.CrearComando(sql);
            bd.AsignarParametroEntero("@folio", folio);
           

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "cmrc" });
            }
            bd.Desconectar();
            return (DS);
        }

        public DataSet BuscarFacman(string tipo_doc, int folio)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select fecemi from facman";
            sql += " where tipo_doc = @tipo_doc and folio = @folio";
            bd.CrearComando(sql);
            
            bd.AsignarParametroCadena("@tipo_doc",tipo_doc);
            bd.AsignarParametroEntero("@folio", folio);
            
            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "facman" });
            }
            bd.Desconectar();
            return (DS);
        }

        public DataSet BuscarDetRecauProce()
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select * from detrecau_proce";
            sql += " where estado = 'RECEPCIONADO'";
            bd.CrearComando(sql);

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "detrecau_proce" });
            }
            bd.Desconectar();
            return (DS);
        }

        public int BuscarCmrcBoleta(int folio)
        {
            DataSet DS = new DataSet();
            BaseDatosIFX bd = Bd.Connect();

            string sql = "select boleta ";
            sql += "from cmrc ";
            sql += "where cmrc_folio = @folio";

            bd.CrearComando(sql);


            bd.AsignarParametroEntero("@folio", folio);

            using (_dr = bd.EjecutarConsulta())
            {
                DS.Load(_dr, LoadOption.OverwriteChanges, new string[] { "cmrc" });
                bd.Desconectar();

                if (DS != null && DS.Tables[0].Columns.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    return int.Parse(DS.Tables[0].Rows[0][0].ToString());
                }

                return 0;
            }
        }
    }
}
