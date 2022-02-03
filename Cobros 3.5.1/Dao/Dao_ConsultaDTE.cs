using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;




namespace Dao
{
    public class Dao_ConsultaDTE
    {
        #region VARIABLES LOCALES
        private ConexionBaseDatos _con = null;
        private DataSet _DS = null;

        #endregion

        #region PROPIEDADES PRIVADAS
        /// <summary>
        /// Propiedad para llamar al objeto que ejecuta la conexión con la base de datos
        /// </summary>
        private ConexionBaseDatos Con
        {
            get { return this._con; }
            set { this._con = value; }
        }

        private DataSet DS
        {
            get { return this._DS; }
            set { this._DS = value; }
        }


        #endregion

        #region CONSTRUCTORES

        public Dao_ConsultaDTE()
        {
            Con = new ConexionBaseDatos();
        }

        ~Dao_ConsultaDTE()
        {
            Dispose(false);
        }

        public void Despose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_DS != null)
                {
                    _DS.Dispose();
                    _DS = null;
                }
            }
        }

        #endregion

        #region
        public DataSet SeleccionarTodo(Dto_Usuario dto)
        {
            const string procedimiento = "ConsultaDTE_SeleccionarTodo";
            List<Parametros> listaParametros = new List<Parametros>();

            listaParametros.Add(new Parametros("@Rut", SqlDbType.Int, dto.Rut));

            DS = Con.ObtenerRegistros(procedimiento, "inter_DTE", listaParametros);

            return DS;
        }
        #endregion

        public DataSet SeleccionarFiltro(Dto_Consulta dto)
        {
            const string procedimiento = "ConsultaDTE_SeleccionarFiltros";
            List<Parametros> listaParametros = new List<Parametros>();

            listaParametros.Add(new Parametros("@Rut", SqlDbType.Int, dto.Rut));
            listaParametros.Add(new Parametros("@Folio", SqlDbType.Int, dto.Folio));
            listaParametros.Add(new Parametros("@Estado", SqlDbType.Int, dto.Estado));
            listaParametros.Add(new Parametros("@Tipo_Doc", SqlDbType.VarChar, dto.TipoDoc));
            listaParametros.Add(new Parametros("@Fec_EmisionD", SqlDbType.DateTime, dto.FecEmisionDesde));
            listaParametros.Add(new Parametros("@Fec_EmisionH", SqlDbType.DateTime, dto.FecEmisionHasta));
            listaParametros.Add(new Parametros("@Fec_ValidacionD", SqlDbType.DateTime, dto.FecValidacionDesde));
            listaParametros.Add(new Parametros("@Fec_ValidacionH", SqlDbType.DateTime, dto.FecValidacionHasta));

            DS = Con.ObtenerRegistros(procedimiento, "inter_DTE", listaParametros);

            return DS;
        }
    }
}
