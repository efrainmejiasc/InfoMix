using System;

namespace Datos
{
    public class Dto_Consulta
    {
        public Dto_Consulta()
        { }

        private int _rut = 0;
        private string _usuario = string.Empty;
        private int? _folio = null;
        private int? _estado = null;
        private string _tipodoc = "0";
        private DateTime? _femisiond = null;
        private DateTime? _femisionh = null;
        private DateTime? _fvalidaciond = null;
        private DateTime? _fvalidacionh = null;

        public int Rut
        {
            get { return this._rut; }
            set { this._rut = value; }
        }

        public string Usuario
        {
            get { return this._usuario; }
            set { this._usuario = value.Trim(); }
        }

        public int? Folio
        {
            get { return this._folio; }
            set { this._folio = value; }
        }

        public int? Estado
        {
            get { return this._estado; }
            set { this._estado = value; }
        }

        public string TipoDoc
        {
            get { return this._tipodoc; }
            set { this._tipodoc = value.Trim(); }
        }

        public DateTime? FecEmisionDesde
        {
            get { return this._femisiond; }
            set { this._femisiond = value; }
        }

        public DateTime? FecEmisionHasta
        {
            get { return this._femisionh; }
            set { this._femisionh = value; }
        }

        public DateTime? FecValidacionDesde
        {
            get { return this._fvalidaciond; }
            set { this._fvalidaciond = value; }
        }

        public DateTime? FecValidacionHasta
        {
            get { return this._fvalidacionh; }
            set { this._fvalidacionh = value; }
        }
    }
}
