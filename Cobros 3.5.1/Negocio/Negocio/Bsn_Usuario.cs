
using System;
using System.Collections.Generic;
using System.Linq;
using Datos;
using Informix.Domain;
using System.Data;

namespace Negocio
{
    public class Bsn_Usuario
    {   
        private ProcedureIfx DaoIFX = new ProcedureIfx();
        private DataSet DS = new DataSet();

        public DataSet BuscarUsuario(string clave)
        {
            return DS = DaoIFX.BuscarUsuario(clave);
        }

        //public DataSet BuscarUsuario2(string clave)
        //{
        //    return DS = DaoIFX.BuscarUsuario2(clave);
        //}

    }
}
