

namespace XML_DATOS
{
    public class DtoDocuPre
    {
       public DtoDocuPre()
        {
        }

        private string _entCodente = string.Empty;
        private string _codOficina = string.Empty;
        private string _tipoDoc = string.Empty;
        private string _folioDoc = string.Empty;
        private string _emision = string.Empty;
        private string _cuenta = string.Empty;
        private string _ccosto = string.Empty;
        private string _depto = string.Empty;
        private string _seccion = string.Empty;
        private string _monto = string.Empty;
        private string _dh = string.Empty;

        private string _idReq = string.Empty;
        private string _origen = string.Empty;
        private string _numSerial = string.Empty;
        private string _prsgSerial = string.Empty;

        private string _folionivel2X = string.Empty;
        private string _correlativoreq = string.Empty;

        public string EntCodente
        {
            get { return _entCodente; }
            set { _entCodente = value.Trim(); }
        }
        public string CodOficina
        {
            get { return _codOficina; }
            set { _codOficina = value.Trim(); }
        }
        public string TipoDoc
        {
            get { return _tipoDoc; }
            set { _tipoDoc = value.Trim(); }
        }
        public string FolioDoc
        {
            get { return _folioDoc; }
            set { _folioDoc = value.Trim(); }
        }
        public string Emision
        {
            get { return _emision; }
            set { _emision = value.Trim(); }
        }
        public string Cuenta
        {
            get { return _cuenta; }
            set { _cuenta = value.Trim(); }
        }
        public string Ccosto
        {
            get { return _ccosto; }
            set { _ccosto = value.Trim(); }
        }
        public string Depto
        {
            get { return _depto; }
            set { _depto = value.Trim(); }
        }
        public string Seccion
        {
            get { return _seccion; }
            set { _seccion = value.Trim(); }
        }
        public string Monto
        {
            get { return _monto; }
            set { _monto = value.Trim(); }
        }
        public string Dh
        {
            get { return _dh; }
            set { _dh = value.Trim(); }
        }

        public string IdReq
        {
            get { return _idReq; }
            set { _idReq = value.Trim(); }
        }

        public string Origen
        {
            get { return _origen; }
            set { _origen = value.Trim(); }
        }

        public string NumSerial
        {
            get { return _numSerial; }
            set { _numSerial = value.Trim(); }
        }
        public string PrsgSerial
        {
            get { return _prsgSerial; }
            set { _prsgSerial = value.Trim(); }
        }

        public string FolioNivel2X
        {
            get { return _folionivel2X; }
            set { _folionivel2X = value.Trim(); }
        }

        public string CorrelativoReq
        {
            get { return _correlativoreq; }
            set { _correlativoreq = value.Trim(); }
        }
    }
}
