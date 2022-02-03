using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Datos;

namespace Dao
{
    public class Dao_Reserva
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

        public Dao_Reserva()
        {
            Con = new ConexionBaseDatos();
        }

        ~Dao_Reserva()
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
            const string procedimiento = "Reserva_SeleccionarTodo";
            List<Parametros> listaParametros = new List<Parametros>();
            listaParametros.Add(new Parametros("@Rut", SqlDbType.Int, dto.Rut));            

            DS = Con.ObtenerRegistros(procedimiento, "reserva", listaParametros);

            return DS;
        }

        public void LiberarFolios(Dto_Reserva dto)
        {
            const string procedimiento = "Reserva_LiberarFolios";
            List<Parametros> listaParametros = new List<Parametros>();
            listaParametros.Add(new Parametros("@Id_Reserva", SqlDbType.Int, dto.ID_Reserva));            

            Con.EjecutarSentencia(procedimiento, listaParametros);            
        }

        public DataSet CantidadDisponible()
        {
            const string procedimiento = "Reserva_CantidadDisponible";

            DS = Con.ObtenerRegistros(procedimiento, "reserva");

            return DS;
        }       



        public void ReservarFolios(Dto_Reserva dto)
        {
            const string procedimiento = "Reserva_ReservaFolios";
            List<Parametros> listaParametros = new List<Parametros>();
            listaParametros.Add(new Parametros("@Tipo_Doc", SqlDbType.VarChar, dto.TipoDoc));
            listaParametros.Add(new Parametros("@Fecha", SqlDbType.DateTime, dto.FecReserva));
            listaParametros.Add(new Parametros("@Usuario", SqlDbType.VarChar, dto.Usuario));
            listaParametros.Add(new Parametros("@Terminal", SqlDbType.Int, dto.Terminal));

            Con.EjecutarSentencia(procedimiento, listaParametros);
        }
        #endregion
    }
}
