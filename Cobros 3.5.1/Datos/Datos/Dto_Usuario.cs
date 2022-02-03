using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos
{
    public class Dto_Usuario
    {
        public Dto_Usuario()
        { }

        private string _usuer = string.Empty;
        private string _pass = string.Empty;
        private int _rut = 0;
        private string _dv = string.Empty;
        private string _apellido = string.Empty;
        private string _ini_user = string.Empty;
        private string _cargo = string.Empty;
        private int? _estado = null;
        private int? _terminal = null;

        public int Rut
        {
            get { return this._rut; }
            set { this._rut = value; }
        }

        public string Dv
        {
            get { return this._dv; }
            set { this._dv = value.Trim(); }
        }

        public string Apellido
        {
            get { return this._apellido; }
            set { this._apellido = value.Trim(); }
        }

        public string Iniciales
        {
            get { return this._ini_user; }
            set { this._ini_user = value.Trim(); }
        }

        public string Cargo
        {
            get { return this._cargo; }
            set { this._cargo = value.Trim(); }
        }

        public string User
        {
            get { return this._usuer; }
            set { this._usuer = value.Trim(); }
        }

        public string Pass
        {
            get { return this._pass; }
            set { this._pass = value.Trim(); }
        }

        public int? Estado
        {
            get { return this._estado; }
            set { this._estado = value; }
        }

        public int? Terminal
        {
            get { return this._terminal; }
            set { this._terminal = value; }
        }
    }
}
