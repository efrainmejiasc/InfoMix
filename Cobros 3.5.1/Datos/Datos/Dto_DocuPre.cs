using System;

namespace Datos
{
    public class Dto_DocuPre
    {
       public Dto_DocuPre()
        {
        }

        private string ent_codente = string.Empty;
        private string cod_oficina = string.Empty;
        private string tipo_doc = string.Empty;
        private string folio_doc = string.Empty;
        private string emision = string.Empty;
        private string cuenta = string.Empty;
        private string ccosto = string.Empty;
        private string depto = string.Empty;
        private string seccion = string.Empty;
        private decimal monto = -1;
        private string dh = string.Empty;

        private string idReq = string.Empty;
        private string origen = string.Empty;
        private string num_serial = string.Empty;
        private string prsg_serial = string.Empty;

        private string folionivel2x = string.Empty;
        private string correlativoreq = string.Empty;

        private string _rutCliente = string.Empty;
        private string _rutfac = string.Empty;

        public string Rut_Fac
        {
            get { return this._rutfac; }
            set { this._rutfac = value.Trim(); }
        }

        public string Ent_Codente
        {
            get { return this.ent_codente; }
            set { this.ent_codente = value.Trim(); }
        }
        public string Cod_Oficina
        {
            get { return this.cod_oficina; }
            set { this.cod_oficina = value.Trim(); }
        }
        public string Tipo_Doc
        {
            get { return this.tipo_doc; }
            set { this.tipo_doc = value.Trim(); }
        }
        public string Folio_Doc
        {
            get { return this.folio_doc; }
            set { this.folio_doc = value.Trim(); }
        }
        public string Emision
        {
            get { return this.emision; }
            set { this.emision = value.Trim(); }
        }
        public string Cuenta
        {
            get { return this.cuenta; }
            set { this.cuenta = value.Trim(); }
        }
        public string Ccosto
        {
            get { return this.ccosto; }
            set { this.ccosto = value.Trim(); }
        }
        public string Depto
        {
            get { return this.depto; }
            set { this.depto = value.Trim(); }
        }
        public string Seccion
        {
            get { return this.seccion; }
            set { this.seccion = value.Trim(); }
        }
        public Decimal Monto
        {
            get { return this.monto; }
            set { this.monto = value; }
        }
        public string DH
        {
            get { return this.dh; }
            set { this.dh = value.Trim(); }
        }

        public string IdReq
        {
            get { return this.idReq; }
            set { this.idReq = value.Trim(); }
        }

        public string Origen
        {
            get { return this.origen; }
            set { this.origen = value.Trim(); }
        }

        public string Num_Serial
        {
            get { return this.num_serial; }
            set { this.num_serial = value.Trim(); }
        }
        public string Prsg_Serial
        {
            get { return this.prsg_serial; }
            set { this.prsg_serial = value.Trim(); }
        }

        public string FolioNivel2x
        {
            get { return this.folionivel2x; }
            set { this.folionivel2x = value.Trim(); }
        }

        public string CorrelativoReq
        {
            get { return this.correlativoreq; }
            set { this.correlativoreq = value.Trim(); }
        }

        public string Rut_Cliente
        {
            get { return this._rutCliente; }
            set { this._rutCliente = value.Trim(); }
        }
    }
}
