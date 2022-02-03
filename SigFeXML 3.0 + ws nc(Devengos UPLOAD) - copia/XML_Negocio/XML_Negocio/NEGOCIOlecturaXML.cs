
using XML_DATOS;
using System.Data;
using Informix.Domain;

namespace XML_Negocio
{
    public class NegociOlecturaXml
    {

        private readonly ProcedureIfx _ifx = new ProcedureIfx();
      

        public DataSet BuscarFacturaFechaTipoDoc(string fecha, string tipoDoc) // busca factura por tipo de doc y fecha
        {
            return _ifx.BuscarFacmanFechaTipo(fecha, tipoDoc);
        }

        public DataSet BuscarDatosxFolio(string folio, string tipoDoc)
        {
            return _ifx.BuscarPorNumeroFolio(folio, tipoDoc);
        }

        public DataSet BuscarDocuPrexFolioTipo(string folio, string tipoDoc)
        {
            return _ifx.BuscarFacmanDocu_PrexFolioTipoDoc(folio, tipoDoc);
        }

        public DataSet BuscarDatosFactura2(string fecha, string tipoDoc) 
        {
            return _ifx.BuscarFacmanFechaTipoDoc(fecha, tipoDoc);
        }

        public DataSet BuscarIdRequerimiento(string ente, string depto, string seccion, string pfGroup, string anho) 
        {
            return _ifx.BuscarIdReq(ente, depto, seccion, pfGroup, anho);
        }

        public DataSet BuscarRutCliente(string rutFactura) 
        {
            return _ifx.BuscarRutCliente(rutFactura);
        }

        public DataSet BuscarBoletaFechaTipo(string fecha, string tipoDoc) 
        {
            return _ifx.BuscarBoletaFechaTipo(fecha,tipoDoc);
        }

        public DataSet BuscarBoletaxfolio(string folio, string tipoDoc) 
        {
            return _ifx.BuscarBoletaFolio(folio, tipoDoc);
        }

        public DataSet BuscarDocuPreBoletaxFolioTipo(string folio, string tipoDoc)
        {
            return _ifx.BuscarBoletaDocu_PrexFolioTipoDoc(folio, tipoDoc);
        }

        public DataSet BuscarCuentaContable(string cuenta)
        {
            return _ifx.BuscarCtaContable(cuenta);
        }

        /*****************Mantenedor************/
        public DataSet BuscarFiltros(DtoManRequerimiento dto)
        {
            return _ifx.BuscarFiltros(dto);
        }

        public void GuardarRequerimiento(DtoManRequerimiento dto)
        {
            _ifx.InsertarRequerimientos(dto);
        }

        public void EditarRequerimiento(DtoManRequerimiento dto)
        {
            _ifx.EditarRequerimientos(dto);
        }

        public DataSet BuscarRequerimiento(DtoManRequerimiento dto)
        {
            return _ifx.BuscarReq(dto);
        }

        public DataSet CargarNivel1()
        {
            return _ifx.CargarNivel1();
        }

        public DataSet CargarNivel2()
        {
            return _ifx.CargarNivel2();
        }

        public DataSet CargarNivel2Filtro(int id)
        {
            return _ifx.CargarNivel2Filtro(id);
        }

        public DataSet CargarNivel2FiltroGlosa(int id)
        {
            return _ifx.CargarNivel2FiltroGlosa(id);
        }

        public void EliminarReq(int id)
        {
            _ifx.EliminarReq(id);
        }

        public DataSet BuscarInterSigfe(string tipRelDoc, int folRelDoc)
        {
            return _ifx.BuscarInterSigfe(tipRelDoc,folRelDoc);
        }

        public DataSet BuscarNotaCreditoSigfe(string folio)
        {
            return _ifx.BuscarNotaCreditoSigfe(folio);
        }
    }
}
