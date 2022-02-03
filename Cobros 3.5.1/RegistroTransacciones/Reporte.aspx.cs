using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;

namespace RegistroTransacciones
{
    public partial class Reporte : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Bsn_DetRecau bsn = new Bsn_DetRecau();
            DataSet ds = new DataSet();

            int caja = int.Parse(ddlCaja.SelectedValue);

            ds = bsn.BuscarReporteDetRecau(caja,ddlMes.SelectedValue,txtanio.Text);

            GVReportes.DataSource = ds;
            GVReportes.DataBind();
        }

        protected void BtnLimpiar_Click(object sender, EventArgs e)
        {

        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            if (GVReportes.Rows.Count > 0)
            {
                string nombre = String.Format("Caja_{0}_Mes_{1}_Año_{2}", ddlCaja.SelectedItem, ddlMes.SelectedItem, txtanio.Text);
                ExportaExcel(GVReportes, Response, nombre);
            }
        }

        public static void ExportaExcel(GridView GRV, System.Web.HttpResponse RS, string nombreArchivo)
        {
            // Damos la salida como attachment
            RS.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo + ".xls");
            // Especificamos el tipo de salida.
            RS.ContentType = "application/vnd.ms-excel";
            // Asociamos la salida con la codificación UTF8 (para poder visualizar los acentos correctamente)
            RS.ContentEncoding = Encoding.Default;
            RS.Charset = "UTF-8";
            StringWriter tw = new StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            GRV.RenderControl(hw); // GRV es el GRIDVIEW
            //Escribimos el HTML en el Explorador
            RS.Write(tw.ToString());
            // Terminamos el Response.
            RS.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
    }
}