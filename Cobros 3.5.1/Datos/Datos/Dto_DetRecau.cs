using System;

namespace Datos
{
    public class Dto_DetRecau
    {
        public Dto_DetRecau()
        {
        }

        private int _cod_oficina = -1;
        private int _cod_caja = -1;
        private string _fecha_regi = string.Empty;
        private int _nro_correl = -1;
        private string _indice = string.Empty;
        private string _tp_docpaga = string.Empty;
        private int _folio_docpaga = -1;
        private string _bco_docpaga = string.Empty;
        private string _fecha_docpaga = string.Empty;
        private decimal _valor_docpaga = -1;
        private string _tp_medpago = string.Empty;
        private string _bco_medpago = string.Empty;
        private DateTime? _fecha_medpago = null;
        private string _nro_medpago = string.Empty;
        private string _cod_conrecau = string.Empty;
        private decimal _valor_medpago = -1;
        private string _sw_estdocu = string.Empty;
        private string _sw_estmedi = string.Empty;
        private string _rut = string.Empty;
        private string _bco_depo = string.Empty;
        private string _cta_depo = string.Empty;
        private int? _nro_depo = null;
        private decimal? _cod_ajuste = null;
        private decimal? _valor_ajuste = null;
        private string _anc_tipodoc = string.Empty;
        private string _anc_foldoc = string.Empty;
        private int? _sucursal_doc = null;
        private int? _folio_cmrc = null;
        private int _folio_sigfe = -1;
        private int _id_agrupacion = -1;
        private long _nro_ticket = -1;
        private string _procesado = string.Empty;
        private DateTime _fechacumplimiento;
        private string _estado = "";
        private string _fechaProce = string.Empty;

        public string FechaProce
        {
            get { return this._fechaProce; }
            set { this._fechaProce = value; }
        }

        public int Cod_oficina
        {
            get { return this._cod_oficina; }
            set { this._cod_oficina = value; }
        }

        public int Cod_caja
        {
            get { return this._cod_caja; }
            set { this._cod_caja = value; }
        }

        public string Fecha_regi
        {
            get { return this._fecha_regi; }
            set { this._fecha_regi = value; }
        }

        public int Nro_correl
        {
            get { return this._nro_correl; }
            set { this._nro_correl = value; }
        }

        public string Indice
        {
            get { return this._indice; }
            set { this._indice = value; }
        }

        public string Tipo_Doc
        {
            get { return this._tp_docpaga; }
            set { this._tp_docpaga = value.Trim(); }
        }

        public int Folio
        {
            get { return this._folio_docpaga; }
            set { this._folio_docpaga = value; }
        }

        public string Bco_docpaga
        {
            get { return this._bco_docpaga; }
            set { this._bco_docpaga = value; }
        }

        public string Fecha_docpaga
        {
            get { return this._fecha_docpaga; }
            set { this._fecha_docpaga = value; }
        }

        public Decimal Valor_docpaga
        {
            get { return this._valor_docpaga; }
            set { this._valor_docpaga = value; }
        }

        public string Tp_medpago
        {
            get { return this._tp_medpago; }
            set { this._tp_medpago = value.Trim(); }
        }

        public string Bco_medpago
        {
            get { return this._bco_medpago; }
            set { this._bco_medpago = value; }
        }

        public DateTime? Fecha_medpago
        {
            get { return this._fecha_medpago; }
            set { this._fecha_medpago = value; }
        }

        public string Nro_Medpago
        {
            get { return this._nro_medpago; }
            set { this._nro_medpago = value; }
        }
        
        public string Cod_conrecau
        {
            get { return this._cod_conrecau; }
            set { this._cod_conrecau = value; }
        }

        public Decimal Valor_Medpago
        {
            get { return this._valor_medpago; }
            set { this._valor_medpago = value; }
        }

        public string Sw_Estdocu
        {
            get { return this._sw_estdocu; }
            set { this._sw_estdocu = value; }
        }

        public string Sw_Estmedi
        {
            get { return this._sw_estmedi; }
            set { this._sw_estmedi = value; }
        }

        public string Rut
        {
            get { return this._rut; }
            set { this._rut = value; }
        }

        public string Bco_Depo
        {
            get { return this._bco_depo; }
            set { this._bco_depo = value; }
        }

        public string Cta_Depo
        {
            get { return this._cta_depo; }
            set { this._cta_depo = value; }
        }

        public int? Nro_Depo
        {
            get { return this._nro_depo; }
            set { this._nro_depo = value; }
        }

        public decimal? Cod_Ajuste
        {
            get { return this._cod_ajuste; }
            set { this._cod_ajuste = value; }
        }

        public decimal? Valor_Ajuste
        {
            get { return this._valor_ajuste; }
            set { this._valor_ajuste = value; }
        }

        public string Anc_tipodoc
        {
            get { return this._anc_tipodoc; }
            set { this._anc_tipodoc = value; }
        }

        public string Anc_Foldoc
        {
            get { return this._anc_foldoc; }
            set { this._anc_foldoc = value; }
        }

        public int? Sucursal_Doc
        {
            get { return this._sucursal_doc; }
            set { this._sucursal_doc = value; }
        }

        public int? Folio_CMRC 
        {
            get { return this._folio_cmrc; }
            set { this._folio_cmrc = value; } 
        }

        public int Folio_Sigfe
        {
            get { return this._folio_sigfe; }
            set { this._folio_sigfe = value; }
        }

        public int Id_Agrupacion
        {
            get { return this._id_agrupacion; }
            set { this._id_agrupacion = value; }
        }

        public long Nro_Ticket
        {
            get { return this._nro_ticket; }
            set { this._nro_ticket = value; }
        }

        public string Procesado
        {
            get { return this._procesado; }
            set { this._procesado = value; }
        }

        public DateTime FechaCumplimiento
        {
            get { return this._fechacumplimiento; }
            set { this._fechacumplimiento = value; }
        }

        public string Estado
        {
            get { return this._estado; }
            set { this._estado = value; }
        }

        public bool Electronica { get; set; }

        public int Boleta { get; set; }
    }
}
