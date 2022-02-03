using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using Datos;
using Informix.Domain;

namespace Negocio
{
    public class Bsn_DetRecau
    {
        #region VARIABLES LOCALES

        private ProcedureIfx DaoIFX = new ProcedureIfx();
        private DataSet DS = new DataSet();

        #endregion

        #region PROPIEDADES PUBLICAS


        public DataSet BuscarDetRecau(string fecha, string caja)
        {
            DS = DaoIFX.BuscarDetRecau(fecha, caja);

            return DS;
        }

        public DataSet BuscarDocumentosDetRecau(string fecha, int caja)
        {
            DS = DaoIFX.BuscarTodosDetRecau(fecha, caja);

            return DS;
        }

        public DataSet ValidarTransaccion(string fecha, int caja)
        {
            DS = DaoIFX.ValidarTransaccion(fecha, caja);

            return DS;
        }

        public DataSet BuscarMediosPago(Dto_DetRecau dto)
        {
            DS = DaoIFX.BuscarMediosPago(dto);

            return DS;
        }

        public DataSet BuscarTipoMedioPago(Dto_DetRecau dto)
        {
            DS = DaoIFX.BuscarTipoMedioPago(dto);

            return DS;
        }

        public DataSet BuscarFolioSigfe(string tipo_doc, int folio)
        {
            DS = DaoIFX.BuscarFolioSigfe(tipo_doc, folio);

            return DS;
        }

        #endregion

        public DataSet BuscarDetRecauFolio(string folio, string TipoDoc, string codcaja)
        {
            DS = DaoIFX.BuscarDetRecauFolio(folio, TipoDoc, codcaja);

            return DS;
        }

        public void InsertarProcesados(Dto_DetRecau dto)
        {
            DaoIFX.InsertarProcesados(dto);
        }

        public void ActualizarProcesados(Dto_DetRecau dto)
        {
            DaoIFX.ActualizaProcesados(dto);
        }


        public DataSet BuscarProcesados(Dto_DetRecau dto)
        {
            DS = DaoIFX.BuscarProcesados(dto);
            return DS;
        }

        public DataSet BuscarTicket(Dto_DetRecau dto)
        {
            DS = DaoIFX.BuscarTicket(dto);
            return DS;
        }

        public DataSet BuscarReporteDetRecau(int caja, string mes, string anio)
        {
            DS = DaoIFX.BuscarReporteDetRecau(caja, mes, anio);

            return DS;
        }

        public DataSet ValidarMontosTransaccion(string fecha, int caja, int correl)
        {
            DS = DaoIFX.ValidarMontosTransaccion(fecha, caja, correl);

            return DS;
        }

        public DataSet BuscarDetRecau(string fecha, int caja, int correl)
        {
            DS = DaoIFX.BuscarCorrelDetRecau(fecha, caja, correl);

            return DS;
        }

        public DataSet BuscarDetRecauProce()
        {
            return DaoIFX.BuscarDetRecauProce();
        }

        public int BuscarCmrcBoleta(int folio)
        {
            return DaoIFX.BuscarCmrcBoleta(folio);
        }
    }
}
