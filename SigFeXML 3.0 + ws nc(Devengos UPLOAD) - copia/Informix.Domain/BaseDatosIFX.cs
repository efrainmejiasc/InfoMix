using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
namespace Informix.Domain
{
    /// <summary>
    /// Representa la base de datos en el sistema.
    /// Ofrece los métodos de acceso a misma.
    /// </summary>
    public class BaseDatosIFX : IDisposable
    {

        private DbConnection Conexion { get; set; }
        private DbCommand Comando { get; set; }
        //private DbTransaction Transaccion { get; set; }
        private DbProviderFactory Factory { get; set; }
        private string ConeccionString { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseDatosIFX()
        {
            try
            {
                this.ConeccionString = ConfigurationManager.AppSettings["ConneccionIfx"];
                Factory = DbProviderFactories.GetFactory(ConfigurationManager.AppSettings["Provider"]);
            }
            catch (ConfigurationException ex)
            {
                throw new BaseDatosException("Error al cargar la configuración del acceso a datos INFORMIX.", ex);
            }
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~BaseDatosIFX()
        {
            Dispose(true);
        }
        /// <summary>
        /// Permite desconectarse de la base de datos.
        /// </summary>
        public void Desconectar()
        {
            if (Conexion == null) return;
            if (this.Conexion.State.Equals(ConnectionState.Open))
            {
                this.Conexion.Close();
                Dispose();
            }
        }
        /// <summary>
        /// Cierra toda referencia del objeto creado
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Cierra referencia de toda conexion
        /// </summary>
        /// <param name="disposing">True o false para realizar la accion dispose</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Conexion != null)
                {
                    Conexion.Dispose();
                    Conexion = null;
                }
            }
        }
        /// <summary>
        /// retorna conección con la base de datos.
        /// </summary>
        /// <returns></returns>
        ///<exception cref="BaseDatosException">Si existe un error al conectarse.</exception>
        public BaseDatosIFX Connect()
        {
            try
            {
                if (this.Conexion != null && !this.Conexion.State.Equals(ConnectionState.Closed))
                {
                    return this;//retorna conección actual
                    //throw new BaseDatosException("La conexión ya se encuentra abierta.");
                }
                this.Conexion = Factory.CreateConnection();
                Conexion.ConnectionString = this.ConeccionString;

                this.Conexion.Open();
                return this;
            }
            catch (ConfigurationException ex)
            {
                throw new BaseDatosException("Error al cargar la configuración del acceso a datos INFORMIX.", ex);
            }
        }
        /// <summary>
        /// Crea un comando en base a una sentencia SQL.
        /// Ejemplo:
        /// <code>SELECT * FROM Tabla WHERE campo1=@campo1, campo2=@campo2</code>
        /// Guarda el comando para el seteo de parámetros y la posterior ejecución.
        /// </summary>
        /// <param name="sentenciaSql">La sentencia SQL con el formato: SENTENCIA [param = @param,]</param>
        public void CrearComando(string sentenciaSql)
        {
            this.Comando = Factory.CreateCommand();
            this.Comando.Connection = this.Conexion;
            this.Comando.CommandType = CommandType.Text;
            this.Comando.CommandText = sentenciaSql;
            //if (this.Transaccion != null)
            //{
            //    this.Comando.Transaction = this.Transaccion;
            //}

        }
        /// <summary>
        /// Setea un parámetro como nulo del comando creado.
        /// </summary>
        /// <param name="nombre">El nombre del parámetro cuyo valor será nulo.</param>
        public void AsignarParametroNulo(string nombre)
        {
            AsignarParametro(nombre, string.Empty, "NULL");            
        }

       
        /// <summary>
        /// Asigna un parámetro de tipo cadena al comando creado.
        /// </summary>
        /// <param name="nombre">El nombre del parámetro.</param>
        /// <param name="valor">El valor del parámetro.</param>
        public void AsignarParametroCadena(string nombre, string valor)
        {
            AsignarParametro(nombre, "'", valor != null ? valor.Replace("'", "''") : null);
        }
        /// <summary>
        /// Asigna un parámetro de tipo entero al comando creado.
        /// </summary>
        /// <param name="nombre">El nombre del parámetro.</param>
        /// <param name="valor">El valor del parámetro.</param>
        public void AsignarParametroEntero(string nombre, int valor)
        {
            AsignarParametro(nombre, string.Empty, valor.ToString());
        }
        /// <summary>
        /// Asigna un parámetro al comando creado.
        /// </summary>
        /// <param name="nombre">El nombre del parámetro.</param>
        /// <param name="separador">El separador que será agregado al valor del parámetro.</param>
        /// <param name="valor">El valor del parámetro.</param>
        private void AsignarParametro(string nombre, string separador, string valor)
        {
            int indice = this.Comando.CommandText.IndexOf(nombre);
            string prefijo = this.Comando.CommandText.Substring(0, indice);
            string sufijo = this.Comando.CommandText.Substring(indice + nombre.Length);
            this.Comando.CommandText = prefijo + separador + valor + separador + sufijo;
        }
        /// <summary>
        /// Ejecuta el comando creado y retorna el resultado de la consulta.
        /// </summary>
        /// <returns>El resultado de la consulta.</returns>
        /// <exception cref="BaseDatosException">Si ocurre un error al ejecutar el comando.</exception>
        public DbDataReader EjecutarConsulta()
        {
            Comando.CommandTimeout = 240;//4 minutos para ejecutar consultas mas pesadas
            return this.Comando.ExecuteReader();
        }
        //ERROR [HYC00] [Informix][Informix ODBC Driver]Driver not capable
        ///// <summary>
        ///// Comienza una transacción en base a la conexion abierta.
        ///// Todo lo que se ejecute luego de esta ionvocación estará 
        ///// dentro de una tranasacción.
        ///// </summary>
        //public void ComenzarTransaccion()
        //{
        //    if (this._transaccion == null)
        //    {
        //        this._transaccion = this._conexion.BeginTransaction();
        //    }
        //}

        ///// <summary>
        ///// Cancela la ejecución de una transacción.
        ///// Todo lo ejecutado entre ésta invocación y su 
        ///// correspondiente <c>ComenzarTransaccion</c> será perdido.
        ///// </summary>
        //public void CancelarTransaccion()
        //{
        //    if (this._transaccion != null)
        //    {
        //        this._transaccion.Rollback();
        //    }
        //}

        ///// <summary>
        ///// Confirma todo los comandos ejecutados entre el <c>ComanzarTransaccion</c>
        ///// y ésta invocación.
        ///// </summary>
        //public void ConfirmarTransaccion()
        //{
        //    if (this._transaccion != null)
        //    {
        //        this._transaccion.Commit();
        //    }
        //}

    }
}
