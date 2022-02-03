using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Informix.Domain;

namespace Negocio
{
    public class Bsn_Docupre
    {
        #region VARIABLES LOCALES

        private ProcedureIfx DaoIFX = new ProcedureIfx();
        private DataSet DS = new DataSet();

        #endregion

        #region PROPIEDADES PUBLICAS


        public DataSet BuscarDocupre(int folio, string tipo_doc)
        {
            DS = DaoIFX.BuscarDocupre(folio, tipo_doc);

            return DS;
        }

        public DataSet BuscarRutFactura(int folio, string tipodoc)
        {

            DS = DaoIFX.BuscarRutFactura(folio, tipodoc);

            return DS;
        }

        public DataSet BuscarRutBoleta(int folio)
        {

            DS = DaoIFX.BuscarRutBoleta(folio);

            return DS;
        }

        public DataSet BuscarRutCliente(string Rut_Cliente)
        {

            DS = DaoIFX.BuscarRutCliente(Rut_Cliente);

            return DS;
        }

        public DataSet BuscarCmrc(int folio)
        {
            return DaoIFX.BuscarCmrc(folio);
        }

        public DataSet BuscarFacman(string tipo_doc, int folio)
        {
            return DaoIFX.BuscarFacman(tipo_doc, folio);
        }

        #endregion

        //public DataSet BuscarDocupreBol(int folio, string tipo_doc)
        //{
        //    DS = DaoIFX.BuscarDocupreBol(folio, tipo_doc);

        //    return DS;
        //}

      
    }
}
