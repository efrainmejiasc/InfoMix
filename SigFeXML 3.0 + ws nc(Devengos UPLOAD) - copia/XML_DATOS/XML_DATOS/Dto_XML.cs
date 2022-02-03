
namespace XML_DATOS
{
    public class DtoXml
    {
        public DtoXml()
        { }

        private string _codFactura = string.Empty;
        private string _rutFac = string.Empty;
        private string _dvFac = string.Empty;
        private string _tipoDoc = string.Empty;
        private string _folio = string.Empty;
        private string _folioOriginal = string.Empty;
        private string _fecemi = string.Empty;
        private string _fecven = string.Empty;
        private string _feccumplimiento = string.Empty;
        private string _folrelDoc = string.Empty;
        private string _valorNeto = string.Empty;
        private string _valorExento = string.Empty;
        private string _valorFactura = string.Empty;
        private string _valorNetof = string.Empty;
        private string _tipRelDoc = string.Empty;

        private string _fechaEmision = string.Empty;
        private string _costo = string.Empty;
        private string _depto = string.Empty;
        private string _seccion = string.Empty;
        private string _valorMonto = string.Empty;
        private string _dh = string.Empty;
        private string _cuenta = string.Empty;

        private string _pfGrupo = string.Empty;

        private string _impuesto = string.Empty;
        private string _cabecera = string.Empty;

        //----------------------------
        private string _iva = string.Empty;
        private string _descuento = string.Empty;
        private string _despacho = string.Empty;
        private string _tipoTransaccion = string.Empty;

        private string _reqOriginal = string.Empty;
        private string _correlativoDocumento = string.Empty;
        private string _folioSigfe = string.Empty;
        private string _codDocumento = string.Empty;
        private string _tipoAjuste = string.Empty;

        public string CodigoFactura
       {
           get { return _codFactura; }
           set { _codFactura = value.Trim(); }
       }

        public string RutFactura
        {
            get { return _rutFac; }
            set { _rutFac = value.Trim(); }
        }

        public string DvFactura
        {
            get { return _dvFac; }
            set { _dvFac = value.Trim(); }
        }

        public string TipoDoc
        {
            get { return _tipoDoc; }
            set { _tipoDoc = value.Trim(); }
        }

        public string Folio
        {
            get { return _folio; }
            set { _folio = value.Trim(); }
        }

        public string FolioOriginal
        {
            get { return _folioOriginal; }
            set { _folioOriginal = value.Trim(); }
        }

        public string FechaEmision
        {
            get { return _fecemi; }
            set { _fecemi = value.Trim(); }
        }

        public string FechaVencimiento
        {
            get { return _fecven; }
            set { _fecven = value.Trim(); }
        }

        public string FechaCumplimiento
        {
            get { return _feccumplimiento; }
            set { _feccumplimiento = value.Trim(); }
        }

         public string FolRelDoc
        {
            get { return _folrelDoc; }
            set { _folrelDoc = value.Trim(); }
        }
        
        public string ValorNeto
        {
            get { return _valorNeto; }
            set { _valorNeto = value.Trim(); }
        }
        public string ValorExento
        {
            get { return _valorExento; }
            set { _valorExento = value.Trim(); }
        }
        public string ValorFactura
        {
            get { return _valorFactura; }
            set { _valorFactura = value.Trim(); }
        }
        public string ValorNetoF
        {
            get { return _valorNetof; }
            set { _valorNetof = value.Trim(); }
        }
        public string TipRelDoc
        {
            get { return _tipRelDoc; }
            set { _tipRelDoc = value.Trim(); }
        }

        public string EmisionFacturaDp
        {
            get { return _fechaEmision; }
            set { _fechaEmision = value.Trim(); }
        }

        public string Costo
        {
            get { return _costo; }
            set { _costo = value.Trim(); }
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

        public string ValorMonto
        {
            get { return _valorMonto; }
            set { _valorMonto = value.Trim(); }
        }
        public string Dh
        {
            get { return _dh; }
            set { _dh = value.Trim(); }
        }
        public string Cuenta
        {
            get { return _cuenta; }
            set { _cuenta = value.Trim(); }
        }

        public string PfGrupo
        {
            get { return _pfGrupo; }
            set { _pfGrupo = value.Trim(); }
        }

        public string Impuesto
        {
            get { return _impuesto; }
            set { _impuesto = value.Trim(); }
        }

        public string Cabecera
        {
            get { return _cabecera; }
            set { _cabecera = value.Trim(); }
        }

        public string Iva
        {
            get { return _iva; }
            set { _iva = value.Trim(); }
        }

        public string Descuento
        {
            get { return _descuento; }
            set { _descuento = value.Trim(); }
        }

        public string Despacho
        {
            get { return _despacho; }
            set { _despacho = value.Trim(); }
        }

        public string TipoTansaccion
        {
            get { return _tipoTransaccion; }
            set { _tipoTransaccion = value.Trim(); }
        }

        public string RequerimientoOriginal
        {
            get { return _reqOriginal; }
            set { _reqOriginal = value.Trim(); }
        }

        public string CorrelativoDocumento
        {
            get { return _correlativoDocumento; }
            set { _correlativoDocumento = value.Trim(); }
        }

        public string FolioSigfe
        {
            get { return _folioSigfe; }
            set { _folioSigfe = value.Trim(); }
        }

        public string CodDocumento
        {
            get { return _codDocumento; }
            set { _codDocumento = value.Trim(); }
        }

        public string TipoAjuste
        {
            get { return _tipoAjuste; }
            set { _tipoAjuste = value.Trim(); }
        }
    }
}
