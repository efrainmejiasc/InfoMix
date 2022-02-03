

namespace XML_DATOS
{
    public class DtoManRequerimiento
    {
        public DtoManRequerimiento()
        { 
        
        }

        private string _id = null;
        private string _depto = null;
        private string _seccion = null;
        private string _descripcion = null;
        private string _folio = null;
        private string _concepto = null;
        private string _grupo = null;
        private string _agrupacion = null;
        private string _año = null;


        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Departamento
        {
            get { return _depto; }
            set { _depto = value; }
        }

        public string Seccion
        {
            get { return _seccion; }
            set { _seccion = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value.ToUpper(); }
        }

        public string Folio
        {
            get { return _folio; }
            set { _folio = value; }
        }

        public string Concepto
        {
            get { return _concepto; }
            set { _concepto = value; }
        }

        public string Grupo
        {
            get { return _grupo; }
            set { _grupo = value; }
        }

        public string Agrupacion
        {
            get { return _agrupacion; }
            set { _agrupacion = value; }
        }

        public string Año
        {
            get { return _año; }
            set { _año = value; }
        }


    }
}
